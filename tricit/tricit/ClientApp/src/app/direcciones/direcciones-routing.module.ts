import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DireccionesComponent } from './direcciones.component';
import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';

const routes: Routes = [
    { path: 'CatalogoDirecciones', component: DireccionesComponent, canActivate: [GuardsAuth]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DireccionesRoutingModule { }
