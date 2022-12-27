import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';
import { Router, ActivatedRoute } from '@angular/router';
import { DialogoAgregarProveedorComponent } from './dialogo-agregar-proveedor/dialogo-agregar-proveedor.component';
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component';
import { DialogoAsociarProductosComponent } from '../dialogo-asociar-productos/dialogo-asociar-productos.component';
import { DataServices } from '../../Interfaces/Services/general.service';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-catalogo-proveedores',
  templateUrl: './catalogo-proveedores.component.html',
  styleUrls: ['./catalogo-proveedores.component.less']
})
export class CatalogoProveedoresComponent implements OnInit {

  data_table: any[] = [
    { id: 1, proveedor: "nombre proveedor", razonsocial: "razon social", telefono: 3232435465 },
    { id: 2, proveedor: "nombre proveedor", razonsocial: "razon social", telefono: 3232435465 },
    { id: 3, proveedor: "nombre proveedor", razonsocial: "razon social", telefono: 3232435465 },
    { id: 4, proveedor: "nombre proveedor", razonsocial: "razon social", telefono: 3232435465 }
  ]

  displayedColumns: string[] = ['select', 'providerNumber', 'providerName', 'businessName', 'phone'];
  dataSource = new MatTableDataSource<any>(this.data_table);
  selection = new SelectionModel<any>(false, []);
  itemsPagina: number[] = enviroments.pageSize;

  proveedor: string = "";
  razonSocial: string = "";
  proId: number;
  name: string = "";
  rS: string = "";
  overlayRef: OverlayRef;
  status: boolean = false;

  //Representa componente de paginación para la busqueda
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //Representa la instancia para el sorting de columnas
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _dialog: MatDialog,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
  ) { }

  //Permisos
  allowSave: boolean = sessionStorage.hasOwnProperty('Agregar Empacado y Etiquetado');
  allowEdit: boolean = sessionStorage.hasOwnProperty('Editar Empacado y Etiquetado');
  allowDelete: boolean = sessionStorage.hasOwnProperty('Eliminar Empacado y Etiquetado');
  allowOption: boolean = sessionStorage.hasOwnProperty('Visualizar Empacado y Etiquetado');

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


  dialogoAgregarProveedor(action = 0) {
    let titulo: string = "Agregar";

    if (action != 0) {
      titulo = "Editar";
    }

    let _dialogRef = this._dialog.open(DialogoAgregarProveedorComponent, {
      panelClass: "dialog-aprod",
      data: { titulo: titulo, action: action, businessName: this.rS, providerName: this.name, providerId: this.proId }
    });

    _dialogRef.afterClosed().subscribe(res => {
      if (res == true) {
        this.obtenerDatos();
      }
    })
  }

  dialogoEliminarProveedor() {
    //hacer validaciones del estatus y weas

    let validacion = 1;
    if (this.status == true) {
      validacion = 0;
    } else {
      validacion = 1;
    }

    let action: any = {
      mensaje: validacion == 1 ? "Esta seguro de eliminar el proveedor seleccionado" : "Solo se puede eliminar proveedores con estatus Inactivo", //Si no, solo mostrar el mensaje de error,
      tipo: validacion == 1 ? 0 : 1 //alguna validación para incitar en eliminar
    }

    let _dialogRef = this._dialog.open(DialogoEliminarComponent, {
      panelClass: "dialog-aprod",
      data: { action, businessName: this.rS, providerName: this.name, providerId: this.proId }
    });

    _dialogRef.afterClosed().subscribe(res => {
      if (res == true) {
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
      data: { titulo: "Agregar", type: 1, id: this.proId }
    });

    _dialogRef.afterClosed().subscribe(ref => {
      if (ref == true) {
        if (this.proveedor != "" || this.razonSocial != "") {
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
        provider: "",
        businessName: "",
        opc: false
      }
    } else { //busqueda con filtros
      data = {
        provider: this.proveedor,
        businessName: this.razonSocial,
        opc: true
      }
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("Providers/SearchProviders", sessionStorage.getItem("token"), data).subscribe(
      data => {

        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log(data, "Recibido");

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.dataSource.data = data['providers'];
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
    this.selection.selected.map(item => { this.rS = item.businessName; this.name = item.providerName, this.status = item.status, this.proId = item.providerId });
  }

  ngOnInit() {
    this.dataSource.sort = this.sort;

    this.proveedor = "";
    this.razonSocial = "";

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
