import { Component, OnInit } from '@angular/core';
import { DialogoAgregarLineaComponent } from './dialogo-agregar-linea/dialogo-agregar-linea.component';
import { DialogoEliminarLineaComponent } from './dialogo-eliminar-linea/dialogo-eliminar-linea.component';
import { MatDialog, MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { DataServices } from '../../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-catalogo-lineas-produccion',
  templateUrl: './catalogo-lineas-produccion.component.html',
  styleUrls: ['./catalogo-lineas-produccion.component.less']
})
export class CatalogoLineasProduccionComponent implements OnInit {

  lineasList: any[] = [

  ]

  direccionList: any[] = [
  ]

  empresa: string = "";
  /**
   * 1 empacado interno, 2 empacado externo
   */
  proviene: number = 0;
  directionId: number = 0;
  empacadorId: number = 0;
  direction;
  directionName: string = "";
  state: any = {};
  tipoCompany:  string = "";



  urlDirecciones: string;
  urlLineas: string;
  urlLineaInfo: string;
  urlGuardarLinea: string;
  urlEliminarLinea: string;

  overlayRef: OverlayRef;

  fkEmpacadorId: number =  0;
  constructor(
    private _dialog: MatDialog,
    private _route: ActivatedRoute,
    private _router: Router,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private $route: ActivatedRoute
  ) {
    this.empresa = "";
    this.$route.params.forEach(param =>
      this.fkEmpacadorId = Number(param.id)
    );
  }

  ngOnInit() {
    this.state = window.history.state;
    this.proviene = parseInt(this._route.snapshot.paramMap.get('proviene'), 10);
    this.empacadorId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
    this.empresa = this._route.snapshot.paramMap.get('name');


    if (this.proviene == undefined || this.empacadorId == undefined || this.proviene == 0 || this.empacadorId == 0) {
      this._router.navigateByUrl('EmpacadoEtiquetado');
    }

    switch (this.proviene) {
      case 1:
        this.tipoCompany = "Empacado interno"; //interno
        this.urlDirecciones = "AddressProvider/SearchAddressCombo";
        this.urlLineas = "InternalPacked/SearchPackedProdLines";
        this.urlLineaInfo = "InternalPacked/SearchPackedProdLinesData";
        this.urlGuardarLinea = "InternalPacked/SavePackedProdLine";
        this.urlEliminarLinea = "InternalPacked/deletePackedProdLine";
        break;
      case 2:
        this.tipoCompany = "Empacado externo"; //externo
        this.urlDirecciones = "AddressProvider/SearchAddressCombo";
        this.urlLineas = "ExternalPacked/SearchPackedProdLines";
        this.urlLineaInfo = "ExternalPacked/SearchPackedProdLinesData";
        this.urlGuardarLinea = "ExternalPacked/SavePackedProdLine";
        this.urlEliminarLinea = "ExternalPacked/deletePackedProdLine";
        break;
      default:
        break;
    }

    this.obtenerDirecciones();
  }

  back = () => {
    switch (this.proviene) {
      case 1: //regresar a empacado interno
        this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoInterno', { state: { registro: 1 } })
        break;
      case 2:
        this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoExterno', { state: { registro: 1 } })
        break;
      default:
        this._router.navigateByUrl('EmpacadoEtiquetado')
        break;
    }
    // this._location.back();
  }

  obtenerDirecciones() {

    
    let data: any = {
      empacadorId: this.fkEmpacadorId
    };


    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>(this.urlDirecciones, sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.direccionList = data["addressComboLst"];
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

  obtenerLineas(valor: object = {}) {

    if (valor["id"]) {
      this.directionId = valor["id"];
      this.directionName = valor["data"];
    }

    let data: any = {
      packedId: this.empacadorId,
      addressId: this.directionId
    }

    setTimeout(() => {
      if (!this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);

    this._dataService.postData<any>(this.urlLineas, sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"].length) {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.lineasList = [];
          this.lineasList = data["prodLineList"];
        }
      },
      error => {
        console.log("error", error);
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

  agregarLinea(action = 0, lineId = 0) {

    if (this.directionId == 0) {
      this.openSnack("Debe seleccionar una dirección.", "Aceptar");
      return;
    }

    let _dialogRef = this._dialog.open(DialogoAgregarLineaComponent, {
      panelClass: "dialog-aprod",
      data: { packedId: this.empacadorId, tipo: this.proviene, action: action, lineId: lineId, directionName: this.directionName }
    });

    _dialogRef.afterClosed().subscribe(reg => {
      if (reg == true) {
        this.obtenerLineas();
      }
    })
  }

  eliminarLinea(lineId = 0) {
    if (lineId == 0) {
      return;
    }

    let _dialogRef = this._dialog.open(DialogoEliminarLineaComponent, {
      panelClass: "dialog-aprod",
      data: { lineId: lineId, proviene: this.proviene }
    });

    _dialogRef.afterClosed().subscribe(reg => {
      if (reg == true) {
        this.obtenerLineas();
      }
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
            this.obtenerDirecciones();
            break;
          case "2":
            this.obtenerLineas();
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
