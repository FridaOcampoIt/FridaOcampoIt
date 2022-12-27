import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { DialogoAgregarComponent } from '../dialogo-agregar/dialogo-agregar.component';
import { DialogoAgregarLinksComponent } from '../dialogo-agregar-links/dialogo-agregar-links.component';
import { ActivatedRoute, Router } from '@angular/router';

//jquery
declare var jquery: any;
declare var $: any;

@Component({
    selector: 'app-productos-relacionados',
    templateUrl: './productos-relacionados.component.html',
    styleUrls: ['./productos-relacionados.component.css'],
    providers: [],
    entryComponents: [DialogoAgregarComponent, DialogoAgregarLinksComponent]
})
export class ProductosRelacionadosComponent implements OnInit {

    familiaId: number = 0;
    constructor(
        private _route: ActivatedRoute,
        private _router: Router,
        private _location: Location)
    {
        this.familiaId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
    }

    back = () => {
        this._router.navigateByUrl('Familias/CatalogoFamilias', { state: { familyId: this.familiaId } })
        // this._location.back();
    }

    ngOnInit() {
    }
}
