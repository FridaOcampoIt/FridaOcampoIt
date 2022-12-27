import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { DialogoAgregarComponent } from '../dialogo-agregar/dialogo-agregar.component';
import { DialogoAgregarLinksComponent } from '../dialogo-agregar-links/dialogo-agregar-links.component';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-guias-uso',
    templateUrl: './guias-uso.component.html',
    styleUrls: ['./guias-uso.component.css'],
    providers: [],
    entryComponents: [DialogoAgregarComponent, DialogoAgregarLinksComponent]
})
export class GuiasUsoComponent implements OnInit {

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
