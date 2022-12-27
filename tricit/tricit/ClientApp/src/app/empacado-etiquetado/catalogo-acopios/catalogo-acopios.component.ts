import { Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';
import { Router, ActivatedRoute } from '@angular/router';
import { DataServices } from '../../Interfaces/Services/general.service';
import { OverlayService } from '../../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { LoginUserResponse, LoginUserRequest } from '../../Interfaces/Models/LoginModels';
import { DialogoAgregarEditarAcopioComponent } from './dialogo-agregar-editar-acopio/dialogo-agregar-editar-acopio.component';
import { DialogoEliminarAcopioComponent } from './dialogo-eliminar-acopio/dialogo-eliminar-acopio.component';


import { searchAcopioResponse } from '../../Interfaces/Models/AcopioModels';
@Component({
  selector: 'app-catalogo-acopios',
  templateUrl: './catalogo-acopios.component.html',
  styleUrls: ['./catalogo-acopios.component.less']
})
export class CatalogoAcopiosComponent implements OnInit, AfterViewInit {

    data_table: Array<searchAcopioResponse> = [];
    
    displayedColumns: string[] = [
        'select',
        'numeroAcopio',
        'nombreAcopio',
        'activo',
        'paisNombre',
        'estadoNombre'
    ];
    dataSource = new MatTableDataSource<any>(this.data_table);
    selection = new SelectionModel<any>(false, []);

    //Representa componente de paginación para la busqueda
    @ViewChild(MatPaginator) paginator: MatPaginator;
    //Representa la instancia para el sorting de columnas
    @ViewChild(MatSort) sort: MatSort;

    
    constructor(
        private _router: Router,
        private _route: ActivatedRoute,
        private _dialog: MatDialog,
        private _dataService: DataServices,
        private _overlay: OverlayService,
        private snack: MatSnackBar
    ){}
    companiaId: number = 0;
    ngOnInit(): void {
        this.companiaId =   parseInt(sessionStorage.getItem("company"), 10);
        this.getListAcopio();
    }
    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }
    busquedaGenerica: string;
    //Funcion para recuperar los acopios registrados
    getListAcopio() { 
        let data: any = {
            companiaId: this.companiaId,
            nombreNumeroAcopio: this.busquedaGenerica == null || this.busquedaGenerica == null ? '' : this.busquedaGenerica
        };
        let auxLoading: any;
        setTimeout(() => {
          auxLoading = this._overlay.open();
         }, 1);
        this._dataService.postData<searchAcopioResponse>("Acopio/searchListAcopios", sessionStorage.getItem("token"), data).subscribe(
            data => {
                //console.log('Rsponse data', data);
                if(data["messageEsp"]  != null){
                    this.openSnack(data["messageEsp"],"Aceptar");
                }
                else if (data["searchListAcopio"]) {
                    //console.log('Data', data['searcListadoAcopio']);
                    this.dataSource.data = data["searchListAcopio"];
                }
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1);
            },error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("1");
                } else {
                    //console.log(error);
                }
                setTimeout(() => {
                    this._overlay.close(auxLoading);
                },1);
            }
        );
    }
    
    acopioId: number;
    numeroAcopio: string;
    nombreAcopio: string;
    status: boolean = true;
    /** The label for the checkbox on the passed row */
    checkboxLabel(row?: any): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }
    acopioName: string
    //Evento para realizar la seleccion de la tabla
    seleccion = () => {
        this.selection.selected.map(item => {
            this.numeroAcopio = item.numeroAcopio; this.nombreAcopio = item.nombreAcopio, this.status = item.status, this.acopioId = item.acopioId
        });
    }
    back = () => {
        this._router.navigateByUrl('EmpacadoEtiquetado', { state: { registro: 1 } })
    }

    verActividad = () => {
        this._router.navigateByUrl('EmpacadoEtiquetado/Acopios/'+this.nombreAcopio+'/'+this.acopioId, { state: { registro: 1 } })
    }

    catalogoProductores() {
        this._router.navigateByUrl('EmpacadoEtiquetado/Acopios/GestionDeProductores');
    }

    agregaEditarAcopioDialog(action: any){
        //Definimos el titulo del modal
        let titulo =  action == 'add' ?  'Agregar' : 'Editar';
        
        if (this.selection.selected.length > 0 && action == 'edit') {
            this.selection.selected.map(item => this.acopioId = item.acopioId);
        }

        let _dialogRef = this._dialog.open(DialogoAgregarEditarAcopioComponent, {
        panelClass: "dialog-aprod",
            data: {
                id: this.acopioId,
                action: titulo
            }
        });

        _dialogRef.afterClosed().subscribe(res => {
            if (res == true) {
                this.getListAcopio();
            }
        });
    }

    eliminarAcopio(id){
        //console.log('Elinamos el acopio con el id', id);
        let _dialogRef = this._dialog.open(DialogoEliminarAcopioComponent, {
        panelClass: "dialog-aprod",
            data: {
                id: id
            }
        });
        _dialogRef.afterClosed().subscribe(res => {
            if(res == true){
                this.getListAcopio();
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
                        this.getListAcopio();
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
    generarExcel(){}

}