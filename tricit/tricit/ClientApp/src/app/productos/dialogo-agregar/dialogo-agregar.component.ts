import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { DataServices } from '../../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';
import {
	SearchProductDropDownRequest,
	SearchProductDropDownResponse,
	ProductDataRequest,
	SearchProductDataResponse,
	ProductDataSave,
	ProcessResponse,
	SearchOriginDropDownRequest,
    ProductDataEdition,
	familyDropDown
}from '../../Interfaces/Models/Product';
//import { request } from 'https';
//import { privateDecrypt } from 'crypto';
import { stringify } from '@angular/core/src/util';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { element } from '@angular/core/src/render3';

@Component({
    selector: 'app-dialogo-agregar',
    templateUrl: './dialogo-agregar.component.html',
    styleUrls: ['./dialogo-agregar.component.less']
})
export class DialogoAgregarComponent implements OnInit {
	combo: number = 1;
    constructor(
        @Inject(MAT_DIALOG_DATA) private _data: any,
        private _dialogRef: MatDialogRef<DialogoAgregarComponent>,
        private dataService: DataServices,
		private snack: MatSnackBar,
		private _router: Router, private _overlay: OverlayService) { }

    combosResponse = new SearchProductDropDownResponse();
    family: number = 0;
    origin: number = 0;
    titulo: string;
	response = new SearchProductDataResponse();
	subiendo: boolean = false;
	selectedFamily = new familyDropDown();
	familyExpiration: boolean = false;
	overlayRef: OverlayRef;
	dataEmbalajes: any = [];
	packagingId: number = 0;


	BusquedaCombos() {
		var request = new SearchProductDropDownRequest();
		request.company = parseInt(localStorage.getItem("company"));

		this.dataService.postData<SearchProductDropDownResponse>("Product/searchProductDropDown", sessionStorage.getItem("token"), request).subscribe(
			data => {
				this.combosResponse = data;
			},
			error => {

			}
		)
	}

	BusquedaComboOrigen(id, extra) {
		//console.log(event);
		//var selectFamily = event.target;
		//console.log(selectFamily.options[selectFamily.selectedIndex].getAttribute('caducidad'));
		if(extra == 1){
			this.familyExpiration = true;
		}else{
			this.familyExpiration = false;
		}
		this.response.productDetails.familyId = id;
		var request = new SearchOriginDropDownRequest();
		request.filter = this.response.productDetails.familyId;

		this.dataService.postData<SearchProductDropDownResponse>("Product/searchProductDropDownOrigen", sessionStorage.getItem("token"), request).subscribe(
			data => {
				this.combosResponse.originDropDown = data.originDropDown;
			},
			error => {

			}
		)
	}

	
	CambiarStatus() {

	}

	//Función para cargar los datos del producto que se seleccionó
	cargarDatos(){
		console.log("Loading data for " + this._data.id);
		this.response = new SearchProductDataResponse();
		var request = new ProductDataRequest();
		if(this._data.id > 0){
			request.idProduct = this._data.id;

			this.dataService.postData<SearchProductDataResponse>("Product/searchProductEdit", sessionStorage.getItem("token"), request).subscribe(
				data => {
					
					this.response = data;
					this.selectedFamily.id = data.productDetails.familyId;
					
					this.BusquedaComboOrigen(data.productDetails.familyId, data.productDetails.f_extra);
					this.response.productDetails.directionId = data.productDetails.directionId;
					//console.log('response status:---- ' + this.response.productDetails.status);
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

	guardarProducto() {
		var requestSave;
		var processUrl = "";
		console.log(this.response);
		if (this.response.productDetails.familyId == 0 || (this.response.productDetails.expirationDate == "" && this.familyExpiration) || this.response.productDetails.directionId == 0 || this.packagingId == 0){
			this.openSnack("Faltan datos obligatorios", "Aceptar");
			return;
		}

		if(this.response.productDetails.idProduct == 0){
			requestSave = new ProductDataSave();
			requestSave.directionId = this.response.productDetails.directionId;
			requestSave.expirationDate = this.response.productDetails.expirationDate;
			requestSave.familyProductId = this.response.productDetails.familyId;
			requestSave.udid = this.response.productDetails.udid;
			requestSave.packagingId = this.packagingId;

			processUrl = "Product/saveProduct";
		}else{
			requestSave = new ProductDataEdition();
			requestSave.directionId = this.response.productDetails.directionId;
			requestSave.expirationDate = this.response.productDetails.expirationDate;
			requestSave.familyProductId = this.response.productDetails.familyId;
			requestSave.udid = this.response.productDetails.udid;
			requestSave.idProduct = this.response.productDetails.idProduct;
			requestSave.status = this.response.productDetails.status;

			processUrl = "Product/editProduct";
		}
		
		this.subiendo = true;
		//this.overlayRef = this._overlay.open();
		this.dataService.postData<ProcessResponse>(processUrl, sessionStorage.getItem("token"), requestSave).subscribe(
			data => {
				if (data.messageEsp != "") {
					this.openSnack(data.messageEsp, "Aceptar");
				} else {
					//this._overlay.close(this.overlayRef);
					this.openSnack("Producto almacenado éxito", "Aceptar");
					this._dialogRef.close(true);
				}
				this.subiendo = false;
				//this._overlay.close(this.overlayRef);
			},
			error => {
				if (error.error.hasOwnProperty("messageEsp")) {
					this.relogin("saveProduct");
				} else {
					//this._overlay.close(this.overlayRef);
					this.openSnack("Error al mandar la solicitud", "Aceptar");
					this.subiendo = false;
				}
				//this._overlay.close(this.overlayRef);
			}
		);
		this._dialogRef.close(true);
		
	}

	obtenerEmbalajes() {

		this.packagingId = 0;

		let data: any = {
			id: this.selectedFamily.id
		  };
	  
		  setTimeout(() => {
			this.overlayRef = this._overlay.open();
		  }, 1);
	  
		  this.dataService.postData<any>("PackedLabeled/SearchPackagingCombo", sessionStorage.getItem("token"), data).subscribe(
			data => {
			  setTimeout(() => {
				this._overlay.close(this.overlayRef);
			  }, 1);
			  console.log("success", data);
	  
			  if (data["messageEng"].length) {
				this.openSnack("No hay emabalajes registrados o hubo error en la consulta", "Aceptar");
				return;
			  } else {

				  this.dataEmbalajes = data["packagingcombo"];
			  }
			},
			error => {
			  console.log("error", error);
			  setTimeout(() => {
				this._overlay.close(this.overlayRef);
			  }, 1);
			  this.openSnack("Error en la solicitud de embalajes", "Aceptar");
			}
		  )
	}

	//Funcion para realizar el proceso del relogin
	relogin(peticion) {
		console.log('relogin....');
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
					case "saveProduct":
						this.guardarProducto();
						break;
					case "cargarDatos":
						this.cargarDatos();
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
	//Funcion para abrir el modal del mensaje
	openSnack = (message: string, action: string) => {
		this.snack.open(message, action, {
			duration: 5000
		})
	}

	ngOnInit() {
		this.titulo = this._data.title;
		this.BusquedaCombos();
		this.cargarDatos();
	}

	closeD = () => {
		this.guardarProducto();
		//this._dialogRef.close(true);
	}
}
