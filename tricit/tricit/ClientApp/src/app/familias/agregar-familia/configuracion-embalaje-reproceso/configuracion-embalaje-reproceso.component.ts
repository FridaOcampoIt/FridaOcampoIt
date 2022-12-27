import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTableDataSource, MatDialog, MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../../../Interfaces/Enviroments/enviroments';
import { DialogoAgregarConfiguracionReprocesoComponent } from './dialogo-agregar-reproceso/dialogo-agregar-reproceso.component';
import { DialogoEliminarConfiguracionReprocesoComponent } from './dialogo-eliminar-reproceso/dialogo-eliminar-reproceso.component';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-configuracion-embalaje-reproceso',
  templateUrl: './configuracion-embalaje-reproceso.component.html',
  styleUrls: ['./configuracion-embalaje-reproceso.component.less']
})
export class ConfiguracionEmbalajeReprocesoComponent implements OnInit {

  familiaId: number = 0;
  embalajeId: number = 0;

  displayedColumns: string[] = ['select', 'packagingType', 'readingType', 'unitsPerBox', 'copiesPerBox', 'boxesPerPallet', 'copiesPerPallet'];
  dataSource = new MatTableDataSource<any>([{ id: 1, tempaque: 1, tlectura: 1, ucaja: 1, copias: 1, upallet: 1, copias2: 1 }]);
  selection = new SelectionModel<any>(false, []);
  itemsPagina: number[] = enviroments.pageSize;

  //Representa componente de paginación para la busqueda
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //Representa la instancia para el sorting de columnas
  @ViewChild(MatSort) sort: MatSort;

  overlayRef: OverlayRef;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _dialog: MatDialog,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
  ) {
    this.familiaId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
  }

  //Permisos
  allowSave: boolean = sessionStorage.hasOwnProperty('Agregar Configuración de Embalaje');
  allowEdit: boolean = sessionStorage.hasOwnProperty('Editar Configuración de Embalaje');
  allowDelete: boolean = sessionStorage.hasOwnProperty('Eliminar Configuración de Embalaje');

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return "{this.isAllSelected() ? 'select' : 'deselect'} all";
    }
    return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
  }

  back = () => {
    this._router.navigateByUrl('Familias/CatalogoFamilias', { state: { familyId: this.familiaId } })
    // this._location.back();
  }

  /**
   * action = 0 -> Nuevo,
   * action = 1 -> Edición
   */
  dialogoAgregar(action = 0) {

    if (action == 0) {
      this.embalajeId = 0;
    }

    const _dialogRef = this._dialog.open(DialogoAgregarConfiguracionReprocesoComponent, {
      panelClass: 'dialog-aprod',
      disableClose: true,
      data: { familyId: this.familiaId, embalajeId: this.embalajeId, action: action }
    })

    /**
     * Obtener los datos del multiple seleccionado o setearlos, dependiendo como lo quieran programar
     */
    _dialogRef.afterClosed().subscribe(res => {
      if (res == true) {
        this.obtenerDatos();
      }
    })
  }

  dialogoEliminar() {
    const _dialogRef = this._dialog.open(DialogoEliminarConfiguracionReprocesoComponent, {
      panelClass: 'dialog-aprod',
      disableClose: false,
      data: { embalajeId: this.embalajeId }
    })

    _dialogRef.afterClosed().subscribe(res => {
      if (res) {
        this.obtenerDatos();
      }
    })
  }

  obtenerDatos(action = 0) {
    //  
    let data: any = {};
    //Busqueda general
    data = {
      familyId: this.familiaId,
      packagingId: 0
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("Families/getPackagingReprocess", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log(data, "Recibido");
        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.dataSource.data = data['packagingList'];
          this.selection.clear();
        }

      },
      error => {

        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("1")
        } else {
          console.log(error);
        }
        console.log(error, "error");
      }
    );

  }

  seleccion = () => {
    this.selection.selected.map(item => {
      this.embalajeId = item.packagingId
    }
    );
  }

  ngOnInit() {
    this.dataSource.sort = this.sort;
    this.embalajeId = 0;

    if (this.familiaId > 0) {
      this.obtenerDatos();
    } else {
      this._router.navigate(['/Familias/CatalogoFamilias']);
    }

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
