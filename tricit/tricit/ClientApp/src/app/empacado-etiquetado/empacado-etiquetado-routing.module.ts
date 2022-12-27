import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmpacadoEtiquetadoComponent } from './empacado-etiquetado.component';
import { CatalogoProveedoresComponent } from './catalogo-proveedores/catalogo-proveedores.component';
import { CatalogoDistribuidoresComponent } from './catalogo-distribuidores/catalogo-distribuidores.component';
import { CatalogoEmpacadoInternoComponent } from './catalogo-empacado-interno/catalogo-empacado-interno.component';
import { CatalogoEmpacadoExternoComponent } from './catalogo-empacado-externo/catalogo-empacado-externo.component';
import { CatalogoEmpacadoAgroComponent } from './catalogo-empacado-agro/catalogo-empacado-agro.component';
import { CatalogoOperadoresComponent } from './catalogo-operadores/catalogo-operadores.component';
import { CatalogoLineasProduccionComponent } from './catalogo-lineas-produccion/catalogo-lineas-produccion.component';
import { GestionCajasComponent } from './gestion-cajas/gestion-cajas.component';
import { ReporteArmadosComponent } from './reporte-armados/reporte-armados.component';
import { ReporteEmbalajeReprocesoComponent } from './reporte-embalaje-reproceso/reporte-embalaje-reproceso.component';



//Acopios
import { CatalogoAcopiosComponent } from './catalogo-acopios/catalogo-acopios.component';
import { CatalogoProductoresComponent } from './catalogo-acopios/catalogo-productores/catalogo-productores.component';
import { CatalogoActividadesComponent } from './catalogo-acopios/catalogo-actividades/catalogo-actividades.component';
const routes: Routes = [
  { path: '', component: EmpacadoEtiquetadoComponent },
  { path: 'Proveedores', component: CatalogoProveedoresComponent },
  { path: 'Distribuidores', component: CatalogoDistribuidoresComponent },
  { path: 'EmpacadoInterno', component: CatalogoEmpacadoInternoComponent },
  { path: 'EmpacadoExterno', component: CatalogoEmpacadoExternoComponent },
  { path: 'EmpacadoAgro', component: CatalogoEmpacadoAgroComponent },
  { path: 'Acopios', component: CatalogoAcopiosComponent },
  { path: 'Acopios/GestionDeProductores', component: CatalogoProductoresComponent },
//{ path: 'Acopios/:acopio', component: CatalogoProductoresComponent },
  { path: 'Acopios/:acopioName/:id', component: CatalogoActividadesComponent},
  { path: 'EmpacadoExterno/Operadores/:proviene/:id/:name', component: CatalogoOperadoresComponent },
  { path: 'EmpacadoInterno/Operadores/:proviene/:id/:name', component: CatalogoOperadoresComponent },
  { path: 'EmpacadoInterno/LineasOperacion/:proviene/:id/:name', component: CatalogoLineasProduccionComponent },
  { path: 'EmpacadoExterno/LineasOperacion/:proviene/:id/:name', component: CatalogoLineasProduccionComponent },
  { path: 'EmpacadoExterno/GestionCajas/:proviene/:id/:name', component: GestionCajasComponent },
  { path: 'EmpacadoInterno/GestionCajas/:proviene/:id/:name', component: GestionCajasComponent },
  { path: 'EmpacadoExterno/ReporteArmados/:proviene/:id/:name', component: ReporteArmadosComponent },
  { path: 'EmpacadoInterno/ReporteArmados/:proviene/:id/:name', component: ReporteArmadosComponent },
  { path: 'EmpacadoAgro/ReporteEmbalajeReproceso/:id/:number/:name', component: ReporteEmbalajeReprocesoComponent },

  
    
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmpacadoEtiquetadoRoutingModule { }
