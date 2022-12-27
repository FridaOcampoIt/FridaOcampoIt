import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { DialogoAgregarDireccionComponent } from './dialogo-agregar-direccion/dialogo-agregar-direccion.component';
import { DialogoEliminarDireccionComponent } from './dialogo-eliminar-direccion/dialogo-eliminar-direccion.component';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { DataServices } from '../Interfaces/Services/general.service';
import { SearchDropDownListAddressRequest, SearchDropDownListAddressResponse } from '../Interfaces/Models/AddressModels';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-direcciones-proveedor',
  templateUrl: './direcciones-proveedor.component.html',
  styleUrls: ['./direcciones-proveedor.component.less']
})
export class DireccionesProveedorComponent implements OnInit, AfterViewInit {

  data_table: any[] = [
  ]

  tiposComp: any[] = [
    { value: "Proveedor" },
    { value: "Distribuidor" },
    { value: "Empacado externo" },
    { value: "Empacado interno" },
  ]

  familyData: Array<any> = [];

  displayedColumns: string[] = ['select', 'nombre', 'tipo'];
  dataSource = new MatTableDataSource<any>(this.data_table);
  selection = new SelectionModel<any>(false, []);
  itemsPagina: number[] = enviroments.pageSize;

  actualCompany: number = parseInt(sessionStorage.getItem("company"));

  compania: string;
  companiaApliacada: string;
  familia: number;
  familiaAplicada: number;
  filtroaplicado: boolean = false;
  seleccionado: number;
  familiaSeleccionada: number;

  responseListDropDown = new SearchDropDownListAddressResponse();

  //Representa componente de paginación para la busqueda
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //Representa la instancia para el sorting de columnas
  @ViewChild(MatSort) sort: MatSort;

  overlayRef: OverlayRef;

  constructor(
    private _dialog: MatDialog,
    private _overlay: OverlayService,
    private dataService: DataServices,
    private snack: MatSnackBar,
    private _router: Router
  ) {

  }

  //Permisos
	allowAdd: boolean = sessionStorage.hasOwnProperty('Agregar Dirección de Proveedor');
	allowEdit: boolean = sessionStorage.hasOwnProperty('Editar Dirección de Proveedor');
	allowDelete: boolean = sessionStorage.hasOwnProperty('Eliminar Dirección de Proveedor');
  
  aplicarFiltro() {

    if (this.compania.length == 0) {

      this.openSnack("Seleccione al menos el tipo de compañia.", "Aceptar");
      this.filtroaplicado = false;
      return;
    }


    this.filtroaplicado = true;
    this.companiaApliacada = this.compania;
    this.familiaAplicada = this.familia;

    this.obtenerDatos(1);
  }

  obtenerFamilias() {

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    let data = {
      id: parseInt(sessionStorage.getItem("company"))
    }

    var request = new SearchDropDownListAddressRequest();
    // debugger;
    this.dataService.postData<SearchDropDownListAddressResponse>("PackedLabeled/SearchFamilyProductCombo", sessionStorage.getItem("token"), data).subscribe(
      data => {
        this.familyData = data["familiescombo"]; //Obtener las familias
        // this.obtenerDatos(1);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
      },
      error => {
        //debugger
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("error", error);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("2");
        } else {
          console.log(error);
        }
      }
    );

  }

  obtenerDatos(action = 0) {

    if (action == 0) {
      this.obtenerFamilias();
    } else {

      if (this.companiaApliacada == "") {
        return;
      }

      let data: any = {
        typeCompany: this.companiaApliacada,
        familyId: this.familiaAplicada
      };

      setTimeout(() => {
        if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
          this.overlayRef = this._overlay.open();
        }
      }, 1);

      //Consultar los registros existentes generales
      this.dataService.postData<any>("AddressProvider/SearchAddress", sessionStorage.getItem("token"), data).subscribe(
        data => {
          console.log(data, "Recibido");
          setTimeout(() => {
            this._overlay.close(this.overlayRef);
          }, 1);

          if (data["messageEsp"] != "") {
            this.openSnack(data["messageEsp"], "Aceptar");
          } else {
            this.dataSource.data = data['addressLst'];
            this.selection.clear();
          }


        },
        error => {
          setTimeout(() => {
            this._overlay.close(this.overlayRef);
          }, 1);
          console.log("error", error);
          if (error.error.hasOwnProperty("messageEsp")) {
            if (action == 0) {
              this.relogin("2");
            } else {
              this.relogin("1");
            }
          } else {
            console.log(error);
          }
        }
      );

    }


  }

  //Evento para realizar la seleccion de la tabla
  seleccion = () => {
    this.selection.selected.map(item => { this.seleccionado = item.addressId, this.familiaSeleccionada = item.FamilyId });
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return "{this.isAllSelected() ? 'select' : 'deselect'} all";
    }
    return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
  }

  agregarDireccion(action = 0) {

    let titulo: string = "Agregar";

    if (action != 0) {
      titulo = "Editar";
    }

    let _dialogRef = this._dialog.open(DialogoAgregarDireccionComponent, {
      panelClass: "dialog-aprod",
      data: { titulo: titulo, action: action, addressId: this.seleccionado, typeCompany: this.companiaApliacada, familyId: this.familiaAplicada }
    });

    _dialogRef.afterClosed().subscribe(res => {
      if (res == true) {
        this.obtenerDatos(1);
      }
    })
  }

  eliminarDireccion() {
    let _dialogRef = this._dialog.open(DialogoEliminarDireccionComponent, {
      panelClass: "dialog-aprod",
      data: { addressId: this.seleccionado }
    });

    _dialogRef.afterClosed().subscribe(res => {
      if (res == true) {
        this.obtenerDatos(1);
      }
    })
  }

  ngOnInit() {
    this.filtroaplicado = false;
    this.compania = "";
    this.familia = 0;
    this.seleccionado = 0;

    this.dataSource.sort = this.sort;
    this.obtenerFamilias();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
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
            this.obtenerDatos(1);
            break;
          case "2":
            this.obtenerFamilias();
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

}
