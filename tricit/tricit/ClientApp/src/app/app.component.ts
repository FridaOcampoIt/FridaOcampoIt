import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Location } from '@angular/common';
import { filter } from 'rxjs/operators';
import { MatSidenav } from '@angular/material';


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    screenWidth: number;

    @ViewChild(MatSidenav) sidenav: MatSidenav;

    constructor(private _router: Router,
        private route: ActivatedRoute,
        private location: Location) {
        this.screenWidth = window.innerWidth;

        

        window.addEventListener('resize', () => {
            this.screenWidth = window.innerWidth;
        });

        this.CheckIn();

        this.urlActual = 0;

        _router.events.pipe(
            filter(event => event instanceof NavigationEnd)
        ).subscribe((evento) => {
            console.log("activated", evento);
            let urlo: string = evento['url'];
            
            if(urlo.includes("/origin")) {
                this.urlActual = 1;
            } else if (urlo.includes("/tracking"))
            {  
                this.urlActual = 2;
             } else {
                this.urlActual = 0;
            }
            console.log("thisurlactual", this.urlActual);

        })



    }

    title = 'app';
    Login = sessionStorage;
    User: string = "";
    urlActual: number = 0;

    logout = () => {
        sessionStorage.clear();
        this.Login = sessionStorage;
        this._router.navigate(["Login"]);
    }

    CheckIn = (e?) => {
        console.log('chekin', this.Login);
        this.User = sessionStorage.getItem("name");
    }

    ngOnInit = () => {


    }

    sideToogle = () => {
        this.sidenav.toggle();
    }


}
