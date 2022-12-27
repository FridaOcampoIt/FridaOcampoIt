import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild, ActivatedRoute } from '@angular/router';
import { CanActivate } from '@angular/router';

@Injectable({
    providedIn: 'root'
})

export class GuardsAuth implements CanActivate, CanActivateChild {

    constructor(private router: Router, private aRoute: ActivatedRoute) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        var bandera = false;
        //Validacion de los permisos
        switch (route.routeConfig.path) {
            case "CatalogoPerfiles":
                if (sessionStorage.hasOwnProperty("Módulo Perfiles"))
                    bandera = true;
                break;
            case "CatalogoUsuarios":
                if (sessionStorage.hasOwnProperty("Módulo Usuarios"))
                    bandera = true;
                break;
            case "CatalogoCompanias":
                if (sessionStorage.hasOwnProperty("Módulo Compañía"))
                    bandera = true;
                break;
            case "CatalogoFamilias":
                if (sessionStorage.hasOwnProperty("Módulo Familia"))
                    bandera = true;
                break;
            case "CatalogoFamilias/Agregar":
                if (sessionStorage.hasOwnProperty("Agregar Familia"))
                    bandera = true;
                break;
            case "CatalogoFamilias/Editar/:id":
                if (sessionStorage.hasOwnProperty("Editar Familia"))
                    bandera = true;
                break;
            case "CatalogoFamilias/Especificaciones/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Especificación Técnica") )
                    bandera = true;
                break;
            case "CatalogoFamilias/GuiasUso/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Tips & Uso") )
                    bandera = true;
                break;
            case "CatalogoFamilias/GuiasInstalacion/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Guías de Instalación") )
                    bandera = true;
                break;
            case "CatalogoFamilias/GarantiaServicio/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Garantías y Servicios") )
                    bandera = true;
                break;
            case "CatalogoFamilias/ProductosRelacionados/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Productos Relacionados") )
                    bandera = true;
                break;
            case "CatalogoDirecciones":
                if (sessionStorage.hasOwnProperty("Módulo Direcciones"))
                    bandera = true;
                break;
            case "CatalogoProductos":
                if (sessionStorage.hasOwnProperty("Módulo Productos"))
                    bandera = true;
                break;
            case "CatalogoSolicitud":
                if (sessionStorage.hasOwnProperty("Módulo Solicitud de Etiquetas") )
                    bandera = true;
                break;
            case "CatalogoVisor":
                if (sessionStorage.hasOwnProperty("Módulo Visor"))
                    bandera = true;
                break;
            case "ConfiguracionGeneral":
                if (sessionStorage.hasOwnProperty("Módulo Configuración"))
                    bandera = true;
                break;
            default:
                break;
        };

        if (bandera) {
            return bandera;
        } else {

            this.router.navigate(["Home"]);
            return false;
        }
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        var bandera = false;

        //Validacion de los permisos
        switch (route.routeConfig.path) {
            case "Perfiles":
                if (sessionStorage.hasOwnProperty("Módulo Perfiles"))
                    bandera = true;
                break;
            case "Usuarios":
                if (sessionStorage.hasOwnProperty("Módulo Usuarios") )
                    bandera = true;
                break;
            case "Companias":
                if (sessionStorage.hasOwnProperty("Módulo Compañía"))
                    bandera = true;
                break;
            case "Familias":
                if (sessionStorage.hasOwnProperty("Módulo Familia") )
                    bandera = true;
                break;
            case "Familias/Agregar":
                if (sessionStorage.hasOwnProperty("Agregar Familia") )
                    bandera = true;
                break;
            case "Familias/Editar/:id":
                if (sessionStorage.hasOwnProperty("Editar Familia") )
                    bandera = true;
                break;
            case "Familias/Especificaciones/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Especificación Técnica"))
                    bandera = true;
                break;
            case "Familias/GuiasUso/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Tips & Uso"))
                    bandera = true;
                break;
            case "Familias/GuiasInstalacion/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Guías de Instalación"))
                    bandera = true;
                break;
            case "Familias/GarantiaServicio/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Garantías y Servicios"))
                    bandera = true;
                break;
            case "Familias/ProductosRelacionados/:id":
                if (sessionStorage.hasOwnProperty("Visualizar Productos Relacionados"))
                    bandera = true;
                break;
            case "Direcciones":
                if (sessionStorage.hasOwnProperty("Módulo Direcciones") && parseInt(sessionStorage.getItem("isType"), 10) == 0)
                    bandera = true;
                break;
            case "Productos":
                if (sessionStorage.hasOwnProperty("Módulo Productos"))
                    bandera = true;
                break;
            case "Solicitud":
                if (sessionStorage.hasOwnProperty("Módulo Solicitud de Etiquetas") )
                    bandera = true;
                break;
            case "Visor":
                if (sessionStorage.hasOwnProperty("Módulo Visor"))
                    bandera = true;
                break;
            case "Configuracion":
                if (sessionStorage.hasOwnProperty("Módulo Configuración"))
                    bandera = true;
                break;
            default:
                break;
        };

        if (bandera) {
            return bandera;
        } else {
            this.router.navigate(["Home"]);
            return false;
        }
    }
}