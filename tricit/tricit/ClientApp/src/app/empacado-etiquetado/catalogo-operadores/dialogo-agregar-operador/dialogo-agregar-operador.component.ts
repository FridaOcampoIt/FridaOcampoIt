import { Component, OnInit, Inject } from '@angular/core';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { OverlayRef } from '@angular/cdk/overlay';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-agregar-operador',
  templateUrl: './dialogo-agregar-operador.component.html',
  styleUrls: ['./dialogo-agregar-operador.component.less']
})
export class DialogoAgregarOperadorComponent implements OnInit {

  opForm: FormGroup;

  titulo: string = "";

  overlayRef: OverlayRef;
  directionName: string;
  image: any;
  companiaId: number = 0;

  urlGuardarOperador: string = "";;
  urlOperadorInfo: string = "";
  constructor(
    private _fb: FormBuilder,
    private _dialogRef: MatDialogRef<DialogoAgregarOperadorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router
  ) {
    this.titulo = this.data["action"] ? "Editar" : "Agregar";
  }

  formGetter = (_campo) => {
    return this.opForm.get(_campo);
  }

  ngOnInit() {
    this.companiaId = parseInt(sessionStorage.getItem("company"));

    switch (this.data["tipo"]) {
      case 1:
        //interno
        this.urlGuardarOperador = "InternalPacked/SavePackedOperator";
        this.urlOperadorInfo = "InternalPacked/SearchPackedOperatorsData";
        break;
      case 2:
        //externo
        this.urlGuardarOperador = "ExternalPacked/SavePackedOperator";
        this.urlOperadorInfo = "ExternalPacked/SearchPackedOperatorsData";
        break;
      default:
        break;
    }


    this.directionName = this.data["directionName"];

    this.opForm = this._fb.group({
      operatorId: [0],
      code: ["", [Validators.required]],
      name: ["", [Validators.required]],
      image: [""],
      imageName: [""],
      directionName: [this.data["directionName"]],
      packedId: [this.data["packedId"]]
    })

    this.opForm.patchValue({
      directionName: this.data["directionName"],
      addressId: this.data["directionId"],
      packedId: this.data["packedId"],
      companyId: this.companiaId
    })

    if (this.data["action"] == 1) {
      this.opForm.patchValue({ operatorId: this.data["operatorId"], companyId: this.companiaId })
      this.obtenerDatos();
    }
  }

  obtenerDatos() {

    let data: any = {
      packedId: this.data["packedId"],
      operatorId: this.data["operatorId"],
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>(this.urlOperadorInfo, sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"].length) {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.opForm.patchValue(data["operatorList"][0]);
          this.opForm.patchValue({companyId: this.companiaId })
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

  //Funcion para la imagen de familia
  ImagenFamilia(files) {
    if (files.length == 0) {
      return;
    }

    if((files[0].size / 1024 / 1024) >= 1 ) {
      this.openSnack("El tamaño debe ser menor a 1 MB", "Aceptar"); return;
    }
    if(files[0].name.split('.').pop().toLowerCase() != "jpg" && files[0].name.split('.').pop().toLowerCase() != "jpeg" && files[0].name.split('.').pop().toLowerCase() != "png") {
      this.openSnack("La imágen debe ser jpg, jpeg o png", "Aceptar"); return;
    }
    
    var archivos = files[0];

    this.getBase64(archivos).then(
      data => {

        // this.opForm.patchValue({ image: data.toString(), imageName: files[0].name });
        this.opForm.patchValue({ imageName: files[0].name });
        this.opForm.patchValue({ image: data.toString() });
      }
    );
  }

  //Funcion para transformar la imagen en base 64
  getBase64(file) {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = error => reject(error);
    });
  }

  guardarOperador() {
    if (!this.opForm.valid) {
      this.markFormGroupTouched(this.opForm);
      return;
    }

    let data: any = {
      operatorData: this.opForm.value
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);


    this._dataService.postData<any>(this.urlGuardarOperador, sessionStorage.getItem("token"), data).subscribe(
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
          this.relogin("2")
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
            this.guardarOperador();
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
