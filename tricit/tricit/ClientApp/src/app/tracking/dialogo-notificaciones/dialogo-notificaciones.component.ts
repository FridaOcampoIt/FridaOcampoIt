import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';

@Component({
  selector: 'app-dialogo-notificaciones',
  templateUrl: './dialogo-notificaciones.component.html',
  styleUrls: ['./dialogo-notificaciones.component.css']
})
export class DialogoNotificacionesComponent implements OnInit {

  notificaciones: any[] = []
  familia: any[] = [];
  ciu: string = "";

  constructor(
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) { }
  
  ln: any;
  espanol: boolean = true;
  ngOnInit() {
    
    this.ln  = navigator.language || window.navigator['userLanguage']; 

    console.log('Detectamos lenguaje', this.ln);
    if(this.ln.match('es')){
      console.log('Español');
      this.espanol = true;
    }else if(this.ln.match('en')){
      console.log('Ingles');
      this.espanol = false;
    }

    console.log('GALERIA', this._data)
    console.log('NOTIFICACION',this._data);
    this.notificaciones = this._data["notificacion"];
    this.familia = this._data["familia"];
    this.ciu = this._data["codigo"];
    this.notificaciones["nombreArchivo"] = this._data["galeria"] ? `${enviroments.urlBase}${this.notificaciones["nombreArchivo"]}` :`${enviroments.urlBase}AlertFiles/${this.notificaciones["nombreArchivo"]}`;
    console.log(this.notificaciones, this.notificaciones["nombreArchivo"]);
  }

}
