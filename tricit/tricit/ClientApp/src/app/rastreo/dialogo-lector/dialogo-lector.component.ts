import { Component, OnInit, AfterViewInit, ViewChild, Inject } from '@angular/core';
import { BarcodeFormat } from '@zxing/library';
import { BehaviorSubject } from 'rxjs';
import { MatDialog, MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OverlayRef } from '@angular/cdk/overlay';
import { OverlayService } from '../../Interfaces/Services/overlay.service';

declare var Html5QrcodeScanner: any;
@Component({
  selector: 'app-dialogo-lector',
  templateUrl: './dialogo-lector.component.html',
  styleUrls: ['./dialogo-lector.component.less']
})
export class DialogoLectorComponent implements OnInit {

  overlayRef: OverlayRef;
  availableDevices: MediaDeviceInfo[];
  currentDevice: MediaDeviceInfo = null;
  scannerEnabled: boolean = true;

  formatsEnabled: BarcodeFormat[] = [
    BarcodeFormat.CODE_128,
    BarcodeFormat.DATA_MATRIX,
    BarcodeFormat.EAN_13,
    BarcodeFormat.QR_CODE,
  ];

  hasDevices: boolean;
  hasPermission: boolean;

  qrResultString: string;

  constructor(private readonly _dialog: MatDialog, private snack: MatSnackBar, private _dialogRef: MatDialogRef<DialogoLectorComponent>, private _overlay: OverlayService,
    @Inject(MAT_DIALOG_DATA) public _data: any) { }

  ngOnInit() {

  }

  clearResult(): void {
    this.qrResultString = null;
  }

  onCamerasFound(devices: MediaDeviceInfo[]): void {
    this.availableDevices = devices;
    this.hasDevices = Boolean(devices && devices.length);
  }

  onCodeResult(resultString: string) {
    this.qrResultString = resultString;

    //ejemplo https://traceit.net/rastreo?ciu=AGDKDU37365W5DHDH886
    //ejemplo 2 https://data.traceit.net/tracking?ciu={"T":"C","P":50,"I":"38DC22A20A35B","F":"38DC22A20A38C","ID":"Pedido161220-PL-1-Bx-1"}

    let stringerino: string = resultString;
    if (stringerino.includes("ciu=")) {
      let clave = stringerino.split("=");
      let resultado: string = clave[1];

      if (resultado.length) {
        this.scannerEnabled = false;
        if(this._data != null) {
          this._dialogRef.close("");
          return;
        }
        this._dialogRef.close(resultado);
      } else {
        this.openSnack("El QR tiene una liga vacia o no es válido", "Aceptar");
      }

    }
    
    if (stringerino.includes("qr=")) {
      let clave = stringerino.split("=");
      let resultado: string = clave[1];
      
      if (resultado.length) {
      
        let jsorino = JSON.parse(decodeURI(resultado));
        if(this._data != null) {
          this._dialogRef.close(this.qrResultString);
          return;
        };

        let type = jsorino["T"];
        
      
        if (type == "C" || type == "P") {

          let inicial = jsorino["I"];
          if (inicial.length) {
            this.scannerEnabled = false;
            this._dialogRef.close(inicial);
          } else {
            this.openSnack("El QR tiene una liga vacia o no es válido", "Aceptar");
          }
  
        } else if (type == "A") {
          let inicial = jsorino["O"];

          if (inicial.length) {
            this.scannerEnabled = false;
            this._dialogRef.close(inicial);
          } else {
            this.openSnack("El QR tiene una liga vacia o no es válido", "Aceptar");
          }
          
        }
      } else {
        this.openSnack("El QR tiene una liga vacia o no es válido", "Aceptar");
      }


    }

  }

  // buscarCIU(idOperacion = 0) {

  //   if (idOperacion == 0) {
  //     this.openSnack("La etiqueta cuenta con un error de formato.", "Aceptar");
  //     console.log("La operación no puede ser 0");
  //     return;
  //   }

  //   let data: any = {
  //     operacionId: idOperacion
  //   };

  //   setTimeout(() => {
  //     this.overlayRef = this._overlay.open();
  //   }, 1);

  //   this._dataService.postData<any>("Tracking/SearchTracking", sessionStorage.getItem("token"), data).subscribe(
  //     data => {
  //       setTimeout(() => {
  //         this._overlay.close(this.overlayRef);
  //       }, 1);
  //       console.log("success", data);
  //       if (data["messageEng"] != "") {
  //         this.openSnack(data["messageEsp"], "Aceptar");
  //       } else {
  //         if (data.trackingList.length == 0) {
  //           this.openSnack("No hay registros", "Aceptar");
  //         } else {
  //           this.creacionLinea(data.trackingList);
  //         }
  //       }
  //     },
  //     error => {
  //       console.log("error", error);
  //       setTimeout(() => {
  //         this._overlay.close(this.overlayRef);
  //       }, 1);
  //       if (error.error.hasOwnProperty("messageEsp")) {
  //         this.relogin("1");
  //       } else {
  //         console.log(error);
  //       }
  //     }
  //   )

  // }

  onDeviceSelectChange(selected: string) {
    const device = this.availableDevices.find(x => x.deviceId === selected);
    this.currentDevice = device || null;
  }

  onHasPermission(has: boolean) {
    this.hasPermission = has;
  }

  empezarCaptura() {
    this.scannerEnabled = true;
  }

  cerrarCaptura() {
    this.scannerEnabled = false;
  }

  //Funcion para abrir el modal del mensaje
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
      duration: 5000
    })
  }


}
