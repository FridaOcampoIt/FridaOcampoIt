import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from '@angular/router';
import { CanActivate } from '@angular/router';

@Injectable({
    providedIn: 'root'
})

export class GuardsLogin implements CanActivate, CanActivateChild {

    constructor(private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (sessionStorage.hasOwnProperty("idUser")) {

            if (state.url == '/Login') {
                this.router.navigate(["Home"]);
                return false
            }

            return true;
        } else {

            if (state.url == '/Login') {
                sessionStorage.clear();
                return true;
            }

            this.router.navigate(["Login"]);
            sessionStorage.clear();
            return false;
        }
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (sessionStorage.hasOwnProperty("idUser")) {

            if (state.url == '/Login') {
                this.router.navigate(["Home"]);
                return false
            }

            return true;
        } else {

            if (state.url == '/Login') {
                sessionStorage.clear();
                return true;
            }

            this.router.navigate(["Login"]);
            sessionStorage.clear();
            return false;
        }
    }
}
