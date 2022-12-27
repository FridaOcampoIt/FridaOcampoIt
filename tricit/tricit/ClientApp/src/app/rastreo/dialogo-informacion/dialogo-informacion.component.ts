import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { DataServices } from '../../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dialogo-informacion',
  templateUrl: './dialogo-informacion.component.html',
  styleUrls: ['./dialogo-informacion.component.less']
})
export class DialogoInformacionComponent implements OnInit {

  infoForm: FormGroup;

  titulo: string = "";
  nombre: string = "";
  refInter: string = "";
  refExter: string = "";
  paisId: number = 0;
  estadoId: number = 0;

  overlayRef: OverlayRef;

  responsePaisEstado: any = {};

  constructor(
    private _fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router
  ) { }

  formGetter = (_campo) => {
    return this.infoForm.get(_campo);
  }

  ngOnInit() {

    this.infoForm = this._fb.group({
      nombre: [""],
      razonSocial: [""],
      contacto: [""],
      telefono: [""],
      pais: [0],
      estado: [0],
      ciudad: [""],
      cp: [""],
      domicilio: [""]
    });

    this.titulo = this.data["titulo"];
    this.BusquedaCombos();


  }

  /**
   * 1 general
   * 2 remitente
   * 3 infolegal
   * 4 destinatario
   */
  obtenerDatos(campo = 6) {
    let data: any = {
      trackingId: this.data["id"],
      opc: campo
    };

    this._dataService.postData<any>("Tracking/GetTrackingInfo", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {

          let action = "";
          let letter = "";
          let texto = ""; 1
          if (this.data["action"] == 0) {
            //remitente
            action = "eventSenderInfo";
            texto = "Remitente";
            letter = "R";
          } else {
            //Receptor
            action = "eventRecipientInfo";
            texto = "Destinatario";
            letter = "D";
          }

          this.refExter = data["eventInfo"][0]["referenciaExterna"];
          this.refInter = data["eventInfo"][0]["referenciaInterna"];
          this.nombre = data["eventInfo"][0]["nombreAgrupacion"];
          this.infoForm.patchValue({
            nombre: data[action][0][`nombreCompania${letter}`],
            razonSocial: data[action][0][`rzCompania${letter}`],
            contacto: data[action][0][`nombre${texto}`],
            telefono: data[action][0][`telefono${letter}`],
            pais: data[action][0][`pais${letter}`],
            estado: data[action][0][`estado${letter}`],
            ciudad: data[action][0][`ciudad${letter}`],
            cp: data[action][0][`cp${letter}`],
            domicilio: data[action][0][`domicilio${letter}`]
          })
          this.paisId = data["eventRecipientInfo"][0]["paisD"];
          this.estadoId = data["eventRecipientInfo"][0]["estadoD"];

        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("1");
        } else {
          this.openSnack("Error en la solicitud", "Aceptar");
          console.log(error);
        }
      }
    )
  }

  BusquedaCombos() {
    var request = {};
    request["paisId"] = this.paisId;

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("Movimientos/searchPaisEstadoDropDown", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responsePaisEstado = data;

        this.obtenerDatos();
      },
      error => {
        this._overlay.close(this.overlayRef);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("2");
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
        sessionStorage.setItem("isType", data.userData.userData.isType.toString());

        switch (peticion) {
          case "1":
            this.obtenerDatos();
            break;
          case "2":
            this.BusquedaCombos();
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
