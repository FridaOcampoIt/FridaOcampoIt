import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import * as CryptoJS from 'crypto-js';

import { DataServices } from '../../Interfaces/Services/general.service';

import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { searchAcopioResponse } from '../../Interfaces/Models/AcopioModels';
import {
    SearchUserDropDownRequest,
    SearchUserDropDownResponse,
    SearchUserDataResponse,
    SearchUserDataRequest,
    SaveUserRequest,
    UserProcessResponse,
    EditUserRequest
} from '../../Interfaces/Models/UserModels';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';
import { exit } from 'process';

@Component({
    selector: 'app-dialogo-agregar',
    templateUrl: './dialogo-agregar.component.html',
    styleUrls: ['./dialogo-agregar.component.less']
})
export class DialogoAgregarComponent implements OnInit {
    constructor(
        @Inject(MAT_DIALOG_DATA) private _data: any,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private dialogRef: MatDialogRef<DialogoAgregarComponent>,
        private _router: Router,
        private _overlay: OverlayService,
    ){ }

    //Variables para la vista
    title: string = "";
    idUser: number = 0;
    password: string = "";
    visible: boolean = false;
    optionCombos: number = 0;
    response = new SearchUserDataResponse();
    responseDropDown = new SearchUserDropDownResponse();

    //Funcion para la busqueda combos para el modulo
    BusquedaCombos(option) {
        var requestCombos = new SearchUserDropDownRequest();
        requestCombos.option = option;
        requestCombos.user = this.idUser;
        this.optionCombos = option;

        if (option == 2)
            requestCombos.company = this.response.dataUser.company;

        this.dataService.postData<SearchUserDropDownResponse>("User/searchUserDropDown", sessionStorage.getItem("token"), requestCombos).subscribe(
            data => {
                if (option == 1 || option == 3) {
                    this.responseDropDown = data;
                    this.responseDropDown.dropDown.companies = this.responseDropDown.dropDown.companies.filter(x => x.data != "");
                    let company: number = parseInt(sessionStorage.getItem("company"));

                    if (company != 0) {
                        this.responseDropDown.dropDown.companies = this.responseDropDown.dropDown.companies.filter(x => x.id == company);
                    }

                }
                else {
                    this.responseDropDown.dropDown.profiles = data.dropDown.profiles;
                    this.response.dataUser.profile = 0;
                }

                if (this.idUser != 0 && option == 3)
                    this.BusquedaDatos();
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

    //Funcion para consultar los datos del usuario
    BusquedaDatos() {
        let auxLoading: any;
        setTimeout(() => {
            auxLoading = this._overlay.open();
        }, 1000);
        var requestData = new SearchUserDataRequest();
        requestData.idUser = this.idUser;

        this.dataService.postData<any>("User/searchUserData", sessionStorage.getItem("token"), requestData).subscribe(
            data => {
                if(data.dataUser['acopiosIds']){
                    let arraytemporal =  data.dataUser['acopiosIds'].split(',');
                    //Convertimos el string a enteros (porque yolo, no funciona con string)
                    let arrayNumeros = arraytemporal.map(str => {
                    return Number(str);
                    });
                    data.dataUser['acopiosIds'] = arrayNumeros;
                }
                this.response.dataUser = data.dataUser;
                this.response.dataUser.auxAcopiosIds = this.response.dataUser.acopiosIds.length > 0 ? this.response.dataUser.acopiosIds : null; 
                console.log('this.response', this.response.dataUser);
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1000);
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("BusquedaDatos");
                } else {
                    console.log(error);
                }
                
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1000);
            }
        )
    }

    //Funcion para guardar los datos del usuario
    SaveUser() {
        if(this.isCompany > 0){
            this.response.dataUser.company = this.isCompany;
        }
        console.log('Response', this.response, this.isCompany, (this.isCompany > 0  ? 'aqui' : 
        true) ,(this.response.dataUser.email == ""));
        if (this.idUser == 0) {
            //Validación para nuevos usuarios.
            if (this.response.dataUser.email == "" || this.response.dataUser.lastName ==  "" || this.response.dataUser.name == "" 
            || this.response.dataUser.position == "" || this.response.dataUser.profile == 0 || this.response.dataUser.rol == 0 
            || (this.isCompany > 0  ? (this.response.dataUser.company == 0 || this.response.dataUser.acopiosIds.length <=  0) : 
               (this.response.dataUser.rol == 1 && this.response.dataUser.company != 0)) ) {
                return this.openSnack("Captura los datos obligatorios del usuario", "Aceptar");
            }
        }
        else {
            //Validación para editar usuarios resgistrados.
            if (this.response.dataUser.email == "" || this.response.dataUser.lastName ==  "" || this.response.dataUser.name == "" 
            || this.response.dataUser.position == "" || this.response.dataUser.profile == 0 || this.response.dataUser.rol == 0 
            || (this.isCompany > 0  ? (this.response.dataUser.company == 0 || this.response.dataUser.acopiosIds.length <=  0) : 
               (this.response.dataUser.rol == 1 && this.response.dataUser.company != 0)) ){
                return this.openSnack("Captura los datos obligatorios del usuario", "Aceptar");
                ;
            }
        }
        console.log('Response', this.response);
        let auxLoading: any;
        setTimeout(() => {
          auxLoading = this._overlay.open();
         }, 1000);
        if (this.idUser == 0) {
            var requestSave = new SaveUserRequest();
            requestSave.companyId = this.response.dataUser.company;
            requestSave.companyId = this.isType == 0 && this.isCompany == 0 ? this.response.dataUser.company : this.isCompany;
            requestSave.email = this.response.dataUser.email;
            requestSave.lastName = this.response.dataUser.lastName;
            requestSave.name = this.response.dataUser.name;
            requestSave.password = CryptoJS.SHA256(this.password).toString();
            requestSave.position = this.response.dataUser.position;
            requestSave.profile = this.response.dataUser.profile;
            requestSave.rolId = this.response.dataUser.rol;
            requestSave.acopiosIds = this.isType == 0 && this.isCompany >  0 ? this.response.dataUser.acopiosIds : null;
            this.dataService.postData<UserProcessResponse>("User/saveUser", sessionStorage.getItem("token"), requestSave).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Usuario guardado con éxito", "Aceptar");
                        this.dialogRef.close(true);
                    }
                    setTimeout(() => {
                        this._overlay.close(auxLoading);
                    },1000);
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("SaveUser");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                    
                    setTimeout(() => {
                        this._overlay.close(auxLoading);
                    },1000);
                }
            )
        } else {
            var requestUpdate = new EditUserRequest();
            requestUpdate.companyId = this.isType == 0 && this.isCompany == 0 ? this.response.dataUser.company : this.isCompany;
            requestUpdate.email = this.response.dataUser.email;
            requestUpdate.idUser = this.idUser;
            requestUpdate.lastName = this.response.dataUser.lastName;
            requestUpdate.name = this.response.dataUser.name;
            requestUpdate.password = this.password != "" ? CryptoJS.SHA256(this.password).toString() : this.password;
            requestUpdate.position = this.response.dataUser.position;
            requestUpdate.profile = this.response.dataUser.profile;
            requestUpdate.rolId = this.response.dataUser.rol;
            requestUpdate.acopiosIds = this.isType == 0 && this.isCompany >  0 ? this.response.dataUser.acopiosIds : null;
            requestUpdate.auxAcopiosIds = this.isType == 0 && this.isCompany > 0 ? this.response.dataUser.auxAcopiosIds : null;

            this.dataService.postData<UserProcessResponse>("User/editUser", sessionStorage.getItem("token"), requestUpdate).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Usuario guardado con éxito", "Aceptar");
                        this.dialogRef.close(true);
                    }
                    setTimeout(() => {
                        this._overlay.close(auxLoading);
                    },1000);
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("SaveUser");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                    
                    setTimeout(() => {
                        this._overlay.close(auxLoading);
                    },1000);
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
                sessionStorage.setItem("company", data.userData.userData.company.toString());
                sessionStorage.setItem("isType", data.userData.userData.isType.toString());

                switch (peticion) {
                    case "SaveUser":
                        this.SaveUser();
                        break;
                    case "BusquedaCombos":
                        this.BusquedaCombos(this.optionCombos);
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

    //Funcion para mostrar u ocultar la contraseña
    mostrarPassword(input) {
        if (input.type == "password") {
            input.type = "text";
        } else {
            input.type = "password";
        }
    }

    //Funcion para abrir el modal del mensaje
    openSnack = (message: string, action: string) => {
        this.snack.open(message, action, {
            duration: 5000
        })
    }

    isCompany: number = 0;
    isType: number = 0;
    ngOnInit() {
        this.isCompany = parseInt(sessionStorage.getItem("company"), 10);
        this.isType = parseInt(sessionStorage.getItem("isType"), 10);
        if(this.isCompany > 0){
            this.getListAcopio();
        }
        this.title = this._data.title;
        this.idUser = this._data.id;

        if (this.idUser == 0)
            this.BusquedaCombos(1);
        else {
            this.visible = true;
            this.BusquedaCombos(3);
        }
    }

    //Funciones para acopios 
    listAcopio : Array<searchAcopioResponse> = [];
    getListAcopio() { 
        let data: any = {
            companiaId: this.isCompany,
            nombreNumeroAcopio: ''
        };
        let auxLoading: any;
        setTimeout(() => {
          auxLoading = this._overlay.open();
         }, 1);
        this.dataService.postData<searchAcopioResponse>("Acopio/searchListAcopios", sessionStorage.getItem("token"), data).subscribe(
            data => {
                //console.log('Rsponse data', data);
                if(data["messageEsp"]  != null){
                    this.openSnack(data["messageEsp"],"Aceptar");
                }
                else if (data["searchListAcopio"]) {
                    //console.log('Data', data['searchListAcopio']);
                    this.listAcopio = data['searchListAcopio'];
                }
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1000);
            },error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("1");
                } else {
                    //console.log(error);
                }
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1000);
            }
        );
    }
    

    
    onChange(event) {
        //Tenemos que hacer todo esto, por falta de estandar de formularios reactivos. 
        //El back, esta preparado, para recibir un array de acopios, para usuarios multiacopios. Bye y suerte programador nuevo 
        // Quizas sea dificil este proyecto.
        console.log(event);
        this.response.dataUser.acopiosIds = [event.acopioId];
    }
}
