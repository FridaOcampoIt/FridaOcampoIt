import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar } from '@angular/material';

import { DialogoAgregarComponent } from './dialogo-agregar/dialogo-agregar.component';
import { DialogoImportarComponent } from './dialogo-importar/dialogo-importar.component';
import { DialogoEliminarComponent } from './dialogo-eliminar/dialogo-eliminar.component';
import { DataServices } from '../Interfaces/Services/general.service';
import { Router } from '@angular/router';
import {
    SearchProductDropDownRequest,
    SearchProductDropDownResponse,
    SearchProductDataResponse,
    ProductDataRequest,
    SearchProductsRequest,
    SearchProductsResponse,
    ProcessResponse,
    ProductData,

    ProductIdList
} from '../Interfaces/Models/Product';
import { LoginUserRequest, LoginUserResponse } from '../Interfaces/Models/LoginModels';
import { forEach } from '@angular/router/src/utils/collection';
import { enviroments } from '../Interfaces/Enviroments/enviroments';
import { OverlayService } from '../Interfaces/Services/overlay.service';
import { OverlayRef } from '@angular/cdk/overlay';
import * as XLSX from 'xlsx';
//import { request } from 'http';

export interface Producto {
    // udid: string;
    ciu: string;
    family: string;
    productId: number;
}

/*const ELEMENT_DATA: Producto[] = [

    { productId: 0, ciu: 'AS8D359912', familia: 'Familia 1'}
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
    { udid: 'AS8D359912', familia: 'Familia 1' },
]*/

const initialSelection = [];
const allowMultiSelect = false;

@Component({
    selector: 'app-productos',
    templateUrl: './productos.component.html',
    styleUrls: ['./productos.component.less']
})
export class ProductosComponent implements OnInit, AfterViewInit {
    constructor(
        private _dialog: MatDialog,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private _router: Router,
        private _overlay: OverlayService
    ) { }

    companiaAplicada: any = {};
    familiaImportacion: number = 0;
    combosResponse = new SearchProductDropDownResponse();
    family: number = 0;
    udid: string = "";
    resultImport: string = "";
    idProduct: number = 0;
    idCompany: number = parseInt(sessionStorage.getItem("company"));
    response = new SearchProductsResponse();
    responseImportList: any = [];
    permissionReGenerate = sessionStorage.hasOwnProperty("Re - generar etiquetas");
    itemsPagina: number[] = enviroments.pageSize;
    displayedColumns: string[] = ['select', 'ciu', 'qr', 'udid', 'family'];
    dataSource = new MatTableDataSource<ProductData>(this.response.products);
    selection = new SelectionModel<ProductData>(true, []);
    overlayRef: OverlayRef;
    emptyList: boolean = false;
    companies : any[] = [];

    @ViewChild(MatPaginator) paginator: MatPaginator;

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
    checkboxLabel(row?: any): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }

    agregarProducto = (edit) => {
        if (edit == 0) {
            var title = "Agregar";
            this.idProduct = 0;//to avoid loading a selected product to edit if the user click the add product button
        } else {
            //if (this.selection.selected.length > 0) {
            this.selection.selected.map(item => this.idProduct = item.productId);
            title = "Editar";
        }
        //}else{

        //}

        const dialogRef = this._dialog.open(DialogoAgregarComponent, {
            panelClass: "dialog-aprod",
            disableClose: true,
            data: {
                id: this.idProduct,
                title: title
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            this.idProduct = 0;
            // setTimeout(() => this.Busqueda(), 500);
        });
    }

    importarProductos = () => {

        if (!this.companiaAplicada.hasOwnProperty("nombre")) {
            this.openSnack("Seleccione una compañia", "OK");
            return;
        }

        const dialogRef = this._dialog.open(DialogoImportarComponent, {
            panelClass: 'dialog-aprod',
            disableClose: true,
            data: { companiaId: this.companiaAplicada["id"], compania: this.companiaAplicada["nombre"] }
        });

        dialogRef.afterClosed().subscribe(result => {
            //console.log(result, 'resultado udid');
            this.idProduct = 0;
            this.resultImport = result;
            if (result) {
                // true -> archivo, false -> cantidad
                // this.familiaImportacion = result["familia"];
                // if (result.tipo) {
                //     setTimeout(() => this.Busqueda(1), 500);
                // } else {
                //     setTimeout(() => this.Busqueda(2), 500);
                // }
            }
        });
    }

    BusquedaCombos(action = 0) {
        var request = new SearchProductDropDownRequest();
        request.company = this.idCompany;//parseInt(localStorage.getItem("company"));

        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);

        this.dataService.postData<SearchProductDropDownResponse>("Product/searchProductDropDown", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.combosResponse = data;
                this.companies = this.combosResponse.companyDropDown.filter(x => x['data'] != "");
                console.log("idaplicado antes", this.idCompany);
                if (action == 1) {
                    if (this.idCompany != 0) {

                        this.companiaAplicada = { id: this.idCompany, nombre: this.combosResponse.companyDropDown.filter(ref => ref['id'] == this.idCompany)[0]['data'] };
                        console.log("compania", this.companiaAplicada);
                        console.log(this.combosResponse.companyDropDown, 'comapnias');
                        console.log("idaplicado despues", this.idCompany);

                    }

                }

                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
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

    Busqueda(action = 0) {
        var request = new SearchProductsRequest();
        request.udid = this.udid;
        request.idFamily = this.family;

        if (request.idFamily == 0) {
            this.openSnack("Seleccione una familia", "Ok");
            return;
        }

        if (!this.overlayRef.hasAttached() || this.overlayRef === null) {
            setTimeout(() => {
                this.overlayRef = this._overlay.open();
            }, 1);
        }


        this.dataService.postData<SearchProductsResponse>("Product/searchProduct", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.paginator.pageSize = 20;
                this.response = data;
                if (this.response.products.length > 0)
                    this.emptyList = false;
                else
                    this.emptyList = true;
                this.dataSource = new MatTableDataSource<ProductData>(this.response.products);
                if (action == 1) { //importación por archivo
                    let arrayRes = this.resultImport["insertado"].split(',');
                    let arrayTemp: any = [];
                    this.responseImportList = [];
                    arrayRes.forEach(item => {
                        arrayTemp = this.response.products.filter(produc => produc.udid == item);
                        if (arrayTemp.length) {
                            arrayTemp.forEach(element => {
                                this.responseImportList.push(element);
                            });
                        }
                    });
                    this.selection = new SelectionModel<ProductData>(true, this.responseImportList);
                    this.exportarQrs(1);
                } else if (action == 2) { //importación cantidad
                    let arrayRes = this.resultImport["insertado"].split(',');
                    let arrayTemp: any = [];
                    this.responseImportList = [];
                    arrayRes.forEach(item => {
                        arrayTemp = this.response.products.filter(produc => produc.ciu == item);
                        if (arrayTemp.length) {
                            arrayTemp.forEach(element => {
                                this.responseImportList.push(element);
                            });
                        }
                    });
                    //console.log("this.responseImportList: ", this.responseImportList);
                    this.selection.clear();
                    this.selection = new SelectionModel<ProductData>(true, this.responseImportList);
                    this.exportarQrs(1);
                }
                else {
                    this.selection = new SelectionModel<ProductData>(true, []);
                }
                this.dataSource.paginator = this.paginator;
                //this.selection.
                //console.log(this.response);

                //Dispose/Terminar la Referencia del overlay
                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("Busqueda");
                } else {
                    //console.log(error);
                }
                //Dispose/Terminar la Referencia del overlay
                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
            }
        );
    }

    //Funcion para eliminar la dirección
    eliminarProducto() {
        // if (bandera == 1) {
        const dialogRef = this._dialog.open(DialogoEliminarComponent, {
            width: 'dialog-comp',
            disableClose: true,
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                var requestDelete = new ProductDataRequest();
                this.selection.selected.map(item => requestDelete.idProduct = item.productId);

                this.dataService.postData<ProcessResponse>("Product/deleteProduct", sessionStorage.getItem("token"), requestDelete).subscribe(
                    data => {
                        if (data.messageEsp != "") {
                            this.openSnack(data.messageEsp, "Aceptar");
                        } else {
                            this.openSnack("Producto eliminado con éxito", "Aceptar");
                            this.Busqueda();
                        }
                    },
                    error => {
                        if (error.error.hasOwnProperty("messageEsp")) {
                            this.relogin("eliminarProducto");
                        } else {
                            this.openSnack("Error al mandar la solicitud", "Aceptar");
                        }
                    }
                )
            }
        });
        /*}else{
            var requestDelete = new ProductDataRequest();
            this.selection.selected.map(item => requestDelete.idProduct = item.productId);

            this.dataService.postData<ProcessResponse>("Address/deleteAddress", sessionStorage.getItem("token"), requestDelete).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Producto eliminado con éxito", "Aceptar");
                        this.Busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("eliminar");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            )
        }*/
    }
    regenerarEtiqueta() {
        var idList = [];
        //console.log(this.selection.selected);
        for (var i in this.selection.selected) {
            idList[i] = this.selection.selected[i].productId;
        }
        //console.log(idList);
        var request = new ProductIdList();
        request.idProducts = idList;
        this.dataService.postData<ProcessResponse>("Product/generateProductCode", sessionStorage.getItem("token"), request).subscribe(
            data => {
                if (data.messageEsp != "") {
                    this.openSnack(data.messageEsp, "Aceptar");
                } else {
                    this.openSnack("Regeneración creada exitosamente, correo enviado", "Aceptar");
                    this.Busqueda();
                }
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("regenerarEtiqueta");
                } else {
                    this.openSnack("Error al mandar la solicitud", "Aceptar");
                }
            }
        )
    }
    //Funcion para realizar el proceso del relogin
    relogin(peticion) {
        var requestLogin = new LoginUserRequest();
        requestLogin.user = sessionStorage.getItem("email");
        requestLogin.password = sessionStorage.getItem("password");

        this.dataService.postData<LoginUserResponse>("User/loginUser", "", requestLogin).subscribe(
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
                    case "BusquedaCombos":
                        this.BusquedaCombos();
                        break;
                    case "Busqueda":
                        this.Busqueda();
                        break;
                    case "eliminarProducto":
                        this.eliminarProducto();
                        break;
                    case "regenerarEtiqueta":
                        this.regenerarEtiqueta();
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
        //this.selection.isMultipleSelection = true;
        this.BusquedaCombos();
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }

    exportarQrs(action = 0) {
        debugger;


        //cajas
        let cajasobj: any[] = [];
        let familia;
        //obtener el nombre de la famiñia
        if (action == 1) {
            //si es por importación
            familia = this.combosResponse.familyDropDown.filter(reg => reg.id == this.familiaImportacion)[0]['data'];
        } else {
            //si es por boton de generar QR
            familia = this.combosResponse.familyDropDown.filter(reg => reg.id == this.family)[0]['data'];
        }
        let compania = this.companiaAplicada['nombre'];

        if (this.selection.selected.length == 0 || this.selection.selected.length === undefined) {
            this.openSnack("No hay registros", "Aceptar");
            return;
        }

        for (let index = 0; index < this.selection.selected.length; index++) {
            cajasobj.push({
                "QR": `https://data.traceit.net/origin?ciu=${this.selection.selected[index].ciu}`,
                "CIU": this.selection.selected[index].ciu
            })
        }

        let titulo: any = {
            "empresa": `${compania}_${familia}`
        };

        //crear  hoja
        const wsCajas: XLSX.WorkSheet = XLSX.utils.json_to_sheet(cajasobj);
        XLSX.utils.json_to_sheet(titulo);

        //crear libro
        const wb: XLSX.WorkBook = XLSX.utils.book_new();

        XLSX.utils.book_append_sheet(wb, wsCajas, "QRs CIUS");

        XLSX.writeFile(wb, `${compania}_${familia}.xlsx`);



    }

    enableQrs() {
        if (this.selection.selected.length == 0 || this.family == 0 || this.idCompany == 0) {
            return true;
        } else {
            return false;
        }
    }
}
