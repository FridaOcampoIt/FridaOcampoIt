import { Component, OnInit } from '@angular/core';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { Router,ActivatedRoute, NavigationEnd } from '@angular/router';
import { DialogEliminarSectorComponent } from './dialog-eliminar/dialog-eliminar-sector.component';
import {SelectionModel} from '@angular/cdk/collections';
import { MatSnackBar } from '@angular/material';
import { DialogAgregarComponent } from './dialog-agregar/dialog-agregar.component';



//DataServices and Models
import {
  Sector,
  SearchSectorsResponse,
  SectorProcessResponse,
} from '../../Interfaces/Models/Sectors&Forms';
import { DataServices } from '../../Interfaces/Services/general.service';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
@Component({
  selector: 'app-sectores',
  templateUrl: './sectores.component.html',
  styleUrls: ['./sectores.component.less']
})

export class SectoresComponent implements OnInit {
  
  constructor( private router: Router, 
  private _dialog: MatDialog,
  private _dataService: DataServices,
  private snack: MatSnackBar,
  private _overlay: OverlayService,) { 
  }

  valor=null;
  overlayRef: OverlayRef;
  auxLoading: any;
  //variables para la vista 
  nombre:string="";
  sectorId:number=0;
  descripcionCorta:string="";
  response = new SearchSectorsResponse();
    

  ngOnInit() {
    this.SearchSectors()
    this.valor=null;
  }

  /*NAVEGAR A FORMULARIOS*/
  apartadoForm(sectorId){
    this.router.navigate(['Configuracion/ConfiguracionGeneral/Sector', sectorId, 'Formulario']);  
  }

  /*SELECCIÓN DE SECTOR */
  selectSector(i:number){
    this.valor=i;
  }

  /*MODALS */
  modalEliminarSector(sectorId:number,nombre:string,estatus:number){
    let title="Eliminar";
    let estatusSector=true;
    const dialogRef = this._dialog.open(DialogEliminarSectorComponent, {
      width: 'dialog-comp  ',
      disableClose: true,
      data: {
          title: title,
          nombre:nombre,
          estatus:estatus,
          estatusEliminar:estatusSector
      }, 
    });
    dialogRef.afterClosed().subscribe(result => { 
      if (result) {
        this.deleteSectors(sectorId);
      }  
    });
  }

  modalAgregarSector(bandera,idSector){
    let title="Agregar sector";
    let temporal=[];
    this.response.sectorsDataList.forEach(item=>{
      temporal.push(item.nombre.toUpperCase())
    })
    if (!bandera) {
      title = "Editar sector";
    }
    const dialogRef = this._dialog.open(DialogAgregarComponent, {
      width: 'dialog-comp  ',
      disableClose: true,
      data: {
        title: title,
        nombres:temporal,
        idSector:idSector
      }, 
    });
    dialogRef.afterClosed().subscribe(  result => {
      console.log(result)
      if (result) {
        console.log("mostar nueva consulta")
         this.ngOnInit();
      }
    });
  }

 
  
  //consulta
  SearchSectors() {
    let request = new Sector();
    request.nombre = this.nombre;
    request.descripcionCorta = this.descripcionCorta;
    setTimeout(() => {
      this.auxLoading = this._overlay.open();
    }, 1);
   
    this._dataService.postData<SearchSectorsResponse>("Sectors/searchListSectors", sessionStorage.getItem("token"), request).subscribe(
      data => {
        console.log(data)
        this.response = data;
        this.response.sectorsDataList;
        console.log(this.response.sectorsDataList)
        setTimeout(() => {
          this.auxLoading = this._overlay.close(this.auxLoading);
        }, 1);
      },
      error => {
          if (error.error.hasOwnProperty("messageEsp")) {
            console.log(error);
          } else {
             console.log(error);
          }
      }
    );  
  }

  deleteSectors(sectorId){
    let request = new Sector();
    request.sectorId = sectorId;
    console.log(request.sectorId)
    setTimeout(() => {
      this.auxLoading = this._overlay.open();
     }, 1)
    this._dataService.postData<SectorProcessResponse>("Sectors/deleteSectors", sessionStorage.getItem("token"), request).subscribe(
       data => {
      console.log(data);
      if (data.messageEsp != "") {
          this.openSnack(data.messageEsp, "Aceptar");
      } else {
        this.openSnack("Sector eliminado con éxito", "Aceptar");
        setTimeout(() => {
          this.auxLoading = this._overlay.close(this.auxLoading);
        }, 1);    
        this.ngOnInit() 
      }
      },
      error => {
        if (error.error.hasOwnProperty("messageEsp")) {

        } else {
            this.openSnack("Error al mandar la solicitud", "Aceptar");
            this.openSnack(error.messageEsp, "Aceptar");
        }
      }
    )
  }

  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
        duration: 5000
    })
  }
}
