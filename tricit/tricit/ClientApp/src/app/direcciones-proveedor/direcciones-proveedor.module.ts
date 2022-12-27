import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DireccionesProveedorRoutingModule } from './direcciones-proveedor-routing.module';
import { DireccionesProveedorComponent } from './direcciones-proveedor.component';
import { MaterialModule } from '../material/material.module';
import { DialogoAgregarDireccionComponent } from './dialogo-agregar-direccion/dialogo-agregar-direccion.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AgmCoreModule } from '@agm/core';
import { DialogoEliminarDireccionComponent } from './dialogo-eliminar-direccion/dialogo-eliminar-direccion.component';

@NgModule({
  declarations: [DireccionesProveedorComponent, DialogoAgregarDireccionComponent, DialogoEliminarDireccionComponent],
  imports: [
    DireccionesProveedorRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyAYMnOhMJEe9A8M9vYh7FizJoMzEzMmC90'
    }),
    CommonModule
  ],
  entryComponents: [DialogoAgregarDireccionComponent, DialogoEliminarDireccionComponent]
})
export class DireccionesProveedorModule { }
