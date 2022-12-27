import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { MaterialModule } from '../material/material.module';
import { SlickCarouselModule } from 'ngx-slick-carousel';

import { SwiperModule } from 'ngx-swiper-wrapper';
import { SWIPER_CONFIG } from 'ngx-swiper-wrapper';
import { SwiperConfigInterface } from 'ngx-swiper-wrapper';

import { FamiliasComponent } from './familias.component';
import { FamiliasRoutingModule } from './familias-routing.module';
import { AgregarFamiliaComponent } from './agregar-familia/agregar-familia.component';
import { EspecificacionesComponent } from './agregar-familia/especificaciones/especificaciones.component';
import { GuiasUsoComponent } from './agregar-familia/guias-uso/guias-uso.component';
import { GuiasInstalacionComponent } from './agregar-familia/guias-instalacion/guias-instalacion.component';
import { GarantiaServicioComponent } from './agregar-familia/garantia-servicio/garantia-servicio.component';
import { ProductosRelacionadosComponent } from './agregar-familia/productos-relacionados/productos-relacionados.component';
import { DialogoAgregarComponent } from './agregar-familia/dialogo-agregar/dialogo-agregar.component';
import { DialogoAgregarLinksComponent } from './agregar-familia/dialogo-agregar-links/dialogo-agregar-links.component';
import { DialogoPreguntasComponent } from './agregar-familia/dialogo-preguntas/dialogo-preguntas.component';
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component'

//DataServices
import { DataServices, ServicesComponent } from '../Interfaces/Services/general.service';

import { SwiperVideosGUComponent } from './agregar-familia/guias-uso/swiper-videos/swiper-videos.component';
import { SwiperLinksGUComponent } from './agregar-familia/guias-uso/swiper-links/swiper-links.component';
import { SwiperDocsGUComponent } from './agregar-familia/guias-uso/swiper-docs/swiper-docs.component';

import { SwiperVideosGIComponent } from './agregar-familia/guias-instalacion/swiper-videos/swiper-videos.component';
import { SwiperLinksGIComponent } from './agregar-familia/guias-instalacion/swiper-links/swiper-links.component';
import { SwiperDocsGIComponent } from './agregar-familia/guias-instalacion/swiper-docs/swiper-docs.component';

import { SwiperLinksPRComponent } from './agregar-familia/productos-relacionados/swiper-links/swiper-links.component';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { ConfiguracionEmbalajeComponent } from './agregar-familia/configuracion-embalaje/configuracion-embalaje.component';
import { DialogoAgregarConfiguracionComponent } from './agregar-familia/configuracion-embalaje/dialogo-agregar/dialogo-agregar.component';
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { DialogoEliminarConfiguracionComponent } from './agregar-familia/configuracion-embalaje/dialogo-eliminar/dialogo-eliminar.component';
import { ConfiguracionEmbalajeReprocesoComponent } from './agregar-familia/configuracion-embalaje-reproceso/configuracion-embalaje-reproceso.component';
import { DialogoAgregarConfiguracionReprocesoComponent } from './agregar-familia/configuracion-embalaje-reproceso/dialogo-agregar-reproceso/dialogo-agregar-reproceso.component';
import { DialogoEliminarConfiguracionReprocesoComponent } from './agregar-familia/configuracion-embalaje-reproceso/dialogo-eliminar-reproceso/dialogo-eliminar-reproceso.component';

import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { SearchPipe } from './pipes/search.pipe';
export const DEFAULTCONF: SwiperConfigInterface = {
    direction: 'horizontal',
    observer: true,
    spaceBetween: 5,
    slidesPerView: 4,
    centeredSlides: false,
    navigation: true,
    init: false,
    allowTouchMove: false,
    breakpoints: {
        // when window width is <= 320px
        576: {
            slidesPerView: 1,
            spaceBetween: 10
        },
        // when window width is <= 480px
        763: {
            slidesPerView: 2,
            spaceBetween: 20
        },
        // when window width is <= 640px
        994: {
            slidesPerView: 3,
            spaceBetween: 30
        }
    }
}

import { MAT_COLOR_FORMATS, NgxMatColorPickerModule, NGX_MAT_COLOR_FORMATS } from '@angular-material-components/color-picker';

@NgModule({
    declarations: [
        FamiliasComponent,
        AgregarFamiliaComponent,
        EspecificacionesComponent,
        GuiasUsoComponent,
        GuiasInstalacionComponent,
        GarantiaServicioComponent,
        ProductosRelacionadosComponent,
        DialogoAgregarComponent,
        DialogoAgregarLinksComponent,
        DialogoPreguntasComponent,
        DialogoEliminarComponent,
        SwiperVideosGUComponent,
        SwiperLinksGUComponent,
        SwiperDocsGUComponent,
        SwiperVideosGIComponent,
        SwiperLinksGIComponent,
        SwiperDocsGIComponent,
        SwiperLinksPRComponent,
        ConfiguracionEmbalajeComponent,
        DialogoAgregarConfiguracionComponent,
        DialogoEliminarConfiguracionComponent,
        DialogoEliminarConfiguracionReprocesoComponent,
        ConfiguracionEmbalajeReprocesoComponent,
        DialogoAgregarConfiguracionReprocesoComponent,
        DialogoEliminarConfiguracionReprocesoComponent,
        SearchPipe
    ],
    imports: [
        SwiperModule,
        MaterialModule,
        CommonModule,
        FamiliasRoutingModule,
        FormsModule,
        HttpClientModule,
        FontAwesomeModule,
        ReactiveFormsModule,
        SlickCarouselModule,
        MatSelectModule,
        MatFormFieldModule,
        MatInputModule,
        NgxMatColorPickerModule
    ],
	providers: [DataServices, { provide: SWIPER_CONFIG, useValue: DEFAULTCONF }, ServicesComponent, OverlayService,  { provide: MAT_COLOR_FORMATS, useValue: NGX_MAT_COLOR_FORMATS }],
    entryComponents: [
        DialogoEliminarComponent,
        DialogoAgregarComponent,
        DialogoAgregarLinksComponent,
        DialogoPreguntasComponent,
        DialogoAgregarConfiguracionComponent,
        DialogoEliminarConfiguracionComponent,
        DialogoAgregarConfiguracionReprocesoComponent,
        DialogoEliminarConfiguracionReprocesoComponent
    ]
})
export class FamiliasModule { }
