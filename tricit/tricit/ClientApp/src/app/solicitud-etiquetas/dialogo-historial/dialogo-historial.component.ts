import { Component, OnInit, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { DataServices } from '../../Interfaces/Services/general.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import {
	HistoryBitacoraResponse, Bitacora, LabelListResponse
} from '../../Interfaces/Models/LabelModels';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';

@Component({
	selector: 'app-dialogo-historial',
	templateUrl: './dialogo-historial.component.html',
	styleUrls: ['./dialogo-historial.component.less']
})
export class DialogoHistorialComponent implements OnInit {


	constructor(
		@Inject(MAT_DIALOG_DATA) private _data: any,
		private dataService: DataServices,
		private snack: MatSnackBar,
		private _dialogRef: MatDialogRef<DialogoHistorialComponent>,
		private _router: Router, private _overlay: OverlayService) { }

	date = new FormControl(new Date());
	serializedDate = new FormControl((new Date()).toISOString());
	response = new HistoryBitacoraResponse();
	emptyHistory = false;
	overlayRef: OverlayRef;

	cargarDatos() {
		if (this._data.id > 0) {
			var request = { solicitudId: this._data.id };

			setTimeout(() => {
				this.overlayRef = this._overlay.open();
			}, 1);
			this.dataService.postData<HistoryBitacoraResponse>("Labels/historialBitacora", sessionStorage.getItem("token"), request).subscribe(
				data => {
					this.response = data;
					if (this.response.listaBitacoras.length === 0)
						this.emptyHistory = true;
					//console.log(this.response);
					/*for (var i in this.response.listaBitacoras) {
						
						//console.log(this.response.listaBitacoras[i]);
					}*/
					setTimeout(() => {
						this._overlay.close(this.overlayRef);
					}, 1);
				},
				error => {
					if (error.error.hasOwnProperty("messageEsp")) {
						this.relogin("cargarDatos");
					} else {
						console.log(error);
					}
					setTimeout(() => {
						this._overlay.close(this.overlayRef);
					}, 1);
				}
			)
		}

	}

	//Funcion para abrir el modal del mensaje
	openSnack = (message: string, action: string) => {
		this.snack.open(message, action, {
			duration: 5000
		})
	}

	//Funcion para realizar el proceso del relogin
	relogin(peticion) {
		var requestLogin = new LoginUserRequest();
		requestLogin.user = sessionStorage.getItem("email");
		requestLogin.password = sessionStorage.getItem("password");

		this.dataService.postData<LoginUserResponse>("User/loginUser", "", requestLogin).subscribe(
			data => {
				if (data.messageEsp != "") {
					sessionStorage.clear();
					this._router.navigate(['Login']);
					this.openSnack(data.messageEsp, "Aceptar");
					return;
				}

				sessionStorage.clear();

				data.userData.userPermissions.forEach((it, id) => {
					sessionStorage.setItem(it.namePermission, it.permissionId.toString());
				});

				sessionStorage.setItem("token", data.token);
				sessionStorage.setItem("name", data.userData.userData.name);
				sessionStorage.setItem("idUser", data.userData.userData.idUser.toString());
				sessionStorage.setItem("email", requestLogin.user);
				sessionStorage.setItem("password", requestLogin.password);
				sessionStorage.setItem("company", data.userData.userData.company.toString());
				sessionStorage.setItem("isType", data.userData.userData.isType.toString());

				switch (peticion) {
					case "cargarDatos":
						this.cargarDatos();
						break;
					/*case "cambiarStatus":
						this.cambiarStatus();
						break;
					case "guardarBitacora":
						this.guardarBitacora();
						break;*/
					default:
						break;
				}
			},
			error => {
				sessionStorage.clear();
				this._router.navigate(['Login']);
				this.openSnack("Error al mandar la solicitud", "Aceptar");
				return;
			}
		)
	}
	ngOnInit() {
		this.cargarDatos();
	}

}
