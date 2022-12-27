import { Component, OnInit, AfterViewInit, Inject } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';

import { DataTreeProfile, FlatNodeProfile } from '../../Interfaces/Enviroments/dataClassSource';
import { DataServices } from '../../Interfaces/Services/general.service';
import {
    SearchDropDownPermissionRequest,
    SearchDropDownPermissionResponse,
    SearchProfileDataRequest,
    SearchProfileDataResponse,
    SaveProfileRequest,
    SaveUpdateProccess,
    EditProfileRequest
} from '../../Interfaces/Models/ProfilesModels';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';

@Component({
    selector: 'app-dialogo-agregar',
    templateUrl: './dialogo-agregar.component.html',
    styleUrls: ['./dialogo-agregar.component.less']
})
export class DialogoAgregarComponent implements OnInit, AfterViewInit {

    constructor(
        @Inject(MAT_DIALOG_DATA) private _data: any,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private dialogRef: MatDialogRef<DialogoAgregarComponent>,
        private _router: Router)
    {
        this.dataSource.data = this.dataTree;
    }

    //Variables para la vista 
    title: string = "";
    idProfile: number = 0;
    idCompany: number = 0;
    nameProfile: string = "";
    permission: number[] = [];

    responseDropDown = new SearchDropDownPermissionResponse();
    companies : any[] = [];

    dataTree: DataTreeProfile[] = [];
    private _transformer = (node: DataTreeProfile, level: number) => {
        return {
            expandable: !!node.children && node.children.length > 0,
            name: node.name,
            level: level,
            parent: node.parent,
            id: node.id,
            idParent: node.idParent,
			check: node.check,
			isForCompany: node.isForCompany
        };
    }

    treeControl = new FlatTreeControl<FlatNodeProfile>(node => node.level, node => node.expandable);
    treeFlattener = new MatTreeFlattener(this._transformer, node => node.level, node => node.expandable, node => node.children);
    dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);
    hasChild = (_: number, node: FlatNodeProfile) => node.expandable;
    isParent = (_: number, node: FlatNodeProfile) => node.parent;

    //Funcion para la busqueda de los datos del perfil
    BusquedaDatos() {
        var requestDatos = new SearchProfileDataRequest();
        requestDatos.profileId = this.idProfile;

        this.dataService.postData<SearchProfileDataResponse>("Profiler/searchProfileData", sessionStorage.getItem("token"), requestDatos).subscribe(
            data => {
                //Se setea los datos del perfil
                this.nameProfile = data.permissionProfileData.profileData.name;
                this.idCompany = data.permissionProfileData.profileData.company;

                data.permissionProfileData.permissions.forEach((it, id) => {
                    this.permission.push(it.permission);

                    //Se recorre el datatree para ver que permisos se checkearan
                    this.dataTree.forEach((it2, id2) => {
                        //Se valida que el id del permiso no sea igula al id del padre
                        if (it2.id === it.permission)
                            return;
                        else {
                            //Se valida que tenga un permiso hijo con el mismo id
                            if (it2.children.filter(map => map.id == it.permission).length > 0) {
                                it2.children.forEach((it3, id3) => {
                                    if (it3.id == it.permission) {
                                        it3.check = true;
                                        return;
                                    }
                                });
                            } else {
                                return;
                            }
                        }
                    });
				});


				this.dataSource.data = this.dataTree;
				console.log("tree with data", this.dataTree);
				console.log("the sourceData", this.dataSource);
                this.treeControl.expandAll();
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("BusquedaDatos");
                } else {
                    console.log(error);
                }
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
                this.dataTree = [];
                console.log("equisde");
                this.companies = this.responseDropDown.dropDownProfilesPermission.companyList.filter(x => x.data.length);

                this.responseDropDown.dropDownProfilesPermission.permissions.forEach((it, id) => {
                    var dateTree = new DataTreeProfile();
                    dateTree.id = it.idPermission;
                    dateTree.name = it.name;
                    dateTree.parent = true;

                    it.permission.forEach((it2, id2) => {
                        dateTree.children.push({
                            check: false,
                            id: it2.idPermission,
                            idParent: it.idPermission,
                            name: it2.name,
							parent: false,
							isForCompany: it2.isForCompany
                        })
                    });

                    this.dataTree.push(dateTree);
				});
				this.dataSource.data = this.dataTree;

				console.log("empty tree", this.dataTree);
				console.log("source original", this.dataSource);

                this.treeControl.expandAll();

                if(this.idProfile != 0)
                    this.BusquedaDatos();
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

    //Funcion para agregar o quitar los permisos
	AgregarQuitarPermiso(idParent: number, idPermission: number, check: any, node: any) {
		console.log(node);
        //Se valida si se quiere agregar
        if (check.checked) {
            //Se valida si el permiso padre ya esta en la lista de permisos
            if (this.permission.includes(idParent)) {
                this.permission.push(idPermission)

                this.dataTree.forEach((it, id) => {
                    if (it.id == idParent) {
                        for (var i = 0; i < it.children.length; i++) {
                            if (it.children[i].id == idPermission) {
                                it.children[i].check = true;
                                break;
                            }
                        }
                    }
                });
            }
            //De lo contrario se agrega 
            else {
                this.permission.push(idParent);
                this.permission.push(idPermission);

                this.responseDropDown.dropDownProfilesPermission.permissions.forEach((it, id) => {
                    if (it.idPermission == idParent) {
                        for (var i = 0; i < it.permission.length; i++) {
                            if (it.permission[i].idPermission == idPermission) {
                                it.permission[i].check = true;
                                break;
                            }
                        }
                    }
                });
            }
        }
        //De lo contrario se elimina
        else {
            this.dataTree.forEach((it, id) => {
                //Se valida que sea el padre
                if (it.id == idParent) {
                    //Se valida que haya mas de un permiso que esta agregado que tenga relacion con el padre
                    if (it.children.filter(map => map.check == true).length > 1) {

                        //Se valida para poner el check en false de los permisos
                        for (var i = 0; i < it.children.length; i++) {
                            //Se valida que sea el permiso
                            if (it.children[i].id == idPermission) {
                                it.children[i].check = false;
                                break;
                            }
                        }

                        //Se valida para eliminar el permiso hijo
                        for (var i = 0; i < this.permission.length; i++) {
                            if (this.permission[i] == idPermission) {
                                this.permission.splice(i, 1);
                                break;
                            }
                        }
                    } else {
                        //Se valida para poner el check en false de los permisos
                        for (var i = 0; i < it.children.length; i++) {
                            //Se valida que sea el permiso
                            if (it.children[i].id == idPermission) {
                                it.children[i].check = false;
                                break;
                            }
                        }

                        //Se valida para eliminar el permiso hijo y el padre
                        for (var i = 0; i < this.permission.length; i++) {
                            if (this.permission[i] == idPermission) {
                                this.permission.splice(i, 1);
                                break;
                            }
                        }

                        for (var i = 0; i < this.permission.length; i++) {
                            if (this.permission[i] == idParent) {
                                this.permission.splice(i, 1);
                                break;
                            }
                        }
                    }
                }
            });
        }        
    }

    //Función para guardar o editar el perfil
    SaveProfiles() {
        if (this.nameProfile == "") {
            this.openSnack("Captura el nombre del perfil", "Aceptar");
            return;
        }

        if (this.permission.length == 0 || this.permission == null) {
            this.openSnack("Selecciona los permisos del perfil", "Aceptar");
            return;
        }

        if (this.idProfile == 0) {
            var requestSave = new SaveProfileRequest();
            requestSave.company = this.idCompany;
            requestSave.name = this.nameProfile;
            requestSave.permission = this.permission;

            this.dataService.postData<SaveUpdateProccess>("Profiler/saveProfile", sessionStorage.getItem("token"), requestSave).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Perfil guardado con éxito", "Aceptar");
                        this.dialogRef.close(true);
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("SaveProfiles");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            );
        } else {
            var requestUpdate = new EditProfileRequest();
            requestUpdate.company = this.idCompany;
            requestUpdate.name = this.nameProfile;
            requestUpdate.permission = this.permission;
            requestUpdate.profileId = this.idProfile;

            this.dataService.postData<SaveUpdateProccess>("Profiler/editProfile", sessionStorage.getItem("token"), requestUpdate).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Perfil guardado con éxito", "Aceptar");
                        this.dialogRef.close(true);
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("SaveProfiles");
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
                    case "SaveProfiles":
                        this.SaveProfiles();
                        break;
                    case "BusquedaCombos":
                        this.BusquedaCombos();
                        break;
                    case "BusquedaDatos":
                        this.BusquedaDatos();
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
        this.title = this._data.title;
        this.idProfile = this._data.id;
        
        this.BusquedaCombos();
    }

    ngAfterViewInit() {
        this.treeControl.expandAll();
    }
}
