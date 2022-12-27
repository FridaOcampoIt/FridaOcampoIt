import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RastreoRoutingModule } from './rastreo-routing.module';
import { RastreoComponent } from './rastreo.component';
import { MaterialModule } from '../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { DialogoUbicacionComponent } from './dialogo-ubicacion/dialogo-ubicacion.component';
import { AgmCoreModule } from '@agm/core';
import { DialogoMultiplesComponent } from './dialogo-multiples/dialogo-multiples.component';
import { DialogoDetalleComponent } from './dialogo-detalle/dialogo-detalle.component';
import { DialogoInformacionComponent } from './dialogo-informacion/dialogo-informacion.component';
import { DialogoLegalComponent } from './dialogo-legal/dialogo-legal.component';

@NgModule({
  declarations: [RastreoComponent, DialogoUbicacionComponent, DialogoMultiplesComponent, DialogoDetalleComponent, DialogoInformacionComponent, DialogoLegalComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MaterialModule,
    FontAwesomeModule,
    RastreoRoutingModule,
    FormsModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyD7DabiqicXnI2USUBChdApKURDeQdtdvg'
    })
  ],
  entryComponents: [DialogoUbicacionComponent, DialogoMultiplesComponent, DialogoDetalleComponent, DialogoLegalComponent, DialogoInformacionComponent]
})
export class RastreoModule { }
