import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA, MatTableDataSource, MatPaginator, MatSort, MatDialog } from '@angular/material';
import { DialogoLectorComponent } from '../../../rastreo/dialogo-lector/dialogo-lector.component';
import { OverlayRef } from '@angular/cdk/overlay';

@Component({
  selector: 'app-dialogo-pallets',
  templateUrl: './dialogo-pallets.component.html',
  styleUrls: ['./dialogo-pallets.component.less']
})
export class DialogoPalletsComponent implements OnInit {
  displayedColumns: string[] = ['palletName','product', 'quantity', 'delete'];
	dataSource: MatTableDataSource<any>;

	@ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('qrInput') qrElement: ElementRef;
  
  constructor(
    private _dialog: MatDialog,
    private _router: Router,
    private _dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _dialogRef: MatDialogRef<DialogoPalletsComponent>,
    @Inject(MAT_DIALOG_DATA) public _data: any,
  ) { }

  overlayRef: OverlayRef;

  searchCode: string = "";
  init: number = 0;

  lstPallets: any = [];

  ngAfterViewInit() {
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
	}

  ngOnInit() {
    //this.setup(this.printerList);
    console.log("data: ", this._data);

		this.dataSource = new MatTableDataSource();
		this.dataSource.data = [];
  }

  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }

  applyFilter(event: Event) {
		const filterValue = (event.target as HTMLInputElement).value;
		this.dataSource.filter = filterValue.trim().toLowerCase();

		if (this.dataSource.paginator) {
			this.dataSource.paginator.firstPage();
		}
  }
  
  openScanerDialog() {
    const _dialogRef = this._dialog.open(DialogoLectorComponent, {
      panelClass: 'dialog-aprod',
      disableClose: false,
      data: { upallet: "true" }
    })
    /**
     * Obtener los datos del multiple seleccionado o setearlos, dependiendo como lo quieran programar
     */
    _dialogRef.afterClosed().subscribe(res => {
      if (res) {
        console.log("respuesta", res);
        this.searchCode = res;
      }
    });
  }

  aplicarBusqueda() {

    if (this.searchCode.trim().length == 0) {
      this.openSnack("Ingrese o tome lectura de una etiqueta", "Aceptar");
      return;
    }
    let codeqr:any = {};
    if(this.searchCode.includes("http")) {
      this.searchCode = decodeURIComponent(this.searchCode);
      if(this.searchCode.includes("¿")) {
        let result = "";
        for(let x=0;x<this.searchCode.length; x++) {
          result += this.searchCode[x].replace("[", '"').replace("-", "/").replace("'", "-").replace("Ñ", ":").replace("ñ",":").replace("*","}").replace("¿", "=").replace("_", "?").replace("¨", "{");
        };
        this.searchCode = result;
      }

      if(!this.searchCode.includes("tracking?qr=")) {
        this.openSnack("La etiqueta escaneada no corresponde a un pallet", "Aceptar");
        return;
      }
    } else if(this.searchCode.includes("{")) {
      this.searchCode = decodeURIComponent(this.searchCode);
    } else if(!this.searchCode.includes("{") && !this.searchCode.includes("http")) {
      this.openSnack("La etiqueta escaneada no corresponde a un pallett", "Aceptar");
      return;
    }

    codeqr = this.searchCode = this.searchCode.includes("qr=") ? this.searchCode.split("qr=")[1] : this.searchCode;
    codeqr = JSON.parse(this.searchCode);
    this.searchCode = "";
    this.qrElement.nativeElement.focus();

    if(codeqr.T != "P") {
      this.openSnack("La etiqueta escaneada no corresponde a un pallet", "Aceptar");
      return;
    }

    this.init = this.init > 1 ? 1 : this.init;
    
    let data: any = {
      ciuI: codeqr.I,
      ciuF: codeqr.F,
      init: this.init,
      productId: this.lstPallets.length > 0 ? this.lstPallets[0].productId : 0,
      packId: this.lstPallets.length > 0 ? this.lstPallets[0].packagingId : 0
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);
    
    this._dataService.postData<any>("PackedLabeled/searchInfoQrCode", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        if(!(data.infoLst.length > 0)) {
          this.openSnack("El pallet escaneado cuenta con otro tipo de producto", "Aceptar");
          return;
        }
        
        let existsPallet = false;
        let validProduct = false;
        let newItem: any = {};

        // Actualiza Tabla de Pallets Escaneados
        data.infoLst.forEach(item => {
          newItem = { opId: item.opId, ci: codeqr.I, cf: codeqr.F, pallet: item.pallet.split("PL-")[1].split("-")[0], palletName: item.pallet, productId: item.productId, product: item.product, quantity: codeqr.P, packagingId: item.packagingId };
          this.lstPallets.forEach(pallet => {
            // Validar que no se repita la misma etiqueta
            if(pallet.opId == newItem.opId && pallet.pallet == newItem.pallet) {
              existsPallet = true;
            }
            // Validar que siempre sea el mismo tipo de producto al primer escaneado
            if(item.productId != pallet.productId) {
              validProduct = true;
            }
          });

          // Actualizar la lista
          if(!existsPallet && !validProduct) {
            this.lstPallets.push(newItem);
          }
        });

        this.dataSource.data = [];
        this.dataSource.data = this.lstPallets;
        this.init++;

        if(validProduct) {
          this.openSnack("La etiqueta escaneada no es del mismo tipo de producto", "Aceptar");
          return;
        }
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        this.openSnack("Error al consultar la información", "Aceptar");
      }
    );
  }

	deleteItem(item) {
		this.dataSource.data.splice(item, 1);
		this.dataSource.data = this.dataSource.data;
    if(!this.dataSource.data.length)
      this.init = 0
	}

	save() {
    // save Operación
    console.log("this.lstPallets: ", this.lstPallets);
    let data: any = {
      pallets: this.lstPallets
    };

    setTimeout(() => {
      this.overlayRef = this._overlay.open();
    }, 1);

    this._dataService.postData<any>("PackedLabeled/saveOperationPallet", sessionStorage.getItem("token"), data).subscribe(
      data => {
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        console.log("success", data);
        this._dialogRef.close(data);
      },
      error => {
        console.log("error", error);
        setTimeout(() => {
          this._overlay.close(this.overlayRef);
        }, 1);
        this.openSnack("Error al consultar la información", "Aceptar");
      }
    );
	}

	cancel() {
		this._dialogRef.close(false);
	}
}
