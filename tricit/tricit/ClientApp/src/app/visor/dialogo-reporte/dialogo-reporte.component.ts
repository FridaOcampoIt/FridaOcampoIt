import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, Inject, OnInit , ElementRef , ViewChild} from '@angular/core';
import { DataServices } from '../../Interfaces/Services/general.service';

import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router } from '@angular/router';
import { DynamicDatabase, DynamicFlatNode, DynamicDataSource } from '../TreeClasses';

import { mapeoControlesMaestrosMultiples } from '../../Interfaces/Means/ControlesMaestrosMultiples'

import { searchProductsRequest, ListFamilyProducts, SaveReportRequest, ReportList, LoadReportRequest } from '../../Interfaces/Models/VisorModels';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { listener } from '@angular/core/src/render3';
import { ValueTransformer } from '@angular/compiler/src/util';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { formatDate } from '@angular/common';
import { SearchUserDropDownRequest, SearchUserDropDownResponse } from '../../Interfaces/Models/UserModels';
import { SearchFamilyProductRequest, SearchFamilyProductResponse } from '../../Interfaces/Models/FamilyModels';

@Component({
	selector: 'app-dialogo-reporte',
	templateUrl: './dialogo-reporte.component.html',
	styleUrls: ['./dialogo-reporte.component.css']
})
export class DialogoReporteComponent implements OnInit {
	tipoReportes: any[] = [
		{ tipo: 0, desc: "Seleccione una opción" },
		{ tipo: 1000008, desc: "Lote (Traceit)" },
		{ tipo: 1000009, desc: "Familia" },
		{ tipo: 1000010, desc: 'Pallet' },
		{ tipo: 1000011, desc: 'Caja' },
		{ tipo: 1000012, desc: 'CIU' },
		{ tipo: 1000013, desc: 'Agrupación' }
	];

	tipoAlertas: any[] = [
		{ tipo: 1000001, desc: "Recall", color: "Amarillo" },
		{ tipo: 1000002, desc: "Robo", color: "Rojo" },
		{ tipo: 1000003, desc: "Otro", color: "Azul" },
	];

	listaCajas: any[] = [];

	responseDropDown = new SearchUserDropDownResponse();

	tipoAlertaId: number = 0;
	tipoReporteId: number = 0;
	codigoAlerta: any = ""

	constructor(
		@Inject(MAT_DIALOG_DATA) private _data: any,
		private dataService: DataServices,
		private _dialogRef: MatDialogRef<DialogoReporteComponent>,
		private snack: MatSnackBar,
		private _router: Router,
		private database: DynamicDatabase,
		private _overlay: OverlayService) {

		this.treeControl = new FlatTreeControl<DynamicFlatNode>(this.getLevel, this.isExpandable);
	}
	treeControl: FlatTreeControl<DynamicFlatNode>;
	combosResponse = new SearchFamilyProductResponse();
	familyId: number = 0;
	originId: number = 0;
	dataSource: DynamicDataSource;
	getLevel = (node: DynamicFlatNode) => node.level;
	isExpandable = (node: DynamicFlatNode) => node.expandable;
	hasChild = (_: number, _nodeData: DynamicFlatNode) => _nodeData.expandable;
	family: number = 0;
	codeSearch: string = "";
	idCompany: number = parseInt(sessionStorage.getItem("company"));
	addedProducts = [];
	descriptionEN: string = "";
	alertTitleEN: string = "";
	descriptionES: string = "";
	alertTitleES: string = "";
	companiaId: number = 0;
	familiaId: number = 0;
	usuarioSolicitud: string = "";
	notifyMovilCiu: boolean = false;
	notifyTracking: boolean = false;
	productList = [];
	agrupacionId: boolean = false;
	overlayRef: OverlayRef;

	tmpFile: any = "";

	fileData = new FormData();
	archivo = {
		nombre: "",
		nombreArchivo: "",
		base64textString: ""
	};

	action = "Agregar";

	/**1 GENERAR REPORTE, 2 VER REPORTE*/
	type = 1;

	selectedValue: any;
    searchTxt: any;
    @ViewChild('search') searchElement: ElementRef;
    doSomething(any){
        setTimeout(()=>{ // this will make the execution after the above boolean has changed
            this.searchElement.nativeElement.focus();
        },0);  
    }
	
	selectedValueFamily: any;
    searchTxtFamily: any;
    @ViewChild('searchFamilyInput') searchElementFamily: ElementRef;
    doSomethingFamily(any){
        setTimeout(()=>{ // this will make the execution after the above boolean has changed
            this.searchElementFamily.nativeElement.focus();
        },0);  
    }

	
	selectedValueUser: any;
    searchTxtUser: any;
    @ViewChild('searchUser') searchElementUser: ElementRef;
    doSomethingUser(any){
        setTimeout(()=>{ // this will make the execution after the above boolean has changed
            this.searchElementUser.nativeElement.focus();
        },0);  
    }
	users : Array<any> = [];
	usuarioId: number = 0;
	busquedaproductos(companiaId = 0, familyId = 0, usuarioId = 0) {
		if (companiaId == 0) {
			this.openSnack("Favor de seleccionar una Compañia", "Aceptar");
			return;
		}

		setTimeout(() => {
			this.overlayRef = this._overlay.open();
		}, 1);

		var request = new SearchFamilyProductRequest();
		request.name = "";
		request.companyId = companiaId;

		this.dataService.postData<SearchFamilyProductResponse>("Families/searchFamilyProduct", sessionStorage.getItem("token"), request).subscribe(
			data => {
				this.familiaId = 0;

				this.combosResponse = data;
				console.log(this.combosResponse);
				if (this.type == 1) {

				}
				else if (this.type == 2)
					this.familiaId = familyId;
			},
			error => {

			}
		);

		this.dataService.postData<any>("User/getUsuarioByCompanyId", sessionStorage.getItem("token"), request).subscribe(
			data => {
				this.usuarioId = 0;

				this.users = data['dataUserByCompanyId'];
				console.log('USERS', this.users);
				if (this.type == 2)
					this.usuarioId = usuarioId;
			},
			error => {

			}
		);

		setTimeout(() => {
			this._overlay.close(this.overlayRef);
		}, 1000);
	}

	BusquedaCombos() {
		var requestCombos = new SearchUserDropDownRequest();
		requestCombos.company = parseInt(sessionStorage.getItem("company"));
		requestCombos.option = 1;

		this.dataService.postData<SearchUserDropDownResponse>("User/searchUserDropDown", sessionStorage.getItem("token"), requestCombos).subscribe(
			data => {
				this.responseDropDown = data;
				this.responseDropDown.dropDown.companies = this.responseDropDown.dropDown.companies.filter(x => x.data.length);

				if (this.type == 2) {
						this.loadReport();
				}
			},
			error => {
				if (error.error.hasOwnProperty("messageEsp")) {
					this.relogin("BusquedaCombos");
				} else {
					console.log(error);
				}
			}
		)
	}

	detalleCajas: any = "";
	loadReport() {
		var request = new LoadReportRequest();
		request.roboId = this._data.id;
		this.overlayRef = this._overlay.open();

		this.dataService.postData<SaveReportRequest>("Robbery/roboDetalle", sessionStorage.getItem("token"), request).subscribe(
			data => {
				console.log('DETALLE REPPORTE ROBO', data);

				this.busquedaproductos(data.companiaId, data.familiaId, data.usuarioSolicitoId);

				this.descriptionEN = data.descripcionEN;
				this.alertTitleEN = data.tituloAlertaEN;
				this.descriptionES = data.descripcionES;
				this.alertTitleES = data.tituloAlertaES;
				this.companiaId = data.companiaId;
				this.tipoAlertaId = data.tipoAlertaId;
				this.tipoReporteId = data.tipoReporteId;
				this.usuarioId = data.usuarioSolicitoId;
				this.archivo.nombre = data.nombreArchivo;
				this.notifyTracking = data.notificacionTracking;
				this.notifyMovilCiu = data.notificacionMovilCiu;
				this.familiaId = data.familiaId;
				this.codigoAlerta = data.codigoAlerta;
				this.agrupacionId = data.agrupacionId;
				//this.detalleCajas = JSON.parse(data.detalle);
				this.detalleCajas = data.detalle ? JSON.parse(data.detalle): null;
				console.log('Cajas', this.detalleCajas, data.detalle)
				this._overlay.close(this.overlayRef);
			},
			error => {
				if (error.error.hasOwnProperty("messageEsp")) {
					this.relogin("loadReport");
				} else {
					this.openSnack("Error al mandar la solicitud", "Aceptar");
				}
				this._overlay.close(this.overlayRef);
			}
		);
	}

	addProduct(node) {
		console.log(node);
		var productToAdd = { 'ciu': node.item, 'id': node.productId };
		var index = this.addedProducts.findIndex(x => x.id == node.productId && x.ciu == node.item);
		console.log(productToAdd);
		console.log(index);
		if (index === -1)
			this.addedProducts.push(productToAdd);
		else
			this.openSnack("Este producto ya ha sido agregado a la lista", "Aceptar");
	}

	removeProduct(index) {
		this.addedProducts.splice(index, 1);
	}

	save() {
		if (this.descriptionES == "" || this.alertTitleES == "" || (this.tipoReporteId == 1000009 ?  this.familiaId == 0 : this.codigoAlerta == ""  ) || this.archivo.nombre == "" || this.familiaId == 0 || this.companiaId == 0) {
			this.openSnack("Faltan datos obligatorios", "Aceptar");
			return;
		} else {
			var request = new SaveReportRequest();

			request.tituloAlertaES = this.alertTitleES;
			request.descripcionES = this.descriptionES;
			request.tituloAlertaEN = this.alertTitleEN;
			request.descripcionEN = this.descriptionEN;
			request.notificacionMovilCiu = this.notifyMovilCiu;
			request.notificacionTracking = this.notifyTracking;
			request.usuarioCreadorId = parseInt(sessionStorage.getItem("idUser"));
			request.companiaId = this.companiaId;
			request.usuarioSolicitoId = this.usuarioId;
			request.tipoAlertaId = this.tipoAlertaId;
			request.nombreArchivo = this.archivo.nombreArchivo;
			request.tipoReporteId = this.tipoReporteId;
			request.familiaId = this.familiaId;
			request.codigoAlerta = this.codigoAlerta;
			request.agrupacionId = Boolean(this.agrupacionId);
			console.log('LO QUE SE ENVIA', request);
			this.overlayRef = this._overlay.open();
			this.dataService.postData<SaveReportRequest>("Robbery/guardarReporteRobo", sessionStorage.getItem("token"), request).subscribe(
				data => {
					if (data['messageEsp'] != "") {
						setTimeout(() => {
							this._overlay.close(this.overlayRef);
						}, 1);
						this.openSnack(data['messageEsp'], "Aceptar");
					} else {
						//loading overlay here!
						console.log("Reporte guardado con éxito");

						if (this.archivo.nombreArchivo.length > 0) {
							this.saveDoc();
						} else {
							setTimeout(() => {
								this._overlay.close(this.overlayRef);
							}, 1);
							this._dialogRef.close(true);
							this.openSnack("Reporte almacenado con éxito", "Aceptar");
						}
					}
				},
				error => {
					this.openSnack("Error al mandar la solicitud", "Aceptar");
					this._overlay.close(this.overlayRef);
				}
			);
			//this._dialogRef.close(true);
		}
	}

	saveDoc() {
		console.log('this.fileData', this.fileData);
		this.dataService.postDataDocs<any>("Robbery/saveDocsRobery", sessionStorage.getItem("token"), this.fileData).subscribe(
			data => {

				setTimeout(() => {
					this._overlay.close(this.overlayRef);
				}, 1);
				if (data.messageEsp != "") {
					this.openSnack(data.messageEsp, "Aceptar");
				} else if (data.accion != 1) {
					this.openSnack("Hubo un error al guardar el documento.", "Aceptar");
				} else {
					this.openSnack("Reporte almacenado con éxito", "Aceptar");
					this.tmpFile = "";
					this.archivo = {
						nombre: "",
						nombreArchivo: "",
						base64textString: ""
					};
					this.fileData = new FormData();
					this._dialogRef.close(true);
				}
			},
			error => {
				this._overlay.close(this.overlayRef);
				if (error.error.hasOwnProperty("messageEsp")) {
					this.relogin("SaveDocs");
				} else {
					this.openSnack("Error al mandar la solicitud", "Aceptar");
				}
			}
		)
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
					case "BusquedaCombos":
						this.BusquedaCombos();
						break;
					case "Guardar":
						this.save();
						break;
					case "loadReport":
						this.loadReport();
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
		console.log('DATA', this._data, this.type, this.action);
		if (this._data.id > 0) {
			this.type = 2;//view
			this.action = "Visualizar";
		}
		this.BusquedaCombos();
	}

	seleccionarArchivo(event) {
		this.fileData = new FormData();
		if (event.target.files.length > 0) {
			var allowedExtensions = /(\.jpg|\.jpeg|\.png|\.pdf|\.docx|\.doc|\.xlsx|\.xls|\.csv|\.msg)$/i;
			var FileSize = event.target.files[0].size / 1024 / 1024;

			if (FileSize > 10) {
				this.openSnack("El archivo debe de tener un tamaño máximo 10 MB", "Aceptar");
				return;
			}

			let newNombre = event.target.files[0].name;
			if (!allowedExtensions.exec(newNombre.toLowerCase())) {
				this.openSnack("Solo se aceptan archivos jpg, jpeg, png, pdf, xlsx, xls, csv, docx, doc, msg", "Aceptar");
				return;
			}
			this.archivo.nombreArchivo = newNombre;
			this.archivo.nombre = newNombre;

			this.fileData = new FormData();
			this.fileData.append('fileData', event.target.files[0], this.archivo.nombre);
			this.fileData.append('fileName', this.archivo.nombre);
			this.fileData.append("doc", this.archivo.nombreArchivo);
			this.fileData.append("fecha", formatDate(new Date(), 'yyyy-MM-dd hh:mm:ss a', 'en-US', '0500'));
			this.fileData.append("nombreDoc", this.archivo.nombre);
		}
	}
}
