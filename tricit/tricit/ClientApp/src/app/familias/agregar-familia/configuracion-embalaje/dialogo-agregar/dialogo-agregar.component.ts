import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { DataServices } from '../../../../Interfaces/Services/general.service';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { OverlayService } from '../../../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../../Interfaces/Models/LoginModels';


interface EmpacadoresInternosExternos{
  packedId : number;
  packedNumber : string;
  packedName : string;
  type : number;
  companyId : number;
}
@Component({
  selector: 'app-dialogo-agregar',
  templateUrl: './dialogo-agregar.component.html',
  styleUrls: ['./dialogo-agregar.component.less']
})
export class DialogoAgregarConfiguracionComponent implements OnInit {

  tiposLectura: any[] = [
    { id: 1, nombre: "Por Línea" },
    { id: 2, nombre: "Por Caja" },
    { id: 3, nombre: 'Por Unidad' }
  ]

  tipoEtiquetacaja: any[] = [
    { id: 1, nombre: "Por Línea" },
    { id: 2, nombre: "Por Caja" },
    { id: 3, nombre: 'Por Unidad' }
  ];

  tipoEtiquetaPallet: any = [
    { id: 1, nombre: "Por Línea" },
    { id: 2, nombre: "Por Caja" },
    { id: 3, nombre: 'Por Unidad' }
  ];

  confiForm: FormGroup;

  overlayRef: OverlayRef;

  constructor(
    private _fb: FormBuilder,
    private _dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dialogRef: MatDialogRef<DialogoAgregarConfiguracionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _router: Router
  ) { }

  formGetter = (_campo) => {
    return this.confiForm.get(_campo);
  }

  async ngOnInit() {
    this.confiForm = this._fb.group({
      packagingId: [this.data["embalajeId"]],
      packagingType: ["", [Validators.required]],
      readingType: ["", [Validators.required]],
      // boxLabelType: ["", [Validators.required]],
      // boxLabelPallet: ["", [Validators.required]],
      unitsPerBox: ["", [Validators.required, Validators.pattern("^[0-9]*$")]],
      copiesPerBox: ["", [Validators.required, Validators.pattern("^[0-9]*$")]],
      linesPerBox: ["", [Validators.required, Validators.pattern("^[0-9]*$")]],
      grossWeightPerBox: [0, [Validators.required]],
      dimensionsWeightPerBox: ["", [Validators.required]],
      boxesPerPallet: ["", [Validators.required, Validators.pattern("^[0-9]*$")]],
      copiesPerPallet: ["", [Validators.required, Validators.pattern("^[0-9]*$")]],
      grossWeightPerPallet: [0, [Validators.required]],
      dimensionsPerPallet: ["", [Validators.required]],
      instructionsWarnings: [""],
      familyId: [this.data["familyId"]], 
      empacadoresId: [[], [Validators.required, Validators.minLength(1)]],
      auxEmpacadoresId: []
    })

    this.confiForm.patchValue({ familyId: this.data["familyId"] });

    this.obtenerEtiquetas();

    this.changeEFunctions();
    await this.obtenerEmpacadores(this.data["companyId"]);
  }
  
  empacadores: Array<EmpacadoresInternosExternos> = [];
  obtenerEmpacadores(id) {
    let data: any = {
      companyId: id
    };
    let auxLoading: any;
    setTimeout(() => {
      auxLoading = this._overlay.open();
     }, 1);
  
    this._dataService.postData<any>("InternalPacked/SearchInternalExternalPacked", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(auxLoading);
        }, 1);

        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.empacadores = data['packedList'];
          console.warn('EMPACADORES', this.empacadores, this.empacadores.length);
        }
      },
      error => {
        setTimeout(() => {
          this._overlay.close(auxLoading);
        }, 1);
        console.error('Error al obtener empacadores', error);
      }
    );
  }
    
  async obtenerEtiquetas() {
    let dato = {
      opc: 2,
      name: sessionStorage.getItem("company")
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("LabelsQR/SearchLabelsCombo", sessionStorage.getItem("token"), dato).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("data", data);
        if (data["messageEsp"] !== "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.tipoEtiquetaPallet = data["labelsqrcombo"];
          this.tipoEtiquetacaja = data["labelsqrcombo"];

          /**
           * Al editar, mandar dato del id seleccionado, buscar sus datos, hacerle seteo al form.
           */

          if (this.data["action"] != 0) {
            this.obtenerDatos();
          }

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

  async obtenerDatos() {
    let data: any = {
      familyId: this.data["familyId"],
      packagingId: this.data["embalajeId"],
      companyId: this.data["companyId"]
    };
    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    await this._dataService.postData<any>("Families/getPackaging", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("data", data);
        if (data["messageEsp"] != "") {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.confiForm.patchValue(data["packagingList"][0]);
          this.confiForm.patchValue({ readingType: parseInt(data["packagingList"][0]["readingType"]) });
          console.warn(data['packagingList'])
          //Array en string
          let arraytemporal = data["packagingList"][0].getEmpacadoresId.split(',');
          //Convertimos el string a enteros (porque yolo, no funciona con string)
          let arrayNumeros = arraytemporal.map(str => {
            return Number(str);
          });
          this.confiForm.controls['empacadoresId'].setValue(arrayNumeros);
          //Si recuperamos la información de un registro guardamos los id's que estaban en el arreglo
          this.confiForm.controls['auxEmpacadoresId'].setValue(arrayNumeros);
          console.log('FORMULARIO', this.confiForm);
        }
      },
      error => {
        console.log("error", error);
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

  guardarConfiguracion() {
    if (!this.confiForm.valid) {
      this.markFormGroupTouched(this.confiForm);
      return;
    }

    let data: any = this.confiForm.value;

    //  

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("Families/savePackaging", sessionStorage.getItem("token"), data).subscribe(
      data => {
        console.log(`respuesta`, data)
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        //  
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
          this.relogin("3")
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

        switch (peticion) {
          case "1":
            this.obtenerEtiquetas();
            break;
          case "2":
            this.obtenerDatos();
            break;
          case "3":
            this.guardarConfiguracion();
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

  changeEFunctions() {
    this.confiForm.get("grossWeightPerBox").valueChanges.subscribe(val => {
      console.log("bruo caja", val);
      let pesoCaja = 0;
      let cajaPorPallet = 0;

      //si no tiene valor, no proceder
      if (!this.confiForm.get("boxesPerPallet").value || !val) {
        return;
      }

      cajaPorPallet = parseFloat(this.confiForm.get("boxesPerPallet").value);

      try {
        pesoCaja = parseFloat(val);
      } catch (error) {
        this.openSnack("El valor ingresado en el campo 'Peso bruto por caja' no es un valor númerico", "ok");
        return;
      }

      if (pesoCaja > 0) {
        if (cajaPorPallet > 0) {
          this.confiForm.patchValue({
            grossWeightPerPallet: pesoCaja * cajaPorPallet
          })
        }
      }
    })

    this.confiForm.get("boxesPerPallet").valueChanges.subscribe(val => {
      console.log("caja por pallet", val);
      let pesoCaja = 0 ;
      let cajaPorPallet = 0;

      //si no tiene valor, no proceder
      if (!this.confiForm.get("grossWeightPerBox").value || !val) {
        return;
      }
      
      pesoCaja = parseFloat(this.confiForm.get("grossWeightPerBox").value);

      try {
        cajaPorPallet = parseFloat(val);
      } catch (error) {
        this.openSnack("El valor ingresado en el campo Cajas por pallet no es un valor númerico", "ok");
        return;
      }

      if (pesoCaja > 0) {
        if (cajaPorPallet > 0) {
          this.confiForm.patchValue({
            grossWeightPerPallet: pesoCaja * cajaPorPallet
          })
        }
      }
    })
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
