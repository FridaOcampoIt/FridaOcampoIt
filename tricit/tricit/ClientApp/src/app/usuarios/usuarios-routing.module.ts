import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsuariosComponent } from './usuarios.component';
import { GuardsAuth } from '../Interfaces/Guards/GuardsAuth.guard';


const usuariosRoutes: Routes = [
    {
        path: 'CatalogoUsuarios', component: UsuariosComponent, canActivate: [GuardsAuth]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(usuariosRoutes)
    ],
    exports: [RouterModule]
})
export class UsuariosRoutingModule { }
