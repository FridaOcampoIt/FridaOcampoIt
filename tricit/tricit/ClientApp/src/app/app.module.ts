import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common/';
import { FormsModule } from '@angular/forms';

//Modulos
import { AppRoungModule } from './app-routing-module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginModule } from './login/login.module';
import { HomeModule } from './home/home.module';
import { FamiliasModule } from './familias/familias.module';
import { UsuariosModule } from './usuarios/usuarios.module';
import { CompaniasModule } from './companias/companias.module';
import { DireccionesModule } from './direcciones/direcciones.module';
import { LoginComponent } from './login/login.component';
import { ProductosModule } from './productos/productos.module';
import { SolicitudEtiquetasModule } from './solicitud-etiquetas/solicitud-etiquetas.module';
import { VisorModule } from './visor/visor.module';
import { ConfiguracionModule } from './configuracion/configuracion.module';
import { PerfilesModule } from './perfiles/perfiles.module';

//Material
import { MaterialModule } from './material/material.module';
import { getSpanishPaginatorIntl } from './material/spanis-paginator-intl';

//Componentes
import { AppComponent } from './app.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { HomeComponent } from './home/home.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { SidenavMenuComponent } from './sidenav/sidenav-menu/sidenav-menu.component';
import { SidenavLinkComponent } from './sidenav/sidenav-link/sidenav-link.component';
import { MatPaginatorIntl, MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material';
import { OverlayCargaComponent } from './overlay-carga/overlay-carga.component';
import { OriginCiuComponent } from './origin-ciu/origin-ciu.component';
import { SafePipe, TrackingComponent } from './tracking/tracking.component';
import { DialogoNotificacionesComponent } from './tracking/dialogo-notificaciones/dialogo-notificaciones.component';
import { DialogoLectorComponent } from './rastreo/dialogo-lector/dialogo-lector.component';
import { ZXingScannerModule } from '@zxing/ngx-scanner';


@NgModule({
    declarations: [
        AppComponent,
        NotFoundComponent,
        HomeComponent,
        SidenavComponent,
        SidenavMenuComponent,
        SidenavLinkComponent,
        LoginComponent,
        OverlayCargaComponent,
        OriginCiuComponent,
        TrackingComponent,
        SafePipe,
        DialogoNotificacionesComponent,
        DialogoLectorComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        CommonModule,
        LoginModule,
        HomeModule,
        FormsModule,
        MaterialModule,
        AppRoungModule,
        ZXingScannerModule
    ],
    providers: [
        { provide: MatPaginatorIntl, useValue: getSpanishPaginatorIntl() },
        { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { panelClass:['primary-snack'], verticalPosition: 'top'} }
    ],
    entryComponents: [OverlayCargaComponent, DialogoNotificacionesComponent, DialogoLectorComponent],
    bootstrap: [AppComponent]
})
export class AppModule { }
