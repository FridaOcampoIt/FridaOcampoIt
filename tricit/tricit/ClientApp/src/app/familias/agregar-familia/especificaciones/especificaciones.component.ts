import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { MatSnackBar, MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

//DataServices
import { DataTreeEspecification, FlatNode } from '../../../Interfaces/Enviroments/dataClassSource';
import {
    SearchTechnicalSpecificationFamilyRequest,
    SearchTechnicalSpecificationFamilyResponse,
    SaveTechnicalSpecificationRequest,
    FamilyProcessResponse,
    UpdateTechnicalSpecificationRequest,
    SaveTechnicalSpecificationDetailsRequest,
    UpdateTechnicalSpecificationDetailsRequest,
    DeleteTechnicalSpecificationRequest,
    DeleteTechnicalSpecificationDetailsRequest,
    SearchFamilyProductDateRequest,
    SearchFamilyProductDateResponse,
    UpdateFamilyRequest
} from '../../../Interfaces/Models/FamilyModels';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { DialogoEliminarComponent } from '../../dialogo-eliminar/dialogo-eliminar.component';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';

@Component({
    selector: 'app-especificaciones',
    templateUrl: './especificaciones.component.html',
    styleUrls: ['./especificaciones.component.css']
})
export class EspecificacionesComponent implements OnInit, AfterViewInit {
    sWidth: number;
    @ViewChild('fileInputEsp') file: ElementRef;

    //Variables para la vista
    familiaId: number = 0;

    banderaTitulo: boolean = true; //Bandera para mostrar el formulario de titulo
    banderaSubtitulo: boolean = false; //Bandera para mostrar el formulario de subtitulo
    banderaEdicion: boolean = false; //Bandera para detonar la accion que se esta tratando de hacer(Editar y Agregar)
    banderaEdicionImagen: boolean = false; //Bandera para detonar si se realizo la accion de remplazo de la imagen

    //Variables para agregar titulos
    tituloEspanol: string = "";
    tituloIngles: string = "";
    idParent: number = 0;

    //Variables para los subtitulos
    subtituloIngles: string = "";
    subtituloEspanol: string = "";
    informacionEspanol: string = "";
    informacionIngles: string = "";
    ImagenPrevistaEspecificacion: string = '';
    idChildren: number = 0;

    //Referencia a los id que se estan seleccionando
    idParentSelected: number = 0;
    idChildrenSelected: number = 0;

    //Variables para el mapeo de datos
    response = new SearchTechnicalSpecificationFamilyResponse();
	responseFamily = new SearchFamilyProductDateResponse();

	//the limits
	familySpecCharLimit: number = 0;
	specCharUsed: number = 0;
	imageLimit: number = 0;
	imgUsed: number = 0;

    //Funciones para construir el arbol
    private transformer = (node: DataTreeEspecification, level: number) => {
        return {
            expandable: !!node.children && node.children.length > 0,
            name: node.title,
            level: level,
            id: node.id,
            idParent: node.idPaaarent,
            parent: node.parent
        };
    }
    nodeActual: FlatNode;

    //Validaciones para la construccion
    dataTree: DataTreeEspecification[] = [];
    treeControl = new FlatTreeControl<FlatNode>(node => node.level, node => node.expandable);
    treeFlattener = new MatTreeFlattener(this.transformer, node => node.level, node => node.expandable, node => node.children);
    dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);
    isParent = (_: number, node: FlatNode) => node.parent;

    constructor(
        private _route: ActivatedRoute,
        private snack: MatSnackBar,
        private dataServices: DataServices,
        private _dialog: MatDialog,
        private _location: Location,
        private _router: Router) {
        this.dataSource.data = this.dataTree;
        this.sWidth = window.innerWidth;
        window.addEventListener('resize', () => {
            this.sWidth = window.innerWidth;
        })

        let id = this._route.snapshot.paramMap.get('id');

        if (id != undefined || id != null)
            this.familiaId = parseInt(id, 10);
    }

    //Funcion para la busqueda de los datos de la familia en caso de realizar una edicion 
    BusquedaDatosFamilias() {
        var request = new SearchFamilyProductDateRequest();
        request.familyId = this.familiaId;

        this.dataServices.postData<SearchFamilyProductDateResponse>("Families/searchFamilyProductDate", sessionStorage.getItem("token"), request).subscribe(
            data => {
				this.responseFamily = data;
				this.openSnack("Su paquete contratado cuenta con " + this.responseFamily.productFamily.limitsFamily.nCharEspec + " caracteres y " + this.responseFamily.productFamily.limitsFamily.nImg + " imágenes", "Aceptar");
				this.specCharUsed = data.productFamily.specCharUse.totalUsed;
				this.familySpecCharLimit = data.productFamily.limitsFamily.nCharEspec;
				this.imageLimit = data.productFamily.limitsFamily.nImg;
				this.imgUsed = data.productFamily.imageUse.totalUsed;
				console.log("images used: " + this.imgUsed);
				//console.log("Su paquete contratado cuenta con " + this.responseFamily.productFamily.limitsFamily.nCharEspec + " caracteres y " + this.responseFamily.productFamily.limitsFamily.nImg + " imágenes", "Aceptar");
				console.log(this.responseFamily.productFamily);
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("BusquedaDatosFamilias");
                } else {
                    console.log(error);
                }
            }
        )
    }

    //Funcion para la imagen de especificacion
    ImagenEspecificacion(files) {
        var archivos = files[0];
        this.getBase64(archivos).then(
            data => {
                this.banderaEdicionImagen = true;
                this.ImagenPrevistaEspecificacion = data.toString();
            }
        );
    }

    //Funcion para quitar la imagen
    eliminarImagen() {
        this.banderaEdicionImagen = true;
        this.ImagenPrevistaEspecificacion = "";
    }

    //Funcion para transformar la imagen en base 64
    getBase64(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        });
    }

    //Funcion para activar el formulario de agregar titulo
    anadirTitulo() {
        this.banderaSubtitulo = false;
        this.banderaTitulo = true;
        this.banderaEdicionImagen = false;
        this.banderaEdicion = false;
        this.idChildrenSelected = 0;
        this.idParentSelected = 0;

        this.tituloEspanol = "";
        this.tituloIngles = "";
        this.ImagenPrevistaEspecificacion = "";
        this.informacionEspanol = "";
        this.informacionIngles = "";
        this.subtituloEspanol = "";
        this.subtituloIngles = "";
    }

    //Funcion para activar el formulario de agregar subtitulos
    anadirSubtitulo(node: FlatNode) {
        this.banderaSubtitulo = true;
        this.banderaTitulo = false;
        this.banderaEdicionImagen = false;
        this.banderaEdicion = false;
        this.idChildrenSelected = 0;

        this.tituloEspanol = "";
        this.tituloIngles = "";

        this.ImagenPrevistaEspecificacion = "";
        this.informacionEspanol = "";
        this.informacionIngles = "";
        this.subtituloEspanol = "";
        this.subtituloIngles = "";

        this.idParentSelected = node.id;
    }

    //Funcion para registrar en el objeto los datos obtenidos
    registrar() {
        //Agregar un nuevo titulo
        if (this.banderaTitulo && !this.banderaEdicion) {
            if (this.tituloEspanol.trim() == "" && this.tituloIngles.trim() == "") {
                this.openSnack("Captura el título en español y/o inglés", "Aceptar");
            }
            else {
                var requestSave = new SaveTechnicalSpecificationRequest();
                requestSave.familyId = this.familiaId;
                requestSave.technicalSpecification.title = this.tituloEspanol;
				requestSave.technicalSpecification.titleEnglish = this.tituloIngles;

				if (this.familySpecCharLimit < (this.specCharUsed + this.tituloEspanol.length + this.tituloIngles.length)) {
					this.openSnack("Ha alcanzado el límite de caracteres, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
					console.log("too much length");
					return;
				}
				//else
					//console.log("not very lengthy", this.familySpecCharLimit, (this.specCharUsed + this.tituloEspanol.length + this.tituloIngles.length));


                this.dataServices.postData<FamilyProcessResponse>("Families/saveTechnicalSpecification", sessionStorage.getItem("token"), requestSave).subscribe(
                    data => {
						if (data.messageEsp != "") {
							if (data.messageEsp.endsWith("overLimit"))
								this.openSnack("Ha alcanzado el límite de caracteres, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
							else
								this.openSnack(data.messageEsp, "Aceptar");
                        } else {
                            this.openSnack("Especificación guardada con éxito", "Aceptar");

                            this.banderaSubtitulo = false;
                            this.banderaTitulo = true;
                            this.banderaEdicion = false;
                            this.banderaEdicionImagen = false;
                            this.file.nativeElement.value = "";

                            this.tituloEspanol = "";
                            this.tituloIngles = "";
                            this.informacionEspanol = "";
                            this.informacionIngles = "";
                            this.subtituloEspanol = "";
                            this.subtituloIngles = "";
                            this.ImagenPrevistaEspecificacion = "";
                            this.idParentSelected = 0;
                            this.idChildrenSelected = 0;

                            this.Busqueda();
                        }
                    },
                    error => {
                        if (error.error.hasOwnProperty("messageEsp")) {
                            this.relogin("registrar");
                        } else {
                            this.openSnack("Error al mandar la solicitud", "Aceptar");
                        }
                    }
                )
            }
        }
        //Editar titulo
        else if (this.banderaTitulo && this.banderaEdicion) {
            if (this.tituloEspanol.trim() == "" && this.tituloIngles.trim() == "") {
                this.openSnack("Captura el título en español y/o inglés", "Aceptar");
            }
            else {
                var requestUpdate = new UpdateTechnicalSpecificationRequest();
                requestUpdate.technicalSpecification.specificationTechnicalId = this.idParentSelected;
                requestUpdate.technicalSpecification.title = this.tituloEspanol;
				requestUpdate.technicalSpecification.titleEnglish = this.tituloIngles;


                this.dataServices.postData<FamilyProcessResponse>("Families/updateTechnicalSpecification", sessionStorage.getItem("token"), requestUpdate).subscribe(
                    data => {
						if (data.messageEsp != "") {
							if (data.messageEsp.endsWith("overLimit"))
								this.openSnack("Ha alcanzado el límite de caracteres, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
							else
								this.openSnack(data.messageEsp, "Aceptar");
                        } else {
                            this.openSnack("Especificación guardada con éxito", "Aceptar");

                            this.banderaSubtitulo = false;
                            this.banderaTitulo = true;
                            this.banderaEdicion = false;
                            this.banderaEdicionImagen = false;
                            this.file.nativeElement.value = "";

                            this.tituloEspanol = "";
                            this.tituloIngles = "";
                            this.informacionEspanol = "";
                            this.informacionIngles = "";
                            this.subtituloEspanol = "";
                            this.subtituloIngles = "";
                            this.ImagenPrevistaEspecificacion = "";
                            this.idParentSelected = 0;
                            this.idChildrenSelected = 0;

                            this.Busqueda();
                        }
                    },
                    error => {
                        if (error.error.hasOwnProperty("messageEsp")) {
                            this.relogin("registrar");
                        } else {
                            this.openSnack("Error al mandar la solicitud", "Aceptar");
                        }
                    }
                )
            }
        }
        //Agregar un nuevo subtitulo
        else if (this.banderaSubtitulo && !this.banderaEdicion) {
            if (this.subtituloEspanol.trim() == "") { /*|| this.informacionEspanol.trim() == ""*/
                if (this.subtituloIngles.trim() == "") { /*|| this.informacionIngles.trim() == ""*/
                    this.openSnack("Captura los datos del subtítulo en español y/o inglés", "Aceptar");
                    return;
                }
            }
            if (this.ImagenPrevistaEspecificacion != "" && this.ImagenPrevistaEspecificacion != null) {
                var PNG = this.ImagenPrevistaEspecificacion.includes("png;base64,");
                var JPG = this.ImagenPrevistaEspecificacion.includes("jpg;base64,");
                var JPEG = this.ImagenPrevistaEspecificacion.includes("jpeg;base64,");

                if (!PNG) {
                    if (!JPG) {
                        if (!JPEG) {
                            this.openSnack("Formato de imagen no permitido", "Aceptar");
                            return;
                        }
                    }
                }
			}


			if (this.familySpecCharLimit < (this.specCharUsed + this.informacionEspanol.length + this.informacionIngles.length + this.subtituloEspanol.length + this.subtituloIngles.length)) {
				this.openSnack("Ha alcanzado el límite de caracteres, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
				return;
			}

			//validate here the img
			if(this.ImagenPrevistaEspecificacion != null && this.ImagenPrevistaEspecificacion != ""){
				//there is an image
				if (this.imageLimit < (this.imgUsed + 1)) {
					this.openSnack("Ha alcanzado el límite de caracteres, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
					return;
				} else {
					console.log("Limit vs used: " + this.imageLimit + " : " + this.imgUsed);
				}
			}

            var requestSaveDetails = new SaveTechnicalSpecificationDetailsRequest();
            requestSaveDetails.technicalSpecificationDetails.description = this.informacionEspanol;
            requestSaveDetails.technicalSpecificationDetails.descriptionEnglish = this.informacionIngles;
            requestSaveDetails.technicalSpecificationDetails.imageBase = this.ImagenPrevistaEspecificacion == null ? "" : this.ImagenPrevistaEspecificacion;
            requestSaveDetails.technicalSpecificationDetails.subtitle = this.subtituloEspanol;
            requestSaveDetails.technicalSpecificationDetails.subtitleEnglish = this.subtituloIngles;
            requestSaveDetails.technicalSpecificationId = this.idParentSelected;

            this.dataServices.postData<FamilyProcessResponse>("Families/saveTechnicalSpecificationDetails", sessionStorage.getItem("token"), requestSaveDetails).subscribe(
                data => {
                    if (data.messageEsp != "") {
						if (data.messageEsp.endsWith("overLimit"))
							this.openSnack("Ha alcanzado el límite de caracteres, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
						else
							this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Especificación guardada con éxito", "Aceptar");
						if (this.ImagenPrevistaEspecificacion != null && this.ImagenPrevistaEspecificacion != "")
							this.imgUsed++;
                        this.banderaSubtitulo = false;
                        this.banderaTitulo = true;
                        this.banderaEdicion = false;
                        this.banderaEdicionImagen = false;
                        this.file.nativeElement.value = "";

                        this.tituloEspanol = "";
                        this.tituloIngles = "";
                        this.informacionEspanol = "";
                        this.informacionIngles = "";
                        this.subtituloEspanol = "";
                        this.subtituloIngles = "";
                        this.ImagenPrevistaEspecificacion = "";
                        this.idParentSelected = 0;
                        this.idChildrenSelected = 0;

                        this.Busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("registrar");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            )
        }
        //editar un subtitulo
        else if (this.banderaSubtitulo && this.banderaEdicion) {
            if (this.subtituloEspanol.trim() == "" ) {  /*|| this.informacionEspanol.trim() == ""*/
                if (this.subtituloIngles.trim() == "") { /*|| this.informacionIngles.trim() == ""*/
                    this.openSnack("Captura los datos del subtítulo en español y/o inglés", "Aceptar");
                    return;
                }
            }
            if ((this.ImagenPrevistaEspecificacion != "" &&
                this.ImagenPrevistaEspecificacion != null) && this.banderaEdicionImagen) {
                var PNG = this.ImagenPrevistaEspecificacion.includes("png;base64,");
                var JPG = this.ImagenPrevistaEspecificacion.includes("jpg;base64,");
                var JPEG = this.ImagenPrevistaEspecificacion.includes("jpeg;base64,");

                if (!PNG) {
                    if (!JPEG) {
                        if (!JPG) {
                            this.openSnack("Formato de imagen no permitido", "Aceptar");
                            return;
                        }
                    }
                }
            }

            var requestUpdateDetails = new UpdateTechnicalSpecificationDetailsRequest();
            requestUpdateDetails.imagenEliminado = this.banderaEdicionImagen;
            requestUpdateDetails.technicalSpecificationDetails.specificationTechnicalDetailId = this.idChildrenSelected;
            requestUpdateDetails.technicalSpecificationDetails.description = this.informacionEspanol;
            requestUpdateDetails.technicalSpecificationDetails.descriptionEnglish = this.informacionIngles;
            requestUpdateDetails.technicalSpecificationDetails.imageBase = !this.banderaEdicionImagen ? "" : this.ImagenPrevistaEspecificacion;
            requestUpdateDetails.technicalSpecificationDetails.subtitle = this.subtituloEspanol;
            requestUpdateDetails.technicalSpecificationDetails.subtitleEnglish = this.subtituloIngles;

            this.dataServices.postData<FamilyProcessResponse>("Families/updateTechnicalSpecificationDetails", sessionStorage.getItem("token"), requestUpdateDetails).subscribe(
                data => {
                    if (data.messageEsp != "") {
						if (data.messageEsp.endsWith("overLimit"))
							this.openSnack("Ha alcanzado el límite de caracteres, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
						else
							this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Especificación guardada con éxito", "Aceptar");

                        this.banderaSubtitulo = false;
                        this.banderaTitulo = true;
                        this.banderaEdicion = false;
                        this.banderaEdicionImagen = false;
                        this.file.nativeElement.value = "";

                        this.tituloEspanol = "";
                        this.tituloIngles = "";
                        this.informacionEspanol = "";
                        this.informacionIngles = "";
                        this.subtituloEspanol = "";
                        this.subtituloIngles = "";
                        this.ImagenPrevistaEspecificacion = "";
                        this.idParentSelected = 0;
                        this.idChildrenSelected = 0;

                        this.Busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("registrar");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            )
        }
    }

    //Función para guardar la descripcion de la familia
    guardarDescripcionFamilia() {
        var request = new UpdateFamilyRequest();

        if (this.responseFamily.productFamily.productFamilyData.description == "" &&
            this.responseFamily.productFamily.productFamilyData.descriptionEnglish == "") {
            this.openSnack("No hay descripción en inglés o en español", "Aceptar");
            //return;
        }

        request.familyData.description = this.responseFamily.productFamily.productFamilyData.description;
        request.familyData.descriptionEnglish = this.responseFamily.productFamily.productFamilyData.descriptionEnglish;
        request.familyData.familyId = this.familiaId;
        request.option = 2;

        this.dataServices.postData<FamilyProcessResponse>("Families/updateFamily", sessionStorage.getItem("token"), request).subscribe(
            data => {
                if (data.messageEsp != "") {
                    this.openSnack(data.messageEsp, "Aceptar");
                } else {
                    this.openSnack("Descripción editado con éxito", "Aceptar");
                }

                this.BusquedaDatosFamilias();
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("guardarDescripcionFamilia");
                } else {
                    this.openSnack("Error al mandar la solicitud", "Aceptar");
                }
            }
        )
    }

    //Funcion para editar un titulo cargando los datos
    editarTitulo(node: FlatNode) {
        this.response.technicalSpecifications.forEach((it, id) => {
            if (it.specificationTecnicalId == node.id) {
                this.tituloEspanol = it.title;
                this.tituloIngles = it.titleEnglish;
                this.idParentSelected = it.specificationTecnicalId;

                this.ImagenPrevistaEspecificacion = "";
                this.informacionEspanol = "";
                this.informacionIngles = "";
                this.subtituloEspanol = "";
                this.subtituloIngles = "";

                this.banderaSubtitulo = false;
                this.banderaTitulo = true;
                this.banderaEdicion = true;
                this.idChildrenSelected = 0;
                this.banderaEdicionImagen = false;
                this.file.nativeElement.value = "";
            }
        });
    }

    //Funcion para editar un subtitulo cargando los datos
    editarSubtitulo(node: FlatNode) {
        this.response.technicalSpecifications.forEach((it, id) => {
            if (it.specificationTecnicalId == node.idParent) {
                it.technicalSpecificationDetails.forEach((it2, id2) => {
                    if (it2.specificationTechnicalDetailId == node.id) {
                        this.informacionEspanol = it2.description;
                        this.informacionIngles = it2.descriptionEnglish;
                        this.ImagenPrevistaEspecificacion = it2.image;
                        this.subtituloEspanol = it2.subtitle;
                        this.subtituloIngles = it2.subtitleEnglish;
                        this.idChildrenSelected = it2.specificationTechnicalDetailId;
                        this.idParentSelected = node.idParent;

                        this.tituloEspanol = "";
                        this.tituloIngles = "";

                        this.banderaSubtitulo = true;
                        this.banderaTitulo = false;
                        this.banderaEdicion = true;
                        this.banderaEdicionImagen = false;
                        this.file.nativeElement.value = "";
                    }
                });
            }
        });
    }

    //Funcion para eliminar los titulos
    eliminarTitulo(node: FlatNode, bandera: number) {
        this.nodeActual = node;
        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true,
                data: {
                    palabra: "la especificación técnica"
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    var requestDelete = new DeleteTechnicalSpecificationRequest();
                    requestDelete.technicalSpecificationId = node.id;

                    this.dataServices.postData<FamilyProcessResponse>("Families/deleteTechnicalSpecification", sessionStorage.getItem("token"), requestDelete).subscribe(
                        data => {
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");
                            } else {
                                this.openSnack("Especificación eliminada con éxito", "Aceptar");

                                this.banderaSubtitulo = false;
                                this.banderaTitulo = true;
                                this.banderaEdicion = false;
                                this.banderaEdicionImagen = false;
                                this.file.nativeElement.value = "";

                                this.tituloEspanol = "";
                                this.tituloIngles = "";
                                this.informacionEspanol = "";
                                this.informacionIngles = "";
                                this.subtituloEspanol = "";
                                this.subtituloIngles = "";
                                this.ImagenPrevistaEspecificacion = "";
                                this.idParentSelected = 0;
                                this.idChildrenSelected = 0;

                                this.Busqueda();
                            }
                        },
                        error => {
                            if (error.error.hasOwnProperty("messageEsp")) {
                                this.relogin("eliminarTitulo");
                            } else {
                                this.openSnack("Error al mandar la solicitud", "Aceptar");
                            }
                        }
                    )
                }
            });
        } else {
            var requestDelete = new DeleteTechnicalSpecificationRequest();
            requestDelete.technicalSpecificationId = node.id;

            this.dataServices.postData<FamilyProcessResponse>("Families/deleteTechnicalSpecification", sessionStorage.getItem("token"), requestDelete).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Especificación eliminada con éxito", "Aceptar");

                        this.banderaSubtitulo = false;
                        this.banderaTitulo = true;
                        this.banderaEdicion = false;
                        this.banderaEdicionImagen = false;
                        this.file.nativeElement.value = "";

                        this.tituloEspanol = "";
                        this.tituloIngles = "";
                        this.informacionEspanol = "";
                        this.informacionIngles = "";
                        this.subtituloEspanol = "";
                        this.subtituloIngles = "";
                        this.ImagenPrevistaEspecificacion = "";
                        this.idParentSelected = 0;
                        this.idChildrenSelected = 0;

                        this.Busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("eliminarTitulo");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            )
        }
    }

    //Funcion para eliminar subtitulos
    eliminarSubtitulo(node: FlatNode, bandera: number) {
        this.nodeActual = node;
        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true,
                data: {
                    palabra: "el detalle de la especificación técnica"
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    var requestDelete = new DeleteTechnicalSpecificationDetailsRequest();
                    requestDelete.TechnicalSpecificationDetailsId = node.id;

                    this.dataServices.postData<FamilyProcessResponse>("Families/deleteTechnicalSpecificationDetails", sessionStorage.getItem("token"), requestDelete).subscribe(
                        data => {
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");
                            } else {
                                this.openSnack("Especificación eliminada con éxito", "Aceptar");

                                this.banderaSubtitulo = false;
                                this.banderaTitulo = true;
                                this.banderaEdicion = false;
                                this.banderaEdicionImagen = false;
                                this.file.nativeElement.value = "";

                                this.tituloEspanol = "";
                                this.tituloIngles = "";
                                this.informacionEspanol = "";
                                this.informacionIngles = "";
                                this.subtituloEspanol = "";
                                this.subtituloIngles = "";
                                this.ImagenPrevistaEspecificacion = "";
                                this.idParentSelected = 0;
                                this.idChildrenSelected = 0;

                                this.Busqueda();
                            }
                        },
                        error => {
                            if (error.error.hasOwnProperty("messageEsp")) {
                                this.relogin("eliminarSubtitulo");
                            } else {
                                this.openSnack("Error al mandar la solicitud", "Aceptar");
                            }
                        }
                    )
                }
            });
        }
        else {
            var requestDelete = new DeleteTechnicalSpecificationDetailsRequest();
            requestDelete.TechnicalSpecificationDetailsId = node.id;

            this.dataServices.postData<FamilyProcessResponse>("Families/deleteTechnicalSpecificationDetails", sessionStorage.getItem("token"), requestDelete).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Especificación eliminada con éxito", "Aceptar");

                        this.banderaSubtitulo = false;
                        this.banderaTitulo = true;
                        this.banderaEdicion = false;
                        this.banderaEdicionImagen = false;
                        this.file.nativeElement.value = "";

                        this.tituloEspanol = "";
                        this.tituloIngles = "";
                        this.informacionEspanol = "";
                        this.informacionIngles = "";
                        this.subtituloEspanol = "";
                        this.subtituloIngles = "";
                        this.ImagenPrevistaEspecificacion = "";
                        this.idParentSelected = 0;
                        this.idChildrenSelected = 0;

                        this.Busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("eliminarSubtitulo");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                    }
                }
            )
        }
    }

    //Funcion para abrir el modal del mensaje
    openSnack = (message: string, action: string) => {
        this.snack.open(message, action, {
            duration: 5000
        })
    }

    //Funcion para realizar la busqueda de la especificacion tecnica
    Busqueda() {
        var request = new SearchTechnicalSpecificationFamilyRequest();
        request.familyId = this.familiaId;

        this.dataServices.postData<SearchTechnicalSpecificationFamilyResponse>("Families/searchTechnicalSpecificationFamily", sessionStorage.getItem("token"), request).subscribe(
            data => {
                this.response = data;
                this.dataTree = [];

                this.response.technicalSpecifications.forEach((it, id) => {
                    var dateTree = new DataTreeEspecification;
                    dateTree.id = it.specificationTecnicalId;
                    dateTree.title = it.title + "/" + it.titleEnglish;
                    dateTree.parent = true;

                    it.technicalSpecificationDetails.forEach((it2, id2) => {
                        dateTree.children.push({
                            id: it2.specificationTechnicalDetailId,
                            idPaaarent: it.specificationTecnicalId,
                            parent: false,
                            title: it2.subtitle + "/" + it2.subtitleEnglish
                        })
                    });

                    this.dataTree.push(dateTree);
                });

                this.dataSource.data = this.dataTree;
                this.treeControl.expandAll();
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("Busqueda");
                } else {
                    console.log(error);
                }
            }
        )
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
                sessionStorage.setItem("company", data.userData.userData.company.toString());
                sessionStorage.setItem("isType", data.userData.userData.isType.toString());

                switch (peticion) {
                    case "BusquedaDatosFamilias":
                        this.BusquedaDatosFamilias();
                        break;
                    case "registrar":
                        this.registrar();
                        break;
                    case "eliminarTitulo":
                        this.eliminarTitulo(this.nodeActual, 2);
                        break;
                    case "eliminarSubtitulo":
                        this.eliminarSubtitulo(this.nodeActual, 2);
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

    cambiarStyle(ev) {
        let shand = document.getElementsByClassName('mat-drawer-shown') as HTMLCollectionOf<HTMLElement>;
        setTimeout(function () { if (shand.length) { shand[0].style.backgroundColor = "rgba(0, 0, 0, 0)"; } }, 10);
        setTimeout(function () { if (shand.length) { shand[0].style.backgroundColor = "rgba(0, 0, 0, 0)"; } }, 50);
    }

    back = () => {
        console.log("actualizao");
        this._router.navigateByUrl('Familias/CatalogoFamilias', { state: { familyId: this.familiaId } })
        // this._location.back();
    }

    ngOnInit() {
        this.Busqueda();
        this.BusquedaDatosFamilias();
    }

    ngAfterViewInit() {
        this.treeControl.expandAll();
    }
}
