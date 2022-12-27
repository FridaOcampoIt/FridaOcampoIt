import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { enviroments } from '../../Interfaces/Enviroments/enviroments';

@Component({
  selector: 'app-dialogo-multiples',
  templateUrl: './dialogo-multiples.component.html',
  styleUrls: ['./dialogo-multiples.component.less']
})
export class DialogoMultiplesComponent implements OnInit, AfterViewInit {


  element_data: any[] = [
    { ciu: "1", lote: "lote", nserie: "serie", compania: "compañia", familia: "familia", producto: "producto" },
    { ciu: "1", lote: "lote", nserie: "serie", compania: "compañia", familia: "familia", producto: "producto" },
    { ciu: "1", lote: "lote", nserie: "serie", compania: "compañia", familia: "familia", producto: "producto" },
    { ciu: "1", lote: "lote", nserie: "serie", compania: "compañia", familia: "familia", producto: "producto" },
    { ciu: "1", lote: "lote", nserie: "serie", compania: "compañia", familia: "familia", producto: "producto" },
    { ciu: "1", lote: "lote", nserie: "serie", compania: "compañia", familia: "familia", producto: "producto" },
    { ciu: "1", lote: "lote", nserie: "serie", compania: "compañia", familia: "familia", producto: "producto" },
    { ciu: "1", lote: "lote", nserie: "serie", compania: "compañia", familia: "familia", producto: "producto" }
  ];

  displayedColumns: string[] = ['select', 'ciu', 'lote', 'nserie', 'compania', 'familia', 'producto'];

  dataSource = new MatTableDataSource<any >(this.element_data);
  selection = new SelectionModel<any>(false, []);
  itemsPagina: number[] = enviroments.pageSize;

  //Representa componente de paginación para la busqueda
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //Representa la instancia para el sorting de columnas
  @ViewChild(MatSort) sort: MatSort;

  constructor() { }

  ngOnInit() {
    this.dataSource.sort = this.sort;
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  checkboxLabel(row?: any): string {
    if (!row) {
      return "{this.isAllSelected() ? 'select' : 'deselect'} all";
    }
    return "${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}";
  }

}
