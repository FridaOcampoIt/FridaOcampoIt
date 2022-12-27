import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';

import { SolicitudEtiquetasRoutingModule } from './solicitud-etiquetas-routing.module';
import { SolicitudEtiquetasComponent } from './solicitud-etiquetas.component';
import { DialogoSolicitarComponent } from './dialogo-solicitar/dialogo-solicitar.component';
import { DialogoSeguimientoComponent } from './dialogo-seguimiento/dialogo-seguimiento.component';
import { DialogoHistorialComponent } from './dialogo-historial/dialogo-historial.component';
import { OverlayService } from '../Interfaces/Services/overlay.service';


@NgModule({
    declarations: [SolicitudEtiquetasComponent, DialogoSolicitarComponent, DialogoSeguimientoComponent, DialogoHistorialComponent],
    imports: [
        CommonModule,
        MaterialModule,
        SolicitudEtiquetasRoutingModule,
		FormsModule
	],
	providers: [OverlayService],
    entryComponents: [DialogoSolicitarComponent, DialogoSeguimientoComponent, DialogoHistorialComponent]
})
export class SolicitudEtiquetasModule { }
