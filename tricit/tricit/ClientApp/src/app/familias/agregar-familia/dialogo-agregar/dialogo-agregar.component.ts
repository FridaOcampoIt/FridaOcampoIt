import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef, MatDialog } from '@angular/material';

import {
    SearchDropDownListFamilyRequest,
    SearchDropDownListFamilyResponse,
    YoutubeSearchResponse,
    YoutubeSearchRequest,
    YoutubeData,
    linkData,
    SaveLinkRequest,
    FamilyProcessResponse,
    DeleteLinkRequest,
    UpdateLinkRequest
} from '../../../Interfaces/Models/FamilyModels'
import { DataServices } from '../../../Interfaces/Services/general.service'
import { DialogoEliminarComponent } from '../../dialogo-eliminar/dialogo-eliminar.component';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';

@Component({
    selector: 'app-dialogo-agregar',
    templateUrl: './dialogo-agregar.component.html',
    styleUrls: ['./dialogo-agregar.component.less']
})
export class DialogoAgregarComponent implements OnInit, AfterViewInit {

    constructor(
        @Inject(MAT_DIALOG_DATA) private _data: any,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private dialogRef: MatDialogRef<DialogoAgregarComponent>,
        private _dialog: MatDialog,
        private _router: Router) { }

    idFamily: number = 0;
    sectionType: number = 0;
    banderaEliminacion: boolean = true;
    titulo: string = "";
    accionId: number = 0; 
    busqueda: string = "";
    recomendadoPor: number = 0;
    recomendadoPorFiltro: string = "";
    dropDown = new SearchDropDownListFamilyResponse();
    youtubeResponse = new YoutubeSearchResponse();
    youtubeResponseTemp: YoutubeData[] = [];
    checkAll: boolean = false;

    //Videos para gestionarlos
    videosAgregar: linkData[] = [];

    //Funcion para traer los datos de los combos
    BusquedaCombos() {
        var request = new SearchDropDownListFamilyRequest();
        request.option = 1;

        this.dataService.postData<SearchDropDownListFamilyResponse>("Families/searchDropDownListFamily", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.dropDown = data;
            },
            error => {
                console.log(error);
            }
        )
    }

    //Funcion para realizar la busqueda hacia la api de youtube
    busquedaYoutube() {
        if (this.busqueda == "") {
            this.openSnack("Captura un filtro para la búsqueda", "Aceptar");
            return;
        }

        var request = new YoutubeSearchRequest();
        request.dataFilter = this.busqueda;

        this.dataService.postData<YoutubeSearchResponse>("Families/youtubeSearch", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.youtubeResponse = data;
                this.youtubeResponseTemp = data.youtubeData;
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("busquedaYoutube");
                } else {
                    console.log(error);
                }
            }
        );
    }

    //Funcion para abrir el video en una ventana nueva
    abrirVideo(urlId: string) {
        window.open("https://www.youtube.com/watch?v=" + urlId);
    }

    //Funcion para mostrar solo los seleccionados en el combo
    filtraCombo(filtro: string = "") {
        this.videosAgregar = [];
        this.checkAll = false;
        this.youtubeResponseTemp = this.youtubeResponse.youtubeData.filter(element => {
            return this.recomendadoPorFiltro == "All" ? element : (element.recommendedBy === this.recomendadoPorFiltro);
        });
    }

    //Funcion para realizar el filtrado de los videos por recomendacion
    filtraPor() {
        this.videosAgregar = [];
        if(this.checkAll) {
            this.youtubeResponseTemp.forEach(element => {
                const filterVideo = this._data.videos.filter(x => { return (x.url == element.videoId); });
                this.videosAgregar.push({
                    linkId: filterVideo[0].linkId,
                    author: element.channelTitle,
                    linkTypeId: 3,
                    status: true,
                    thumbailUrl: element.thumbnails,
                    title: element.title,
                    url: element.videoId,
                });
            });
        }
    }

    //Funcion para agregar videos con el ckeck
    agregarVideo(video: YoutubeData, check) {
        let contador = 0;
        if(contador == 0) {
            if (this.accionId == 1) {
                if (check.checked) {
                    this.videosAgregar.push({
                        linkId: 0,
                        author: video.channelTitle,
                        linkTypeId: 3,
                        status: true,
                        thumbailUrl: video.thumbnails,
                        title: video.title,
                        url: video.videoId,
                    });
                } else {
                    for (var i = 0; i < this.videosAgregar.length; i++) {
                        if (this.videosAgregar[i].url == video.videoId) {
                            this.videosAgregar.splice(i, 1);
                        }
                    }
                }            
            }
            else {
                if (check.checked) {
                    this._data.videos.forEach((it, id) => {
                        if (video.videoId == it.url) {
                            this.videosAgregar.push({
                                linkId: it.linkId,
                                author: video.channelTitle,
                                linkTypeId: 3,
                                status: true,
                                thumbailUrl: video.thumbnails,
                                title: video.title,
                                url: video.videoId,
                            });
                        }
                    });
                }
                else {
                    for (var i = 0; i < this.videosAgregar.length; i++) {
                        if (this.videosAgregar[i].url == video.videoId) {
                            this.videosAgregar.splice(i, 1);
                        }
                    }
                }
            }
        }
        contador++;
    }

    //Funcion para guardar los videos 
    guardarVideos() {
        if (this.recomendadoPor == 0) {
            this.openSnack("Selecciona la recomendacion del video", "Aceptar");
            return;
        }

        if (this.videosAgregar.length == 0) {
            this.openSnack("Selecciona los videos que se desean agregar", "Aceptar");
            return;
        }

        var requestSave = new SaveLinkRequest();
        requestSave.idFamily = this.idFamily;
        requestSave.recommendedById = this.recomendadoPor;
        requestSave.sectionType = this.sectionType;
        requestSave.linkData = this.videosAgregar;

        this.dataService.postData<FamilyProcessResponse>("Families/saveLink", sessionStorage.getItem("token"), requestSave).subscribe(
            data => {
                if (data.messageEsp != "") {
					if (data.messageEsp.endsWith("overLimit"))
						this.openSnack("Ha alcanzado el límite de registros, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
					else
						this.openSnack(data.messageEsp, "Aceptar");
                } else {
                    if (this.sectionType == 1)
                        this.openSnack("Tips & usos guardado con éxito", "Aceptar");
                    else if (this.sectionType == 3)
                        this.openSnack("Guía de instalación guardado con éxito", "Aceptar");

                    this.dialogRef.close(true);
                }
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("guardarVideos");
                } else {
                    this.openSnack("Error al mandar la solicitud", "Aceptar");
                }                
            }
        )
    }

    //Funcion para eliminar masivamente los videos
    eliminarSeleccionados(bandera: number) {
        if (bandera == 1) {
            if (this.videosAgregar.length == 0) {
                this.openSnack("Selecciona los videos que se desea eliminar", "Aceptar");
                return;
            }

            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true,
                data: {
                    palabra: "los videos"
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    this.banderaEliminacion = true;
                    var banderaCancelacion: boolean = false;

                    this.videosAgregar.forEach((it, id) => {
                        if (banderaCancelacion) {
                            return;
                        }

                        var requestDelete = new DeleteLinkRequest();
                        requestDelete.linkId = it.linkId;

                        this.dataService.postData<FamilyProcessResponse>("Families/deleteLink", sessionStorage.getItem("token"), requestDelete).subscribe(
                            data => {
                                if (data.messageEsp != "") {
                                    this.banderaEliminacion = false;
                                } else {
                                    for (var i = 0; i < this.youtubeResponse.youtubeData.length; i++) {
                                        if (this.youtubeResponse.youtubeData[i].videoId == it.url) {
                                            this.youtubeResponse.youtubeData.splice(i, 1);
                                            break;
                                        }
                                    }
                                    for (var i = 0; i < this.youtubeResponseTemp.length; i++) {
                                        if (this.youtubeResponseTemp[i].videoId == it.url) {
                                            this.youtubeResponseTemp.splice(i, 1);
                                            break;
                                        }
                                    }
                                }
                            },
                            error => {
                                if (error.error.hasOwnProperty("messageEsp")) {
                                    this.relogin("eliminarSeleccionados");
                                    banderaCancelacion = true;
                                } else {
                                    console.log(error);
                                    this.banderaEliminacion = false;
                                }
                            }
                        )
                    });
                }
            });
        }
        else {
            this.banderaEliminacion = true;
            var banderaCancelacion: boolean = false;

            this.videosAgregar.forEach((it, id) => {
                if (banderaCancelacion) {
                    return;
                }

                var requestDelete = new DeleteLinkRequest();
                requestDelete.linkId = it.linkId;

                this.dataService.postData<FamilyProcessResponse>("Families/deleteLink", sessionStorage.getItem("token"), requestDelete).subscribe(
                    data => {
                        if (data.messageEsp != "") {
                            this.banderaEliminacion = false;
                        } else {
                            for (var i = 0; i < this.youtubeResponse.youtubeData.length; i++) {
                                if (this.youtubeResponse.youtubeData[i].videoId == it.url) {
                                    this.youtubeResponse.youtubeData.splice(i, 1);
                                    break;
                                }
                            }
                            for (var i = 0; i < this.youtubeResponseTemp.length; i++) {
                                if (this.youtubeResponseTemp[i].videoId == it.url) {
                                    this.youtubeResponseTemp.splice(i, 1);
                                    break;
                                }
                            }
                        }
                    },
                    error => {
                        if (error.error.hasOwnProperty("messageEsp")) {
                            this.relogin("eliminarSeleccionados");
                            banderaCancelacion = true;
                        } else {
                            console.log(error);
                            this.banderaEliminacion = false;
                        }
                    }
                )
            });
        }        
    }

    //Funcion para cambiar de Recomendado los videos
    actualizarVideos() {
        if (this.recomendadoPor == 0) {
            this.openSnack("Selecciona la recomendacion del video", "Aceptar");
            return;
        }

        if (this.videosAgregar.length == 0) {
            this.openSnack("Selecciona los videos que se desean agregar", "Aceptar");
            return;
        }
        let contador = 0;
        let contador2 = 0;
        var banderaCancelacion: boolean = false;
        this.videosAgregar.forEach(element => {
            if (banderaCancelacion) {
                return;
            }

            var requestUpdate = new UpdateLinkRequest();
                requestUpdate.recommendedById = this.recomendadoPor;
                requestUpdate.linkData.author = element.author;
                requestUpdate.linkData.linkId = element.linkId,
                requestUpdate.linkData.linkTypeId = element.linkTypeId;
                requestUpdate.linkData.status = true;
                requestUpdate.linkData.thumbailUrl = element.thumbailUrl;
                requestUpdate.linkData.title = element.title;
                requestUpdate.linkData.url = element.url;
                this.dataService.postData<FamilyProcessResponse>("Families/updateLink", sessionStorage.getItem("token"), requestUpdate).subscribe(
                    data => {
                        contador2++;
                        if (data.messageEsp != "") {
                            console.log(data.messageEsp);
                        } else {
                            contador++;
                        }
                    },
                    error => {
                        if (error.error.hasOwnProperty("messageEsp")) {
                            banderaCancelacion = true;
                            this.relogin("actualizarVideos");
                        } else {
                            contador2++;
                            console.log(error);
                            this.openSnack("Error al mandar la solicitud", "Aceptar");
                        }
                    },
                    () => {
                        let msg = "";
                        if(contador2 >= this.videosAgregar.length) {
                            if(contador == this.videosAgregar.length) {
                                if (this.sectionType == 1)
                                    msg ="Tips & usos guardados con éxito";
                                else if (this.sectionType == 3)
                                    msg ="Guías de instalación guardados con éxito";
                            }
                            if(contador < this.videosAgregar.length && contador > 0) {
                                if (this.sectionType == 1)
                                    msg ="Hay problemas para guardar algunos Tips & usos";
                                else if (this.sectionType == 3)
                                    msg = "Hay problemas para guardar algunas guías";
                            }
                            if(contador == 0) {
                                if (this.sectionType == 1)
                                    msg = "Error al guardar los Tips & usos";
                                else if (this.sectionType == 3)
                                    msg = "Error al guardar las guías";
                            }

                            this.openSnack(msg, "Aceptar");                            
                            this.dialogRef.close(true);                            
                        }
                    }
                )
        });
    }

    //Funcion para realizar el proceso del relogin
    relogin(peticion) {
        var requestLogin = new LoginUserRequest();
        requestLogin.user = sessionStorage.getItem("email");
        requestLogin.password = sessionStorage.getItem("password");

        this.dataService.postData<LoginUserResponse>("User/loginUser", "", requestLogin).subscribe(
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

                switch (peticion)
                {
                    case "actualizarVideos":
                        this.actualizarVideos();
                        break;
                    case "eliminarSeleccionados":
                        this.eliminarSeleccionados(2);
                        break;
                    case "guardarVideos":
                        this.guardarVideos();
                        break;
                    case "busquedaYoutube":
                        this.busquedaYoutube();
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

    //Funcion para abrir el modal del mensaje
    openSnack = (message: string, action: string) => {
        this.snack.open(message, action, {
            duration: 5000
        })
    }

    ngOnInit() {
        this.titulo = this._data.title;
        this.accionId = this._data.action;
        this.idFamily = this._data.idFamily;
        this.sectionType = this._data.sectionType;

        this.BusquedaCombos();
    }

    ngAfterViewInit() {
        if (this.accionId == 2) {
            this._data.videos.forEach((it, id) => {
                this.youtubeResponse.youtubeData.push({
                    channelTitle: it.author,
                    thumbnails: it.thumbailUrl,
                    title: it.title,
                    videoId: it.url,
                    recommendedBy: it.recommendedBy
                });
                this.youtubeResponseTemp.push({
                    channelTitle: it.author,
                    thumbnails: it.thumbailUrl,
                    title: it.title,
                    videoId: it.url,
                    recommendedBy: it.recommendedBy
                });
            });
        } 
    }
}
