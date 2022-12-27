import { TraceITListDropDown, TraceITMovimientosDataGeneralProd, TraceITMovimientosDataMerma } from "./TraceITBaseModels";

//#region  Request and response del web method "searchMovimiento"
export class SearchMovimientoRequest{
    producto: number;
	tipoMovimientoId: number;
    fechaCaducidad : string;
    fechaIngresoDe : string;
    fechaIngresoHasta : string;
    usuario: number;

    constructor(){
        this.producto = 0;
        this.tipoMovimientoId = 0;
        this.fechaCaducidad  = '';
        this.fechaIngresoDe  = '';
        this.fechaIngresoHasta  = '';
        this.usuario=0;
    }
}

export class SearchMovimientoResponse {
    movimientosDataList: MovimientosData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class MovimientosData {
    movimientoId: number;
    nombreAgrupacion: string;
    numeroPallet: number;
    numeroCajas: number;
    producto: string;
    cantidad: number;
    cantidadRecibida: number;
    tipoMovimiento: string;
    nombreRemitente: string;
    apellidoRemitente: string;
    nombreDestinatario: string;
    apellidoDestinatario: string;
    fechaIngreso: string;
    fechaCaducidad: string;
    cantIndMov:number;
    lote: string;
    referenciaInterna: string;
    referenciaExterna: string;
    codigoQR: string;

    constructor() {
        this.movimientoId = 0;
        this.nombreAgrupacion = '';
        this.numeroPallet = 0;
        this.numeroCajas = 0;
        this.producto = '';
        this.cantidad = 0;
        this.tipoMovimiento = '';
        this.nombreRemitente = '';
        this.apellidoRemitente = '';
        this.nombreDestinatario = '';
        this.apellidoDestinatario = '';
        this.fechaIngreso = '';
        this.fechaCaducidad = '';
        this.cantIndMov = 0;
        this.lote = '';
        this.referenciaInterna = '';
        this.referenciaExterna = '';
        this.codigoQR = '';
    }
}
//#endregion

//#region  Request and response del web method "searchMovimientoDropDown"
export class SearchComboMovimientoRequest {
    tipoMovimiento: number;

    constructor() {
        this.tipoMovimiento = 0;
    }
}

export class SearchDataEtiquetaRequest{
    nombreAgru: string;
    referenciaInt: number;
    referenciaExt: number;

    constructor(){
        this.nombreAgru = "";
        this.referenciaExt=0;
        this.referenciaInt=0;
    }
}

export class SearchComboMovimientoResponse {
    tiposMovimientosDataComboList: TraceITListDropDown[];
    productosDataComboList: TraceITListDropDown[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.tiposMovimientosDataComboList = [];
        this.productosDataComboList = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}
//#endregion

//#region Mostrar datos de un solo movimientos (separados)
// Request and response del web method "searchDataMovimientoGeneral"
export class SearchDataMovimientoGeneralRequest{
    movimientoId: number;

    constructor(){
        this.movimientoId = 0;
    }
}

export class SearchDataMovimientoGeneralResponse {
    movimientosDataGeneralRecepList: MovimientosDataGeneral;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataGeneralRecepList = new MovimientosDataGeneral();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class MovimientosDataGeneral {
    movimientoId: number;
    nombreAgrupacion: string;
    referenciaInterna: number;
    referenciaExterna: number;
    numeroPallet: number;
    numeroCajas: number;
    producto: string;
    cantidad: number;
    tipoMovimiento: number;
    fechaIngreso: string;
    fechaCaducidad: string;
    codigoQR: string;
    codigoI: string;
    codigoF: string;
    tipoRecepcion: number;
    cosechero: string;
    sector: string;
    fechaProduccion: string;
    productosRecibidos: number;
    isAgro: boolean

    constructor() {
        this.movimientoId = 0;
        this.nombreAgrupacion = '';
        this.referenciaInterna = 0;
        this.referenciaExterna = 0;
        this.numeroPallet = 0;
        this.numeroCajas = 0;
        this.producto = '';
        this.cantidad = 0;
        this.tipoMovimiento = 0;
        this.fechaIngreso = '';
        this.fechaCaducidad = '';
        this.codigoQR = '';
        this.codigoI = '';
        this.codigoF = '';
        this.tipoRecepcion = 0;
        this.cosechero = '';
        this.sector = '';
        this.fechaProduccion = '';
        this.productosRecibidos = 0;
        this.isAgro = false;
    }
}

export class SearchDataMovimientoGeneralRecepRequest{
    codigoQR: string;
    codigoI: string;
    codigoF: string;

    constructor(){
        this.codigoQR = "";
        this.codigoI = "";
        this.codigoF = "";
    }
}

export class SearchDataMovimientoGeneralRecepResponse {
    movimientosDataGeneralRecepList: MovimientosDataGeneral;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataGeneralRecepList = new MovimientosDataGeneral();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class SearchDataMovimientoGeneralRecepResponse2 {
    movimientosDataGeneralRecepList: MovimientosDataGeneral[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataGeneralRecepList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

// Request and response del web method "searchDataMovimientoGeneral"
export class SearchDataMovimientoObservacionesRequest{
    movimientoId: number;

    constructor(){
        this.movimientoId = 0;
    }
}

export class SearchDataMovimientoObservacionesRecepRequest{
    codigoQR: string;

    constructor(){
        this.codigoQR = "";
    }
}

export class SearchDataMovimientoObservacionResponse {
    movimientosDataObservacionRecepList: MovimientosDataObservacion;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataObservacionRecepList = new MovimientosDataObservacion();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class MovimientosDataObservacion {
    movimientoId: number;
    observacion: string;
    dimensionesCaja: string;
    pesoCaja: string;

    constructor() {
        this.movimientoId = 0;
        this.observacion = '';
        this.dimensionesCaja = '';
        this.pesoCaja = '';
    }
}



///////////////////////////////77
export class SearchDataMovimientoNombreRecepRequest{
    codigoCompleto: string;

    constructor(){
        this.codigoCompleto = "";
    }
}

export class SearchDataMovimientoNombreResponse {
    movimientosDataNombreRecepList: MovimientosDataNombre;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataNombreRecepList = new MovimientosDataNombre();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class MovimientosDataNombre {
    movimientoId: number;
    nombreAgrupacion: string;

    constructor() {
        this.movimientoId = 0;
        this.nombreAgrupacion = '';
    }
}


// Request and response del web method "searchDataMovimientoGeneralProd"
export class SearchDataMovimientoGeneralProdRequest{
    movimientoId: number;
    codigoI: string;
    codigoF: string;
    codigoTipo: string;
    codigoIHEXA: string;
    codigoFHEXA: string;
    isHexa: boolean;
    totalProductosQR: number;

    constructor(){
        this.movimientoId = 0;
        this.codigoI = '';
        this.codigoF = '';
        this.codigoTipo = "";
        this.codigoIHEXA = '';
        this.codigoFHEXA = '';
        this.totalProductosQR = 0;
        this.isHexa=false;
    }
}

export class SearchDataMovimientoGeneralProdResponse {
    movimientosDataGeneralProdRecepList: TraceITMovimientosDataGeneralProd[];
    movimientosDataGeneralDesProdRecepList : TraceITMovimientosDataGeneralProd[];
    movimientosDataGeneralUnicoProdRecepList : TraceITMovimientosDataGeneralProd[];
    movimientosDataGeneralOperaProdRecepList : TraceITMovimientosDataGeneralProd[];
    //movimientosDataGeneralReagrupadoProdRecepList : TraceITMovimientosDataGeneralProd[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataGeneralProdRecepList = [];
        this.movimientosDataGeneralDesProdRecepList = [];
        this.movimientosDataGeneralUnicoProdRecepList = [];
        this.movimientosDataGeneralOperaProdRecepList = [];
       // this.movimientosDataGeneralReagrupadoProdRecepList = []
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class MovimientosDataGeneralProd {
    movimientoId: number;
    producto: string;
    cantidad: number;

    constructor() {
        this.movimientoId = 0;
        this.producto = '';
        this.cantidad = 0;
    }
}

export class SearchDataMovimientoGeneralProdRecepRequest{
    codigoQR: string;
    codigoI: string;
    codigoF: string;
    codigoIHEXA: string;
    codigoFHEXA: string;
    isHexa: boolean;
    familiaProductoId: number;
    productoMovimientoId: number;

    constructor(){
        this.codigoQR = "";
        this.codigoI = "";
        this.codigoF = "";
        this.codigoIHEXA = "";
        this.codigoFHEXA = "";
        this.isHexa = false;
        this.familiaProductoId = 0;
        this.productoMovimientoId = 0;
    }
}

export class SearchDataMovimientoGeneralProdRecepResponse {
    movimientosDataGeneralProdRecepList: TraceITMovimientosDataGeneralProd[];
    movimientosDataGeneralDesProdRecepList : TraceITMovimientosDataGeneralProd[];
    movimientosDataGeneralUnicoProdRecepList : TraceITMovimientosDataGeneralProd[];
    movimientosDataGeneralOperaProdRecepList : TraceITMovimientosDataGeneralProd[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataGeneralProdRecepList = [];
        this.movimientosDataGeneralDesProdRecepList = [];
        this.movimientosDataGeneralUnicoProdRecepList = [];
        this.movimientosDataGeneralOperaProdRecepList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

// Request and response del web method "searchDataMovimientoRemitente"
export class SearchDataMovimientoRemitenteRequest{
    movimientoId: number;
    company: number;
    constructor(){
        this.movimientoId = 0;
    }
}

export class SearchDataMovimientoRemitenteResponse {
    movimientosDataRemitenteRecepList: MovimientosDataRemitente;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataRemitenteRecepList = new MovimientosDataRemitente();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class MovimientosDataRemitente {
    movimientoId: number;
    nombreRemitente: string;
    apellidoRemitente: string;
    nombreCompaniaR: string;
    rzCompaniaR: string;
    telefonoR: string;
    paisR: number;
    estadoR: number;
    paisNR: string;
    estadoNR: string;
    ciudadR: string;
    cpR: string;
    domicilioR: string;
    ranchoR: string;
    sectorR: string;
    nombreCompaniaHeader: string;

    constructor() {
        this.movimientoId = 0;
        this.nombreRemitente = '';
        this.apellidoRemitente = '';
        this.nombreCompaniaR = '';
        this.rzCompaniaR = '';
        this.telefonoR = '';
        this.paisR = 0;
        this.estadoR = 0;
        this.paisNR = '';
        this.estadoNR = '';
        this.ciudadR = '';
        this.cpR = '';
        this.domicilioR = '';
        this.ranchoR = '';
        this.sectorR = '';
        this.nombreCompaniaHeader = '';
    }
}

export class SearchDataMovimientoRemitenteRecepRequest{
    codigoQR: string;

    constructor(){
        this.codigoQR = "";
    }
}

export class SearchDataMovimientoRemitenteRecepResponse {
    movimientosDataRemitenteRecepList: MovimientosDataRemitente;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataRemitenteRecepList = new MovimientosDataRemitente();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

// Request and response del web method "searchDataMovimientoDestinatario"
export class SearchDataMovimientoDestinatarioRequest{
    movimientoId: number;

    constructor(){
        this.movimientoId = 0;
    }
}

export class SearchDataMovimientoDestinatarioResponse {
    movimientosDataDestinatarioRecepList: MovimientosDataDestinatario;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataDestinatarioRecepList = new MovimientosDataDestinatario();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class MovimientosDataDestinatario {
    movimientoId: number;
    idDestinatario: number;
    nombreDestinatario: string;
    apellidoDestinatario: string;
    nombreCompaniaD: string;
    rzCompaniaD: string;
    telefonoD: string;
    paisD: number;
    estadoD: number;
    paisND: string;
    estadoND: string;
    ciudadD: string;
    cpD: string;
    domicilioD: string;
    numeroC : string;
    ranchoD : string;
    sectorD : string;

    constructor() {
        this.movimientoId = 0;
        this.idDestinatario=0;
        this.nombreDestinatario = '';
        this.apellidoDestinatario = '';
        this.nombreCompaniaD = '';
        this.rzCompaniaD = '';
        this.telefonoD = '';
        this.paisD = 0;
        this.estadoD = 0;
        this.paisND = '';
        this.estadoND = '';
        this.ciudadD = '';
        this.cpD = '';
        this.domicilioD = '';
        this.numeroC = "";
        this.ranchoD = '';
        this.sectorD = '';
    }
}

export class SearchDataMovimientoDestinatarioRecepRequest{
    codigoQR: string;

    constructor(){
        this.codigoQR = "";
    }
}

export class SearchDataMovimientoDestinatarioRecepResponse {
    movimientosDataDestinatarioRecepList: MovimientosDataDestinatario;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataDestinatarioRecepList = new MovimientosDataDestinatario();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

// Request and response del web method "searchDataMovimientoTransportista"
export class SearchDataMovimientoTransportistaRequest{
    movimientoId: number;

    constructor(){
        this.movimientoId = 0;
    }
}

export class SearchDataMovimientoTransportistaRecepRequest{
    codigoQR: string;

    constructor(){
        this.codigoQR = "";
    }
}

export class SearchDataMovimientoTransportistaResponse {
    movimientosDataTransportistaList: MovimientosDataTransportista;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataTransportistaList = new MovimientosDataTransportista();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class MovimientosDataTransportista {
    movimientoId: number;
    transportista: string;
    numReferencia: string;
    fechaEmbarque: string;

    constructor() {
        this.movimientoId = 0;
        this.transportista = '';
        this.numReferencia = '';
        this.fechaEmbarque = '';

    }
}

// Request and response del web method "searchDataMovimientoInfoLegal"
export class SearchDataMovimientoInfoLegalRequest{
    movimientoId: number;

    constructor(){
        this.movimientoId = 0;
    }
}

export class SearchDataMovimientoInfoLegalResponse {
    movimientosDataInfoLegalRecepList: MovimientosDataInfoLegal;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataInfoLegalRecepList = new MovimientosDataInfoLegal();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class MovimientosDataInfoLegal {
    movimientoId: number;
    infoLegalId:number;
    tipoInfo: number;
    nombreInfo: string;
    direccionInfo: string;
    contactoInfo: string;
    nombreInfoExp: string;
    direccionInfoExp: string;
    contactoInfoExp: string;

    constructor() {
        this.movimientoId = 0;
        this.infoLegalId = 0;
        this.tipoInfo = 0;
        this.nombreInfo = '';
        this.direccionInfo = '';
        this.contactoInfo = '';
        this.nombreInfoExp = '';
        this.direccionInfoExp = '';
        this.contactoInfoExp = '';
    }
}

export class SearchDataMovimientoInfoLegalRecepRequest{
    codigoQR: string;

    constructor(){
        this.codigoQR = "";
    }
}

export class SearchDataMovimientoInfoLegalRecepResponse {
    movimientosDataInfoLegalRecepList: MovimientosDataInfoLegal;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientosDataInfoLegalRecepList = new MovimientosDataInfoLegal();
        this.messageEng = "";
        this.messageEsp = "";
    };
}


// Request and response del web method "searchDataMovimientoInfoLegal"
// export class MovimientosDataMerma {
//     producto: string;
//     merma:number;
//     productoMovimiento: number;

//     constructor() {
//         this.producto = "";
//         this.merma = 0;
//         this.productoMovimiento = 0;
//     }
// }

// export class SearchDataMovimientoMermaRequest{
//     codigoId: string;

//     constructor(){
//         this.codigoId = "";
//     }
// }

// export class SearchDataMovimientoMermaResponse {
//     movimientosDataMermaList: TraceITMovimientosDataMerma[];
//     messageEng: string;
//     messageEsp: string;

//     constructor() {
//         this.movimientosDataMermaList = [];
//         this.messageEng = "";
//         this.messageEsp = "";
//     };
// }


//#endregion

//#region  Request and response del web method "searchPaisEstadoDropDown"
export class SearchComboPaisEstadoRequest {
    paisId: number;

    constructor() {
        this.paisId = 0;
    }
}

export class SearchComboPaisEstadoResponse {
    paisesDataComboList: TraceITListDropDown[];
    estadosDataComboList: TraceITListDropDown[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.paisesDataComboList = [];
        this.estadosDataComboList = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}
//#endregion

//#region  Request and response del web method "searchPaisEstadoDropDown"
export class SearchRadioTipoInfoRequest {
    tipoInfoLegalId: number;

    constructor() {
        this.tipoInfoLegalId = 0;
    }
}

export class SearchRadioTipoInfoResponse {
    tipoInfoRadioList: TraceITListDropDown[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.tipoInfoRadioList = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}
//#endregion

//#region Edicion de datos de un solo movimiento (separados)
// Request and response del web method "editDataMovimientoGeneral"
export class EditDataMovimientoGeneralRequest {
    movimientoId: number;
    nombreAgrupacion: string;
    referenciaInterna: number;
    referenciaExterna: number;
    numeroPallet: number;
    numeroCajas: number;
    producto: string;
    cantidad: number;
    tipoMovimiento: number;
	fechaIngreso: string;
	fechaCaducidad: string;


    constructor() {
        this.movimientoId = 0;
        this.nombreAgrupacion = "";
        this.referenciaInterna = 0;
        this.referenciaExterna = 0;
        this.numeroPallet = 0;
        this.numeroCajas = 0;
        this.producto = "";
        this.cantidad = 0;
		this.tipoMovimiento = 0;
		this.fechaIngreso = "";
		this.fechaCaducidad = "";
    }
}

export class EditDataMovimientoGeneralResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}

// Request and response del web method "editDataMovimientoObservacion"
export class EditDataMovimientoObservacionRequest {
    movimientoId: number;
    observacion: string;

    constructor() {
        this.movimientoId = 0;
        this.observacion = "";
    }
}

export class EditDataMovimientoObservacionResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}


////////////////////////
// Request and response del web method "editDataMovimientoObservacion"
export class EditDataMovimientoNombreRequest {
    codigoCompleto: string;
    nombre: string;

    constructor() {
        this.codigoCompleto = "";
        this.nombre = "";
    }
}

export class EditDataMovimientoNombreResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}


// Request and response del web method "editDataMovimientoRemitente"
export class EditDataMovimientoRemitenteRequest {
    movimientoId: number;
    nombreRemitente: string;
    apellidoRemitente: string;
    nombreCompaniaR: string;
    rzCompaniaR: string;
    telefonoR: string;
    paisR: number;
    estadoR: number;
    ciudadR: string;
	cpR: string;
    domicilioR: string;
    ranchoR: string;
    sectorR:string;

    constructor() {
        this.movimientoId = 0;
        this.nombreRemitente = "";
        this.apellidoRemitente = "";
        this.nombreCompaniaR = "";
        this.rzCompaniaR = "";
        this.telefonoR = "";
        this.paisR = 0;
        this.estadoR = 0;
		this.ciudadR = "";
		this.cpR = "";
        this.domicilioR = "";
        this.ranchoR = "";
        this.sectorR = "";
    }
}

export class EditDataMovimientoRemitenteResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}

// Request and response del web method "editDataMovimientoDestinatario"
export class EditDataMovimientoDestinatarioRequest {
    movimientoId: number;
    nombreDestinatario: string;
    apellidoDestinatario: string;
    nombreCompaniaD: string;
    rzCompaniaD: string;
    telefonoD: string;
    paisD: number;
    estadoD: number;
    ciudadD: string;
	cpD: string;
    domicilioD: string;
    numeroC : string;
    ranchoD: string;
    sectorD:string;

    constructor() {
        this.movimientoId = 0;
        this.nombreDestinatario = "";
        this.apellidoDestinatario = "";
        this.nombreCompaniaD = "";
        this.rzCompaniaD = "";
        this.telefonoD = "";
        this.paisD = 0;
        this.estadoD = 0;
		this.ciudadD = "";
		this.cpD = "";
        this.domicilioD = "";
        this.numeroC ="";
        this.ranchoD ="";
        this.sectorD ="";
    }
}

export class EditDataMovimientoDestinatarioResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}

// Request and response del web method "editDataMovimientoTransportista"
export class EditDataMovimientoTransportistaRequest {
    movimientoId: number;
    transportista: string;
    numReferencia: string;
    fechaEmbarque: string;

    constructor() {
        this.movimientoId = 0;
        this.transportista = "";
        this.numReferencia = "";
        this.fechaEmbarque = "";
    }
}

export class EditDataMovimientoTransportistaResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}

// Request and response del web method "editDataMovimientoInfoLegal"
export class EditDataMovimientoInfoLegalRequest {
    movimientoId: number;
    tipoInfo: number;
    nombreInfo: string;
    direccionInfo: string;
    contactoInfo: string;
    nombreInfoExp: string;
    direccionInfoExp: string;
    contactoInfoExp: string;

    constructor() {
        this.movimientoId = 0;
        this.tipoInfo = 0;
        this.nombreInfo = "";
        this.direccionInfo = "";
        this.contactoInfo = "";
        this.nombreInfoExp = "";
        this.direccionInfoExp = "";
        this.contactoInfoExp = "";
    }
}

export class EditDataMovimientoInfoLegalResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}


// export class EditDataMovimientoMermaRequest{
//     codigoId : string;
//     merma : number;
//     productoMovimientoId : number

//     constructor(){
//         this.codigoId = "";
//         this.merma = 0;
//         this.productoMovimientoId = 0;
//     }
// }

// export class EditDataMovimientoMermaResponse {
//     messageEng: string;
//     messageEsp: string;

//     constructor() {
//         this.messageEng = "";
//         this.messageEsp = "";
//     };
// }
//#endregion

//#region  Request and response del web method "searchDocsInfoLegal"
export class SearchDocsInfoLegalRequest{
    movimientoId: number;


    constructor(){
        this.movimientoId = 0;
    }
}

export class SearchDocsInfoLegaloResponse {
    docsInfoLegalDataList: DocsInfoLegalData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docsInfoLegalDataList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class DocsInfoLegalData {
    movimientoId: number;
    docId: number;
    fechaDoc: string;
    rutaDoc: string;
    Nombredoc: string;
    tipoArchivo: number;
    tipoArchivoName: string;

    constructor() {
        this.movimientoId = 0;
        this.docId = 0;
        this.fechaDoc = "";
        this.rutaDoc = "";
        this.Nombredoc = '';
        this.tipoArchivo = 0;
        this.tipoArchivoName = "";
    }
}
//#endregion

//#region  Request and response del web method "searchExistenciaEstimada"
export class SearchExistenciaEstimadaRequest{
    nombreFamiliaCIU: string;
    fechaCaducidad: string;
    fechaIngresoDe: string;
    fechaIngresoHasta: string;
    ordenar: number;
    usuario:number;

    constructor(){
        this.nombreFamiliaCIU = "";
        this.fechaCaducidad = "";
        this.fechaIngresoDe = "";
        this.fechaIngresoHasta = "";
        this.ordenar = 0;
        this.usuario=0;
    }
}

export class SearchExistenciaEstimadaResponse {
    existenciaEstimadaDataList: ExistenciaEstimadaData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.existenciaEstimadaDataList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class ExistenciaEstimadaData {
    movimientoId: number;
    producto: string;
    cajas: string;
    cantidad: string;
    fechaIngreso: string;
    fechaCaducidad: string;
    lote: string;
    cantIndMov:number;
    nombreAgrupacion: string;

    constructor() {
        this.producto = "";
        this.cajas = "";
        this.cantidad = "";
        this.fechaIngreso = "";
        this.fechaCaducidad = "";
        this.lote = "";
        this.movimientoId = 0;
        this.cantIndMov = 0;
        this.nombreAgrupacion ="";
    }
}
//#endregion

//#region  Request and response del web method "saveDocsInfo"
export class SaveDocsInfoLegalRequest{
    doc: string;
    fecha: string;
    infoLegalId: number;
    nombreDoc : string;

    constructor(){
        this.doc = "";
        this.fecha = "";
        this.infoLegalId = 0;
        this.nombreDoc = "";
    }
}

export class SaveDocsInfoLegalResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    };
}
//#endregion

//#region  Request and response del web method "saveMovimiento"
export class SaveMovimientoRequest{
    nombreAgrupacion: string;
    fechaIngreso: string;
    referenciaInt: number;
    referenciaExt : number;
    usuario : number;
    latitud:string;
    longitud:string;

    constructor(){
        this.nombreAgrupacion = "";
        this.fechaIngreso = "";
        this.referenciaInt = 0;
        this.referenciaExt = 0;
        this.usuario = 0;
        this.latitud = "";
        this.longitud = "";
    }
}

export class SaveMovimientosResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    };
}
//#endregion

// Request and response del web method "editDataEnvioARecep y editDataAgruAEnvio"
export class EditDataAgruAEnvioRequest {
    movimientoId: number;
    usuario: number;
    fecha: string;
    noEsnormal: boolean;
    codigoI: string;
    codigoF: string;
    codigoTipo: string;
	latitud: string;
    longitud: string;
    codigoCompleto: string;
    nombre:string;
    caja:string;
    codigoIHEXA: string;
    codigoFHEXA: string;
    isHexa: boolean;

    constructor() {
        this.movimientoId = 0;
        this.usuario = 0;
        this.fecha = "";
        this.noEsnormal = false;
        this.codigoI = "";
        this.codigoF = "";
        this.codigoTipo = "";
        this.latitud="";
        this.longitud="";
        this.codigoCompleto = "";
        this.nombre ="";
        this.caja="";
        this.codigoIHEXA ="";
        this.codigoFHEXA="";
        this.isHexa=false;
    }
}

export class EditDataAgruAEnvioResponse {
    movimientoId: number;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.movimientoId = 0;
        this.messageEng = "";
        this.messageEsp = "";
    }
}

//#region  Request and response del web method "saveUsuario"
export class SaveUsuarioRequest{
    nombreComp: string;
    razonSocial: string;
    correoComp: string;
    telefono: string;
    direccion: string;
    nombrePais: string;

    codigoPostal: string;
    ciudad: string;
    nombreEstado: string;

    email1: string;
    pass1: string;

    email2: string;
    pass2: string;

    email3: string;
    pass3: string;

    email4: string;
    pass4: string;

    email5: string;
    pass5: string;

    usuario: number;

    constructor(){
        this.nombreComp = "";
        this.razonSocial = "";
        this.correoComp = "";
        this.telefono = "";
        this.direccion = "";
        this.nombrePais = "";

        this.codigoPostal = "";
        this.ciudad = "";
        this.nombreEstado = "";

        this.email1 = "";
        this.pass1 = "";

        this.email2 = "";
        this.pass2 = "";

        this.email3 = "";
        this.pass3 = "";

        this.email4 = "";
        this.pass4 = "";

        this.email5 = "";
        this.pass5 = "";

        this.usuario=0;
    }
}

export class SaveUsuarioResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    };
}
//#endregion

//#region  Request and response del web method "saveUsuarioInvitado"
export class SaveUsuarioInvitadoRequest{
    email1: string;
    pass1: string;

    constructor(){
        this.email1 = "";
        this.pass1 = "";
    }
}

export class SaveUsuarioInvitadoResponse {
    usuarioInvitadoList: usuarioInvitadoData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.usuarioInvitadoList = new usuarioInvitadoData();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class usuarioInvitadoData {
    email: string;
    password: string;
    usuario: number;

    constructor() {
        this.email = "";
        this.password = "";
        this.usuario = 0;
    }
}

export class SearchDataUsuarioInvitadoResponse{
    dataUsuarioInvitadoList: usuarioInvitadoData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.dataUsuarioInvitadoList = new usuarioInvitadoData();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

//Eliminar todos los datos evento 10 hrs
export class DeleteUsuarioInvitadoRequest {
    email1:string;
    usuario:number;

    constructor() {
        this.email1 = "";
        this.usuario = 0;
    }
}

export class DeleteUsuarioInvitadoResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}
//#endregion

//#region Request and response del web method "searchDocDetalleProducto y totalex"
export class SearchDocDetalleProductosRequest{
    movimientoId: number;

    constructor(){
        this.movimientoId = 0;
    }
}

export class SearchDocDetalleProductosRecepRequest{
    codigoQR: string;

    constructor(){
        this.codigoQR = "";
    }
}

export class SearchDocDetalleProductosAgruRequest{
    idFicha: number;

    constructor(){
        this.idFicha = 0;
    }
}

export class SearchDocDetalleProductosIndiRequest{
    movimientoId: string;
    codigoTipo: string;
    codigoI: string;
    codigoF: string;

    constructor(){
        this.movimientoId = "";
        this.codigoTipo = "";
        this.codigoI = "";
        this.codigoF = "";
    }
}

export class SearchDocDetalleProductosIndiResponse{
    docDetalleProductoIndiList: DocDetalleProductoData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docDetalleProductoIndiList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class SearchDocDetalleProductosCajasResponse{
    docDetalleProductoCajasList: DocDetalleProductoData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docDetalleProductoCajasList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}


export class SearchDocDetalleProductosResponse {
    docDetalleProductoList: DocDetalleProductoData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docDetalleProductoList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class DocDetalleProductoData {
    producto: string;
    numPallet: number;
    numCajas: number;
    cantidad: number;
    pesoBruto: number;
    dimensiones: string;
    fechaCaducidad: string;
    lote: string;
    ciu:string;
    numSerie:string;
    codigoQR:string;
    gtin:string;
    tipoEmpaque:string;
    codigoPallet:string;
    codigoCaja:string;

    constructor() {
        this.producto = "";
        this.numPallet = 0;
        this.numCajas = 0;
        this.cantidad = 0;
        this.pesoBruto = 0;
        this.dimensiones = "";
        this.fechaCaducidad = "";
        this.lote = "";
        this.ciu="";
        this.numSerie ="";
        this.codigoQR ="";
        this.gtin="";
        this.tipoEmpaque="";
        this.codigoPallet ="";
        this.codigoCaja ="";
    }
}

//Response total productos
export class SearchDocDetalleTotalProdResponse {
    docDetalleTotalProdList: DocDetalleTotalProdData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docDetalleTotalProdList = new DocDetalleTotalProdData();
        this.messageEng = "";
        this.messageEsp = "";
    };
}


export class DocDetalleTotalProdData {
    totalProducto: number;

    constructor() {
        this.totalProducto = 0;
    }
}

//Response total pallets
export class SearchDocDetalleTotalPalletResponse {
    docDetalleTotalPalletList: DocDetalleTotalPalletData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docDetalleTotalPalletList = new DocDetalleTotalPalletData();
        this.messageEng = "";
        this.messageEsp = "";
    };
}


export class DocDetalleTotalPalletData {
    totalPallet: number;

    constructor() {
        this.totalPallet = 0;
    }
}

//Response total cajas
export class SearchDocDetalleTotalCajasResponse {
    docDetalleTotalCajasList: DocDetalleTotalCajasData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docDetalleTotalCajasList = new DocDetalleTotalCajasData();
        this.messageEng = "";
        this.messageEsp = "";
    };
}


export class DocDetalleTotalCajasData {
    totalCajas: number;

    constructor() {
        this.totalCajas = 0;
    }
}

//Response total cantidad
export class SearchDocDetalleTotalCantidadResponse {
    docDetalleTotalCantidadList: DocDetalleTotalCantidadData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docDetalleTotalCantidadList = new DocDetalleTotalCantidadData();
        this.messageEng = "";
        this.messageEsp = "";
    };
}


export class DocDetalleTotalCantidadData {
    totalCantidad: number;

    constructor() {
        this.totalCantidad = 0;
    }
}

//Response total del peso bruto
export class SearchDocDetalleTotalPesoResponse {
    docDetalleTotalPesoList: DocDetalleTotalPesoData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docDetalleTotalPesoList = new DocDetalleTotalPesoData();
        this.messageEng = "";
        this.messageEsp = "";
    };
}


export class DocDetalleTotalPesoData {
    totalPeso: number;

    constructor() {
        this.totalPeso = 0;
    }
}

//Response fecha de caducidad minima
export class SearchDocDetalleFechaMinResponse {
    docDetalleFechaMinList: DocDetalleFechaMinData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.docDetalleFechaMinList = new DocDetalleFechaMinData();
        this.messageEng = "";
        this.messageEsp = "";
    };
}


export class DocDetalleFechaMinData {
    fechaMin: number;

    constructor() {
        this.fechaMin = 0;
    }
}
//#endregion

//#region  Request and response del web method "searchListDestinatarios"
export class SearchListDestinatariosRequest{
    usuario: number;

    constructor(){
        this.usuario=0;
    }
}

export class SearchListDestinatariosRequest2{
    usuario: number;

    constructor(){
        this.usuario=0;
    }
}

export class SearchListDestinatariosResponse {
    destinatariosDataList: ListDestinatariosData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.destinatariosDataList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class SearchListDestinatariosResponse2 {
    destinatariosDataList2: ListDestinatariosData2[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.destinatariosDataList2 = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class ListDestinatariosData {
    
    razonSocial : string;
    nombreDes : string;
    apellidoDes : string;
    telefono : string;
    pais : number;
    estado : number;
    npais : string;
    nestado : string;
    ciudad : string;
    codigoPostal : number;
    direccion : string;
    usuario : number;
    numeroC : string;
    rancho : string;
    sector : string;

    constructor() {
        this.razonSocial = "";
        this.nombreDes = "";
        this.apellidoDes = "";
        this.telefono = "";
        this.pais = 0;
        this.estado = 0;
        this.ciudad = "";
        this.codigoPostal = 0;
        this.direccion = "";
        this.usuario = 0;
        this.numeroC = "";
        this.rancho = "";
        this.sector = "";
    }
}

export class ListDestinatariosData2 {
    
    razonSocial : string;
    nombreDes : string;
    apellidoDes : string;
    telefono : string;
    pais : number;
    estado : number;
    npais : string;
    nestado : string;
    ciudad : string;
    codigoPostal : number;
    direccion : string;
    usuario : number;
    numeroC : string;
    rancho : string;
    sector : string;

    constructor() {
        this.razonSocial = "";
        this.nombreDes = "";
        this.apellidoDes = "";
        this.telefono = "";
        this.pais = 0;
        this.estado = 0;
        this.ciudad = "";
        this.codigoPostal = 0;
        this.direccion = "";
        this.usuario = 0;
        this.numeroC = "";
        this.rancho = "";
        this.sector = "";
    }
}
//#endregion


//#region  Request and response del web method "saveEstadosSiNoExiste"
export class SaveEstadosSiNoExisteRequest{
    pais: number;
    nombreEstado: string;

    constructor(){
        this.pais = 0;
        this.nombreEstado = "";
    }
}

export class SaveEstadosSiNoExisteResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    };
}
//#endregion

//#region  Request and response del web method "saveEstadosSiNoExiste"
export class SearchExistenciaEstadoRequest{
    pais: number;
    nombreEstado: string;

    constructor(){
        this.pais = 0;
        this.nombreEstado = "";
    }
}

export class SearchExistenciaEstadoResponse {

    existenciaEstadoList: EstadoExistencia;
    messageEng: string;
    messageEsp: string;

    constructor() {
        
        this.existenciaEstadoList = new EstadoExistencia();
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class EstadoExistencia {
    bandera: boolean;

    constructor() {
        this.bandera = false;
    }
}
//#endregion