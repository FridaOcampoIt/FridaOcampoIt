import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar, MatDialogRef } from '@angular/material';
import * as CryptoJS from 'crypto-js';

import { DataServices } from '../../Interfaces/Services/general.service';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-contrasena',
  templateUrl: './dialogo-contrasena.component.html',
  styleUrls: ['./dialogo-contrasena.component.less']
})
export class DialogoContrasenaComponent implements OnInit {

  constructor(

    private snack: MatSnackBar,
    private _router: Router,
    private dataService: DataServices,
    private dialogRef: MatDialogRef<DialogoContrasenaComponent>,

  ) { }

  title: string = "Reestablecer Contraseña";

  emailRecovery: string = "";
  recoveryCode: string = "";
  newPassword: string = "";
  rePassword: string = "";


  recoverPwd1: boolean = true;
  recoverPwd2: boolean = false;

  ngOnInit() {
  }

  validateEmail(email) {
    var re = /\S+@\S+\.\S+/;
    return re.test(email);
  }

  close(){
    this.dialogRef.close();
  }

  requestRecover() {
    if (this.emailRecovery !== "" && this.validateEmail(this.emailRecovery)) {
      this.dataService.postData<LoginUserResponse>("User/recoveryPassword", "", { 'email': this.emailRecovery }).subscribe(
        data => {
          if (data.messageEsp != "") {
            event.stopPropagation(); event.preventDefault();
            this._router.navigate(['Login']);
            this.openSnack(data.messageEsp, "Aceptar");
            return false;
          } else {

            this.recoverPwd1 = false;
            this.recoverPwd2 = true;
            this.openSnack("Código de recuperación enviado", "Aceptar");
          }
        },
        error => {
          event.stopPropagation(); event.preventDefault();
          console.log(error);
          this._router.navigate(['Login']);
          this.openSnack("Error al mandar la solicitud", "Aceptar");
          return false;
        }
      );
    } else {
      this.openSnack("Dirección inválida", "Aceptar");
    }
  }

  resetPassword() {
    if (this.recoveryCode !== "" && this.newPassword !== "" && this.rePassword !== "") {
      if ((this.newPassword === this.rePassword)) {

        var request = { 'email': this.emailRecovery, 'recoveryCode': this.recoveryCode, 'password': CryptoJS.SHA256(this.newPassword).toString() };
        this.recoveryCode = "";
        this.emailRecovery = "";
        this.newPassword = "";
        this.rePassword = "";
        this.dataService.postData<LoginUserResponse>("User/restorePassword", "", request).subscribe(
          data => {
            if (data.messageEsp != "") {
              event.stopPropagation(); event.preventDefault();
              this._router.navigate(['Login']);
              this.openSnack(data.messageEsp, "Aceptar");
              return false;
            } else {
              
              this.recoverPwd1 = false;
              this.recoverPwd2 = false;
              this.openSnack("Contraseña reestablecida exitosamente", "Aceptar");
              this.dialogRef.close(true);
            }
          },
          error => {
            event.stopPropagation(); event.preventDefault();
            console.log(error);
            this._router.navigate(['Login']);
            this.openSnack("Error al mandar la solicitud", "Aceptar");
            return false;
          }
        );
      } else {
        this.openSnack("Confirmación de contraseña no coincide", "Aceptar");
      }
    } else {
      this.openSnack("Todos los campos son obligatorios", "Aceptar");
    }
  }

  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }

}
