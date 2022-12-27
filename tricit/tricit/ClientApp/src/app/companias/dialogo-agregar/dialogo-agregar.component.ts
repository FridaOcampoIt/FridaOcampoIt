import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';

//DataServices
import {
    SearchCompanyDataRequest,
    SearchCompanyDataResponse,
    SaveCompanyRequest,
    CompanyProcessResponse,
    EditCompanyRequest
} from '../../Interfaces/Models/CompanyModels'

import { DataServices } from '../../Interfaces/Services/general.service'
import { SearchCountriesResponse } from '../../Interfaces/Models/TraceITBaseModels';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';
//import { request } from 'http';

@Component({
    selector: 'app-dialogo-agregar',
    templateUrl: './dialogo-agregar.component.html',
    styleUrls: ['./dialogo-agregar.component.less']
})

export class DialogoAgregarComponent implements OnInit {

    //Constructor del componente
    constructor(
        @Inject(MAT_DIALOG_DATA)
        private _data: any,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private dialogRef: MatDialogRef<DialogoAgregarComponent>,
        private _router: Router) { }

    //Variables que seran utilizadas en la vista agregar
    title: string;
    dataSelect = [];
    subiendo: boolean = false;

    arrgiro:Array<any>=[ 
        {
        id:1000022,
        value:"Reproceso"
        },
        
        {
        id:1000023,
        value:"No Reproceso"
        },
        {
        id:1000024,
        value:"Hìbrido"   
        }
    ]
        
    
    

    //Variables enviados por el WS 
    isDefault: boolean = false;
    responseCountries = new SearchCountriesResponse();
    response = new SearchCompanyDataResponse();

    validURL(str) {
        var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
            '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
            '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
            '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
            '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
            '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locator
        return !!pattern.test(str);
    }

    //Funcion para cargar los paises en el combo
    BusquedaPaises() {
        this.dataService.postData<SearchCountriesResponse>("Families/searchCountries", sessionStorage.getItem("token")).subscribe(
            data => {
                this.responseCountries = data;
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("BusquedaPaises");
                } else {
                    console.log(error);
                }
            }
        );
    }

    //Funcion para cargar los datos en el formulario en caso que se tratara de una edicion
    ngOnInit() {
        //Se resiven las variables enviadas desde el componente que lo mando llamar
        this.title = this._data.title;

        if (this._data.id == 0)
            this.BusquedaPaises();
        else {
            var request = new SearchCompanyDataRequest();
            request.idCompany = this._data.id;

            this.dataService.postData<SearchCompanyDataResponse>("Companies/searchCompanyData", sessionStorage.getItem("token"), request).subscribe(
                data => {
                    this.response = data;
                    this.BusquedaPaises();

                    //Validacion de los contactos
                    if (this.response.contactData.defaultFirst) {
                        this.isDefault = false;
                    } else if (this.response.contactData.defaultSecond) {
                        this.isDefault = true;
                    }
                },
                error => {
                    console.log(error);
                }
            );
        }
    }
    
    //Funcion para guardar los datos de la compañia
    SaveCompany() {
        //Se valida si se quiere realizar una edicion o un agregar
        if (this.response.companyData.idCompany != 0) {
            //Validacion de los datos generales de las compañias
            if (this.response.companyData.address.trim() == "" || this.response.companyData.businessName.trim() == "" ||
                this.response.companyData.country.trim() == "" || this.response.companyData.email.trim() == "" ||
                this.response.companyData.name.trim() == "" || this.response.companyData.phone.trim() == "" ||
                this.response.companyData.webSite.trim() == "") {
                this.openSnack("Captura los datos generales de la compañía", "Aceptar");
                return;
            }
            console.log("equisde");
            //Validacion de los datos de los contactos de las compañias
            if (this.response.contactData.contactEmailFirst == null || this.response.contactData.contactEmailFirst.trim() == "" ||
                this.response.contactData.contactNameFirst == null || this.response.contactData.contactNameFirst.trim() == "" ||
                this.response.contactData.contactPhoneFirst == null || this.response.contactData.contactPhoneFirst.trim() == "") {
                this.openSnack("Captura los datos de contacto 1 de la compañía", "Aceptar");
                return;
            }

            if (this.isDefault) {
                if (this.response.contactData.contactEmailSecond == null || this.response.contactData.contactEmailSecond.trim() == "" ||
                    this.response.contactData.contactNameSecond == null || this.response.contactData.contactNameSecond.trim() == "" ||
                    this.response.contactData.contactPhoneSecond == null || this.response.contactData.contactPhoneSecond.trim() == "") {
                    this.openSnack("Captura los datos de contacto 2 de la compañía", "Aceptar");
                    return;
                }
            }

            if (this.response.companyData.facebook.length > 0 && !this.validURL(this.response.companyData.facebook)) {
                this.openSnack("El enlace de Facebook no es válido", "Aceptar");
                return;
            }
            if (this.response.companyData.youtube.length > 0 && !this.validURL(this.response.companyData.youtube)) {
                this.openSnack("El enlace de Youtube no es válido", "Aceptar");
                return;
            } if (this.response.companyData.linkedin.length > 0 && !this.validURL(this.response.companyData.linkedin)) {
                this.openSnack("El enlace de LinkedIn no es válido", "Aceptar");
                return;
            }

            
           
            var requestUpdate = new EditCompanyRequest();
            requestUpdate.idCompany = this.response.companyData.idCompany;
            requestUpdate.name = this.response.companyData.name;
            requestUpdate.businessName = this.response.companyData.businessName;
            requestUpdate.email = this.response.companyData.email;
            requestUpdate.webSite = this.response.companyData.webSite;
            requestUpdate.phone = this.response.companyData.phone;
            requestUpdate.country = this.response.companyData.country;
            requestUpdate.address = this.response.companyData.address;
            requestUpdate.status = this.response.companyData.status;
            requestUpdate.facebook = this.response.companyData.facebook;
            requestUpdate.youtube = this.response.companyData.youtube;
            requestUpdate.linkedin = this.response.companyData.linkedin;
            requestUpdate.clientNumber = this.response.companyData.clientNumber;
            requestUpdate.tipoGiro= this.response.companyData.tipoGiro;
            

            requestUpdate.contactCompanies.idContactFirst = this.response.contactData.idContactFirst;
            requestUpdate.contactCompanies.contactNameFirst = this.response.contactData.contactNameFirst;
            requestUpdate.contactCompanies.contactPhoneFirst = this.response.contactData.contactPhoneFirst;
            requestUpdate.contactCompanies.contactEmailFirst = this.response.contactData.contactEmailFirst;
            requestUpdate.contactCompanies.defaultFirst = !this.isDefault ? true : false;
            requestUpdate.contactCompanies.idContactSecond = this.response.contactData.idContactSecond;
            requestUpdate.contactCompanies.contactNameSecond = this.response.contactData.contactNameSecond;
            requestUpdate.contactCompanies.contactPhoneSecond = this.response.contactData.contactPhoneSecond;
            requestUpdate.contactCompanies.contactEmailSecond = this.response.contactData.contactEmailSecond;
            requestUpdate.contactCompanies.defaultSecond = this.isDefault ? true : false;
            
            this.subiendo = true;
            this.dataService.postData<CompanyProcessResponse>("Companies/editCompany", sessionStorage.getItem("token"), requestUpdate).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Compañía editada con éxito", "Aceptar");
                        this.dialogRef.close(true);
                    }
                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("SaveCompany");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendo = false;
                    }
                }
            );

        } else {
            //Validacion de los datos generales de las compañias
            if (this.response.companyData.address.trim() == "" || this.response.companyData.businessName.trim() == "" ||
                this.response.companyData.country.trim() == "" || this.response.companyData.email.trim() == "" ||
                this.response.companyData.name.trim() == "" || this.response.companyData.phone.trim() == "" ||
                this.response.companyData.webSite.trim() == "") {
                this.openSnack("Captura los datos generales de la compañía", "Aceptar");
                return;
            }

            //Validacion de los datos de los contactos de las compañias
            if (this.response.contactData.contactEmailFirst == null || this.response.contactData.contactEmailFirst.trim() == "" ||
                this.response.contactData.contactNameFirst == null || this.response.contactData.contactNameFirst.trim() == "" ||
                this.response.contactData.contactPhoneFirst == null || this.response.contactData.contactPhoneFirst.trim() == "") {
                this.openSnack("Captura los datos de contacto 1 de la compañía", "Aceptar");
                return;
            }

            if (this.isDefault) {
                if (this.response.contactData.contactEmailSecond == null || this.response.contactData.contactEmailSecond.trim() == "" ||
                    this.response.contactData.contactNameSecond == null || this.response.contactData.contactNameSecond.trim() == "" ||
                    this.response.contactData.contactPhoneSecond == null || this.response.contactData.contactPhoneSecond.trim() == "") {
                    this.openSnack("Captura los datos de contacto 2 de la compañía", "Aceptar");
                    return;
                }
            }
            
            var requestSave = new SaveCompanyRequest();
            requestSave.name = this.response.companyData.name;
            requestSave.businessName = this.response.companyData.businessName;
            requestSave.email = this.response.companyData.email;
            requestSave.webSite = this.response.companyData.webSite;
            requestSave.phone = this.response.companyData.phone;
            requestSave.country = this.response.companyData.country;
            requestSave.address = this.response.companyData.address;
            requestSave.status = this.response.companyData.status;
            requestSave.facebook = this.response.companyData.facebook;
            requestSave.youtube = this.response.companyData.youtube;
            requestSave.linkedin = this.response.companyData.linkedin;
            requestSave.clientNumber = this.response.companyData.clientNumber;
            requestSave.tipoGiro= this.response.companyData.tipoGiro;
            


            requestSave.contactCompanies.contactNameFirst = this.response.contactData.contactNameFirst;
            requestSave.contactCompanies.contactPhoneFirst = this.response.contactData.contactPhoneFirst;
            requestSave.contactCompanies.contactEmailFirst = this.response.contactData.contactEmailFirst;
            requestSave.contactCompanies.defaultFirst = !this.isDefault ? true : false;
            requestSave.contactCompanies.contactNameSecond = this.response.contactData.contactNameSecond;
            requestSave.contactCompanies.contactPhoneSecond = this.response.contactData.contactPhoneSecond;
            requestSave.contactCompanies.contactEmailSecond = this.response.contactData.contactEmailSecond;
            requestSave.contactCompanies.defaultSecond = this.isDefault ? true : false;

            this.subiendo = true;

            this.dataService.postData<CompanyProcessResponse>("Companies/saveCompany", sessionStorage.getItem("token"), requestSave).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Compañía guardada con éxito", "Aceptar");
                        this.dialogRef.close(true);
                    }
                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("SaveCompany");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendo = false;
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
                    case "SaveCompany":
                        this.SaveCompany();
                        break;
                    case "BusquedaPaises":
                        this.BusquedaPaises();
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
}
