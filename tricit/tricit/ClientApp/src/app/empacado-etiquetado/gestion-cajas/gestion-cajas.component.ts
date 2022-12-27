import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';
import { Router, ActivatedRoute } from '@angular/router';
import { DialogoDetalleComponent } from './dialogo-detalle/dialogo-detalle.component';
import { PdfMakeWrapper, Table, Img, Columns } from 'pdfmake-wrapper';
import { ExporterService } from '../../Interfaces/Services/exporter.service';
import pdfFont from 'pdfmake/build/vfs_fonts';
//format dates
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { AppDateAdapter, APP_DATE_FORMATS } from '../../shared/format-datepicker';
//Cargar Fuentes desde PDf Make Wrapper
PdfMakeWrapper.setFonts(pdfFont);
import {
  faClone, faPrint, faFileAlt, faCheck, faBoxes
} from "@fortawesome/free-solid-svg-icons";
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { DataServices } from '../../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';
import { DialogoUnirComponent } from './dialogo-unir/dialogo-unir.component';
import { DialogoPalletsComponent } from './dialogo-pallets/dialogo-pallets.component';

declare var BrowserPrint: any;

//DataServices and Models
import {
  SearchCompanyRequest,
  SearchCompanyResponse,
  CompaniesData
} from '../../Interfaces/Models/CompanyModels';
//import { timeStamp } from 'console';
@Component({
  selector: 'app-gestion-cajas',
  templateUrl: './gestion-cajas.component.html',
  styleUrls: ['./gestion-cajas.component.less'],
  providers: [
    { provide: DateAdapter, useClass: AppDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS }
  ]
})
export class GestionCajasComponent implements OnInit {

  data_table: any[] = [
  ]

  productsCombo = [];
  typoVista = [
    { id: 1, data: "Agrupación" },
    { id: 2, data: "Pallet" },
    { id: 3, data: "Caja" }
  ]
  direccionesCombo = [];

  displayedColumns: string[] = ['select', 'grouping', 'pallet', 'box', 'product', 'quantity', 'isGroup'];
  dataSource = new MatTableDataSource<any>(this.data_table);
  selection = new SelectionModel<any>(true, []);
  itemsPagina: number[] = enviroments.pageSize;

  tipoCompania: number = 0;
  tipoVista: number = 0;
  empresa: string = "ALEN";
  companiaId: number;

  proviene: number = 0;
  empacadorId: number = 0;

  faClone = faClone;
  faPrint = faPrint;
  faDetalle = faFileAlt;
  faCheck = faCheck;
  faBoxes = faBoxes;

  directionId: number;
  addressId: number;
  productId: number;
  IdProducto: number;
  typeView: number;
  dateStart: string;
  dateEnd: string;
  searchField: string;

  groupingId: number;

  urlObtenerGrupos: string;

  groupingName: string = "";
  operacionid1: number = 0;
  operacionid2: number = 0;

  printerList: any[] = [];
  devices: any[] = [];
  selected_device: any;

  box: number = 0;
  pallet: number = 0;

  aplicado: number;
  //Representa componente de paginación para la busqueda
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //Representa la instancia para el sorting de columnas
  @ViewChild(MatSort) sort: MatSort;

  overlayRef: OverlayRef;

  constructor(
    private _router: Router,
    private _dialog: MatDialog,
    private _overlay: OverlayService,
    private _dataService: DataServices,
    private snack: MatSnackBar,
    private _route: ActivatedRoute,
    private excelService: ExporterService,
  ) { }

  back = () => {
    switch (this.proviene) {
      case 1:
        //internal
        this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoInterno', { state: { registro: 1 } })
        break;
      case 2:
        //external
        this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoExterno', { state: { registro: 1 } })
        break;
      default:
        break;
    }

    // this._location.back();
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return "{this.isAllSelected() ? 'select' : 'deselect'} all";
    }
    return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
  }

  companyIdForIf: number = 0;
  isType: number = 0;
  isCompany: number = 0;
  ngOnInit() {
    this.isCompany = parseInt(sessionStorage.getItem("company"), 10);
    this.isType = parseInt(sessionStorage.getItem("isType"), 10);
    console.log('Is type y compani', this.isType, this.companiaId);
    this.proviene = parseInt(this._route.snapshot.paramMap.get('proviene'), 10);
    this.empacadorId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
    this.empresa = this._route.snapshot.paramMap.get('name');
    if (this.isType == 1 || (this.isCompany == 0 && this.isType == 0)) {
      this.getCompaniasForEmpacador();
    } else {
      this.obtenerProductos();
    }
    switch (this.proviene) {
      case 1:
        //interno
        this.urlObtenerGrupos = "InternalPacked/SearchBoxManagement";
        break;
      case 2:
        //externo
        this.urlObtenerGrupos = "ExternalPacked/SearchBoxManagement";
        break;
      default:
        break;
    }

    this.addressId = 0;
    this.productId = 0;
    this.typeView = 0;
    this.dateStart = "";
    this.dateEnd = "";
    this.searchField = "";
    this.groupingId = 0;
    this.companiaId = 0;

    this.setup(this.printerList, this.devices);
  }

  //Traemos el lisado compañia por empacador
  response = new SearchCompanyResponse();
  dataSourceCompany: Array<CompaniesData>;
  emptyList: boolean = false;
  /*
   * Funcion para obtener listado de compañias por empacador (según a las que se encueentre ligado)
   *  Autor: Hernán Gómez 
   * 10-Marzo-2022
   */
  async getCompaniasForEmpacador() {
    var request = new SearchCompanyRequest();
    console.error('Empacaoddr', parseInt(this._route.snapshot.paramMap.get('id'), 10), this.empacadorId);
    request.packedId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
    let overlayRef: any;
    setTimeout(() => {
      overlayRef = this._overlay.open();
    }, 1);

    await this._dataService.postData<SearchCompanyResponse>
      ("Companies/searchCompanyEmpacador", sessionStorage.getItem("token"), request).subscribe(
        data => {
          console.log('DATA', data);
          this.response = data;
          if (this.response.companiesDataList.length > 0)
            this.emptyList = false;
          else
            this.emptyList = true;
          this.dataSourceCompany = this.response.companiesDataList.filter(x => x.name != "");
          setTimeout(() => {
            this._overlay.close(overlayRef);
          }, 1);
        },
        error => {
          console.log(error);
          setTimeout(() => {
            this._overlay.close(overlayRef);
          }, 1);
        }
      );
  }
  //Obtener las impresoras zebra agregadas
  setup(list, listdevices) {
    //Get the default device from the application as a first step. Discovery takes longer to complete.
    BrowserPrint.getDefaultDevice("printer", function (device) {
      //Discover any other devices available to the application
      listdevices.push(device)
      BrowserPrint.getLocalDevices(function (device_list) {
        for (var i = 0; i < device_list.length; i++) {
          listdevices.push(device_list[i]);
          list.push({ name: device_list[i].name, uid: device_list[i].uid })
        }
      }, function () { console.log("Error getting local devices") }, "printer");

    }, function (error) {
      console.log(error);
    })
  }

  onDeviceSelected(selected) {
    console.log("seleccionada: " + selected);
    for (var i = 0; i < this.devices.length; ++i) {
      console.log("printer: " + this.devices[i].uid);
      if (selected == this.devices[i].uid) {
        this.selected_device = this.devices[i];
        return;
      }
    }
  }

  printLabel(copies, operationType, operationScanned, first, last, group, pallet, box, superiorText, superiorText2, leftText, leftText2, rightText, rightText2, bottomText, bottomText2, etiquetaID, instrucciones, medianilx, medianily, fecha, linea) {
    //T: C - caja, P - Pallet
    //P: Cantidad de unidades escaneadas
    //I: veryFirst
    //F: veryLast
    //Id: agrupacion - pallet - box


    /*

      Primario
       { id: 1, value: "Producto" }
       { id: 2, value: "Rangos" }
       { id: 3, value: "Datos de empaque" }
       { id: 4, value: "compañia" }
      Secundario
       { id: 1, value: "Producto" }
       { id: 2, value: "Rangos" }
       { id: 3, value: "Datos de empaque" }
       { id: 4, value: "compañia" }
      en db tengo por cada lado
      _topPrimary,
      _topSecondary,
      _rightPrimary,
      _rightSecondary,
      _bottomPrimary,
      _bottomSecondary,
      _leftPrimary,
      _leftSecondary,
     */


    let dates: string = fecha.replace(/-/gi, '/');

    console.log("inciar construcción de etiqueta");
    let labelZPL = "^XA" +
      "^PW4000" +
      `^LH${medianilx},${medianily}` +
      "^A1N,50,40" +
      "^FO731,50,1" +
      "^FD" + (operationType == "P" ? "PALLET" : "CARTON") +
      "^FS" +
      "^A1N,20,12" +
      "^FO100,72,0" +
      "^FD" + etiquetaID +
      "^FS" +
      "^A1N,25,15" +
      "^FO100,120,0" +
      "^FD" + superiorText + //Compañia
      "^FS" +
      "^A1N,15,10" +
      "^FO100,150,0" +
      "^FD" + superiorText2 + //Nombre del producto de la familia
      "^FS" +
      "^A1N,15,10" +
      "^FO195,858,0" +
      "^FD" + superiorText2 + //Nombre del producto de la familia
      "^FS" +
      "^A1N,15,10" +
      "^FO195,1030,0" +
      "^FD" + superiorText2 + //Nombre del producto de la familia
      "^FS" +
      "^FO120,200" +
      //Data matriz del lado superior izquiedo
      "^BXN,6,200^FD" + window.location.origin + "/tracking?qr={\"T\":\"" + operationType + "\",\"P\":" + operationScanned + ",\"I\":\"" + first + "\",\"F\":\"" + last + "\",\"ID\":\"" + group + "-" + pallet + "-" + box + "\"}" + //DM
      "^FS" +
      //QR del lado superior derecho
      "^FO485,200" +
      "^BQN,2,4,^FDQA," + window.location.origin + "/tracking?qr={\"T\":\"" + operationType + "\",\"P\":" + operationScanned + ",\"I\":\"" + first + "\",\"F\":\"" + last + "\",\"ID\":\"" + group + "-" + pallet + "-" + box + "\"}" + //QR
      "^FS" +
      //Datamatriz del primer hijo (Izquierda)
      "^FO20,853" +
      "^BXN,3,200^FD" + window.location.origin + "/tracking?qr={\"T\":\"" + operationType + "\",\"P\":" + operationScanned + ",\"I\":\"" + first + "\",\"F\":\"" + last + "\",\"ID\":\"" + group + "-" + pallet + "-" + box + "\"}" + //DM
      "^FS" +
      //QR del primer hijo (Derecha)
      "^FO620,853" +
      "^BQN,2,2,^FDQA," + window.location.origin + "/tracking?qr={\"T\":\"" + operationType + "\",\"P\":" + operationScanned + ",\"I\":\"" + first + "\",\"F\":\"" + last + "\",\"ID\":\"" + group + "-" + pallet + "-" + box + "\"}" + //QR
      "^FS" +
      //Datamatriz del segundo hijo (Izquierda)
      "^FO20,1053" +
      "^BXN,3,200^FD" + window.location.origin + "/tracking?qr={\"T\":\"" + operationType + "\",\"P\":" + operationScanned + ",\"I\":\"" + first + "\",\"F\":\"" + last + "\",\"ID\":\"" + group + "-" + pallet + "-" + box + "\"}" + //DM
      "^FS" +
      //QR del segundo hijo (derecha)
      "^FO620,1053" +
      "^BQN,2,2,^FDQA," + window.location.origin + "/tracking?qr={\"T\":\"" + operationType + "\",\"P\":" + operationScanned + ",\"I\":\"" + first + "\",\"F\":\"" + last + "\",\"ID\":\"" + group + "-" + pallet + "-" + box + "\"}" + //QR
      "^FS" +
      "^A2R,20,12" +
      "^FO40,150,1" +
      //Fecha lado izquiero texto vertical (datamatrix superior)
      "^FD" + leftText + ", " + linea + ", " + new Date(dates).getDate() + "/" + ((new Date(dates).getMonth() + 1) < 10 ? `0${(new Date(dates).getMonth() + 1)}` : `${(new Date(dates).getMonth() + 1)}`) + "/" + new Date(dates).getFullYear() + ' ' + (new Date(dates).getHours() < 10 ? `0${new Date(dates).getHours()}` : `${new Date(dates).getHours()}`) + ":" + (new Date(dates).getMinutes() < 10 ? `0${new Date(dates).getMinutes()}` : `${new Date(dates).getMinutes()}`) +
      "^FS" +
      "^A1N,23,13" +
      "^FO750,500,1" +
      //Titulo count
      "^FDCount" +
      "^FS" +
      "^A1N,23,13" +
      "^FO100,500,0" +
      "^FDCode Ranges";


    if (operationType == "P") {
      let aRngsX = leftText2.split(';');
      first = aRngsX[0].split('-')[0];
    }
    leftText2 = first + " - " + last;
    let aRngs = leftText2.split(';');
    for (var x = 0; x < aRngs.length; x++) {
      if (aRngs[x] != "") {
        labelZPL += "^FS" +
          "^A1N,20,10" +
          "^FO100," + (550 + (x * 20)) + ",0" +
          "^FD" + aRngs[x] + "                      " + operationScanned +
          "^FS" +
          "^A1N,20,10" +
          "^FO220," + (878 + (x * 20)) + ",0" +
          "^FD" + aRngs[x] +
          "^FS" +
          "^A1N,20,10" +
          "^FO220," + (1078 + (x * 20)) + ",0" +
          "^FD" + aRngs[x];
      }
    }

    labelZPL += "^XZ";

    console.log(labelZPL);
    console.log("There will be " + copies + " copies.");
    for (var i = 0; i < copies; i++)
      this.writeToSelectedPrinter(labelZPL)
  }

  writeToSelectedPrinter(dataToWrite) {
    this.selected_device.send(dataToWrite, undefined, this.errorCallback);
  }

  errorCallback = function (errorMessage) {
    console.log("Error: " + errorMessage);
  }

  obtenerDirecciones() {
    let datos: any = {
      id: 0
    };

    switch (this.proviene) {
      case 1:
        //Interno
        datos = {
          id: 1,
          id2: 0
        }
        break;
      case 2:
        //externo
        datos = {
          id: 2,
          id2: 0
        }
        break;
      default:
        break;
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    //debugger
    this._dataService.postData<any>("PackedLabeled/SearchAddressCombo", sessionStorage.getItem("token"), datos).subscribe(
      data => {
        //  
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.direccionesCombo = data["addressLst"];

          this.obtenerProductos();
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

  obtenerProductos() {
    let datos: any = {
      id: this.isCompany ? this.isCompany : this.companiaId,
    };

    setTimeout(() => {
      if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);
    //debugger
    this._dataService.postData<any>("PackedLabeled/SearchProductCombo", sessionStorage.getItem("token"), datos).subscribe(
      data => {
        //  
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.productsCombo = data["productscombo"];
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

  obtenerAgrupaciones() {
    if (this.typeView == 0) {
      this.openSnack("Seleccione un tipo de vista", "Aceptar");
      return;
    }
    let _dateStart = new Date();
    _dateStart.setMonth(_dateStart.getMonth() - 3);
    let datos: any = {
      packedId: this.empacadorId,
      addressId: this.addressId,
      productId: this.productId,
      typeView: this.typeView,
      companiaId: this.isType == 0 && this.isCompany > 0 ? this.isCompany : this.companiaId,
      dateStart: !!this.dateStart ? new Date(this.dateStart).toISOString() : _dateStart.toISOString(),
      dateEnd: !!this.dateEnd ? new Date(this.dateEnd).toISOString() : new Date()
    };
    if (this.dateEnd != "" && this.dateStart == "") {
      this.openSnack("Seleccione una fecha inicial", "Aceptar");
      return;
    }
    setTimeout(() => {
      if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);
    //debugger
    this._dataService.postData<any>(this.urlObtenerGrupos, sessionStorage.getItem("token"), datos).subscribe(
      data => {
        //  
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        this.selection.clear();
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {

          this.dataSource.data = data["infoData"];

        }

        this.aplicado = this.typeView;
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

  dialogoDetalle() {

    let direccion = "";

    if (this.addressId != 0) {
      direccion = this.direccionesCombo.find(x => x.id == this.addressId).data;
    }
    let auxdata = { viewType: this.typeView, groupingId: this.selection.selected[0]["groupingId"], proviene: this.proviene, productoId: this.IdProducto, direccion: direccion, box: this.box, pallet: this.pallet };
    let _dialogRef = this._dialog.open(DialogoDetalleComponent, {
      panelClass: "dialog-min",
      data: { viewType: this.typeView, groupingId: this.selection.selected[0]["groupingId"], proviene: this.proviene, productoId: this.IdProducto, direccion: direccion, box: this.box, pallet: this.pallet },

    })
    console.log('DAta para modal', auxdata);
  }

  Union() {

    let _dialogoRef = this._dialog.open(DialogoUnirComponent, {
      panelClass: "dialog-min",
      data: {
        agrupacion1: this.selection.selected[0]["groupingId"],
        agrupacion2: this.selection.selected[1]["groupingId"],
        agrupaciones: this.selection.selected,
        tipoVista: this.typeView,
        Tipo: 1,
        Devices: this.printerList
      }
    });

    _dialogoRef.afterClosed().subscribe(res => {
      if (res.result == true) {
        this.obtenerAgrupaciones();
      }
    })

  }

  reimprimirEtiqueta() {

    if (this.selection.selected.length < 1) {
      return;
    }
    //Codigo nuevo inicio
    let _dialogoRef = this._dialog.open(DialogoUnirComponent, {
      panelClass: "dialog-min",
      data: { tipoVista: this.typeView, Tipo: 2, Devices: this.printerList }
    });

    _dialogoRef.afterClosed().subscribe(res => {
      if (res.result == true) {
        if (res.printerId == 0) {
          this.openSnack("No se realizará la impresión de la etiqueta, seleccionar una impresora", "Aceptar");
          // return;
        } else {
          this.onDeviceSelected(res.printerId)
        }
        this.BusquedaEtiqueta(res.id, true, res.x, res.y);
        //console.log("impresora: ", res.printerId)
      }
    })
    //codigo nuevo FIN
  }

  //Funcion para realizar la busqueda  (Las variables son las que se envian para hacer la búsqueda)
  BusquedaEtiqueta(id = 0, getGroups = false, x = 20, y = 20, tview = 0, nbox = 0, npal = 0) {
    let request: any = {};
    request.groupingId = id != 0 ? id : this.selection.selected[0]["groupingId"];
    request.groupingType = tview ? tview : this.typeView;
    request.box = nbox ? nbox : this.box;
    request.pallet = npal ? npal : this.pallet;

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, .001);

    this._dataService.postData<any>("ExternalPacked/PrintOperationLabel", sessionStorage.getItem("token"), request).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log(data, 'data');

        //copies, operationType, operationScanned, first, last, group, pallet, box, 
        //superiorText, superiorText2, leftText, leftText2, rightText, rightText2, bottomText, bottomText2
        let typeV = tview ? tview : this.typeView;

        if (typeV == 1) {
          this.openSnack("No se puede imprimir una etiqueta con tipo de grupación, seleccione Por Caja o Por Pallet", "ACEPTAR");
        }
        else {
          let inicial, final = "";
          let buxes: number = this.box;

          //pallet
          if (typeV == 2) {
            let aux = data["ranges"].split(";");
            inicial = aux[0].split("-")[0];
            final = aux[aux.length - 1].split("-")[1];
            buxes = aux.length;
          }
          // caja
          if (typeV == 3) {
            inicial = data["ranges"].split('-')[0];
            final = data["ranges"].split('-')[1];
          }

          this.printLabel(1, typeV == 2 ? "P" : "C", data["operationScanned"], inicial, final, data['groupingName'], `PL-${(npal ? npal : this.pallet)}`, `Bx-${buxes}`, data["company"], data["productName"], data["packagin"],
            data["ranges"], "", "", "", "", data["etiquetaId"], data["instructions"], x, y, data["date"], data["line"]);
        }

        if (getGroups)
          this.obtenerAgrupaciones();
      },
      error => {
        this._overlay.close(this.overlayRef);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("1");
        } else {
          console.log(error);
        }
      }
    );
  }

  makePallet() {

    let _dialogoRef = this._dialog.open(DialogoPalletsComponent, {
      width: 'auto',
      height: 'auto',
      disableClose: true,
      data: {
        tipoVista: this.typeView,
        Tipo: 1,
        Devices: this.printerList
      }
    });

    _dialogoRef.afterClosed().subscribe(res => {
      console.log("res pallet: ", res);
      if (typeof (res) == "object") {
        if (res.success) {
          let operacionId = res.messageEsp.split(',')[0];
          let nPallets = res.messageEsp.split(',')[1];

          //Codigo nuevo inicio
          let _dialogoRef = this._dialog.open(DialogoUnirComponent, {
            panelClass: "dialog-min",
            data: { tipoVista: 2, Tipo: 2, Devices: this.printerList }
          });

          _dialogoRef.afterClosed().subscribe(res => {
            if (res.result == true) {
              if (res.printerId == 0) {
                this.openSnack("No se realizará la impresión de la etiqueta, seleccionar una impresora", "Aceptar");
                // return;
              } else {
                this.onDeviceSelected(res.printerId)
              }
              console.log("operacionId: ", operacionId);
              for (let i = 1; i <= nPallets; i++) {
                console.log("i: ", i);
                this.BusquedaEtiqueta(operacionId, true, res.x, res.y, 2, 0, i);
              }
            }
          });

        }
      }
    })

  }

  pdfPrintGrouping(data) {
    const pdf = new PdfMakeWrapper();

    pdf.info({
      title: 'PDF Etiqueta Nombre del Producto',
      author: 'Trace IT',
      subject: ''
    });

    pdf.pageOrientation('landscape');
    pdf.pageMargins([20, 20, 20, 20]);

    pdf.add(
      new Table([
        ['', '****', 'TOP', '****', '',
          { text: ' ', rowSpan: 8, noBorders: false },
          { text: ' ', rowSpan: 8 },
          { text: ' ', rowSpan: 8 },
          { text: 'T\nr\na\nc\ne\n-\nI\nt\n', fontSize: '15', bold: true, rowSpan: 8, marginTop: 70 },
          { text: ' ', rowSpan: 8 },
          { text: ' ', rowSpan: 8 },
          { text: ' ', rowSpan: 8 },
          { text: 'E\nN\nC\nR\nY\nP\nT\nE\nD\n \nD\nE\nC\nR\nY\nP\nT\nE\nD', fontSize: '10', rowSpan: 5 },
          { text: 'E\nN\nC\nR\nY\nP\nT\nE\nD\n \nD\nE\nC\nR\nY\nP\nT\nE\nD', fontSize: '10', rowSpan: 5 },
          { text: ' ', rowSpan: 8 },
          { text: 'E\nN\nC\nR\nY\nP\nT\nE\nD\n \nD\nE\nC\nR\nY\nP\nT\nE\nD', fontSize: '10', rowSpan: 5 },
          { text: 'E\nN\nC\nR\nY\nP\nT\nE\nD\n \nD\nE\nC\nR\nY\nP\nT\nE\nD', fontSize: '10', rowSpan: 5 }
        ],
        [{ text: 'N\no\nm\nb\nr\ne\n \nd\ne\n \np\nr\no\nd\nu\nc\nt\no', fontSize: '10', rowSpan: 5, wrap: 'vertical' },
        { text: data["groupingName"], fontSize: '20', bold: true, colSpan: 3 }, '', '',
        { text: 'D\nE\nC\nR\nY\nP\nT\nE\nD', fontSize: '10', rowSpan: 2 }, '', '', '', '', '', '', '', '', '', '', '', ''],
        ['',
          { qr: data["qrCode"], colSpan: 3, rowSpan: 3, marginTop: 35 }, '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
        ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
        ['', '', '', '', { text: 'E\nN\nC\nR\nY\nP\nT\nE\nD', fontSize: '10', rowSpan: 2 }, '', '', '', '', '', '', '', '', '', '', '', ''],
        ['',
          { text: '1 de ' + data["range"], colSpan: 3 }, '', '', '', '', '', '', '', '', '', '',
          { qr: data["qrCode"], rowSpan: 2, colSpan: 2, fit: 50 }, '', '',
          { qr: data["qrCode"], rowSpan: 2, colSpan: 2, fit: 50 }, ''],
        ['',
          { text: data["qrCode"], colSpan: 3 }, '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
        [{ text: ' ', colSpan: 5 }, '', '', '', '', '', '', '', '', '', '', '',
        { text: '1 de ' + data["range"], colSpan: 2 }, '', '',
        { text: '1 de ' + data["range"], colSpan: 2 }, '']
      ]).widths(['auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'])
        .alignment('center')
        .layout('noBorders').end
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



  //Evento para realizar la seleccion de la tabla
  seleccion = () => {
    this.selection.selected.map(item => {
      this.groupingId = item.groupingId;
      this.IdProducto = item.productId;
      if (this.typeView > 1) {
        if (this.typeView == 2) {
          this.pallet = parseInt(item.pallet);
          this.box = 0;
        }

        if (this.typeView == 3) {
          this.pallet = parseInt(item.pallet.split('-')[1]);
          this.box = parseInt(item.box.split('-')[1]);
        }

      } else {
        this.box = 0;
        this.pallet = 0;
      }


    });
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
            this.obtenerDirecciones();
            break;
          case "2":
            this.obtenerProductos();
            break;
          case "3":
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

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  aplicarFiltro() {
    this.dataSource.filter = this.searchField;
  }

  generarExcel(): void {
    var datosExportar = this.dataSource.data.map(item => {
      console.log(item);
      return {
        'Agrupación': item.groupingName,
        'Pallet(s)': item.pallet,
        'Caja(s)': item.box,
        'Producto': item.productName,
        'Cantidad': item.quantity,
      };
    });
    this.excelService.exportToExcel(datosExportar, 'Exportacion de movimientos');
  }
}
