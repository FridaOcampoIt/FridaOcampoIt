import { Component, OnInit, Inject } from '@angular/core';
import { MouseEvent } from '@agm/core';
import { MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-dialogo-ubicacion',
  templateUrl: './dialogo-ubicacion.component.html',
  styleUrls: ['./dialogo-ubicacion.component.less']
})
export class DialogoUbicacionComponent implements OnInit {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }


  //Variables para el control del mapa
  latitudMarker = this.data.lat;
  longitudMarker = this.data.long;
  latitudFloat;
  longitudFloat;

  //Funcion para iniciar y centrar el mapa
  initMap() {
    this.latitudFloat = 25.2113201;
    this.longitudFloat = -101.524912;
  }

  ngOnInit() {
    this.initMap();
  }

}
