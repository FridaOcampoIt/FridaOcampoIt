import { Component, OnInit, Inject } from '@angular/core';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router } from '@angular/router';
import { OverlayRef } from '@angular/cdk/overlay';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-detalle',
  templateUrl: './dialogo-detalle.component.html',
  styleUrls: ['./dialogo-detalle.component.less']
})
export class DialogoDetalleComponent implements OnInit {

  datosList: any[] = [

  ]

  vistasList: any[] = [
    { id: 1, nombre: "Agrupación" },
    { id: 2, nombre: "Pallet" },
    { id: 3, nombre: "Caja" },
  ]

  viewType: number;
  groupingId: number;
  proviene: number;
  box: number;
  pallet: number;

  direccion: string;

  overlayRef: OverlayRef;

  urlObtenerDatos: string;

  dataDetalle: any = {};

  constructor(
    private dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dialogRef: MatDialogRef<DialogoDetalleComponent>,
    @Inject(MAT_DIALOG_DATA) public _data: any,
    private _router: Router
  ) { }

  ngOnInit() {
    this.direccion = "";
    this.viewType = this._data["viewType"];
    this.groupingId = this._data["groupingId"];
    this.proviene = this._data["proviene"];
    this.direccion = this._data["direccion"];
    this.box = this._data["box"];
    this.pallet = this._data["pallet"];


    if (this.proviene == null || this.groupingId == null) {
      this._router.navigateByUrl('EmpacadoEtiquetado', { state: { registro: 1 } })
    }

    switch (this.proviene) {
      case 1:
        //Interno
        this.urlObtenerDatos = "InternalPacked/SearchBoxManagementDetail";
        break;
      case 2:
        //externo
        this.urlObtenerDatos = "ExternalPacked/SearchBoxManagementDetail";
        break;
      default:
        break;
    }

    
    this.obtenerDatos();
  }

  obtenerDatos(action = 0) {

    if (action == 1) {
      this.construidDatos();
    }
    else {
      let data: any = {
        groupingId: this.groupingId,
        typeView: this.viewType,
        productId: this._data["productoId"],
        box: this.box,
        pallet: this.pallet
      };
      console.log('DATA GET', )
      setTimeout(() => {
        this.overlayRef = this._overlay.open();
      }, 1);
      //debugger
      this.dataService.postData<any>(this.urlObtenerDatos, sessionStorage.getItem("token"), data).subscribe(
        data => {
          setTimeout(() => {
            this._overlay.close(this.overlayRef);
          }, 1);
          console.log("data", data);
          if (data["messageEsp"] != "") {
            this.openSnack(data["messageEsp"], "Aceptar");
          } else {

            if (data["infoOperation"] && data["operationDetails"] && data["detailsOrders"]) {
              this.dataDetalle = {
                operationInfo: data["infoOperation"],
                operationDetails: data["operationDetails"],
                detailsOrders: data["detailsOrders"]
              }

              this.construidDatos();
            }


            // this.datosList = data["infoData"];
            // this.direccion = data["infoData"][0]["addressName"];
          }
        },
        error => {
          console.log("error", error);
          setTimeout(() => {
            this._overlay.close(this.overlayRef);
          }, 1);
          if (error.error.hasOwnProperty("messageEsp")) {
            this.relogin("1")
          } else {
            console.log(error);
          }
        }
      )
    }
  }

  construidDatos() {
    let datos = {};
    this.datosList = [];
    switch (this.viewType) {
      case 1: // Agrupación -> operación
        datos = {};
        datos["registerDate"] = this.dataDetalle["operationInfo"]["fechaRegistro"];
        datos["lineName"] = this.dataDetalle["operationInfo"]["linea"];
        datos["operatorName"] = this.dataDetalle["operationInfo"]["operator"];
        datos["rangeDetail"] = `${this.dataDetalle["operationDetails"][0]["rangoMin"]}-${this.dataDetalle["operationDetails"][this.dataDetalle["operationDetails"].length - 1]["rangoMin"]}`;
        datos["productName"] = this.dataDetalle["operationInfo"]["product"];
        datos["quantity"] = (parseInt(this.dataDetalle["operationInfo"]["unitsBox"]) * this.dataDetalle["operationDetails"].length);

        this.datosList.push(datos);
        break;
      case 2: // Pallet

        let boxesPallet = [];

        let maxPallet: number = 0;

        // Obtener cuantos pallets hay en la operación
        this.dataDetalle["operationDetails"].forEach(box => {
          if (parseInt(box["pallet"].split("-")[1]) > maxPallet) {
            maxPallet++;
          }
        });

        let contPallet = 1;
        // separar todas las cajas del mismo Pallet
        do {
          
          boxesPallet.push({
            pallet: contPallet,
            cajas: this.dataDetalle["operationDetails"].filter(x => x.pallet.split("-")[1] == contPallet)
          });
          contPallet++;
        } while (contPallet <= maxPallet);
        
        

        // Obtener el rango inicial y final de cada pallet
        boxesPallet.forEach(pallet => {
          if(this._data.viewType != 1) {
            if(pallet.pallet == this._data.pallet) {
              this.datosList.push({
                registerDate: this.dataDetalle["operationInfo"]["fechaRegistro"],
                lineName: this.dataDetalle["operationInfo"]["linea"],
                operatorName: this.dataDetalle["operationInfo"]["operator"],
                productName: this.dataDetalle["operationInfo"]["product"],
                rangeDetail : `${pallet["cajas"][0]["rangoMin"]}-${pallet["cajas"][pallet["cajas"].length - 1]["rangoMax"]}`,
                quantity : (parseInt(this.dataDetalle["operationInfo"]["unitsBox"]) * pallet["cajas"].length)
              });
            }
          } else {
            this.datosList.push({
              registerDate: this.dataDetalle["operationInfo"]["fechaRegistro"],
              lineName: this.dataDetalle["operationInfo"]["linea"],
              operatorName: this.dataDetalle["operationInfo"]["operator"],
              productName: this.dataDetalle["operationInfo"]["product"],
              rangeDetail : `${pallet["cajas"][0]["rangoMin"]}-${pallet["cajas"][pallet["cajas"].length - 1]["rangoMax"]}`,
              quantity : (parseInt(this.dataDetalle["operationInfo"]["unitsBox"]) * pallet["cajas"].length)
            });
          }
        });


        break;
      case 3: // Caja
      // Por cada caja existente agregar el registro(ficha)
      this.dataDetalle["operationDetails"].forEach(caja => {
        if(this._data.viewType == 2) {
          if(caja.pallet == "PL-"+this._data.pallet) {
            this.datosList.push({
              registerDate: this.dataDetalle["operationInfo"]["fechaRegistro"],
              lineName: this.dataDetalle["operationInfo"]["linea"],
              operatorName: this.dataDetalle["operationInfo"]["operator"],
              productName: this.dataDetalle["operationInfo"]["product"],
              rangeDetail : `${caja["rangoMin"]}-${caja["rangoMax"]}`,
              quantity : (parseInt(this.dataDetalle["operationInfo"]["unitsBox"]))
            });
          }
        } else if(this._data.viewType == 3) {
          if(caja.pallet == "PL-"+this._data.pallet && caja.caja == "Bx-"+this._data.box) {
            this.datosList.push({
              registerDate: this.dataDetalle["operationInfo"]["fechaRegistro"],
              lineName: this.dataDetalle["operationInfo"]["linea"],
              operatorName: this.dataDetalle["operationInfo"]["operator"],
              productName: this.dataDetalle["operationInfo"]["product"],
              rangeDetail : `${caja["rangoMin"]}-${caja["rangoMax"]}`,
              quantity : (parseInt(this.dataDetalle["operationInfo"]["unitsBox"]))
            });
          }
        } else {
          this.datosList.push({
            registerDate: this.dataDetalle["operationInfo"]["fechaRegistro"],
            lineName: this.dataDetalle["operationInfo"]["linea"],
            operatorName: this.dataDetalle["operationInfo"]["operator"],
            productName: this.dataDetalle["operationInfo"]["product"],
            rangeDetail : `${caja["rangoMin"]}-${caja["rangoMax"]}`,
            quantity : (parseInt(this.dataDetalle["operationInfo"]["unitsBox"]))
          });
        }
      });

        break;
      default:
        break;
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
          case "1":
            this.obtenerDatos();
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
