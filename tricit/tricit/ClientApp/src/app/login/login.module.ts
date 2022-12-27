import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

//material 
import { DialogoContrasenaComponent } from './dialogo-contrasena/dialogo-contrasena.component';
import { MaterialModule } from '../material/material.module';

@NgModule({
  declarations: [DialogoContrasenaComponent],
  imports: [
      CommonModule,
      MaterialModule,
      BrowserModule,
      FormsModule
  ],
  entryComponents: [DialogoContrasenaComponent]
})
export class LoginModule { }
