import { Component, OnInit, ViewChild } from '@angular/core';
import { Chart, ChartDataSets, ChartData } from 'chart.js'; 
import { DataServices } from '../Interfaces/Services/general.service';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { HttpClient, } from '@angular/common/http';


interface etiquetaEmbalajeFruta {
  etiquetaEmpacador: number;
  embalajeReproceso: number;
  recepcionFruta: number;
}

interface empaquesEnviadosReprocesoFrutaRecibida{
  productoId: number;
  nombre: number;
  companyId: number;
  totalProductos: number;
}

interface operacionEmpacador{
  mes: number;
  idEmpacador: number;
  totalOperaciones: number;
  numeroE: string;
  nombreE: string; 
  tipoEmpacador: string;
  FechaInicio: string;
  CompaniaId: number;
}
@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

    //Variabes tabla de donas
    Player: Array<any> = [];  
    Run: Array<any> = [];  
    chart: any; 
    Player2: Array<any> = [];  
    Run2: Array<any> = [];  
    chart2: any;  
    active: boolean = false;
    FechaInicio: Date; //Inicia en el primer día del mes actual, o en el primer día del mes del mes actual menos dos meses (operación para trimestre)
    FechaFinal: Date; //Inicia  la variable en el día actual, pero lo actualizamos al ultimo día del mes actual.


    companiaActual: number =  0;
    isType: number = 0;
    periodo;
    constructor(
      private dataService: DataServices,
      private _overlay: OverlayService,
      private http: HttpClient
    ) {
    }
  moduloEstadistica = 0;
  async ngOnInit() {
    //Evaluamos cual tiene permisos
    //Periodo trimestral = 79 | Periodo Semestral = 80 | Periodo Anual = 81 
    this.moduloEstadistica = parseInt(sessionStorage.getItem("Módulo de Estadísticas")) == 78 ? parseInt(sessionStorage.getItem("Módulo de Estadísticas")) : 0 ;
    this.periodo = parseInt(sessionStorage.getItem("Periodo Trimestral")) | parseInt(sessionStorage.getItem("Perdiodo Semestral")) | parseInt(sessionStorage.getItem("Periodo Anual"));
    console.log('Periodo', this.periodo, this.moduloEstadistica, parseInt(sessionStorage.getItem("Módulo de Estadísticas")));
    this.companiaActual =parseInt(sessionStorage.getItem("company"));
    this.FechaInicio = await new Date();
    this.FechaInicio = new Date(this.FechaInicio.getFullYear(), this.FechaInicio.getMonth(), 1);
    this.FechaFinal = await new Date();
    this.FechaFinal = new Date(this.FechaFinal.getFullYear(), this.FechaFinal.getMonth() + 1, 0);
    this.isType = parseInt(sessionStorage.getItem("isType"), 10);
    //console.log('Fecha Actual', this.FechaInicio, '\n Fecha final:', this.FechaFinal); 
    await this.callAllFunctions();
  }

  /**
   * Función encargada de llamar todas las a
  */
  endLoad : boolean = false;
  async callAllFunctions(){
    //Reiniciamos las variables de las graficas
    this.Player = [];
    this.Run = [];
    this.responseEmpaqueEnviadoReroceso = [];
    this.Player2 = [];
    this.Run2 = [];
    this.FrutaRecibidaReproceso = [];
    // Empacadores 
    this.operacionEmpacadores = [];
    this.labelEmpacadores = [];
    this.operacionEmpacador = [];
    await this.getEtiquetaEmabalajeFruta();
    await this.getEmpaquesEnviadoReproceso();
    await this.getFrutaRecibidaReproceso();
    await this.getEmpaquesEnviados();
    await this.getFrutaRecibida();
    await this.getOperacionesEmpacador();
    await this.getOperacionEmpacadores(); 
    this.endLoad = await true;
  }
  async changeSelect(any){
    if(any === 'Trimestre'){
      this.FechaInicio = await new Date();
      this.FechaInicio = new Date(this.FechaInicio.getFullYear(), this.FechaInicio.getMonth(), 1);
      this.FechaInicio.setMonth(this.FechaInicio.getMonth() - 3);
      console.log('Trimestre', this.FechaInicio);
      await this.callAllFunctions();
    }
    else if(any === 'MesAnterior'){
      this.FechaInicio = await new Date();
      this.FechaInicio = new Date(this.FechaInicio.getFullYear(), this.FechaInicio.getMonth(), 1);
      this.FechaInicio.setMonth(this.FechaInicio.getMonth() - 1);
      this.FechaFinal = new Date(this.FechaFinal.getFullYear(), this.FechaFinal.getMonth(), 0);
      console.log('Mes Anterior', this.FechaInicio, '\n Final', this.FechaFinal);
      await this.callAllFunctions();
    }
    else if(any === 'MesActual'){
      this.FechaInicio = await new Date();
      this.FechaInicio = new Date(this.FechaInicio.getFullYear(), this.FechaInicio.getMonth(), 1);
      console.log('Mes Actual', this.FechaInicio);
      await this.callAllFunctions();
    }
    else if(any == 'Semestral'){
      this.FechaInicio = await new Date();
      this.FechaInicio = new Date(this.FechaInicio.getFullYear(), this.FechaInicio.getMonth(), 1);
      this.FechaInicio.setMonth(this.FechaInicio.getMonth() - 6);
      console.log('Semestral', this.FechaInicio);
      await this.callAllFunctions();
    }
    else if(any == 'Anual'){
      this.FechaInicio = await new Date();
      this.FechaInicio = new Date(this.FechaInicio.getFullYear(), this.FechaInicio.getMonth(), 1);
      this.FechaInicio.setMonth(this.FechaInicio.getMonth() - 12);
      console.log('Anual', this.FechaInicio);
      await this.callAllFunctions();
    }
  }
  responseEtiquetaEmbalajeFruta: etiquetaEmbalajeFruta;
  response: any;
  emptyListEtiquetaEmablajeFruta: boolean = false;
  /**
   * Función encargada de recolectar la información de:
   * Etiquetado empacadores
   * Envío de empaque a reproceso / productores
   * Recepción de fruta de reproceso / productores
   */
  async getEtiquetaEmabalajeFruta() {
    let request = {
      companyId: this.companiaActual,
      fechaInicio: this.FechaInicio,
      fechaFinal: this.FechaFinal
    }
    let overlayRef : any;
    setTimeout(() => {
      overlayRef = this._overlay.open();
    }, 1);

    this.dataService.postData("Home/searchEtiquetadoEmpacadores", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.response = data;
        if(this.response)
          this.emptyListEtiquetaEmablajeFruta = false;
        else
          this.emptyListEtiquetaEmablajeFruta = true;
        this.responseEtiquetaEmbalajeFruta = this.response;
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);
      },
      error => {
        //console.log(error);
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);
      }
    );
  }

  responseEmpaqueEnviadoReroceso: Array<empaquesEnviadosReprocesoFrutaRecibida> = [];
  responseEmpaqueReproceso: any;
  emptyListEmpaqueReproceso: boolean = false;
  /**
   * Función encaragada de traer el listado de empaques enviados a reproceso / productores
   * por clamshell
   */
   emptyListEnviadoReproceso: boolean  = false;
  async getEmpaquesEnviadoReproceso() {
    let request = {
      companyId: this.companiaActual,
      fechaInicio: this.FechaInicio,
      fechaFinal: this.FechaFinal
    }
    let overlayRef : any;
    setTimeout(() => {
      overlayRef = this._overlay.open();
    }, 1);
    this.dataService.postData("Home/searchEmpaquesEnviadosReproceso", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseEmpaqueReproceso = data;
        if(this.responseEmpaqueReproceso)
          this.emptyListEnviadoReproceso = false;
        else
          this.emptyListEnviadoReproceso = true;
        this.responseEmpaqueEnviadoReroceso = this.responseEmpaqueReproceso.listEmpaquesEnviadosReproceso;
        let auxColores = [];
        this.responseEmpaqueEnviadoReroceso.forEach(element => {
         this.Player.push(element.nombre);
         this.Run.push(element.totalProductos);
         auxColores.push(this.colorHEX());
        });
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);
        this.chart =  new Chart('canvas', {  
          type: 'doughnut',  
          data: {  
            labels: this.Player,  
            datasets: [  
              {  
                data: this.Run,  
                borderColor: '#3cba9f',  
                backgroundColor: auxColores,
                fill: true  
              }  
            ]  
          },  
          options: {  
            title: {
              display: true,
              position: 'top',
              fontSize: 20,
              text: 'Empaques enviados a reproceso / productores.',
              fontStyle: 'italic'
            },
            legend: {  
              display: true,
              position: 'right',
              labels: {
                usePointStyle: true
              }
            },  
            scales: {  
              xAxes: [{  
                display: false // para no mostrar las leyendas debajo del la grafica  
              }],  
              yAxes: [{  
                display: false 
              }],  
            },
            responsive: true
          }  
        }).render();
      },
      error => {
        //console.log(error);
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);
      }
    );
  }

  
  FrutaRecibidaReproceso: Array<empaquesEnviadosReprocesoFrutaRecibida> = [];
  responseFrutaRecibidaReproceso: any;
  listFrutaRecibidaReprocesoResponse: boolean = false;
  /**
   * Función para obtener el procentaje de fruta recibida en reproceso.
   */
  emptyListFrutaRecibida: boolean = false;
  async getFrutaRecibidaReproceso() {
    let request = {
      companyId: this.companiaActual,
      fechaInicio: this.FechaInicio,
      fechaFinal: this.FechaFinal
    }
    let overlayRef : any;
    setTimeout(() => {
      overlayRef = this._overlay.open();
    }, 1);
    this.dataService.postData("Home/searchFrutaRecibidaReproceso", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseFrutaRecibidaReproceso = data;
        if(this.responseFrutaRecibidaReproceso)
          this.emptyListFrutaRecibida = false;
        else
          this.emptyListFrutaRecibida = true;

        //console.log('Fruta recibida', this.responseFrutaRecibidaReproceso);
        let coloresAux = [];
        this.FrutaRecibidaReproceso = this.responseFrutaRecibidaReproceso.listFrutaRecibidaReprocesoResponse;
        this.FrutaRecibidaReproceso.forEach(element => {
         this.Player2.push(element.nombre);
         this.Run2.push(element.totalProductos);
         coloresAux.push(this.colorHEX());
        });
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);

        this.chart2 =  new Chart('canvas2', {  
          type: 'doughnut',  
          data: {  
            labels: this.Player2,  
            datasets: [  
              {  
                data: this.Run2,  
                borderColor: '',  
                backgroundColor: coloresAux
              }  
            ]  
          },  
          options: { 
            title: {
              display: true,
              position: 'top',
              fontSize: 20,
              text: 'Fruta recibida de reproceso / productores.',
              fontStyle: 'italic'
            },
            legend: {  
              display: true,
              position: 'right',
              labels: {
                usePointStyle: true
              }
            }, 
            scales: {  
              xAxes: [{  
                display: false // para no mostrar las leyendas debajo del la grafica  
              }],  
              yAxes: [{  
                display: false 
              }],  
            }  
          }  
        }).render();
        console.log('Fruta recibida', this.FrutaRecibidaReproceso, this.chart2, this.responseFrutaRecibidaReproceso);
      },
      error => {
        //console.log(error);
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);
      }
    );
  }

  /**
   * Función encagada de agrupar los resultado, por meses, para la actividad de empacadores.
  */
  agrupa(data){
    let sorted = data.sort((a,b) => { return a.mes - b.mes });
    let grouped = [];
    let aux = {data: [], label: [], backgroundColor: [], fill: true};
    let currentId = null;
    sorted.forEach((item) => {
      if(currentId !== item.mes){
        if(currentId !== null)
          grouped.push(aux);
        currentId = item.mes;
        aux = {data: [], label: [], backgroundColor: [], fill: true};
      }
      aux.data.push(item.totalOperaciones);
      aux.label.push(item.nombreE);
      aux.backgroundColor.push(this.colorHEX());
    });
    return grouped;
  }
  labelEmpacadores: Array<any> =  [];  
  RunBar = [20,20,30, 0];  
  empacadorClamshell : any;
  operacionEmpacadores: Array<operacionEmpacador> = [];
  responseOperacionEmpacadores: any;
  listsearchOperacionEmpacadoresResponse: boolean = false;

  /**
   * generaLetra() / colorHEX(), estas funciones estan ligadas 
   * para generar colores hexadecimales.
  */
  generarLetra(){
    var letras = ["a","b","c","d","e","f","0","1","2","3","4","5","6","7","8","9"];
    var numero = (Math.random()*15).toFixed(0);
    return letras[numero];
  }
  colorHEX(){
    var coolor = "";
    for(var i=0;i<6;i++){
      coolor = coolor + this.generarLetra() ;
    }
    return "#" + coolor;
  }
  emptyListOperacionEmpacadores: boolean = false;
  /**
   * Funcion encargada de  traer la actividad genera de los empacadores, separando por empacador y mes
  */
  async getOperacionEmpacadores() {
    let request = {
      companyId: this.companiaActual,
      fechaInicio: this.FechaInicio,
      fechaFinal: this.FechaFinal
    }
    let overlayRef : any;
    setTimeout(() => {
      overlayRef = this._overlay.open();
    }, 1);
    let meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
    ////console.log('Color', await this.colorHEX());
    this.dataService.postData("Home/searchOperacionEmpacadores", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseOperacionEmpacadores = data;
        if(this.responseOperacionEmpacadores)
          this.emptyListOperacionEmpacadores = false;
        else
          this.emptyListOperacionEmpacadores = true;

        this.operacionEmpacadores = this.responseOperacionEmpacadores.listsearchOperacionEmpacadoresResponse;
        //Obtenemos los labels (Meses)
        let mesesAux = this.operacionEmpacadores.map(x =>  JSON.stringify(x.mes));
        let resultMeses = [];
        mesesAux.forEach(function(item, pos) {
          if(mesesAux.indexOf(item) == pos){
            resultMeses.push(meses[Number(item) - 1]);
          }
        });
        this.labelEmpacadores = [...resultMeses];
        let datas: Array<any> =  this.agrupa(this.operacionEmpacadores);
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);
        this.empacadorClamshell = new Chart('empacadorPorClamshell', {  
          type: 'horizontalBar',
          data: {  
            labels: this.labelEmpacadores,  
            datasets: datas
          },  
          options: { 
            title: {
              display: true,
              position: 'top',
              fontSize: 20,
              fontStyle: 'italic',
              text: 'Datos mostrados por unidad'
            }, 
            legend: {  
              display: true,
              align: 'center',
              position: 'top',
              labels: {
                usePointStyle: true
              }
            },  
            scales: {  
              xAxes: [{  
                display: true  
              }],  
              yAxes: [{  
                display: true  
              }],
            },
            responsive: true
          }
        }); 
      },
      error => {
        //console.log(error);
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);
      }
    );
  }

  // Acopios
  empaquesEnviados: any;
  frutaRecibida: any;

  async getEmpaquesEnviados() {
    this.empaquesEnviados = new Chart('canvasTotalEmpaquesEnviados', {  
      type: 'line',
      data: {
        
      }, 
      options: { 
        title: {
          display: true,
          position: 'top',
          fontSize: 20,
          fontStyle: 'italic',
          text: 'Empaques enviados a reproceso/productores'
        }, 
        legend: {  
          display: true,
          align: 'center',
          position: 'top',
          labels: {
            usePointStyle: true
          }
        },  
        scales: {  
          xAxes: [{  
            display: true  
          }],  
          yAxes: [{  
            display: true  
          }],
        }
      }
    }); 
  }

  async getFrutaRecibida() {
    const DATA_COUNT = 12;
    const labels = [];
    for (let i = 0; i < DATA_COUNT; ++i) {
      labels.push(i.toString());
    }
    const datapoints = [0, 20, 20, 60, 60, 120, 160, 180, 120, 125, 105, 110, 170];

    this.frutaRecibida = new Chart('canvasFrutaRecibida', {  
      type: 'line',
      data: {
        labels: labels,
        datasets: [
          {
            label: 'Acopio 1',
            data: datapoints,
            fill: false,
            cubicInterpolationMode: 'monotone',
          }
        ]
      }, 
      options: { 
        title: {
          display: true,
          position: 'top',
          fontSize: 20,
          fontStyle: 'italic',
          text: 'Fruta recibida de reproceso/productores'
        }, 
        legend: {  
          display: true,
          align: 'center',
          position: 'top',
          labels: {
            usePointStyle: true
          }
        },  
        scales: {  
          xAxes: [{  
            display: true  
          }],  
          yAxes: [{  
            display: true  
          }],
        }
      }
    });
  } 

  totalEmpacadorLabel = [];  
  dataSetsTotalEmpacador = [];  
  totalEmpacador : any;
  operacionEmpacador: Array<operacionEmpacador> = [];
  responseOperacionEmpacador: any;
  backgroundColorTotalEmpacadores: Array<any> = [];
  listsearchOperacionEmpacadorResponse: boolean = false; 
  emptyListOperacionEmpacador: boolean = false;
  /**
   * Función encargada de traer actividad de empacadores, agrupado por operaciones (no diferencia meses.)
   */
  async getOperacionesEmpacador(){
    let request = {
      companyId: this.companiaActual,
      fechaInicio: this.FechaInicio,
      fechaFinal: this.FechaFinal
    }
    let overlayRef : any;
    setTimeout(() => {
      overlayRef = this._overlay.open();
    }, 1);
    this.dataService.postData("Home/searchOperacionEmpacador", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseOperacionEmpacador = data;
        if(this.responseOperacionEmpacador)
          this.emptyListOperacionEmpacador = false;
        else
          this.emptyListOperacionEmpacador = true;

        this.operacionEmpacador = this.responseOperacionEmpacador.listsearchOperacionEmpacadorResponse;

        let datasetFinal = [];
        for(const element of this.operacionEmpacador){
          let auxDataset = {
            label: element.nombreE,
            data: [element.totalOperaciones,0],
            backgroundColor: [this.colorHEX()],
            fill: true
          }
          datasetFinal.push(auxDataset);
          console.log('FOR', element);
        };
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);
        this.totalEmpacador = new Chart('canvasTotalPorEmpacador', {  
          type: 'horizontalBar',
          data: {
            datasets: [...datasetFinal]
          }, 
          options: { 
            title: {
              display: true,
              position: 'top',
              fontSize: 20,
              fontStyle: 'italic',
              text: 'Total por empacador'
            }, 
            legend: {  
              display: true,
              align: 'center',
              position: 'top',
              labels: {
                usePointStyle: true
              }
            },  
            scales: {  
              xAxes: [{  
                display: true  
              }],  
              yAxes: [{  
                display: true  
              }],
            }
          }
        }); 
      },
      error => {
        //console.log(error);
        setTimeout(() => {
          this._overlay.close(overlayRef);
        }, 1);
      }
    );
  }
}
