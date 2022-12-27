import { Component, OnInit, SecurityContext } from '@angular/core';
import { MatDialog, MatSnackBar, MatSnackBarRef, MatTableDataSource } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { Pipe, PipeTransform } from '@angular/core';
import { OverlayRef } from '@angular/cdk/overlay';
import { DataServices } from '../Interfaces/Services/general.service';
import { ModalGaleriaComponent } from './modal-galeria/modal-galeria.component';
import {
  PdfMakeWrapper,
  Table,
  Img,
  Columns,
  Cell,
  Txt,
} from 'pdfmake-wrapper';
import pdfMake from "pdfmake/build/pdfmake";
import pdfFonts from "pdfmake/build/vfs_fonts";
import { parse } from '@fortawesome/fontawesome-svg-core';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { DialogoNotificacionesComponent } from './dialogo-notificaciones/dialogo-notificaciones.component';

import { LocationService } from '../Interfaces/Services/location.service';
@Component({
  selector: 'app-tracking',
  templateUrl: './tracking.component.html',
  styleUrls: ['./tracking.component.css']
})
export class TrackingComponent implements OnInit {

  ciu: string = "";
  PDFurl: string = "";
  type: number = 0; // 0 (C o P) ciu , 1 (A) operacion
  operacion: number = 0;

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

  lat: string = "";
  lon: string = "";

  codigoQR: string = "";
  codigoQR2: string = "";
  movimientoId: number = 0;
  aDate = new Date();
  fechaIngresoDe: string = "";

  //recepcion
  nombre: string = "";
  apellido: string = "";
  cargo: string = "";


  overlayRef: OverlayRef;

  displayedColumns: string[] = ['producto', 'ciu', 'gtin', 'cajapallet', 'ucaja', 'tunidades', 'caducidad'];
  displayedColumnsPallet: string[] = ['producto', 'marca', 'gtin', 'empaque', 'uempaque', 'ccaja', 'lote', 'caducidad'];
  dataSource = new MatTableDataSource<any>([]);
  dataSourcePallets = new MatTableDataSource<any>([]);
  datasourceCajas = new MatTableDataSource<any>([]);

  arrayAlertas: any[] = [];
  arrayDocumentos: any[] = [];
  arrayDocumentosMostrar: any[] = [];
  infoFamilia: any[] = [];

  notificacionAutentidad: any[] = [
    {
      tituloAlerta: "",
      tipoAlerta: 0,
      tipoReporte: 0,
      descripcion: ""
    }
  ]

  contador: number = 0;
  mostrarArchivos: boolean = false;
  showingFiles = []
  files = []

  constructor(
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private sanitizer: DomSanitizer,
    private dataService: DataServices,
    private _dialog: MatDialog,
    private _locationService: LocationService,
    public dialog: MatDialog
  ) {
    PdfMakeWrapper.setFonts(pdfFonts);
  }

  locationNavegador: any = {
    latitude: '',
    longitude: ''
  };
  tipoBusqueda: string = '';
  ngOnInit() {
    this.ciu = "";
    this.PDFurl = '={"T":"C","P":40,"I":"38E035D23B605","F":"38E035D23B62C","ID":"-PL-1-Bx-2"}';

    let stringerino: string = window.location.href;
    if (stringerino.includes("qr=")) {
      let clave = stringerino.split("=");
      let resultado: string = clave[1];
      this.codigoQR = decodeURI(resultado);
      this.aDate.setDate(this.aDate.getDate());
      this.fechaIngresoDe = this.aDate.toISOString()

      if (resultado.length) {

        let jsorino = JSON.parse(this.codigoQR);
        let type = jsorino["T"];

      console.log('Pintamos toda la casa, sin derramar una gota de pintura, QUE ES ESOO!!', stringerino, clave, resultado, jsorino, type);
      
        this.tipoBusqueda = type;
        if (type == "C") {

          let inicial = jsorino["I"];
          if (inicial.length) {
            this.type = 0;
            this.ciu = inicial;
            this.obtenerMovimiento();
          } else {
            this.openSnack("El QR no es válido. (1)", "Aceptar");
          }

        } else if (type == "A") {
          // let inicial = jsorino["ID"];
          let inicial = jsorino["I"];

          if (inicial.length) {
            this.type = 1;
            this.ciu = inicial;
            // this.movimientoId = parseInt(inicial);
            this.obtenerMovimiento();
            // this.Busqueda();

          } else {
            this.openSnack("El QR no es válido. (2)", "Aceptar");
          }

        } else if (type == "P") {
          let inicial = jsorino["I"];
          if (inicial.length) {
            this.type = 0;
            this.ciu = inicial;
            this.obtenerMovimiento();
          } else {
            this.openSnack("El QR no es válido. (3)", "Aceptar");
          }
        }
      } else {
        this.openSnack("El QR tiene una liga vacia o no es válido. (-1)", "Aceptar");
      }


    }


  }

  openModalAngular(name: string, type: string, file?){
    if (name.toLocaleLowerCase().includes(".png") || name.toLocaleLowerCase().includes(".jpg") || name.toLocaleLowerCase().includes(".jpeg")) {
      type = "Foto";
      this.dialog.open(ModalGaleriaComponent,
        {
          panelClass: 'app-modal-galeria',
          width: '100%',
          data:  { name: name , type: type , icono : file}
      })
    } else if (name.toLocaleLowerCase().includes(".pdf")){
      type = "File";
      this.dialog.open(ModalGaleriaComponent,
        {
          panelClass: 'app-modal-galeria',
          width: '100%',
          height: '70%',
          data:  { name: name , type: type , icono : file}
      })
    } else {
      window.open(name, '_blank');
    }
  }

  openSingleFile(search: string) {
    let file = this.files.find((e) => e.name === search);
    this.openModalAngular(file.file, file.type);
  }


  async getLocation() {
    this._locationService.getPosition().then(pos => {
        this.locationNavegador = {
          latitude: pos.lat,
          longitude: pos.lng
        }
        this.obtenerCoord();
        console.log('Localizacion', this.locationNavegador);
    });
  }


  obtenerMovimiento() {
    let data = {
      ciu: this.ciu,
      type: this.type
    };

    setTimeout(() => {
      if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);

    this.dataService.postData<any>("Tracking/SearchLastMovement", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(data, "Recibido");
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          if (data["moveId"] == 0) {
            this.openSnack("Este CIU no cuenta con movimientos registrados.", "Aceptar");
          } else {
            this.movimientoId = data["moveId"];
            this.BuscarDocumentosYAlertas();
          }
        }


      },
      error => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("error", error);
        if (error.error.hasOwnProperty("messageEsp")) {
        } else {
          console.log(error);
        }
      }
    );

  }

  BuscarDocumentosYAlertas() {
    let data = {
      ciu: this.ciu,
      type: this.movimientoId,
      tipoBusqueda: this.tipoBusqueda
    };

    setTimeout(() => {
      if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);

    this.dataService.postData<any>("Tracking/SearchDocs", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(data, "Recibido");
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          console.log(data);
          this.arrayAlertas = data["alertas"];
          this.infoFamilia = data["infoFamilia"];
          this.arrayDocumentos.push({ familia: data["docsFamilia"], stock: data["docsStock"] });
          this.getLocation();
          //this.obtenerCoord();
          if (data["alertas"].length) {
            this.mostrarNotificaciones();
          } else {
            this.arrayAlertas = this.notificacionAutentidad;
            this.mostrarNotificaciones();
          }
        }

      },
      error => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("error", error);
        if (error.error.hasOwnProperty("messageEsp")) {
        } else {
          console.log(error);
        }
      }
    );
  }

  //Funcion para realizar la busqueda  (Las variables son las que se envian para hacer la búsqueda)
  Busqueda() {

    if (this.movimientoId == 0) {
      this.openSnack("El movimiento no puede ser nulo o inexistente", "Aceptar");
      return;
    }

    var request = {
      movimientoId: this.movimientoId
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this.dataService.postData<any>("Movimientos/searchDataMovimientoGeneral", sessionStorage.getItem("token"), request).subscribe(
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

    if (this.movimientoId == 0) {
      this.openSnack("El movimiento no puede ser nulo o inexistente", "Aceptar");
      return;
    }

    var request = {
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDataMovimientoDestinatario", sessionStorage.getItem("token"), request).subscribe(
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
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDataMovimientoRemitente", sessionStorage.getItem("token"), request).subscribe(
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
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDataMovimientoTransportista", sessionStorage.getItem("token"), request).subscribe(
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
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDataMovimientoInfoLegal", sessionStorage.getItem("token"), request).subscribe(
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
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDocDetalleProductos", sessionStorage.getItem("token"), request).subscribe(
      data => {
        console.log('DATOS dE REPONSE', data);
        this.responseDocsProductos = data;
        this.dataSource.data = this.responseDocsProductos.docDetalleProductoList;
        this.BusquedaDetalleProductos();
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
  BusquedaDetalleProductos() {

    var request = {
      movimientoId: this.codigoQR
    }

    this.dataService.postData<any>("Movimientos/searchDocDetalleProductosIndi", sessionStorage.getItem("token"), request).subscribe(
      data => {
        // aquíu esta el seto prro
        this.responseDocsProductosIndi = data;
        this.dataSourcePallets.data = this.responseDocsProductosIndi.docDetalleProductoIndiList;
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
  BusquedaDetalleProductos2() {

    var request = {
      movimientoId: this.movimientoId
    }

    //Obtener valor de rango en formato Json
    if (JSON.parse('{ "return":' + this.codigoQR + ' }')) {
      let objJson = JSON.parse('{ "return":' + this.codigoQR + ' }')
      let inicial = objJson.return.I;
      let final = objJson.return.F;
      let tipo = objJson.return.T;

      request["codigoTipo"] = tipo;
      request["codigoI"] = inicial;
      request["codigoF"] = final;
    }


    this.dataService.postData<any>("Movimientos/searchDocDetalleProductosCajas", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsProductosCajas = data;
        this.datasourceCajas.data = this.responseDocsProductosCajas.docDetalleProductoCajasList;
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
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDocDetalleTotalProd", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsTotalProd = data;
        this.BusquedaDocsTotalPallet();
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
  BusquedaDocsTotalPallet() {

    var request = {
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDocDetalleTotalPallet", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsTotalPallet = data;

        if (this.responseDocsTotalPallet.docDetalleTotalPalletList.totalPallet == 0) {
          this.dataSourcePallets.data = [];
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
  }

  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  BusquedaDocsTotalCajas() {

    var request = {
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDocDetalleTotalCajas", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDocsTotalCajas = data;
        console.log('DARA CAJAS', data);
        if (this.responseDocsTotalCajas.docDetalleTotalCajasList.totalCajas == 0) {
          this.datasourceCajas.data = [];
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
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDocDetalleTotalCantidad", sessionStorage.getItem("token"), request).subscribe(
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
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDocDetalleTotalPeso", sessionStorage.getItem("token"), request).subscribe(
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

    //funcion fecha se creo el 25/04/2022 por Frida Ocampo
    padTo2Digits (num) {
      return num.toString().padStart(2,'0');
    }
    formatDate(date){
      return [
        this.padTo2Digits(date.getDate()),
        this.padTo2Digits(date.getMonth() + 1),
        date.getFullYear(),
      ].join('/');
    }
  //Funcion para realizar la busqueda  de la info que irá en la tabla del documento de info legal
  BusquedaDocsFechaMin() {

    var request = {
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDocDetalleFechaMin", sessionStorage.getItem("token"), request).subscribe(
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
      movimientoId: this.movimientoId
    }

    this.dataService.postData<any>("Movimientos/searchDataObservcion", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseDataComentario = data;
        this._overlay.close(this.overlayRef);
        this.generatePDF();
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

  async generatePDF() {

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
    pdf.pageMargins([20, 60, 20, 30]);

    let tableHeader = new Table([
      [{ text: this.responseDataRem.movimientosDataRemitenteRecepList.nombreCompaniaHeader, bold: true, fontSize: 18 }
      ]
    ]).widths(['*']).alignment('center').color('#012152').alignment('left').margin([20, 20, 40, 0]).end;

    if (this.responseDataRem.movimientosDataRemitenteRecepList.nombreCompaniaHeader == "") {
      tableHeader = new Table([
        [new Cell(new Txt(' ').bold().fontSize(12).end).fillColor("#012152").end]
      ]).widths(['*']).alignment('center').color('#012152').alignment('left').margin([20, 20, 40, 0]).end;
    }

    pdf.header(
      new Columns([
        tableHeader,
        await new Img('../../assets/img/img-logo-stock-3.png').width('100').margin([-20, 20, 20, 0]).build()
      ]).end,
    );

    pdf.add(
      pdf.ln(1)
    );

    let textCajaId = this.responseDocsProductosCajas.docDetalleProductoCajasList.length > 0 ? this.responseDocsProductosCajas.docDetalleProductoCajasList[0].codigoCaja.split(',')[0] : '';
    let date = new Date(this.fechaIngresoDe);
    let day = date.getDate();
    let month = date.getMonth() + 1;
    let year = date.getFullYear();
    pdf.add(
      new Columns([
        new Table([
          ['', '', ''], ['', '', ''], ['', '', ''], ['', '', ''], ['', '', ''], ['', '', ''],
          [{ text: 'Guía de Embarque', bold: true, alignment: 'left', fontSize: 16, colSpan: 3 }],
          ['', '', ''], ['', '', ''], ['', '', ''], ['', '', ''], ['', '', ''], ['', '', ''], ['', '', ''],
          [ {text:'Fecha: ', alignment:'right', bold:true}, 
          {text: this.formatDate(date), alignment:'left', color: 'gray'},
            ''
          ],
          [{ text: 'OP Interna: ', alignment: 'right', bold: true },
          { text: this.responseData.movimientosDataGeneralRecepList.referenciaInterna, alignment: 'left', color: 'gray' },
            ''
          ],
          [{ text: 'No. cliente: ', alignment: 'right', bold: true },
          { text: this.responseDataDes.movimientosDataDestinatarioRecepList.numeroC, alignment: 'left', color: 'gray' },
            ''
          ],
          [{ text: 'Cliente: ', alignment: 'right', bold: true },
          { text: this.responseDataDes.movimientosDataDestinatarioRecepList.nombreCompaniaD, alignment: 'left', color: 'gray' },
            ''
          ],
          [{ text: 'OP Cliente: ', alignment: 'right', bold: true },
          { text: this.responseData.movimientosDataGeneralRecepList.referenciaExterna, alignment: 'left', color: 'gray' },
            ''
          ]
        ]).widths([80, 150, 40]).fontSize(9).layout('noBorders').end,
        new Table([
          [{ qr: enviroments.qrUrl + this.responseData.movimientosDataGeneralRecepList.codigoQR, alignment: 'center', fit: 100, colSpan: 2 },
            ''
          ],
          [{ text: textCajaId, color: 'gray', colSpan: 2 }]
        ]).widths([80, 150]).alignment('center').fontSize(9).layout('noBorders').end
      ]).end
    );

    pdf.add(
      pdf.ln(1)
    );

    // Domicilio Destinatario
    let fullDomicilioDest = this.responseDataDes.movimientosDataDestinatarioRecepList.domicilioD;
    fullDomicilioDest += fullDomicilioDest != "" ?
      (this.responseDataDes.movimientosDataDestinatarioRecepList.ciudadD != '' ? ', ' + this.responseDataDes.movimientosDataDestinatarioRecepList.ciudadD : '')
      :
      (this.responseDataDes.movimientosDataDestinatarioRecepList.ciudadD != '' ? this.responseDataDes.movimientosDataDestinatarioRecepList.ciudadD : '');
    fullDomicilioDest += fullDomicilioDest != "" ?
      (this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND != 'Seleccionar un estado' ? ', ' + this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND : '')
      :
      (this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND != 'Seleccionar un estado' ? this.responseDataDes.movimientosDataDestinatarioRecepList.estadoND : '');
    fullDomicilioDest += fullDomicilioDest != "" ?
      (this.responseDataDes.movimientosDataDestinatarioRecepList.paisND != 'Seleccionar un pais' ? ', ' + this.responseDataDes.movimientosDataDestinatarioRecepList.paisND : '')
      :
      (this.responseDataDes.movimientosDataDestinatarioRecepList.paisND != 'Seleccionar un pais' ? this.responseDataDes.movimientosDataDestinatarioRecepList.paisND : '');
    // Domicilio Remitente
    let fullDomicilioRem = this.responseDataRem.movimientosDataRemitenteRecepList.domicilioR;
    fullDomicilioRem += fullDomicilioRem != "" ?
      (this.responseDataRem.movimientosDataRemitenteRecepList.cpR != '' && this.responseDataRem.movimientosDataRemitenteRecepList.cpR != '0' ? ', C.P. ' + this.responseDataRem.movimientosDataRemitenteRecepList.cpR : '')
      :
      (this.responseDataRem.movimientosDataRemitenteRecepList.cpR != '' && this.responseDataRem.movimientosDataRemitenteRecepList.cpR != '0' ? 'C.P. ' + this.responseDataRem.movimientosDataRemitenteRecepList.cpR : '');
    fullDomicilioRem += fullDomicilioRem != "" ?
      (this.responseDataRem.movimientosDataRemitenteRecepList.ciudadR != '' ? ', ' + this.responseDataRem.movimientosDataRemitenteRecepList.ciudadR : '')
      :
      (this.responseDataRem.movimientosDataRemitenteRecepList.ciudadR != '' ? this.responseDataRem.movimientosDataRemitenteRecepList.ciudadR : '');
    fullDomicilioRem += fullDomicilioRem != "" ?
      (this.responseDataRem.movimientosDataRemitenteRecepList.estadoNR != 'Seleccionar un estado' ? ', ' + this.responseDataRem.movimientosDataRemitenteRecepList.estadoNR : '')
      :
      (this.responseDataRem.movimientosDataRemitenteRecepList.estadoNR != 'Seleccionar un estado' ? this.responseDataRem.movimientosDataRemitenteRecepList.estadoNR : '');
    fullDomicilioRem += fullDomicilioRem != "" ?
      (this.responseDataRem.movimientosDataRemitenteRecepList.paisNR != 'Seleccionar un pais' ? ', ' + this.responseDataRem.movimientosDataRemitenteRecepList.paisNR : '')
      :
      (this.responseDataRem.movimientosDataRemitenteRecepList.paisNR != 'Seleccionar un pais' ? this.responseDataRem.movimientosDataRemitenteRecepList.paisNR : '');

    pdf.add(
      new Columns([
        new Table([
          [{ text: 'Destinatario: ', alignment: 'right', bold: true, fontSize: 11 },
          {
            text: /*this.responseDataDes.movimientosDataDestinatarioRecepList.nombreCompaniaD+' '+*/
              this.responseDataDes.movimientosDataDestinatarioRecepList.rzCompaniaD, alignment: 'left', color: 'gray'
          }
          ],
          [{ text: 'Dirección: ', alignment: 'right', bold: true },
          { text: fullDomicilioDest, alignment: 'left', color: 'gray' }
          ],/*
          [{ text: 'CP: ', alignment: 'right', bold: true },
          { text: (this.responseDataDes.movimientosDataDestinatarioRecepList.cpD == "0" ? "" : this.responseDataDes.movimientosDataDestinatarioRecepList.cpD), alignment: 'left', color: 'gray' }
          ],
          [{ text: 'Tel: ', alignment: 'right', bold: true },
          { text: this.responseDataDes.movimientosDataDestinatarioRecepList.telefonoD, alignment: 'left', color: 'gray' }
          ],
          [{ text: 'Atn: ', alignment: 'right', bold: true },
          {
            text: this.responseDataDes.movimientosDataDestinatarioRecepList.nombreDestinatario + ' ' +
              this.responseDataDes.movimientosDataDestinatarioRecepList.apellidoDestinatario, alignment: 'left', color: 'gray'
          }
          ]*/
        ]).widths([80, 150]).fontSize(9).end,
        new Table([
          [{ text: 'Remitente: ', alignment: 'right', bold: true, fontSize: 11 },
          {
            text: this.responseDataRem.movimientosDataRemitenteRecepList.nombreCompaniaR + ' ' +
              this.responseDataRem.movimientosDataRemitenteRecepList.rzCompaniaR, alignment: 'left', color: 'gray'
          }
          ],
          [{ text: 'Dirección: ', alignment: 'right', bold: true },
          { text: fullDomicilioRem, alignment: 'left', color: 'gray' }
          ],
          [
            {
              text: 'Atn: ', alignment: 'right', bold: true 
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
          [{ text: 'Transporte: ', alignment: 'right', bold: true, fontSize: 11 },
          { text: this.responseDataTrans.movimientosDataTransportistaList.transportista, alignment: 'left', color: 'gray' }
          ],
          [{ text: 'Fecha embarque: ', alignment: 'right', bold: true },
          { text: this.responseDataTrans.movimientosDataTransportistaList.fechaEmbarque, alignment: 'left', color: 'gray' }
          ]
          ,
          [{ text: 'Referencia: ', alignment: 'right', bold: true },
          { text: this.responseDataTrans.movimientosDataTransportistaList.numReferencia, alignment: 'left', color: 'gray' }
          ]
        ]).widths([80, 150]).fontSize(9).end,
        new Table([
          [{ text: 'Observaciones: ', alignment: 'right', bold: true, fontSize: 11 },
          { text: this.responseDataComentario.movimientosDataObservacionRecepList.observacion, alignment: 'left', color: 'gray' }
          ],
          [{ text: 'Dimensiones por caja (cms): ', alignment: 'right', bold: true },
          { text: this.responseDataComentario.movimientosDataObservacionRecepList.dimensionesCaja, alignment: 'left', color: 'gray' }
          ],
          [{ text: 'Peso por caja (Kg): ', alignment: 'right', bold: true },
          { text: this.responseDataComentario.movimientosDataObservacionRecepList.pesoCaja, alignment: 'left', color: 'gray' }
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
          [{ text: 'Resumen de Productos/Pedido: ', bold: true, colSpan: 8, fontSize: 11, alignment: 'left' },
            '', '', '', '', '', '', ''
          ],
          [{ text: 'Producto/Familia: ', bold: true },
          { text: 'Marca: ', bold: true },
          { text: 'GTIN: ', bold: true },
          { text: 'Código Pallets: ', bold: true },
          { text: 'Cajas por pallet: ', bold: true },
          { text: 'Unidades por caja: ', bold: true },
          { text: 'Total de unidades: ', bold: true },
          { text: 'Caducidad prox: ', bold: true },
          ]
        ]).widths([100, 65, 55, 35, 40, 50, 60, 75]).alignment('center').fontSize(9).end,
      ]).end
    );

    //Productos FOR
    for (let products of this.responseDocsProductos.docDetalleProductoList) {
      var a;
      if (products.numPallet == 0) {
        a = 1;
      } else {
        a = products.numPallet;
      }
      pdf.add(
        new Table([
          [{ text: products.producto, alignment: 'right' },
          { text: products.ciu, alignment: 'right' },
          products.gtin,
            0,
          products.numCajas / a,
          products.cantidad / products.numCajas,
          products.cantidad,
          products.fechaCaducidad
          ],
        ]).widths([100, 65, 55, 35, 40, 50, 60, 75]).alignment('center').fontSize(8).end
      );
    }

    /*pdf.add( //Descomentar para separar codigos
      new Table([
        [
          { text: '', bold: true, colSpan: 9, fontSize: 11, alignment: 'left', pageBreak: 'before' },
          '', '', '', '', '', '', '', ''
        ]
      ]).widths([120, 40, 70, 40, 40, 40, 40, 40, 40]).alignment('center').fontSize(9).layout('noBorders').end
    );*/

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
          { text: 'Detalle de Productos/Pedido ', bold: true, colSpan: 8, fontSize: 11, alignment: 'left' },
          '', '', '', '', '', '', ''
        ],
        [{ text: 'Producto / Familia', bold: true },
        { text: 'Marca', bold: true },
        // { text: 'GTIN', bold: true },
        { text: 'Tipo de empaque', bold: true },
        { text: 'Unidades por empaque', bold: true },
        {text:'Código pallet', bold:true} ,
        { text: 'Código caja', bold: true },
        { text: 'Lote / Serial', bold: true },
        { text: 'Caducidad prox', bold: true }
        ]
      ]).widths([100, 50,/* 70,*/ 40, 40,50, 80, 40, 50]).alignment('center').fontSize(9).end
    );

    if (this.responseDocsTotalCajas.docDetalleTotalCajasList.totalCajas == 0) {
      pdf.add(
        new Table([
          [{ text: 'Sin Cajas', bold: true }
          ]
        ]).widths(['*']).alignment('center').fontSize(8).end
      );
    } else {
     /* //Productos FOR
      for (let cajas of this.responseDocsProductosCajas.docDetalleProductoCajasList) {
        pdf.add(
          new Table([
            [{ text: cajas.producto, bold: true },
            { text: cajas.ciu, bold: true },
            { text: cajas.gtin, bold: true },
            { text: cajas.tipoEmpaque, bold: true },
            { text: cajas.cantidad / cajas.numCajas, bold: true },
            //{text: cajas.codigoPallet, bold:true} ,
            {text: cajas.codigoCaja.includes("X") ? cajas.codigoCaja.split("X").join("") : cajas.codigoCaja, bold:true} ,
            {text: cajas.lote, bold:true} ,
            {text: cajas.fechaCaducidad, bold:true} 
          ]
        ]).widths([100, 50, 70, 40, 40,80,40,50]).alignment('center').fontSize(8).end
        );*/
        //Productos FOR
        console.log('ESTO ES LO QUE SE MUESTRA, ',  this.responseDocsProductosCajas.docDetalleProductoCajasList);
       for (let cajas of this.responseDocsProductosCajas.docDetalleProductoCajasList) {

            pdf.add(
              new Table([
                [{ text: cajas.producto, bold: true },
              { text: cajas.ciu, bold: true },
             // { text: cajas.gtin, bold: true },
              { text: cajas.tipoEmpaque, bold: true },
              { text: cajas.cantidad / cajas.numCajas, bold: true },
              {text: cajas.codigoPallet, bold:true} ,
              /*{text: cajas.codigoCaja.includes("X") ? cajas.codigoCaja.split("X").join("") : cajas.codigoCaja, bold:true}*/
              {text: cajas.codigoCaja, bold:true} ,
              {text: cajas.lote, bold:true} ,
              {text: cajas.fechaCaducidad, bold:true} 
              ]
            ]).widths([100,50, /*70,*/ 40,40,50,80,40,50]).alignment('center').fontSize(8).end
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
              { text: 'Controla tus inventarios y automatiza las comunicaciones de tu almacen con Traceit Stock   https://stock.traceit.net' }]
          ]).widths(['*']).alignment('center').layout('noBorders').fontSize(7).end
      ]).end


    );



    if (screen.width <= 576) {
      pdf.create().download();
    }
    if (screen.width > 576 && screen.width <= 768) {
      pdf.create().download();
    }
    if (screen.width > 768 && screen.width <= 992) {
      pdf.create().open();
    }
    if (screen.width > 992 && screen.width <= 1200) {
      pdf.create().open();
    }
    if (screen.width > 1200) {
      pdf.create().open();
    }


  }

  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string, type = 0, contador = 0, duration = 5000) => {
    let snacjko: MatSnackBarRef<any>;
    switch (type) {
      case 0:
        snacjko = this.snack.open(message, action, {
          duration: duration
        });
        break;
      case 1:
        snacjko = this.snack.open(message, action);

        snacjko.afterDismissed().subscribe(() => {

          if (this.arrayAlertas.length > contador) {

            this.openSnack(this.arrayAlertas[contador]['text'], 'Aceptar', 1, this.arrayAlertas[contador]['id']);

          };

        });

        break;
      default:
        break;
    }
  }

  mostrarNotificaciones() {

    const dialogRef = this._dialog.open(DialogoNotificacionesComponent, {
      disableClose: false,
      role: 'alertdialog',
      width: '400',
      height: '200',
      minWidth: '350',
      minHeight: '150',
      panelClass: 'custom-dialog-container',
      data: {
        notificacion: this.arrayAlertas[this.contador], familia: this.infoFamilia, codigo: this.ciu, galeria: true
      }
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (this.arrayAlertas.length > (this.contador + 1)) {
        this.contador++;
        this.mostrarNotificaciones();
      } else {
        this.contador = 0;
      }
    })
  }

  mostrarDocs(type = -1) {
    this.arrayDocumentosMostrar = [];

    switch (type) {
      case -1:
        //los documentos PDFs
        this.arrayDocumentos[0]["familia"].forEach(doc => {
          this.arrayDocumentosMostrar.push({
            nombre: doc.titulo,
            url: doc.url,
            tipo: -1
          });
        });

        if (this.arrayDocumentosMostrar.length == 0) {
          this.openSnack("No se cuenta con documentos disponibles", "Aceptar", 0, 0, 5000);
        }
        break;
      case 0:
        // Otros
        this.arrayDocumentos[0]["stock"].forEach(doc => {
          if (doc.tipoArchivo == 3) {
            this.arrayDocumentosMostrar.push({
              nombre: doc.nombreDoc,
              url: doc.documento,
              tipo: 1
            })
          }
        });

        if (this.arrayDocumentosMostrar.length == 0) {
          this.openSnack("No se cuenta con documentos disponibles", "Aceptar", 0, 0, 5000);
        }
        break;
      case 1:
        // Fotos
        this.arrayDocumentos[0]["stock"].forEach(doc => {
          if (doc.tipoArchivo == 2) {
            this.arrayDocumentosMostrar.push({
              nombre: doc.nombreDoc,
              url: doc.documento,
              tipo: 1
            })
          }
        });

        if (this.arrayDocumentosMostrar.length == 0) {
          this.openSnack("No se cuenta con documentos disponibles", "Aceptar", 0, 0, 5000);
        }
        break;
      case 2:
        // Certificados
        this.arrayDocumentos[0]["stock"].forEach(doc => {
          if (doc.tipoArchivo == 1) {
            this.arrayDocumentosMostrar.push({
              nombre: doc.nombreDoc,
              url: doc.documento,
              tipo: 1
            })
          }
        });

        if (this.arrayDocumentosMostrar.length == 0) {
          this.openSnack("No se cuenta con documentos disponibles", "Aceptar", 0, 0, 5000);
        }
        break;
      case 3:
        // abrir guia de embarque
        this.Busqueda();
        break;
      default:
        break;
    }
  }

  descargarDocLegal(nombreDoc = "") {
    let params: any = {};
    params = {
      movimientoId: this.movimientoId,
      nombreDoc: nombreDoc
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, .001);

    this.dataService.postDataDownDocs<any>('Movimientos/DownloadDocsInfoLegal', sessionStorage.getItem("token"), params).subscribe(
      data => {
        this._overlay.close(this.overlayRef);
        let dataType = data.type;
        let binaryData = [];
        binaryData.push(data);
        let downloadLink = document.createElement('a');
        downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: dataType }));
        downloadLink.setAttribute('download', nombreDoc);
        document.body.appendChild(downloadLink);
        downloadLink.click();
      },
      error => {
        this._overlay.close(this.overlayRef);
        this.openSnack("Error al obtener el archivo", "Aceptar");
      }
    );
  }

  mostrarDocumento(url = "", tipo = -1) {

    switch (tipo) {
      case -1:
        this.openModalAngular(url, tipo.toString());
        //window.open(url, '_blank');
        break;
      case 1:
        this.descargarDocLegal(url);
        break;
      default:
        break;
    }

  }

  obtenerCoord() {
    this.dataService.getCoorde().subscribe(
      data => {
        this.lat = data["latitude"];
        this.lon = data["longitude"];
        this.registroLog();
      },
      error => {
        console.log("error al obtener la latitud y longitud", error);
      }
    )
  }

  registroLog() {
    let data = {
      ciu: this.ciu,
      lat: this.locationNavegador ? this.locationNavegador.latitude : this.lat,
      lon: this.locationNavegador ? this.locationNavegador.longitude : this.lon,
      jeson: this.codigoQR,
      tipo: 1 // 1 = Tracking, 2 = Origin
    };

    this.dataService.postData<any>("Tracking/RegistroLog", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log("Log Registrado ", data);
      },
      error => {
        console.log("Error al registrar el Log ", error);
      }
    );
  }

  listado(type: string) {
    this.showingFiles = this.files.filter((file) => file.type === type);
    this.mostrarArchivos = true;
  }

  registroRecepcion() {

    if (this.nombre == "" || this.apellido == "" || this.cargo == "") {
      this.openSnack("Faltan datos obligatorios.", "Aceptar");
      return;
    }



    let data = {
      movimientoId: this.movimientoId,
      nombre: this.nombre,
      apellido: this.apellido,
      cargo: this.cargo,
      fecha: `${new Date().getDate()}/${(new Date().getMonth() + 1)}/${new Date().getFullYear()} ${new Date().getHours()}:${new Date().getMinutes()}:${new Date().getSeconds()}`,
      lat: this.lat,
      lon: this.lon
    };

    this.dataService.postData<any>("Tracking/RegistroRecepcion", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log("Log Registrado ", data);
        this.nombre = "";
        this.apellido = "";
        this.cargo = "";

        this.openSnack("La firma de recepción ha sido aceptada.", "Aceptar");
      },
      error => {
        console.log("Error al registrar el Log ", error);
      }
    );
  }

}


@Pipe({ name: 'safe' })
export class SafePipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) { }
  transform(url) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
}