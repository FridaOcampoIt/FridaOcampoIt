import { Component, OnInit, Inject } from '@angular/core';

import { faPallet, faBoxOpen, faBox, faAddressBook, faAddressCard, faFileAlt, faBalanceScale } from "@fortawesome/free-solid-svg-icons";
import { PdfMakeWrapper, Table, Img, Columns, } from 'pdfmake-wrapper';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { DataServices } from '../../Interfaces/Services/general.service';
import { Router } from '@angular/router';
import { OverlayRef } from '@angular/cdk/overlay';
import { DialogoInformacionComponent } from '../dialogo-informacion/dialogo-informacion.component';
import { DialogoLegalComponent } from '../dialogo-legal/dialogo-legal.component';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import pdfFonts from "pdfmake/build/vfs_fonts";

@Component({
  selector: 'app-dialogo-detalle',
  templateUrl: './dialogo-detalle.component.html',
  styleUrls: ['./dialogo-detalle.component.less']
})
export class DialogoDetalleComponent implements OnInit {

  /**
   * variables de iconos
   */

  faPallet = faPallet;
  faBoxO = faBoxOpen;
  faBox = faBox;
  faAddressB = faAddressBook;
  faAddressC = faAddressCard;
  faFile = faFileAlt;
  faLegal = faBalanceScale;

  palletes: number = 0;
  boxes: number = 0;
  productos: number = 0;
  refInterna: number = 0;
  refExterna: number = 0;
  nomPedido: string = "";
  generalData: Object = {};


  overlayRef: OverlayRef;

  eventInfo: any = [];
  eventLegalInfo: any = [];
  eventProductsInfo: any = [];
  eventRecipientInfo: any = [];
  eventSenderInfo: any = [];

  movimientoId: number = 0;

  allDatas: any = {};

  constructor(
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router,
    private _dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  ngOnInit() {
    this.palletes = 0;
    this.boxes = 0;
    this.productos = 0;

    this.obtenerDatos();
  }

  /**
   */
  obtenerDatos(campo = 6) {
    let data: any = {
      trackingId: this.data["id"],
      opc: campo
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("Tracking/GetTrackingInfo", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {

          this.generalData["eventInfo"] = data["eventInfo"][0];

          this.nomPedido = data["eventInfo"][0].nombreAgrupacion;
          this.palletes = data["eventInfo"][0].numeroPallet;
          this.boxes = data["eventInfo"][0].numeroCajas;
          this.productos = data["eventInfo"][0].numeroPallet;
          this.refInterna = data["eventInfo"][0].referenciaInterna;
          this.refExterna = data["eventInfo"][0].referenciaExterna;

          this.eventInfo = data["eventInfo"][0];
          this.eventLegalInfo = data["eventLegalInfo"][0];
          this.eventProductsInfo = data["eventProductsInfo"];
          this.eventRecipientInfo = data["eventRecipientInfo"][0];
          this.eventSenderInfo = data["eventSenderInfo"][0];

          this.allDatas = data;
          this.movimientoId = data["eventInfo"][0].movimientoId;
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

  dialogoInformacion(action = 0) {

    let _dialogRef = null;

    /**
     * 0 - información Remitente
     */
    if (action == 0) {
      _dialogRef = this._dialog.open(DialogoInformacionComponent, {
        panelClass: 'dialog-aprod',
        disableClose: false,
        data: { id: this.data["id"], titulo: "Información de remitente", action: action }
      })
    } else {
      _dialogRef = this._dialog.open(DialogoInformacionComponent, {
        panelClass: 'dialog-aprod',
        disableClose: false,
        data: { id: this.data["id"], titulo: "Información destinatario ", action: action }
      })
    }

  }

  dialogoLegal() {

    const _dialogRef = this._dialog.open(DialogoLegalComponent, {
      panelClass: 'dialog-aprod',
      disableClose: false,
      data: { id: this.data["id"], titulo: "Información legal " }
    })

  }

  async generatePDF() {

    PdfMakeWrapper.setFonts(pdfFonts);
    const pdf = new PdfMakeWrapper();


    pdf.info({
      title: 'PDF Detalle Envío',
      author: 'Trace IT',
      subject: ''
    });

    pdf.pageOrientation('landscape');
    pdf.pageMargins([20, 20, 20, 20]);

    pdf.add(
      new Columns([
        new Table(
          [
            [{ text: this.eventInfo.nombreAgrupacion, bold: true },
            { qr: this.eventInfo.codigoQR, fit: 75 }
            ],
            [' ', ''],
            [{ text: 'Referencia interna: ', marginLeft: 30 }, this.eventInfo.referenciaInterna],
            [{ text: 'Referencia externa: ', marginLeft: 30 }, this.eventInfo.referenciaExterna]
          ]).widths(['auto', 'auto']).alignment('center').layout('noBorders').end,
        new Table(
          [
            [{ text: 'Destinatario', bold: true },
            { text: 'Información de Transporte', bold: true },
            { text: 'Remitente', bold: true }
            ],
            [{ text: 'Compañia / Razón Social: \n ' + this.eventRecipientInfo.nombreCompaniaD + ' ' + this.eventRecipientInfo.rzCompaniaD, marginLeft: 5, fontSize: '10' },
            { text: 'Núm de Referencia: \n ' + this.allDatas["eventTransportInfo"][0].numReferencia, marginLeft: 5, fontSize: '10' },
            { text: 'Compañia / Razón Social: \n ' + this.eventSenderInfo.nombreCompaniaR + ' ' + this.eventSenderInfo.rzCompaniaR, marginLeft: 5, fontSize: '10' }
            ],
            [{ text: 'Dirección: \n ' + this.eventRecipientInfo.domicilioD, marginLeft: 5, fontSize: '10' },
            { text: 'Comp. Transportista: \n ' + this.allDatas["eventTransportInfo"][0].transportista, marginLeft: 5, fontSize: '10' },
            { text: 'Contacto: \n ' + this.eventSenderInfo.nombreRemitente + ' ' + this.eventSenderInfo.apellidoRemitente, marginLeft: 5, fontSize: '10' }
            ],
            [{ text: 'Contacto: \n ' + this.eventRecipientInfo.nombreDestinatario + ' ' + this.eventRecipientInfo.apellidoDestinatario, marginLeft: 5, fontSize: '10' },
            { text: 'Fecha de Embarque: \n ' + this.allDatas["eventTransportInfo"][0].fechaEmbarque, marginLeft: 5, fontSize: '10' },
            { text: ' ', marginLeft: 5, fontSize: '10' }
            ]
          ]).widths(['auto', 'auto', 'auto']).margin([-130, 0, 20, 0]).end,
        await new Img('../../assets/img/logofondo3.png').width('100').build()
      ]).end

    );

    pdf.add(
      pdf.ln(1)
    );

    pdf.add(
      new Table([
        [{ text: 'Origen de la Mercancía', bold: true },
        { text: 'Información Legal', bold: true }
        ],
        [{
          text: 'Origen: ' + this.eventSenderInfo.domicilioR + ', ' + this.eventSenderInfo.ciudadR + ', C.P. ' +
            this.eventSenderInfo.cpR, marginLeft: 5, fontSize: '10'
        },
        { text: 'Importador: ' + this.eventLegalInfo.nombreInfo + ' ' + this.eventSenderInfo.apellidoRemitente, marginLeft: 5, fontSize: '10' }
        ],
        [{ text: 'Nombre de compañia: ' + this.eventSenderInfo.nombreCompaniaR, marginLeft: 5, fontSize: '10' },
        { text: 'Dirección: ' + this.eventLegalInfo.direccionInfo, marginLeft: 5, fontSize: '10' }
        ],
        [{ text: ' ', marginLeft: 5, fontSize: '10' },
        { text: 'Contacto: ' + this.eventLegalInfo.contactoInfo, marginLeft: 5, fontSize: '10' }
        ]
      ]).widths(['auto', 'auto']).end
    );

    pdf.add(
      pdf.ln(1)
    );
    //Cabeceras
    pdf.add(
      new Table([
        [{ text: 'Consolidado de Productos', bold: true, alignment: 'left', colSpan: 8 }, '', '', '', '', '', '', ''],
        [{ text: ' ', colSpan: 8 }, '', '', '', '', '', '', ''],
        [{ text: 'Producto', bold: true },
        { text: 'Pallets', bold: true },
        { text: 'Cajas', bold: true },
        { text: 'Unidades', bold: true },
        { text: 'Peso Bruto', bold: true },
        { text: 'Dimesiones', bold: true },
        { text: 'Caducidad Min.', bold: true },
        { text: 'Lote', bold: true }
        ]
      ]).widths([100, 70, 70, 70, 100, 100, 120, 100]).alignment('center').fontSize(11).end
    );
    //Productos FOR
    for (let products of this.allDatas["eventProdDetailInfo"]) {
      pdf.add(
        new Table([
          ['' + products.producto, '' + products.numPallet, '' + products.numCajas, '' + products.cantidad, '' + products.pesoBruto + ' Kgs', '' + products.dimensiones, '' + `${new Date(products.fechaCaducidad).getFullYear()}-${new Date(products.fechaCaducidad).getMonth() + 1}-${new Date(products.fechaCaducidad).getDate()}`, '' + products.lote],
        ]).widths([100, 70, 70, 70, 100, 100, 120, 100]).alignment('center').fontSize(11).end
      );
    }
    //Totales
    pdf.add(
      new Table([
        [' ', '', '', '', '', '', '', ''],
        [{ text: 'Total', bold: true }, '', '', '', '', '', '', ''],
        ['' + this.allDatas["eventTotalProdInfo"][0].totalProducto + ' productos', '' + this.allDatas["eventTotalPalletInfo"][0].totalPallet + ' pallets', '' + this.allDatas["eventTotalBoxInfo"][0].totalCajas + ' cajas',
        '' + this.allDatas["eventTotalQuantityInfo"][0].totalCantidad + ' unidades', '' + this.allDatas["eventTotalWeightInfo"][0].totalPeso + ' Kgs', '---', '' + `${new Date(this.allDatas["eventDateMinInfo"][0].fechaMin).getFullYear()}-${new Date(this.allDatas["eventDateMinInfo"][0].fechaMin).getMonth() + 1}-${new Date(this.allDatas["eventDateMinInfo"][0].fechaMin).getDate()}`, ''],
      ]).widths([100, 70, 70, 70, 100, 100, 120, 100]).alignment('center').fontSize(11).end
    );


    pdf.add(
      pdf.ln(1)
    );



    pdf.add(
      pdf.ln(22)
    );

    pdf.add(
      new Table(
        [
          [{ text: this.eventInfo.nombreAgrupacion, bold: true, rowSpan: 2 },
            '',
          { text: 'Referencia interna: ', marginLeft: 30 }, this.eventInfo.referenciaInterna,
          { text: 'Referencia externa: ', marginLeft: 30 }, this.eventInfo.referenciaExterna,
          ],
          [' ', ' ', ' ', ' ', ' ', ' '],
          [{ text: 'Pallets:', marginLeft: 30 }, '' + this.allDatas["eventTotalPalletInfo"][0].totalPallet,
          { text: 'Cajas:', marginLeft: 60 }, '' + this.allDatas["eventTotalBoxInfo"][0].totalCajas,
          { text: 'Total de Unidades:', marginLeft: 30 }, '' + this.allDatas["eventTotalQuantityInfo"][0].totalCantidad]
        ]).widths(['auto', 'auto', 'auto', 'auto', 'auto', 'auto']).alignment('center').layout('noBorders').margin([150, 0, 150, 0]).end
    );
    pdf.add(
      pdf.ln(1)
    );

    pdf.add(
      new Table([
        [{ text: ' ', bold: true },
        { text: 'Familia', bold: true },
        { text: 'CIU', bold: true },
        { text: 'Lote', bold: true },
        { text: 'Caducidad', bold: true },
        { text: 'Número de Serie', bold: true }
        ]
      ]).widths([30, 100, 100, 100, 100, 100]).alignment('center').fontSize(11).margin([100, 0, 150, 0]).end
    );
    for (let products of this.allDatas["eventProdDetailInfo"]) {
      pdf.add(
        new Table([
          ["    ", '' + products.producto, '' + products.ciu, '' + products.lote, '' + `${new Date(products.fechaCaducidad).getFullYear()}-${new Date(products.fechaCaducidad).getMonth() + 1}-${new Date(products.fechaCaducidad).getDate()}`, '' + products.numSerie],
        ]).widths([30, 100, 100, 100, 100, 100]).alignment('center').fontSize(11).margin([100, 0, 150, 0]).end
      );
      pdf.create().open();
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
