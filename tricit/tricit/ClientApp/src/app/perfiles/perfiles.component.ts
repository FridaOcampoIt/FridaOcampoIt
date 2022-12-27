import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar, MatSort } from '@angular/material';

import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import { DataServices } from '../Interfaces/Services/general.service';
import {
    Profiles,
    SearchProfileResponse,
    SearchDropDownPermissionResponse,
    SearchDropDownPermissionRequest,
    SearchProfileRequest,
    DeleteProfileRequest,
    SaveUpdateProccess
} from '../Interfaces/Models/ProfilesModels';
import { DialogoEliminarComponent } from '../perfiles/dialogo-eliminar/dialogo-eliminar.component';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';


@Component({
    selector: 'app-perfiles',
    templateUrl: './perfiles.component.html',
    styleUrls: ['./perfiles.component.less']
})
export class PerfilesComponent implements OnInit, AfterViewInit {
    constructor(
        private _dialog: MatDialog,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private _router: Router,
        private _overlay: OverlayService) { }

    //Variables para la vista
    idProfile: number = 0;
    nameProfile: string = "";
    idCompany: number = 0;
    response = new SearchProfileResponse();
    responseDropDown = new SearchDropDownPermissionResponse();
    itemsPagina: number[] = enviroments.pageSize;
    overlayRef: OverlayRef;
    emptyList: boolean = false;
    haveCompany: number;
    companies: any[] = [];
    //Representa componente de paginación para la busqueda
    @ViewChild(MatPaginator) paginator: MatPaginator;

    //Representa la instancia para el sorting de columnas
    @ViewChild(MatSort) sort: MatSort;

    //variables para configurar la tabla
    displayedColumns: string[] = ['select', 'name', 'company'];
    dataSource = new MatTableDataSource<Profiles>(this.response.profiles);
    selection = new SelectionModel<Profiles>(false, []);

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
    checkboxLabel(row?: Profiles): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }

    //Función para agregar un perfil
    AgregarPerfil = (bandera) => {
        var title = "Agregar Perfil";
        if (this.selection.selected.length > 0 && !bandera) {
            this.selection.selected.map(item => this.idProfile = item.profileId);
            title = "Editar Perfil";
        }

        const dialogDef = this._dialog.open(DialogoAgregarComponent, {
            panelClass: 'dialog-aprod',
            disableClose: true,
            data: {
                id: this.idProfile,
                title: title
            }
        });

        dialogDef.afterClosed().subscribe(res => {
            this.Busqueda();
            this.idProfile = 0;
        });
    }

    //Funcion para realizar la busqueda de perfiles
    Busqueda() {
        //Crear referencia del overlay;
        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);

        var requestBusqueda = new SearchProfileRequest();
        requestBusqueda.company = this.idCompany;
        requestBusqueda.name = this.nameProfile;

        this.dataService.postData<SearchProfileResponse>("Profiler/searchProfile", sessionStorage.getItem("token"), requestBusqueda).subscribe(
            data => {
				this.response = data;

				if (this.response.profiles.length > 0)
					this.emptyList = false;
				else
					this.emptyList = true;
                this.dataSource.data = this.response.profiles;
                this.selection.clear();
                // this.selection = new SelectionModel<Profiles>(false, []);
                // this.dataSource.paginator = this.paginator;
                // this.dataSource.sort = this.sort;

                //Dispose/Terminar la Referencia del overlay
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

                //Dispose/Terminar la Referencia del overlay
                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
            }
        )
    }

    //Funcion para la busqueda de los combos
    BusquedaCombos() {
        var requestCombos = new SearchDropDownPermissionRequest();
        requestCombos.company = parseInt(sessionStorage.getItem("company"));

        this.dataService.postData<SearchDropDownPermissionResponse>("Profiler/searchDropDownPermission", sessionStorage.getItem("token"), requestCombos).subscribe(
            data => {
                this.responseDropDown = data;
                console.log("equisde");
                this.companies = this.responseDropDown.dropDownProfilesPermission.companyList.filter(x => x.data.length);
                
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

    //Funcion para eliminar el perfil
    EliminarPerfil(bandera: number) {
        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    var requestDelete = new DeleteProfileRequest();
                    this.selection.selected.map(item => requestDelete.profileId = item.profileId);

                    this.dataService.postData<SaveUpdateProccess>("Profiler/deleteProfile", sessionStorage.getItem("token"), requestDelete).subscribe(
                        data => {
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");
                            } else {
                                this.openSnack("Perfil eliminado con éxito", "Aceptar");
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
            var requestDelete = new DeleteProfileRequest();
            this.selection.selected.map(item => requestDelete.profileId = item.profileId);

            this.dataService.postData<SaveUpdateProccess>("Profiler/deleteProfile", sessionStorage.getItem("token"), requestDelete).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Perfil eliminado con éxito", "Aceptar");
                        this.Busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("EliminarPerfil");
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
                sessionStorage.setItem("company", data.userData.userData.company.toString());
                sessionStorage.setItem("isType", data.userData.userData.isType.toString());

                switch (peticion) {
                    case "Busqueda":
                        this.Busqueda();
                        break;
                    case "BusquedaCombos":
                        this.BusquedaCombos();
                        break;
                    case "EliminarPerfil":
                        this.EliminarPerfil(2);
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
        this.haveCompany = parseInt(sessionStorage.getItem("company"));

    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }
}
