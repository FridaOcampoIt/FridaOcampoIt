import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { OverlayRef } from '@angular/cdk/overlay';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-agregar-linea',
  templateUrl: './dialogo-agregar-linea.component.html',
  styleUrls: ['./dialogo-agregar-linea.component.less']
})
export class DialogoAgregarLineaComponent implements OnInit {

  linForm: FormGroup;

  titulo: string = "";

  urlGuardarLinea: string = "";
  urlLineaInfo: string = "";
  companiaId: number = 0;

  overlayRef: OverlayRef;
  constructor(
    private _fb: FormBuilder,
    private _dialogRef: MatDialogRef<DialogoAgregarLineaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router
  ) {
    this.titulo = this.data["action"] ? "Editar" : "Agregar";
  }

  formGetter = (_campo) => {
    return this.linForm.get(_campo);
  }

  ngOnInit() {

    this.companiaId = parseInt(sessionStorage.getItem("company"));

    switch (this.data["tipo"]) {
      case 1:
        //interno
        this.urlGuardarLinea = "InternalPacked/SavePackedProdLine";
        this.urlLineaInfo = "InternalPacked/SearchPackedProdLinesData";
        break;
      case 2:
        //externo
        this.urlGuardarLinea = "ExternalPacked/SavePackedProdLine";
        this.urlLineaInfo = "ExternalPacked/SearchPackedProdLinesData";
        break;
      default:
        break;
    }

    this.linForm = this._fb.group({
      lineId: [0],
      name: ["", [Validators.required]],
      directionName: [""],
      packedId: [0]
    })

    this.linForm.patchValue({
      directionName: this.data["directionName"],
      packedId: this.data["packedId"]
    })

    if (this.data["action"] == 1) {
      this.linForm.patchValue({ lineId: this.data["lineId"]})
      this.obtenerDatos();
    }
  }

  obtenerDatos() {

    let data: any = {
      packedId: this.data["packedId"],
      lineId: this.data["lineId"],
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>(this.urlLineaInfo, sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"].length) {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.linForm.patchValue(data["prodLineList"][0]);
          this.linForm.patchValue({companyId: this.companiaId })
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

  guardarLinea() {
    if (!this.linForm.valid) {
      this.markFormGroupTouched(this.linForm);
      return;
    }

    let data: any = {
      prodLineData: this.linForm.value
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);


    this._dataService.postData<any>(this.urlGuardarLinea, sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(`respuesta`, data)
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (data["messageEsp"].length) {
          this.openSnack(data["messageEsp"], "Aceptar");
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

        switch (peticion) {
          case "1":
            this.obtenerDatos();
            break;
          case "2":
            this.guardarLinea();
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

  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }

}
