import { Component, OnInit, Inject, ViewChild, AfterViewInit, ElementRef, ContentChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { DataServices } from '../../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { SearchProductorById } from '../../../../Interfaces/Models/ProductorModels';
@Component({
    selector: 'app-dialogo-eliminar-productor',
    templateUrl: './dialogo-eliminar-productor.component.html',
    styleUrls: ['./dialogo-eliminar-productor.component.less']
})
export class DialogoEliminarProductorComponent implements OnInit {
    _productorById: SearchProductorById;
    constructor(
        private _overlay: OverlayService,
        private snack: MatSnackBar,
        private _dialogRef: MatDialogRef<DialogoEliminarProductorComponent>,
        @Inject(MAT_DIALOG_DATA) public _data: any,
        private _dataService: DataServices
    ){};

    productorId: number = 0;
    ngOnInit() {
        this.productorId = this._data['id'];
        this.obtenerDatos();
    }

    obtenerDatos(){
        let auxLoading: any;
        setTimeout(() =>{
            auxLoading= this._overlay.open();
        });
        let data = {
            productorId: this.productorId
        }
        this._dataService.postData<SearchProductorById>("Productor/searchProductorById", sessionStorage.getItem("token"), data).subscribe(
            data =>{
                if(data["messageEsp"] == null){
                    this._productorById = data;
                    //console.log('Response', this._productorById);
                }else{
                    this.openSnack(data["messageEsp"],"Aceptar");
                }
                setTimeout(()=>{
                    this._overlay.close(auxLoading);
                }, 1000);
            },error =>{
                this.openSnack(error.error.hasOwnProperty("messageEsp"),"Aceptar");
                setTimeout(() => {
                    this._dialogRef.close(true);
                    this._overlay.close(auxLoading);
                },1);
            }
        );
    }
    deleteProductor() {
        let auxLoading: any;
        setTimeout(() =>{
            auxLoading= this._overlay.open();
        });
        let data = {
            productorId: this.productorId
        }
        this._dataService.postData<any>("Productor/deleteProductorById", sessionStorage.getItem("token"), data).subscribe(
            data =>{
                this.openSnack("Productor eliminado con éxito","Aceptar");
                setTimeout(()=>{
                    this._dialogRef.close(true);
                    this._overlay.close(auxLoading);
                }, 7000);
            },error =>{
                this.openSnack(error.error.hasOwnProperty("messageEsp"),"Aceptar");
                setTimeout(() => {
                    this._dialogRef.close(true);
                    this._overlay.close(auxLoading);
                },1);
            }
        );
    }

    openSnack = (message: string, action: string) => {
        this.snack.open(message, action, {
            duration: 5000
        });
    }
}