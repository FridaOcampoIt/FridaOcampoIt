import { Injectable } from '@angular/core';
import * as FileSaver from 'file-saver';
import * as XLSX from 'xlsx';
import { formatDate } from '@angular/common';

const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet; charset=UTF-8';
const EXCEL_EXT = '.xlsx';

@Injectable({
  providedIn: 'root'
})
export class ExporterService {

  constructor() { }

  exportToExcel(json:any[], excelFileName:string): void{
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(json);
    const workbook: XLSX.WorkBook = { Sheets: { 'data':worksheet },
      SheetNames:['data']
    };
    const excelBuffer:any = XLSX.write(workbook,{bookType:'xlsx',type:'array'});
    //Llamar al metodo (buffer y nombre)
    this.saveAsExcel(excelBuffer, excelFileName);
  }

  private saveAsExcel(buffer:any, fileName:string):void{
    const data: Blob = new Blob([buffer], {type: EXCEL_TYPE});
    FileSaver.saveAs(data, fileName + '_export_' + formatDate(new Date(), 'yyyy-MM-dd hh:mm:ss a', 'en-US', '0600') + EXCEL_EXT);
  }
}
