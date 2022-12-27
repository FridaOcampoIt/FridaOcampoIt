import { Component, OnInit, Inject } from '@angular/core';
import { DataServices } from '../../Interfaces/Services/general.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import {
	LabelData, LabelConstants, LabelListResponse, AddBitacoraRequest
} from '../../Interfaces/Models/LabelModels';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
@Component({
	selector: 'app-dialogo-seguimiento',
	templateUrl: './dialogo-seguimiento.component.html',
	styleUrls: ['./dialogo-seguimiento.component.less']
})
export class DialogoSeguimientoComponent implements OnInit {
	readonly CONSTANTS = new LabelConstants;
	constructor(@Inject(MAT_DIALOG_DATA) private _data: any, private dataService: DataServices, private snack: MatSnackBar, private _dialogRef: MatDialogRef<DialogoSeguimientoComponent>,private _router: Router) { }

	response = new LabelData();
	nextStatus: string = "";
	nextStatusId : number = 0;
	bitacoraText: string = "";

	cambiarStatus(){
		var request = {"solicitudId": this._data.id,"estatusSolicitudId": this.nextStatusId};
		this.dataService.postData<LabelListResponse>("Labels/actualizarEstatusSolicitud", sessionStorage.getItem("token"), request).subscribe(
			data => {
				console.log(data);
				if (data.messageEsp != "") {
					this.openSnack(data.messageEsp, "Aceptar");
				}else{
					this.response.status = this.nextStatus;
					this.response.estatusSolicitudId = this.nextStatusId;
					switch(this.nextStatusId){
						case this.CONSTANTS.STATUS_SOLICITADO:
							this.nextStatus = "APROBADO";
							break;
						case this.CONSTANTS.STATUS_APROBADO:
							this.nextStatus = "REALIZADO";
							break;
						default:
							this.nextStatus = "";
							//this.nextStatusId = 0;
							break;
					}
				}
			},
			error => {
				if (error.error.hasOwnProperty("messageEsp")) {
					this.relogin("cambiarStatus");
				} else {
					console.log(error);
				}
			}
		)
	}

	guardarBitacora(){
		
		var request = new AddBitacoraRequest();
		request.descripcion = this.bitacoraText;
		request.estatusSolicitudId = this.response.estatusSolicitudId;
		request.solicitudId = this._data.id;
		request.usuarioId = parseInt(sessionStorage.getItem("idUser"));
		this.dataService.postData<LabelListResponse>("Labels/guardarBitacoraSolicitud", sessionStorage.getItem("token"), request).subscribe(
			data => {
				if (data.messageEsp == "") {
					console.log(data);
					this.openSnack("Bitácora registrada exitosamente", "Aceptar");
					this._dialogRef.close(true);
				}else{
					this.openSnack(data.messageEsp, "Aceptar");
				}
			},
			error => {
				if (error.error.hasOwnProperty("messageEsp")) {
					this.relogin("cambiarStatus");
				} else {
					console.log(error);
				}
			}
		)
	}

	cargarDatos(){
		console.log("Loading data for " + this._data.id);
		this.response = new LabelData();
		if(this._data.id > 0){
			var request = {solicitudId:this._data.id};

			this.dataService.postData<LabelListResponse>("Labels/detalleSolicitud", sessionStorage.getItem("token"), request).subscribe(
				data => {
					this.response = data.listaSolicitudEtiquetas[0];
					console.log(this.response.status);
					switch(this.response.estatusSolicitudId){
						case this.CONSTANTS.STATUS_SOLICITADO:
							this.nextStatus = "APROBADO";
							this.nextStatusId = this.CONSTANTS.STATUS_APROBADO;
							break;
						case this.CONSTANTS.STATUS_APROBADO:
							this.nextStatusId = this.CONSTANTS.STATUS_REALIZADO;
							this.nextStatus = "REALIZADO";
							break;
						default:
							this.nextStatus = "";
							break;
					}
				},
				error => {
					if (error.error.hasOwnProperty("messageEsp")) {
						this.relogin("cargarDatos");
					} else {
						console.log(error);
					}
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
				sessionStorage.setItem("company",data.userData.userData.company.toString());
				sessionStorage.setItem("isType", data.userData.userData.isType.toString());

				switch (peticion) {
					case "cargarDatos":
						this.cargarDatos();
						break;
					case "cambiarStatus":
						this.cambiarStatus();
						break;
					case "guardarBitacora":
						this.guardarBitacora();
						break;
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

	closeD = () => {
		this.guardarBitacora();
		//this._dialogRef.close(true);
	}
}
