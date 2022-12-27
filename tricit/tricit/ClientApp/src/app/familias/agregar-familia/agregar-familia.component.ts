import { Component, OnInit , ViewContainerRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { Location } from '@angular/common';

import { DataServices } from '../../Interfaces/Services/general.service';
import {
    SearchFamilyProductDateRequest,
    SearchFamilyProductDateResponse,
    SearchDropDownListFamilyResponse,
    SearchDropDownListFamilyRequest,
    SaveFamilyRequest,
    UpdateFamilyRequest,
    FamilyProcessResponse
} from '../../Interfaces/Models/FamilyModels';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

import { NgxMatColorPickerInput, Color } from '@angular-material-components/color-picker';
@Component({
    selector: 'app-agregar-familia',
    templateUrl: './agregar-familia.component.html',
    styleUrls: ['./agregar-familia.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class AgregarFamiliaComponent implements OnInit {
    banderaImagen: boolean = false;
    idFamily: number = 0;
    companyId: number = 0;
    response = new SearchFamilyProductDateResponse();
    servicesCenters: number[] = [];
    dropDown = new SearchDropDownListFamilyResponse();
    subiendo: boolean = false;
    banderaSelectDrop: boolean;
    @ViewChild(NgxMatColorPickerInput) pickerInput: NgxMatColorPickerInput;
    colorFamilia: Color;
    constructor(
        private route: ActivatedRoute,
        private routelink: Router,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private _location: Location
    ) {
        let id = this.route.snapshot.paramMap.get('id');
        let companiId = this.route.snapshot.paramMap.get('companyId');
        if (id != undefined || id != null)
            this.idFamily = parseInt(id, 10);
        if(!!companiId)
            this.companyId = parseInt(companiId, 10);
    }

    company: number = (sessionStorage.getItem("company") === null ? -1 : parseInt(sessionStorage.getItem("company")));
    allowAutoLote: boolean = sessionStorage.hasOwnProperty('Visualizar Manejar Lote Automático');
    allowEditLote: boolean = sessionStorage.hasOwnProperty('Visualizar Manejar Lote Manual');
    allowPrefix: boolean = sessionStorage.hasOwnProperty('Visualizar Prefijo');
    allowConsecutivoLote : boolean = sessionStorage.hasOwnProperty('Visualizar Consecutivo Lote');

    //Funcion para la imagen de familia
    ImagenFamilia(files) {
        var archivos = files[0];
        this.getBase64(archivos).then(
            data => {
                if (this.idFamily != 0)
                    this.banderaImagen = true;

                this.response.productFamily.productFamilyData.image = data.toString();
            }
        );
    }

    //Funcion para transformar la imagen en base 64
    getBase64(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        });
    }

    //Funcion para la busqueda de los datos de la familia en caso de realizar una edicion 
    BusquedaDatos() {
        var request = new SearchFamilyProductDateRequest();
        request.familyId = this.idFamily;

        this.dataService.postData<SearchFamilyProductDateResponse>("Families/searchFamilyProductDate", sessionStorage.getItem("token"), request).subscribe(
            data => {
                
                this.response = data;
                const temp = this.hexToRgb(this.response.productFamily.productFamilyData.colorFamilia);
                this.colorFamilia = new Color(temp.r, temp.g, temp.b);
                this.response.productFamily.directionFamily.forEach((it, id) => {
                    this.servicesCenters.push(it.directionId);
                });

                this.BusquedaCombos();
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("BusquedaDatos");
                } else {
                    console.log(error);
                }
            }
        )
        //this.Prueba();
        //this.Prueba2();
    }

    // esta es una prueba para cuando alguien hace un escaneo de codigo de barras en la app
    Prueba() {
        var request2 = { "IdUser": 0, "BarCode": "a1b2c3d4e5", "Latitude": 0.0, "Longitude": 0.0 };
        console.log('prueba');
        this.dataService.postData<SearchFamilyProductDateResponse>("Families/SearchFamily", sessionStorage.getItem("token"), request2).subscribe(
            data => {
                console.log(data);
            },
            error => {
                console.log(error);
            }
        );
    }
    // esta es una prueba para para agregar garantia en la app
    Prueba2() {
        var request2 = {
            "Id": 0,
            "Folio": null,
            "DateBuy": "2019-07-30T00:00:00-05:00",
            "PlaceBuy": "Walmart",
            "PhotoTicket": "data:image/png;base64,R0lGODdhBQAFAIACAAAAAP/eACwAAAAABQAFAAACCIwPkWerClIBADs=",
            "DaysNotification": 7,
            "Expiration": "2019-08-26T00:00:00-06:00",
            "PeriodMonth": 3,
            "SerialNumber": "1154",
            "RegisterName": "Martin",
            "LastNameRegister": "Valenzuela",
            "EmailRegister": "jaaler_exdream@hotmail.com",
            "Age": "32",    //se maneja como string
            "Gender": "Masculino",
            "Country": "Mexico",
            "City": "GDL",
            "WarrantyId": 19,
            "UserMobileId": 42
        };
        console.log('prueba de garantia');
        console.log('token de la aplicacion:  ' + sessionStorage.getItem("token"));
        this.dataService.postData<SearchFamilyProductDateResponse>("UserMobile/saveWarrantyUser", sessionStorage.getItem("token"), request2).subscribe(
            data => {
                console.log(data);
            },
            error => {
                console.log(error);
            }
        );
    }
    //Funcion para traer los datos de los combos
    BusquedaCombos() {
        var request = new SearchDropDownListFamilyRequest();
		request.option = 1;
		request.idCompany = parseInt( sessionStorage.getItem("company"));

        this.dataService.postData<SearchDropDownListFamilyResponse>("Families/searchDropDownListFamily", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.dropDown = data;
                //console.log('dropdown:----- ' + this.dropDown);
                if (this.idFamily != 0)
                    this.filtrarCombo(false);

                this.dropDown.companyData = this.dropDown.companyData.filter(x => x.data != "");
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

    //Funcion para traer los centros de servicio filtrados por compañia
    filtrarCombo(banderaSelect) {
        var request = new SearchDropDownListFamilyRequest();
        request.idCompany = this.response.productFamily.productFamilyData.company;
        request.option = 2;
        this.banderaSelectDrop = banderaSelect;

        this.dataService.postData<SearchDropDownListFamilyResponse>("Families/searchDropDownListFamily", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.dropDown.addressData = data.addressData;

                if (banderaSelect) {
                    this.response.productFamily.directionFamily = [];
                }
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("filtrarCombo");
                } else {
                    console.log(error);
                }
            }
        )
    }

    //Funcion para guardar los datos de la familia
    GuardarFamilia() {
        if(this.response.productFamily.productFamilyData.autoLote && this.allowAutoLote && (this.allowConsecutivoLote && this.company <= 0)) {
            if(this.response.productFamily.productFamilyData.consecutiveLote >= 10000000000) {
                this.openSnack("El consecutivo lote supera el límite de 10 dígitos", "Aceptar");
                return;
            }
        }
        if(this.response.productFamily.productFamilyData.name != null || this.response.productFamily.productFamilyData.model != null || this.colorFamilia != null ||
            this.response.productFamily.productFamilyData.category != null || this.response.productFamily.productFamilyData.company != null || 
            this.response.productFamily.productFamilyData.gtin != null || this.response.productFamily.productFamilyData.sku != null){
                this.openSnack("Todos los campos \"*\" son obligatorios.", "Aceptar");
        }
        if (this.idFamily == 0) {
            var request = new SaveFamilyRequest();
            this.response.productFamily.productFamilyData.colorFamilia = this.colorFamilia.hex;
            if (this.response.productFamily.productFamilyData.category == 0 ||
                this.response.productFamily.productFamilyData.company == 0 ||
                this.response.productFamily.productFamilyData.gtin.trim() == "" || this.response.productFamily.productFamilyData.gtin.trim() == null ||
                this.response.productFamily.productFamilyData.image == "" || this.response.productFamily.productFamilyData.image == null ||
                this.response.productFamily.productFamilyData.model.trim() == "" || this.response.productFamily.productFamilyData.model.trim() == null ||
                this.response.productFamily.productFamilyData.name.trim() == "" || this.response.productFamily.productFamilyData.name.trim() == null ||
                this.response.productFamily.productFamilyData.sku.trim() == "" || this.response.productFamily.productFamilyData.sku.trim() == null || 
                this.response.productFamily.productFamilyData.colorFamilia.trim() == "" || this.response.productFamily.productFamilyData.colorFamilia.trim() == null) {
                    console.log(!!this.colorFamilia, !this.colorFamilia);
                this.openSnack("Captura los datos generales de la familia", "Aceptar");
                return;
            }

            if (this.response.productFamily.productFamilyData.autoLote && (this.allowAutoLote && this.company <= 0)) {
                let error = false;
                if(this.response.productFamily.productFamilyData.consecutiveLote == 0 && (this.allowConsecutivoLote && this.company <= 0))
                    error = true;
                if(this.response.productFamily.productFamilyData.prefix.trim() == "" && (this.allowPrefix && this.company <= 0))
                    error = true;
                
                if(error) {
                    this.openSnack("Captura los datos generales de la familia", "Aceptar");
                    return;
                }
            }

            var PNG = this.response.productFamily.productFamilyData.image.includes("png;base64,");

            if (!PNG) {
                this.openSnack("Formato de imagen no permitido", "Aceptar");
                return;
            }

            request.familyData.addTicket = this.response.productFamily.productFamilyData.addTicket;
            request.familyData.category = this.response.productFamily.productFamilyData.category;
            request.familyData.company = this.response.productFamily.productFamilyData.company;
            request.familyData.expiration = this.response.productFamily.productFamilyData.expiration;
            request.familyData.gtin = this.response.productFamily.productFamilyData.gtin;
            request.familyData.imageBaseFamily = this.response.productFamily.productFamilyData.image;
            request.familyData.model = this.response.productFamily.productFamilyData.model;
            request.familyData.name = this.response.productFamily.productFamilyData.name;
            request.familyData.sku = this.response.productFamily.productFamilyData.sku;
            request.familyData.status = this.response.productFamily.productFamilyData.status;
            request.familyData.warranty = this.response.productFamily.productFamilyData.warranty;
            request.familyData.lifeDays = this.response.productFamily.productFamilyData.lifeDays;
            request.familyData.autoLote = this.response.productFamily.productFamilyData.autoLote;
            request.familyData.editLote = this.response.productFamily.productFamilyData.editLote;
            request.familyData.consecutiveLote = this.response.productFamily.productFamilyData.consecutiveLote;
            request.familyData.prefix = this.response.productFamily.productFamilyData.prefix;
            request.familyData.colorFamilia = this.colorFamilia.hex;
            console.warn('Color', this.colorFamilia);
            this.servicesCenters.forEach((it, id) => {
                request.directionFamily.push({ directionId: it });
            });

            this.subiendo = true;

            this.dataService.postData<FamilyProcessResponse>("Families/saveFamily", sessionStorage.getItem("token"), request).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Familia guardada con éxito", "Aceptar");
                        setTimeout(() => this.navegar(), 3000);
                    }
                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("GuardarFamilia");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendo = false;
                    }
                }
            );
        }
        else {
            var requestUpdate = new UpdateFamilyRequest();

            if (this.response.productFamily.productFamilyData.category == 0 ||
                this.response.productFamily.productFamilyData.company == 0 ||
                this.response.productFamily.productFamilyData.gtin.trim() == "" || this.response.productFamily.productFamilyData.gtin.trim() == null ||
                this.response.productFamily.productFamilyData.image == "" || this.response.productFamily.productFamilyData.image == null ||
                this.response.productFamily.productFamilyData.model.trim() == "" || this.response.productFamily.productFamilyData.model.trim() == null ||
                this.response.productFamily.productFamilyData.name.trim() == "" || this.response.productFamily.productFamilyData.name.trim() == null ||
                this.response.productFamily.productFamilyData.sku.trim() == "" || this.response.productFamily.productFamilyData.sku.trim() == null) {
                this.openSnack("Captura los datos generales de la familia", "Aceptar");
                return;
            }

            if (this.response.productFamily.productFamilyData.autoLote && (this.allowAutoLote && this.company <= 0)) {
                let error = false;
                if(this.response.productFamily.productFamilyData.consecutiveLote == 0 && (this.allowConsecutivoLote && this.company <= 0))
                    error = true;
                if(this.response.productFamily.productFamilyData.prefix.trim() == "" && (this.allowPrefix && this.company <= 0))
                    error = true;
                
                if(error) {
                    this.openSnack("Captura los datos generales de la familia", "Aceptar");
                    return;
                }
            }

            var PNG = this.response.productFamily.productFamilyData.image.includes("png;base64,");

            if (this.banderaImagen) {
                if (!PNG) {
                    this.openSnack("Formato de imagen no permitido", "Aceptar");
                    return;
                }
            }

            requestUpdate.familyData.familyId = this.idFamily;
            requestUpdate.familyData.addTicket = this.response.productFamily.productFamilyData.addTicket;
            requestUpdate.familyData.category = this.response.productFamily.productFamilyData.category;
            requestUpdate.familyData.company = this.response.productFamily.productFamilyData.company;
            requestUpdate.familyData.expiration = this.response.productFamily.productFamilyData.expiration;
            requestUpdate.familyData.gtin = this.response.productFamily.productFamilyData.gtin;
            requestUpdate.familyData.imageBaseFamily = !this.banderaImagen ? "" : this.response.productFamily.productFamilyData.image;
            requestUpdate.familyData.model = this.response.productFamily.productFamilyData.model;
            requestUpdate.familyData.name = this.response.productFamily.productFamilyData.name;
            requestUpdate.familyData.sku = this.response.productFamily.productFamilyData.sku;
            requestUpdate.familyData.status = this.response.productFamily.productFamilyData.status;
            requestUpdate.familyData.warranty = this.response.productFamily.productFamilyData.warranty;
            requestUpdate.familyData.lifeDays = this.response.productFamily.productFamilyData.lifeDays;
            requestUpdate.familyData.autoLote = this.response.productFamily.productFamilyData.autoLote;
            requestUpdate.familyData.editLote = this.response.productFamily.productFamilyData.editLote;
            requestUpdate.familyData.consecutiveLote = this.response.productFamily.productFamilyData.consecutiveLote;
            requestUpdate.familyData.prefix = this.response.productFamily.productFamilyData.prefix;
            requestUpdate.familyData.colorFamilia = this.colorFamilia.hex;
            requestUpdate.option = 1;

            console.warn('Color', this.colorFamilia);

            this.servicesCenters.forEach((it, id) => {
                requestUpdate.directionFamily.push({ directionId: it });
            });

            this.subiendo = true;
            
            this.dataService.postData<FamilyProcessResponse>("Families/updateFamily", sessionStorage.getItem("token"), requestUpdate).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Familia editada con éxito", "Aceptar");
                        setTimeout(() => this.navegar(), 3000);
                    }
                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("GuardarFamilia");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendo = false;
                    }
                }
            );
        }
    }

    //Funcion para quitar la imagen
    eliminarImagen() {
        this.response.productFamily.productFamilyData.image = "";
    }

    //Funcion para navegar a la pantalla principal
    navegar() {
        if (this.idFamily != 0) {
            this.routelink.navigateByUrl('Familias/CatalogoFamilias', { state: { familyId: this.idFamily } })
        } else {

            this.routelink.navigate(['/Familias/CatalogoFamilias']);
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
                    case "GuardarFamilia":
                        this.GuardarFamilia();
                        break;
                    case "filtrarCombo":
                        this.filtrarCombo(this.banderaSelectDrop);
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
                this.routelink.navigate(['Login']);
                this.openSnack("Error al mandar la solicitud", "Aceptar");
                return;
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
        if (this.idFamily != 0) {
            this.BusquedaDatos();
        }
        else
            this.BusquedaCombos();
    }
    hexToRgb(hex) {
        const shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
        hex = hex.replace(shorthandRegex, (m, r, g, b) => {
            return r + r + g + g + b + b;
        });
        const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
        return result ? {
            r: parseInt(result[1], 16),
            g: parseInt(result[2], 16),
            b: parseInt(result[3], 16)
        } : null;
    }  
    ngAfterViewInit(): void {
    }
    back = () => {
        if (this.idFamily != 0) {
            this.routelink.navigateByUrl('Familias/CatalogoFamilias', { state: { familyId: this.idFamily } })
        } else {
            this._location.back();
        }
    }
}
