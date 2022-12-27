import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { SwiperComponent, SwiperConfigInterface } from 'ngx-swiper-wrapper';
import { MatDialog, MatSnackBar } from '@angular/material';
import { DialogoAgregarComponent } from '../../dialogo-agregar/dialogo-agregar.component';
import { DialogoAgregarLinksComponent } from '../../dialogo-agregar-links/dialogo-agregar-links.component';

import {
    SearchLinkFamilyResponse,
    SearchLinkFamilyRequest,
    DeleteLinkRequest,
    FamilyProcessResponse
} from '../../../../Interfaces/Models/FamilyModels'
import { DataServices } from '../../../../Interfaces/Services/general.service'
import { DialogoEliminarComponent } from '../../../dialogo-eliminar/dialogo-eliminar.component';
import { LoginUserRequest, LoginUserResponse } from '../../../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';

@Component({
    selector: 'app-swiper-videos-GI',
    templateUrl: './swiper-videos.component.html',
    styleUrls: ['./swiper-videos.component.less'],
    entryComponents: [DialogoAgregarComponent, DialogoAgregarLinksComponent]
})
export class SwiperVideosGIComponent implements OnInit {

    constructor(
        private _dialog: MatDialog,
        private dataServices: DataServices,
        private snack: MatSnackBar,
        private _router: Router) { }

    @ViewChild('slideVideos') swiperVideos?: SwiperComponent;

    //Variables para la vista
    response = new SearchLinkFamilyResponse();
    idActual: number = 0;
	@Input() idFamily: number;
	canAdd = sessionStorage.hasOwnProperty('Agregar Guías de Instalación');

    confiVideos: SwiperConfigInterface = {
        direction: 'horizontal',
        observer: true,
        spaceBetween: 5,
        slidesPerView: 4,
        centeredSlides: false,
        navigation: false,
        init: false,
        allowTouchMove: true,
        breakpoints: {
            // when window width is <= 320px
            576: {
                slidesPerView: 1,
                spaceBetween: 10
            },
            // when window width is <= 480px
            763: {
                slidesPerView: 2,
                spaceBetween: 20
            },
            // when window width is <= 640px
            994: {
                slidesPerView: 3,
                spaceBetween: 30
            }
        }
    }

    nextV = () => {
        this.swiperVideos.directiveRef.nextSlide();
    }

    prevV = () => {
        this.swiperVideos.directiveRef.prevSlide();
    }

    destr = () => {
        this.swiperVideos.directiveRef.swiper().destroy(false, true);
    }

    //Funcion para eliminar un video
    eliminarVideo(id: number, bandera: number) {
        this.idActual = id;
        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true,
                data: {
                    palabra: "el video"
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    var requestDelete = new DeleteLinkRequest();
                    requestDelete.linkId = id;

                    this.dataServices.postData<FamilyProcessResponse>("Families/deleteLink", sessionStorage.getItem("token"), requestDelete).subscribe(
                        data => {
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");
                            } else {
                                this.openSnack("Video eliminado con éxito", "Aceptar");
                                this.busqueda();
                            }
                        },
                        error => {
                            if (error.error.hasOwnProperty("messageEsp")) {
                                this.relogin("busqueda");
                            } else {
                                this.openSnack("Error al mandar la solicitud", "Aceptar");
                            }
                        }
                    )
                }
            });
        }
        else {
            var requestDelete = new DeleteLinkRequest();
            requestDelete.linkId = id;

            this.dataServices.postData<FamilyProcessResponse>("Families/deleteLink", sessionStorage.getItem("token"), requestDelete).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Video eliminado con éxito", "Aceptar");
                        this.busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("busqueda");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            )
        }
    }

    //Funcion para realizar la busquda la busqueda 
    busqueda() {
        var request = new SearchLinkFamilyRequest();
        request.familyId = this.idFamily;
        request.linkType = 3;
        request.sectionType = 3;

        this.dataServices.postData<SearchLinkFamilyResponse>("Families/searchLinkFamily", sessionStorage.getItem("token"), request).subscribe(
            data => {
				this.response = data;
				this.openSnack("Su paquete contratado cuenta con " + data.linkFamilies.limitsFamily.nVid + " videos y " + data.linkFamilies.limitsFamily.npdf + " links para archivos PDF’s", "Aceptar");
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("busqueda");
                } else {
                    console.log(error);
                }
            }
        )
    }

    //Funcion para abrir el video en una ventana nueva
    abrirVideo(urlId: string) {
        window.open("https://www.youtube.com/watch?v=" + urlId);
    }

    //Funcion para agregar un nuevo video
    AgregarVideo = (action: number, title: string) => {
        if (action === 1) {
            const dialogRef = this._dialog.open(DialogoAgregarComponent, {
                panelClass: "dialog-videos",
                disableClose: true,
                data: {
                    title: title,
                    action: action,
                    idFamily: this.idFamily,
                    sectionType: 3
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                this.busqueda();
            });
        }
        else {
            const dialogRef = this._dialog.open(DialogoAgregarComponent, {
                panelClass: "dialog-videos",
                disableClose: true,
                data: {
                    title: title,
                    action: action,
                    idFamily: this.idFamily,
                    sectionType: 3,
                    videos: this.response.linkFamilies
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                this.busqueda();
            });
        }
    }

    //Funcion para abrir el modal del mensaje
    openSnack = (message: string, action: string) => {
        this.snack.open(message, action, {
            duration: 5000
        })
    }

    //Funcion para realizar el proceso del relogin
    relogin(peticion) {
        var requestLogin = new LoginUserRequest();
        requestLogin.user = sessionStorage.getItem("email");
        requestLogin.password = sessionStorage.getItem("password");

        this.dataServices.postData<LoginUserResponse>("User/loginUser", "", requestLogin).subscribe(
            data => {
                if (data.messageEsp != "") {
                    sessionStorage.clear();
                    this._router.navigate(['Login']);
                    this.openSnack(data.messageEsp, "Aceptar");
                    return;
                }

                sessionStorage.clear();

                data.userData.userPermissions.forEach((it, id) => {
                    sessionStorage.setItem(it.namePermission, it.permissionId.toString());
                });

                sessionStorage.setItem("token", data.token);
                sessionStorage.setItem("name", data.userData.userData.name);
                sessionStorage.setItem("idUser", data.userData.userData.idUser.toString());
                sessionStorage.setItem("email", requestLogin.user);
                sessionStorage.setItem("password", requestLogin.password);
                sessionStorage.setItem("company",data.userData.userData.company.toString());
                sessionStorage.setItem("isType", data.userData.userData.isType.toString());

                switch (peticion) {
                    case "busqueda":
                        this.busqueda();
                        break;
                    case "eliminarVideo":
                        this.eliminarVideo(this.idActual, 2);
                        break;
                    default:
                        break;
                }
            },
            error => {
                sessionStorage.clear();
                this._router.navigate(['Login']);
                this.openSnack("Error al mandar la solicitud", "Aceptar");
                return;
            }
        )
    }

    ngOnInit() {
        this.busqueda();
    }

    ngAfterViewInit() {
        this.swiperVideos.directiveRef.init();
        this.swiperVideos.directiveRef.update();
    }
}
