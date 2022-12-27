import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DialogEliminarSectorComponent } from '../dialog-eliminar/dialog-eliminar-sector.component';

@Component({
  selector: 'app-dialog-propiedaes-input',
  templateUrl: './dialog-propiedaes-input.component.html',
  styleUrls: ['./dialog-propiedaes-input.component.less']
})
export class DialogPropiedaesInputComponent implements OnInit {
  @Input()
  title:string;
  bandera:string;
  numero:number;
  form:FormGroup;

  formualrio=[];
 

  constructor( 
    @Inject(MAT_DIALOG_DATA)
    private _data: any,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<DialogEliminarSectorComponent>) { }

  ngOnInit() {
    this.title=this._data.title;
    this.bandera=this._data.bandera;
    this.crearFormulario(); 
  }
  save() {
    this.formualrio=[
      this._data.conteo,
      this._data.nombreInput,
      100042,
      1,
       this._data.tipoInput,
      "",
    ]
    
   // console.log(this.formualrio)
    this.dialogRef.close(this.formualrio);

  }
  agreagr(){
 
  }
  onNoClick(selected:string){
    console.log(selected)
    this.dialogRef.close(true);
  }
  
  delete() {
    this.dialogRef.close(false);

  }

  crearFormulario() {
    this.form = this.fb.group({
      experienciaLaboral: this.fb.array([])
    });
  }

  get experienciaLaboral(): FormArray {
    return this.form.get('experienciaLaboral') as FormArray;
  }
  
  addCampo() {
    const trabajo = this.fb.group({
      empresa: new FormControl(''),
      puesto: new FormControl(''),
      descripcion: new FormControl('')
    });
  
    this.experienciaLaboral.push(trabajo);

  }
  borrarTrabajo(indice: number) {
    this.experienciaLaboral.removeAt(indice);
  }


      

}
