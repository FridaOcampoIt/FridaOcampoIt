/// <reference types="@types/googlemaps" />
import { Component, OnInit, Inject, ViewChild, AfterViewInit } from '@angular/core';
import { MouseEvent, AgmMap, GoogleMapsAPIWrapper, MapsAPILoader } from '@agm/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';
import { SearchCountriesResponse } from '../../Interfaces/Models/TraceITBaseModels';
import { DataServices } from '../../Interfaces/Services/general.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-dialogo-agregar-direccion',
  templateUrl: './dialogo-agregar-direccion.component.html',
  styleUrls: ['./dialogo-agregar-direccion.component.less']
})
export class DialogoAgregarDireccionComponent implements OnInit, AfterViewInit {

  //Variables para el control del mapa
  latitudMarker = 0;
  longitudMarker = 0;
  latitudFloat;
  longitudFloat;

  direForm: FormGroup;

  regNumerico: RegExp = new RegExp(enviroments.patterns.numerical);

  responseCountries = new SearchCountriesResponse();

  overlayRef: OverlayRef;

  @ViewChild("direccionInp") direccionInp: any;
  @ViewChild(AgmMap) mapo: AgmMap;

  bounds: any = null;

  dataDireccionMap: Array<any> = [
    "route",
    "street_number",
    "sublocality_level_1"
  ]

  title: string = "";

  empacadoresLst: any[] = [];

  empacadoresLstSelecteds: any[] = [];

  constructor(
    private _fb: FormBuilder,
    private _dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dialogRef: MatDialogRef<DialogoAgregarDireccionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _router: Router,
    private apiloader: MapsAPILoader
  ) {
  }

  //Funcion para iniciar y centrar el mapa
  initMap() {
    this.latitudFloat = 25.2113201;
    this.longitudFloat = -101.524912;
  }

  formGetter = (_campo) => {
    return this.direForm.get(_campo);
  }

  //Funcion para agregar el marcador
  mapClicked($event: MouseEvent) {
    this.latitudMarker = $event.coords.lat;
    this.longitudMarker = $event.coords.lng;

    this.direForm.patchValue({
      latitude: $event.coords.lat.toString(),
      longitude: $event.coords.lng.toString()
    });
  }

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

      this.direForm.patchValue({
        latitude: this.latitudMarker,
        longitude: this.longitudMarker,
        address: stringDirection
      });

      this.latitudFloat = this.latitudMarker;
      this.longitudFloat = this.longitudMarker;
    })

  }

  ngOnInit() {
    this.initMap();

    this.direForm = this._fb.group({
      addressId: [0],
      status: [true],
      addressName: ["", [Validators.required]],
      phone: ["", [Validators.required, Validators.pattern(this.regNumerico)]],
      country: ["", [Validators.required]],
      state: ["", []],
      city: ["", []],
      cp: ["", [Validators.maxLength(10), Validators.pattern(this.regNumerico)]],
      address: ["", []],
      latitude: [""],
      longitude: [""],
      typeCompany: [],
      providers: [this.empacadoresLstSelecteds],
      familyId: []
    });

    this.BusquedaPaises();

    this.direForm.patchValue({
      status: true,
      familyId: this.data["familyId"],
      typeCompany: this.data["typeCompany"],
      addressId: this.data["addressId"]
    });


    this.title = "Agregar dirección de proveedor";
    

    // this.autoplete();
  }

  ngAfterViewInit(): void {
    this.apiloader.load().then(() => {

      this.autoplete();
    })

  }

  obtenerDatos() {
    let data: any = {
      addressId: this.data["addressId"]
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("AddressProvider/SearchAddressData", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log("data", data);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.direForm.patchValue(data["addressLst"][0]);
          
          this.latitudMarker = data["addressLst"][0]["latitude"];
          this.longitudMarker = data["addressLst"][0]["longitude"];

          this.latitudFloat = data["addressLst"][0]["latitude"];
          this.longitudFloat = data["addressLst"][0]["longitude"];
        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("2");
        } else {
          console.log(error);
        }
      }
    )
  }

  //Funcion para cargar los paises en el combo
  BusquedaPaises() {
    this._dataService.postData<SearchCountriesResponse>("Families/searchCountries", sessionStorage.getItem("token")).subscribe(
      data => {

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.responseCountries = data;

          this.busquedaEmpacadores();
        }
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("1");
        } else {
          console.log(error);
        }
      }
    );
  }

  busquedaEmpacadores() {

    let data: any = {
      compania: parseInt(sessionStorage.getItem("company"), 10)
    };

    this._dataService.postData<SearchCountriesResponse>("AddressProvider/SearchProviders", sessionStorage.getItem("token")).subscribe(
      data => {

        console.log(data);

        if (this.data["typeCompany"] == "Proveedor") {
          
        } else if (this.data["typeCompany"] == "Distribuidor") {
          
        } else if (this.data["typeCompany"] == "Empacado externo") {
          data["providers"].forEach(element => {
            element["type"] == 1 ? this.empacadoresLst.push({id: element["id"], name: element["name"]}) : null;
          });
        } else if (this.data["typeCompany"] == "Empacado interno") {
          data["providers"].forEach(element => {
            element["type"] == 2 ? this.empacadoresLst.push({id: element["id"], name: element["name"]}) : null;
          });
        }

        if (this.data["action"] == 1) {
          this.obtenerDatos();
          this.title = "Editar dirección de proveedor";
        }
      },
      error => {
        console.log("error", error);

        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("2");
        } else {
          console.log(error);
        }
      }
    );

  }

  guardarDireccion() {

    if (!this.direForm.valid) {
      this.markFormGroupTouched(this.direForm);
      return;
    }

    let data: any = this.direForm.value;


    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("AddressProvider/SaveAddress", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(`respuesta`, data)
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (data["messageEsp"] != "") {
          this.openSnack(`${data["messageEsp"]}`, "Aceptar");
        } else {
          this._dialogRef.close(true);
        }

      },
      error => {
        console.log("error", error)
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("3");
        } else {
          console.log(error);
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
            this.BusquedaPaises();
            break;
          case "2":
            this.obtenerDatos();
            break;
          case "3":
            this.guardarDireccion();
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

  public compareObjets(obj1, obj2): boolean {
    return obj1["id"] == obj2["id"];
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

  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }

}
