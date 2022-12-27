import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar, MatSort } from '@angular/material';

import { DataServices } from '../Interfaces/Services/general.service';
import {
    SearchFamilyProductRequest,
    SearchFamilyProductResponse,
    ProductFamily,
    SearchDropDownListFamilyRequest,
    SearchDropDownListFamilyResponse,
    DeleteFamilyRequest,
    FamilyProcessResponse
} from '../Interfaces/Models/FamilyModels'
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';
import { Router, ActivatedRoute, NavigationStart } from '@angular/router';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import { Location } from '@angular/common';
import { map, filter } from 'rxjs/operators';
import { Observable } from 'rxjs';

import { faBoxes } from "@fortawesome/free-solid-svg-icons";
@Component({
    selector: 'app-familias',
    templateUrl: './familias.component.html',
    styleUrls: ['./familias.component.css']
})
export class FamiliasComponent implements OnInit, AfterViewInit {
    selectedValue: any;
    searchTxt: any;
    @ViewChild('search') searchElement: ElementRef;
    doSomething(any){
        setTimeout(()=>{ // this will make the execution after the above boolean has changed
            this.searchElement.nativeElement.focus();
        },0);  
    }
    constructor(
        private dataServices: DataServices,
        private _dialog: MatDialog,
        private snack: MatSnackBar,
        private _router: Router,
        private _overlay: OverlayService,
        private _activatedRoute: ActivatedRoute) { }

    @ViewChild(MatPaginator) paginator: MatPaginator;

    //Representa la instancia para el sorting de columnas
    @ViewChild(MatSort) sort: MatSort;

    //Modelos para la vista
    idFamily: number = 0;
    companyId: number = 0;
    company: number = (sessionStorage.getItem("company") === null ? -1 : parseInt(sessionStorage.getItem("company")));
    companyUs: number = (sessionStorage.getItem("company") === null ? -1 : parseInt(sessionStorage.getItem("company")));
    family: string = "";
    response = new SearchFamilyProductResponse();
    dropDown = new SearchDropDownListFamilyResponse();
    itemsPagina: number[] = enviroments.pageSize;
    allowEspec: boolean = sessionStorage.hasOwnProperty('Visualizar Especificación Técnica');
    allowTips: boolean = sessionStorage.hasOwnProperty('Visualizar Tips & Uso');
    allowGuides: boolean = sessionStorage.hasOwnProperty('Visualizar Guías de Instalación');
    allowWarr: boolean = sessionStorage.hasOwnProperty('Visualizar Garantías y Servicios');
    allowRel: boolean = sessionStorage.hasOwnProperty('Visualizar Productos Relacionados');
    allowConfiguracion : boolean = sessionStorage.hasOwnProperty('Visualizar Configuración de Embalaje');
    allowConfiguracionRepro : boolean = sessionStorage.hasOwnProperty('Visualizar Configuración de Embalaje Reproceso');

    /**
     * iconos
     */
    faBoxes = faBoxes;

    //Variables para el manejo de la tabla
    displayedColumns: string[] = ['select', 'name', 'model', 'sku', 'gtin'];
    dataSource = new MatTableDataSource<ProductFamily>(this.response.productFamilyData);
    selection = new SelectionModel<any>(false, []);
    overlayRef: OverlayRef;
    emptyList: boolean = false;

    state: any = {};
    ///METODOS DE LA VISTA

    /** Whether the number of selected elements matches the total number of rows. */
    isAllSelected() {
        const numSelected = this.selection.selected.length;
        const numRows = this.dataSource.data.length;
        return numSelected === numRows;
    }

    /** Selects all rows if they are not all selected; otherwise clear selection. */
    masterToggle() {
        this.isAllSelected() ?
            this.selection.clear() :
            this.dataSource.data.forEach(row => this.selection.select(row));
    }

    /** The label for the checkbox on the passed row */
    checkboxLabel(row?: ProductFamily): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }

    //Evento para realizar la seleccion de la tabla
    seleccion = () => {
        this.selection.selected.map(item => { this.idFamily = item.familyProductId , this.companyId = item.companyId});
    }

    //Funcion para eliminar la familia
    eliminarFamilia(bandera: number) {
        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true,
                data: {
                    palabra: "la familia"
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                console.log(result);

                if (result) {
                    var request = new DeleteFamilyRequest();
                    request.familyId = this.idFamily;
                    this.dataServices.postData<FamilyProcessResponse>("Families/deleteFamily", sessionStorage.getItem("token"), request).subscribe(
                        data => {
                            console.log(data);
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");

                            } else {
                                this.openSnack("Familia eliminada con éxito", "Aceptar");
                                this.Busqueda();

                            }
                        },
                        error => {
                            //console.log(error);
                            if (error.error.hasOwnProperty("messageEsp")) {
                                this.relogin("eliminarFamilia");

                            } else {
                                this.openSnack("Error al mandar la solicitud", "Aceptar");
                                this.openSnack(error.messageEsp, "Aceptar");

                            }
                        }
                    )
                }
            });
        } else {
            var request = new DeleteFamilyRequest();
            request.familyId = this.idFamily;

            this.dataServices.postData<FamilyProcessResponse>("Families/deleteFamily", sessionStorage.getItem("token"), request).subscribe(
                data => {
                    //console.log(data);
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");

                    } else {
                        this.openSnack("Familia eliminada con éxito", "Aceptar");
                        this.Busqueda();

                    }
                },
                error => {
                    console.log(error);
                    if (error.error.hasOwnProperty("messageEsp")) {
                        //this.relogin("eliminarFamilia");
                        if (error.messageEsp != null) {
                            this.openSnack(error.error.messageEsp, "Aceptar");
                            //console.log('true');
                        } else {
                            this.openSnack(error.error.messageEsp, "Aceptar");
                            //this.openSnack('No tienes los permisos para realizar esta acción', "Aceptar");
                            //console.log('false');
                        }

                    } else {
                        //console.log(sessionStorage);
                        //this.openSnack("Error al mandar la solicitud", "Aceptar");
                        //this.openSnack(error.messageEsp, "Aceptar");
                        if (error.messageEsp != null) {
                            this.openSnack(error.messageEsp, "Aceptar");
                            console.log('verdader');
                        } else {
                            this.openSnack('Error al mandar la solicitud', "Aceptar");
                            console.log('falso');
                        }

                    }
                }
            )
        }
    }

    //Funcion para realizar la busqueda de combos
    BusquedaCombos() {
        var request = new SearchDropDownListFamilyRequest();
        request.option = 1;
        request.idCompany = this.company;
        this.dataServices.postData<SearchDropDownListFamilyResponse>("Families/searchDropDownListFamily", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.dropDown = data
                this.Busqueda();
                //console.log('data-----' + data);

                this.dropDown.companyData = data.companyData.filter(x => x.data != "");
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("BusquedaCombos");
                } else {
                    console.log(error);
                }
            }
        )
    }

    //Funcion para realizar la busqueda
    Busqueda() {
        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);
        var request = new SearchFamilyProductRequest();
        request.companyId = this.company;
        request.name = this.family;

        this.dataServices.postData<SearchFamilyProductResponse>("Families/searchFamilyProduct", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.response = data;
                if (this.response.productFamilyData.length > 0)
                    this.emptyList = false;
                else
                    this.emptyList = true;
                this.dataSource.data = this.response.productFamilyData;
                this.selection.clear();

                if (this.state.hasOwnProperty('familyId')) {
                    this.loadSelected();
                }
                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
                console.warn('data all', data);
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("Busqueda");
                } else {
                    console.log(error);
                }

                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
            }
        )
    }

    //Funcion para realizar el proceso del relogin
    relogin(peticion) {
        var requestLogin = new LoginUserRequest();
        requestLogin.user = sessionStorage.getItem("email");
        requestLogin.password = sessionStorage.getItem("password");

        this.dataServices.postData<LoginUserResponse>("User/loginUser", "", requestLogin).subscribe(
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
                    case "eliminarFamilia":
                        this.eliminarFamilia(2);
                        break;
                    case "Busqueda":
                        this.Busqueda();
                        break;
                    case "BusquedaCombos":
                        this.BusquedaCombos();
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

    //Funcion para abrir el modal del mensaje
    openSnack = (message: string, action: string) => {
        this.snack.open(message, action, {
            duration: 5000
        })
    }

    ngOnInit() {
        this.BusquedaCombos();
        this.dataSource.sort = this.sort;
        this.state = window.history.state;
        console.log("state init", this.state);
    }

    loadSelected = () => {
        console.log("state loadSelected", this.state);
        let selected: any[] = [];
        selected.push(this.dataSource.data.find(sel => sel.familyProductId == this.state.familyId));

        console.log("selected", selected);

        this.idFamily = this.state.familyId;
        this.companyId = selected[0].companyId;
        this.selection = new SelectionModel<any>(false, selected);
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }
}
