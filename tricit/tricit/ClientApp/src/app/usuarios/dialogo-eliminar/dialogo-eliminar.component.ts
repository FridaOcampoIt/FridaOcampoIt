import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
    selector: 'app-dialogo-eliminar',
    templateUrl: './dialogo-eliminar.component.html',
    styleUrls: ['./dialogo-eliminar.component.less']
})

export class DialogoEliminarComponent implements OnInit {
    constructor(
        private dialogRef: MatDialogRef<DialogoEliminarComponent>) { };

    ngOnInit() {

    }

    DeleteUser() {
        this.dialogRef.close(true);
    }
}