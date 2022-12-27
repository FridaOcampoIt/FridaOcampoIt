import { Pipe, PipeTransform } from '@angular/core';
interface Compania {
    id: number; 
    data: string;
}
@Pipe({
  name: 'search'
})
export class SearchPipe implements PipeTransform {
    transform(compania: Compania[], searchTxt: any): Compania[] { 
        if(!searchTxt) return compania;
        const result = [];
        for(const com of compania){
            if(com.data.toLowerCase().indexOf(searchTxt.toLowerCase()) > -1){
                result.push(com);
            }
        }
        return result;
    }
}
/*FILTRAR POR NOMBRE COMPAÑIA  */
@Pipe({
    name: 'filter'
  })
  export class filterpipe implements PipeTransform {
      transform(value:any, arg: any): any {
          const resultPost=[];
          for(const compania of value){
              if(compania.name.toLowerCase().indexOf(arg.toLowerCase()) > -1){
                  resultPost.push(compania);
              }
          }
          return resultPost;
  
      }
  }
  