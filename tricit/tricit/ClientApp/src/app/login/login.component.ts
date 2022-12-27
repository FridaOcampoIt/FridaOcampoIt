import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar, MatDialog } from '@angular/material';
import * as CryptoJS from 'crypto-js';

import { DataServices } from '../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';

import { DialogoContrasenaComponent } from "./dialogo-contrasena/dialogo-contrasena.component";

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
	@Output() Login = new EventEmitter<any>();

	//Ocultar contraseña
	fieldTextType: boolean;
	constructor(
		private snack: MatSnackBar,
		private _router: Router,
		private dataService: DataServices,
		private _dialog: MatDialog) { }

	email: string = "";
	password: string = "";
	loginView: boolean = true;
	recoverPwd1: boolean = false;
	recoverPwd2: boolean = false;
	traceitInfo: boolean = false;
	emailRecovery: string = "";
	recoveryCode: string = "";
	newPassword: string = "";
	rePassword: string = "";


	onSubmit(event) {
		if (this.email.trim() == "" || this.password.trim() == "") {
			event.stopPropagation(); event.preventDefault();
			this.openSnack("Captura el usuario y contraseña", "Aceptar");
			return false;
		}
		// debugger;
		var request = new LoginUserRequest();
		request.password = CryptoJS.SHA256(this.password).toString();
		request.user = this.email;
		request.id = "0";
		this.dataService.postData<any>("User/loginUser", "", request).subscribe(
			data => {
				//debugger;
				
				if (data.messageEsp != "") {
					event.stopPropagation(); event.preventDefault();
					this._router.navigate(['Login']);
					this.openSnack(data.messageEsp, "Aceptar");
					return false;
				}

				if (data.userData.userData.origen == 1) {
					this.openSnack("El usuario no tiene acceso al sistema. Usuario de Stock.", "Aceptar");
					event.stopPropagation(); event.preventDefault();
					return false;
				}

				data.userData.userPermissions.forEach((it, id) => {
					sessionStorage.setItem(it.namePermission, it.permissionId.toString());
				});
				if(!!data.userData.userData.acopiosIds){
					//Si nuestro usuario trae acopios, los guardamos de en nuestro local storage					
                    let arraytemporal =  data.userData.userData.acopiosIds.split(',');
                    //Convertimos el string a enteros (porque yolo, no funciona con string)
                    let arrayNumeros = arraytemporal.map(str => {
                    	return Number(str);
                    });
					//Se convierte en arreglo, pero al parecer no funciona y se tendra que convertir cada vez, que se quiera hacer uso del recurso. 
					sessionStorage.setItem("acopiosIds", arrayNumeros);
				}
				sessionStorage.setItem("token", data.token);
				sessionStorage.setItem("name", data.userData.userData.name);
				sessionStorage.setItem("idUser", data.userData.userData.idUser.toString());
				sessionStorage.setItem("company", data.userData.userData.company.toString());
				sessionStorage.setItem("isType", data.userData.userData.isType.toString());
				sessionStorage.setItem("email", this.email);
				sessionStorage.setItem("password", CryptoJS.SHA256(this.password).toString());

				document.body.style.overflow = "hidden";
				this.Login.emit(true);
				this._router.navigate(["Home"]);
			},
			error => {
				event.stopPropagation(); event.preventDefault();
				console.log(error);
				this._router.navigate(['Login']);
				this.openSnack("Error al mandar la solicitud", "Aceptar");
				return false;
			}
		)
	}

	validateEmail(email) {
		var re = /\S+@\S+\.\S+/;
		return re.test(email);
	}

	recoverPassword() {

		this.loginView = false;
		this.recoverPwd1 = true;
		this.recoverPwd2 = false;
	}

	requestRecover() {
		if (this.emailRecovery !== "" && this.validateEmail(this.emailRecovery)) {
			this.dataService.postData<LoginUserResponse>("User/recoveryPassword", "", { 'email': this.emailRecovery }).subscribe(
				data => {
					if (data.messageEsp != "") {
						event.stopPropagation(); event.preventDefault();
						this._router.navigate(['Login']);
						this.openSnack(data.messageEsp, "Aceptar");
						return false;
					} else {
						this.loginView = false;
						this.recoverPwd1 = false;
						this.recoverPwd2 = true;
						this.openSnack("Código de recuperación enviado", "Aceptar");
					}
				},
				error => {
					event.stopPropagation(); event.preventDefault();
					console.log(error);
					this._router.navigate(['Login']);
					this.openSnack("Error al mandar la solicitud", "Aceptar");
					return false;
				}
			);
		} else {
			this.openSnack("Dirección inválida", "Aceptar");
		}
	}
	resetPassword() {
		if (this.recoveryCode !== "" && this.newPassword !== "" && this.rePassword !== "") {
			if ((this.newPassword === this.rePassword)) {

				var request = { 'email': this.emailRecovery, 'recoveryCode': this.recoveryCode, 'password': CryptoJS.SHA256(this.newPassword).toString() };
				this.recoveryCode = "";
				this.emailRecovery = "";
				this.newPassword = "";
				this.rePassword = "";
				this.dataService.postData<LoginUserResponse>("User/restorePassword", "", request).subscribe(
					data => {
						if (data.messageEsp != "") {
							event.stopPropagation(); event.preventDefault();
							this._router.navigate(['Login']);
							this.openSnack(data.messageEsp, "Aceptar");
							return false;
						} else {
							this.loginView = true;
							this.recoverPwd1 = false;
							this.recoverPwd2 = false;
							this.openSnack("Contraseña reestablecida exitosamente", "Aceptar");
						}
					},
					error => {
						event.stopPropagation(); event.preventDefault();
						console.log(error);
						this._router.navigate(['Login']);
						this.openSnack("Error al mandar la solicitud", "Aceptar");
						return false;
					}
				);
			} else {
				this.openSnack("Confirmación de contraseña no coincide", "Aceptar");
			}
		} else {
			this.openSnack("Todos los campos son obligatorios", "Aceptar");
		}
	}

	infoContact = () => {
		this.loginView = false;
		this.traceitInfo = true;
	}

	backLogin = () => {
		this.loginView = true;
		this.traceitInfo = false;
	}

	ngOnInit() {
		localStorage.clear();
		document.body.style.overflow = "auto";
	}

	ngOnDestroy(): void {
		//Called once, before the instance is destroyed.
		//Add 'implements OnDestroy' to the class.
		document.body.style.overflow = "hidden";
	}

	//Funcion para abrir el modal del mensaje
	openSnack = (message: string, action: string) => {
		this.snack.open(message, action, {
			duration: 5000
		})
	}

	resetPass = () => {
		const dialogRef = this._dialog.open(DialogoContrasenaComponent, {
			panelClass: 'dialog-visor-edit',
			disableClose: true,
			data: {}
		});

		dialogRef.afterClosed().subscribe(result => {

		});
	}

	scrollBot = () => {
		window.scrollTo(0, document.body.scrollHeight);
	}

	downloadPDF = () => {
		let linko = document.createElement("a");
		linko.download = "(Traceit) Guia de operacion y funcionamiento";
		linko.href = "../../assets/doc/(Traceit)Guiadeoperacionyfuncionamiento.pdf";
		linko.click();

	}

	// Mostrar/Ocultar contrasena
	toggleFieldTextType() {
		this.fieldTextType = !this.fieldTextType;
	  }
}
