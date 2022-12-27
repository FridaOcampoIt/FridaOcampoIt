import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog, MatSnackBar } from '@angular/material';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-eliminar',
  templateUrl: './dialogo-eliminar.component.html',
  styleUrls: ['./dialogo-eliminar.component.less']
})
export class DialogoEliminarComponent implements OnInit {

  mensaje: string = "";
  tipo: number = 0;
  providerName: string = "";
  businessName: string = "";

  overlayRef: OverlayRef;

  constructor(
    private dataService: DataServices,
    private _dialogRef: MatDialogRef<DialogoEliminarComponent>,
    private _dialog: MatDialog,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _router: Router
  ) {
    this.mensaje = data["action"].mensaje;
    this.tipo = data['action'].tipo;
    this.providerName = data["providerName"];
    this.businessName = data["businesName"];
  }

  eliminarProveedor() {

    let dataP: any = {
      providerId: this.data["providerId"]
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this.dataService.postData<any>("Providers/deleteProvider", sessionStorage.getItem("token"), dataP).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this._dialogRef.close(true);
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
            this.eliminarProveedor();
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
    })
  }

  ngOnInit() {
  }


}
