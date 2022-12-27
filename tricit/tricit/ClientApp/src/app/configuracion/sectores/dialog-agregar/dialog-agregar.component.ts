import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MatSnackBar, MAT_DIALOG_DATA } from '@angular/material';
import { DialogEliminarSectorComponent } from '../dialog-eliminar/dialog-eliminar-sector.component';

import {
  Sector,
  SearchSectorsResponse,
  Form,
  SearchFormsResponse
  
} from '../../../Interfaces/Models/Sectors&Forms';

import { DataServices } from '../../../Interfaces/Services/general.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { async } from '@angular/core/testing';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-dialog-agregar',
  templateUrl: './dialog-agregar.component.html',
  styleUrls: ['./dialog-agregar.component.less']
})
export class DialogAgregarComponent implements OnInit {
  @Input()
  /* The above code is declaring the variables that will be used in the component. */
  title:string;
  idSector:number;
  formId:number;
  nombres:Array<any>;
  nombresForm:Array<any>;
  temporaleditar:Array<any>;
  idSectorToForm:number;
 
 

  /* Creating a new instance of the class. */
  response = new Sector();
  responseSearch = new SearchSectorsResponse();
  responseForm = new Form();

  constructor( 
    @Inject(MAT_DIALOG_DATA)
    private _data: any,
    private dialogRef: MatDialogRef<DialogAgregarComponent>,
    private _dataService: DataServices,
    private _overlay: OverlayService,
    private snack: MatSnackBar,
    private _fb: FormBuilder,
  ) { }

  sectorForm: FormGroup;
  Form: FormGroup;
  auxLoading: any;
  datosConsultados;

  
   
  ngOnInit() {
    this.title=this._data.title;
    this.nombres=this._data.nombres;
  
    /* Used to print the values of the variables in the console. */
    console.log(this.nombres);
    console.log(this.temporaleditar);
    console.log(this._data.nombresForm);
 
      /* Creating a form group sector. */
      this.sectorForm=this._fb.group({
        sectorId:[this._data.idsector],
        estatusId:[''.toUpperCase()],
        nombre:[''.toUpperCase(),Validators.required],
        descripcionCorta:[''.toUpperCase(), Validators.required]
      })
    
      /* Creating a form group formulario. */
      this.Form=this._fb.group({
        formularioId:[],
        estatusFormularioId:[''],
        nombre:['',Validators.required],
        descripcionCorta:['', Validators.required],
        sectorId:[this._data.idSectorToForm]
      })
   
     if (this._data.title=="Editar sector") {
      this.SearchSectors();
    }else if(this._data.title=="Editar formulario"){
      this.SearchForms();
    }

  }
  

  /**
  * This function is used to search a sector by its id
  */
   SearchSectors(){
    let data = {sectorId: this._data.idSector};
    
    setTimeout(() => {
      this.auxLoading = this._overlay.open();
      }, 1);
    
     this._dataService.postData<SearchSectorsResponse>("Sectors/searchSectorsById", sessionStorage.getItem("token"), data).subscribe(
      data =>{
        this.responseSearch=data;
        console.log(this.responseSearch.sectorsDataList);
        this.sectorForm.patchValue(data);
        setTimeout(() => {
          this.auxLoading = this._overlay.close(this.auxLoading);
        }, 1); 
      },
      error =>{
        if (error.error.hasOwnProperty("messageEsp")) {
            
        } else {
            //console.log(error);
        }
      }
    )
    
  }

  /**
  * A function that is used to search for a form by id.
  */
  async SearchForms(){
    let data = {formularioId: this._data.formId};
    setTimeout(() => {
      this.auxLoading = this._overlay.open();
    }, 1);
    
    await this._dataService.postData<SearchFormsResponse>("Forms/searchFormsById", sessionStorage.getItem("token"), data).subscribe(
      data =>{
        this.Form.patchValue(data);
        this.datosConsultados=data;
        setTimeout(() => {
          this.auxLoading = this._overlay.close(this.auxLoading);
          }, 1);
      },
      error =>{
        if (error.error.hasOwnProperty("messageEsp")) {
            
        } else {
            //console.log(error);
        }
      }
    )
  }
  
 /**converti los dato en uper y low camel case
  * https://www.freecodecamp.org/espanol/news/como-convertir-una-cadena-en-minusculas-y-mayusculas-en-js/
  */

  /**
  * A function that is used to save the data that is entered in the form and sector.
  */
  guardar() {
    let data= {
      /* Converting the first letter of the name to uppercase and the rest to lowercase. */
      nombre:`${this.sectorForm.value.nombre[0].toUpperCase()}${this.sectorForm.value.nombre.slice(1).toLowerCase()}`,
      estatusId:this.sectorForm.value.estatusId,
      descripcionCorta:`${this.sectorForm.value.descripcionCorta[0].toUpperCase()}${this.sectorForm.value.descripcionCorta.slice(1).toLowerCase()}`,
      sectorId:this.sectorForm.value.sectorId
    }
    let result=this.nombres.indexOf(this.sectorForm.value.nombre.toUpperCase())/* Returning the function. */
    console.log(result);
    console.log(data)
    
    if (this._data.title=='Agregar sector') {
      delete data['estatusId'];
      delete data['sectorId'];
      if(result !== -1){
        this.openSnack('Seleccione otro nombre','Aceptar');
      }else{
        setTimeout(() => {
          this.auxLoading = this._overlay.open();
        }, 1); 
        this._dataService.postData<any>("Sectors/saveSectors", sessionStorage.getItem("token"), data).subscribe(
           data => {
            setTimeout(() => {
             this.auxLoading = this._overlay.close(this.auxLoading);
           }, 1);
           this.dialogRef.close(true)
          },
          error => {
            console.log(error)   
          }
        );
       
      }
      
    } else {
      if(result !== -1 && this.sectorForm.value.estatusId === 1000040){
        console.log(this.sectorForm.value.estatusId)
        this.openSnack('Seleccione otro nombre','Aceptar');
      }else{
        setTimeout(() => {
          this.auxLoading = this._overlay.open();
        }, 1);
        
        this._dataService.postData<SearchSectorsResponse>("Sectors/updateSectors", sessionStorage.getItem("token"), data).subscribe(
          data => {
            console.log("ejecuto guardar"); 
            setTimeout(() => {
              this.auxLoading = this._overlay.close(this.auxLoading);
             }, 1);
             this.dialogRef.close(true)
          },
          error => {
            if (error.error.hasOwnProperty("messageEsp")) {
  
            } 
          }
        );
      }
      
    }
  }

  /**
  * The function that is executed when the user clicks on the save and edit button.
  */
  guardarForm() {
    
  
    let dataForm= {
      /* Converting the first letter of the name to uppercase and the rest to lowercase. */
      nombre:`${this.Form.value.nombre[0].toUpperCase()}${this.Form.value.nombre.slice(1).toLowerCase()}`,
      estatusFormularioId:this.Form.value.estatusFormularioId,
      descripcionCorta:`${this.Form.value.descripcionCorta[0].toUpperCase()}${this.Form.value.descripcionCorta.slice(1).toLowerCase()}`,
      formularioId:this.Form.value.formularioId,
      sectorId:this.Form.value.sectorId
    }
    let result=this._data.nombresForm.indexOf(this.Form.value.nombre.toUpperCase())/* Returning the function. */
    console.log(result);
    console.log(dataForm)


    if (this._data.title=='Agregar formulario') {
      delete dataForm['estatusFormularioId'];
      delete dataForm['formularioId'];
      if(result !== -1){
        this.openSnack('Seleccione otro nombre','Aceptar');
      }else{
        setTimeout(() => {
          this.auxLoading = this._overlay.open();
        }, 1); 
        this._dataService.postData<SearchFormsResponse>("Forms/saveForms", sessionStorage.getItem("token"), dataForm).subscribe(
          data => {
            console.log(data)
            setTimeout(() => {
              this.auxLoading = this._overlay.close(this.auxLoading);
            }, 1);
            this.dialogRef.close(true)
          },
          error => {
              console.log(error)  
          }
        );
      }
    } else {
      
      if(result !== -1 && this.Form.value.estatusFormularioId === 1000043){
        console.log(this.Form.value.estatusFormularioId)
        this.openSnack('Seleccione otro nombre','Aceptar');
      }else{
      setTimeout(() => {
        this.auxLoading = this._overlay.open();
      }, 1);
      
        this._dataService.postData<SearchFormsResponse>("Forms/updateForms", sessionStorage.getItem("token"), dataForm).subscribe(
          data => {
            console.log("ejecuto guardar"); 
            setTimeout(() => {
              this.auxLoading = this._overlay.close(this.auxLoading);
            }, 1);
            this.dialogRef.close(true)
          },
          error => {
            if (error.error.hasOwnProperty("messageEsp")) {

            } 
          }
        );
      }
    }
  }

  /* A function that is used to open a snackbar. */
  openSnack = (message: string, action: string) => {
    this.snack.open(message, action, {
        duration: 5000
    })
  }
 
}
