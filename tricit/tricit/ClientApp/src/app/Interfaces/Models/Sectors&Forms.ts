/*SECOTRES */
export class Sector{
    sectorId: number;
    nombre: string;
    descripcionCorta: string;
    usuarioCreadorId: number;
    usuarioModificadorId: number;
    fechaCreacion: Date;
    fechaModificacion: Date;
    estatusId: number;

    constructor() {
        this.sectorId = 0;
            this.nombre = "";
            this.descripcionCorta = "";
            this.usuarioCreadorId = 0;
            this.usuarioModificadorId = 0;
            this.fechaCreacion = new Date();
            this.fechaModificacion = new Date();
            this.estatusId = 0;
    }
}
export class SearchSectorsResponse {
    sectorsDataList: Sector[];
    messageEng: string;
    messageEsp: string;
    

    constructor() {
        this.sectorsDataList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class SectorProcessResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}
export class Sectore{
    sectorId: number;
    nombre: string;
    descripcionCorta: string;
    estatusId: number;

    constructor() {
        this.sectorId = 0;
            this.nombre = "";
            this.descripcionCorta = "";
            this.estatusId = 0;
    }
}

/*Formularios */

export class Form{
    formularioId: number;
    nombre: string;
    descripcionCorta: string;
    usuarioCreadorId: number;
    usuarioModificadorId: number;
    fechaCreacion: Date;
    fechaModificacion: Date;
    estatusFormularioId: number;
    sectorId:number;
    constructor(){
        this.formularioId = 0;
        this.nombre ="";
        this.descripcionCorta = "";
        this.usuarioCreadorId = 0;
        this.fechaCreacion = new Date();
        this.usuarioModificadorId = 0;
        this.fechaModificacion = new Date();
        this.estatusFormularioId = 0;
        this.sectorId=0;
    }
}
export class SearchFormsResponse {
    formsDataList: Form[];
    messageEng: string;
    messageEsp: string;
    

    constructor() {
        this.formsDataList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class FormProcessResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}
export class Forme{
    sectorId: number;
    nombre: string;
    descripcionCorta: string;
    estatusId: number;

    constructor() {
        this.sectorId = 0;
            this.nombre = "";
            this.descripcionCorta = "";
            this.estatusId = 0;
    }
}


