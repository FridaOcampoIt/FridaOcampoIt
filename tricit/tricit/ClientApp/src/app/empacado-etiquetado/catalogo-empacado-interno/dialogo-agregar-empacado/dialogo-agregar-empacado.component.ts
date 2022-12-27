import { AfterViewInit, Component, OnDestroy, OnInit, Inject, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { enviroments } from '../../../Interfaces/Enviroments/enviroments';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import * as CryptoJS from 'crypto-js';
import { ReplaySubject, Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';

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

  iEmp: FormGroup;
  regExMail: RegExp = RegExp(enviroments.patterns.email);
  regExNum: RegExp = RegExp(enviroments.patterns.numerical);
  titulo: string = "";

  overlayRef: OverlayRef;
  protected _onDestroy = new Subject();
  public websiteMultiCtrl: FormControl = new FormControl();
  public companyMultiFilterCtrl: FormControl = new FormControl();

  constructor(
    private _fb: FormBuilder,
    private dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dialogRef: MatDialogRef<DialogoAgregarEmpacadoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  formGetter = (_campo) => {
    return this.iEmp.get(_campo);
  }
  tipoUsuario: Number = 0;
  company : number = 0;
  isType: number = 0;
  //TraceIt : company = 0 y isType = 0
  //Compañia: company != 0 y isType = 0
  //Empacador: company != 0 y isType = 1 
  async ngOnInit() {
    this.iEmp = await this._fb.group({
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
    });
    this.company = parseInt(sessionStorage.getItem("company"));
    this.isType = parseInt (sessionStorage.getItem("isType"));
    console.log("Company", this.company);
    await this.iEmp.patchValue({ estatus: true })
    this.titulo = await this.data["titulo"];
    this.tipoUsuario = await this.data["tipoUsuario"];
    if (this.data["action"] != 0) {
      await this.obtenerDatos();
      this.iEmp.get("password").clearValidators();
      this.iEmp.get("password").updateValueAndValidity();
    }else if(this.data['tipoUsuario'] == 0){
      await this.getCompaniasForEmpacador();
    }
  }
  
  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
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
    await this.dataService.postData<any>("InternalPacked/SearchPackedData", sessionStorage.getItem("token"), data).subscribe(
      data => {
        // console.log(data);
        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
          setTimeout(() => {
            this._overlay.close(auxLoading);
          }, 1);
        }else {
          this.iEmp.patchValue(data["packedList"][0]);
          console.log('DATA', data['packedList'][0]);
           if(this.data['tipoUsuario'] == 0){
            //Array en string
            let arraytemporal = data["packedList"][0].getCompaniasId.split(',');
            //Convertimos el string a enteros (porque yolo, no funciona con string)
            let arrayNumeros = arraytemporal.map(str => {
              return Number(str);
            });
            //Seteamos la información en el select
            this.iEmp.controls['companiasId'].setValue(arrayNumeros);
            //Si recuperamos la información de un registro guardamos los id's que estaban en el arreglo
            this.iEmp.controls['auxGetCompany'].setValue(arrayNumeros);
          }else if(this.data['tipoUsuario'] != 0){ //Evaluamos que sea una compania para mosrar solo los datos
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
          this._overlay.close(auxLoading);
        }, 1);
        this.openSnack("Error en la solicitud", "Aceptar");
      }
    );
  }
  guardarEmpacado() {    
    console.log('data', !this.iEmp.valid, this.iEmp.valid, this.data['tipoUsuario'],  this.markFormGroupTouched(this.iEmp), this.iEmp.value);
    if (!this.iEmp.valid && (this.data['tipoUsuario'] == 0)) {
      this.markFormGroupTouched(this.iEmp);
      return;
    }

    if(this.iEmp.get("password").value.toString().length > 0){
      let pasSHa = CryptoJS.SHA256(this.iEmp.get("password").value).toString()
      this.iEmp.patchValue({ password: pasSHa });
    }

    let data: any = {
      packedData: this.iEmp.value
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);
    console.warn('DATOS', data);
    this.dataService.postData<any>("InternalPacked/SavePacked", sessionStorage.getItem("token"), data).subscribe(
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

        if (error.messageEsp != null) {
          this.openSnack(error.messageEsp, "Aceptar");
          console.log('verdader');
        } else {
          this.openSnack('Error al mandar la solicitud', "Aceptar");
          console.log('falso');
        }
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
