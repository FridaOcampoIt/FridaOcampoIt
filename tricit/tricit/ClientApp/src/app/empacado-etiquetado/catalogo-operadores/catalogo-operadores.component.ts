import { Component, OnInit } from '@angular/core';
import { MatDialog, MatSnackBar } from '@angular/material';
import { DialogoAgregarOperadorComponent } from './dialogo-agregar-operador/dialogo-agregar-operador.component';
import { DialogoEliminarOperadorComponent } from './dialogo-eliminar-operador/dialogo-eliminar-operador.component';
import { Router, ActivatedRoute } from '@angular/router';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { DataServices } from '../../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-catalogo-operadores',
  templateUrl: './catalogo-operadores.component.html',
  styleUrls: ['./catalogo-operadores.component.less']
})
export class CatalogoOperadoresComponent implements OnInit {

  OperadoresList: any[] = [
    // { id: 1, nombre: "Raul Flores Hernandes", img: "https://picsum.photos/300" },
    // { id: 1, nombre: "Raul Flores Hernandes", img: "https://picsum.photos/400" },
    // { id: 1, nombre: "Raul Flores Hernandes", img: "https://picsum.photos/500" },
    // { id: 1, nombre: "Raul Flores Hernandes", img: "https://picsum.photo  s/300" },
    // { id: 1, nombre: "Raul Flores Hernandes", img: "https://picsum.photos/600" },
    // { id: 1, nombre: "Raul Flores Hernandes", img: "https://picsum.photos/300" },
    // { id: 1, nombre: "Raul Flores Hernandes", img: "https://picsum.photos/300" },
  ]

  direccionList: any[] = [
  ]

  empresa: string = "";
  /**
   * 1 empacado interno, 2 empacado externo
   */
  proviene: number = 0;
  direction;
  directionName: string = "";
  directionId: number = 0;
  empacadorId: number = 0;
  state: any = {};
  tipoCompany:  string = "";

  urlDirecciones: string;
  urlOperadores: string;
  urlOperadorInfo: string;
  urlGuardarOperador: string;
  urlEliminarOperador: string;

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
    this.empresa = "ALEN";

    this.$route.params.forEach(param =>
      this.fkEmpacadorId = Number(param.id)
    );
  }

  ngOnInit() {

    this.proviene = parseInt(this._route.snapshot.paramMap.get('proviene'), 10);
    this.empacadorId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
    this.empresa = this._route.snapshot.paramMap.get('name');
    this.directionId = 0;

    if (this.proviene == undefined || this.empacadorId == undefined || this.proviene == 0 || this.empacadorId == 0 || isNaN(this.empacadorId)) {
      this._router.navigateByUrl('EmpacadoEtiquetado');
    }

    switch (this.proviene) {
      case 1:
        this.tipoCompany = "Empacado interno"; //interno
        this.urlDirecciones = "AddressProvider/SearchAddressCombo";
        this.urlOperadores = "InternalPacked/SearchPackedOperators";
        this.urlOperadorInfo = "InternalPacked/SearchPackedOperatorsData";
        this.urlGuardarOperador = "InternalPacked/SavePackedOperator";
        this.urlEliminarOperador = "InternalPacked/deletePackedOperator";
        break;
      case 2:
        this.tipoCompany = "Empacado externo"; //externo
        this.urlDirecciones = "AddressProvider/SearchAddressCombo";
        this.urlOperadores = "ExternalPacked/SearchPackedOperators";
        this.urlOperadorInfo = "ExternalPacked/SearchPackedOperatorsData";
        this.urlGuardarOperador = "ExternalPacked/SavePackedOperator";
        this.urlEliminarOperador = "ExternalPacked/deletePackedOperator";
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
      case 2: //empacado externo
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

  obtenerOperadores(valor: object = {}) {


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

    this._dataService.postData<any>(this.urlOperadores, sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"].length) {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.OperadoresList = [];
          this.OperadoresList = data["operatorList"];
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

  agregarOperador(action = 0, operatorId = 0) {


    if (this.directionId == 0) {
      this.openSnack("Debe seleccionar una dirección.", "Aceptar");
      return;
    }

    let _dialogRef = this._dialog.open(DialogoAgregarOperadorComponent, {
      panelClass: "dialog-aprod",
      data: { directionId: this.directionId, packedId: this.empacadorId, tipo: this.proviene, action: action, operatorId: operatorId, directionName: this.directionName }
    });

    _dialogRef.afterClosed().subscribe(reg => {
      if (reg == true) {
        this.obtenerOperadores();
      }
    })
  }

  eliminarOperador(operatorId = 0) {

    if (operatorId == 0) {
      return;
    }

    let _dialogRef = this._dialog.open(DialogoEliminarOperadorComponent, {
      panelClass: "dialog-aprod",
      data: { operatorId: operatorId, proviene: this.proviene }
    });

    _dialogRef.afterClosed().subscribe(reg => {
      if (reg == true) {
        this.obtenerOperadores();
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
            this.obtenerOperadores();
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
