import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatTableDataSource, MatDialog, MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';

import { PdfMakeWrapper, 
  Table, 
  Img, 
  Columns,
  Cell,
  Txt, } from 'pdfmake-wrapper';

import pdfFonts from "pdfmake/build/vfs_fonts";
/**
 * Iconos Font Awesome
 */
import {
  faFileExcel, faMapMarkerAlt, faBuilding, faTruckLoading, faStore
} from "@fortawesome/free-solid-svg-icons";
import { DialogoUbicacionComponent } from './dialogo-ubicacion/dialogo-ubicacion.component';
import { DialogoMultiplesComponent } from './dialogo-multiples/dialogo-multiples.component';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { DialogoDetalleComponent } from './dialogo-detalle/dialogo-detalle.component';
import { OverlayRef } from '@angular/cdk/overlay';
import { ActivatedRoute, Router } from '@angular/router';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { DataServices } from '../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';
import { DialogoLectorComponent } from './dialogo-lector/dialogo-lector.component';

const element_data: any[] = [

]

const Meses: any = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
@Component({
  selector: 'app-rastreo',
  templateUrl: './rastreo.component.html',
  styleUrls: ['./rastreo.component.css']
})
export class RastreoComponent implements OnInit, AfterViewInit {

  overlayRef: OverlayRef;
  constructor(
    private _dialog: MatDialog,
    private _route: ActivatedRoute,
    private _router: Router,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices
  ) {
    PdfMakeWrapper.setFonts(pdfFonts);
   }

  //Representa componente de paginación para la busqueda
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //Representa la instancia para el sorting de columnas
  @ViewChild(MatSort) sort: MatSort;

  /**
   * Variables
   */
  faExcel = faFileExcel;
  faMarker = faMapMarkerAlt;
  faBuild = faBuilding;
  faTruck = faTruckLoading;
  faStore = faStore;

  CUICodeControl = new FormControl();


  displayedColumns: string[] = ['select', 'ciu', 'lote', 'nserie', 'fecha', 'evento', 'ubicacion', 'detalle', 'distribuidor'];
  dataSource = new MatTableDataSource<any>(element_data);
  selection = new SelectionModel<any>(false, []);
  itemsPagina: number[] = enviroments.pageSize;
  globalCodigoQR = "";
  globalMovimientoId = 0;

  lineaTabla: boolean = false;

  searchCode: string = "";
  searchCIUCode: string;
  phase: number;

  linea_data: any[] = [];

  phasesList = [
    { id: 1, data: "Compañia" },
    { id: 2, data: "Distribuidor" },
    { id: 3, data: "Consumo" },
  ]

  options: string[] = [];

  responseDataDes: any = "";
  responseDataRem: any = "";
  responseDataTrans: any = "";
  responseDataInfo: any = "";
  responseDocsProductos: any = "";
  responseDocsProductosIndi: any = "";
  responseDocsProductosCajas: any = "";
  responseDocsTotalProd: any = "";
  responseDocsTotalPallet: any = "";
  responseDocsTotalCajas: any = "";
  responseDocsTotalCantidad: any = "";
  responseDocsTotalPeso: any = "";
  responseDocsFechaMin: any = "";
  responseDataComentario: any = "";
  responseData: any = "";

  codigoQR: string = "";
  codigoQR2: string = "";
  movimientoId: number = 0;

  ngOnInit() {
    this.dataSource.sort = this.sort;
    this.searchCode = "";
    this.searchCIUCode = "";
    this.phase = 0;
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  checkboxLabel(row?: any): string {
    if (!row) {
      return "{this.isAllSelected() ? 'select' : 'deselect'} all";
    }
    return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
  }

  openUbicacionDialog(lat, long) {
    const _dialogRef = this._dialog.open(DialogoUbicacionComponent, {
      panelClass: 'dialogo-mapa',
      disableClose: false,
      data: { lat: lat, long: long }
    })
  }

  aplicarBusqueda() {

    if (this.searchCode.trim().length == 0) {
      this.openSnack("Ingrese un Código", "Aceptar");
      return;
    }

    let data: any = {
      searchCode: this.searchCode.trim(),
      searchCIUCode: this.CUICodeControl.value,
      phase: this.phase,
      companiaId: parseInt(sessionStorage.getItem("company"))
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);
    //debugger
    this._dataService.postData<any>("Tracking/SearchTracking", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          if (data.trackingList.length == 0) {
            this.openSnack("No hay registros", "Aceptar");
          } else {
            this.creacionLinea(data.trackingList);
          }
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


    this.lineaTabla = true;
    // if (this.searchCode == "12345") {
    //   this.openMultiplesDialog();
    // } else {
    //   console.log(this.linea_data, 'data de la linea');
    //   this.searchCode.length ? this.lineaTabla = true : this.lineaTabla = false;
    // }
  }

  creacionLinea(data: any) {
    console.log("creacion", data);

    let eventos = [];
    this.options = [];

    data.forEach(regi => {

      if (regi.typeGrouping != "Agrupación" && regi.typeGrouping != "Ninguno") {

        
        let date = new Date(regi.date);

        if (regi.ciu.length) {
          if (!this.options.includes(regi.ciu)) {
            this.options.push(regi.ciu);
          }
        }

        if (regi.serialN.length) {
          if (!this.options.includes(regi.serialN)) {
            this.options.push(regi.serialN);
          }
        }

        let formato = `${Meses[date.getMonth()]}-${date.getDate()}-${date.getFullYear()}`;

        let indExist = eventos.findIndex(reg => reg.fecha == formato);

        if (indExist == -1) {
          //si no existe se genera el objeto
          let evento = {
            fecha: formato,
            datos: []
          };
          eventos.push(evento);
          indExist = eventos.findIndex(reg => reg.fecha == formato);
        }

        //generación de un evento
        let evento = {
          id_evento: regi.id,
          hora: `${date.getHours() < 10 ? '0' : ''}${date.getHours()}:${date.getMinutes() < 10 ? '0' : ''}${date.getMinutes()}`,
          // icono: regi.phase == "Distribucion" ? 2 : regi.phase == "Compania" ? 1 : 3,
          icono: regi.eventName == "Origen" ? 5 : regi.eventName == "Tracking" ? 6 : regi.eventName == "RecTrack" ? 7 : regi.phase == "Compania" ? 1 : regi.typeGrouping == "Envío" ? 2 : regi.typeGrouping == "Recepción" ? 3 : regi.phase == "Consumo" ? 4 : 0, // faltan los de movil
          compania: regi.companyName,
          operador: regi.operator,
          empacador: regi.empacador,
          descripcion: regi.eventDetail ? regi.eventDetail : regi.phase == "Consumo" ? "" : "No hay descripcion.",
          lat: regi.latitude,
          long: regi.longitude,
          latstock: regi.stockUserLatitud,
          lonstock: regi.stockUserLongitud,
          accion: regi.typeGrouping,
          data: regi
        }

        eventos[indExist]["datos"].push(evento);
      }
    });

    this.linea_data = eventos;
    console.log(eventos);
  }

  openScanerDialog() {
    const _dialogRef = this._dialog.open(DialogoLectorComponent, {
      panelClass: 'dialog-aprod',
      disableClose: false
    })
    /**
     * Obtener los datos del multiple seleccionado o setearlos, dependiendo como lo quieran programar
     */
    _dialogRef.afterClosed().subscribe(res => {
      if (res) {
        console.log("respuesta", res);
        this.searchCode = res;
      }
    });
  }

  openMultiplesDialog() {
    const _dialogRef = this._dialog.open(DialogoMultiplesComponent, {
      panelClass: 'dialog-aprod',
      disableClose: false
    })

    /**
     * Obtener los datos del multiple seleccionado o setearlos, dependiendo como lo quieran programar
     */
    _dialogRef.afterClosed().subscribe(res => {
      if (res) { }
    });
  }

  openDetalleDialog(idevento = 0) {
    if (idevento == 0) {
      this.openSnack("Seleccione un evento", "Aceptar");
      return;
    }

    const _dialogRef = this._dialog.open(DialogoDetalleComponent, {
      panelClass: 'dialog-aprod',
      disableClose: false,
      data: { id: idevento }
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
        sessionStorage.setItem("isType", data.userData.userData.isType.toString());

        switch (peticion) {
          case "1":
            this.aplicarBusqueda();
            break;
          case "2":

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

  BusquedaDatosPDF(movimientoIdi = 0, codigoQR = "") {
    
    if (movimientoIdi == 0) {
      this.openSnack("El movimiento no puede ser nulo o inexistente", "Aceptar");
      return;
    }

    if (codigoQR == "") {
      this.openSnack("El movimiento no cuenta con un código QR", "Aceptar");
      return;
    }

    this.globalMovimientoId = movimientoIdi;
    this.globalCodigoQR = codigoQR;

    var request = {
      movimientoId: movimientoIdi
    }
        
    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("Movimientos/searchDataMovimientoGeneral", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseData = data;
        this.codigoQR2 = this.responseData.movimientosDataGeneralRecepList.codigoQR;
        this.BusquedaDes();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  BusquedaDes() {

    if (this.globalMovimientoId == 0) {
      this.openSnack("El movimiento no puede ser nulo o inexistente", "Aceptar");
      return;
    }

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDataMovimientoDestinatario", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDataDes = data;
        this.BusquedaRem();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  BusquedaRem() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDataMovimientoRemitente", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDataRem = data;
        this.BusquedaTrans();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  BusquedaTrans() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDataMovimientoTransportista", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDataTrans = data;
        this.BusquedaInfo();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  //Funcion para realizar la busqueda  (Las variables son las que se envian para hacer la búsqueda)
  BusquedaInfo() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDataMovimientoInfoLegal", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDataInfo = data;
        this.BusquedaDocsProductos();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }
  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  BusquedaDocsProductos() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDocDetalleProductos", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsProductos = data;
        this.dataSource.data = this.responseDocsProductos.docDetalleProductoList;
        //this.BusquedaDetalleProductos();
        this.BusquedaDetalleProductos2();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  /*BusquedaDetalleProductos() { // Productos tabla Pallet

    var request = {
      movimientoId: this.globalCodigoQR
    }

    this._dataService.postData<any>("Movimientos/searchDocDetalleProductosIndi", sessionStorage.getItem("token"), request).subscribe(
      data => {
        // aquíu esta el seto prro
        this.responseDocsProductosIndi = data;
        this.BusquedaDetalleProductos2();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }*/

  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  BusquedaDetalleProductos2() { // Productos tabla Cajas

    var request = {
      movimientoId: this.globalMovimientoId
    }

    //Obtener valor de rango en formato Json
    if (JSON.parse('{ "return":' + this.globalCodigoQR + ' }')) {
      let objJson = JSON.parse('{ "return":' + this.globalCodigoQR + ' }')
      let inicial = objJson.return.I;
      let final = objJson.return.F;
      let tipo = objJson.return.T;

      request["codigoTipo"] = tipo;
      request["codigoI"] = inicial;
      request["codigoF"] = final;
    }


    this._dataService.postData<any>("Movimientos/searchDocDetalleProductosCajas", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsProductosCajas = data;
        this.BusquedaDocsTotalProd();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  BusquedaDocsTotalProd() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDocDetalleTotalProd", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsTotalProd = data;
        //this.BusquedaDocsTotalPallet();
        this.BusquedaDocsTotalCajas();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  /*BusquedaDocsTotalPallet() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDocDetalleTotalPallet", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsTotalPallet = data;

        if (this.responseDocsTotalPallet.docDetalleTotalPalletList.totalPallet==0) {
          
        }
        this.BusquedaDocsTotalCajas();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }*/

  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  BusquedaDocsTotalCajas() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDocDetalleTotalCajas", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsTotalCajas = data;

        if (this.responseDocsTotalCajas.docDetalleTotalCajasList.totalCajas == 0) {
          
        }
        this.BusquedaDocsTotalCantidad();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  BusquedaDocsTotalCantidad() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDocDetalleTotalCantidad", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsTotalCantidad = data;
        this.BusquedaDocsTotalPeso();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  BusquedaDocsTotalPeso() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDocDetalleTotalPeso", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsTotalPeso = data;
        this.BusquedaDocsFechaMin();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  BusquedaDocsFechaMin() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDocDetalleFechaMin", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsFechaMin = data;
        this.BusquedaComentario();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  //Funcion para realizar la busqueda  (Las variables son las que se envian para hacer la búsqueda)
  BusquedaComentario() {

    var request = {
      movimientoId: this.globalMovimientoId
    }

    this._dataService.postData<any>("Movimientos/searchDataObservcion", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDataComentario = data;
        this.generatePDF();
        // se limpia el código qr brindado.
        this.globalCodigoQR = "";
        this._overlay.close(this.overlayRef);
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          console.log(error);
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }

  async generatePDF(){
    this.responseDataDes.movimientosDataDestinatarioRecepList.paisND.includes("Seleccionar") ? this.responseDataDes.movimientosDataDestinatarioRecepList.paisND = "" : this.responseDataDes.movimientosDataDestinatarioRecepList.paisND;
    this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND.includes("Seleccionar ") ? this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND = "" : this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND;
    this.responseDataDes.movimientosDataDestinatarioRecepList.cpD == 0 ? this.responseDataDes.movimientosDataDestinatarioRecepList.cpD = "" : this.responseDataDes.movimientosDataDestinatarioRecepList.cpD;

    const pdf = new PdfMakeWrapper();

    pdf.info({
      title: 'Guía de Embarque',
      author: 'Trace IT',
      subject: ''
    });

    pdf.pageOrientation('portrait');
    pdf.pageMargins([ 20, 60, 20, 30 ]);

    let tableHeader = new Table([
        [ {text:this.responseDataRem.movimientosDataRemitenteRecepList.rzCompaniaR, bold:true, fontSize:18}
        ]
      ]).widths(['*']).alignment('center').color('#012152').alignment('left').margin([20,20,40,0]).end;

    if(this.responseDataRem.movimientosDataRemitenteRecepList.rzCompaniaR == "") {
      tableHeader = new Table([
        [ new Cell( new Txt(' ').bold().fontSize(12).end).fillColor("#012152").end ]
      ]).widths(['*']).alignment('center').color('#012152').alignment('left').margin([20,20,40,0]).end;
    }

    pdf.header(
      new Columns([
        tableHeader,
        await new Img('../../assets/img/img-logo-stock-3.png').width('100').margin([-20,20,20,0]).build()
      ]).end,
    );

    pdf.add(
      pdf.ln(1)
    );
    
    let textCajaId = this.responseDocsProductosCajas.docDetalleProductoCajasList.length > 0 ? this.responseDocsProductosCajas.docDetalleProductoCajasList[0].codigoCaja.split(',')[0] : '';
    
    pdf.add(
      new Columns([
        new Table([
          ['','',''],['','',''],['','',''],['','',''],['','',''],['','',''],
          [ {text: 'Guía de Embarque', bold:true, alignment:'left', fontSize:16, colSpan: 3} ],
          ['','',''],['','',''], ['','',''],  ['','',''],['','',''],  ['','',''],  ['','',''],  
          [ {text:'OP Interna: ', alignment:'right', bold:true}, 
            {text: this.responseData.movimientosDataGeneralRecepList.referenciaInterna, alignment:'left', color: 'gray'},
            ''
          ],
          [ {text:'No. cliente: ', alignment:'right', bold:true}, 
            {text: this.responseDataDes.movimientosDataDestinatarioRecepList.numeroC, alignment:'left', color: 'gray'},
            ''
          ],
          [ {text:'Cliente: ', alignment:'right', bold:true}, 
            {text: this.responseDataDes.movimientosDataDestinatarioRecepList.nombreCompaniaD, alignment:'left', color: 'gray'},
            ''
          ],
          [ {text:'OP Cliente: ', alignment:'right', bold:true}, 
            {text: this.responseData.movimientosDataGeneralRecepList.referenciaExterna, alignment:'left', color: 'gray'},
            ''
          ]
        ]).widths([80, 150, 40]).fontSize(9).layout('noBorders').end,
        new Table([
          [ {qr: enviroments.qrUrl + this.responseData.movimientosDataGeneralRecepList.codigoQR, alignment:'center', fit:100, colSpan:2},
            ''
          ],
          [ {text: textCajaId, color: 'gray', colSpan:2}]
        ]).widths([80, 150]).alignment('center').fontSize(9).layout('noBorders').end
      ]).end
    );

    pdf.add(
      pdf.ln(1)
    );

    // Domicilio Destinatario
    let fullDomicilioDest = this.responseDataDes.movimientosDataDestinatarioRecepList.domicilioD;
    fullDomicilioDest += fullDomicilioDest != "" ? 
        (this.responseDataDes.movimientosDataDestinatarioRecepList.ciudadD != '' ? ', '+this.responseDataDes.movimientosDataDestinatarioRecepList.ciudadD : '')
      : 
        (this.responseDataDes.movimientosDataDestinatarioRecepList.ciudadD != '' ? this.responseDataDes.movimientosDataDestinatarioRecepList.ciudadD : '');
    fullDomicilioDest += fullDomicilioDest != "" ? 
        (this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND != 'Seleccionar un estado' ? ', '+this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND : '') 
      : 
        (this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND != 'Seleccionar un estado' ? this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND : '');
    fullDomicilioDest += fullDomicilioDest != "" ? 
        (this.responseDataDes.movimientosDataDestinatarioRecepList.paisND != 'Seleccionar un pais' ? ', '+this.responseDataDes.movimientosDataDestinatarioRecepList.paisND : '')
      : 
        (this.responseDataDes.movimientosDataDestinatarioRecepList.paisND != 'Seleccionar un pais' ? this.responseDataDes.movimientosDataDestinatarioRecepList.paisND : '');
    // Domicilio Remitente
    let fullDomicilioRem = this.responseDataRem.movimientosDataRemitenteRecepList.domicilioR;
    fullDomicilioRem += fullDomicilioRem != "" ? 
        (this.responseDataRem.movimientosDataRemitenteRecepList.cpR != '' && this.responseDataRem.movimientosDataRemitenteRecepList.cpR != '0' ? ', C.P. '+this.responseDataRem.movimientosDataRemitenteRecepList.cpR : '')
      : 
        (this.responseDataRem.movimientosDataRemitenteRecepList.cpR != '' && this.responseDataRem.movimientosDataRemitenteRecepList.cpR != '0' ? 'C.P. '+this.responseDataRem.movimientosDataRemitenteRecepList.cpR : '');
    fullDomicilioRem += fullDomicilioRem != "" ? 
        (this.responseDataRem.movimientosDataRemitenteRecepList.ciudadR != '' ? ', '+this.responseDataRem.movimientosDataRemitenteRecepList.ciudadR : '')
      : 
        (this.responseDataRem.movimientosDataRemitenteRecepList.ciudadR != '' ? this.responseDataRem.movimientosDataRemitenteRecepList.ciudadR : '');
    fullDomicilioRem += fullDomicilioRem != "" ? 
        (this.responseDataRem.movimientosDataRemitenteRecepList.estadoNR != 'Seleccionar un estado' ? ', '+this.responseDataRem.movimientosDataRemitenteRecepList.estadoNR : '') 
      : 
        (this.responseDataRem.movimientosDataRemitenteRecepList.estadoNR != 'Seleccionar un estado' ? this.responseDataRem.movimientosDataRemitenteRecepList.estadoNR : '');
    fullDomicilioRem += fullDomicilioRem != "" ? 
        (this.responseDataRem.movimientosDataRemitenteRecepList.paisNR != 'Seleccionar un pais' ? ', '+this.responseDataRem.movimientosDataRemitenteRecepList.paisNR : '')
      : 
        (this.responseDataRem.movimientosDataRemitenteRecepList.paisNR != 'Seleccionar un pais' ? this.responseDataRem.movimientosDataRemitenteRecepList.paisNR : '');

    pdf.add(
      new Columns([
        new Table([
          [ {text:'Destinatario: ', alignment:'right', bold:true, fontSize:11}, 
            {text: /*this.responseDataDes.movimientosDataDestinatarioRecepList.nombreCompaniaD+' '+*/
                   this.responseDataDes.movimientosDataDestinatarioRecepList.rzCompaniaD, alignment:'left', color: 'gray'}
          ],
          [ {text:'Dirección: ', alignment:'right', bold:true}, 
            {text: fullDomicilioDest, alignment:'left', color: 'gray'}
          ]
        ]).widths([80, 150]).fontSize(9).end,
        new Table([
          [ {text:'Remitente: ', alignment:'right', bold:true, fontSize:11}, 
            {text: this.responseDataRem.movimientosDataRemitenteRecepList.nombreCompaniaR+' '+
                   this.responseDataRem.movimientosDataRemitenteRecepList.rzCompaniaR, alignment:'left', color: 'gray'}
          ],
          [ {text:'Dirección: ', alignment:'right', bold:true}, 
            {text: fullDomicilioRem, alignment:'left', color: 'gray'} 
          ],
          [ 
            {
              text:'Atn: ', alignment:'right', bold:true
            }, 
            {
              text: this.responseDataRem.movimientosDataRemitenteRecepList.atendioUsuario, alignment:'left', color: 'gray'
            }
          ]
        ]).widths([80, 150]).alignment('center').fontSize(9).end
      ]).end
    );

    pdf.add(
      pdf.ln(1)
    );

    pdf.add(
      new Columns([
        new Table([
          [ {text:'Transporte: ', alignment:'right', bold:true, fontSize:11}, 
            {text: this.responseDataTrans.movimientosDataTransportistaList.transportista, alignment:'left', color: 'gray'}
          ],
          [ {text:'Fecha embarque: ', alignment:'right', bold:true}, 
            {text: this.responseDataTrans.movimientosDataTransportistaList.fechaEmbarque, alignment:'left', color: 'gray'}
          ]
          ,
          [ {text:'Referencia: ', alignment:'right', bold:true}, 
            {text: this.responseDataTrans.movimientosDataTransportistaList.numReferencia, alignment:'left', color: 'gray'}
          ]
        ]).widths([80, 150]).fontSize(9).end,
        new Table([
          [ {text:'Observaciones: ', alignment:'right', bold:true, fontSize:11}, 
            {text: this.responseDataComentario.movimientosDataObservacionRecepList.observacion, alignment:'left', color: 'gray'}
          ],
          [ {text:'Dimensiones por caja (cms): ', alignment:'right', bold:true}, 
            {text: this.responseDataComentario.movimientosDataObservacionRecepList.dimensionesCaja, alignment:'left', color: 'gray'}
          ],
          [ {text:'Peso por caja (Kg): ', alignment:'right', bold:true}, 
            {text: this.responseDataComentario.movimientosDataObservacionRecepList.pesoCaja, alignment:'left', color: 'gray'}
          ]
        ]).widths([80, 150]).alignment('center').fontSize(9).end
      ]).end
    );

    pdf.add(
      pdf.ln(2)
    );

    pdf.add(
      new Columns([
        new Table([
          [ {text:'Resumen de Productos/Pedido: ', bold:true, colSpan:8, fontSize:11, alignment:'left'}, 
            '','','','','','',''
          ],
          [ {text:'Producto/Familia: ', bold:true}, 
            {text: 'Marca: ', bold:true},
            {text: 'GTIN: ', bold:true},
            {text: 'Pallets: ', bold:true},
            {text: 'Cajas por pallet: ', bold:true},
            {text: 'Unidades por caja: ', bold:true},
            {text: 'Total de unidades: ', bold:true},
            {text: 'Caducidad prox: ', bold:true},
          ]
        ]).widths([100, 65, 55, 35, 40, 50, 60, 75]).alignment('center').fontSize(9).end,
      ]).end
    );

    //Productos FOR
    for (let products of this.responseDocsProductos.docDetalleProductoList) {
      var a;
      if(products.numPallet==0){
        a=1;
      }else{
        a=products.numPallet;
      }
      pdf.add(
        new Table([
          [ {text:products.producto, alignment:'right'}, 
            {text: products.ciu, alignment:'right'}, 
            products.gtin,
            0, 
            products.numCajas/a, 
            products.cantidad/products.numCajas, 
            products.cantidad, 
            products.fechaCaducidad
          ],
        ]).widths([100, 65, 55, 35, 40, 50, 60, 75]).alignment('center').fontSize(8).end
      );
    }

    pdf.add(
      new Table([
        [ 
          {text:'', bold:true, colSpan:9, fontSize:11, alignment:'left',pageBreak: 'before'}, 
            '','','','','','','',''
        ]
      ]).widths([120, 40, 70, 40, 40,40,40,40,40]).alignment('center').fontSize(9). layout('noBorders').end
    );

    /*pdf.add(
      pdf.ln(2)
    );

    pdf.add(
      new Table([
        [ 
          {text:'Detalle de Productos/Pedido ', bold:true, colSpan:9, fontSize:11, alignment:'left'}, 
            '','','','','','','',''
        ],
        [ {text:'Producto / Familia', bold:true}, 
          {text:'Marca', bold:true}, 
          {text:'GTIN', bold:true}, 
          {text:'Tipo de empaque', bold:true}, 
          {text:'Unidades por empaque', bold:true}, 
          {text:'Código pallet', bold:true} ,
          {text:'Código caja', bold:true} ,
          {text:'Lote / Serial', bold:true} ,
          {text:'Caducidad prox', bold:true} 
        ]
      ]).widths([100, 50, 70, 40, 40,40,40,40,50]).alignment('center').fontSize(9).end
    );
    
    if(this.responseDocsTotalPallet.docDetalleTotalPalletList.totalPallet==0){
      pdf.add(
        new Table([
          [ {text: 'Sin Pallets', colSpan:9, bold:true}, 
          '','','','','','','',''
        ]
      ]).widths([100, 50, 70, 40, 40,40,40,40,50]).alignment('center').fontSize(8).end
      );
    }else{
      //Productos FOR
      for (let pallets of this.responseDocsProductosIndi.docDetalleProductoIndiList) {
        pdf.add(
          new Table([
            [ {text: pallets.producto, bold:true}, 
            {text: pallets.ciu, bold:true}, 
            {text: pallets.gtin, bold:true}, 
            {text: pallets.tipoEmpaque, bold:true}, 
            {text: pallets.cantidad/pallets.numPallet, bold:true}, 
            {text: pallets.codigoPallet, bold:true} ,
            {text: ' ', bold:true} ,
            {text: pallets.lote, bold:true} ,
            {text: pallets.fechaCaducidad, bold:true} 
          ]
        ]).widths([100, 50, 70, 40, 40,40,40,40,50]).alignment('center').fontSize(8).end
        );
      }
    }*/

    pdf.add(
      pdf.ln(2)
    );
    
    pdf.add(
      new Table([
        [ 
          {text:'Detalle de Productos/Pedido ', bold:true, colSpan:8, fontSize:11, alignment:'left'}, 
            '','','','','','',''
        ],
        [ {text:'Producto / Familia', bold:true}, 
          {text:'Marca', bold:true}, 
          {text:'GTIN', bold:true}, 
          {text:'Tipo de empaque', bold:true}, 
          {text:'Unidades por empaque', bold:true}, 
          //{text:'Código pallet', bold:true} ,
          {text:'Código caja', bold:true} ,
          {text:'Lote / Serial', bold:true} ,
          {text:'Caducidad prox', bold:true} 
        ]
      ]).widths([100, 50, 70, 40, 40,80,40,50]).alignment('center').fontSize(9).end
    );

    if(this.responseDocsTotalCajas.docDetalleTotalCajasList.totalCajas==0){
      pdf.add(
        new Table([
          [ {text: 'Sin Cajas', bold:true}
        ]
      ]).widths(['*']).alignment('center').fontSize(8).end
      );
    }else{
      //Productos FOR
      for (let cajas of this.responseDocsProductosCajas.docDetalleProductoCajasList) {
        pdf.add(
          new Table([
            [ {text: cajas.producto, bold:true}, 
            {text: cajas.ciu, bold:true}, 
            {text: cajas.gtin, bold:true}, 
            {text: cajas.tipoEmpaque, bold:true}, 
            {text: cajas.cantidad/cajas.numCajas, bold:true}, 
            //{text: cajas.codigoPallet, bold:true} ,
            {text: cajas.codigoCaja.includes("X") ? cajas.codigoCaja.split("X").join("") : cajas.codigoCaja, bold:true} ,
            {text: cajas.lote, bold:true} ,
            {text: cajas.fechaCaducidad, bold:true} 
          ]
        ]).widths([100, 50, 70, 40, 40,80,40,50]).alignment('center').fontSize(8).end
        );
      }
    }

    /*for (let products of this.responseDocsProductosIndi.docDetalleProductoIndiList) {
      pdf.add(
        new Table([
          ["    ",''+products.producto, ''+products.ciu,''+products.lote,''+products.fechaCaducidad, ''+products.numSerie],
        ]).widths([30, 120, 80, 80, 80, 80]).alignment('center').fontSize(9).end
      );
    }*/


    pdf.footer(
      new Columns([
        new Table(
        [
          [ 
            {text:'Controla tus inventarios y automatiza las comunicaciones de tu almacen con Traceit Stock   https://stock.traceit.net' }]
        ]).widths(['*']).alignment('center').layout('noBorders').fontSize(7).end
      ]).end


    );

    

    if ( screen.width <= 576)
		{
			pdf.create().download();
		}
		if (screen.width > 576 && screen.width <= 768)
		{
      pdf.create().download();
		}  
		if (screen.width > 768 && screen.width <= 992)
		{
			pdf.create().open();
		}  
		if (screen.width > 992 && screen.width <= 1200)
		{
			pdf.create().open();
		} 
		if (screen.width > 1200){
			pdf.create().open();
    }

    
  }

}
