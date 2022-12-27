import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-dialogo-pre-vista',
  templateUrl: './dialogo-pre-vista.component.html',
  styleUrls: ['./dialogo-pre-vista.component.less']
})
export class DialogoPreVistaComponent implements OnInit {
  @Input()
  title:string;
  done:string;
  formPrevista:[];
  i:number;
  name:[];
  
    

  constructor( 
    @Inject(MAT_DIALOG_DATA)
    private _data: any,
    private dialogRef: MatDialogRef<DialogoPreVistaComponent>) { }

  ngOnInit() {
    this.title=this._data.title;
    this.formPrevista=this._data.formPrevista;
    this.name=this._data.name;
    console.log(this.name)
  }
  
  DeleteCompany() {
    this.dialogRef.close(true);
}
   
consultar

}
