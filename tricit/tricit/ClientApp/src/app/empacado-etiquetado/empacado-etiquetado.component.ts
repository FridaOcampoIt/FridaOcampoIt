import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatSnackBar, MatDatepicker } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { Router, ActivatedRoute } from '@angular/router';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { DataServices } from '../Interfaces/Services/general.service';
import { VERSION } from '@angular/material';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-empacado-etiquetado',
  templateUrl: './empacado-etiquetado.component.html',
  styleUrls: ['./empacado-etiquetado.component.less']
})
export class EmpacadoEtiquetadoComponent implements OnInit, AfterViewInit {
  version = VERSION;

  data_table: Array<any> = [];
  public dataSource = new MatTableDataSource<any>(this.data_table);  // <-- STEP (1)
  displayedColumns: string[] = [
    'nombreEmpacador',
    'groupingName',
    'pallet',
    'box',
    'productName',
    'quantity',
    'registerDate'
  ];
  displayedColumnsAcopio = [
    'nombreAcopio',
    'movimientoId',
    'nombreAgrupacion',
    'numeroCajas',
    'producto',
    'cantidad',
    'tipoMovimiento',
    'nombreRemitente',
    'apellidoRemitente',
    'nombreDestinatario',
    'apellidoDestinatario',
    'quien',
    'fechaIngreso',
    'cantIndMov',
    'lote'
  ];

  @ViewChild('scheduledOrdersPaginator') paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;// <-- STEP 
  proveedores: any[] = [
    /*{ id: 1, name: "Proveedores" },*/
    /*{ id: 2, name: "Distribuidores" },*/
    { id: 3, name: "Empacador externo" },
    { id: 4, name: "Empacador interno" },
    /*{ id: 5, name: "Empacador agro" }*/
  ]

  productosList: any[] = [];

  tipoCompania: number = 0;
  /**
   * 0 => Trace it, 1 => otro
   */
  tipoUsuario: number = 0;

  dateStart: any = "";
  dateEnd: any = "";
  maxDate: any = "";
  busquedaGenerica: string = "";
  traceitDistributor: boolean = false;

  itemsPagina: number[] = enviroments.pageSize;
  overlayRef: OverlayRef;

  searchField: string;

  tUsuario: number;

  productId: number = 0;
  usuarioId: number = 0;
  @ViewChild("dateStart") dateStartObj: MatDatepicker<any>;
  @ViewChild("dateEnd") dateEndObj: MatDatepicker<any>;

  //Variables auxiliares, para el cagadero de permisos que hay. 
  //Traceit (isType = 0, isCompany = 0), Compañía (isType = 0, isCompany = [>1]), empacador (isType = 1, isCompany = [>1])
  isType: number = 0;
  isCompany: number = 0;
  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices
  ) {
    this.tipoCompania = 0;
    this.tipoUsuario = 1;
    this.usuarioId = parseInt(sessionStorage.getItem("idUser"), 10);
    this.tUsuario = parseInt(sessionStorage.getItem("company"), 10);
    this.isType = parseInt(sessionStorage.getItem("isType"), 10);
    this.isCompany = parseInt(sessionStorage.getItem("company"), 10);
  }
  doSomething(event) {
    this.dataSource = null;
    this.emptyList = false;
  }

  modoUsuario() {
    if (this.tUsuario == 0) {
      return false;
    } else {
      return true;
    }
  }

  registroCompaniaAcces() {
    this.tipoUsuario == 1 ? false : true;
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }


  registroCompania() {

    if (this.tipoCompania == 0) {
      this.openSnack("Seleccione una tipo de compañia", "Aceptar");
      return;
    }

    switch (this.tipoCompania) {
      case 1: //proveedores
        this._router.navigateByUrl('EmpacadoEtiquetado/Proveedores');
        break;
      case 2: //Distribuidores
        this._router.navigateByUrl('EmpacadoEtiquetado/Distribuidores');
        break;
      case 3: //Empacado Externo
        this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoExterno');
        break;
      case 4: //empacado interno
        this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoInterno');
        break;
      case 5: //empacado agro
        if (this.modoUsuario()) {
          this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoAgro');
        }
        break;
      case 6: //Acopios
        this._router.navigateByUrl('EmpacadoEtiquetado/Acopios');
        break;
      default:
        break;
    }
  }

  company: number = 0;
  ngOnInit() {
    this.emptyList = false;
    this.dataSource.sort = this.sort;

    this.dateStart = "";
    this.dateEnd = "";
    this.traceitDistributor = false;
    this.productId = 0;

    //Es sí o sí usuario tipo empacado ext o inter
    if (parseInt(sessionStorage.getItem("isType"), 10) != 0) {
      this.tipoUsuario = 1;
    } else {
      //no es usuario empacado, pero si es un usuario con una compañia asignada
      if (parseInt(sessionStorage.getItem("company"), 10) != 0) {
        this.tipoUsuario = 1;
      } else {
        //Es un usuario tipo trace it ya que no tiene compañia asignada
        this.tipoUsuario = 0;
      }
    }

    this.company = parseInt(sessionStorage.getItem("company"), 10);
    this.isType = parseInt(sessionStorage.getItem("isType"), 10);
    if (this.company > 0 && this.isType == 0) {
      this.proveedores.push({
        id: 6, name: 'Acopios'
      })
    }
    this.obtenerProductos();
  }


  ngAfterViewInit(): void {
    setTimeout(() => {
      //this.dataSource.paginator = this.paginator;  // <-- STEP (4)
    });
  }
  obtenerProductos() {
    let datos: any = {
      id: 0
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);
    //debugger
    this._dataService.postData<any>("PackedLabeled/SearchProductCombo", sessionStorage.getItem("token"), datos).subscribe(
      data => {
        //  
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.productosList = data["productscombo"];
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

  emptyList: boolean = false;
  aplicarBusqueda() {
    if (this.tipoCompania == 0)
      return this.openSnack("Por favor, seleccione un tipo de compañía.", "Aceptar");
    //validaciones
    let datos: any = {
      companyId: this.tUsuario,
      productId: this.productId,
      searchGeneric: this.busquedaGenerica,
      datestart: "",
      dateEnd: "",
      chkDistributor: this.traceitDistributor,
      opc: this.tipoCompania,
      companiaId: this.isCompany ? this.isCompany : null,
      empacadorId: this.isType == 1 ? this.usuarioId : 0
    };


    if (this.dateEnd != "" && this.dateStart == "") {
      this.openSnack("Seleccione una fecha inicial", "Aceptar");
      return;
    }

    if (this.dateStart != "") {

      if (this.dateEnd == "") {
        this.openSnack("Seleccione una fecha final", "Aceptar");
        return;
      }

      datos["dateStart"] = `${new Date(this.dateStart).getFullYear()}-${new Date(this.dateStart).getMonth() + 1}-${new Date(this.dateStart).getDate()}`;
      datos["dateEnd"] = `${new Date(this.dateEnd).getFullYear()}-${new Date(this.dateEnd).getMonth() + 1}-${new Date(this.dateEnd).getDate() + 1}`; // getDate() + 1
      console.log("FECHAS ", datos);
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    if (this.tipoCompania >= 1 && this.tipoCompania < 6) {
      this._dataService.postData<any>("PackedLabeled/SearchGrouping", sessionStorage.getItem("token"), datos).subscribe(
        data => {
          //  
          setTimeout(() => {
            this._overlay.close(this.overlayRef);
          }, 1);
          if (data["messageEng"] != "") {
            this.openSnack(data["messageEsp"], "Aceptar");
          } else {
            if (data["groupingList"].length > 0)
              this.emptyList = false;
            if (this.isCompany == 0 && this.isType == 0) //Con esto agregamos para que se vea el dato de "compañia en la tabla"
              if (!this.displayedColumns.indexOf('companyName')) // Sino existe la columna la agregamos, si existe la omitimos
                this.displayedColumns.unshift('companyName');
            this.dataSource = new MatTableDataSource(data["groupingList"]);
            setTimeout(() => {
              this.dataSource.paginator = this.paginator;
              this.dataSource.sort = this.sort;
            });
            this.emptyList = true;
          }
          console.log("success", data);
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
    } else {
      this._dataService.postData<any>("Acopio/searchInformationAcopio", sessionStorage.getItem("token"), datos).subscribe(
        data => {
          console.log('RESPONSE', data);
          setTimeout(() => {
            this._overlay.close(this.overlayRef);
          }, 1);
          if (data["messageEng"] != "") {
            this.openSnack(data["messageEsp"], "Aceptar");
          } else {
            if (data["movimientosDataList"].length > 0)
              this.emptyList = false;
            this.dataSource = new MatTableDataSource(data["movimientosDataList"]);
            setTimeout(() => {
              this.dataSource.paginator = this.paginator;
              this.dataSource.sort = this.sort;
            });
            this.emptyList = true;
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
  }

  formatearFecha(fecha: string): string {
    if (fecha != "" && fecha != null) {
      let date = new Date(fecha);

      return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;
    } else {
      return "";
    }

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
            this.obtenerProductos();
            break;
          case "2":
            this.aplicarBusqueda();
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

  aplicarFiltro() {
    this.dataSource.filter = this.searchField;
  }

}
