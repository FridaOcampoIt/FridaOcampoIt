import { OverlayRef } from '@angular/cdk/overlay';
import { Component, Inject, OnInit } from '@angular/core';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';

//declare var BrowserPrint: any;

@Component({
  selector: 'app-dialogo-unir',
  templateUrl: './dialogo-unir.component.html',
  styleUrls: ['./dialogo-unir.component.less']
})
export class DialogoUnirComponent implements OnInit {

  constructor(
    private _router: Router,
    private _dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dialogRef: MatDialogRef<DialogoUnirComponent>,
    @Inject(MAT_DIALOG_DATA) public _data: any,
  ) { }

  unionName: string = "";
  selected_device: any;
  devices: [];
  printerList: any[] = [];
  selectedPrinter: number = 0;
  flag: number = 1; //Por default aparecera la info para union 
  medianilX: number = 20;
  medianilY: number = 20;

  overlayRef: OverlayRef;
  agrupations: number[] = [];

  ngOnInit() {
    //this.setup(this.printerList);
    this.flag = this._data["Tipo"];
    this.printerList = this._data["Devices"];
    console.log("data: ", this._data["Tipo"]);

    //Si _data trae info desde Union considerar agregarla al if con this.flag == 1 en caso contrario, se esta llamando 
    //este modal desde el boton de reimprimir, para ese caso es this.flag == 2... Saludos
    if(this.flag == 1){
      this._data["agrupaciones"].forEach(opera => {
        this.agrupations.push(opera.groupingId);
      });
    }
  }

  /*setup(list)
  {
    //Get the default device from the application as a first step. Discovery takes longer to complete.
    BrowserPrint.getDefaultDevice("printer", function(device)
        {       
          //Discover any other devices available to the application
          BrowserPrint.getLocalDevices(function(device_list){
            for(var i = 0; i < device_list.length; i++)
            {
              list.push({name: device_list[i].name, uid: device_list[i].uid})
            }
          }, function(){console.log("Error getting local devices")},"printer");
          
        }, function(error){
          console.log(error);
        })
  }*/

  accionBoton() {
    if (this.flag == 1) {//Si se esta haciendo una union
      this.unirSeleccionados();
    } else { //Si solo es seleccionar la impresora
      this._dialogRef.close({ result: true, printerId: this.selectedPrinter, x: this.medianilX, y:this.medianilY });
    }
  }
  unirSeleccionados() {
    console.log("union");
    if (this.unionName.trim().length == 0) {
      this.openSnack("Ingrese un nombre para la únion.", "Aceptar");
      return;
    }

    let datos: any = {
      groupingPId: this._data["agrupacion1"],
      groupingSId: this._data["agrupacion2"],
      groupings: this.agrupations,
      groupingType: this._data["tipoVista"],
      groupingName: this.unionName
    };

    setTimeout(() => {
      if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);
    //debugger
    this._dataService.postData<any>("ExternalPacked/SaveUnionOperations", sessionStorage.getItem("token"), datos).subscribe(
      data => {
        //  
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          if (this.medianilY <= 0 || this.medianilY.toString().length == 0) {
            this.medianilY = 20;
          }

          if (this.medianilX <= 0 || this.medianilX.toString().length == 0) {
            this.medianilX = 20;
          }

          this._dialogRef.close({ result: true, id: data["groupingId"], printerId: this.selectedPrinter });
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
          this.openSnack("Hubo un error en la petición.", "Aceptar");
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
            this.unirSeleccionados();
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
