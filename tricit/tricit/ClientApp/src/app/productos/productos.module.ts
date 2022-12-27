import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { ProductosRoutingModule } from './productos-routing.module';
import { MaterialModule } from '../material/material.module';
import { ProductosComponent } from './productos.component';
import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import { DialogoImportarComponent } from './dialogo-importar/dialogo-importar.component';
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component';
import { DataServices } from '../Interfaces/Services/general.service';
import { OverlayService } from '../Interfaces/Services/overlay.service';



@NgModule({
    declarations: [ProductosComponent, DialogoAgregarComponent, DialogoImportarComponent,DialogoEliminarComponent],
    imports: [
        CommonModule,
        MaterialModule,
        ProductosRoutingModule,
        FormsModule,
        HttpClientModule
    ],
    providers: [DataServices, OverlayService],
    entryComponents: [DialogoAgregarComponent, DialogoImportarComponent,DialogoEliminarComponent]
})
export class ProductosModule { }
