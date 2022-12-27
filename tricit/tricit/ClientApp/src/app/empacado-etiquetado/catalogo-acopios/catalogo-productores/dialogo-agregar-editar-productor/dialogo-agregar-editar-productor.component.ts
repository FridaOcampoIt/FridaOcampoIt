import { Component, OnInit, Inject, ViewChild, AfterViewInit, ElementRef, ContentChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { OverlayRef } from '@angular/cdk/overlay';
import { Router, ActivatedRoute } from '@angular/router';
import { enviroments } from '../../../../Interfaces/Enviroments/enviroments';
import { DataServices } from '../../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../../Interfaces/Services/overlay.service';
import { LoginUserRequest, LoginUserResponse } from '../../../../Interfaces/Models/LoginModels';


import { MouseEvent, AgmMap, GoogleMapsAPIWrapper, MapsAPILoader } from '@agm/core';

import { SearchCountriesResponse } from  '../../../../Interfaces/Models/TraceITBaseModels';
import { SaveProductorRequest, SearchProductorById, UpdateProductorByIdRequest } from '../../../../Interfaces/Models/ProductorModels';
import { searchAcopioResponse } from '../../../../Interfaces/Models/AcopioModels';
import { SaveAddressRequest } from '../../../../Interfaces/Models/AddressModels';
interface compania {
    businessName: string;
    idCompany: number;
    name: string;
    phone: string;
}
@Component({
selector: 'app-dialogo-agregar-editar-productor',
templateUrl: './dialogo-agregar-editar-productor.component.html',
styleUrls: ['./dialogo-agregar-editar-productor.component.less']
})
export class DialogoAgregarEditarProductorComponent implements OnInit, AfterViewInit  {
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
    productorForm: FormGroup;
    //Titulo de nuestro modal
    action: string;
    //Variable de listado de paises
    nameCompania: compania;

    //Bloqueo pantalla carga
    overlayRef: OverlayRef;
    constructor(
        private _fb: FormBuilder,
        private _overlay: OverlayService,
        private snack: MatSnackBar,
        private _dialogRef: MatDialogRef<DialogoAgregarEditarProductorComponent>,
        @Inject(MAT_DIALOG_DATA) public _data: any,
        private _router: Router,
        private _dataService: DataServices,
        private apiloader: MapsAPILoader
    ){
    }

    companiaId: number = 0;
    ngOnInit(){        
        this.companiaId =   parseInt(sessionStorage.getItem("company"), 10);
        this.getCompanyNameLogeada(parseInt(sessionStorage.getItem("company"), 10));
        this.getListAcopio();
        this.productorForm = this._fb.group({
            productorId: [0],
            activo: [true, Validators.required],
            numeroProductor: ['', [Validators.required, Validators.pattern(/^-?(0|[1-9]\d*)?$/)]],
            nombreProductor: ['', Validators.required],
            nombreRancho: ['', Validators.required],
            nombreContacto: [''],
            apellidoContacto: [''],
            telefonoContacto: [''],
            address: [""],
            latitude: [""],
            longitude: [""],
            acopiosId: [0,[ Validators.required, Validators.minLength(1), Validators.maxLength(1)]],
            companiaId: [this.companiaId],
            auxAcopiosId: [0]
        });
        
        this.productorForm.patchValue({activo: true});
        if((this._data['action']) == 'Editar'){
            this.obtenerDatos();
        }   
        this.action = this._data['action'];
    }

    ngAfterViewInit(): void  {
        setTimeout(() =>{
            this.apiloader.load().then(() => {
                this.autoplete();
            })
        }, 1000);
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

        this.productorForm.patchValue({
        latitude: $event.coords.lat.toString(),
        longitude: $event.coords.lng.toString()
        });
    }
    private autoplete() {
        //console.log('Native', this.direccionInp);
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
    
          this.productorForm.patchValue({
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

    obtenerDatos(){
        let data =  {
            productorId: this._data['id']
        }
        //console.log('Obtiene datos', this._data);
        let auxLoading: any;
        setTimeout(() => {
            auxLoading = this._overlay.open();
        }, 1);
        this._dataService.postData<SearchProductorById>("Productor/searchProductorById", sessionStorage.getItem("token"), data).subscribe(
            data =>{
                //console.log('RECUPERACION REGISTRO', data);
                if(!!data['messageEsp']){
                    this.openSnack(data['messageEsp'], "Aceptar");
                    setTimeout(() =>{
                        this._dialogRef.close(true);
                    },5000);
                }
                else if(data['messageEsp'] == '' || data['messageEsp'] ==  null){
                    //Array en string
                    let arraytemporal = data["getAcopiosId"].split(',');
                    //Convertimos el string a enteros (porque yolo, no funciona con string)
                    let arrayNumeros = arraytemporal.map(str => {
                        return Number(str);
                    });
                    //Si recuperamos la información de un registro guardamos los id's que estaban en el arreglo
                    this.productorForm.patchValue({
                        acopiosId: arrayNumeros,
                        auxAcopiosId: arrayNumeros
                    });
                    //console.log('GETACOPIOS', this.productorForm.get('auxAcopiosId').value);
                    this.productorForm.patchValue(data);
                }
                this._overlay.close(auxLoading);
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
    visualAlertaError: boolean = false;
    visualAlertaSuccess: boolean = false;
    guardarEditarProductor(){
        //console.log('Entra al guardar', this.productorForm.value);
        //Validamos el formulario, antes de enviar información
        if (!this.productorForm.valid) {
            this.markFormGroupTouched(this.productorForm);
            //console.log(this.markFormGroupTouched(this.productorForm));
            return;
        }
        if(this._data['action'] == 'Editar'){
            //console.log('ESTO SE GUARDA', this.productorForm.value);
            let _updateProductorByIdRequest = this.productorForm.value
            let auxLoading: any;
            setTimeout(() => {
              auxLoading = this._overlay.open();
             }, 1);
             //console.log('DataSaveUpate', _updateProductorByIdRequest);
             this._dataService.postData<UpdateProductorByIdRequest>("Productor/updateProductorById", sessionStorage.getItem("token"), _updateProductorByIdRequest).subscribe(
                 data =>{
                     //console.log('Update data', data);
                    this._overlay.close(auxLoading);
                    if(data['messageEsp'] != ''){
                        this.visualAlertaError = true;
                        setTimeout(() =>{
                            this.visualAlertaError  = false;
                        },5000);
                    }
                    else if(data['messageEsp'] == ''){
                        this.visualAlertaSuccess = true;
                        this.openSnack("Productor editado con éxito","Aceptar");
                    }
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
             
        }else if( (this._data['action']) == 'Agregar'){
            let _saveProductorRequest =  this.productorForm.value
            //console.log('DataSave before', _saveProductorRequest);
            let auxLoading: any;
            setTimeout(() => {
              auxLoading = this._overlay.open();
             }, 1);
             //console.log('DataSave', _saveProductorRequest);
             this._dataService.postData<SaveProductorRequest>("Productor/saveProductor", sessionStorage.getItem("token"), _saveProductorRequest).subscribe(
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
                        this.openSnack("Productor guardado con éxito","Aceptar");
                    }
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
        }
    }

    listAcopio : Array<searchAcopioResponse> = [];
    getListAcopio() { 
        let data: any = {
            companiaId: this.companiaId,
            nombreNumeroAcopio: ''
        };
        let auxLoading: any;
        setTimeout(() => {
          auxLoading = this._overlay.open();
         }, 1);
        this._dataService.postData<searchAcopioResponse>("Acopio/searchListAcopios", sessionStorage.getItem("token"), data).subscribe(
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
    

    /**
     * Función para validar el formulario
     */
    formGetter = (_campo) => {
        return this.productorForm.get(_campo);
    } 
    /**
     * 
     * @param id
     * Función para recuperar el nombre de la compañía logeada
     *
     */
    getCompanyNameLogeada(id){
        let auxLoading: any;
        setTimeout(() => {
          auxLoading = this._overlay.open();
         }, 1);
        let data = {
            companiaId: id
        }
        this._dataService.postData("Companies/searchCompanyName", sessionStorage.getItem("token"), data).subscribe(
            data =>{
                if (data["messageEsp"] != "") {
                    this.openSnack(data["messageEsp"], "Aceptar");
                }else {
                    this.nameCompania = data['companiaName'];
                    this.productorForm.patchValue({
                        nombreRancho: this.nameCompania.name
                    });
                    setTimeout(() => {
                        this._overlay.close(auxLoading);
                    },1);
                    //console.log('Datos company', data, this.nameCompania);
                }
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
        )
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
                        this.guardarEditarProductor();
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