import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { DataServices } from '../../Interfaces/Services/general.service';
import { HttpClient, } from '@angular/common/http';
import {
	SearchProductDropDownRequest,
	SearchProductDropDownResponse,
	ProductDataRequest,
	SearchProductDataResponse,
	ImportProductRequest,
	ProcessResponse,
	familyDropDown,
	SearchOriginDropDownRequest
} from '../../Interfaces/Models/Product';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';


@Component({
	selector: 'app-dialogo-importar',
	templateUrl: './dialogo-importar.component.html',
	styleUrls: ['./dialogo-importar.component.less']
})
export class DialogoImportarComponent implements OnInit {

	constructor(
		@Inject(MAT_DIALOG_DATA) private _data: any,
		private _dialogRef: MatDialogRef<DialogoImportarComponent>,
		private dataService: DataServices,
		private snack: MatSnackBar,
		private http: HttpClient,
		private _overlay: OverlayService
	) { }

	dataEmbalajes: any = [];
	combosResponse = new SearchProductDropDownResponse();
	selectedFamily = new familyDropDown();
	familyExpiration: boolean = false;
	response = new SearchProductDataResponse();
	importRequest = new ImportProductRequest();
	overlayRef: OverlayRef;
	fileData: string = null;
	directionID: number = 0;
	familyID: number = 0;
	packagingId: number = 0;
	hasFile: boolean = false;
	fileLabel: string = "Archivo";
	byFile: boolean = true;
	productQty = 0;
	percentage = 0;
	total = 0;
	expiry = "";
	udid = "";
	numColumns: any = [];
	numContratation: any = [];
	columnas = 0;

	urlDescarga: string = "";
	@ViewChild("descargarDoc") btnDescarga: HTMLAnchorElement;

	BusquedaCombos() {
		var request = new SearchProductDropDownRequest();
		request.company = this._data["companiaId"]; //parseInt(localStorage.getItem("company"));

		setTimeout(() => { this.overlayRef = this._overlay.open(); }, 1);
		this.dataService.postData<SearchProductDropDownResponse>("Product/searchProductDropDownImport", sessionStorage.getItem("token"), request).subscribe(
			data => {
				setTimeout(() => { this._overlay.close(this.overlayRef); }, 1);
				this.combosResponse = data;
			},
			error => {
				setTimeout(() => { this._overlay.close(this.overlayRef); }, 1);
			}
		)
	}

	BusquedaComboOrigen(event) {
		//console.log(event);
		var selectFamily = event.target;
		//console.log(selectFamily.options[selectFamily.selectedIndex].getAttribute('caducidad'));
		if (this.selectedFamily.extra == 1) {
			this.familyExpiration = true;
		} else {
			this.familyExpiration = false;
		}
		this.familyID = this.selectedFamily.id;
		var request = new SearchOriginDropDownRequest();
		request.filter = this.familyID;

		if(this.familyID == undefined || this.familyID == 0) {
			this.combosResponse.originDropDown = [];
			return;
		}

		//this.obtenerEmbalajes();
		setTimeout(() => { this.overlayRef = this._overlay.open(); }, 1);
		this.dataService.postData<SearchProductDropDownResponse>("Product/searchProductDropDownOrigenImport", sessionStorage.getItem("token"), request).subscribe(
			data => {
				setTimeout(() => { this._overlay.close(this.overlayRef); }, 1);
				this.combosResponse.originDropDown = data.originDropDown;
			},
			error => {
				setTimeout(() => { this._overlay.close(this.overlayRef); }, 1);
			}
		)
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

	processFile(files) {
		var archivos = files[0];
		this.fileLabel = archivos['name'];
		this.getBase64(archivos).then(
			data => {
				if (this.familyID != 0)
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


	importProduct() {

		if (this.familyID != 0) {
			if (this.byFile) {
				if (this.hasFile) {
					this.importRequest.familyProductId = this.familyID;
					this.importRequest.directionId = this.directionID;
					this.importRequest.fileBase = this.fileData;
					this.importRequest.byFile = true;
					this.importRequest.userId = parseInt(sessionStorage.getItem("idUser"));
					this.importRequest.company = this._data["compania"];
					this.importRequest.family = this.combosResponse.familyDropDown.filter(reg => reg.id == this.familyID)[0]["data"];
					this.importRequest.packagingId = this.packagingId;
				}

			} else {

				if (this.productQty > 1000000) {
					this.openSnack("La cantidad máxima es 1,000,000",'Aceptar');
					return;
				}

				if (!(this.columnas > 0)) {
					this.openSnack("Seleccione la cantidad de columnas",'Aceptar');
					return;
				}

				if (this.familyID != 0) {
					this.importRequest.familyProductId = this.familyID;
					this.importRequest.directionId = this.directionID;
					this.importRequest.byFile = false;
					this.importRequest.amount = this.total;
					this.importRequest.expiry = this.expiry;
					this.importRequest.udid = this.udid;
					this.importRequest.userId = parseInt(sessionStorage.getItem("idUser"));
					this.importRequest.company = this._data["compania"];
					this.importRequest.family = this.combosResponse.familyDropDown.filter(reg => reg.id == this.familyID)[0]["data"];
					this.importRequest.packagingId = this.packagingId;
					this.importRequest.columns = this.columnas;
				}
			}

			this.overlayRef = this._overlay.open();
			this.dataService.postData<any>("Product/saveImportProduct", sessionStorage.getItem("token"), this.importRequest).subscribe(
				data => {
					//console.log(data);
					// this.udid = data.ciusFile;
					this.openSnack("Importación exitosa", "Aceptar");
					setTimeout(() => {
						this._overlay.close(this.overlayRef);
					}, 1);

					if (data["messageEsp"] != "") {
						this.openSnack(data["messageEsp"], "Aceptar");
					} else {
						this.urlDescarga = `${data["urlFile"]}${this.importRequest.company}_${this.importRequest.family}.` + (this.importRequest.byFile ? 'csv' : 'xlsx');
						window.open(this.urlDescarga);
						this._dialogRef.close(false);
					}

					
					// this._dialogRef.close({ insertado: this.udid, tipo: this.importRequest.byFile, familia: this.familyID });
				},
				error => {
					console.log("error", error);
					this.openSnack("Sucedio un error al importar productos", "Aceptar");
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

	ngOnInit() {
		this.BusquedaCombos();
		this.fileLabel = "Archivo";

		this.numColumns = Array(50).fill(0).map((x,i)=>i+1); // [0,1,2,3,4]
		this.numContratation = Array(21).fill(0).map((x,i)=>i); // [0,1,2,3,4]
	}

	closeD = () => {
		this.importProduct();
		//this._dialogRef.close(true);;
	}

	updateTotal() {
		this.total = Math.round(((this.productQty * this.percentage) / 100) + this.productQty);
	}


}
