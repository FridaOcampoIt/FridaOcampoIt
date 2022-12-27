import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PerfilesComponent } from './perfiles.component';
import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';

const routes: Routes = [
  { path: 'CatalogoPerfiles', component: PerfilesComponent, canActivate: [GuardsAuth] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PerfilesRoutingModule { }
