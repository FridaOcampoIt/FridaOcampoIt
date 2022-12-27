import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RastreoComponent } from './rastreo.component';

const routes: Routes = [
  { path: '', component: RastreoComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RastreoRoutingModule { }
