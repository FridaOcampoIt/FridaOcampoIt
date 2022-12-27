import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';

import { VisorRoutingModule } from './visor-routing.module';
import { VisorComponent } from './visor.component';
import { DialogoReporteComponent } from './dialogo-reporte/dialogo-reporte.component';
import { OverlayService } from '../Interfaces/Services/overlay.service';

import { DynamicDatabase } from './TreeClasses';

import { SearchPipe } from './pipes/search.pipe';
import { SearchPipeFamily } from './pipes/searchFamily.pipe';
import { SearchPipeUser } from './pipes/searchUser.pipe';


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
  
import { DialogDeleteItem } from '../services-alerts/service-alerts.components';
@NgModule({
    declarations: [
        VisorComponent,
        DialogoReporteComponent,
        SearchPipe,
        SearchPipeFamily,
        SearchPipeUser,
        DialogDeleteItem
    ],
    imports: [
        CommonModule,
        MaterialModule,
        VisorRoutingModule,
		FormsModule,
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
        DialogoReporteComponent,
        DialogDeleteItem
    ],
	providers: [
        DynamicDatabase, 
        OverlayService
    ]
})
export class VisorModule { }
