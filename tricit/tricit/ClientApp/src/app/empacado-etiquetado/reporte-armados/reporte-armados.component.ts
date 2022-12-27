import { Component, OnInit, ViewChild, QueryList, ViewChildren } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Label, Color, BaseChartDirective } from 'ng2-charts';
import * as pluginAnnotations from 'chartjs-plugin-annotation';

import { faFileExport } from "@fortawesome/free-solid-svg-icons";
import { ActivatedRoute, Router } from '@angular/router';
import { OverlayRef } from '@angular/cdk/overlay';
import { MatDialog, MatSnackBar, DateAdapter } from '@angular/material';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { DataServices } from '../../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

import * as XLSX from 'xlsx';


//DataServices and Models
import {
  SearchCompanyRequest,
  SearchCompanyResponse,
  CompaniesData
} from '../../Interfaces/Models/CompanyModels';
@Component({
  selector: 'app-reporte-armados',
  templateUrl: './reporte-armados.component.html',
  styleUrls: ['./reporte-armados.component.less']
})
export class ReporteArmadosComponent implements OnInit {

  faFileExport = faFileExport;
  /**
   * Libreria de la grafica https://valor-software.com/ng2-charts/#/LineChart
   */
  public lineChartDataCajas: ChartDataSets[] = [
    { data: [0], label: 'Cajas', fill: false }
  ];

  public lineChartDataMermas: ChartDataSets[] = [
    { data: [0], label: 'Mermas', fill: false }
  ];
  public lineChartLabelsCajas: Label[] = [""];
  public lineChartLabelsMermas: Label[] = [""];
  public lineChartOptions: (ChartOptions & { annotation: any }) = {
    responsive: true,
    scales: {
      // We use this empty structure as a placeholder for dynamic theming.
      xAxes: [{}],
      yAxes: [
        {
          id: 'y-axis-0',
          position: 'left',
          ticks: {
            min: 0,
            callback: function (label: number, index, labels) {
              if (Math.floor(label) === label) {
                return label;
              }
            },
          }
        },
        /*{
          id: 'y-axis-1',
          position: 'right',
          gridLines: {
            color: 'rgba(255,0,0,0.3)',
          },
          ticks: {
            fontColor: 'red',
          }
        }*/
      ]
    },
    annotation: {

    }
  };
  public lineChartColors: Color[] = [
    { // greybackgroundColor: 'rgba(148,159,177,0)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    },
    { // dark grey
      backgroundColor: 'rgba(77,83,96,0)',
      borderColor: 'rgba(77,83,96,1)',
      pointBackgroundColor: 'rgba(77,83,96,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(77,83,96,1)'
    },
    { // red
      backgroundColor: 'rgba(255,0,0,0)',
      borderColor: 'red',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }
  ];
  public lineChartLegend = true;
  public lineChartType = 'line';
  public lineChartPlugins = [pluginAnnotations];

  @ViewChildren(BaseChartDirective) chart: QueryList<BaseChartDirective>;

  proviene: number = 0;
  empacadorId: number = 0;
  empresa: string;
  companiaId: number;
  productoId: number;
  direccionId: number;
  state: any = {};
  overlayRef: OverlayRef;
  busquedaGenerica: string;
  tipoCompania: number;

  tipoCompanias: any[] = [
    //{ id: 1, name: "Proveedor" },
  ]

  companias: any[] = [];
  direcciones: any[] = [];
  productos: any[] = [];

  dateStart: Date = new Date();
  dateEnd: Date = new Date();
  maxDate: Date = new Date();
  minDate: Date = new Date();

  resultadosCajas: any = [];
  resultadosMermas: any = [];
  resultadosMermasOperaciones: any = [];
  company : number = 0;
  isType: number = 0;

  constructor(
    private _router: Router,
    private _dialog: MatDialog,
    private _overlay: OverlayService,
    private _dataService: DataServices,
    private snack: MatSnackBar,
    private _route: ActivatedRoute,
    private dateAdapter: DateAdapter<Date>
  ) {
    this.dateAdapter.setLocale('en-GB');
  }

  ngOnInit() {
    this.getCompaniasForEmpacador();
    if (sessionStorage.getItem("isType") == "1") {
      this.tipoCompanias.push({ id: 2, name: "Empacador externo" });
    }
    if (sessionStorage.getItem("isType") == "2") {
      this.tipoCompanias.push({ id: 3, name: "Empacador interno" });
    }
    if (sessionStorage.getItem("isType") == "0") {
      this.tipoCompanias.push({ id: 2, name: "Empacador externo" });
      this.tipoCompanias.push({ id: 3, name: "Empacador interno" });
    }
    this.proviene = parseInt(this._route.snapshot.paramMap.get('proviene'), 10);
    this.empacadorId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
    this.empresa = this._route.snapshot.paramMap.get('name');

    this.busquedaGenerica = "";
    this.productoId = 0;
    this.direccionId = 0;
    this.companiaId = 0;
    this.tipoCompania = this.proviene == 2 ? 2 : 3;

    // debugger;
    let today: Date = new Date();
    this.dateStart = today;
    this.dateEnd.setDate(today.getDate() + 1);
    this.minDate.setDate(today.getDate() + 1);
    this.company = parseInt(sessionStorage.getItem("company"));
    this.isType = parseInt (sessionStorage.getItem("isType"));
    console.log("Company", this.company);

  }

  
  //Traemos el lisado compañia por empacador
  responseCompany = new SearchCompanyResponse();
  dataSourceCompany : Array<CompaniesData>; 
  emptyList: boolean = false;
  /*
   * Funcion para obtener listado de compañias por empacador (según a las que se encueentre ligado)
   *  Autor: Hernán Gómez 
   * 10-Marzo-2022
   */
  async getCompaniasForEmpacador() {
    var request = new SearchCompanyRequest();
    request.packedId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
    let overlayRef: any;
    setTimeout(() => {
      overlayRef = this._overlay.open();
    }, 1);

    await this._dataService.postData<SearchCompanyResponse>
    ("Companies/searchCompanyEmpacador", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseCompany = data;
        if(this.responseCompany.companiesDataList.length > 0)
          this.emptyList = false;
        else
          this.emptyList = true;
          this.companias = this.responseCompany.companiesDataList.filter(x => x.name != "");
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

  setearFechas(fecha: Date) {
    // debugger;
    this.minDate.setDate(fecha.getDate() + 1);
    this.minDate.setMonth(fecha.getMonth());
  }

  formatearFecha(fecha: string): string {
    let date = new Date(fecha);

    return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;
  }

  obtenerCompanias(action = 1) {
    let datos: any = {
      id: 0
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    //debugger
    this._dataService.postData<any>("PackedLabeled/SearchCompanyCombo", sessionStorage.getItem("token"), datos).subscribe(
      data => {
        //  
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.companias = data["companiescombo"];

          if (action == 0) {
            this.companiaId = parseInt(sessionStorage.getItem("company"));

            if (this.companiaId != 0) {
              // hacer el filtrado de solamente la compañia a la que pertenece
              let temp: any[] = [];

              temp = this.companias.filter(x => x.id == this.companiaId);

              this.companias = temp;
            }

            this.obtenerDirecciones();
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
  }

  obtenerDirecciones(action = 1) {
    let datos: any = {
      id: action == 2 && this.tipoCompania != 0 ? (this.tipoCompania == 2 ? -2 : -3) : (this.proviene == 2 ? -2 : -3),
      id2: this.companiaId
    };

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
          this.direcciones = data["addressLst"];

          this.obtenerProductos();
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

  obtenerProductos() {
    let datos: any = {
      id: this.companiaId,
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
          this.productos = data["productscombo"];
        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("3");
        } else {
          console.log(error);
        }
      }
    )
  }

  aplicarFiltros() {

    let datos: any = {
      packagingId: this.empacadorId,
      typeCompany: this.tipoCompania,
      companyId: this.companiaId,
      addressId: this.direccionId,
      productId: this.productoId,
      dateStart: "",
      dateEnd: ""
    };

    // if (this.tipoCompania == 0) {
    //   this.openSnack("Seleccione un tipo de compañia", "Aceptar");
    //   return;
    // }

    // if (this.companiaId == 0) {
    //   this.openSnack("Seleccione una compañia", "Aceptar");
    //   return;
    // }

    // if (this.direccionId == 0) {
    //   this.openSnack("Seleccione una dirección", "Aceptar");
    //   return;
    // }

    // if (this.productoId == 0) {
    //   this.openSnack("Seleccione un producto", "Aceptar");
    //   return;
    // }

    if (this.dateEnd.toString() != "" && this.dateStart.toString() == "") {
      this.openSnack("Seleccione una fecha inicial", "Aceptar");
      return;
    }

    if (this.dateStart.toString() != "") {

      if (this.dateEnd.toString() == "") {
        this.openSnack("Seleccione una fecha final", "Aceptar");
        return;
      }

      datos["dateStart"] = `${new Date(this.dateStart).getFullYear()}-${new Date(this.dateStart).getMonth() + 1}-${new Date(this.dateStart).getDate()}`;
      datos["dateEnd"] = `${new Date(this.dateEnd).getFullYear()}-${new Date(this.dateEnd).getMonth() + 1}-${new Date(this.dateEnd).getDate()}`;
    }


    setTimeout(() => {
      if (this.overlayRef === undefined || !this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);

    // debugger
    this._dataService.postData<any>("ExternalPacked/SearchArmingReport", sessionStorage.getItem("token"), datos).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.generarDatos(data);
        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("3");
        } else {
          console.log(error);
        }
      }
    )
  }

  generarDatos(datos) {

    let mesCajas = [];
    let mesMermas = [];
    let mesMermasOperacion = [];

    //cajas
    datos["infoDataBoxes"].forEach(boxDay => {
      let fecha = new Date("" + boxDay["dateArm"].split('-')[0] + "/" + (parseInt(boxDay["dateArm"].split('-')[1])) + "/" + (parseInt(boxDay["dateArm"].split('-')[2])));
      let formato = "";
      let semanaini = 1;
      let semanaend = 7;

      let initd = new Date(this.dateStart);
      let initf = new Date(this.dateEnd);
      let difFechas = Math.round((initf.getTime() - initd.getTime()) / (1000 * 60 * 60 * 24));
      let esdia = (fecha.getDate() + "").length == 1 ? "0" + fecha.getDate() : fecha.getDate();
      let esmes = ((fecha.getMonth() + 1) + "").length == 1 ? "0" + (fecha.getMonth() + 1) : fecha.getMonth() + 1;

      if (initd.getMonth() != initf.getMonth()) {
        formato = `${esmes}/${fecha.getFullYear()}`; // filtrado por mes
      } else if (difFechas <= 6) {
        formato = `${esdia}/${esmes}/${fecha.getFullYear()}`; // filtrado por dia
      } else {
        if(fecha.getDate() >= 8 && fecha.getDate() <= 14) {
          semanaini = 8; semanaend = 14;
        } else if(fecha.getDate() >= 15 && fecha.getDate() <= 21) {
          semanaini = 15; semanaend = 21;
        } else if(fecha.getDate() >= 22 && fecha.getDate() <= 28) {
          semanaini = 22; semanaend = 28;
        } else if(fecha.getDate() >= 29) {
          semanaini = 29; semanaend = 35;
        }
        formato = `(${semanaini}-${semanaend})/${esmes}/${fecha.getFullYear()}`; // filtrado por semana
      }

      let indExist = mesCajas.findIndex(reg => reg.mes == formato);

      if (indExist == -1) {
        let sumcaja = {
          mes: formato,
          total: boxDay["total"]
        }

        mesCajas.push(sumcaja);
      } else {
        //Ya existe el mes, ahora sumar
        mesCajas[indExist]["total"] += boxDay["total"];
      }
    });

    //mermas
    datos["infoDataWaste"].forEach(boxDay => {
      let fecha = new Date("" + boxDay["dateArm"].split('-')[0] + "/" + (parseInt(boxDay["dateArm"].split('-')[1])) + "/" + (parseInt(boxDay["dateArm"].split('-')[2])));
      let formato = "";
      let semanaini = 1;
      let semanaend = 7;

      let initd = new Date(this.dateStart);
      let initf = new Date(this.dateEnd);
      let difFechas = Math.round((initf.getTime() - initd.getTime()) / (1000 * 60 * 60 * 24));
      let esdia = (fecha.getDate() + "").length == 1 ? "0" + fecha.getDate() : fecha.getDate();
      let esmes = ((fecha.getMonth() + 1) + "").length == 1 ? "0" + (fecha.getMonth() + 1) : fecha.getMonth() + 1;

      if (initd.getMonth() != initf.getMonth()) {
        formato = `${esmes}/${fecha.getFullYear()}`; // filtrado por mes
      } else if (difFechas <= 6) {
        formato = `${esdia}/${esmes}/${fecha.getFullYear()}`; // filtrado por dia
      } else {
        if (fecha.getDate() > 7 && fecha.getDate() < 14) {
          semanaini = 8; semanaend = 14;
        } else if (fecha.getDate() > 14 && fecha.getDate() < 21) {
          semanaini = 15; semanaend = 21;
        } else if (fecha.getDate() > 21 && fecha.getDate() < 28) {
          semanaini = 22; semanaend = 28;
        } else if (fecha.getDate() > 28) {
          semanaini = 29; semanaend = 35;
        }
        formato = `(${semanaini}-${semanaend})/${esmes}/${fecha.getFullYear()}`; // filtrado por semana
      }

      let indExist = mesMermas.findIndex(reg => reg.mes == formato);

      if (indExist == -1) {
        let sumcaja = {
          mes: formato,
          total: boxDay["total"]
        }

        mesMermas.push(sumcaja);
      } else {
        //Ya existe el mes, ahora sumar
        mesMermas[indExist]["total"] += boxDay["total"];
      }

      let index: number = datos["infoDataWasteOperation"].findIndex(x => x.mes == boxDay.dateArm);

      if (index == -1) {
        datos["infoDataWasteOperation"].push({
          dateArm: "2021-09-15",
          lineName: "",
          operatorName: "",
          total: 0
        })
      }

    });

    //mermasoperaciones
    datos["infoDataWasteOperation"].forEach(boxDay => {
      let fecha = new Date("" + boxDay["dateArm"].split('-')[0] + "/" + (parseInt(boxDay["dateArm"].split('-')[1])) + "/" + (parseInt(boxDay["dateArm"].split('-')[2])));
      let formato = "";
      let semanaini = 1;
      let semanaend = 7;

      let initd = new Date(this.dateStart);
      let initf = new Date(this.dateEnd);
      let difFechas = Math.round((initf.getTime() - initd.getTime()) / (1000 * 60 * 60 * 24));
      let esdia = (fecha.getDate() + "").length == 1 ? "0" + fecha.getDate() : fecha.getDate();
      let esmes = ((fecha.getMonth() + 1) + "").length == 1 ? "0" + (fecha.getMonth() + 1) : fecha.getMonth() + 1;

      if (initd.getMonth() != initf.getMonth()) {
        formato = `${esmes}/${fecha.getFullYear()}`; // filtrado por mes
      } else if (difFechas <= 6) {
        formato = `${esdia}/${esmes}/${fecha.getFullYear()}`; // filtrado por dia
      } else {
        if (fecha.getDate() > 7 && fecha.getDate() < 14) {
          semanaini = 8; semanaend = 14;
        } else if (fecha.getDate() > 14 && fecha.getDate() < 21) {
          semanaini = 15; semanaend = 21;
        } else if (fecha.getDate() > 21 && fecha.getDate() < 28) {
          semanaini = 22; semanaend = 28;
        } else if (fecha.getDate() > 28) {
          semanaini = 29; semanaend = 35;
        }
        formato = `(${semanaini}-${semanaend})/${esmes}/${fecha.getFullYear()}`; // filtrado por semana
      }

      let indExist = mesMermasOperacion.findIndex(reg => reg.mes == formato);

      if (indExist == -1) {
        let sumcaja = {
          mes: formato,
          total: boxDay["total"]
        }

        mesMermasOperacion.push(sumcaja);
      } else {
        //Ya existe el mes, ahora sumar
        mesMermasOperacion[indExist]["total"] += boxDay["total"];
      }
    });

    //construcción de lineas
    let mesTotalCajas = [];
    let mesEtiquetasCajas = [];
    mesCajas.forEach(mes => {
      mesTotalCajas.push(mes.total);
      mesEtiquetasCajas.push(mes.mes);
    });

    let mesTotalMermas = [];
    let mesEtiquetasMermas = [];
    // mesMermas.forEach(mes => {
    //   mesTotalMermas.push(mes.total);
    //   mesEtiquetasMermas.push(mes.mes);
    // });

    let totalMermasOperaciones = [];
    let mesEtiquetasMermasOperaciones = [];
    // mesMermasOperacion.forEach(mez => {
    //   totalMermasOperaciones.push(mez.total);
    //   mesEtiquetasMermasOperaciones.push(mez.mes);
    // })

    let arregloDatosFechas: any[] = [];
    let arregloFecha: any[] = [];

    //obtener todas las fechas de ambos arreglos sin repetir
    mesMermasOperacion.forEach(mermaOp => {
      arregloDatosFechas.push(
        {
          mes: mermaOp.mes,
          valores: {
            operacional: mermaOp.total,
            normal: 0
          }
        }
      )
    });

    mesMermas.forEach(merma => {
      //buscar el indice en el que se encuentra el mes identico
      let index: number = arregloDatosFechas.findIndex(x => x.mes == merma.mes);
      //si lo encuentra agregar a su valor campos la cantidad de mermas normales
      if (index != -1) {

        arregloDatosFechas[index]['valores'].normal = merma.total;

      }

    });

    //obtener los valorees de cada tipo de merma y el arreglo de las etiquetas
    arregloDatosFechas.forEach(fecha => {
      mesEtiquetasMermas.push(fecha.mes);

      mesTotalMermas.push(fecha['valores'].normal);
      totalMermasOperaciones.push(fecha['valores'].operacional);
    });

    this.resultadosCajas = {
      fechas: mesEtiquetasCajas, datos: mesTotalCajas
    };


    this.resultadosMermas = {
      fechas: mesEtiquetasMermas, datos: mesTotalMermas
    }

    this.resultadosMermasOperaciones = {
      fechas: mesEtiquetasMermasOperaciones, datos: totalMermasOperaciones
    }

    this.lineChartDataCajas = [
      { data: mesTotalCajas, label: "Cajas", fill: false, lineTension: 0.1 }
    ]

    this.lineChartLabelsCajas = mesEtiquetasCajas;

    this.lineChartDataMermas = [
      { data: mesTotalMermas, label: "Mermas", fill: false, lineTension: 0.1 },
      { data: totalMermasOperaciones, label: "Mermas Operaciones", fill: false, lineTension: 0.1 }
    ]

    this.lineChartLabelsMermas = mesEtiquetasMermas;

  }

  public randomize(): void {
    for (let i = 0; i < this.lineChartDataCajas.length; i++) {
      for (let j = 0; j < this.lineChartDataCajas[i].data.length; j++) {
        this.lineChartDataCajas[i].data[j] = this.generateNumber(i);
      }
    }

    for (let i = 0; i < this.lineChartDataMermas.length; i++) {
      for (let j = 0; j < this.lineChartDataMermas[i].data.length; j++) {
        this.lineChartDataMermas[i].data[j] = this.generateNumber(i);
      }
    }

    this.chart.forEach((child) => {
      child.chart.update()
    });
  }

  private generateNumber(i: number) {
    return Math.floor((Math.random() * (i < 2 ? 100 : 1000)) + 1);
  }

  // events
  public chartClicked({ event, active }: { event: MouseEvent, active: {}[] }): void {
    //console.log(event, active);
  }

  public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
    //console.log(event, active);
  }

  public changeColor() {
    this.lineChartColors[2].borderColor = 'green';
    this.lineChartColors[2].backgroundColor = `rgba(0, 255, 0, 0.3)`;
  }

  public changeLabel() {

    // this.chart.update();
  }

  back = () => {
    // debugger;
    switch (this.proviene) {
      case 1: //regresar a empacado interno
        this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoInterno', { state: { registro: 1 } })
        break;
      case 2:
        this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoExterno', { state: { registro: 1 } })
        break;
      default:
        this._router.navigateByUrl('EmpacadoEtiquetado')
        break;
    }
    // this._location.back();
  }

  generarExcel() {

    //console.log("cajas", this.resultadosCajas);
    //console.log("mermas", this.resultadosMermas);

    //cajas
    let cajasobj: any[] = [];
    let mermasobj: any[] = [];

    if (this.resultadosCajas.datos.length == 0 || this.resultadosCajas.datos === undefined) {
      this.openSnack("No hay registros", "Aceptar");
      return;
    }

    let fechaMes = "";
    cajasobj = [
      { A: this.companiaId == 0 ? "" : this.companias.filter(x => x.id == this.companiaId)[0].data, B: "", C: "", D: "" },
      { A: this.productoId == 0 ? "" : this.productos.filter(x => x.id == this.productoId)[0].data, B: "", C: "", D: "" },
      { A: this.direccionId == 0 ? "" : this.direcciones.filter(x => x.id == this.direccionId)[0].data, B: "", C: "", D: "" },
      { A: "", B: "", C: "", D: "" },
      { A: "Mes", B: "Cantidad de cajas", C: "Cantidad de mermas", D: "Cantidad de mermas operacionales" }
    ];

    let ws = XLSX.utils.json_to_sheet(cajasobj, { header: ["A", "B", "C", "D"], skipHeader: true });

    cajasobj = [];
    for (let index = 0; index < this.resultadosCajas.datos.length; index++) {
      fechaMes = this.resultadosCajas.fechas[index].split('/');
      cajasobj.push(
        {
          A: (fechaMes.length > 2 ? fechaMes[0] + "/" : "") + (fechaMes.length > 2 ? fechaMes[1] + "/" + fechaMes[2] : fechaMes[0] + "/" + fechaMes[1]),
          B: this.resultadosCajas.datos[index],
          C: this.resultadosMermas.datos[index],
          D: this.resultadosMermasOperaciones.datos[index]
        }
      );
    }

    //crear  hoja
    const wsCajas: XLSX.WorkSheet = XLSX.utils.sheet_add_json(ws, cajasobj, { skipHeader: true, origin: "A6" });
    wsCajas['!cols'] = [
      { width: 20 },
      { width: 20 },
      { width: 20 },
      { width: 20 },
    ];

    //crear libro
    const wb: XLSX.WorkBook = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(wb, wsCajas, "Reporte de armados");

    XLSX.writeFile(wb, "Exporte de reporte de armados.xlsx");

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
            this.getCompaniasForEmpacador();
            break;
          case "2":
            this.obtenerDirecciones();
            break;
          case "3":
            this.obtenerProductos();
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
