import { Component, HostListener, OnInit } from '@angular/core';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { Router } from '@angular/router';
import {SelectionModel} from '@angular/cdk/collections';
import { DialogAgregarComponent } from '../dialog-agregar/dialog-agregar.component';
import { DialogEliminarSectorComponent } from '../dialog-eliminar/dialog-eliminar-sector.component';
import { ActivatedRoute, Params } from '@angular/router';
import { DialogAsignarFormularioComponent } from '../dialog-asignar-formulario/dialog-asignar-formulario.component';
import { Location } from '@angular/common';


import {
  Form,
  SearchFormsResponse,
  FormProcessResponse,
} from '../../../Interfaces/Models/Sectors&Forms';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';

@Component({
  selector: 'app-formularios',
  templateUrl: './formularios.component.html',
  styleUrls: ['./formularios.component.less']
})
export class FormulariosComponent implements OnInit {
  constructor( private router: Router, 
    private _dialog: MatDialog,
    private rutaActiva: ActivatedRoute,
    private location:Location,
    private _overlay: OverlayService,
    private _dataService: DataServices,
    ) {
      this.rutaActiva.params.forEach(params=>{
        this.idSector=params.id;
      })
     }
   
      valor=null;
      Sector: {id: number};
      ruta;
      overlayRef: OverlayRef;
      auxLoading: any;
      idSector:number=0;
      //varibles para la vista
      nombre:string="";
      sectorId:number=0;
      descripcionCorta:string="";
      response = new SearchFormsResponse();
      
    ngOnInit() {
    //valor pasado por parametro del componente sector
    this.Sector = {
      id: this.rutaActiva.snapshot.params.id,
    };
    this.rutaActiva.params.subscribe(
      (params: Params) => {
        this.Sector.id = params.id;
      }
    );
    this.SearchForms();  
    }

    /*NAVEGAR A FORMULARIOS*/
    apartadoCate(nombre,formularioIdd){
      this.router.navigate(['Configuracion/ConfiguracionGeneral/Sector/'+this.Sector.id+'/Formulario' , formularioIdd, 'Categoria',nombre]);
    }
  
    /*SELECCION DE SECTOR */
    selectSector(i:number){
    this.valor=i;
    }

    /*MODALS */
    eliminarForm(estatus,nombre){
      let title="Eliminar";
      let eliminarForm=false;
  
      const dialogRef = this._dialog.open(DialogEliminarSectorComponent, {
        width: 'dialog-comp  ',
        disableClose: true,
        data: {
            /*id: this.idCompany,*/
            title: title,
            estatusEliminar:eliminarForm,
            estatusForm:estatus,
            nombre:nombre
        }, 
    });
    dialogRef.afterClosed().subscribe(result => {
    });
    }

    agregarSector(bandera,formularioId){
      console.log(this.idSector)
      let title="Agregar formulario"
      let temporal=[];
    this.response.formsDataList.forEach(item=>{
      temporal.push(item.nombre.toUpperCase())
    })
      if (!bandera) {
        title = "Editar formulario"
      }
      const dialogRef = this._dialog.open(DialogAgregarComponent, {
        width: 'dialog-comp  ',
        disableClose: true,
        data: {
            /*id: this.idCompany,*/
            title: title,
            formId:formularioId,
            idSectorToForm:this.idSector,
            nombresForm:temporal
        }, 
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.SearchForms();
      }
      
    });
    }
    
    propiedadesInput(valor:number){
      valor;
      const dialogRef = this._dialog.open(DialogAsignarFormularioComponent, {
        width: 'dialog-comp  ',
        disableClose: true,
        data: {
            valor:valor,
        }, 
      });
      dialogRef.afterClosed().subscribe(result => {
      });
    }
    goBack(){
      this.location.back();
      this.ruta=this.router.url;
    }

    
    SearchForms() {
    let request = new Form();
    request.sectorId = this.idSector;
    setTimeout(() => {
      this.overlayRef = this._overlay.open();
     }, 1);
   
    this._dataService.postData<SearchFormsResponse>("Forms/searchListForms", sessionStorage.getItem("token"), request).subscribe(
      data => {
        this.response = data;
        console.log(this.response = data)
        this.response.formsDataList;
        console.log(this.response.formsDataList)
        setTimeout(() => {
          this.auxLoading = this._overlay.close(this.overlayRef);
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
    this.valor=null;  
  }
    asignarCompnia(){

  }
}
