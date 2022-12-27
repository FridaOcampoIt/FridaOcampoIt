import { Pipe, PipeTransform } from '@angular/core';
interface Compania {
    id: number; 
    data: string;
}
@Pipe({
  name: 'searchData'
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