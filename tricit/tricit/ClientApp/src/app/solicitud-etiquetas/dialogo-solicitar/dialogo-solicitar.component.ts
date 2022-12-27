import { Component, OnInit } from '@angular/core';
import { DataServices } from '../../Interfaces/Services/general.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import {
	SearchProductDropDownRequest,
	SearchProductDropDownResponse,
	SearchOriginDropDownRequest,
	familyDropDown
}from '../../Interfaces/Models/Product';

import {
	LabelData, LabelConstants
} from '../../Interfaces/Models/LabelModels';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';

@Component({
  selector: 'app-dialogo-solicitar',
  templateUrl: './dialogo-solicitar.component.html',
  styleUrls: ['./dialogo-solicitar.component.less']
})
export class DialogoSolicitarComponent implements OnInit {

	constructor(private dataService: DataServices, private snack: MatSnackBar, private _dialogRef: MatDialogRef<DialogoSolicitarComponent>, private _overlay: OverlayService) { }
  
	combosResponse = new SearchProductDropDownResponse();
	selectedFamily = new familyDropDown();
	familyId: number = 0;
	familyExpiration: boolean = false;
	originId: number = 0;
	newRecord = new LabelData();
	byFile: boolean = true;
	hasFile: boolean = false;
	fileLabel: string = "Archivo";
	productQty = 0;
	expiry = "";
	udid = "";
	fileData: string = null;
	overlayRef: OverlayRef;

	readonly CONSTANTS = new LabelConstants;

	busquedaCombos() {
		var request = new SearchProductDropDownRequest();
		request.company = parseInt(sessionStorage.getItem("company"));

		this.dataService.postData<SearchProductDropDownResponse>("Product/searchProductDropDown", sessionStorage.getItem("token"), request).subscribe(
			data => {
				this.combosResponse = data;
			},
			error => {

			}
		)
	}

	busquedaComboOrigen(e = null) {
		var request = new SearchOriginDropDownRequest();
		request.filter = this.familyId;
		if(this.selectedFamily.extra == 1){
			this.familyExpiration = true;
		}else{
			this.familyExpiration = false;
		}
		this.familyId = this.selectedFamily.id;
		var request = new SearchOriginDropDownRequest();
		request.filter = this.familyId;

		this.dataService.postData<SearchProductDropDownResponse>("Product/searchProductDropDownOrigen", sessionStorage.getItem("token"), request).subscribe(
			data => {
				this.combosResponse.originDropDown = data.originDropDown;
			},
			error => {

			}
		)
	}

	processFile(files) {
		var archivos = files[0];
		this.fileLabel = archivos['name'];
		this.getBase64(archivos).then(
			data => {
				if (this.familyId != 0)
					this.hasFile = true;

				this.fileData = data.toString();
			}
		);
	}

	getBase64(file) {
		return new Promise((resolve, reject) => {
			const reader = new FileReader();
			reader.readAsDataURL(file);
			reader.onload = () => resolve(reader.result);
			reader.onerror = error => reject(error);
		});
	}

	guardarSolicitud(){
		var send = false;
		if (this.familyId != 0 && this.originId != 0) {

			this.newRecord.companiaId = (sessionStorage.getItem("company") === null ? 0 : parseInt(sessionStorage.getItem("company")));
			this.newRecord.estatusSolicitudId = this.CONSTANTS.STATUS_SOLICITADO;
			this.newRecord.familiaId = this.familyId;
			this.newRecord.direccionId = this.originId;
			this.newRecord.usuarioId = parseInt(sessionStorage.getItem("idUser"));
			if(this.byFile){
				if(this.hasFile){
					this.newRecord.byFile = true;
					this.newRecord.Base64File = this.fileData;
					send = true;
				}
			}else{
				if(this.productQty > 0){
					console.trace("qty: " + this.productQty);
					this.newRecord.udid = this.udid;
					this.newRecord.cantidad = this.productQty;
					this.newRecord.caducidad = new Date(this.expiry);
					console.log("to the service");
					send = true;

				}
			}
			if(send){
				this.overlayRef = this._overlay.open();
				this.dataService.postData<SearchProductDropDownResponse>("Labels/guardarSolicitud", sessionStorage.getItem("token"), this.newRecord).subscribe(
					data => {
						console.log(data);
						this.openSnack("Solicitud creada exitosamente", "Aceptar");
						setTimeout(() => {
							this._overlay.close(this.overlayRef);
						}, 1);
						this._dialogRef.close(true);
					},
					error => {
						console.error(error);
						this.openSnack("Sucedió un error al generar solicitud", "Aceptar");
						setTimeout(() => {
							this._overlay.close(this.overlayRef);
						}, 1);
					}
				)
			}
			//this._dialogRef.close(true);
		}
	}

	openSnack = (message: string, action: string) => {
		this.snack.open(message, action, {
			duration: 5000
		})
	}

	ngOnInit() {
		this.busquedaCombos();
	}
	closeD = () => {
		this.guardarSolicitud();
		//this._dialogRef.close(true);
	}

}
