import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from '../material/material.module';

import { ConfiguracionRoutingModule } from './configuracion-routing.module';
import { ConfiguracionComponent } from './configuracion.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { DataServices } from '../Interfaces/Services/general.service';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { SectoresComponent } from './sectores/sectores.component';
import { FormulariosComponent } from './sectores/formularios/formularios.component';
import { DialogEliminarSectorComponent } from './sectores/dialog-eliminar/dialog-eliminar-sector.component';
import { CategoriaComponent } from './sectores/categoria/categoria.component';
import { DialogoPreVistaComponent } from './sectores/dialogo-pre-vista/dialogo-pre-vista.component';
import { DialogPropiedaesInputComponent } from './sectores/dialog-propiedaes-input/dialog-propiedaes-input.component';
import { DialogAgregarComponent } from './sectores/dialog-agregar/dialog-agregar.component';
import { MatSelectModule } from '@angular/material';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import {BreadcrumbModule} from 'angular-crumbs';
import { DialogAsignarFormularioComponent } from './sectores/dialog-asignar-formulario/dialog-asignar-formulario.component';



import { filterpipe } from '../visor/pipes/search.pipe';



@NgModule({
  declarations: [ConfiguracionComponent, 
    SectoresComponent, 
    FormulariosComponent, 
    DialogEliminarSectorComponent, 
    CategoriaComponent, 
    DialogoPreVistaComponent, 
    DialogPropiedaesInputComponent, 
    DialogAgregarComponent, 
    DialogAsignarFormularioComponent,
    filterpipe,
  ],
  imports: [
      CommonModule,
      MaterialModule,
      ConfiguracionRoutingModule,
      FormsModule,
      HttpClientModule,
      ReactiveFormsModule,
      MatSelectModule,
      BreadcrumbModule,
     
      
    ],schemas: [CUSTOM_ELEMENTS_SCHEMA],
	providers: [DataServices, OverlayService],
  entryComponents: [DialogEliminarSectorComponent,DialogoPreVistaComponent,DialogPropiedaesInputComponent,DialogAgregarComponent,DialogAsignarFormularioComponent]
})
export class ConfiguracionModule { }
