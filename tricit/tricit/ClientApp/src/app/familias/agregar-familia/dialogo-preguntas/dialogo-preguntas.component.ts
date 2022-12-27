import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { DataServices } from '../../../Interfaces/Services/general.service';
import { FrequentQuestionsFamilyData, FamilyProcessResponse, SaveFrequentQuestionsRequest, UpdateFrequentQuestionRequest } from '../../../Interfaces/Models/FamilyModels';
import { LoginUserRequest, LoginUserResponse } from '../../../Interfaces/Models/LoginModels';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dialogo-preguntas',
  templateUrl: './dialogo-preguntas.component.html',
  styleUrls: ['./dialogo-preguntas.component.less']
})
export class DialogoPreguntasComponent implements OnInit {

    constructor(
        @Inject(MAT_DIALOG_DATA) private _data: any,
        private dataServices: DataServices,
        private snack: MatSnackBar,
        private dialogRef: MatDialogRef<DialogoPreguntasComponent>,
        private _router: Router) { }

    titulo: string = "";
    action: number = 0;

    model = new FrequentQuestionsFamilyData();
    Pregunta: string = "";
    PreguntaIngles: string = "";
    Respuesta: string = "";
    RespuestaIngles: string = "";
    idFamily: number = 0;

    subiendo: boolean = false;

    ngOnInit() {
        this.titulo = this._data.title;
        this.action = this._data.action;
        this.idFamily = this._data.idFamily

        if (this.action == 2) {
            this.model = this._data.datos;
            this.Pregunta = this.model.questionSpanish;
            this.PreguntaIngles = this.model.questionEnglish;
            this.Respuesta = this.model.responseSpanish;
            this.RespuestaIngles = this.model.responseEnglish;
        }
    }

    //Funcion para guardar una pregunta
    guardarPregunta() {
        if (this.action == 1) {
            if (this.Pregunta.trim() == "" || this.Respuesta.trim() == "")
            {
                if (this.PreguntaIngles.trim() == "" || this.RespuestaIngles.trim() == "")
                {
                    this.openSnack("Captura los datos de la pregunta frecuente en ingles y/o español", "Aceptar");
                    return;
                }                
            }

            var requestSave = new SaveFrequentQuestionsRequest();
            requestSave.familyId = this.idFamily;
            requestSave.frequentQuestions.question = this.Pregunta;
            requestSave.frequentQuestions.questionEnglish = this.PreguntaIngles;
            requestSave.frequentQuestions.response = this.Respuesta;
            requestSave.frequentQuestions.responseEnglish = this.RespuestaIngles;

            this.subiendo = true;

            this.dataServices.postData<FamilyProcessResponse>("Families/saveFrequentQuestions", sessionStorage.getItem("token"), requestSave).subscribe(
                data => {
                    if (data.messageEsp != "") {
						if (data.messageEsp.endsWith("overLimit"))
							this.openSnack("Ha alcanzado el límite de preguntas frecuentes, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
						else
							this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Pregunta guardada con éxito", "Aceptar");
                        this.dialogRef.close(true);
                    }
                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("guardarPregunta");
                    } else {
                        this.openSnack("Error al mandar la solicitud", "Aceptar");
                        this.subiendo = false;
                    }                    
                }
            )
        }
        else {
            if (this.Pregunta.trim() == "" || this.Respuesta.trim() == "")
            {
                if (this.PreguntaIngles.trim() == "" || this.RespuestaIngles.trim() == "")
                {
                    this.openSnack("Captura los datos de la pregunta frecuente en ingles y/o español", "Aceptar");
                    return;
                }
            }

            var requestUpdate = new UpdateFrequentQuestionRequest();
            requestUpdate.frequentQuestions.question = this.Pregunta;
            requestUpdate.frequentQuestions.questionEnglish = this.PreguntaIngles;
            requestUpdate.frequentQuestions.response = this.Respuesta;
            requestUpdate.frequentQuestions.responseEnglish = this.RespuestaIngles;
            requestUpdate.frequentQuestions.questionId = this.model.faqId;

            this.subiendo = true;

            this.dataServices.postData<FamilyProcessResponse>("Families/updateFrequentQuestions", sessionStorage.getItem("token"), requestUpdate).subscribe(
                data => {
                    if (data.messageEsp != "") {
						if (data.messageEsp.endsWith("overLimit"))
							this.openSnack("Ha alcanzado el límite de preguntas frecuentes, actualice su versión contratada para cargar todos sus contenidos", "Aceptar");
						else
							this.openSnack(data.messageEsp, "Aceptar");
                    } else {
                        this.openSnack("Pregunta guardada con éxito", "Aceptar");
                        this.dialogRef.close(true);
                    }
                    this.subiendo = false;
                },
                error => {
                    if (error.error.hasOwnProperty("messageEsp")) {
                        this.relogin("guardarPregunta");
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
                    case "guardarPregunta":
                        this.guardarPregunta();
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
}
