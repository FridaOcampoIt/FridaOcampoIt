import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { enviroments } from '../../../Interfaces/Enviroments/enviroments';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { OverlayRef } from '@angular/cdk/overlay';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';
import * as CryptoJS from 'crypto-js';


//DataServices and Models
import {
  SearchCompanyRequest,
  SearchCompanyResponse,
  CompaniesData
} from '../../../Interfaces/Models/CompanyModels';
@Component({
  selector: 'app-dialogo-agregar-empacado',
  templateUrl: './dialogo-agregar-empacado.component.html',
  styleUrls: ['./dialogo-agregar-empacado.component.less']
})
export class DialogoAgregarEmpacadoComponent implements OnInit {

  eEForm: FormGroup;
  regExMail: RegExp = RegExp(enviroments.patterns.email);
  regExNum: RegExp = RegExp(enviroments.patterns.numerical);

  titulo: string = "";

  overlayRef: OverlayRef;

  constructor(
    private _fb: FormBuilder,
    private dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dialogRef: MatDialogRef<DialogoAgregarEmpacadoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _router: Router
  ) { }

  formGetter = (_campo) => {
    return this.eEForm.get(_campo);
  }

  tipoUsuario: Number = 0;
  company : number = 0;
  isType: number = 0;
  async ngOnInit() {
    this.eEForm = this._fb.group({
      packedId: [0],
      status: [true],
      packedNumber: ["", [Validators.required]],
      packedName: ["", [Validators.required]],
      email: ["", [Validators.required, Validators.pattern(this.regExMail)]],
      password: ["", [Validators.required]],
      phone: ["", [Validators.pattern(this.regExNum)]],
      merma: [0,],
      companiasId: [[], [Validators.required, Validators.minLength(1)]],
      auxGetCompany: []
    })


    
    this.company = parseInt(sessionStorage.getItem("company"));
    this.isType = parseInt (sessionStorage.getItem("isType"));
    console.log("Company", this.company);
    await this.eEForm.patchValue({ estatus: true })
    this.titulo = await this.data["titulo"];
    this.tipoUsuario = await this.data["tipoUsuario"];

    this.titulo = this.data["titulo"];

    if (this.data["action"] != 0) {
      await this.obtenerDatos();
      this.eEForm.get("password").clearValidators();
      this.eEForm.get("password").updateValueAndValidity();
    }else if(this.data['tipoUsuario'] == 0){
      await this.getCompaniasForEmpacador();
    }
  }
   //Variables para la vista
   name: string = "";
   businessName: string = "";
   idCompany: number = 0;
   response = new SearchCompanyResponse();
   itemsPagina: number[] = enviroments.pageSize;
   emptyList: boolean = false;
   dataSource : Array<CompaniesData>; 
 
   /*
   * Funcion para obtener listado de compañias
   *  Autor: Hernán Gómez 
   * 10-Marzo-2022
   */
   async getCompaniasForEmpacador() {
     var request = new SearchCompanyRequest();
     request.name = this.name;
     request.businessName = this.businessName;
     this.idCompany = 0;
 
     setTimeout(() => {
      this.overlayRef = this._overlay.open();
     }, 1);
 
     await this.dataService.postData<SearchCompanyResponse>
     ("Companies/searchCompany", sessionStorage.getItem("token"), request).subscribe(
       data => {
         this.response = data;
         if(this.response.companiesDataList.length > 0)
           this.emptyList = false;
         else
           this.emptyList = true;
         this.dataSource = this.response.companiesDataList.filter(x => x.name != "");
         setTimeout(() => {
           this._overlay.close(this.overlayRef);
         }, 1);
       },
       error => {
         console.log(error);
         setTimeout(() => {
           this._overlay.close(this.overlayRef);
         }, 1);
       }
     );
   }

  tempStatus: boolean = false;
  tempNInterno: Number = 0;
  tempName: String = '';
  tempCorreo: String = '';
  async obtenerDatos() {
    //Creamos una variable temporal para el bloqueo de pantalla, porque typeScript YOLO, no funciona.
    let auxLoading: any;
    setTimeout(() => {
      auxLoading = this._overlay.open();
     }, 1);
    if(this.data['tipoUsuario'] == 0){
      await this.getCompaniasForEmpacador();
    }
    let data: any = {
      packedId: this.data['packedId'],
      companyId: this.data['tipoUsuario']
    };

    this.dataService.postData<any>("ExternalPacked/SearchPackedData", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log("data", data);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.eEForm.patchValue(data["packedList"][0]);
          if(this.data['tipoUsuario'] == 0){
            //Array en string
            let arraytemporal = data["packedList"][0].getCompaniasId.split(',');
            //Convertimos el string a enteros (porque yolo, no funciona con string)
            let arrayNumeros = arraytemporal.map(str => {
              return Number(str);
            });
            this.eEForm.controls['companiasId'].setValue(arrayNumeros);
            //Si recuperamos la información de un registro guardamos los id's que estaban en el arreglo
            this.eEForm.controls['auxGetCompany'].setValue(arrayNumeros);
          }else if( this.data['tipoUsuario'] != 0){ //Evaluamos que sea una compania para mosrar solo los datos
            this.tempStatus =  data["packedList"][0].status;
            this.tempNInterno = data["packedList"][0].packedNumber;
            this.tempName = data["packedList"][0].packedName;
            this.tempCorreo = data["packedList"][0].email;
          }
          console.error(data["packedList"][0]);
          setTimeout(() => {
            this._overlay.close(auxLoading);
          },1);
        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("1")
        } else {
          console.log(error);
        }
      }
    )
  }

  guardarEmpacado() {
    if (!this.eEForm.valid && (this.data['tipoUsuario'] == 0)) {
      this.markFormGroupTouched(this.eEForm);
      return;
    }

    if(this.eEForm.get("password").value.toString().length > 0){
      let pasSHa = CryptoJS.SHA256(this.eEForm.get("password").value).toString()
      this.eEForm.patchValue({ password: pasSHa });
    }

    let data: any = {
      packedData: this.eEForm.value
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);


    this.dataService.postData<any>("ExternalPacked/SavePacked", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(`respuesta`, data)
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this._dialogRef.close(true);
        }
      },
      error => {
        console.log("error", error)
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);

        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("2")
        } else {
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

    this.dataService.postData<LoginUserResponse>("User/loginUser", "", requestLogin).subscribe(
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

        switch (peticion) {
          case "1":
            this.obtenerDatos();
            break;
          case "2":
            this.guardarEmpacado();
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

  public compareObjets(obj1, obj2): boolean {
    return obj1["id"] == obj2["id"];
  }

  /**
  * Marks all controls in a form group as touched
  * @param formGroup - The form group to touch
  */
  private markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }

}
