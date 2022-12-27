import { constructDependencies } from "@angular/core/src/di/reflective_provider";

export class LabelConstants{
	readonly STATUS_SOLICITADO: number = 1;
	readonly STATUS_APROBADO: number = 2;
	readonly STATUS_REALIZADO: number = 3;
}

export class SearchLabelDropDownRequest {
	company: number;
	constructor() {
		this.company = 0;
	}
}

export class companyOptionData{
	companiaId: number;
	nombre: string;
	razonSocial: string;

	constructor(){
		this.companiaId = 0;
		this.nombre = "";
		this.razonSocial = "";
	}
}

export class SearchLabelDropDownResponse{
	listaCompanias: companyOptionData[];
	messageEng: string;
	messageEsp: string;

	constructor(){
		this.listaCompanias = [];
		this.messageEng = "";
		this.messageEsp = "";
	}
}

export class SearchLabelRequest{
	folio: string;
	companiaId: number;

	constructor(){
		this.folio = "";
		this.companiaId = 0;
	}
}

export class AddBitacoraRequest{
	estatusSolicitudId: number;
	descripcion: string;
	solicitudId: number;
	usuarioId: number;

	constructor(){
		this.estatusSolicitudId = 0;
		this.descripcion = "";
		this.solicitudId = 0;
		this.usuarioId = 0;
	}

}

export class Bitacora{
	fechaGeneracion: Date;
	estatusSolicitudId : number;
	status : string;
	descripcion : string;
	solicitudId : number;
	usuarioId : number;
	usuario: string;

	constructor(){
		this.fechaGeneracion = new Date();
		this.estatusSolicitudId = 0;
		this.status = "";
		this.descripcion = "";
		this.solicitudId = 0;
		this.usuarioId = 0;
		this.usuario = "";
	}
}

export class HistoryBitacoraResponse{
	listaBitacoras: Bitacora[];
	constructor(){
		this.listaBitacoras = [];
	}
}

export class LabelListResponse{
	listaSolicitudEtiquetas: LabelData[];
	messageEng: string;
	messageEsp: string;

	constructor(){
		this.listaSolicitudEtiquetas = [];
		this.messageEng = "";
		this.messageEsp = "";
	}
}

export class LabelData{
	solicitudId: number;
	folio: string;
	fechaGeneracion: Date;
	estatusSolicitudId: number;
	status: string;
	familiaId: number;
	familia: string;
	modelo: string;
	companiaId: number;
	companiaNombre: string;
	razonSocial: string;
	usuarioId: number;
	direccionId: number;
	udid: string;
	caducidad: Date;
	cantidad: number;
	byFile: boolean;
	Base64File: string;

	constructor(){
		this.solicitudId= 0;
		this.folio = "";
		this.fechaGeneracion = new Date();
		this.estatusSolicitudId = 0;
		this.status = "";
		this.familiaId= 0;
		this.familia= "";
		this.modelo = "";
		this.companiaId = 0;
		this.companiaNombre = "";
		this.razonSocial= "";
		this.usuarioId = 0;
		this.direccionId = 0;
		this.udid = "";
		this.caducidad = new Date();
		this.cantidad = 0;
		this.byFile = false;
		this.Base64File = "";
	}
}

export class SearchLabelsResponse{
	listaSolicitudEtiquetas: LabelData[];
	messageEng: string;
	messageEsp: string;

	constructor(){
		this.listaSolicitudEtiquetas = [];
		this.messageEng = "";
		this.messageEsp = "";
	}
}
