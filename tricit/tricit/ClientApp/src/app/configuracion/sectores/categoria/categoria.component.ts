import { Component, NgModule, OnInit } from '@angular/core';  
import { MatDialog, MatFormFieldModule, MatInputModule } from '@angular/material';
import { FormGroup, FormsModule, ReactiveFormsModule, FormControl, FormBuilder, Validators, FormArray  } from '@angular/forms';
import { DialogoPreVistaComponent } from '../dialogo-pre-vista/dialogo-pre-vista.component';
import { DialogPropiedaesInputComponent } from '../dialog-propiedaes-input/dialog-propiedaes-input.component';
import { CompaniasRoutingModule } from '../../../companias/companias-routing.module';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Location } from '@angular/common';


@NgModule({
  imports: [
      MatFormFieldModule,
      MatInputModule,
      FormsModule,
      ReactiveFormsModule,
  ]
})
@Component({
  selector: 'app-categoria',
  templateUrl: './categoria.component.html',
  styleUrls: ['./categoria.component.less']
})

export class CategoriaComponent implements OnInit {
  form: FormGroup;
  formm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private _dialog: MatDialog,
    private rutaActiva: ActivatedRoute,
    private router: Router,
    private location:Location
    ) {
      
  }
 
  nombreCategoria;
  ruta;
  formulario=[{
    id:0,
    nombre:"",
    estatus:100042,
    usuarioCreador:1,
    tipo:"",
    placeholder:"",
    propiedades:[{nombre:"",value:2}]
  }]
  conteo=0;

  crearFormulario() {
    this.formm = this.fb.group({
      experienciaLaboral: this.fb.array([])
    });
  }

  get experienciaLaboral(): FormArray {
    return this.formm.get('experienciaLaboral') as FormArray;
  }
  
  addCampo() {
    const trabajo = this.fb.group({
      empresa: new FormControl({value:this.nombreCategoria,disabled:true}),
      puesto: new FormControl(''),
      descripcion: new FormControl('')
    });
  
    this.experienciaLaboral.push(trabajo);
    console.log(this.nombreCategoria)
    this.nombreCategoria=" ";
    console.log(this.nombreCategoria)
  }
  borrarTrabajo(indice: number) {
    this.experienciaLaboral.removeAt(indice);
  }


  
   
  /*Nombre del formualrio que llega por parametro */
  Formulario: {nombre: string};

  
  ngOnInit() {
    // nombre de sector por parametro
   this.Formulario = {
    nombre: this.rutaActiva.snapshot.params.nombre,
    };
    this.rutaActiva.params.subscribe(
      (params: Params) => {
        this.Formulario.nombre = params.nombre;
      }
    );
    this.ruta=this.router.url;
    this.crearFormulario();
  }

  goBack(){
    this.location.back();
    this.ruta=this.router.url;
  }


  //MODALS
  propiedadesInput(bandera:boolean,numero:number){
    var title="Agregar categoria";
    console.log(bandera)
    if (!bandera) {
      var  title="Propiedades del campo";
    }
    this.conteo++;
    var numero=numero;
    var bandera=bandera;

    const dialogRef = this._dialog.open(DialogPropiedaesInputComponent, {
      width: 'dialog-comp  ',
      disableClose: true,
      data: {
          title: title, 
          bandera:bandera,
          numero:numero,
          conteo:this.conteo,
      }, 
    });
    dialogRef.afterClosed().subscribe(result => {
      // EMPUJA LO QUE VIENE DLE MODAL

    });
  }

  vistaPrevia(){
    
    var title="Vista previa";
    console.log(bandera)
   
    var i=i;
    var bandera=bandera;
    var nombre=nombre;

    const dialogRef = this._dialog.open(DialogoPreVistaComponent, {
      width: 'dialog-comp  ',
      disableClose: true,
      data: {
          /*id: this.idCompany,*/
          title: title, 
          bandera:bandera,
          i:i,
     

      }, 
    });
    dialogRef.afterClosed().subscribe(result => {
    // this.animal = result;
      if (result && !null) {
      /* this.addform();
        console.log(this.animal = result)
        this.name.push(this.animal = result)
        console.log(this.name)*/
        
      }
    });
  }
  

  /*FUNCION PARA CREAR LOS INPUTS */
  cantidad = 0;
 agregarHijo(result,numero:number) {
  console.log(this.formulario)
  this.cantidad++;
     //SELECT
  if (this.formulario[this.cantidad].tipo=="select") {
    var inputSelect = document.createElement('select');
    inputSelect.name = result.nombre + this.cantidad;
    inputSelect.id = result.id+'nombre' + this.cantidad;
    inputSelect.className = 'col-4 form-control bg-light border-info m-3';
   
    document.getElementById(`fs-${numero}`).appendChild(inputSelect);
    document.getElementById(`fs-${numero}`).appendChild(document.createElement('br'));
    //TEXTARE
  } else if(this.formulario[this.cantidad].tipo=="textarea"){
    var inputTextarea = document.createElement('textarea');
    inputTextarea.name = this.formulario[this.cantidad].nombre + this.cantidad;
    inputTextarea.id = 'nombre' + this.cantidad;
    inputTextarea.className = ' col-4 form-control bg-light border-info m-3';

    document.getElementById(`fs-${numero}`).appendChild(inputTextarea);
    const input= document.getElementById(`fs-${numero}`).appendChild(document.createElement('input'));
    input.type="button";
    input.className="btn";
    input.value="⁞";
    //RADIO & CHECKBOX
  }else if(this.formulario[this.cantidad].tipo=="radio"||this.formulario[this.cantidad].tipo=="checkbox"){
    var inputRC = document.createElement('input');
    inputRC.type = this.formulario[this.cantidad].tipo;
    inputRC.name = this.formulario[this.cantidad].nombre + this.cantidad;
    inputRC.value = this.formulario[this.cantidad].nombre;
    inputRC.id = this.formulario[this.cantidad].nombre+ this.cantidad;

   
    inputRC.className = ' col-12 bg-light ';
    document.getElementById(`fs-${numero}`).appendChild(inputRC);
     const label=document.getElementById(`fs-${numero}`).appendChild(document.createElement('label'));
     label.innerText="valor";
     label.htmlFor=this.formulario[this.cantidad].nombre+ this.cantidad;
    
    //INPUTS RESTANTES
  }else{
    var inputGeneral = document.createElement('input');
    inputGeneral.type = this.formulario[this.cantidad].tipo;
    inputGeneral.name = this.formulario[this.cantidad].nombre + this.cantidad;
    inputGeneral.placeholder = this.formulario[this.cantidad].nombre;
    inputGeneral.id = "" + this.cantidad;
    inputGeneral.className = ' col-4 form-control bg-light border-info m-3';

    document.getElementById(`fs-${numero}`).appendChild(inputGeneral);
    const input= document.getElementById(`fs-${numero}`).appendChild(document.createElement('input'));
    input.type="button";
    input.className="btn";
    input.value="⁞";
    const a= document.querySelector(`fs-${numero}`);
    a.innerHTML= `<input type="button (click)="propiedadesInput()" value="agregar/>`;
    
    
  }
  
  }
}
