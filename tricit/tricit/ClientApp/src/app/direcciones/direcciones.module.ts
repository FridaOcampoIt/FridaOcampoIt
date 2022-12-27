import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AgmCoreModule } from '@agm/core';

import { MaterialModule } from '../material/material.module';
import { DireccionesRoutingModule } from './direcciones-routing.module';

import { DireccionesComponent } from './direcciones.component';
import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component';
import { OverlayService } from '../Interfaces/Services/overlay.service';

//DataServices
import { DataServices } from '../Interfaces/Services/general.service';

@NgModule({
    declarations: [DireccionesComponent, DialogoAgregarComponent, DialogoEliminarComponent],
    imports: [
        CommonModule,
        MaterialModule,
        DireccionesRoutingModule,
        FormsModule,
        HttpClientModule,
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyAYMnOhMJEe9A8M9vYh7FizJoMzEzMmC90'
        })
    ],
    providers: [DataServices, OverlayService],
    entryComponents: [DialogoAgregarComponent, DialogoEliminarComponent]
})
export class DireccionesModule { }
