import { Component, OnInit } from '@angular/core';
import { NgForOf } from '@angular/common';
import { forEach } from '@angular/router/src/utils/collection';
import { DataServices } from '../../Interfaces/Services/general.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { MatSnackBar } from '@angular/material';
import { isNull } from 'util';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-agregar-etiqueta',
  templateUrl: './agregar-etiqueta.component.html',
  styleUrls: ['./agregar-etiqueta.component.less']
})
export class AgregarEtiquetaComponent implements OnInit {

  /**
   * https://www.npmjs.com/package/angularx-qrcode
   */

  primarioSuperior: any;
  secundarioSuperior: any;
  primarioIzquierdo: any;
  secundarioIzquierdo: any;
  primarioDerecho: any;
  secundarioDerecho: any;
  primarioInferior: any;
  secundarioInferior: any;
  hijos: any[] = [0, 1, 2, 3, 4, 5, 6, 7, 8];
  qrWidth: number = 240;

  dataArrays: any = {
    orientation: [{ id: 1, name: "ori-1" }, { id: 2, name: "ori-2" }, { id: 3, name: "ori-3" }, { id: 4, name: "ori-4" }, { id: 5, name: "ori-5" }],
    topPrimary: [{ id: 1, name: "asdasdas adsdasda sadasd asd" }, { id: 2, name: "pSu-2" }, { id: 3, name: "pSu-3" }, { id: 4, name: "pSu-4" }, { id: 5, name: "pSu-5" }],
    topSecondary: [{ id: 1, name: "sSu-1" }, { id: 2, name: "sSu-2" }, { id: 3, name: "sSu-3" }, { id: 4, name: "sSu-4" }, { id: 5, name: "sSu-5" }],
    leftPrimary: [{ id: 1, name: "pIz-1" }, { id: 2, name: "pIz-2" }, { id: 3, name: "pIz-3" }, { id: 4, name: "pIz-4" }, { id: 5, name: "pIz-5" }],
    leftSecondary: [{ id: 1, name: "sIz-1" }, { id: 2, name: "apsodkaposdkpaosdkpaoskdpo aposdkpaoskd" }, { id: 3, name: "sIz-3" }, { id: 4, name: "sIz-4" }, { id: 5, name: "sIz-5" }],
    rightPrimary: [{ id: 1, name: "pDe-1" }, { id: 2, name: "pDe-2" }, { id: 3, name: "pDe-3" }, { id: 4, name: "pDe-4" }, { id: 5, name: "pDe-5" }],
    rightSecondary: [{ id: 1, name: "sDe-1" }, { id: 2, name: "sDe-2" }, { id: 3, name: "sDe-3" }, { id: 4, name: "sDe-4" }, { id: 5, name: "sDe-5" }],
    bottomPrimary: [{ id: 1, name: "pIn-1" }, { id: 2, name: "pIn-2" }, { id: 3, name: "pIn-3" }, { id: 4, name: "pIn-4" }, { id: 5, name: "pIn-5" }],
    bottomSecondary: [{ id: 1, name: "sIn-1" }, { id: 2, name: "sIn-2" }, { id: 3, name: "aoskdm aosdoaisjd oaisjd aoisjd oaijs doaijsodaijsojaasdij3" }, { id: 4, name: "sIn-4" }, { id: 5, name: "sIn-5" }]
  }

  datas: Array<any> = [
    { id: 1, value: "Producto" },
    { id: 2, value: "Rangos" },
    { id: 3, value: "Datos de empaque" },
    { id: 4, value: "compañia" }
  ]


  public myAngularxQrCode: string = null;
  qrStringConstructor: any = {
    name: "",
    orientation: 0,
    grouper: false,
    nChildren: 2,
    topPrimary: "",
    topSecondary: "",
    leftPrimary: "",
    leftSecondary: "",
    rightPrimary: "",
    rightSecondary: "",
    bottomPrimary: "",
    bottomSecondary: ""
  };

  labelsConstructos: any = {
    topPrimary: "",
    topSecondary: "",
    leftPrimary: "",
    leftSecondary: "",
    rightPrimary: "",
    rightSecondary: "",
    bottomPrimary: "",
    bottomSecondary: ""
  }

  cantidadHijos: number = 0;
  cantidadArray: any[] = [];

  screenWidth: number;

  overlayRef: OverlayRef;

  state: any = {};
  action: string = "";

  constructor(
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router,
  ) {
    this.myAngularxQrCode = '';

    this.cantidadHijos = 0;

    this.screenWidth = window.innerWidth;

    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth;

      if (window.innerWidth < 620) {
        this.qrWidth = this.qrWidth / 2;
      } else {
        this.qrWidth = 240;
      }
    });
  }


  changeStrings(value, name) {

    //  
    //Seteo en el arreglo
    this.qrStringConstructor[name] = value;
    this.labelsConstructos[name] = value;
    this.setQrString();

  }

  setQrString() {
    let cons = "";

    for (let key of Object.keys(this.qrStringConstructor)) {

      cons += this.qrStringConstructor[key];

    }

    for (let key of Object.keys(this.labelsConstructos)) {

      if (this.state["action"] == 1) {
        this.labelsConstructos[key] = this.qrStringConstructor[key];
      }

      if (this.labelsConstructos[key] > 0) {
        this.labelsConstructos[key] = this.datas.find(dat => dat.id == this.labelsConstructos[key])["value"];
      }
    }

    this.myAngularxQrCode = cons;
  }

  cantidadHijosSeleccionados(newValue) {
    let cantidad = Array(newValue).fill(1);
    this.cantidadHijos = newValue;
    this.cantidadArray = cantidad;

  }

  ngOnInit() {
    this.myAngularxQrCode = '';

    this.state = window.history.state;

    if (Object.keys(this.state).length == 1) {
      this.back();
    }

    this.qrStringConstructor = {
      name: "",
      orientation: 0,
      grouper: false,
      nChildren: 2,
      topPrimary: "",
      topSecondary: "",
      leftPrimary: "",
      leftSecondary: "",
      rightPrimary: "",
      rightSecondary: "",
      bottomPrimary: "",
      bottomSecondary: ""
    }

    this.labelsConstructos = {
      topPrimary: "",
      topSecondary: "",
      leftPrimary: "",
      leftSecondary: "",
      rightPrimary: "",
      rightSecondary: "",
      bottomPrimary: "",
      bottomSecondary: ""
    }

    this.action = "Agregar";
    if (this.state["action"] == 1) {
      this.obtenerDatos();
      this.action = "Editar";
    }
  }

  obtenerDatos() {
    let data: any = {
      labelId: this.state["labelId"]
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("LabelsQR/SearchLabelsData", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.qrStringConstructor = data["labelsqr"][0];

          this.setQrString();
        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        this.openSnack("Error en la solicitud", "Aceptar");
      }
    )
  }

  guardarEtiqueta() {

    //analizar
    console.log("QR sabe", this.qrStringConstructor);

    let index = 0;
    for (let key of Object.keys(this.qrStringConstructor)) {
      if (index > 3 || index == 0) {
        if (this.qrStringConstructor[key] == "") {
          console.log("QR: ",this.qrStringConstructor[key]," key: ", key , "Index: ", index);
          if (index == 0) {
            this.openSnack(`Ingrese un nombre`, "Aceptar");
          } else if (index == 1) {
            this.openSnack(`Seleccionar una horientación`, "Aceptar");
          } else {
            this.openSnack(`Hay campos vacios`, "Aceptar");
          }
          return;
        }
      }
      index++;
    }

    console.log("sigue");

    this.qrStringConstructor["companyId"] = parseInt(sessionStorage.getItem("company"));
    
    let data: any = {
      labelqrData: this.qrStringConstructor
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("LabelsQR/SaveLabelQR", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(`respuesta`, data)
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (data["messageEsp"].length) {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.back();
        }
      },
      error => {
        console.log("error", error)
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("1");
        } else {
          console.log(error);
        }
      }
    )
  }

  back() {
    this._router.navigateByUrl('Etiquetas');
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
            this.guardarEtiqueta();
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
