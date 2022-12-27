import { Pipe, PipeTransform } from '@angular/core';
interface UserByCompanyId {
    id: number;
    nombre: string;
    apellido: string;
    email: string;
    companyId: number;
}
@Pipe({
  name: 'SearchUser'
})
export class SearchPipeUser implements PipeTransform {
    transform(users: UserByCompanyId[], searchTxt: any): UserByCompanyId[] { 
        if(!searchTxt) return users;
        const result = [];
        for(const user of users){
            if((user.nombre.toLowerCase().indexOf(searchTxt.toLowerCase()) > -1) || (user.apellido.toLowerCase().indexOf(searchTxt.toLowerCase()) > -1)){
                result.push(user);
            }
        }
        return result;
    }
}