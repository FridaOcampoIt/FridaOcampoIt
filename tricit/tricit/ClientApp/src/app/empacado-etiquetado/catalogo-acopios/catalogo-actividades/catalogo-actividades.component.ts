import { Component, OnInit , ViewChild} from "@angular/core";
import { DataServices } from '../../../Interfaces/Services/general.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar, MatSort } from '@angular/material';
import { DateAdapter } from '@angular/material/core';
import { OverlayService } from  '../../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { ExporterService } from '../../../Interfaces/Services/exporter.service';
import { MovimientosData,
         SearchMovimientoRequest,
         SearchMovimientoResponse,
         SearchComboMovimientoRequest,
         SearchComboMovimientoResponse
        } from '../../../Interfaces/Models/MovimientosModels';
import { enviroments } from '../../../Interfaces/Enviroments/enviroments';

@Component({
    selector: 'catalogo-actividades-component',
    templateUrl: './catalogo-actividades.component.html',
    styleUrls:['./catalogo-actividades.component.less']
})
export class CatalogoActividadesComponent implements OnInit {
  itemsPagina: number[] = enviroments.pageSize;
  
  //Representa la instancia para el sorting de columnas (Clasificación por)
  @ViewChild(MatSort) sort: MatSort;

  //Representa componente de paginación para la tabla
  @ViewChild(MatPaginator) paginator: MatPaginator;
  response = new SearchMovimientoResponse();
  displayedColumns: string[] = [
                                'movimientoId',
                                'nombreAgrupacion', 
                                'numeroCajas',
                                'producto',  
                                'cantidad',
                                //'numeroMerma', 
                                'tipoMovimiento', 
                                'nombreRemitente',
                                'apellidoRemitente', 
                                'nombreDestinatario',
                                'apellidoDestinatario',
                                'quien',
                                'fechaIngreso',
                                'cantIndMov',
                                'lote'];
  dataSource = new MatTableDataSource<MovimientosData>();
  selection = new SelectionModel<MovimientosData>(false, []);
  idAcopio
  acopioName: any
  constructor(
    private dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _router: Router,
    private dateAdapter: DateAdapter<Date>,
    private $route: ActivatedRoute,
    private excelService: ExporterService,
  ){
    this.dateAdapter.setLocale('en-GB'); //dd/MM/yyyy
    let auxiliarAcopio:any;
    this.$route.params.forEach(param =>{
      this.acopioName = param.acopioName;
      this.idAcopio = param.id;
    }
      );
  }

  companiaId = 0;
  acopioId = 0;
  async ngOnInit() {
    console.log('AcopioId', this.acopioId);
    this.companiaId =  await parseInt(sessionStorage.getItem("company"), 10);
    await this.BusquedaCombos();
    await this.comboProducto(this.companiaId);
  }

   // Variables para la vista
   productoId: number = 0;
   cantidad: number;
   tipoMovimientoId: number = 0; 
   movimientoId: number = 0;
   fechaIngreso: string;
   fechaCaducidad: string = ''; 

   anio: string = new Date().getFullYear().toString();
   //Seteo de hoy
   aDate = new Date();
   fechaIngresoDe: Date = new Date();
   fechaIngresoHasta: string = this.anio.concat('-12-31'.toString());

   overlayRef: OverlayRef;
   emptyList: boolean = false;
   quienes : number = 1;
   list = [{
    "id": 1,
    "data": "Remitente"
    }, {
    "id": 2,
    "data": "Destinatario"
   }]

   //variables para los combos
   responseCombos = new SearchComboMovimientoResponse();


   //Funcion despues de obtener todos los datos se establece el paginado
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }
  BusquedaCombos() {
    var request = new SearchComboMovimientoRequest();
    
    let auxOverlayRef = this._overlay.open();
    this.dataService.postData<SearchComboMovimientoResponse>("Movimientos/searchMovimientoDropDown", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.responseCombos = data;
        console.log('DATA', this.responseCombos);
        this._overlay.close(auxOverlayRef);
        this.Busqueda();
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
           // this.relogin("BusquedaCombos");
        } else {
          console.log(error);
        }
      }
    )
  }

  productosListadoByCompany: any;
  comboProducto(companyId) {
    let data = {
      company: companyId
    }
    let auxOverlayRef = this._overlay.open();
    this.dataService.postData<any>("Product/searchProductDropDownImport", sessionStorage.getItem("token"), data).subscribe(
      data => {
        this.productosListadoByCompany = data['familyDropDown'];
        this._overlay.close(auxOverlayRef);
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
           // this.relogin("BusquedaCombos");
        } else {
          console.log(error);
        }
      }
    )
  }
  //Funcion para realizar la busqueda  (Las variables son las que se envian para hacer la búsqueda)
  Busqueda() {
    const dateAuxiliar = new Date(this.fechaIngresoDe);
    dateAuxiliar.setDate(dateAuxiliar.getDate() - 1);
    var dataRequest = {
      producto: this.productoId,
      tipoMovimientoId: this.tipoMovimientoId,
      fechaIngresoDe: dateAuxiliar.toISOString(),
      fechaIngresoHasta: this.fechaIngresoHasta,
      acopioId: this.idAcopio
    };
    console.log('Request',dataRequest);
    //request.usuario = parseInt(sessionStorage.getItem("idUser"));
    let auxOverlayRef = this._overlay.open();
    this.dataService.postData<any>("Movimientos/searchMovimientoByAcopioId", sessionStorage.getItem("token"), dataRequest).subscribe(
      data => {
        console.log('DATA Movientos', data);
        if (data['movimientosDataList'].length > 0)
          this.emptyList = false;
        else
          this.emptyList = true;
          this.dataSource.data = data['movimientosDataList'];
          this.selection.clear();
          this._overlay.close(auxOverlayRef);
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {
          //this.relogin("Busqueda");
        } else {
          console.log(error);
        }
        this._overlay.close(this.overlayRef);
      }
    );
  }
  //Funcion para realizar el proceso del relogin
  back = () => {
    this._router.navigateByUrl('EmpacadoEtiquetado/Acopios', { state: { registro: 1 } })
  }
  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
        duration: 5000
    })
  }
  
  generarExcel():void{
    var datosExportar = this.dataSource.data.map( item => { 
      console.log(item);
      return { 'Agrupación': item.nombreAgrupacion , 
              Cajas: item.numeroCajas,
              Producto: item.producto,
              Cantidad:  item.cantidad,
              Tipo: item.tipoMovimiento,
              Remitente: item.nombreRemitente,
              Destinatario: item.nombreDestinatario,
              'Fecha Ingreso':  item.fechaIngreso,
              'Fecha Caducidad': item.fechaCaducidad,
              'Lote': item.lote,
            }; 
    });
    this.excelService.exportToExcel(datosExportar, 'Exportacion de movimientos');
  }
}