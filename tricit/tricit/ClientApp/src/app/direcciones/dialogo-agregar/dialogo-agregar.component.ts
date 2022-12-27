/// <reference types="@types/googlemaps" />
import { Component, OnInit, Inject, ViewChild, AfterViewInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatSnackBar } from '@angular/material';
import { MouseEvent, MapsAPILoader } from '@agm/core'

//DataServices
import {
    SearchDropDownListAddressResponse,
    SearchDropDownListAddressRequest,
    SearchAddressDataRequest,
    SearchAddressDataResponse,
    SaveAddressRequest,
    AddressProcessResponse,
    EditAddressRequest
} from '../../Interfaces/Models/AddressModels';
import { DataServices } from '../../Interfaces/Services/general.service'
import { SearchCountriesResponse } from '../../Interfaces/Models/TraceITBaseModels';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';

@Component({
    selector: 'app-dialogo-agregar',
    templateUrl: './dialogo-agregar.component.html',
    styleUrls: ['./dialogo-agregar.component.less']
})

export class DialogoAgregarComponent implements OnInit, AfterViewInit {

    constructor(
        @Inject(MAT_DIALOG_DATA)
        private _data: any,
        private dataService: DataServices,
        private _dialogRef: MatDialogRef<DialogoAgregarComponent>,
        private snack: MatSnackBar,
        private _router: Router,
        private apiloader: MapsAPILoader) { }

    //Variables para el control del mapa
    latitudMarker = 0;
    longitudMarker = 0;
    latitudFloat;
    longitudFloat;

    //Modelos y variables para la vista
    title: string;
    responseListDropDown = new SearchDropDownListAddressResponse();
    response = new SearchAddressDataResponse();
    responseCountries = new SearchCountriesResponse();
    subiendo: boolean = false;
    companies : any[] = [];

    dataDireccionMap: Array<any> = [
        "route",
        "street_number",
        "sublocality_level_1"
    ]

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

    //Funcion para iniciar y centrar el mapa
    initMap() {
        this.latitudFloat = 25.2113201;
        this.longitudFloat = -101.524912;
    }

    //Funcion para agregar el marcador
    mapClicked($event: MouseEvent) {
        this.latitudMarker = $event.coords.lat;
        this.longitudMarker = $event.coords.lng;
    }

    @ViewChild("direccionInp") direccionInp: any;

    private autoplete() {
        const autoco = new google.maps.places.Autocomplete(this.direccionInp.nativeElement, {
            types: ["address"]
        });

        // google.maps.event.addListener(autoco,'place_changed',() => {

        // })
        autoco.addListener("place_changed", () => {
            let place = autoco.getPlace();

            console.log("place", place);
            this.latitudMarker = place.geometry.location.lat();
            this.longitudMarker = place.geometry.location.lng();

            let direction = {};


            for (var i = 0; i < place.address_components.length; i++) {
                var addressType = place.address_components[i].types[0];
                if (this.dataDireccionMap.includes(addressType)) {
                    var val = place.address_components[i]["long_name"];
                    direction[addressType] = val;
                }
            }

            let stringDirection = "";

            let datosIn = 0;
            this.dataDireccionMap.forEach(key => {
                if (datosIn == this.dataDireccionMap.length - 1) {
                    stringDirection += `${direction[key]}`;
                } else {
                    stringDirection += `${direction[key]}, `;
                }
                datosIn++;
            });


            this.response.addressData.address = stringDirection;

            this.latitudFloat = this.latitudMarker;
            this.longitudFloat = this.longitudMarker;
        })

    }

    //Funcion para realizar la busqueda de combos de compañia y tipo direccion
    cargarCombos() {
        var request = new SearchDropDownListAddressRequest();
        request.idCompany = parseInt(sessionStorage.getItem("company"));
        this.dataService.postData<SearchDropDownListAddressResponse>("Address/searchDropDownListAddress", sessionStorage.getItem("token"), request).subscribe(
            data => {

                this.responseListDropDown = data;

                this.companies = this.responseListDropDown.companyData.filter(x => x.data.length);

                this.response.addressData.companyId = parseInt(sessionStorage.getItem("company"));

                if (this._data.id != 0) {
                    this.cargarDatos();
                }
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("cargarCombos");
                } else {
                    console.log(error);
                }
            }
        );
    }

    //Funcion para cargar los datos de las direcciones que se selecciono
    cargarDatos() {
        var request = new SearchAddressDataRequest();
        request.idAddress = this._data.id;

        this.dataService.postData<SearchAddressDataResponse>("Address/searchAddressData", sessionStorage.getItem("token"), request).subscribe(
            data => {

                this.response = data;
                this.latitudMarker = parseFloat(this.response.addressData.latitude);
                this.longitudMarker = parseFloat(this.response.addressData.longitude);
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("cargarDatos");
                } else {
                    console.log(error);
                }
            }
        )
    }

    //Funcion de guardar direcciones
    saveAddress() {
        if (this.response.addressData.idAddress == 0) {
            if (this.response.addressData.address.trim() == "" ||
                this.response.addressData.country.trim() == "" || this.response.addressData.directionType == 0 ||
                this.response.addressData.companyId == 0 || this.response.addressData.name.trim() == "" ||
                this.response.addressData.phone.trim() == "") {
                this.openSnack("Captura los datos generales de la dirección", "Aceptar");
                return;
            }

            if (this.latitudMarker == 0 || this.longitudMarker == 0) {
                this.openSnack("Selecciona la ubicación geográfica", "Aceptar");
                return;
            }

            var requestSave = new SaveAddressRequest();
            requestSave.address = this.response.addressData.address;
            requestSave.city = this.response.addressData.city;
            requestSave.country = this.response.addressData.country;
            requestSave.latitude = this.latitudMarker.toString();
            requestSave.longitude = this.longitudMarker.toString();
            requestSave.name = this.response.addressData.name;
            requestSave.phone = this.response.addressData.phone;
            requestSave.postalCode = this.response.addressData.postalCode;
            requestSave.state = this.response.addressData.state;
            requestSave.status = this.response.addressData.status;
            requestSave.idCompany = this.response.addressData.companyId;
            requestSave.idTypeAddress = this.response.addressData.directionType;

            this.subiendo = true;

            this.dataService.postData<AddressProcessResponse>("Address/saveAddres", sessionStorage.getItem("token"), requestSave).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Dirección guardada con éxito", "Aceptar");
                        this._dialogRef.close(true);
                    }
                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("saveAddress");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendo = false;
                    }
                }
            )
        } else {
            if (this.response.addressData.address.trim() == "" ||
                this.response.addressData.country.trim() == "" || this.response.addressData.directionType == 0 ||
                this.response.addressData.companyId == 0 || this.response.addressData.name.trim() == "" ||
                this.response.addressData.phone.trim() == "") {
                this.openSnack("Captura los datos generales de la dirección", "Aceptar");
                return;
            }

            if (this.latitudMarker == 0 || this.longitudMarker == 0) {
                this.openSnack("Selecciona la ubicación geográfica", "Aceptar");
                return;
            }

            var requestEdit = new EditAddressRequest();
            requestEdit.idAddress = this.response.addressData.idAddress;
            requestEdit.address = this.response.addressData.address;
            requestEdit.city = this.response.addressData.city;
            requestEdit.country = this.response.addressData.country;
            requestEdit.latitude = this.latitudMarker.toString();
            requestEdit.longitude = this.longitudMarker.toString();
            requestEdit.name = this.response.addressData.name;
            requestEdit.phone = this.response.addressData.phone;
            requestEdit.postalCode = this.response.addressData.postalCode;
            requestEdit.state = this.response.addressData.state;
            requestEdit.status = this.response.addressData.status;
            requestEdit.idCompany = this.response.addressData.companyId;
            requestEdit.idTypeAddress = this.response.addressData.directionType;

            this.subiendo = true;

            this.dataService.postData<AddressProcessResponse>("Address/editAddress", sessionStorage.getItem("token"), requestEdit).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Dirección editada con éxito", "Aceptar");
                        this._dialogRef.close(true);
                    }
                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("saveAddress");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendo = false;
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
                sessionStorage.setItem("company", data.userData.userData.company.toString());
                sessionStorage.setItem("isType", data.userData.userData.isType.toString());

                switch (peticion) {
                    case "BusquedaPaises":
                        this.BusquedaPaises();
                        break;
                    case "cargarCombos":
                        this.cargarCombos();
                        break;
                    case "cargarDatos":
                        this.cargarDatos();
                        break;
                    case "saveAddress":
                        this.saveAddress();
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

    //Funcion que manda llamar las funciones necesarias para los datos
    ngOnInit() {
        this.title = this._data.title;
        this.initMap();
        this.cargarCombos();
        this.BusquedaPaises();
    }

    ngAfterViewInit(): void {
        this.apiloader.load().then(() => {

            this.autoplete();
        })
    }
}
