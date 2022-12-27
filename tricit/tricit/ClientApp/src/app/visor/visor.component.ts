import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar, MatSort } from '@angular/material';

import { DataServices } from '../Interfaces/Services/general.service';
import { Router } from '@angular/router';
import { DialogoReporteComponent } from './dialogo-reporte/dialogo-reporte.component';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { DialogDeleteItem } from '../services-alerts/service-alerts.components';

import { SearchReportsRequest, ReportList, ReportData } from '../Interfaces/Models/VisorModels';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
@Component({
    selector: 'app-visor',
    templateUrl: './visor.component.html',
    styleUrls: ['./visor.component.less']
})
export class VisorComponent implements OnInit, AfterViewInit {


    @ViewChild(MatPaginator) paginator: MatPaginator;

    @ViewChild(MatSort) sort: MatSort;
    constructor(
        private _dialog: MatDialog,
        private snack: MatSnackBar,
        private dataService: DataServices,
        private _router: Router,
        private _overlay: OverlayService
    ) { }

    displayedColumns: string[] = [
        'select',
        'roboId',
        'tipoAlertaNombre',
        'nombreUsuario',
        'fechaRobo',
        'compania',
        'familia',
        'usuarioSolicitud',
        'tipoReporteNombre',
        'codigoAlerta'
    ];
    response = new ReportList();
    dataSource = new MatTableDataSource<ReportData>(this.response.listaRobo);
    selection = new SelectionModel<ReportData>(false, []);
    idCompany: number = parseInt(sessionStorage.getItem("company"));
    startDate = "";
    endDate = "";
    idRecord: number = 0;
    itemsPagina: number[] = enviroments.pageSize;
    addReport: boolean = sessionStorage.hasOwnProperty("Agregar Visor");
    viewReport: boolean = sessionStorage.hasOwnProperty("Visualizar Reporte");
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
    checkboxLabel(row?: ReportData): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }

    applyFilter(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
        this.dataSource.filter = filterValue;
    }

    busquedaGenerica: string = '';
    busqueda() {
        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);

        var request = new SearchReportsRequest();
        request.companiaFamilia = this.busquedaGenerica;
       

        this.dataService.postData<ReportList>("Robbery/buscarReportesRobo", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.paginator.pageSize = 20;
                this.response = data;
                console.log('Response ', this.response);
                if (this.response.listaRobo.length > 0)
                    this.emptyList = false;
                else
                    this.emptyList = true;
                this.dataSource.data = this.response.listaRobo;
                this.selection.clear();
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

    dialogoReporte = (edit) => {
        if (this.selection.isSelected && edit)
            this.selection.selected.map(item => this.idRecord = item.roboId);
        else
            this.idRecord = 0;
        const dialogRef = this._dialog.open(DialogoReporteComponent, {

            disableClose: true,
            panelClass: edit ? 'dialog-visor-edit' : 'dialog-aprod',
            data: {
                id: this.idRecord
            }
        });
        dialogRef.afterClosed().subscribe(result => {
            this.idRecord = 0;
            console.log("closed");
            setTimeout(() => this.busqueda(), 1);
        });
    }

    eliminarItem () {
        const dialogItem = this._dialog.open(DialogDeleteItem, {
            disableClose: true,
            panelClass:  'dialog-aprod',
            role: 'alertdialog',
            width: '400',
            height: '200',
            minWidth: '350',
            minHeight: '150',
            data: {
                _item:  this.selection.selected[0],
                _url: 'Robbery/eliminarAlertaRobo',
                _property: ['codigoAlerta','tipoReporteNombre'],
                _deleteProperty: ['roboId']
            }
        });

        
        dialogItem.afterClosed().subscribe(result => {
            setTimeout(() => this.busqueda(), 1);
        });
    }

    eliminarAlerta = () => {
        if (this.selection.isSelected)
            this.selection.selected.map(item => this.idRecord = item.roboId);
        else
            this.idRecord = 0;

        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);

        let data: any = {
            roboId: this.idRecord
        }

        this.dataService.postData<any>("Robbery/eliminarAlertaRobo", sessionStorage.getItem("token"), data).subscribe(
            data => {
                
                if (data["messageEsp"] != "") {
                    this.openSnack(data["messageEsp"], "Aceptar");
                  } else {

                    if (data["accion"] == 0) {
                        this.openSnack("Error en eliminar la notificación", "Aceptar");
                    } else if (data["accion"] == 1) {
                        this.openSnack("Alerta eliminada", "Aceptar");
    
                        this.idRecord = 0;
                        console.log("closed");
                        setTimeout(() => this.busqueda(), 1);
                    }

                }

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
                sessionStorage.setItem("company", data.userData.userData.company.toString());
                sessionStorage.setItem("isType", data.userData.userData.isType.toString());

                switch (peticion) {
                    case "Busqueda":
                        this.busqueda();
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
        this.busqueda();
        this.dataSource.sort = this.sort;
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }

}
