import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';

import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import { UsuariosComponent } from './usuarios.component';
import { UsuariosRoutingModule } from './usuarios-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { DataServices } from '../Interfaces/Services/general.service';
import { DialogoEliminarComponent } from '../usuarios/dialogo-eliminar/dialogo-eliminar.component';
import { OverlayService } from '../Interfaces/Services/overlay.service';

@NgModule({
  declarations: [DialogoAgregarComponent, UsuariosComponent, DialogoEliminarComponent],
  imports: [
      CommonModule,
      MaterialModule,
      UsuariosRoutingModule,
      FormsModule,
      HttpClientModule
    ],
	providers: [DataServices, OverlayService],
    entryComponents: [DialogoAgregarComponent, DialogoEliminarComponent]
})
export class UsuariosModule { }
