import { Pipe, PipeTransform } from '@angular/core';
interface Family {
    familyProductId: number;
    name: string;
    model: string;
    sku: string;
    gtin: string;
    companyId: number;
}
@Pipe({
  name: 'searchFamily'
})
export class SearchPipeFamily implements PipeTransform {
    transform(familia: Family[], searchTxt: any): Family[] { 
        if(!searchTxt) return familia;
        const result = [];
        for(const fam of familia){
            if(fam.name.toLowerCase().indexOf(searchTxt.toLowerCase()) > -1){
                result.push(fam);
            }
        }
        return result;
    }
}