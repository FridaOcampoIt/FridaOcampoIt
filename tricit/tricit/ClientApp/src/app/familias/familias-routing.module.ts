import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AgregarFamiliaComponent } from './agregar-familia/agregar-familia.component';
import { FamiliasComponent } from './familias.component';
import { EspecificacionesComponent } from './agregar-familia/especificaciones/especificaciones.component';
import { GuiasUsoComponent } from './agregar-familia/guias-uso/guias-uso.component';
import { GuiasInstalacionComponent } from './agregar-familia/guias-instalacion/guias-instalacion.component';
import { GarantiaServicioComponent } from './agregar-familia/garantia-servicio/garantia-servicio.component';
import { ProductosRelacionadosComponent } from './agregar-familia/productos-relacionados/productos-relacionados.component';
import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';
import { ConfiguracionEmbalajeComponent } from './agregar-familia/configuracion-embalaje/configuracion-embalaje.component';
import { ConfiguracionEmbalajeReprocesoComponent } from './agregar-familia/configuracion-embalaje-reproceso/configuracion-embalaje-reproceso.component';


const familiasRoutes: Routes = [
    {
        path: 'CatalogoFamilias', component: FamiliasComponent, canActivate: [GuardsAuth]
    },
    {
        path: 'CatalogoFamilias/Editar/:id', component: AgregarFamiliaComponent, canActivate: [GuardsAuth]
    },
    {
        path: 'CatalogoFamilias/Agregar', component: AgregarFamiliaComponent, canActivate: [GuardsAuth]
    },
    {
        path: 'CatalogoFamilias/Especificaciones/:id', component: EspecificacionesComponent, canActivate: [GuardsAuth]
    },
    {
        path: 'CatalogoFamilias/GuiasUso/:id', component: GuiasUsoComponent, canActivate: [GuardsAuth]
    },
    {
        path: 'CatalogoFamilias/GuiasInstalacion/:id', component: GuiasInstalacionComponent, canActivate: [GuardsAuth]
    },
    {
        path: 'CatalogoFamilias/GarantiaServicio/:id', component: GarantiaServicioComponent, canActivate: [GuardsAuth]
    },
    {
        path: 'CatalogoFamilias/ProductosRelacionados/:id', component: ProductosRelacionadosComponent, canActivate: [GuardsAuth]
    },
    {
        path: 'CatalogoFamilias/ConfiguracionEmbalaje/:id/:companyId', component: ConfiguracionEmbalajeComponent
    },
    {
        path: 'CatalogoFamilias/ConfiguracionEmbalajeReproceso/:id', component: ConfiguracionEmbalajeReprocesoComponent
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(familiasRoutes)
    ],
    exports: [RouterModule]
})
export class FamiliasRoutingModule { }
