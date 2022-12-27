import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmpacadoEtiquetadoRoutingModule } from './empacado-etiquetado-routing.module';
import { MaterialModule } from '../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { EmpacadoEtiquetadoComponent } from './empacado-etiquetado.component';
import { CatalogoProveedoresComponent } from './catalogo-proveedores/catalogo-proveedores.component';
import { DialogoAgregarProveedorComponent } from './catalogo-proveedores/dialogo-agregar-proveedor/dialogo-agregar-proveedor.component';
import { DialogoEliminarComponent } from './catalogo-proveedores/dialogo-eliminar/dialogo-eliminar.component';
import { CatalogoDistribuidoresComponent } from './catalogo-distribuidores/catalogo-distribuidores.component';
import { DialogoAgregarDistribuidorComponent } from './catalogo-distribuidores/dialogo-agregar-distribuidor/dialogo-agregar-distribuidor.component';
import { DialogoEliminarDistribuidorComponent } from './catalogo-distribuidores/dialogo-eliminar-distribuidor/dialogo-eliminar-distribuidor.component';
import { CatalogoEmpacadoExternoComponent } from './catalogo-empacado-externo/catalogo-empacado-externo.component';
import { CatalogoEmpacadoInternoComponent } from './catalogo-empacado-interno/catalogo-empacado-interno.component';
import { CatalogoEmpacadoAgroComponent } from './catalogo-empacado-agro/catalogo-empacado-agro.component';
import { DialogoAgregarEmpacadoComponent as DialogoAgregarEmpacadoInterno } from './catalogo-empacado-interno/dialogo-agregar-empacado/dialogo-agregar-empacado.component';
import { DialogoEliminarEmpacadoExternoComponent } from './catalogo-empacado-externo/dialogo-eliminar-empacado-externo/dialogo-eliminar-empacado-externo.component';
import { DialogoEliminarEmpacadoInternoComponent } from './catalogo-empacado-interno/dialogo-eliminar-empacado-interno/dialogo-eliminar-empacado-interno.component';
import { DialogoAgregarEmpacadoComponent as DialogoAgregarEmpacadoExterno } from './catalogo-empacado-externo/dialogo-agregar-empacado/dialogo-agregar-empacado.component';
import { CatalogoOperadoresComponent } from './catalogo-operadores/catalogo-operadores.component';
import { DialogoAgregarOperadorComponent } from './catalogo-operadores/dialogo-agregar-operador/dialogo-agregar-operador.component';
import { DialogoEliminarOperadorComponent } from './catalogo-operadores/dialogo-eliminar-operador/dialogo-eliminar-operador.component';
import { CatalogoLineasProduccionComponent } from './catalogo-lineas-produccion/catalogo-lineas-produccion.component';
import { DialogoAgregarLineaComponent } from './catalogo-lineas-produccion/dialogo-agregar-linea/dialogo-agregar-linea.component';
import { DialogoEliminarLineaComponent } from './catalogo-lineas-produccion/dialogo-eliminar-linea/dialogo-eliminar-linea.component';
import { DialogoAsociarProductosComponent } from './dialogo-asociar-productos/dialogo-asociar-productos.component';
import { GestionCajasComponent } from './gestion-cajas/gestion-cajas.component';
import { DialogoDetalleComponent } from './gestion-cajas/dialogo-detalle/dialogo-detalle.component';
import { ReporteArmadosComponent } from './reporte-armados/reporte-armados.component';
import { ChartsModule } from 'ng2-charts';
import { DialogoUnirComponent } from './gestion-cajas/dialogo-unir/dialogo-unir.component';
import { ReporteEmbalajeReprocesoComponent } from './reporte-embalaje-reproceso/reporte-embalaje-reproceso.component';
import { DialogoPalletsComponent } from './gestion-cajas/dialogo-pallets/dialogo-pallets.component';
// import { DialogoDireccionEmpacadoComponent} from './catalogo-empacado-externo/dialogo-direccion-empacado/dialogo-direccion-empacado.component';



import { SearchPipe } from './pipes/search.pipe';

import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatStepperModule
} from '@angular/material';

import { AgmCoreModule } from '@agm/core';
//Importamos los catalogos
import { CatalogoAcopiosComponent } from './catalogo-acopios/catalogo-acopios.component';
import { CatalogoProductoresComponent} from './catalogo-acopios/catalogo-productores/catalogo-productores.component';
import { CatalogoActividadesComponent } from './catalogo-acopios/catalogo-actividades/catalogo-actividades.component';
//Importamos los dialogos
import { DialogoAgregarEditarAcopioComponent } from './catalogo-acopios/dialogo-agregar-editar-acopio/dialogo-agregar-editar-acopio.component';
import { DialogoAgregarEditarProductorComponent } from './catalogo-acopios/catalogo-productores/dialogo-agregar-editar-productor/dialogo-agregar-editar-productor.component';
import { DialogoEliminarAcopioComponent } from './catalogo-acopios/dialogo-eliminar-acopio/dialogo-eliminar-acopio.component';
import { DialogoDireccionEmpacadoComponent } from './dialogo-direccion-empacado/dialogo-direccion-empacado.component';
import { DialogoEliminarProductorComponent } from './catalogo-acopios/catalogo-productores/dialogo-eliminar-productor/dialogo-eliminar-productor.component';



@NgModule({
  declarations: [
    EmpacadoEtiquetadoComponent,
    CatalogoProveedoresComponent,
    DialogoAgregarProveedorComponent,
    DialogoEliminarComponent,
    CatalogoDistribuidoresComponent,
    DialogoAgregarDistribuidorComponent,
    DialogoEliminarDistribuidorComponent,
    CatalogoEmpacadoExternoComponent,
    CatalogoEmpacadoInternoComponent,
    CatalogoEmpacadoAgroComponent,
    DialogoAgregarEmpacadoExterno,
    DialogoAgregarEmpacadoInterno,
    DialogoEliminarEmpacadoExternoComponent,
    DialogoEliminarEmpacadoInternoComponent,
    CatalogoOperadoresComponent,
    DialogoAgregarOperadorComponent,
    DialogoEliminarOperadorComponent,
    CatalogoLineasProduccionComponent,
    DialogoAgregarLineaComponent,
    DialogoEliminarLineaComponent,
    DialogoAsociarProductosComponent,
    GestionCajasComponent,
    DialogoDetalleComponent,
    ReporteArmadosComponent,
    ReporteEmbalajeReprocesoComponent,
    DialogoUnirComponent,
    DialogoPalletsComponent,
    CatalogoAcopiosComponent,
    CatalogoProductoresComponent,
    CatalogoActividadesComponent,
    DialogoAgregarEditarAcopioComponent,
    DialogoAgregarEditarProductorComponent,
    DialogoEliminarAcopioComponent,
    DialogoEliminarProductorComponent,
    DialogoDireccionEmpacadoComponent,
    SearchPipe,
  

    
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    ChartsModule,
    EmpacadoEtiquetadoRoutingModule,
    MatInputModule,
    AgmCoreModule.forRoot({
        apiKey: 'AIzaSyAYMnOhMJEe9A8M9vYh7FizJoMzEzMmC90'
    }),
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatStepperModule
  ],
  entryComponents: [
    DialogoAgregarProveedorComponent,
    DialogoEliminarComponent,
    DialogoAgregarDistribuidorComponent,
    DialogoEliminarDistribuidorComponent,
    DialogoAgregarEmpacadoInterno,
    DialogoAgregarEmpacadoExterno,
    DialogoEliminarEmpacadoExternoComponent,
    DialogoEliminarEmpacadoInternoComponent,
    DialogoAgregarOperadorComponent,
    DialogoEliminarOperadorComponent,
    DialogoAgregarLineaComponent,
    DialogoEliminarLineaComponent,
    DialogoAsociarProductosComponent,
    DialogoDetalleComponent,
    DialogoUnirComponent,
    DialogoPalletsComponent,
    DialogoAgregarEditarAcopioComponent,
    DialogoAgregarEditarProductorComponent,
    DialogoEliminarAcopioComponent,
    DialogoEliminarProductorComponent,
    DialogoDireccionEmpacadoComponent,
    
  ]
})
export class EmpacadoEtiquetadoModule { }
