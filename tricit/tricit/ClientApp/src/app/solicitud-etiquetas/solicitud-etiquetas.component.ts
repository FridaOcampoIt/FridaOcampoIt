import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar, MatSort } from '@angular/material';

import { DialogoHistorialComponent } from './dialogo-historial/dialogo-historial.component';
import { DialogoSeguimientoComponent } from './dialogo-seguimiento/dialogo-seguimiento.component';
import { DialogoSolicitarComponent } from './dialogo-solicitar/dialogo-solicitar.component';
import { Router } from '@angular/router';
import { DataServices } from '../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';
import { 
	SearchLabelDropDownRequest,
	SearchLabelDropDownResponse,
	SearchLabelRequest,
	SearchLabelsResponse,
	LabelData,
	LabelConstants
} from '../Interfaces/Models/LabelModels';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';

export interface Solicitud {
    folio: number;
    fecha: string;
    estatus: string;
    familia: string;
    compania: string;
}

const ELEMENT_DATA: Solicitud[] = [
    { folio: 123456789, fecha: '20/20/20', estatus: 'archivado', familia: 'Familia 1', compania: 'Compañia 1' },
    { folio: 123456789, fecha: '20/20/20', estatus: 'archivado', familia: 'Familia 1', compania: 'Compañia 1' },
    { folio: 123456789, fecha: '20/20/20', estatus: 'archivado', familia: 'Familia 1', compania: 'Compañia 1' },
    { folio: 123456789, fecha: '20/20/20', estatus: 'archivado', familia: 'Familia 1', compania: 'Compañia 1' },
    { folio: 123456789, fecha: '20/20/20', estatus: 'archivado', familia: 'Familia 1', compania: 'Compañia 1' },
    { folio: 123456789, fecha: '20/20/20', estatus: 'archivado', familia: 'Familia 1', compania: 'Compañia 1' },
    { folio: 123456789, fecha: '20/20/20', estatus: 'archivado', familia: 'Familia 1', compania: 'Compañia 1' },
    { folio: 123456789, fecha: '20/20/20', estatus: 'archivado', familia: 'Familia 1', compania: 'Compañia 1' }
]
const CONSTANTS = new LabelConstants;
const initialSelection = [];
const allowMultiSelect = false;

@Component({
    selector: 'app-solicitud-etiquetas',
    templateUrl: './solicitud-etiquetas.component.html',
    styleUrls: ['./solicitud-etiquetas.component.less']
})
export class SolicitudEtiquetasComponent implements OnInit, AfterViewInit {

	@ViewChild(MatPaginator) paginator: MatPaginator;
	
	//Representa la instancia para el sorting de columnas
    @ViewChild(MatSort) sort: MatSort;

	constructor(private _dialog: MatDialog, private _router: Router, private dataService: DataServices, private snack: MatSnackBar, private _overlay: OverlayService) { }

    displayedColumns: string[] = ['select', 'folio', 'fechaGeneracion', 'status', 'familia', 'companiaNombre'];
	response = new SearchLabelsResponse();
    dataSource = new MatTableDataSource<LabelData>(this.response.listaSolicitudEtiquetas);
    selection = new SelectionModel<LabelData>(allowMultiSelect, []);
	idCompany: number = (sessionStorage.getItem("company") === null ? -1 : parseInt(sessionStorage.getItem("company")));
	combosResponse = new SearchLabelDropDownResponse();
	folio: string = "";
	companiaCmb: number = 0;
	idRecord: number = 0;
	itemsPagina: number[] = enviroments.pageSize;
	addSolicity: boolean = sessionStorage.hasOwnProperty("Agregar Solicitud");
	viewHistory: boolean = sessionStorage.hasOwnProperty("Visualizar Historial de Seguimiento");
	addHistory: boolean = sessionStorage.hasOwnProperty("Agregar Bitácora");
	overlayRef: OverlayRef;
	emptyList: boolean = false;

    /** Whether the number of selected elements matches the total number of rows. */
    isAllSelected() {
        const numSelected = this.selection.selected.length;
        const numRows = this.dataSource.data.length;
        return numSelected === numRows;
    }

    /** Selects all rows if they are not all selected; otherwise clear selection. */
    masterToggle() {
        this.isAllSelected() ?
            this.selection.clear() :
            this.dataSource.data.forEach(row => this.selection.select(row));
    }

    /** The label for the checkbox on the passed row */
    checkboxLabel(row?: Solicitud): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }

	BusquedaCombos() {
		//var request = new SearchLabelDropDownRequest();
		this.dataService.postData<SearchLabelDropDownResponse>("Labels/buscarCompanias", sessionStorage.getItem("token"), this.idCompany).subscribe(
			data => {
			console.log(data);
				this.combosResponse = data;
				this.combosResponse.listaCompanias = this.combosResponse.listaCompanias.filter(x => x.nombre != "");
				this.Busqueda();
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

	Busqueda() {
		var request = new SearchLabelRequest();
		request.companiaId = this.companiaCmb;
		request.folio = this.folio;
		setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);


		this.dataService.postData<SearchLabelsResponse>("Labels/buscarSolicitudEtiquetas", sessionStorage.getItem("token"), request).subscribe(
			data => {
				this.paginator.pageSize = 20;
				this.response = data;
				if (this.response.listaSolicitudEtiquetas.length > 0)
					this.emptyList = false;
				else
					this.emptyList = true;
				this.dataSource.data = this.response.listaSolicitudEtiquetas;
				this.selection.clear();
				//this.selection.
				//console.log(this.response);
				setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
			},
			error => {
				if (error.error.hasOwnProperty("messageEsp")) {
					this.relogin("Busqueda");
				} else {
					console.log(error);
				}
				setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
			}
		);
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
					case "BusquedaCombos":
						this.BusquedaCombos();
						break;
					case "Busqueda":
						this.Busqueda();
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

    dialogoHistorial = () => {
		this.selection.selected.map(item => this.idRecord = item.solicitudId);
        const dialogRef = this._dialog.open(DialogoHistorialComponent, {
            panelClass: "dialog-historial",
            disableClose: true,
			data: {
                id: this.idRecord,
            }
        });
        dialogRef.afterClosed().subscribe(result => {
			this.idRecord = 0;
			setTimeout(() => this.Busqueda(), 500);
		});
    }

    dialogoSeguimiento = () => {
		this.selection.selected.map(item => this.idRecord = item.solicitudId);
        const dialogRef = this._dialog.open(DialogoSeguimientoComponent, {
            disableClose: true,
			data: {
                id: this.idRecord,
            }
        });
        dialogRef.afterClosed().subscribe(result => { });
    }

    dialogoSolicitar = () => {
        const dialogRef = this._dialog.open(DialogoSolicitarComponent, {
            panelClass: "dialog-aprod",
            disableClose: true,
        });
        dialogRef.afterClosed().subscribe(result => { setTimeout(() => this.Busqueda(), 500); });
    }

    ngOnInit() {
		this.companiaCmb = this.idCompany;
		this.BusquedaCombos();
		this.dataSource.sort = this.sort;
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }

}
