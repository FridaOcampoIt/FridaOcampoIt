import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar, MatSort } from '@angular/material';

import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import {
    SearchUserResponse,
    DataUserBackOffice,
    SearchUserDropDownResponse,
    SearchUserDropDownRequest,
    SearchUserRequest,
    DeleteUserRequest,
    UserProcessResponse
} from '../Interfaces/Models/UserModels';
import { DataServices } from '../Interfaces/Services/general.service';
import { DialogoEliminarComponent } from '../usuarios/dialogo-eliminar/dialogo-eliminar.component';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';

@Component({
    selector: 'app-usuarios',
    templateUrl: './usuarios.component.html',
    styleUrls: ['./usuarios.component.less']
})
export class UsuariosComponent implements OnInit, AfterViewInit {
    constructor(
        private _dialog: MatDialog,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private _router: Router,
        private _overlay: OverlayService) { }

    //Variables para la vista
    idUser: number = 0;
    idCompany: number = 0;
    idRol: number = 0;
    nameUser: string = "";
    response = new SearchUserResponse();
    responseDropDown = new SearchUserDropDownResponse();
    itemsPagina: number[] = enviroments.pageSize;
    overlayRef: OverlayRef;
    emptyList: boolean = false;

    //Representa componente de paginación para la busqueda
    @ViewChild(MatPaginator) paginator: MatPaginator;

    //Representa la instancia para el sorting de columnas
    @ViewChild(MatSort) sort: MatSort;

    //variables para configurar la tabla
    displayedColumns: string[] = ['select', 'name', 'email', 'profile', 'rol', 'company'];
    dataSource = new MatTableDataSource<DataUserBackOffice>(this.response.dataUser);
    selection = new SelectionModel<DataUserBackOffice>(false, []);

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
    checkboxLabel(row?: DataUserBackOffice): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }

    //Funcion para la busqueda combos para el modulo
    BusquedaCombos() {
        var requestCombos = new SearchUserDropDownRequest();
        requestCombos.company = parseInt(sessionStorage.getItem("company"));
        requestCombos.option = 1;

        this.dataService.postData<SearchUserDropDownResponse>("User/searchUserDropDown", sessionStorage.getItem("token"), requestCombos).subscribe(
            data => {
                this.responseDropDown = data;
                this.responseDropDown.dropDown.companies = this.responseDropDown.dropDown.companies.filter(x => x.data.length);
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
        )
    }

    //Funcion para la busqueda de usuarios
    Busqueda() {
        var requestBusqueda = new SearchUserRequest();
        requestBusqueda.company = this.idCompany;
        requestBusqueda.name = this.nameUser;
        requestBusqueda.rol = this.idRol;
        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);

        this.dataService.postData<SearchUserResponse>("User/searchUser", sessionStorage.getItem("token"), requestBusqueda).subscribe(
            data => {
                this.response = data;
                if (this.response.dataUser.length > 0)
                    this.emptyList = false;
                else
                    this.emptyList = true;
                this.dataSource.data = this.response.dataUser;
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
        )
    }

    //Funcion para agregar el usuario
    agregarUsuario = (bandera) => {
        var title = "Agregar Usuario";
        if (this.selection.selected.length > 0 && !bandera) {
            this.selection.selected.map(item => this.idUser = item.idUser);
            title = "Editar Usuario";
        }

        const dialogDef = this._dialog.open(DialogoAgregarComponent, {
            panelClass: 'dialog-aprod',
            disableClose: true,
            data: {
                id: this.idUser,
                title: title
            }
        });

        dialogDef.afterClosed().subscribe(result => {
            this.Busqueda();
            this.idUser = 0;
        });
    }

    //Funcion para eliminar un usuario
    EliminarUsuario(bandera: number) {
        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    var requestDelete = new DeleteUserRequest();
                    this.selection.selected.map(item => requestDelete.idUser = item.idUser);

                    this.dataService.postData<UserProcessResponse>("User/deleteUser", sessionStorage.getItem("token"), requestDelete).subscribe(
                        data => {
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");
                            } else {
                                this.openSnack("Usuario eliminado con éxito", "Aceptar");
                                this.Busqueda();
                            }
                        },
                        error => {
                            if (error.error.hasOwnProperty("messageEsp")) {
                                this.relogin("EliminarUsuario");
                            } else {
                                this.openSnack("Error al mandar la solicitud", "Aceptar");
                            }
                        }
                    );
                }
            });
        } else {
            var requestDelete = new DeleteUserRequest();
            this.selection.selected.map(item => requestDelete.idUser = item.idUser);

            this.dataService.postData<UserProcessResponse>("User/deleteUser", sessionStorage.getItem("token"), requestDelete).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Usuario eliminado con éxito", "Aceptar");
                        this.Busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("EliminarUsuario");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            );
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
                    case "Busqueda":
                        this.Busqueda();
                        break;
                    case "BusquedaCombos":
                        this.BusquedaCombos();
                        break;
                    case "EliminarUsuario":
                        this.EliminarUsuario(2);
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
        this.BusquedaCombos();
        this.dataSource.sort = this.sort;
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }
}
