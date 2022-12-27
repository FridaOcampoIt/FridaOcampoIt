import { Pipe, PipeTransform } from '@angular/core';
interface Compania {
    id: number; 
    name: string;
}
@Pipe({
  name: 'search'
})
export class SearchPipe implements PipeTransform {
    transform(compania: Compania[], searchTxt: any): Compania[] { 
        if(!searchTxt) return compania;
        const result = [];
        for(const com of compania){
            if(com.name.toLowerCase().indexOf(searchTxt.toLowerCase()) > -1){
                result.push(com);
            }
        }
        return result;
    }
}