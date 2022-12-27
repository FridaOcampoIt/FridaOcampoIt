import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CompaniasComponent } from './companias.component';
import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';

const routes: Routes = [
    { path: 'CatalogoCompanias', component: CompaniasComponent, canActivate: [GuardsAuth]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompaniasRoutingModule { }
