//Modelos de base para utilizarlos en los dropdownList


//Modelo para la respuesta del listado de paises
export class SearchEstadosByPaisIdResponse {
    estadosData: TraceITListDropDown[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.estadosData = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}

//Modelos de base para utilizarlos en los dropdownList
export class TraceITListDropDown {
    id: number;
    data: string;
}

//Modelos de base para utilizarlos en las fichas
export class TraceITListFichas {
    movfichaId : number
    idFicha : number
    nombreFicha : string
    numeroPallets : number
    numeroCajas : number
    producto : string
    cantidad : number
    tipoFicha : number
    nombreTipoFicha : string
    lote : string
    fechaCaducidad : string
    numSerie : string
    usuario : number
}

//Modelo para la respuesta del listado de paises
export class SearchCountriesResponse {
    countriesData: TraceITListDropDown[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.countriesData = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}

//Modelos de base 
export class TraceITMovimientosDataGeneralProd {
    movimientoId : number;
    producto : string;
    cantidad : string;
	numPallet: number;
    numCajas: number;
    pesoBruto : number;
    dimensiones : string;
    fechaCaducidad : string;
    lote : string;
    ciu : string;
    numSerie : string;
    familiaProductoId: number;
    numeroCajas: number;
    embalajeId: number;
}

//Modelos de base de merma
export class TraceITMovimientosDataMerma {
    producto : string;
    merma : number;
    productoMovimiento : number;
    total : number;
}