import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';
import { Router, ActivatedRoute } from '@angular/router';
import { OverlayRef } from '@angular/cdk/overlay';
import { DataServices } from '../../Interfaces/Services/general.service';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-catalogo-empacado-agro',
  templateUrl: './catalogo-empacado-agro.component.html',
  styleUrls: ['./catalogo-empacado-agro.component.less']
})
export class CatalogoEmpacadoAgroComponent implements OnInit {

  data_table: any[] = [
  ]

  displayedColumns: string[] = ['select', 'producerNumber', 'producerName', 'ranch', 'address'];
  dataSource = new MatTableDataSource<any>(this.data_table);
  selection = new SelectionModel<any>(false, []);
  itemsPagina: number[] = enviroments.pageSize;

  overlayRef: OverlayRef;
  status: boolean = true;
  packId: number;
  empacadoAgro: string;
  empacadoName: string = "";
  empacadoNumber: string = "";

  //Representa componente de paginación para la busqueda
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //Representa la instancia para el sorting de columnas
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _dialog: MatDialog,
    private _dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar
  ) { }

  //Permisos
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

  obtenerDatos(action = 0) {
    let data: any = {};

    //Busqueda general
    data = {
      producerNumber: this.empacadoAgro
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("AgroPacked/SearchAgroPacked", sessionStorage.getItem("token"), data).subscribe(
      data => {
        this._overlay.close(this.overlayRef);
        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.dataSource.data = data['producerList'];
          this.selection.clear();
        }
      },
      error => {
        this._overlay.close(this.overlayRef);

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
    this.selection.selected.map(item => { this.packId = item.producerId, this.empacadoName = item.ranch, this.empacadoNumber = item.producerNumber });
  }

  catalogoEmbalajeReproceso() {
    this._router.navigateByUrl(`EmpacadoEtiquetado/EmpacadoAgro/ReporteEmbalajeReproceso/${this.packId}/${this.empacadoNumber}/${this.empacadoName}`, { queryParams: { proviene: 2, id: this.packId, name: this.empacadoName }, state: { proviene: 2, packedId: this.packId, name: this.empacadoName } });
  }

  ngOnInit() {
    this.dataSource.sort = this.sort;
    this.empacadoAgro = "";

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
