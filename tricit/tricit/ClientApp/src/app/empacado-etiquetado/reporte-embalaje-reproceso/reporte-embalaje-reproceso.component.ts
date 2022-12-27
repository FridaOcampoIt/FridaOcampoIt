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


@Component({
  selector: 'app-reporte-embalaje-reproceso',
  templateUrl: './reporte-embalaje-reproceso.component.html',
  styleUrls: ['./reporte-embalaje-reproceso.component.less']
})
export class ReporteEmbalajeReprocesoComponent implements OnInit {

  faFileExport = faFileExport;
  /**
   * Libreria de la grafica https://valor-software.com/ng2-charts/#/LineChart
   */
  public lineChartDataEnviados: ChartDataSets[] = [
    { data: [0], label: 'Productos Enviados', fill: false }
  ];
  public lineChartDataRecibidos: ChartDataSets[] = [
    { data: [0], label: 'Productos Recibidos', fill: false }
  ];
  public lineChartDataMerma: ChartDataSets[] = [
    { data: [0], label: 'Merma Reportada', fill: false }
  ];
  public lineChartDataMermaSinReportar: ChartDataSets[] = [
    { data: [0], label: 'Merma Sin Reportar', fill: false }
  ];
  
  public lineChartLabelsEnviados: Label[] = [""];
  public lineChartLabelsRecibidos: Label[] = [""];
  public lineChartLabelsMerma: Label[] = [""];
  public lineChartLabelsMermaSinReportar: Label[] = [""];
  
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
            callback: function(label: number, index, labels) {
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
    { // grey
      backgroundColor: 'rgba(148,159,177,0)',
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

  producerId: number = 0;
  producerNumber: string;
  producerName: string;
  productoId: number;
  state: any = {};
  overlayRef: OverlayRef;
  busquedaGenerica: string;

  productos: any[] = [];

  dateStart: Date = new Date();
  dateEnd: Date = new Date();
  maxDate: Date = new Date();
  minDate: Date = new Date();

  resultadosEnviados: any = [];
  resultadosRecibidos: any = [];
  resultadosMerma: any = [];
  resultadosMermaSinReportar: any = [];

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
    this.producerId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
    this.producerNumber = this._route.snapshot.paramMap.get('number');
    this.producerName = this._route.snapshot.paramMap.get('name');

    this.busquedaGenerica = "";
    this.productoId = 0;

    // debugger;
    let today: Date = new Date();
    this.dateStart = today;
    this.dateEnd.setDate(today.getDate() + 1);
    this.minDate.setDate(today.getDate() + 1);

    this.obtenerProductos();
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

  obtenerProductos() {
    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);
    
    //debugger
    this._dataService.postData<any>("PackedLabeled/SearchProductCombo", sessionStorage.getItem("token"), {}).subscribe(
      data => {
        //  
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
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
      producerNumber: this.producerNumber,
      productId: this.productoId,
      dateStart: "",
      dateEnd: ""
    };

    if (this.dateEnd.toString() != "" && this.dateStart.toString() == "") {
      this.openSnack("Seleccione una fecha inicial", "Aceptar");
      return;
    }

    if (!(this.productoId > 0)) {
      this.openSnack("Seleccione un producto", "Aceptar");
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
    this._dataService.postData<any>("AgroPacked/SearchReportPackagingReprocessing", sessionStorage.getItem("token"), datos).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
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
    
    /***********************************
     * construcción de lineas Enviados
     ***********************************/
    let mesEnviados = this.getDataByGraph(datos, "infoDataShipment");
    let mesTotalEnviados = [];
    let mesEtiquetasEnviados = [];
    mesEnviados.forEach(mes => {
      mesTotalEnviados.push(mes.total);
      mesEtiquetasEnviados.push(mes.mes);
    });

    this.resultadosEnviados = {
      fechas: mesEtiquetasEnviados, datos: mesTotalEnviados
    };

    this.lineChartDataEnviados = [
      { data: mesTotalEnviados, label: "Productos Enviados", fill: false }
    ];

    this.lineChartLabelsEnviados = mesEtiquetasEnviados;

    /***********************************
     * construcción de lineas Recibidos
     ***********************************/
    let mesRecibidos = this.getDataByGraph(datos, "infoDataReceived");
    let mesTotalRecibidos = [];
    let mesEtiquetasRecibidos = [];
    mesRecibidos.forEach(mes => {
      mesTotalRecibidos.push(mes.total);
      mesEtiquetasRecibidos.push(mes.mes);
    });

    this.resultadosRecibidos = {
      fechas: mesEtiquetasRecibidos, datos: mesTotalRecibidos
    };

    this.lineChartDataRecibidos = [
      { data: mesTotalRecibidos, label: "Productos Recibidos", fill: false }
    ];

    this.lineChartLabelsRecibidos = mesEtiquetasRecibidos;

    /***********************************
     * construcción de lineas Merma
     ***********************************/
    let mesMerma = this.getDataByGraph(datos, "infoDataWaste");
    let mesTotalMerma = [];
    let mesEtiquetasMerma = [];
    mesMerma.forEach(mes => {
      mesTotalMerma.push(mes.total);
      mesEtiquetasMerma.push(mes.mes);
    });

    this.resultadosMerma = {
      fechas: mesEtiquetasMerma, datos: mesTotalMerma
    };

    this.lineChartDataMerma = [
      { data: mesTotalMerma, label: "Merma Reportada", fill: false }
    ];

    this.lineChartLabelsMerma = mesEtiquetasMerma;

    /*********************************************
     * construcción de lineas Merma Sin Reportar
     *********************************************/
    let mesMermaSinReportar = this.getDataByGraph(datos, "infoDataWasteReport");
    let mesTotalMermaSinReportar = [];
    let mesEtiquetasMermaSinReportar = [];
    mesMermaSinReportar.forEach(mes => {
      mesTotalMermaSinReportar.push(mes.total);
      mesEtiquetasMermaSinReportar.push(mes.mes);
    });

    this.resultadosMermaSinReportar = {
      fechas: mesEtiquetasMermaSinReportar, datos: mesTotalMermaSinReportar
    };

    this.lineChartDataMermaSinReportar = [
      { data: mesTotalMermaSinReportar, label: "Merma Sin Reportar", fill: false }
    ];

    this.lineChartLabelsMermaSinReportar = mesEtiquetasMermaSinReportar;
  }

  getDataByGraph(datos, graph) {
    let mesArray = [];
    datos[graph].forEach(boxDay => {
      let fecha = new Date(""+boxDay["dateMov"].split('-')[0]+"/"+(parseInt(boxDay["dateMov"].split('-')[1]))+"/"+(parseInt(boxDay["dateMov"].split('-')[2])));
      let formato = "";
      let semanaini = 1;
      let semanaend = 7;

      let initd = new Date(this.dateStart);
      let initf = new Date(this.dateEnd);
      let difFechas = Math.round((initf.getTime() - initd.getTime()) / (1000 * 60 * 60 * 24));
      let esdia = (fecha.getDate()+"").length == 1 ? "0"+fecha.getDate() : fecha.getDate();
      let esmes = ((fecha.getMonth() + 1)+"").length == 1 ? "0"+(fecha.getMonth() + 1) : fecha.getMonth() + 1;

      if (initd.getMonth() != initf.getMonth()) {
        formato = `${esmes}/${fecha.getFullYear()}`; // filtrado por mes
      } else if(difFechas <= 6) {
        formato = `${esdia}/${esmes}/${fecha.getFullYear()}`; // filtrado por dia
      } else {
        if(fecha.getDate() >= 8 && fecha.getDate() <= 14) {
          semanaini = 8; semanaend = 14;
        } else if(fecha.getDate() >= 15 && fecha.getDate() <= 21) {
          semanaini = 15; semanaend = 21;
        } else if(fecha.getDate() >= 22 && fecha.getDate() <= 28) {
          semanaini = 22; semanaend = 28;
        } else if(fecha.getDate() >= 29 && fecha.getDate() <= 35) {
          semanaini = 29; semanaend = 35;
        }
        formato = `(${semanaini}-${semanaend})/${esmes}/${fecha.getFullYear()}`; // filtrado por semana
      }

      let indExist = mesArray.findIndex(reg => reg.mes == formato);

      if (indExist == -1) {
        let sumcaja = {
          mes: formato,
          total: boxDay["total"]
        }

        mesArray.push(sumcaja);
      } else {
        //Ya existe el mes, ahora sumar
        mesArray[indExist]["total"] += boxDay["total"];
      }
    });
    return mesArray;
  }

  public randomize(): void {
    for (let i = 0; i < this.lineChartDataEnviados.length; i++) {
      for (let j = 0; j < this.lineChartDataEnviados[i].data.length; j++) {
        this.lineChartDataEnviados[i].data[j] = this.generateNumber(i);
      }
    }

    for (let i = 0; i < this.lineChartDataRecibidos.length; i++) {
      for (let j = 0; j < this.lineChartDataRecibidos[i].data.length; j++) {
        this.lineChartDataRecibidos[i].data[j] = this.generateNumber(i);
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
    this._router.navigateByUrl('EmpacadoEtiquetado/EmpacadoAgro', { state: { registro: 1 } })
    // this._location.back();
  }

  generarExcel() {
    //cajas
    let objexe: any[] = [];
    let mermasobj: any[] = [];
    let upex = false;

    if (this.resultadosEnviados.datos.length == 0 || this.resultadosEnviados.datos === undefined) {
      this.openSnack("No hay registros", "Aceptar");
      return;
    }

    let fechaMes = "";
    objexe = [
      {A: this.producerName, B: "", C: "", D: "", E: ""},
      {A: this.productoId == 0 ? "" : this.productos.filter(x => x.id == this.productoId)[0].data, B:"", C:"", D:"", E: ""},
      {A: "", B: "", C: "", D: "", E: ""},
      {A: "", B: "", C: "", D: "", E: ""},
      {A: "Mes", B: "Cantidad de enviados", C: "Cantidad de recibidos", D: "Cantidad merma", E: "Cantidad merma sin reportar"}
    ];
    
    let ws = XLSX.utils.json_to_sheet(objexe, {header: ["A", "B", "C", "D", "E"], skipHeader: true});

    objexe = [];
    // Enviados
    for (let index = 0; index < this.resultadosEnviados.datos.length; index++) {
      fechaMes = this.resultadosEnviados.fechas[index].split('/');
      objexe.push(
        {
          A: (fechaMes.length > 2 ? fechaMes[0]+"/" : "")+(fechaMes.length > 2 ? fechaMes[1]+"/"+fechaMes[2] : fechaMes[0]+"/"+fechaMes[1]),
          B: this.resultadosEnviados.datos[index],
          C: "",
          D: "",
          E: ""
        }
      );
    }

    fechaMes = "";
    // Recibidos
    this.resultadosRecibidos.datos.forEach((item, index) => {
      fechaMes = this.resultadosRecibidos.fechas[index].split('/');
      fechaMes = (fechaMes.length > 2 ? fechaMes[0]+"/" : "")+(fechaMes.length > 2 ? fechaMes[1]+"/"+fechaMes[2] : fechaMes[0]+"/"+fechaMes[1]);
      objexe = this.makeBodyExe(item, fechaMes, objexe, 1);
    });

    fechaMes = "";
    // Merma
    this.resultadosMerma.datos.forEach((item, index) => {
      fechaMes = this.resultadosMerma.fechas[index].split('/');
      fechaMes = (fechaMes.length > 2 ? fechaMes[0]+"/" : "")+(fechaMes.length > 2 ? fechaMes[1]+"/"+fechaMes[2] : fechaMes[0]+"/"+fechaMes[1]);
      objexe = this.makeBodyExe(item, fechaMes, objexe, 2);
    });

    fechaMes = "";
    // Merma Sin Reportar
    this.resultadosMermaSinReportar.datos.forEach((item, index) => {
      fechaMes = this.resultadosMermaSinReportar.fechas[index].split('/');
      fechaMes = (fechaMes.length > 2 ? fechaMes[0]+"/" : "")+(fechaMes.length > 2 ? fechaMes[1]+"/"+fechaMes[2] : fechaMes[0]+"/"+fechaMes[1]);
      objexe = this.makeBodyExe(item, fechaMes, objexe, 3);
    });

    //crear  hoja
    const wsCajas: XLSX.WorkSheet = XLSX.utils.sheet_add_json(ws, objexe, {skipHeader: true, origin: "A6"});
    wsCajas['!cols'] = [
      { width: 20 },
      { width: 20 },
      { width: 20 },
      { width: 20 }, 
      { width: 20 }, 
    ];

    //crear libro
    const wb: XLSX.WorkBook = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(wb, wsCajas, "Reporte de embalaje reproceso");

    XLSX.writeFile(wb, "Exporte de reporte de embalaje reproceso.xlsx");

  }

  makeBodyExe(item, fechaMes, objexe, graph) {
    let upex = false;
    objexe.forEach(i => {
      if(i.A == fechaMes) {
        i.C = graph == 1 ? item : i.C;
        i.D = graph == 2 ? item : i.D;
        i.E = graph == 3 ? item : i.E;
      } else {
        upex = true;
      }
    });
    if(upex) {
      objexe.push({
        A: fechaMes,
        B: "",
        C: graph == 1 ? item : "",
        D: graph == 2 ? item : "",
        E: graph == 3 ? item : ""
      });
    }
    return objexe;
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

}
