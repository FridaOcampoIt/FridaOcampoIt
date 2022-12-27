import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VisorComponent } from './visor.component';
import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';

const routes: Routes = [
    { path: 'CatalogoVisor', component: VisorComponent, canActivate: [GuardsAuth]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VisorRoutingModule { }
