import { Component, OnInit, AfterViewInit, ViewChild, Inject } from '@angular/core';
import { MatTableDataSource, MatPaginator, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';
import { FormBuilder, FormGroup } from '@angular/forms';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { DataServices } from '../../Interfaces/Services/general.service';
import { Router } from '@angular/router';
import { OverlayRef } from '@angular/cdk/overlay';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-legal',
  templateUrl: './dialogo-legal.component.html',
  styleUrls: ['./dialogo-legal.component.less']
})
export class DialogoLegalComponent implements OnInit, AfterViewInit {


  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ["fechaDoc", "nombredoc"];
  dataSource = new MatTableDataSource<any>();
  itemsPagina: number[] = enviroments.pageSize;

  overlayRef: OverlayRef;
  infoForm: FormGroup;

  constructor(
    private _fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router
  ) { }

  ngOnInit() {

    this.infoForm = this._fb.group({
      tipo: [1],
      nombreI: [""],
      direccionI: [""],
      contactoI: [""],
      nombreE: [""],
      direccionE: [""],
      contactoE: [""]
    });

    this.obtenerDatos();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  obtenerDatos(campo = 6) {
    let data: any = {
      trackingId: this.data["id"],
      opc: campo
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("Tracking/GetTrackingInfo", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {

          let tipo = data["eventLegalInfo"][0]["tipoInfo"];

          this.infoForm.patchValue({
            tipo: data["eventLegalInfo"][0]["tipoInfo"]
          })

          if (tipo == 1) {
            this.infoForm.patchValue({
              nombreI: data["eventLegalInfo"][0]["nombreInfo"],
              direccionI: data["eventLegalInfo"][0]["direccionInfo"],
              contactoI: data["eventLegalInfo"][0]["contactoInfo"],
            })
          } else {
            this.infoForm.patchValue({
              nombreE: data["eventLegalInfo"][0]["nombreInfo"],
              direccionE: data["eventLegalInfo"][0]["direccionInfo"],
              contactoE: data["eventLegalInfo"][0]["contactoInfo"],
            })
          }

          this.dataSource.data = data["eventLegalDocsInfo"];

        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("1");
        } else {
          this.openSnack("Error en la solicitud", "Aceptar");
          console.log(error);
        }
      }
    )
  }

  //Funcion para realizar el proceso del relogin
  relogin(peticion) {
    var requestLogin = new LoginUserRequest();
    requestLogin.user = sessionStorage.getItem("email");
    requestLogin.password = sessionStorage.getItem("password");

    this._dataService.postData<LoginUserResponse>("User/loginUser", "", requestLogin).subscribe(
      data => {
        if (data.messageEsp != "") {
          sessionStorage.clear();
          this._router.navigate(['Login']);
          this.openSnack(data.messageEsp, "Aceptar");
          return;
        }

        sessionStorage.clear();

        data.userData.userPermissions.forEach((it, id) => {
          sessionStorage.setItem(it.namePermission, it.permissionId.toString());
        });

        sessionStorage.setItem("token", data.token);
        sessionStorage.setItem("name", data.userData.userData.name);
        sessionStorage.setItem("idUser", data.userData.userData.idUser.toString());
        sessionStorage.setItem("email", requestLogin.user);
        sessionStorage.setItem("password", requestLogin.password);
        sessionStorage.setItem("company", data.userData.userData.company.toString());
        sessionStorage.setItem("isType", data.userData.userData.isType.toString());
        sessionStorage.setItem("isType", data.userData.userData.isType.toString());

        switch (peticion) {
          case "1":
            this.obtenerDatos();
            break;
          default:
            break;
        }
      },
      error => {
        sessionStorage.clear();
        this._router.navigate(['Login']);
        this.openSnack("Error al mandar la solicitud", "Aceptar");
        return;
      }
    )
  }

  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }

}
