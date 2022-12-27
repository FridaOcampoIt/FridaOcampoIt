import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EtiquetasComponent } from './etiquetas.component';
import { AgregarEtiquetaComponent } from './agregar-etiqueta/agregar-etiqueta.component';

const routes: Routes = [
  { path: "", component: EtiquetasComponent },
  { path: "AgregarFormato", component: AgregarEtiquetaComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EtiquetasRoutingModule { }
