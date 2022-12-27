import { Component, OnInit, Input } from '@angular/core';
import { MatSidenav } from '@angular/material';

export interface Menu {
    name: string;
    state: string;
    type: string;
    tooltip: string;
}

@Component({
    selector: 'app-sidenav',
    templateUrl: './sidenav.component.html',
    styleUrls: ['./sidenav.component.css']
})
export class SidenavComponent implements OnInit {

    secactive: any;
    active: boolean;
    menu: Menu[] = [];
    @Input() parent: MatSidenav;
    @Input() wid: number;

    constructor() {
        if (sessionStorage.hasOwnProperty("Módulo Perfiles")) {
            this.menu.push({
                name: "Perfiles",
                state: '/Perfiles/CatalogoPerfiles',
                type: 'link',
                tooltip: 'Agrega y gestiona perfiles de usuario'
            });
        }

        if (sessionStorage.hasOwnProperty("Módulo de Estadísticas")) {
            this.menu.push({
                name: "Estadísticas",
                state: '/Home',
                type: 'link',
                tooltip: 'Estadísticas'
            });
        }
        if (sessionStorage.hasOwnProperty("Módulo Usuarios")) {
            this.menu.push({
                name: "Usuarios",
                state: '/Usuarios/CatalogoUsuarios',
                type: 'link',
                tooltip: 'Agrega y gestiona usuarios'
            });
        }
        if (sessionStorage.hasOwnProperty("Módulo Compañía")) {
            this.menu.push({
                name: "Compañías",
                state: '/Companias/CatalogoCompanias',
                type: 'link',
                tooltip: 'Agrega y gestiona compañías'
            });
        }

        if(sessionStorage.hasOwnProperty("Módulo Empacado y Etiquetado")){
            this.menu.push({
                name: "Empacado y Etiquetado",
                state: '/EmpacadoEtiquetado',
                type: 'link',
                tooltip: "Agrega y gestiona Empacado"
            });
        }

        if (sessionStorage.hasOwnProperty("Módulo Familia")) {
            this.menu.push({
                name: "Familias",
                state: '/Familias/CatalogoFamilias',
                type: 'link',
                tooltip: 'Agrega y gestiona las familias, garantias, guías, especificaciones y productos relacionados'
            });
        }

        if (sessionStorage.hasOwnProperty("Módulo Direcciones") && parseInt(sessionStorage.getItem("isType"), 10) == 0) {
            this.menu.push({
                name: "Direcciones",
                state: '/Direcciones/CatalogoDirecciones',
                type: 'link',
                tooltip: 'Agrega y gestiona las direcciones'
            });
        }

        if(sessionStorage.hasOwnProperty("Módulo Direcciones de Proveedor")){
            this.menu.push({
                name: "Direcciones de proveedor",
                state: '/DireccionesProveedores/CatalogoDirecciones',
                type: 'link',
                tooltip: "Agrega y gestiona líneas"
            });
        }

        if (sessionStorage.hasOwnProperty("Módulo Productos")) {
            this.menu.push({
                name: "Productos",
                state: '/Productos/CatalogoProductos',
                type: 'link',
                tooltip: 'Agrega, importa y gestiona productos'
            });
        }

        if (sessionStorage.hasOwnProperty("Módulo Solicitud de Etiquetas")) {
            this.menu.push({
                name: "Solicitud Etiquetas",
                state: '/Solicitud/CatalogoSolicitud',
                type: 'link',
                tooltip: 'Agrega y da seguimiento a la solicitud de etiquetas'
            });
        }

        if (sessionStorage.hasOwnProperty("Módulo Visor")) {
            this.menu.push({
                name: "Visor de reportes",
                state: '/Visor/CatalogoVisor',
                type: 'link',
                tooltip: 'Agrega y consulta reportes de robo'
            });
        }

        if (sessionStorage.hasOwnProperty("Módulo Configuración")) {
            this.menu.push({
                name: "Configuración",
                state: '/Configuracion/ConfiguracionGeneral/Sector',
                type: 'link',
                tooltip: 'Administra la configuración del sistema'
            });
        }
        if(sessionStorage.hasOwnProperty("Módulo Rastreo de Código")){
            this.menu.push({
                name: "Rastreo de código",
                state: '/Rastreo',
                type: 'link',
                tooltip: ""
            });
        }

        if (parseInt(sessionStorage.getItem("isType"), 10) == 0 && parseInt(sessionStorage.getItem("company"), 10) != 0 && sessionStorage.hasOwnProperty("Módulo Etiquetas")) {
            this.menu.push({
                name: "Etiquetas",
                state: '/Etiquetas',
                type: 'link',
                tooltip: ""
            });
        }


        //this.menu.push(
        //    {
        //        name: "Perfiles",
        //        state: '/Perfiles',
        //        type: 'link',
        //    }, {
        //        name: "Usuarios",
        //        state: '/Usuarios',
        //        type: 'link',
        //    }, {
        //        name: "Compañías",
        //        state: '/Companias',
        //        type: 'link',
        //    },
        //    {
        //        name: "Familias",
        //        state: '/Familias',
        //        type: 'link',
        //    },
        //    {
        //        name: "Direcciones",
        //        state: '/Direcciones',
        //        type: 'link',
        //    },
        //    {
        //        name: "Productos",
        //        state: '/Productos',
        //        type: 'link',
        //    },
        //    {
        //        name: "Solicitud Etiquetas",
        //        state: '/Solicitud',
        //        type: 'link',
        //    },
        //    {
        //        name: "Visor",
        //        state: '/Visor',
        //        type: 'link',
        //    },
        //{
        //    name: "Visor",
        //    state: '/Home',
        //    type: 'toggle',
        //    pages: [{
        //        name: 'Agentes',
        //        type: 'link',
        //        state: '/Home',
        //        icon: ''
        //    }, {
        //        name: 'Agentes',
        //        type: 'link',
        //        state: '/Home',
        //        icon: ''
        //    }, {
        //        name: 'Agentes',
        //        type: 'link',
        //        state: '/Home',
        //        icon: ''
        //    }
        //        , {
        //        name: 'Agentes',
        //        type: 'link',
        //        state: '/Home',
        //        icon: ''
        //    }
        //        , {
        //        name: 'Agentes',
        //        type: 'link',
        //        state: '/Home',
        //        icon: ''
        //    }
        //        , {
        //        name: 'Agentes',
        //        type: 'link',
        //        state: '/Home',
        //        icon: ''
        //    }, {
        //        name: "Familias",
        //        state: '/Familias',
        //        type: 'link',
        //    }, 
        //    {
        //        name: "inicio",
        //        state: '/Home',
        //        type: 'toggle',
        //        pages: [{
        //            name: 'Agentes',
        //            type: 'link',
        //            state: '/Home',
        //            icon: ''
        //        }, {
        //            name: 'Agentes',
        //            type: 'link',
        //            state: '/Home',
        //            icon: ''
        //        }]
        //    }
        //    ]
        //},
        //{
        //    name: "Configuración",
        //    state: '/Configuracion',
        //    type: 'link',
        //});
    }

    ngOnInit() {
        this.active = false;
    }

    cambiarActivo = (objActive) => {

        if (objActive !== undefined) {

            if (objActive.list === this.secactive) {

                this.secactive.classList.toggle('oculto');
                this.secactive.classList.toggle('parent-active');

            } else {

                if (this.secactive !== undefined) {

                    this.secactive.classList.add('oculto', 'parent-active');
                    objActive.list.classList.remove('oculto', 'parent-active');

                } else {
                    objActive.list.classList.remove('oculto', 'parent-active');
                }

            }
        }

        this.secactive = objActive.list;
    }
}
