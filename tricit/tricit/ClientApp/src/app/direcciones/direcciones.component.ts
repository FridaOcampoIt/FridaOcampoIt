import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar, MatSort } from '@angular/material';
import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component';

//DataServices
import {
    SearchDropDownListAddressResponse,
    SearchAddressResponse,
    AddressesData,
    SearchDropDownListAddressRequest,
    SearchAddressRequest,
    DeleteAddressRequest,
    AddressProcessResponse
} from '../Interfaces/Models/AddressModels';
import { DataServices } from '../Interfaces/Services/general.service';
import { LoginUserResponse, LoginUserRequest } from '../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';

@Component({
    selector: 'app-direcciones',
    templateUrl: './direcciones.component.html',
    styleUrls: ['./direcciones.component.less']
})
export class DireccionesComponent implements OnInit, AfterViewInit {

    constructor(
        private _dialog: MatDialog,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private _router: Router,
        private _overlay: OverlayService) { }

    //variable para controlar el paginado de la tabla
    @ViewChild(MatPaginator) paginator: MatPaginator;

    //Representa la instancia para el sorting de columnas
    @ViewChild(MatSort) sort: MatSort;

    //Modelos para la vista
    response = new SearchAddressResponse();
    responseListDropDown = new SearchDropDownListAddressResponse();

    companiaActual = parseInt(sessionStorage.getItem("company"));

    allowAdd: boolean = sessionStorage.hasOwnProperty('Agregar Direcciones');
	allowEdit: boolean = sessionStorage.hasOwnProperty('Editar Direcciones');
    allowDelete: boolean = sessionStorage.hasOwnProperty('Eliminar Direcciones');

    idAddress: number = 0;
    idCompany: number = 0;
    idFamily: number = 0;
    itemsPagina: number[] = enviroments.pageSize;

    displayedColumns: string[] = ['select', 'serviceCenter', 'company', 'type'];
    dataSource = new MatTableDataSource<AddressesData>(this.response.addressDataList);
    selection = new SelectionModel<AddressesData>(false, []);
    overlayRef: OverlayRef;
    emptyList: boolean = false;
    companies : any[] = [];

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
    checkboxLabel(row?: AddressesData): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }

    //Funcion para la busqueda de combos
    BusquedaCombos() {
        var request = new SearchDropDownListAddressRequest();
        request.idCompany = parseInt(sessionStorage.getItem("company"));

        this.dataService.postData<SearchDropDownListAddressResponse>("Address/searchDropDownListAddress", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.responseListDropDown = data;
                this.companies = this.responseListDropDown.companyData.filter(x => x.data.length);
                this.idCompany = parseInt(sessionStorage.getItem("company"));
                this.Busqueda();
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("BusquedaCombos");
                } else {
                    console.log(error);
                }
            }
        );
    }

    //Funcion para realizar la busqueda de las direcciones
    Busqueda() {
        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);
        var request = new SearchAddressRequest();
        request.idCompany = this.idCompany;
        request.idFamily = this.idFamily;

        this.dataService.postData<SearchAddressResponse>("Address/searchAddress", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.response = data;
                if (this.response.addressDataList.length > 0)
                    this.emptyList = false;
                else
                    this.emptyList = true;
                this.dataSource.data = this.response.addressDataList;
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

    //función para agregar o editar una dirección
    agregarDireccion = (bandera) => {
        var title = "Agregar Dirección";
        if (this.selection.selected.length > 0 && !bandera) {
            this.selection.selected.map(item => this.idAddress = item.idAddress);
            title = "Editar Dirección";
        }

        const dialogRef = this._dialog.open(DialogoAgregarComponent, {
            panelClass: "dialog-direc",
            disableClose: true,
            data: {
                id: this.idAddress,
                title: title
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            this.idAddress = 0;
            this.Busqueda();
        });
    }

    //Funcion para eliminar la dirección
    eliminarDireccion(bandera: number) {
        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true,
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    var requestDelete = new DeleteAddressRequest();
                    this.selection.selected.map(item => requestDelete.idAddress = item.idAddress);

                    this.dataService.postData<AddressProcessResponse>("Address/deleteAddress", sessionStorage.getItem("token"), requestDelete).subscribe(
                        data => {
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");
                            } else {
                                this.openSnack("Dirección eliminada con éxito", "Aceptar");
                                this.Busqueda();
                            }
                        },
                        error => {
                            if (error.error.hasOwnProperty("messageEsp")) {
                                this.relogin("eliminarDireccion");
                            } else {
                                this.openSnack("Error al mandar la solicitud", "Aceptar");
                            }
                        }
                    )
                }
            });
        }
        else {
            var requestDelete = new DeleteAddressRequest();
            this.selection.selected.map(item => requestDelete.idAddress = item.idAddress);

            this.dataService.postData<AddressProcessResponse>("Address/deleteAddress", sessionStorage.getItem("token"), requestDelete).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Dirección eliminada con éxito", "Aceptar");
                        this.Busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("eliminarDireccion");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            )
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
                    case "BusquedaCombos":
                        this.BusquedaCombos();
                        break;
                    case "eliminarDireccion":
                        this.eliminarDireccion(2);
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
        this.BusquedaCombos();
        this.dataSource.sort = this.sort;
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }
}
