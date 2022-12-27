import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { MaterialModule } from '../material/material.module';
import { CompaniasRoutingModule } from './companias-routing.module';

import { CompaniasComponent } from './companias.component';
import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component';
import { OverlayService } from '../Interfaces/Services/overlay.service';

//DataServices
import { DataServices } from '../Interfaces/Services/general.service';

@NgModule({
    declarations: [CompaniasComponent, DialogoAgregarComponent, DialogoEliminarComponent],
    imports: [
        CommonModule,
        MaterialModule,
        CompaniasRoutingModule,
        FormsModule,
        HttpClientModule
    ],
	providers: [DataServices, OverlayService],
	entryComponents: [DialogoAgregarComponent, DialogoEliminarComponent]
})
export class CompaniasModule { }
