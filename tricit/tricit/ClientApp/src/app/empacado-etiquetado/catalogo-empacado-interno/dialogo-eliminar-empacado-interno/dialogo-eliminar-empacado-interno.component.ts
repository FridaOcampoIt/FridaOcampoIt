import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatSnackBar } from '@angular/material';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dialogo-eliminar-empacado-interno',
  templateUrl: './dialogo-eliminar-empacado-interno.component.html',
  styleUrls: ['./dialogo-eliminar-empacado-interno.component.less']
})
export class DialogoEliminarEmpacadoInternoComponent implements OnInit {

  mensaje: string = "";
  tipo: number = 0;
  overlayRef: OverlayRef;
  activoEmpacador: true;

  constructor(
    private _dialogRef: MatDialogRef<DialogoEliminarEmpacadoInternoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router
  ) {
    this.activoEmpacador = data['empacadorActual'].status;
    if(data['empacadorActual'].status)
      this.mensaje = 'No es posible eliminar el registro, porque aun esta activo u operando.'
    else
      this.mensaje = data["action"].mensaje;
    this.tipo = data['action'].tipo;
  }

  ngOnInit() {
    console.log('Data', this.data)
  }

  eliminarEmpacado() {
    let dataP: any = {
      packedId: this.data["packedId"]
    }


    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("InternalPacked/deletePacked", sessionStorage.getItem("token"), dataP).subscribe(
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
            this.eliminarEmpacado();
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

}
