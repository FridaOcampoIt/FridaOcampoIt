import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';
import { Router, ActivatedRoute } from '@angular/router';
import { DialogoEliminarDistribuidorComponent } from './dialogo-eliminar-distribuidor/dialogo-eliminar-distribuidor.component';
import { DialogoAgregarDistribuidorComponent } from './dialogo-agregar-distribuidor/dialogo-agregar-distribuidor.component';
import { DialogoAsociarProductosComponent } from '../dialogo-asociar-productos/dialogo-asociar-productos.component';
import { DataServices } from '../../Interfaces/Services/general.service';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { LoginUserResponse, LoginUserRequest } from '../../Interfaces/Models/LoginModels';


@Component({
  selector: 'app-catalogo-distribuidores',
  templateUrl: './catalogo-distribuidores.component.html',
  styleUrls: ['./catalogo-distribuidores.component.less']
})
export class CatalogoDistribuidoresComponent implements OnInit {

  data_table: any[] = [
    { id: 1, distribuidor: "nombre distribuidor", razonsocial: "razon social", telefono: 3232435465 },
    { id: 2, distribuidor: "nombre distribuidor", razonsocial: "razon social", telefono: 3232435465 },
    { id: 3, distribuidor: "nombre distribuidor", razonsocial: "razon social", telefono: 3232435465 },
    { id: 4, distribuidor: "nombre distribuidor", razonsocial: "razon social", telefono: 3232435465 }
  ]

  displayedColumns: string[] = ['select', 'distributorNumber', 'distributorName', 'businessName', 'phone'];
  dataSource = new MatTableDataSource<any>(this.data_table);
  selection = new SelectionModel<any>(false, []);
  itemsPagina: number[] = enviroments.pageSize;

  //Representa componente de paginación para la busqueda
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //Representa la instancia para el sorting de columnas
  @ViewChild(MatSort) sort: MatSort;

  distribuidor: string = "";
  razonSocial: string = "";

  disId: number;
  business: string;
  disName: string;
  status: boolean = true;

  overlayRef: OverlayRef;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _dialog: MatDialog,
    private _dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar
  ) {
    this.distribuidor = "";
    this.razonSocial = "";
  }

  //Permisos
  allowSave: boolean = sessionStorage.hasOwnProperty('Agregar Empacado y Etiquetado');
  allowEdit: boolean = sessionStorage.hasOwnProperty('Editar Empacado y Etiquetado');
  allowDelete: boolean = sessionStorage.hasOwnProperty('Eliminar Empacado y Etiquetado');

  back = () => {
    this._router.navigateByUrl('EmpacadoEtiquetado', { state: { registro: 1 } })
    // this._location.back();
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return "{this.isAllSelected() ? 'select' : 'deselect'} all";
    }
    return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
  }


  dialogoAgregarDistribuidor(action = 0) {
    let titulo: string = "Agregar";

    if (action != 0) {
      titulo = "Editar";
    }

    let _dialogRef = this._dialog.open(DialogoAgregarDistribuidorComponent, {
      panelClass: "dialog-aprod",
      data: { titulo: titulo, action: action, businessName: this.business, distributorName: this.disName, distributorId: this.disId }
    });

    _dialogRef.afterClosed().subscribe(res => {
      if (res == true) {
        this.obtenerDatos();
      }
    })
  }

  dialogoEliminarDistribuidor() {
    //hacer validaciones del estatus y weas
    let validacion = 1;
    if (this.status == true) {
      validacion = 0;
    } else {
      validacion = 1;
    }

    let action: any = {
      mensaje: validacion == 1 ? "Esta seguro de eliminar el distribuidor seleccionado?" : "Solo se puede eliminar distribuidores con estatus Inactivo", //Si no, solo mostrar el mensaje de error,
      tipo: validacion == 1 ? 0 : 1 //alguna validación para incitar en eliminar
    }

    let _dialogRef = this._dialog.open(DialogoEliminarDistribuidorComponent, {
      panelClass: "dialog-aprod",
      data: { action, businessName: this.business, distributorName: this.disName, distributorId: this.disId }
    });

    _dialogRef.afterClosed().subscribe(result => {

      if (result == true) {
        console.log("eliminao");
        this.obtenerDatos();
      } else {
        console.log("no eliminao");
      }
    })
  }

  dialogoAsociarProductos() {
    let _dialogRef = this._dialog.open(DialogoAsociarProductosComponent, {
      panelClass: "dialog-aprod",
      data: { titulo: "Agregar", type: 0, id: this.disId }
    });

    _dialogRef.afterClosed().subscribe(ref => {
      if (ref == true) {
        if (this.distribuidor != "" || this.razonSocial != "") {
          this.obtenerDatos(1);
        } else {
          this.obtenerDatos();
        }
      }
    })
  }

  obtenerDatos(action = 0) {

    let data: any = {};

    //Busqueda general
    if (action == 0) {
      data = {
        distributor: "",
        bussinesName: "",
        opc: false
      }
    } else { //busqueda con filtros
      data = {
        distributor: this.distribuidor,
        businessName: this.razonSocial,
        opc: true
      }
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("Distributors/SearchDistributors", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(data, "Recibido");
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.dataSource.data = data['distributors'];
          this.selection.clear();
        }

      },
      error => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (error.error.hasOwnProperty("messageEsp")) {
          if (action == 0) {
            this.relogin("1")
          } else if (action == 1) {
            this.relogin("2")
          }
        } else {
          console.log(error);
        }
      }
    );

  }

  //Evento para realizar la seleccion de la tabla
  seleccion = () => {

    this.selection.selected.map(item => {
      this.business = item.businessName; this.disName = item.distributorName, this.status = item.status, this.disId = item.distributorId
    });
  }

  ngOnInit() {
    this.dataSource.sort = this.sort;

    this.razonSocial = "";
    this.distribuidor = "";

    this.obtenerDatos();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
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
            this.obtenerDatos(1);
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
