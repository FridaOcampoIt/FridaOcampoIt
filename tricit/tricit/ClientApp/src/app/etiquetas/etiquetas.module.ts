import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EtiquetasRoutingModule } from './etiquetas-routing.module';
import { AgregarEtiquetaComponent } from './agregar-etiqueta/agregar-etiqueta.component';
import { EtiquetasComponent } from './etiquetas.component';
import { MaterialModule } from '../material/material.module';
import { DialogoEliminarEtiquetaComponent } from './dialogo-eliminar-etiqueta/dialogo-eliminar-etiqueta.component';
import { FormsModule } from '@angular/forms';
import { QRCodeModule } from 'angularx-qrcode';

@NgModule({
  declarations: [EtiquetasComponent, AgregarEtiquetaComponent, DialogoEliminarEtiquetaComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    QRCodeModule,
    EtiquetasRoutingModule
  ],
  entryComponents: [DialogoEliminarEtiquetaComponent]
})
export class EtiquetasModule { }
