//Request and response del web method "searchDropDownListAddress"
export class SaveProductorRequest {
    activo: boolean; //Es el que define si esta activo o no
    productorId: number;
    estatus: boolean; // Identifica si eta eliminado o no
    numeroProductor: number; 
    nombreProductor: string;
    nombreContacto: string;
    apellidoContacto: string;
    telefonoContacto: string;
    nombreRancho: string;
    direccion: string;
    latitud: string;
    longitud: string;
    usuarioCreador: number;
    companiaId: number;
    fechaCreacion: Date;
    fechaModificacion: Date;
    usuarioModificador: number;
    acopiosId: Array<any>;
    constructor() {
        this.activo = true;
        this.productorId  = 0;
        this.estatus  = true;
        this.numeroProductor = 0;
        this.nombreProductor = '';
        this.nombreContacto = '';
        this.apellidoContacto = '';
        this.telefonoContacto = '';
        this.nombreRancho = '';
        this.direccion  = '';
        this.latitud  = '';
        this.longitud  = '';
        this.usuarioCreador  = 0;
        this.companiaId  = 0;
        this.fechaCreacion  = new Date();
        this.fechaModificacion  = new Date();
        this.usuarioModificador  = 0;
        this.acopiosId = [];
    }
}

export class ProductorList{
    id: number;
    activo: boolean;
    numeroProductor: number;
    nombreProductor: string;
    nombreRancho: string;
    companiaId: number;
    nombreAcopio: string;
    numeroAcopio: number;
    status: boolean;
    constructor(){
        this.id = 0;
        this.activo = true;
        this.numeroProductor = 0;
        this.nombreProductor = '';
        this.nombreRancho = '';
        this.companiaId = 0;
        this.nombreAcopio = '';
        this.numeroAcopio = 0;
        this.status = true;
    }
}

export class SearchProductorById{
    id: number;
    activo: boolean;
    numeroProductor: number;
    nombreProductor: string;
    nombreRancho: string;
    numeroAcopio: number;
    nombreAcopio: string;
    constructor(){
        this.id = 0;
        this.activo = true;
        this.numeroProductor = 0;
        this.nombreProductor = '';
        this.nombreRancho = '';
        this.numeroAcopio = 0;
        this.nombreAcopio = '';
    }
}

//Request and response del web method "searchDropDownListAddress"
export class UpdateProductorByIdRequest {
    productorId: number;
    activo: boolean; //Es el que define si esta activo o no
    numeroProductor: number; 
    nombreProductor: string;
    nombreRancho: string;
    direccion: string;
    latitud: string;
    longitud: string;
    acopiosId: Array<any>;
    getAcopiosId: Array<any>;
    constructor() {
        this.activo = true;
        this.productorId  = 0;
        this.numeroProductor = 0;
        this.nombreProductor = '';
        this.nombreRancho = '';
        this.direccion  = '';
        this.latitud  = '';
        this.longitud  = '';
        this.acopiosId = [];
        this.getAcopiosId = [];
    }
}


export class DeleteProductorRequest {
    productorId: number;
}