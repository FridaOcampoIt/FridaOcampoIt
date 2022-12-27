import { Component, Inject, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MatPaginator, MatSort, MatTableDataSource, MAT_DIALOG_DATA } from '@angular/material';
import { CompaniesData, SearchCompanyRequest, SearchCompanyResponse } from '../../../Interfaces/Models/CompanyModels';
import { OverlayRef } from '@angular/cdk/overlay';
import { enviroments } from "../../../Interfaces/Enviroments/enviroments";
import { OverlayService } from '../../../Interfaces/Services/overlay.service';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-dialog-asignar-formulario',
  templateUrl: './dialog-asignar-formulario.component.html',
  styleUrls: ['./dialog-asignar-formulario.component.less']
})
export class DialogAsignarFormularioComponent implements OnInit {

  typesOfShoes: string[] = ['Boots', 'Clogs', 'Loafers', 'Moccasins', 'Sneakers'];
  @Input()
  valor:number;
  filterPost="";
    //Variables para la vista
    name: string = "";
    businessName: string = "";
    idCompany: number = 0;
    response = new SearchCompanyResponse();
    itemsPagina: number[] = enviroments.pageSize;
    overlayRef: OverlayRef;
    emptyList: boolean = false;

    //Representa componente de paginación para la busqueda
    @ViewChild(MatPaginator) paginator: MatPaginator;

    //Representa la instancia para el sorting de columnas
    @ViewChild(MatSort) sort: MatSort;

    //variables para configurar la tabla
    displayedColumns: string[] = ['name', 'select'];
    dataSource = new MatTableDataSource<CompaniesData>(this.response.companiesDataList);
    selection = new SelectionModel<CompaniesData>(false, []);

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
    checkboxLabel(row?: CompaniesData): string {
        if (!row) {
            return "{this.isAllSelected() ? 'select' : 'deselect'} all";
        }
        return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
    }
    


  constructor( 
    @Inject(MAT_DIALOG_DATA)
    private _data: any,
    private dialogRef: MatDialogRef<DialogAsignarFormularioComponent>,
    private _overlay: OverlayService,
    private dataService: DataServices,) { }

  ngOnInit() {
    this.valor=this._data.valor;
    this.searchCompany();
  }

  searchCompany() {
    var request = new SearchCompanyRequest();
        request.name = this.name;
        request.businessName = this.businessName;
        this.idCompany = 0;

        setTimeout(() => {
            this.overlayRef = this._overlay.open();
        }, 1);

        this.dataService.postData<SearchCompanyResponse>("Companies/searchCompany", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.response = data;
                console.log(this.response.companiesDataList)
                if (this.response.companiesDataList.length > 0)
                    this.emptyList = false;
                else
                    this.emptyList = true;
                this.dataSource.data = this.response.companiesDataList.filter(x => x.name != "");
                this.selection.clear();
                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                   
                } else {
                    console.log(error);
                }
                setTimeout(() => {
                    this._overlay.close(this.overlayRef);
                }, 1);

            }
        );
  }

  asignarCompania(seleccion){
    var request = new SearchCompanyRequest();
    request.name = this.name;
    request.businessName = this.businessName;
    this.idCompany = 0;

    setTimeout(() => {
        this.overlayRef = this._overlay.open();
    }, 1);

    this.dataService.postData<SearchCompanyResponse>("Companies/searchCompany", sessionStorage.getItem("token"), request).subscribe(
        data => {
            this.response = data;
            console.log(data)
            console.log(this.response.companiesDataList)
            if (this.response.companiesDataList.length > 0)
                this.emptyList = false;
            else
                this.emptyList = true;
            this.dataSource.data = this.response.companiesDataList.filter(x => x.name != "");
            this.selection.clear();
            setTimeout(() => {
                this._overlay.close(this.overlayRef);
            }, 1);
        },
        error => {
            if (error.error.hasOwnProperty("messageEsp")) {
               
            } else {
                console.log(error);
            }
            setTimeout(() => {
                this._overlay.close(this.overlayRef);
            }, 1);

        }
    );

  }

}
