import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { DataServices } from '../../../../Interfaces/Services/general.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../../../Interfaces/Services/overlay.service';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-eliminar',
  templateUrl: './dialogo-eliminar.component.html',
  styleUrls: ['./dialogo-eliminar.component.less']
})
export class DialogoEliminarConfiguracionComponent implements OnInit {

  overlayRef: OverlayRef;

  constructor(
    private _dialogRef: MatDialogRef<DialogoEliminarConfiguracionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router
  ) { }

  ngOnInit() {
  }

  eliminar() {
    let dataP: any = {
      packagingId: this.data["embalajeId"]
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);


    this._dataService.postData<any>("Families/deletePackaging", sessionStorage.getItem("token"), dataP).subscribe(
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
            this.eliminar();
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
