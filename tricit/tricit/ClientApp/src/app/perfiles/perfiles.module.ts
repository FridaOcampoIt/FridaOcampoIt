import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MaterialModule } from '../material/material.module';
import { HttpClientModule } from '@angular/common/http';

import { PerfilesRoutingModule } from './perfiles-routing.module';
import { PerfilesComponent } from './perfiles.component';
import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component'
import { DataServices } from '../Interfaces/Services/general.service';
import { OverlayService } from '../Interfaces/Services/overlay.service';




@NgModule({
    declarations: [PerfilesComponent, DialogoAgregarComponent, DialogoEliminarComponent],
    imports: [
        CommonModule,
        MaterialModule,
        PerfilesRoutingModule,
        FormsModule,        
        HttpClientModule
    ],
    providers: [DataServices, OverlayService],
    entryComponents: [DialogoAgregarComponent, DialogoEliminarComponent]
})
export class PerfilesModule { }
