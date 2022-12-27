import { Component, OnInit, Inject } from '@angular/core';
import { OverlayRef } from '@angular/cdk/overlay';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-eliminar-linea',
  templateUrl: './dialogo-eliminar-linea.component.html',
  styleUrls: ['./dialogo-eliminar-linea.component.less']
})
export class DialogoEliminarLineaComponent implements OnInit {

  overlayRef: OverlayRef;

  urlEliminarLinea: string;
  proviene: number = 0;

  constructor(
    private _dialogRef: MatDialogRef<DialogoEliminarLineaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router
  ) { }

  ngOnInit() {

    this.proviene = this.data["proviene"];

    switch (this.proviene) {
      case 1:
        this.urlEliminarLinea = "InternalPacked/deletePackedProdLine";
        break;
      case 2:
        this.urlEliminarLinea = "ExternalPacked/deletePackedProdLine";
        break;
      default:
        break;
    }

  }

  eliminarLinea() {
    let dataP: any = {
      lineId: this.data["lineId"]
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);


    this._dataService.postData<any>(this.urlEliminarLinea, sessionStorage.getItem("token"), dataP).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (data["messageEsp"].length) {
          this.openSnack("Error en la solicitud", "Aceptar");
          console.log("error delete", data);
        } else {
          console.log("success delete", data);
          this._dialogRef.close(true);
        }

      },
      error => {
        console.log("error delete", error);
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

  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
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
            this.eliminarLinea();
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

}
