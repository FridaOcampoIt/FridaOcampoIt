import { Component, OnInit } from '@angular/core';
import { DataServices } from '../Interfaces/Services/general.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { SearchDropDownConfigurationResponse, SearchConfigurationCompanyResponse, SearchConfigurationCompanyRequest, SaveGeneralConfigurationRequest, SaveGeneralConfiguration, SaveGeneralConfigurationResponse, SaveConfigurationCompanyRequest } from '../Interfaces/Models/ConfigurationModels';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { Location } from '@angular/common';
//import { request } from 'http';

@Component({
    selector: 'app-configuracion',
    templateUrl: './configuracion.component.html',
    styleUrls: ['./configuracion.component.less']
})
export class ConfiguracionComponent implements OnInit {

    constructor(
        private dataService: DataServices,
        private routelink: Router,
        private snack: MatSnackBar,
        private _overlay: OverlayService,
        private a:Location) { }

    response = new SearchDropDownConfigurationResponse();

    comentariosNotificarA: string[] = [];
    solicitudNotificarA: string[] = [];
    valueComentarios: string = "";
    valueSolicitud: string = "";

    compania: number = 0;
    NGuiasUso: number = 0;
    NGuiasInstalacion: number = 0;
    NProductoRelacionado: number = 0;
    NPDF: number = 0;
    NCharEspec: number = 0;
    NImagenes: number = 0;
    NVideos: number = 0;
    NCharFaq: number = 0;
    comentariosFabricante: string = "";
    garantiaRegistro: string = "";
    alertaRobo: string = "";
    overlayRef: OverlayRef;
    ruta;


    //Funcion para la busqueda de la configuracion general y los combos
    busquedaConfiguracion() {
        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);

        this.dataService.postData<SearchDropDownConfigurationResponse>("Configuration/searchDropDownConfiguration", sessionStorage.getItem("token")).subscribe(
            data => {
                this.response = data;

                this.response.configuration.companyTraceIT = data.configuration.companyTraceIT.filter(x => x.data != "");

                for (var i = 0; i < this.response.configuration.generalConfigurations.length; i++) {
                    if (this.response.configuration.generalConfigurations[i].configuration == "Notificar comentarios") {
                        this.comentariosNotificarA = this.response.configuration.generalConfigurations[i].value;
                    }
                    else if (this.response.configuration.generalConfigurations[i].configuration == "Notificar solicitudes") {
                        this.solicitudNotificarA = this.response.configuration.generalConfigurations[i].value;
                    }
                }
                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("busquedaConfiguracion");
                } else {
                    console.log(error);
                }
                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
            }
        )
    }

    //Funcion para la busqueda de la configuracion por compañia
    busquedaConfigurationCompania() {
        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);
        var requestConfigurationCompany = new SearchConfigurationCompanyRequest();
        requestConfigurationCompany.idCompany = this.compania;

        this.dataService.postData<SearchConfigurationCompanyResponse>("Configuration/searchConfigurationCompany", sessionStorage.getItem("token"), requestConfigurationCompany).subscribe(
            data => {
                if (data.messageEsp != "") {
                    console.log(data.messageEsp);
                    return;
                }

                this.NGuiasInstalacion = data.companyConfiguration.nInstalationGuides;
                this.NGuiasUso = data.companyConfiguration.nUseGuides;
                this.NProductoRelacionado = data.companyConfiguration.nRelatedProduct;
                this.comentariosFabricante = data.companyConfiguration.notifyComments;
                this.garantiaRegistro = data.companyConfiguration.notifyWarranty;
                this.alertaRobo = data.companyConfiguration.notifyStolen;
                this.NPDF = data.companyConfiguration.nPDF;
                this.NCharEspec = data.companyConfiguration.nCharEspec;
                this.NCharFaq = data.companyConfiguration.nCharFAQ;
                this.NImagenes = data.companyConfiguration.nImg;
                this.NVideos = data.companyConfiguration.nVid;


                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("busquedaConfigurationCompania");
                } else {
                    console.log(error);
                }

                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
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
                    this.routelink.navigate(['Login']);
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
                    case "busquedaConfiguracion":
                        this.busquedaConfiguracion();
                        break;
                    case "busquedaConfigurationCompania":
                        this.busquedaConfigurationCompania();
                        break;
                    case "guardarConfiguracionGeneral":
                        this.guardarConfiguracionGeneral();
                        break;
                    case "guardarConfiguracionCompania":
                        this.guardarConfiguracionCompania();
                        break;
                    default:
                        break;
                }
            },
            error => {
                sessionStorage.clear();
                this.routelink.navigate(['Login']);
                this.openSnack("Error al mandar la solicitud", "Aceptar");
                return;
            }
        )
    }

    //Funcion para guardar las configuraciones generales
    guardarConfiguracionGeneral() {
        if (this.solicitudNotificarA.length == 0 || this.comentariosNotificarA.length == 0) {
            this.openSnack("Captura los datos obligatorios de las configuraciones", "Aceptar");
            return;
        }

        var requestConfiguracionGral = new SaveGeneralConfigurationRequest();

        var configuracionGeneral = new SaveGeneralConfiguration();
        configuracionGeneral.configuration = "Notificar comentarios";
        for (var i = 0; i < this.comentariosNotificarA.length; i++) {
            if (i + 1 == this.comentariosNotificarA.length) {
                configuracionGeneral.value += this.comentariosNotificarA[i].toString();
            } else {
                configuracionGeneral.value += this.comentariosNotificarA[i].toString() + ","
            }
        }
        requestConfiguracionGral.generalConfiguration.push(configuracionGeneral);

        configuracionGeneral = new SaveGeneralConfiguration();
        configuracionGeneral.configuration = "Notificar solicitudes";
        for (var i = 0; i < this.solicitudNotificarA.length; i++) {
            if (i + 1 == this.solicitudNotificarA.length) {
                configuracionGeneral.value += this.solicitudNotificarA[i].toString();
            } else {
                configuracionGeneral.value += this.solicitudNotificarA[i].toString() + ","
            }
        }
        requestConfiguracionGral.generalConfiguration.push(configuracionGeneral);

        this.dataService.postData<SaveGeneralConfigurationResponse>("Configuration/saveGeneralConfiguration", sessionStorage.getItem("token"), requestConfiguracionGral).subscribe(
            data => {
                if (data.messageEsp != "") {
                    this.openSnack(data.messageEsp, "Aceptar");
                } else {
                    this.openSnack("Configuración guardado con éxito", "Aceptar");
                    this.busquedaConfiguracion();
                }
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("guardarConfiguracionGeneral");
                } else {
                    this.openSnack("Error al mandar la solicitud", "Aceptar");
                }
            }
        )
    }

    //Funcion para validar el tope del input de meses
    validarNumero(input: string) {
        switch (input) {
            case "GuiasUso":
                if (this.NGuiasUso < 0) {
                    this.NGuiasUso = 0;
                }

                if (this.NGuiasUso > 500) {
                    this.NGuiasUso = 500;
                }
                break;
            case "GuiasInstalacion":
                if (this.NGuiasInstalacion < 0) {
                    this.NGuiasInstalacion = 0;
                }

                if (this.NGuiasInstalacion > 500) {
                    this.NGuiasInstalacion = 500;
                }
                break;
            case "ProductoRelacionado":
                if (this.NProductoRelacionado < 0) {
                    this.NProductoRelacionado = 0;
                }

                if (this.NProductoRelacionado > 500) {
                    this.NProductoRelacionado = 500;
                }
                break;
            case "pdfLink":
                if (this.NPDF < 0) {
                    this.NPDF = 0;
                }

                if (this.NPDF > 500) {
                    this.NPDF = 500;
                }
                break;
            default:
                break;
        }
    }

    //Funcion para guardar la configuracion de una compañia
    guardarConfiguracionCompania() {
        if (this.compania < 0 || this.NGuiasUso < 0 || this.NGuiasInstalacion < 0 || this.NProductoRelacionado < 0 ||
            this.comentariosFabricante == "" || this.garantiaRegistro == "" || this.alertaRobo == "") {
            this.openSnack("Captura los datos obligatorios de las configuraciones", "Aceptar");
            return;
        }

        var requestConfiguracionCompania = new SaveConfigurationCompanyRequest();
        requestConfiguracionCompania.company = this.compania;
        requestConfiguracionCompania.nInstalationGuides = this.NGuiasInstalacion;
        requestConfiguracionCompania.notifyComments = this.comentariosFabricante;
        requestConfiguracionCompania.notifyStolen = this.alertaRobo;
        requestConfiguracionCompania.notifyWarranty = this.garantiaRegistro;
        requestConfiguracionCompania.nRelatedProduct = this.NProductoRelacionado;
        requestConfiguracionCompania.nUseGuides = this.NGuiasUso;
        requestConfiguracionCompania.nPDF = this.NPDF;
        requestConfiguracionCompania.nCharEspec = this.NCharEspec;
        requestConfiguracionCompania.nCharFAQ = this.NCharFaq;
        requestConfiguracionCompania.nImg = this.NImagenes;
        requestConfiguracionCompania.nVid = this.NVideos;

        this.dataService.postData<SaveGeneralConfigurationResponse>("Configuration/saveCongfigurationCompany", sessionStorage.getItem("token"), requestConfiguracionCompania).subscribe(
            data => {
                if (data.messageEsp != "") {
                    this.openSnack(data.messageEsp, "Aceptar");
                } else {
                    this.openSnack("Configuración guardado con éxito", "Aceptar");

                    this.compania = 0;
                    this.NGuiasUso = 0;
                    this.NGuiasInstalacion = 0;
                    this.NProductoRelacionado = 0;
                    this.NPDF = 0;
                    this.NCharFaq = 0;
                    this.NCharEspec = 0;
                    this.NImagenes = 0;
                    this.comentariosFabricante = "";
                    this.garantiaRegistro = "";
                    this.alertaRobo = "";
                    this.NVideos = 0;
                }
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("guardarConfiguracionCompania");
                } else {
                    this.openSnack("Error al mandar la solicitud", "Aceptar");
                }
            }
        )
    }

    //Funcion para abrir el modal del mensaje
    openSnack = (message: string, action: string) => {
        this.snack.open(message, action, {
            duration: 3000
        })
    }

    ngOnInit() {
        this.busquedaConfiguracion();
        this.ruta=this.routelink.url;
        
   console.log(this.ruta)
   if (this.ruta=="/Configuracion/ConfiguracionGeneral/Sector") {
      console.log("es sector")
   } else if(this.ruta=="/Configuracion/ConfiguracionGeneral/Sector/Formulario") {
      console.log("es formulario")
   }
        
    }
    goBack(){
        this.a.back();
        this.ruta=this.routelink.url;
    }
}
