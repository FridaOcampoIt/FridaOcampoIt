import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog, MatSnackBar, MatDialogRef } from '@angular/material';
import { enviroments } from '../../../Interfaces/Enviroments/enviroments';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { Router } from '@angular/router';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';
import { ProductorList } from '../../../Interfaces/Models/ProductorModels';
import { DialogoAgregarEditarProductorComponent } from './dialogo-agregar-editar-productor/dialogo-agregar-editar-productor.component';
import { DialogoEliminarProductorComponent } from './dialogo-eliminar-productor/dialogo-eliminar-productor.component';
import { SelectionModel } from '@angular/cdk/collections';
@Component({
selector: 'app-catalogo-productores',
templateUrl: './catalogo-productores.component.html',
styleUrls: ['./catalogo-productores.component.less']
})
export class CatalogoProductoresComponent implements OnInit {
    
    data_table: Array<ProductorList> = [];
    
    displayedColumns: string[] = [
        'select',
        'numeroProductor',
        'nombreProductor',
        'nombreRancho',
        'nombreAcopio',
        'activo'
    ];
    dataSource = new MatTableDataSource<any>(this.data_table);
    selection = new SelectionModel<any>(false, []);

    /**
    * Variable, para formularios reactivos
    */
    disForm: FormGroup;

    constructor(
        private _fb: FormBuilder,
        private _dataService: DataServices,
        private _overlay: OverlayService,
        private snack: MatSnackBar,
        private _router: Router,
        private _dialog: MatDialog
    ){
    }
    companiaId:number = 0;
    inputSearch: string = '';
    async ngOnInit(){
        this.companiaId = await parseInt(sessionStorage.getItem("company"), 10);
        this.obtenerDatos();
    }

    
    productorId: number;
    numeroProductor: string;
    productor: string;
    status: boolean = true;
    /** The label for the checkbox on the passed row */
    checkboxLabel(row?: any): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }
    //Evento para realizar la seleccion de la tabla
    seleccion = () => {
        this.selection.selected.map(item => {
            this.numeroProductor = item.numeroProductor; 
            this.productor = item.productor, this.status = item.status, this.productorId = item.productorId
        });
        //console.log('Numero productor', (this.numeroProductor), this.productorId, this.productor);
    }
    obtenerDatos(){
        //console.log('Obtiene Datos');
        let data = {
            companiaId : this.companiaId,
            nombreProductorNumeroNombreAcopio: this.inputSearch
        }
        let auxLoading: any;
        setTimeout(() => {
        auxLoading = this._overlay.open();
        }, 1);
        //console.log('DataSave', data); 
        this._dataService.postData<ProductorList>("Productor/searchListProductor", sessionStorage.getItem("token"), data).subscribe(
            data =>{
                //console.log('DATA', data);
                if(data['searchListProductor'] == null){
                    this.openSnack(data['messageEsp'], "Aceptar");
                }else{
                    //console.log('OBTENER datos', data);
                    this.dataSource.data = data['searchListProductor'];
                    this._overlay.close(auxLoading);
                }
            },error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("1");
                } else {
                    //console.log(error);
                }
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1000);
            }
        );
    }
    back = () => {
        this._router.navigateByUrl('EmpacadoEtiquetado/Acopios', { state: { registro: 1 } })
    }
    /**
     * Función para disparar el modal de productores (Agregar/Editar)
     */
    agregarProductor(action: any){
        //Definimos el titulo del modal
        let titulo =  action == 'add' ?  'Agregar' : 'Editar';
        if (this.selection.selected.length > 0 && action == 'edit') {
            //console.log('Entra en abrir edit');
            this.selection.selected.map(item => this.productorId = item.productorId);
        }
        let _dialogRef = this._dialog.open(DialogoAgregarEditarProductorComponent, {
        panelClass: "dialog-aprod",
            data: { 
                action: titulo,
                id: this.productorId
            }
        });
        _dialogRef.afterClosed().subscribe(res => {
            if (res == true) {
                this.obtenerDatos();
            }
        });
    }

    //Función para abrir modal de eliminar productor
    eliminarProductor(id){
        //console.log('Elinamos el acopio con el id', id);
        let _dialogRef = this._dialog.open(DialogoEliminarProductorComponent, {
        panelClass: "dialog-aprod",
            data: {
                id: id
            }
        });
        _dialogRef.afterClosed().subscribe(res => {
            if(res == true){
                this.obtenerDatos();
            }
        });
    }
    
    //Funcion para realizar el proceso del relogin
    relogin(peticion) {
        var requestLogin = new LoginUserRequest();
        requestLogin.user = sessionStorage.getItem("email");
        requestLogin.password = sessionStorage.getItem("password");

        this._dataService.postData<LoginUserResponse>("User/loginUser", "", requestLogin).subscribe(
            data => {
                if (data.messageEsp != "") {
                    sessionStorage.clear();
                    this._router.navigate(['Login']);
                    this.openSnack(data.messageEsp, "Aceptar");
                    return;
                }

                sessionStorage.clear();

                data.userData.userPermissions.forEach((it, id) => {
                    sessionStorage.setItem(it.namePermission, it.permissionId.toString());
                });

                sessionStorage.setItem("token", data.token);
                sessionStorage.setItem("name", data.userData.userData.name);
                sessionStorage.setItem("idUser", data.userData.userData.idUser.toString());
                sessionStorage.setItem("email", requestLogin.user);
                sessionStorage.setItem("password", requestLogin.password);
                sessionStorage.setItem("company", data.userData.userData.company.toString());
                sessionStorage.setItem("isType", data.userData.userData.isType.toString());

                switch (peticion) {
                    case "1":
                        this.obtenerDatos();
                    break;
                    default:
                    break;
                }
            },
            error => {
                sessionStorage.clear();
                this._router.navigate(['Login']);
                this.openSnack("Error al mandar la solicitud", "Aceptar");
                    return;
            }
        )
    }
    openSnack = (message: string, action: string) => {
        this.snack.open(message, action, {
            duration: 5000
        });
    }

}  