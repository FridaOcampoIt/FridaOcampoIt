/// <reference types="@types/googlemaps" />
import { Component, OnInit, Inject, ViewChild, AfterViewInit, ElementRef, ContentChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { enviroments } from '../../../Interfaces/Enviroments/enviroments';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { Router, ActivatedRoute } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';
import { MouseEvent, AgmMap, GoogleMapsAPIWrapper, MapsAPILoader } from '@agm/core';

import { SearchCountriesResponse, SearchEstadosByPaisIdResponse } from  '../../../Interfaces/Models/TraceITBaseModels';
import { SaveAcopioRequest, searchAcopioById } from '../../../Interfaces/Models/AcopioModels';
export interface PaisEstado {
    id: number,
    data: string
}    
@Component({
selector: 'app-dialogo-agregar-editar-acopio',
templateUrl: './dialogo-agregar-editar-acopio.component.html',
styleUrls: ['./dialogo-agregar-editar-acopio.component.less']
})
export class DialogoAgregarEditarAcopioComponent implements OnInit, AfterViewInit {
    //Variables para el control del mapa
    latitudMarker = 0;
    longitudMarker = 0;
    latitudFloat;
    longitudFloat;

    @ViewChild('direccionInp') direccionInp: ElementRef;
    @ViewChild(AgmMap) mapo: AgmMap;
  
    bounds: any = null;
  
    dataDireccionMap: Array<any> = [
      "route",
      "street_number",
      "sublocality_level_1"
    ]
  

    /**
    * Variable, para formularios reactivos
    */
    acopioForm: FormGroup;
    //Titulo de nuestro modal
    action: string;
    //Variable de listado de paises
    dataCountrys: Array<PaisEstado> = [{id: 0, data: 'Selecciona un País'}];
    dataEstadoByPaisId: Array<PaisEstado> = [{ id: 0 , data: 'Selecciona un Estado'}];
    
    //Bloqueo pantalla carga
    overlayRef: OverlayRef;
    constructor(
        private _fb: FormBuilder,
        private _overlay: OverlayService,
        private snack: MatSnackBar,
        private _dialogRef: MatDialogRef<DialogoAgregarEditarAcopioComponent>,
        @Inject(MAT_DIALOG_DATA) public _data: any,
        private _router: Router,
        private apiloader: MapsAPILoader,
        private _dataService: DataServices
    ){
    }

    //Datos de compañia logeada y tipo.
    company: number = 0;
    isType: number = 0;
    async ngOnInit() { 
        this.action = this._data['action'];
        this.company = parseInt(sessionStorage.getItem("company"), 10);
        this.isType = parseInt(sessionStorage.getItem("isType"), 10);
        this.initMap(); 
        this.BusquedaPaises();
        this.acopioForm = this._fb.group({
            acopioId: [],
            activo: [true, Validators.required],
            numeroAcopio: ['', [Validators.required,Validators.pattern(/^-?(0|[1-9]\d*)?$/)]],
            nombreAcopio: ['', Validators.required],
            paisId: [0, Validators.required],
            estadoId: [0],
            ciudad: [''],
            codigoPostal: ['',Validators.pattern(/^-?(0|[1-9]\d*)?$/)],
            address: [""],
            latitude: [""],
            longitude: [""],
            companiaId: [this.company]
        });
        this.acopioForm.patchValue({activo: true});
        if((this._data['action']) == 'Editar'){
            await this.obtenerDatos();
        }else{
            this.acopioForm.patchValue({
                paisId: 0,
                estadoId: 0
            });
        }
    }
    ngAfterViewInit(): void  {
        setTimeout(() =>{
            this.apiloader.load().then(() => {
                this.autoplete();
            })
        }, 1000);
    }
    /**
     * Función para validar el formulario
     */
     formGetter = (_campo) => {
        return this.acopioForm.get(_campo);
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

        this.acopioForm.patchValue({
        latitude: $event.coords.lat.toString(),
        longitude: $event.coords.lng.toString()
        });
    }
    private autoplete() {
        const autoco = new google.maps.places.Autocomplete(this.direccionInp.nativeElement, {
          types: ["address"]
        });
        autoco.addListener("place_changed", () => {
          let place = autoco.getPlace();
    
          //console.log("place", place);
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
    
          this.acopioForm.patchValue({
            latitude: this.latitudMarker,
            longitude: this.longitudMarker,
            address: stringDirection
          });
    
          this.latitudFloat = this.latitudMarker;
          this.longitudFloat = this.longitudMarker;
        });
    }

    /**
     * Marks all controls in a form group as touched
     * @param formGroup - The form group to touch
     */
    private markFormGroupTouched(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {
            control.markAsTouched();

            if (control.controls) {
            this.markFormGroupTouched(control);
            }
        });
    }
    async obtenerDatos(){
        let auxLoading: any;
        setTimeout(() =>{
            auxLoading= this._overlay.open();
        });
        let data = {
            acopioId: this._data['id']
        }
        await this._dataService.postData<searchAcopioById>("Acopio/searchAcopioById", sessionStorage.getItem("token"), data).subscribe(
            data =>{
                //console.log('Response data', data);
                if(data["messageEsp"] == null){
                    this.BusquedaEstados(data["paisId"]);
                    setTimeout(()=>{
                        this.acopioForm.patchValue(data);
                        this._overlay.close(auxLoading);
                    }, 1000);
                }else{
                    this.openSnack(data["messageEsp"],"Aceptar");
                    setTimeout(()=>{
                        this._overlay.close(auxLoading);
                    }, 1000);
                }
            },error =>{
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("1");
                } else {
                    //console.log(error);
                }
                setTimeout(() => {
                    this._dialogRef.close(true);
                    this._overlay.close(auxLoading);
                },1);
            }
        )
    }
    visualAlertaError: boolean = false;
    visualAlertaSuccess: boolean = false;
    guardarAcopio(){
        //Validamos el formulario, antes de enviar información
        if (!this.acopioForm.valid) {
            this.markFormGroupTouched(this.acopioForm);
            return;
        }
        if(this._data['action'] == 'Editar'){
            //console.log('Editamos registro');
            let _updateAcopioRequest = this.acopioForm.value
            let auxLoading: any;
            setTimeout(() => {
              auxLoading = this._overlay.open();
             }, 1);
             //console.log('DataSaveUpate', _updateAcopioRequest);
             this._dataService.postData<SaveAcopioRequest>("Acopio/updateAcopio", sessionStorage.getItem("token"), _updateAcopioRequest).subscribe(
                 data =>{
                    this._overlay.close(auxLoading);
                    if(data['messageEsp'] != ''){
                        this.visualAlertaError = true;
                        setTimeout(() =>{
                            this.visualAlertaError  = false;
                        },5000);
                    }
                    else if(data['messageEsp'] == ''){
                        this.visualAlertaSuccess = true;
                        //Warning editado 07/06/2022 por Frida Ocampo
                    }
                        this.openSnack("Acopio editado con éxito","Aceptar");
                        setTimeout(()=>{
                            this._dialogRef.close(true);
                        }, 3000);
                 },
                 error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("1");
                    } else {
                        //console.log(error);
                    }
                    setTimeout(() => {
                        this._dialogRef.close(true);
                        this._overlay.close(auxLoading);
                    },1);
                 }
             );
        }else{
            let _saveAcopioRequest = this.acopioForm.value
            let auxLoading: any;
            setTimeout(() => {
              auxLoading = this._overlay.open();
            }, 1);
            console.log('DataSave', _saveAcopioRequest);
            delete _saveAcopioRequest['acopioId'];
            console.log('DataSave', _saveAcopioRequest);
            this._dataService.postData<SaveAcopioRequest>("Acopio/saveAcopio", sessionStorage.getItem("token"), _saveAcopioRequest).subscribe(
                data =>{
                this._overlay.close(auxLoading);
                if(data['messageEsp'] != ''){
                    this.visualAlertaError = true;
                    setTimeout(() =>{
                        this.visualAlertaError  = false;
                    },5000);
                }
                else if(data['messageEsp'] == ''){
                    this.visualAlertaSuccess = true;
                    //Se agrego warning de Guardado con exito 07/06/2022 por Frida Ocampo
                    this.openSnack("Acopio guardado con éxito","Aceptar");
                    setTimeout(()=>{
                        this._dialogRef.close(true);
                    }, 3000);
                }
                },
                error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("1");
                } else {
                    //console.log(error);
                }
                setTimeout(() => {
                    this._dialogRef.close(true);
                    this._overlay.close(auxLoading);
                },1);
                }
            );
        }
    }


    //Funcion para cargar los paises en el combo
    BusquedaPaises() {
        let auxLoading: any;
        setTimeout(() => {
          auxLoading = this._overlay.open();
         }, 1);
        this._dataService.postData<SearchCountriesResponse>("Families/searchCountries", sessionStorage.getItem("token")).subscribe(
            data => {
                if (data["messageEsp"] != "") {
                    this.openSnack(data["messageEsp"], "Aceptar");
                }else {
                    this.dataCountrys = data.countriesData;
                }
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1);
            },error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("1");
                } else {
                    //console.log(error);
                }
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1);
            }
        );
    }
    paisId: number = 0;
    disabledSelected: boolean = false;
    onChangePaisSelected(any){
        this.BusquedaEstados(any);
    }
    //Funcion para cargar los paises en el combo
    BusquedaEstados(paisId: number) {
        let data: any = {
            paisId: paisId
        };
        let auxLoading: any;
        setTimeout(() => {
          auxLoading = this._overlay.open();
         }, 1);
        this._dataService.postData<SearchEstadosByPaisIdResponse>("Families/searchEstadoByPaisId", sessionStorage.getItem("token"), data).subscribe(
            data => {
                if (data["messageEsp"] != "") {
                    this.openSnack(data["messageEsp"], "Aceptar");
                }else {
                    this.dataEstadoByPaisId = data.estadosData;
                    if((this._data['action']) == 'Agregar'){
                        this.acopioForm.patchValue({
                            estadoId: 0
                        });
                    }
                }
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1);
                this.disabledSelected = true;
            },error => {
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1);
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("1");
                } else {
                    //console.log(error);
                }
            }
        );
    }
    //Funcion para realizar el proceso del relogin
    relogin(peticion) {
        var requestLogin = new LoginUserRequest();
        requestLogin.user = sessionStorage.getItem("email");
        requestLogin.password = sessionStorage.getItem("password");

        this._dataService.postData<LoginUserResponse>("User/loginUser", "", requestLogin).subscribe(
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
                    case "1":
                        this.obtenerDatos();
                    break;
                    case "2":
                        this.guardarAcopio();
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
    openSnack = (message: string, action: string) => {
        this.snack.open(message, action, {
            duration: 5000
        });
    }

}  