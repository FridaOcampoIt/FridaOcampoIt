export class SearchReportsRequest {
	companiaFamilia: string;
	constructor() {
		this.companiaFamilia = '';
	}
}

export class ReportData{
	descripcion: string;
	notificacion: boolean;
	usuarioId: number;
	productoId: number[];
	companiaId: number;
	roboId: number;
	udid: string;
	nombreUsuario: string;
	fechaRobo: Date;
	fechaInicio: Date;
	fechaFin: Date;
	codigoQR: string;
	familiaId: number;
	offset: number;
	offsetciu: number;
	limit: number;
	limitciu: number;
	familia: string;
	compania: string
	usuarioSolicitud: string;
	tipoReporteNombre: string;
	tipoAlertaNombre: string;
	codigoAlerta: string;

	constructor(){
		this.descripcion = "";
		this.notificacion = false;
		this.usuarioId = 0;
		this.productoId = [];
		this.companiaId = 0;
		this.roboId = 0;
		this.udid = "";
		this.nombreUsuario = "";
		this.fechaRobo = new Date();
		this.fechaInicio = new Date();
		this.fechaFin = new Date();
		this.codigoQR = "";
		this.familiaId = 0;
		this.offset = 0;
		this.offsetciu = 0;
		this.limit = 0;
		this.limitciu = 0;
		this.familia = "";
		this.compania = "";
		this.usuarioSolicitud = "";
		this.tipoAlertaNombre = "";
		this.tipoReporteNombre = "";
		this.codigoAlerta = "";
	}
}

export class ReportList{
	listaRobo: ReportData[];
	messageEng: string;
	messageEsp: string;

	constructor(){
		this.listaRobo = [];
		this.messageEng = "";
		this.messageEsp = "";
	}
}

export class searchProductsRequest{
	familiaId: number;
	codigoQr: string;
	companyId: number;
	offset: number;
	offsetciu: number;
	limit: number;
	limitciu: number;

	constructor(){
		this.familiaId = 0;
		this.codigoQr = "";
		this.companyId = 0;
		this.offset = 0;
		this.offsetciu = 0;
		this.limit = 10;
		this.limitciu = 0;
	}
}

export class ProductsResponse{
	productoId: number;
	ciu: string;

	constructor(){
		this.productoId = 0;
		this.ciu = "";
	}
}

export class SelectorFamilyProducts{
	udid: string;
	notificacion: boolean;
	productos: ProductsResponse[];
	
	constructor(){
		this.udid = "";
		this.notificacion = false;
		this.productos = [];
	}
}

export class ListFamilyProducts{
	listaUdidsCius: SelectorFamilyProducts[];
	notificacion: boolean;
	descripcion: string;
	tituloAlerta: string;
	descripcionEN: string;
	tituloAlertaEN: string;
	tipoAlertaId: number;
	tipoReporteId: number;
	tipoAlertaNombre: string;
	tipoReporteNombre: string;
	codigoAlerta: string;
	nombreArchivo:string;
	companiaId: number;
	familiaId: number;
	notificarTracking: boolean;
	messageEng: string;
	messageEsp: string;
	usuarioSolicitud: string;
	listaCajas: any[];

	constructor(){
		this.listaUdidsCius = [];
		this.notificacion = false;
		this.descripcion = "";
		this.messageEng = "";
		this.messageEsp = "";
		this.tituloAlerta = "";
		this.descripcionEN = "";
		this.tituloAlertaEN = "";
		this.nombreArchivo = "";
		this.companiaId = 0;
		this.familiaId = 0;
		this.tipoAlertaId = 0;
		this.tipoReporteId = 0;
		this.tipoReporteNombre = '';
		this.tipoAlertaNombre = '';
		this.notificarTracking = false;
		this.usuarioSolicitud = "";
		this.listaCajas = [];
	}
}

export class SaveReportRequest{
	tituloAlertaES: string;
	descripcionES: string;
	tituloAlertaEN: string;
	descripcionEN: string;
	notificacionMovilCiu: boolean;
	notificacionTracking: boolean;
	usuarioCreadorId: number;
	companiaId: number;
	usuarioSolicitoId: number;
	tipoAlertaId: number;
	nombreArchivo: string;
	tipoReporteId: number;
	familiaId: number;
	codigoAlerta: string;
	agrupacionId: boolean;
	detalle: string;
	constructor(){
		this.tituloAlertaES = "";
		this.descripcionES = "";
		this.tituloAlertaEN = "";
		this.descripcionEN = "";
		this.notificacionMovilCiu = false;
		this.notificacionTracking = false;
		this.usuarioCreadorId = 0;
		this.companiaId = 0;
		this.usuarioSolicitoId = 0;
		this.tipoAlertaId = 0;
		this.nombreArchivo = "";
		this.tipoReporteId = 0;
		this.familiaId = 0;
		this.codigoAlerta = "";
		this.agrupacionId = false;
		this.detalle = "";
	}
}

export class LoadReportRequest{
	familiaId: number;
	roboId: number;
	offset: number;
	offsetciu: number;
	limit: number;
	limitciu: number;

	constructor(){
		this.familiaId = 0;
		this.roboId = 0;
		this.offset = 0;
		this.offsetciu = 0;
		this.limit = 10;
		this.limitciu = 0;
	}
}
