import { Component, OnInit } from '@angular/core';
import { MatDialog, MatSnackBar } from '@angular/material';
import { DialogoEliminarEtiquetaComponent } from './dialogo-eliminar-etiqueta/dialogo-eliminar-etiqueta.component';
import { Router, ActivatedRoute } from '@angular/router';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { DataServices } from '../Interfaces/Services/general.service';
import { isNull } from 'util';
import { LoginUserResponse, LoginUserRequest } from '../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-etiquetas',
  templateUrl: './etiquetas.component.html',
  styleUrls: ['./etiquetas.component.less']
})
export class EtiquetasComponent implements OnInit {

  etiquetasLList: any[] = [
    // { id: 1, nombre: "Etiqueta 1", img: "https://picsum.photos/300" },
    // { id: 1, nombre: "Etiqueta 2", img: "https://picsum.photos/400" },
    // { id: 1, nombre: "Etiqueta 3", img: "https://picsum.photos/500" },
    // { id: 1, nombre: "Etiqueta 4", img: "https://picsum.photos/300" },
    // { id: 1, nombre: "Etiqueta 5", img: "https://picsum.photos/600" },
    // { id: 1, nombre: "Etiqueta 6", img: "https://picsum.photos/300" },
    // { id: 1, nombre: "Etiqueta 7", img: "https://picsum.photos/300" },
  ]

  overlayRef: OverlayRef;

  constructor(
    private _dialog: MatDialog,
    private _route: ActivatedRoute,
    private _router: Router,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices
  ) { }

  //Permisos
  allowSave: boolean = sessionStorage.hasOwnProperty('Agregar Etiquetas');
  allowEdit: boolean = sessionStorage.hasOwnProperty('Editar Etiquetas');
  allowDelete: boolean = sessionStorage.hasOwnProperty('Eliminar Etiquetas');

  ngOnInit() {

    this.obtenerEtiquetas();
  }

  obtenerEtiquetas() {

    let data: any = {
      opc: 0
    };

    if (sessionStorage.getItem("company")) {
      data["name"] = sessionStorage.getItem("company");
      data["opc"] = 4;
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);
    //debugger
    this._dataService.postData<any>("LabelsQR/SearchLabels", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.etiquetasLList = data["labelsqr"];
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

  dialogoEliminarEtiqueta(labelId = 0) {
    if (labelId == 0) {
      return;
    }

    let _dialogRef = this._dialog.open(DialogoEliminarEtiquetaComponent, {
      panelClass: "dialog-aprod",
      data: { labelId: labelId }
    })

    _dialogRef.afterClosed().subscribe(reg => {
      if (reg == true) {
        this.obtenerEtiquetas();
      }
    })
  }

  agregarFormato(labelId = 0, action = 0) {
    this._router.navigateByUrl('Etiquetas/AgregarFormato', { state: { registro: 1, labelId: labelId, action: action } })
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
            this.obtenerEtiquetas();
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
