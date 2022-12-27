import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef, MatButton, MatProgressBar } from '@angular/material';

import {
    SearchDropDownListFamilyRequest,
    SearchDropDownListFamilyResponse,
    SaveLinkRequest,
    UpdateLinkRequest,
    FamilyProcessResponse
} from '../../../Interfaces/Models/FamilyModels';
import { DataServices } from '../../../Interfaces/Services/general.service'
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';

@Component({
    selector: 'app-dialogo-agregar-links',
    templateUrl: './dialogo-agregar-links.component.html',
    styleUrls: ['./dialogo-agregar-links.component.less']
})
export class DialogoAgregarLinksComponent implements OnInit {

    @ViewChild('anadir') btnAnadir: MatButton;
    @ViewChild('progress') pdfprogress: MatProgressBar;

    constructor(
        @Inject(MAT_DIALOG_DATA) private _data: any,
        private dataService: DataServices,
        private snack: MatSnackBar,
        private dialogRef: MatDialogRef<DialogoAgregarLinksComponent>,
        private _router: Router) { }

    //Variables que recibe al realizar la referencia del dialogo
    action: number = 0;
    idFamily: number = 0;
    title: string = "";
    vinculoId: number = 0;
    sectionType: number = 0;
    subiendo: boolean = false;

    //variables para la vista
    linkId: number = 0;
    titulo: string = "";
    url: string = "";
    autor: string = "";
    recomendadoPor: number = 0;
    PDFReference: string = "";
	PDF: string = "";
	urlPDF: string = "";
    IMGReference: string = "";
    IMG: string = "";
    dropDown = new SearchDropDownListFamilyResponse();

    //Funcion para traer los datos de los combos
    BusquedaCombos() {
        var request = new SearchDropDownListFamilyRequest();
        request.option = 1;

        this.dataService.postData<SearchDropDownListFamilyResponse>("Families/searchDropDownListFamily", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.dropDown = data;
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("BusquedaCombos");
                } else {
                    console.log(error);
                }
            }
        )
    }

    //Funcion para el archivo PDF
    CargarPDF(files) {
        var archivos = files[0];
        this.getBase64(archivos).then(
            data => {
                this.PDF = "Archivo seleccionado"
                this.PDFReference = data.toString();
            }
        );
    }

    //Funcion para el archivo IMG
    CargarIMG(files) {
        var archivos = files[0];
        this.getBase64(archivos).then(
            data => {
                this.IMG = "Imagen seleccionada"
                this.IMGReference = data.toString();
            }
        );
    }

    //Funcion para transformar el archivo PDF/IMG en base 64
    getBase64(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        });
    }

    //Funcion para guardar un link
    guardarLink() {
        //Validación vinculo tipo sitio web seccion "guias de uso y guias de instalacion"
        if ((this.titulo.trim() == "" || this.recomendadoPor == 0 ||
            this.url.trim() == "" || this.autor.trim() == "") && this.vinculoId == 2 && this.sectionType != 5) {
            this.openSnack("Captura los datos del link", "Aceptar");
            return;
        }
        //Validación vinculo tipo PDF seccion "guias de uso y guias de instalacion"
		else if ((this.titulo.trim() == "" || this.urlPDF.trim() == "" || this.recomendadoPor == 0) && this.vinculoId == 1 && this.sectionType != 5) {
            this.openSnack("Captura los datos del PDF", "Aceptar");
            return;
        }
        //Validación vinculo tipo sitio web seccion "Productos relacionados"
        else if ((this.url.trim() == "" || this.titulo.trim() == "" ||
            this.IMGReference == "") && this.vinculoId == 2 && this.sectionType == 5 && this.action == 1) {
            this.openSnack("Captura los datos del link", "Aceptar");
            return;
        }

        //Validación de los archivos tanto en Productos relacionados como en los links de guias de uso e instalación
        if (this.vinculoId == 1 && this.sectionType != 5)
        {
            /*var PDF = this.PDFReference.includes("data:application/pdf;base64,");

            if (!PDF) {
                this.openSnack("Formato de archivo no permitido", "Aceptar");
                return;
            }*/
        }
        else if (this.vinculoId == 2 && this.sectionType == 5 && this.IMGReference != "")
        {
            var PNG = this.IMGReference.includes("png;base64,");
            var JPG = this.IMGReference.includes("jpg;base64,");
            var JPEG = this.IMGReference.includes("jpeg;base64,");

            if (!PNG) {
                if (!JPG) {
                    if (!JPEG) {
                        this.openSnack("Formato de imagen no permitido", "Aceptar");
                        return;
                    }
                }               
            }
        }

        if (this.action == 1) {
            var requestSave = new SaveLinkRequest();
            requestSave.idFamily = this.idFamily;
            requestSave.recommendedById = this.recomendadoPor;
			requestSave.sectionType = this.sectionType;
			requestSave.userCompanyId = parseInt(sessionStorage.getItem("company"));
            requestSave.linkData.push({
                author: this.autor,
                linkId: this.linkId,
                linkTypeId: this.vinculoId,
                status: true,
                thumbailUrl: this.sectionType == 5 ? this.IMGReference : "",
				title: this.titulo,
				url: this.vinculoId == 1 ? this.urlPDF : this.url
            });

            this.subiendo = true;

            this.dataService.postData<FamilyProcessResponse>("Families/saveLink", sessionStorage.getItem("token"), requestSave).subscribe(
                data => {
					if (data.messageEsp != "") {
						//console.log('cant save, there was error', data);
						if (data.messageEsp.endsWith("overLimit"))
							this.openSnack("Ha alcanzado el límite de registros, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
						else
							this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        if(this.sectionType == 1)
                            this.openSnack("Tips & usos guardado con éxito", "Aceptar");
                        else if (this.sectionType == 3)
                            this.openSnack("Guía de instalación guardado con éxito", "Aceptar");
                        else if (this.sectionType == 5)
                            this.openSnack("Producto relacionado guardado con éxito", "Aceptar");

                        this.dialogRef.close(true);
                    }
                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("guardarLink");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendo = false;
                    }
                }
            )
        } else {
            var requestUpdate = new UpdateLinkRequest();
            requestUpdate.recommendedById = this.recomendadoPor;
            requestUpdate.linkData.author = this.autor;
            requestUpdate.linkData.linkId = this.linkId,
            requestUpdate.linkData.linkTypeId = this.vinculoId;
            requestUpdate.linkData.status = true;
            requestUpdate.linkData.thumbailUrl = this.sectionType == 5 ? this.IMGReference : "";
            requestUpdate.linkData.title = this.titulo;
            //requestUpdate.linkData.url = this.url;
			requestUpdate.linkData.url = this.vinculoId == 1 ? this.urlPDF : this.url

            this.subiendo = true;

            this.dataService.postData<FamilyProcessResponse>("Families/updateLink", sessionStorage.getItem("token"), requestUpdate).subscribe(
                data => {
                    if (data.messageEsp != "") {
						if (data.messageEsp.endsWith("overLimit"))
							this.openSnack("Ha alcanzado el límite de registros, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
						else
							this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Tips & usos guardado con éxito", "Aceptar");
                        this.dialogRef.close(true);
                    }

                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("guardarLink");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendo = false;
                    }                    
                }
            )
        }
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
                    case "guardarLink":
                        this.guardarLink();
                        break;
                    case "BusquedaCombos":
                        this.BusquedaCombos();
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
        this.title = this._data.title;
        this.vinculoId = this._data.vinculoId;
        this.sectionType = this._data.sectionType;
        this.idFamily = this._data.idFamily;
        this.action = this._data.action;
        

        if (this.action == 2) {
            this.titulo = this._data.datos.title;
            this.url = this._data.datos.url;
            this.autor = this._data.datos.author;
            this.recomendadoPor = this._data.datos.recommendedById;
            this.linkId = this._data.datos.linkId;
        }

        this.BusquedaCombos();
    }
}
