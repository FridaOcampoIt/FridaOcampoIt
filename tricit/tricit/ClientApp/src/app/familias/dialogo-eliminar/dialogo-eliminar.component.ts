import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
    selector: 'app-dialogo-eliminar',
    templateUrl: './dialogo-eliminar.component.html',
    styleUrls: ['./dialogo-eliminar.component.less']
})

export class DialogoEliminarComponent implements OnInit {
    constructor(
        private dialogRef: MatDialogRef<DialogoEliminarComponent>,
        @Inject(MAT_DIALOG_DATA)
        private _data: any) { };

    palabra: string = "";

    ngOnInit() {
        this.palabra = this._data.palabra;
    }

    DeleteFamily() {
        this.dialogRef.close(true);
    }
}