import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar, MatSort } from '@angular/material';
import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';

//DataServices and Models
import {
    SearchCompanyRequest,
    SearchCompanyResponse,
    CompaniesData,
    DeleteCompanyRequest,
    CompanyProcessResponse
} from '../Interfaces/Models/CompanyModels';
import { DataServices } from '../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';
import { enviroments } from "../Interfaces/Enviroments/enviroments";

@Component({
    selector: 'app-companias',
    templateUrl: './companias.component.html',
    styleUrls: ['./companias.component.less']
})
export class CompaniasComponent implements OnInit, AfterViewInit {
    constructor(
        private _dialog: MatDialog,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private _router: Router,
        private _overlay: OverlayService) { }

    //Variables para la vista
    name: string = "";
    businessName: string = "";
    idCompany: number = 0;
    response = new SearchCompanyResponse();
    itemsPagina: number[] = enviroments.pageSize;
    overlayRef: OverlayRef;
    emptyList: boolean = false;

    //Representa componente de paginación para la busqueda
    @ViewChild(MatPaginator) paginator: MatPaginator;

    //Representa la instancia para el sorting de columnas
    @ViewChild(MatSort) sort: MatSort;

    //variables para configurar la tabla
    displayedColumns: string[] = ['select', 'name', 'businessName', 'phone'];
    dataSource = new MatTableDataSource<CompaniesData>(this.response.companiesDataList);
    selection = new SelectionModel<CompaniesData>(false, []);

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
    checkboxLabel(row?: CompaniesData): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }

    //Funcion de incio de la vista
    ngOnInit() {
        this.Busqueda();
    }

    //funcion para redireccionar a la vista de agregar compañia
    agregarCompania = (bandera) => {
        var title = "Agregar Compañía";
        if (this.selection.selected.length > 0 && !bandera) {
            this.selection.selected.map(item => this.idCompany = item.idCompany);
            title = "Editar Compañía"
        }

        const dialogRef = this._dialog.open(DialogoAgregarComponent, {
            width: 'dialog-comp',
            disableClose: true,
            data: {
                id: this.idCompany,
                title: title
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            this.idCompany = 0;
            this.Busqueda();
        });
    }

    //Funcion para realizar la busqueda
    Busqueda() {
        var request = new SearchCompanyRequest();
        request.name = this.name;
        request.businessName = this.businessName;
        this.idCompany = 0;

        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);

        this.dataService.postData<SearchCompanyResponse>("Companies/searchCompany", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.response = data;
                if (this.response.companiesDataList.length > 0)
                    this.emptyList = false;
                else
                    this.emptyList = true;
                this.dataSource.data = this.response.companiesDataList.filter(x => x.name != "");
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

    //Funcion para abrir el modal de confirmación y eliminar la compañia seleccionada
    eliminarCompania = (bandera: number) => {
        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    var requestDelete = new DeleteCompanyRequest();
                    this.selection.selected.map(item => requestDelete.idCompany = item.idCompany);

                    this.dataService.postData<CompanyProcessResponse>("Companies/deleteCompany", sessionStorage.getItem("token"), requestDelete).subscribe(
                        data => {
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");
                                console.log(data);
                            } else {
                                this.openSnack("Compañía eliminada con éxito", "Aceptar");
                                this.Busqueda();  
                            }
                        },
                        error => {
                            if (error.error.hasOwnProperty("messageEsp")) {
                                this.relogin("eliminarCompania");
                            } else {
                                this.openSnack("Error al mandar la solicitud", "Aceptar");
                            }
                        }
                    );
                }
            });
        }
        else {
            var requestDelete = new DeleteCompanyRequest();
            this.selection.selected.map(item => requestDelete.idCompany = item.idCompany);

            this.dataService.postData<CompanyProcessResponse>("Companies/deleteCompany", sessionStorage.getItem("token"), requestDelete).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Compañía eliminada con éxito", "Aceptar");
                        this.Busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("eliminarCompania");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            );
        }
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
                    case "Busqueda":
                        this.Busqueda();
                        break;
                    case "eliminarCompania":
                        this.eliminarCompania(2);
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

    //Funcion despues de obtener todos los datos se establece el paginado
    ngAfterViewInit() {
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
    }
}
