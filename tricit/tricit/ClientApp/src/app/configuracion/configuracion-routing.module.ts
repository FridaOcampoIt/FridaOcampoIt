import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ConfiguracionComponent } from './configuracion.component';
import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';
import { CategoriaComponent } from './sectores/categoria/categoria.component';
import { FormulariosComponent } from './sectores/formularios/formularios.component';
import { SectoresComponent } from './sectores/sectores.component';


const routes: Routes = [
  { path: 'ConfiguracionGeneral',  component: ConfiguracionComponent, canActivate: [GuardsAuth],
    children:[
    {
      path: 'Sector',
      component: SectoresComponent,data: { breadcrumb: 'Sector'},
    },
    {
      path: 'Sector/:id/Formulario',
     component: FormulariosComponent,data: { breadcrumb: 'Sectores / Formularios'},
    },
    {
      path: 'Sector/:id/Formulario/:id/Categoria/:nombre',
     component: CategoriaComponent,data: { breadcrumb: ' Sectores / Formularios /  '}
    },
    
    ] 
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfiguracionRoutingModule { }
