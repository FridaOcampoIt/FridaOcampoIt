import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-dialog-eliminar-sector',
  templateUrl: './dialog-eliminar-sector.component.html',
  styleUrls: ['./dialog-eliminar-sector.component.less']
})
export class DialogEliminarSectorComponent implements OnInit {
  @Input()
  title:string;
  nombre:string;
  estatus:number;
  estatusEliminar:boolean;
    
  constructor( 
    @Inject(MAT_DIALOG_DATA)
    private _data: any,
    private dialogRef: MatDialogRef<DialogEliminarSectorComponent>) { }

  ngOnInit() {
    this.title=this._data.title;
  }
  DeleteCompany() {
    this.dialogRef.close(true);
  }
         
}
