import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { MaterialModule } from '../material/material.module';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { ChartsModule } from 'ng2-charts';
import { HttpClientModule } from '@angular/common/http';


import { DataServices } from '../Interfaces/Services/general.service';
@NgModule({
    declarations: [],
    imports: [
        MaterialModule,
        BrowserModule,
        RouterModule,
        CommonModule,
        ChartsModule,
        HttpClientModule
	],
	providers: [OverlayService, DataServices]
})
export class HomeModule { }
