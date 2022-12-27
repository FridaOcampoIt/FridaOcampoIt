import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { GuardsLogin } from './Interfaces/Guards/GuardsLogin.guard';

import { DataServices } from './Interfaces/Services/general.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { OriginCiuComponent } from './origin-ciu/origin-ciu.component';
import { TrackingComponent } from './tracking/tracking.component';

const appRoutes: Routes = [
    { path: 'Login', component: LoginComponent, canActivate: [GuardsLogin] },
    { path: 'Home', component: HomeComponent, canActivate: [GuardsLogin] },
    { path: 'Perfiles', loadChildren: () => import('./perfiles/perfiles.module').then(mod => mod.PerfilesModule) },
    { path: 'Usuarios', loadChildren: () => import('./usuarios/usuarios.module').then(mod => mod.UsuariosModule), canActivate: [GuardsLogin] },
    { path: 'Companias', loadChildren: () => import('./companias/companias.module').then(mod => mod.CompaniasModule), canActivate: [GuardsLogin] },
    { path: 'Familias', loadChildren: () => import('./familias/familias.module').then(mod => mod.FamiliasModule), canActivate: [GuardsLogin] },
    { path: 'Direcciones', loadChildren: () => import('./direcciones/direcciones.module').then(mod => mod.DireccionesModule), canActivate: [GuardsLogin] },
    { path: 'Productos', loadChildren: () => import('./productos/productos.module').then(mod => mod.ProductosModule), canActivate: [GuardsLogin] },
    { path: 'Solicitud', loadChildren: () => import('./solicitud-etiquetas/solicitud-etiquetas.module').then(mod => mod.SolicitudEtiquetasModule), canActivate: [GuardsLogin] },
    { path: 'Visor', loadChildren: () => import('./visor/visor.module').then(mod => mod.VisorModule), canActivate: [GuardsLogin] },
    { path: 'Configuracion', loadChildren: () => import('./configuracion/configuracion.module').then(mod => mod.ConfiguracionModule), canActivate: [GuardsLogin] },
    { path: 'Rastreo', loadChildren: () => import('./rastreo/rastreo.module').then(mod => mod.RastreoModule) },
    { path: 'EmpacadoEtiquetado', loadChildren: () => import('./empacado-etiquetado/empacado-etiquetado.module').then(mod => mod.EmpacadoEtiquetadoModule) },
    { path: 'DireccionesProveedores', loadChildren: () => import('./direcciones-proveedor/direcciones-proveedor.module').then(mod => mod.DireccionesProveedorModule) },
    { path: 'Etiquetas', loadChildren: () => import('./etiquetas/etiquetas.module').then(mod => mod.EtiquetasModule) },
    { path: '', redirectTo: '/Login', pathMatch: 'full' },
    { path: '**', component: NotFoundComponent },
    { path: 'origin', component: OriginCiuComponent },
    { path: 'origin/:ciu', component: OriginCiuComponent },
    { path: 'tracking', component: TrackingComponent},
    { path: 'tracking/:qr', component: TrackingComponent}
];

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forRoot(appRoutes, { enableTracing: false }), // <-- debugging purposes only
        FormsModule,
        HttpClientModule
    ],
    providers: [DataServices],
    exports: [RouterModule]
})

export class AppRoungModule { }