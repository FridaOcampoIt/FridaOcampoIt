import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { DialogoAgregarEmpacadoComponent } from './dialogo-agregar-empacado/dialogo-agregar-empacado.component';
import { DialogoEliminarEmpacadoExternoComponent } from './dialogo-eliminar-empacado-externo/dialogo-eliminar-empacado-externo.component';
import { DialogoDireccionEmpacadoComponent } from '../dialogo-direccion-empacado/dialogo-direccion-empacado.component';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';
import { Router, ActivatedRoute } from '@angular/router';
import { DialogoAsociarProductosComponent } from '../dialogo-asociar-productos/dialogo-asociar-productos.component';
import { OverlayRef } from '@angular/cdk/overlay';
import { DataServices } from '../../Interfaces/Services/general.service';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';


//DataServices and Models
import {
  SearchCompanyRequest,
  SearchCompanyResponse,
  CompaniesData
} from '../../Interfaces/Models/CompanyModels';
@Component({
  selector: 'app-catalogo-empacado-externo',
  templateUrl: './catalogo-empacado-externo.component.html',
  styleUrls: ['./catalogo-empacado-externo.component.less']
})
export class CatalogoEmpacadoExternoComponent implements OnInit {
  selectedValue: any;
    searchTxt: any;
    @ViewChild('search') searchElement: ElementRef;
    doSomething(any){
        setTimeout(()=>{ // this will make the execution after the above boolean has changed
            this.searchElement.nativeElement.focus();
        },0);  
    }
  data_table: any[] = [
  ]

  displayedColumns: string[] = ['select', 'packedNumber', 'packedName', 'company'];
  displayedColumnsCompany: string[] = ['select', 'packedNumber', 'packedName', 'merma'];
  dataSource = new MatTableDataSource<any>(this.data_table);
  selection = new SelectionModel<any>(false, []);
  itemsPagina: number[] = enviroments.pageSize;

  overlayRef: OverlayRef;
  status: boolean = true;
  packId: number;
  empacadoEx: string;
  razonS: string;
  empacadoName: string = "";

  
  tipoUsuario : number = 0;

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


  dialogoAgregarEmpacado(action = 0) {

    let titulo: string = "Agregar";

    if (action != 0) {
      titulo = "Editar";
    }

    let _dialogRef = this._dialog.open(DialogoAgregarEmpacadoComponent, {
      panelClass: "dialog-aprod",
      data: { titulo: titulo, action: action, packedId: this.packId, tipoUsuario: this.tipoUsuario }
    });

    _dialogRef.afterClosed().subscribe(res => {
      if (res == true) {
        this.obtenerDatos();
      }
    })
  }
  dialogoDireccionEmpacado(action = 0){
    let _dialogRef = this._dialog.open(DialogoDireccionEmpacadoComponent, {
      panelClass: "dialog-aprod",
      data: { packedId: this.packId, direccionId: this.direccionId }
    });
    _dialogRef.afterClosed().subscribe(result => {
        this.obtenerDatos();
    });
  }

  dialogoEliminarEmpacado() {
    //hacer validaciones del estatus y weas
    let validacion = 1;
    if (this.status == true) {
      validacion = 0;
    } else {
      validacion = 1;
    }

    let action: any = {
      mensaje: validacion == 1 ? "Esta seguro de eliminar el empacador externo seleccionado" : "Solo se puede eliminar empacados externos con estatus Inactivo", //Si no, solo mostrar el mensaje de error,
      tipo: validacion == 1 ? 0 : 1 //alguna validación para incitar en eliminar
    }

    let _dialogRef = this._dialog.open(DialogoEliminarEmpacadoExternoComponent, {
      panelClass: "dialog-aprod",
      data: { action, packedId: this.packId , empacadorActual: this.empacadorActual}
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
      data: { titulo: "Agregar", type: 3, id: this.packId }
    });

    _dialogRef.afterClosed().subscribe(ref => {
      if (ref == true) {
        if (this.empacadoEx != "" || this.razonS != "") {
          this.obtenerDatos(1);
        } else {
          this.obtenerDatos();
        }
      }
    })

  }
  
  companyIdSearch: Number = 0;
  obtenerDatos(action = 0) {
    let data: any = {};
    console.log("packedName");
    //Busqueda general
    if (action == 0) {
      data = {
        packedName: "",
        opc: false,
        type: parseInt(sessionStorage.getItem("isType"), 10)
      }
    } else { //busqueda con filtros
      data = {
        packedName: this.empacadoEx,
        companyIdSearch: this.isCompany == 0 ? this.companyIdSearch : this.isCompany,
        opc: true,
        type: parseInt(sessionStorage.getItem("isType"), 10)
      }
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("ExternalPacked/SearchExternalPacked", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(data, "Recibido");
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.dataSource.data = data['packedList'];
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
  empacadorActual: any;
  //Evento para realizar la seleccion de la tabla
  direccionId : number = 0;
  seleccion = () => {
    this.selection.selected.map(item => { this.packId = item.packedId, this.status = item.activo, this.empacadoName = item.packedName, this.empacadorActual = item });
    this.selection.selected.map(item => { this.direccionId = item.direccionId });
  }

  accionEnabled = () => {
    //si no hay seleccionado
    if (this.selection.selected.length == 0) {
      return 'disabled';
    } else if (this.selection.selected.length > 0) { // sí hay seleccionado
      //el usuario logueado es tipo empacador?
      if (parseInt(sessionStorage.getItem("isType"), 10) == 1) {
        // si el seleccionado es diferente a mi id, (así no me puedo modificar ni eliminar yo mismo)
        if (this.selection.selected[0].packedId != parseInt(sessionStorage.getItem("idUser"), 10)) {
          return null;
        }  else {
          //el seleccionado soy yo mismo, bloquear
          return 'disabled';
        }
      }else {
        //no es tipo empacador si no directamente la empresa
        return null;
      }

      
    }
    return 'disabled';
    
  }

  catalogoOperador() {
    this._router.navigateByUrl(`EmpacadoEtiquetado/EmpacadoExterno/Operadores/${2}/${this.packId}/${this.empacadoName}`, { queryParams: { proviene: 2, id: this.packId }, state: { proviene: 2, packedId: this.packId } });
  }

  catalogoLOperaciones() {
    this._router.navigateByUrl(`EmpacadoEtiquetado/EmpacadoExterno/LineasOperacion/${2}/${this.packId}/${this.empacadoName}`, { queryParams: { proviene: 2, id: this.packId }, state: { proviene: 2, packedId: this.packId } });
  }

  gestionCajas() {
    this._router.navigateByUrl(`EmpacadoEtiquetado/EmpacadoExterno/GestionCajas/${2}/${this.packId}/${this.empacadoName}`, { queryParams: { proviene: 2, id: this.packId }, state: { proviene: 2, packedId: this.packId } });
  }

  reporteArmados() {
    this._router.navigateByUrl(`EmpacadoEtiquetado/EmpacadoExterno/ReporteArmados/${2}/${this.packId}/${this.empacadoName}`, { queryParams: { proviene: 2, id: this.packId }, state: { proviene: 2, packedId: this.packId } });
  }

  isType: number = 0;
  isCompany: number = 0;
  ngOnInit() {
    
    this.isType = parseInt(sessionStorage.getItem("isType"), 10);
    this.isCompany = parseInt(sessionStorage.getItem("company"), 10);
    this.tipoUsuario = Number(sessionStorage.getItem('company'));
    if(this.tipoUsuario == 0)
      this.getCompaniasForSearch();
    this.dataSource.sort = this.sort;

    this.razonS = "";
    this.empacadoEx = "";

    this.obtenerDatos();
    
  }

  
    /*
  * Funcion para obtener listado de compañias
  *  Autor: Hernán Gómez 
  * 28-Marzo-2022
  */
    name: string = "";
    businessName: string = "";
    idCompany: number = 0;
    response = new SearchCompanyResponse();
    emptyList: boolean = false;
    dataSourceCompany : Array<CompaniesData>; 
    getCompaniasForSearch() {
      var request = new SearchCompanyRequest();
      request.name = this.name;
      request.businessName = this.businessName;
      this.idCompany = 0;
  
      this._dataService.postData<SearchCompanyResponse>("Companies/searchCompany", sessionStorage.getItem("token"), request).subscribe(
        data => {
          this.response = data;
          if(this.response.companiesDataList.length > 0)
            this.emptyList = false;
          else
            this.emptyList = true;
          this.dataSourceCompany = this.response.companiesDataList.filter(x => x.name != "");
        },
        error => {
          console.log(error);
        }
      );
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
