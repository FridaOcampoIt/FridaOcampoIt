import { Component, OnInit, Inject, ViewChild, AfterViewInit, ElementRef, ContentChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { searchAcopioById } from '../../../Interfaces/Models/AcopioModels';
@Component({
    selector: 'app-dialogo-eliminar-acopio',
    templateUrl: './dialogo-eliminar-acopio.component.html',
    styleUrls: ['./dialogo-eliminar-acopio.component.less']
})
export class DialogoEliminarAcopioComponent implements OnInit {
    _acopioById: searchAcopioById;
    constructor(
        private _overlay: OverlayService,
        private snack: MatSnackBar,
        private _dialogRef: MatDialogRef<DialogoEliminarAcopioComponent>,
        @Inject(MAT_DIALOG_DATA) public _data: any,
        private _dataService: DataServices
    ){};

    acopioId: number = 0;
    ngOnInit() {
        this.acopioId = this._data['id'];
        this.obtenerDatos();
    }

    obtenerDatos(){
        let auxLoading: any;
        setTimeout(() =>{
            auxLoading= this._overlay.open();
        });
        let data = {
            acopioId: this.acopioId
        }
        this._dataService.postData<searchAcopioById>("Acopio/searchAcopioById", sessionStorage.getItem("token"), data).subscribe(
            data =>{
                if(data["messageEsp"] == null){
                    this._acopioById = data;
                    //console.log('Response', this._acopioById);
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
    deleteAcopio() {
        let auxLoading: any;
        setTimeout(() =>{
            auxLoading= this._overlay.open();
        });
        let data = {
            acopioId: this.acopioId
        }
        this._dataService.postData<searchAcopioById>("Acopio/deleteAcopioProductores", sessionStorage.getItem("token"), data).subscribe(
            data =>{
                if(data["messageEsp"] == null){
                    this._acopioById = data;
                }else{
                    //Warning eliminado 07/06/2022 por Frida Ocampo
                    this.openSnack("Acopio eliminado con éxito","Aceptar");
                }
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