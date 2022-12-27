import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource, MatPaginator, MatDialog, MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { DialogoPreguntasComponent } from '../dialogo-preguntas/dialogo-preguntas.component';
import { DialogoEliminarComponent } from '../../dialogo-eliminar/dialogo-eliminar.component';

import { DataServices } from '../../../Interfaces/Services/general.service';
import { SearchWarrantiesFaqRequest, SearchWarrantiesFaqResponse, FrequentQuestionsFamilyData, SaveWarrantyRequest, FamilyProcessResponse, DeleteWarrantiesRequest, DeleteFrequentQuestionsRequest } from '../../../Interfaces/Models/FamilyModels';
import { SearchCountriesResponse } from '../../../Interfaces/Models/TraceITBaseModels';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';


@Component({
    selector: 'app-garantia-servicio',
    templateUrl: './garantia-servicio.component.html',
    styleUrls: ['./garantia-servicio.component.css']
})
export class GarantiaServicioComponent implements OnInit, AfterViewInit {

    constructor(
        private _dialog: MatDialog,
        private _route: ActivatedRoute,
        private _location: Location,
        private dataServices: DataServices,
        private snack: MatSnackBar,
        private _router: Router)
    {
        this.familiaId = parseInt(this._route.snapshot.paramMap.get('id'), 10);
    }

    familiaId: number = 0;
    response = new SearchWarrantiesFaqResponse();
    paisGarantia: string = "";
    PDFReference: string = "";
	PDF: string = "";
	urlPDF: string = "";
    Meses: number = 0;
    subiendoPDF: boolean = false;
    responseCountries = new SearchCountriesResponse();

    idGarantia: number = 0;

    @ViewChild('fileInput') file: ElementRef;
    @ViewChild(MatPaginator) paginator: MatPaginator;

    displayedColumns: string[] = ['pregunta'];
    dataSource = new MatTableDataSource<FrequentQuestionsFamilyData>(this.response.warrantiesFaq.frequentQuestions);
    selection = new SelectionModel<FrequentQuestionsFamilyData>(false, []);

    //Funcion para agregar o editar una pregunta
    AgregarEditarPregunta = (Action: number, Title: string, Model?: FrequentQuestionsFamilyData) => {

        if (Action == 1) {
            const dialogRef = this._dialog.open(DialogoPreguntasComponent, {
                panelClass: 'dialog-comp',
                disableClose: true,
                data: {
                    title: Title,
                    action: Action,
                    idFamily: this.familiaId
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                this.busqueda();
            });
        }
        else {
            const dialogRef = this._dialog.open(DialogoPreguntasComponent, {
                panelClass: 'dialog-comp',
                disableClose: true,
                data: {
                    title: Title,
                    action: Action,
                    idFamily: this.familiaId,
                    datos: Model
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                this.busqueda();
            });
        }        
    }

    //Funcion para cargar los paises en el combo
    BusquedaPaises() {
        this.dataServices.postData<SearchCountriesResponse>("Families/searchCountries", sessionStorage.getItem("token")).subscribe(
            data => {
                this.responseCountries = data;
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("BusquedaPaises");
                } else {
                    console.log(error);
                }
            }
        );
    }

    //Funcion para la busqueda de preguntas frecuentes y garantias
    busqueda() {
        var request = new SearchWarrantiesFaqRequest();
        request.familyId = this.familiaId;

        this.dataServices.postData<SearchWarrantiesFaqResponse>("Families/searchWarrantiesFaq", sessionStorage.getItem("token"), request).subscribe(
            data => {
				this.response = data;
				this.openSnack("Su paquete contratado cuenta con " + data.warrantiesFaq.limitsFamily.nCharFAQ + " caracteres", "Aceptar");
                this.dataSource = new MatTableDataSource<FrequentQuestionsFamilyData>(this.response.warrantiesFaq.frequentQuestions);
                this.selection = new SelectionModel<FrequentQuestionsFamilyData>(false, []);
                this.dataSource.paginator = this.paginator;
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

    //Funcion para cargar el base 64 del archivo pdf seleccionado
    CargarPDF(files) {
        var archivos = files[0];
        this.getBase64(archivos).then(
            data => {
                this.PDF = "Archivo seleccionado"
                this.PDFReference = data.toString();
            }
        );
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

    //Funcion para validar el tope del input de meses
    validarNumero() {
        if (this.Meses < 0) {
            this.Meses = 0;
        }

        if (this.Meses > 500) {
            this.Meses = 500
        }
    }

    //funcion para guardar la garantia
	guardarGarantia() {
		if (this.paisGarantia.trim() == "" || this.Meses == 0 || this.urlPDF == "") {
            this.openSnack("Captura los datos de la garantía", "Aceptar");
            return;
        }

        /*var PDF = this.PDFReference.includes("data:application/pdf;base64,");

        if (!PDF) {
            this.openSnack("Formato de archivo no permitido", "Aceptar");
            return;
        }*/

        var request = new SaveWarrantyRequest();
        request.familyId = this.familiaId;
		request.warranty.country = this.paisGarantia;
		request.warranty.pdfBase = this.urlPDF;//this.PDFReference;
        request.warranty.periodMonth = this.Meses;
        
        this.subiendoPDF = true;

        this.dataServices.postData<FamilyProcessResponse>("Families/saveWarranty", sessionStorage.getItem("token"), request).subscribe(
            data => {
                if (data.messageEsp != "") {
                    this.openSnack(data.messageEsp, "Aceptar");
                } else {
                    this.openSnack("Garantía guardada con éxito", "Aceptar");
                    this.Meses = 0;
					this.PDF = "";
					this.urlPDF = "";
                    this.PDFReference = "";
                    this.paisGarantia = "";
                    this.file.nativeElement.value = "";
                    this.busqueda();
                }

                this.subiendoPDF = false;
            },
            error => {
                if (error.error.hasOwnProperty("messageEsp")) {
                    this.relogin("busqueda");
                } else {
                    this.openSnack("Error al mandar la solicitud", "Aceptar");
                    this.subiendoPDF = false;
                }
            }
        )
    }

    //funcion para eliminar la garantia
    eliminarGarantia(id: number, bandera: number) {
        this.idGarantia = id;
        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true,
                data: {
                    palabra: "la garantia"
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    var request = new DeleteWarrantiesRequest();
                    request.warrantyId = id;

                    this.subiendoPDF = true;

                    this.dataServices.postData<FamilyProcessResponse>("Families/deleteWarranties", sessionStorage.getItem("token"), request).subscribe(
                        data => {
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");
                            } else {
                                this.openSnack("Garantía eliminada con éxito", "Aceptar");
                                this.busqueda();
                            }

                            this.subiendoPDF = false;
                        },
                        error => {
                            if (error.error.hasOwnProperty("messageEsp")) {
                                this.relogin("eliminarGarantia");
                            } else {
                                this.openSnack("Error al mandar la solicitud", "Aceptar");
                                this.subiendoPDF = false;
                            }
                        }
                    )
                }
            });
        }
        else {
            var request = new DeleteWarrantiesRequest();
            request.warrantyId = id;

            this.subiendoPDF = true;

            this.dataServices.postData<FamilyProcessResponse>("Families/deleteWarranties", sessionStorage.getItem("token"), request).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Garantía eliminada con éxito", "Aceptar");
                        this.busqueda();
                    }

                    this.subiendoPDF = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("eliminarGarantia");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendoPDF = false;
                    }
                }
            )
        }
    }

    eliminarPregunta(id: number, bandera: number) {
        this.idGarantia = id;

        if (bandera == 1) {
            const dialogRef = this._dialog.open(DialogoEliminarComponent, {
                width: 'dialog-comp',
                disableClose: true,
                data: {
                    palabra: "la pregunta frecuente"
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result) {
                    var request = new DeleteFrequentQuestionsRequest();
                    request.faqId = id;

                    this.dataServices.postData<FamilyProcessResponse>("Families/deleteFrequentQuestions", sessionStorage.getItem("token"), request).subscribe(
                        data => {
                            if (data.messageEsp != "") {
                                this.openSnack(data.messageEsp, "Aceptar");
                            } else {
                                this.openSnack("Pregunta eliminada con éxito", "Aceptar");
                                this.busqueda();
                            }
                        },
                        error => {
                            if (error.error.hasOwnProperty("messageEsp")) {
                                this.relogin("eliminarPregunta");
                            } else {
                                this.openSnack("Error al mandar la solicitud", "Aceptar");
                            }                            
                        }
                    )
                }
            });
        }
        else {
            var request = new DeleteFrequentQuestionsRequest();
            request.faqId = id;

            this.dataServices.postData<FamilyProcessResponse>("Families/deleteFrequentQuestions", sessionStorage.getItem("token"), request).subscribe(
                data => {
                    if (data.messageEsp != "") {
                        this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Pregunta eliminada con éxito", "Aceptar");
                        this.busqueda();
                    }
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("eliminarPregunta");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
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
                    case "BusquedaPaises":
                        this.BusquedaPaises();
                        break;
                    case "busqueda":
                        this.busqueda();
                        break;
                    case "guardarGarantia":
                        this.guardarGarantia();
                        break;
                    case "eliminarGarantia":
                        this.eliminarGarantia(this.idGarantia, 2);
                        break;
                    case "eliminarPregunta":
                        this.eliminarPregunta(this.idGarantia, 2);
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

    back = () => {
        this._router.navigateByUrl('Familias/CatalogoFamilias', { state: { familyId: this.familiaId } })
        // this._location.back();
    }

    ngOnInit() {
        this.BusquedaPaises();
        this.busqueda();        
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }
}
