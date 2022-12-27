import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SolicitudEtiquetasComponent } from './solicitud-etiquetas.component';
import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';

const routes: Routes = [
    { path: 'CatalogoSolicitud', component: SolicitudEtiquetasComponent, canActivate: [GuardsAuth]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SolicitudEtiquetasRoutingModule { }
