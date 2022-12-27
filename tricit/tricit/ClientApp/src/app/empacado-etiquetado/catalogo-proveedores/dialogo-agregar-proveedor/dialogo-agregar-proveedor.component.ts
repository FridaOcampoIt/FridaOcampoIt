import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { enviroments } from '../../../Interfaces/Enviroments/enviroments';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef, MatSnackBar } from '@angular/material';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-agregar-proveedor',
  templateUrl: './dialogo-agregar-proveedor.component.html',
  styleUrls: ['./dialogo-agregar-proveedor.component.less']
})
export class DialogoAgregarProveedorComponent implements OnInit {

  provForm: FormGroup;
  regExMail: RegExp = RegExp(enviroments.patterns.email);
  regExNum: RegExp = RegExp(enviroments.patterns.numerical);

  titulo: string = "";
  overlayRef: OverlayRef;

  constructor(
    private _fb: FormBuilder,
    private dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dialogRef: MatDialogRef<DialogoAgregarProveedorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _router: Router
  ) { }

  formGetter = (_campo) => {
    return this.provForm.get(_campo);
  }

  ngOnInit() {
    this.provForm = this._fb.group({
      providerId: [0],
      status: [true],
      providerNumber: ["", [Validators.required]],
      providerName: ["", [Validators.required]],
      businessName: [""],
      email: ["", [Validators.pattern(this.regExMail)]],
      phone: ["", [Validators.pattern(this.regExNum)]]
    })

    this.provForm.patchValue({ status: true })

    if (this.data["action"] != 0) {
      this.obtenerDatos();
    }

    this.titulo = this.data["titulo"];
  }

  obtenerDatos() {
    let data: any = {
      providerId: this.data['providerId']
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this.dataService.postData<any>("Providers/SearchProviderData", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log("data", data);

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.provForm.patchValue(data["providers"][0]);
        }

        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
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

  guardarProveedor() {
    if (!this.provForm.valid) {
      this.markFormGroupTouched(this.provForm);
      return;
    }

    let data: any = {
      providerData: this.provForm.value
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this.dataService.postData<any>("Providers/SaveProvider", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(`respuesta`, data);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (data["messageEsp"] != "") {
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
          case "2":
            this.guardarProveedor();
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

  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }

}
