//Request and response del web method "searchDropDownListAddress"
export class SaveAcopioRequest {
    activo: boolean;
    acopioId: number;
    estatus: boolean;
    numeroAcopio: number;
    nombreAcopio: string;
    paisId: number;
    estadoId: number;
    ciudad: string;
    codigoPostal: number;
    direccion: string;
    latitud: string;
    longitud: string;
    usuarioCreador: number;
    companiaId: number;
    fechaCreacion: Date;
    fechaModificacion: Date;
    usuarioModificador: number;
    constructor() {
        this.activo = true;
        this.acopioId  = 0;
        this.estatus  = true;
        this.numeroAcopio  = 0;
        this.nombreAcopio  = '';
        this.paisId  = 0;
        this.estadoId  = 0;
        this.ciudad  = '';
        this.codigoPostal  = 0;
        this.direccion  = '';
        this.latitud  = '';
        this.longitud  = '';
        this.usuarioCreador  = 0;
        this.companiaId  = 0;
        this.fechaCreacion  = new Date();
        this.fechaModificacion  = new Date();
        this.usuarioModificador  = 0;
    }
}

export class searchAcopioResponse{
    acopioId: number;
    activo: boolean;
    companiaId: number;
    nombreAcopio: string;
    numeroAcopio: number;
    paisNombre:  string;
    estadoNombre: string;
    constructor(){
        this.acopioId = 0;
        this.activo = true;
        this.companiaId = 0; 
        this.nombreAcopio = '';
        this.numeroAcopio = 0;
        this.paisNombre = '';
        this.estadoNombre = '';
    }
}

export class AcopioResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class searchAcopioById {
    acopioId: number;
    activo: boolean;
    numeroAcopio: number;
    nombreAcopio: string;
    paisId: number;
    estadoId: number;
    ciudad: string;
    codigoPostal: number;
    address: string;
    latitude: string;
    longitude: string;
    constructor(){
        this.acopioId = 0;
        this.activo = true;
        this.numeroAcopio = 0;
        this.nombreAcopio = '';
        this.paisId = 0;
        this.estadoId = 0;
        this.ciudad = '';
        this.codigoPostal = 0;
        this.address = '';
        this.latitude = '';
        this.longitude = '';
    }
}