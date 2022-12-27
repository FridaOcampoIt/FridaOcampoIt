import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { enviroments } from '../Enviroments/enviroments';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class DataServices {
    private httpHeader: HttpHeaders;

    constructor(private httpClient: HttpClient) { };

    //Metodo para realizar los consumos POST al WS
    postData<T>(method: string, bearer: string, objectRequest?: any):Observable<T> {
        this.httpHeader = new HttpHeaders({
            'Authorization': 'Bearer ' + bearer,
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        });

        return this.httpClient.post<T>(`${enviroments.urlBase}${method}`, objectRequest, { headers: this.httpHeader });
    }

    getCoorde():Observable<any> {

        return this.httpClient.get<any>("https://ipgeolocation.abstractapi.com/v1/?api_key=3fba439c1abf4a4aa345639d905e82e4");
    }

    postDataDocs<T>(method: string, bearer: string, objectRequest?: any): Observable<T> {
		this.httpHeader = new HttpHeaders({
			'Authorization': 'Bearer ' + bearer,
			'Access-Control-Allow-Origin': '*',
		});

		return this.httpClient.post<T>(`${enviroments.urlBase}${method}`, objectRequest, { headers: this.httpHeader });
    }

    //Metodo para descagar documentos realizando un consumo POST al WS
	postDataDownDocs<T>(method: string, bearer: string, objectRequest?: any): Observable<Blob> {
		this.httpHeader = new HttpHeaders({
			'Authorization': 'Bearer ' + bearer,
			'Access-Control-Allow-Origin': '*'
		});

		return this.httpClient.post<Blob>(`${enviroments.urlBase}${method}`, objectRequest, { responseType: 'blob' as 'json', headers: this.httpHeader });
	}
}

//Services para compartir los datos entre componentes
@Injectable()
export class ServicesComponent {
    @Output() Family: EventEmitter<number> = new EventEmitter();

    //Servicio para compartir los datos de familia
    familyData(familyIdService : number) {
        this.Family.emit(familyIdService);
    }
}