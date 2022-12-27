import { Component, OnInit, ViewChild, AfterViewInit, Inject } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { DataServices } from '../../Interfaces/Services/general.service';
import { forEach } from '@angular/router/src/utils/collection';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../Interfaces/Models/LoginModels';

@Component({
  selector: 'app-dialogo-asociar-productos',
  templateUrl: './dialogo-asociar-productos.component.html',
  styleUrls: ['./dialogo-asociar-productos.component.less']
})
export class DialogoAsociarProductosComponent implements OnInit, AfterViewInit {

  data_table: any[] = [
    { id: 1, distribuidor: "nombre distribuidor", razonsocial: "razon social", telefono: 3232435465 },
    { id: 2, distribuidor: "nombre distribuidor", razonsocial: "razon social", telefono: 3232435465 },
    { id: 3, distribuidor: "nombre distribuidor", razonsocial: "razon social", telefono: 3232435465 },
    { id: 4, distribuidor: "nombre distribuidor", razonsocial: "razon social", telefono: 3232435465 }
  ]

  displayedColumns: string[] = ['companyName', 'productName', 'rawMaterial', 'packagingName'];
  dataSource = new MatTableDataSource<any>(this.data_table);
  selection = new SelectionModel<any>(false, []);
  itemsPagina: number[] = enviroments.pageSize;



  //Representa componente de paginación para la busqueda
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //Representa la instancia para el sorting de columnas
  @ViewChild(MatSort) sort: MatSort;

  asociarForm: FormGroup;
  overlayRef: OverlayRef;
  urlDatos: string;
  urlGuardar: string;

  dataCompanias: Array<any> = [];
  dataProductos: any = [];
  dataEmbalajes: any = [];

  tipoUsuario: string;
  userId;

  dataToTable: any = [];
  constructor(
    private _fb: FormBuilder,
    private _dialogRef: MatDialogRef<DialogoAsociarProductosComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dataService: DataServices,
    private _router: Router
  ) { }


  formGetter = (_campo) => {
    return this.asociarForm.get(_campo);
  }
  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return "{this.isAllSelected() ? 'select' : 'deselect'} all";
    }
    return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
  }

  ngOnInit() {

    switch (this.data["type"]) {
      /**
       * 0 Signifca que es un distribuidor
       */
      case 0:
        this.urlDatos = "Distributors/SearchAssociateProducts";
        this.urlGuardar = "Distributors/SaveAssociateProducts";
        this.tipoUsuario = "distributorId";
        this.asociarForm = this._fb.group({
          distributorId: [0],
          productId: ["", [Validators.required]],
          productName: [""],
          companyId: ["", [Validators.required]],
          companyName: [""],
          packagingId: ["", [Validators.required]],
          packagingName: [""],
          rawMaterial: ["", [Validators.required]],
          active: [0]
        })

        this.asociarForm.patchValue({
          distributorId: this.data["id"]
        });
        break;
      /**
       * 1 significa que es un proveedor
       */
      case 1:
        this.urlDatos = "Providers/SearchAssociateProducts";
        this.urlGuardar = "Providers/SaveAssociateProducts";
        this.tipoUsuario = "providerId";
        this.asociarForm = this._fb.group({
          providerId: [0],
          productId: ["", [Validators.required]],
          productName: [""],
          companyId: ["", [Validators.required]],
          companyName: [""],
          packagingId: ["", [Validators.required]],
          packagingName: [""],
          rawMaterial: ["", [Validators.required]],
          active: [0] //0 significa nuevo - 1 ya existente
        })

        this.asociarForm.patchValue({
          providerId: this.data["id"]
        });
        break;
      /**
       * 2 significa que es un empacado interno
       */
      case 2:
        this.urlDatos = "InternalPacked/SearchAssociateProducts";
        this.urlGuardar = "InternalPacked/SaveAssociateProducts";
        this.tipoUsuario = "packedId";
        this.asociarForm = this._fb.group({
          packedId: [0],
          productId: ["", [Validators.required]],
          productName: [""],
          companyId: ["", [Validators.required]],
          companyName: [""],
          packagingId: ["", [Validators.required]],
          packagingName: [""],
          rawMaterial: ["", [Validators.required]],
          active: [0]
        })

        this.asociarForm.patchValue({
          packedId: this.data["id"]
        });
        break;
      /**
       * 3 significa que es un empacado externo
       */
      case 3:
        this.urlDatos = "ExternalPacked/SearchAssociateProducts";
        this.urlGuardar = "ExternalPacked/SaveAssociateProducts";
        this.tipoUsuario = "packedId";
        this.asociarForm = this._fb.group({
          packedId: [0],
          productId: ["", [Validators.required]],
          productName: [""],
          companyId: ["", [Validators.required]],
          companyName: [""],
          packagingId: ["", [Validators.required]],
          packagingName: [""],
          rawMaterial: ["", [Validators.required]],
          active: [0]
        })

        this.asociarForm.patchValue({
          packedId: this.data["id"]
        });
        break;
      default:
        this.urlDatos = "";
        break;
    }

    this.dataSource.sort = this.sort;

    this.userId = this.data["id"];

    this.changesEvents();

    this.obtenerDatos();
  }

  obtenerDatos() {
    let data: any;

    switch (this.data["type"]) {
      /**
       * 0 Signifca que es un distribuidor
       */
      case 0:
        data = {
          distributorId: this.data["id"]
        };
        break;
      /**
       * 1 significa que es un proveedor
       */
      case 1:
        data = {
          providerId: this.data["id"]
        };
        break;
      /**
       * 2 - empacado interno
       */
      case 2:
        data = {
          packedId: this.data["id"]
        };
        break;
      /**
       * 3 - empacado externo
       */
      case 3:
        data = {
          packedId: this.data["id"]
        };
        break;
      default:
        this.urlDatos = "";
        break;
    }


    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>(this.urlDatos, sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);


        switch (this.data["type"]) {
          /**
           * 0 Signifca que es un distribuidor
           */
          case 0:
            this.dataSource.data = data['distributors'];
            break;
          /**
           * 1 significa que es un proveedor
           */
          case 1:
            this.dataSource.data = data['productData'];
            break;
          case 2:
            this.dataSource.data = data['productData'];
            break;
          case 3:
            this.dataSource.data = data['productData'];
            break;
          default:
            break;
        }

        this.obtenerCompanias();
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

  obtenerCompanias() {
    let data: any = {
    };

    setTimeout(() => {
      if (!this.overlayRef.hasAttached()) {
        this.overlayRef = this._overlay.open();
      }
    }, 1);

    this._dataService.postData<any>("PackedLabeled/SearchCompanyCombo", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);

        if (data["messageEng"].length) {
          this.openSnack("No hay compañias registradas o hubo error en la consulta", "Aceptar");
          return;
        }
        this.dataCompanias = data["companiescombo"];

        if (parseInt(sessionStorage.getItem("company")) != 0) {
          this.dataCompanias = this.dataCompanias.filter(x => x.id == parseInt(sessionStorage.getItem("company")) );
        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        if (error.error.hasOwnProperty("messageEsp")) {
          this.relogin("2");
        } else {
          console.log(error);
        }
      }
    )
  }

  obtenerProductos() {
    let data: any = {
      id: this.asociarForm.get("companyId").value
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("PackedLabeled/SearchProductCombo", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);

        if (data["messageEng"].length) {
          this.openSnack("No hay productos asociados o hubo error en la consulta", "Aceptar");
          return;
        }
        this.dataProductos = data["productscombo"];
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        this.openSnack("Error en la solicitud de productos", "Aceptar");
      }
    )
  }

  obtenerEmbalajes() {
    let data: any = {
      id: this.asociarForm.get("productId").value
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("PackedLabeled/SearchPackagingCombo", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);

        if (data["messageEng"].length) {
          this.openSnack("No hay emabalajes registrados o hubo error en la consulta", "Aceptar");
          return;
        }
        this.dataEmbalajes = data["packagingcombo"];
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        this.openSnack("Error en la solicitud de embalajes", "Aceptar");
      }
    )
  }

  /**
   * Agregar un registro nuevo a la tabla
   */
  agregarPrododucto() {

    if (!this.asociarForm.valid) {
      this.markFormGroupTouched(this.asociarForm);
      return;
    }

    this.dataSource.data.push(this.asociarForm.value);
    this.dataSource._updateChangeSubscription();

    this.asociarForm.reset();

    switch (this.data["type"]) {
      /**
       * 0 Signifca que es un distribuidor
       */
      case 0:
        this.asociarForm.patchValue({
          distributorId: this.data["id"],
          active: 0
        });
        break;
      /**
       * 1 significa que es un proveedor
       */
      case 1:

        this.asociarForm.patchValue({
          providerId: this.data["id"],
          active: 0
        });
        break;
      case 2:
        this.asociarForm.patchValue({
          packedId: this.data["id"],
          active: 0
        });
        break;
      case 3:
        this.asociarForm.patchValue({
          packedId: this.data["id"],
          active: 0
        });
        break;
      default:
        break;
    }

    //reset de sliders contendores
    this.dataProductos = [];
    this.dataEmbalajes = [];

  }

  asociarProductos() {

    let lista: Array<any> = [];

    this.dataSource.data.forEach(ele => {

      if (ele["active"] != 1) {
        lista.push({
          [this.tipoUsuario]: ele[this.tipoUsuario],
          productId: ele["productId"],
          companyId: ele["companyId"],
          packagingId: ele["packagingId"],
          rawMaterial: ele["rawMaterial"],
          active: ele["active"]
        })  
      }
    });

    let data: any = {
      productData: lista
    };


    if (this.dataSource.data.length == 0) {
      this.openSnack("No hay registros de producto para asociar", "Aceptar");
      return;
    }

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>(this.urlGuardar, sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if (data["messageEng"].length) {
          this.openSnack(data["messageEsp"], "Aceptar");
        } else {
          this.openSnack("Productos Asociados correctamente", "Aceptar");
          this._dialogRef.close(true);
        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        this.openSnack("Error en la solicitud", "Aceptar");
      }
    )

  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  changesEvents() {

    this.asociarForm.get("companyId").valueChanges.subscribe(reg => {
      if (reg != null && reg != "") {

        //Reseteo de producto y de embalaje
        this.asociarForm.patchValue({
          productId: "",
          packagingId: "",
          companyName: this.dataCompanias.find(itm => itm.id == reg)["data"]
        }, { emitEvent: false });

        //busqueda nueva de productos
        this.obtenerProductos();
      }
    });


    this.asociarForm.get("productId").valueChanges.subscribe(reg => {
      if (reg != null && reg != "") {

        //Reseteo de embalaje
        this.asociarForm.patchValue({
          packagingId: "",
          productName: this.dataProductos.find(itm => itm.id == reg)["data"]
        }, { emitEvent: false });

        //Busqueda nueva de embalajes
        this.obtenerEmbalajes();
      }
    });

    this.asociarForm.get("packagingId").valueChanges.subscribe(reg => {
      if (reg != null && reg != "") {
        this.asociarForm.patchValue({
          packagingName: this.dataEmbalajes.find(itm => itm.id == reg)["data"]
        }, { emitEvent: false });
      }
    })
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
            this.obtenerDatos();
            break;
          case "2":
            this.obtenerCompanias();
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

  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }

}
