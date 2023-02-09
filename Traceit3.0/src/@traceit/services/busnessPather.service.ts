import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, ReplaySubject, tap } from 'rxjs';
import { SociosDeNegocios } from '@traceit/types/SocioDeNegocio';

export interface jsonresponse{
    status:number;
    message:string;
    data:Object | Object[];
}
@Injectable({
    providedIn: 'root'
})
export class SociosDeNegociosService
{
    private _dociosdenegocios: ReplaySubject<SociosDeNegocios> = new ReplaySubject<SociosDeNegocios>(1);
    private backendURL: string ="http://localhost:8000/api/v1/BusinessPartner";
    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for user
     *
     * @param value
     */
    set SocioDeNegocio(value: SociosDeNegocios)
    {
        // Store the value
        this._dociosdenegocios.next(value);
    }

    get SocioDeNegocio$(): Observable<SociosDeNegocios>
    {
        return this._dociosdenegocios.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get the current logged in user data
     */
    get(): Observable<SociosDeNegocios>
    {
        return this._httpClient.get<SociosDeNegocios>('api/v1/BusinessPartner').pipe(
            tap((SocioDeNegocio) => {
                this._dociosdenegocios.next(SocioDeNegocio);
                console.log(SocioDeNegocio)
            })
        );
    }
    findAllUsers(): Observable<jsonresponse>{
        return this._httpClient.get<jsonresponse>(`${this.backendURL}/list`);
      }
    findAUser(id:number): Observable<jsonresponse>{
        return this._httpClient.get<jsonresponse>(`${this.backendURL}/list/${id}`);
      }

    /**
     * Update the user
     *
     * @param user
     */
    update(SocioDeNegocio: SociosDeNegocios): Observable<jsonresponse>
    {
        debugger
        return this._httpClient.put<jsonresponse>(`${this.backendURL}/update/`,SocioDeNegocio);
    }
    sett(SocioDeNegocio:SociosDeNegocios):Observable<jsonresponse>{
        
        return this._httpClient.post<jsonresponse>(`${this.backendURL}/create`, SocioDeNegocio)
    }
    deleteUser(id:number): Observable<jsonresponse>{
        return this._httpClient.delete<jsonresponse>(`${this.backendURL}/delete/${id}`);
      }
}
