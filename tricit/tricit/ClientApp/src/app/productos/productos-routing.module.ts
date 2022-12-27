import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductosComponent } from './productos.component';
import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';

const routes: Routes = [
  { path: 'CatalogoProductos', component: ProductosComponent, canActivate: [GuardsAuth] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductosRoutingModule { }
