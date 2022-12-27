import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';
import { DireccionesProveedorComponent } from './direcciones-proveedor.component';

const routes: Routes = [
    { path: 'CatalogoDirecciones', component: DireccionesProveedorComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DireccionesProveedorRoutingModule { }
