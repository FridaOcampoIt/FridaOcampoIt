import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { DataServices } from '../Interfaces/Services/general.service';
import { OverlayService } from '../Interfaces/Services/overlay.service';
@Component({
    selector: 'app-dialog-delete-item',
    templateUrl: './service-alerts.template.html',
    styleUrls: ['./service-alerts.style.less']
})
export class DialogDeleteItem implements OnInit {
    _itemDelete: any;
    _urlOperation: string;
    _property: any;
    _deleteProperty: any;
    constructor(
        private _overlay: OverlayService,
        private snack: MatSnackBar,
        private _dialogRef: MatDialogRef<DialogDeleteItem>,
        @Inject(MAT_DIALOG_DATA) public _data: any,
        private _dataService: DataServices
    ){};

    ngOnInit() {
        this._itemDelete = this._data._item;
        this._urlOperation = this._data._url;
        this._property = this._data._property;
        this._deleteProperty = this._data._deleteProperty;
        console.log('Data', this._itemDelete, this._urlOperation, this._property, this._deleteProperty, this._data);
    }

    cancelOperation(){
        this._dialogRef.close(true);
    }
    async actionConfirmDelete() {
        let auxLoading: any;
        setTimeout(() =>{
            auxLoading= this._overlay.open();
        });
        let data = {};
        this._deleteProperty.forEach(_property => {
           data[_property] = this._itemDelete[_property];
        });
        await this._dataService.postData<any>(this._urlOperation, sessionStorage.getItem("token"), data).subscribe(
            data =>{
                //Warning eliminado 07/06/2022 por Frida Ocampo
                this.openSnack("Recurso eliminado con éxito","Aceptar");
                setTimeout(()=>{
                    this._dialogRef.close(true);
                    this._overlay.close(auxLoading);
                }, 2000);
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