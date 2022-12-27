import { TraceITListDropDown } from "./TraceITBaseModels";

export class SearchProductDropDownRequest {
	company: number;
	constructor() {
		this.company = 0;
	}
}

export class SearchOriginDropDownRequest {
	filter: number;
	constructor() {
		this.filter = 0;
	}
}

export class familyDropDown {
	id: number;
	data: string;
	extra: number;
}

export class SearchProductDropDownResponse{
	familyDropDown: familyDropDown[];
	originDropDown: TraceITListDropDown[];
	companyDropDown: [];
	messageEng: string;
	messageEsp: string;

	constructor(){
		this.familyDropDown = [];
		this.originDropDown = [];
		this.messageEng = "";
		this.messageEsp = "";
	}
}

export class SearchProductsRequest {
	idFamily: number;
	idCompany: number;
	udid: string;

	constructor() {
		this.idCompany = 0;
		this.idFamily = 0;
		this.udid="";
	}
}

export class SearchProductsResponse{
	products: ProductData[];
	messageEng: string;
	messageEsp: string;

	constructor(){
		this.products = [];
		this.messageEng = "";
		this.messageEsp = "";
	}
}

export class ProductData{
	ciu: string;
	productId: number;
	family: string;
	udid: string;

	constructor(){
		this.ciu = "";
		this.productId = 0;
		this.family = "";
		this.udid = "";
	}
}

export class ProductDataRequest{
	idProduct: number;

	constructor(){
		this.idProduct = 0;
	}
}

export class SearchProductDataResponse{
	productDetails: ProductDataEdition;
	messageEng: string;
	messageEsp: string;

	constructor(){
		this.productDetails = new ProductDataEdition();
		this.messageEng = "";
		this.messageEsp = "";
	}
}

export class ImportProductRequest{
	fileBase: string;
	familyProductId: number;
	directionId: number;
	amount: number;
	expiry: string;
	udid: string;
	byFile: boolean;
	userId: number;
	company: string;
	family: string;
	packagingId: number;
	columns: number;
	
	constructor(){
		this.fileBase = "";
		this.familyProductId = 0;
		this.directionId = 0;
		this.amount = 0;
		this.expiry = "";
		this.udid = "";
		this.byFile = true;
		this.userId = 0;
		this.company = "";
		this.family = "";
		this.packagingId = 0;
		this.columns = 0;
	}
}

export class ProductDataEdition{
	idProduct: number;
	udid: string;
	expirationDate: string
	//qrCode: string;
	familyId: number;
	directionId: number;
	f_data: string;
	f_extra: number;
	status: number;

	constructor(){
		this.idProduct = 0;
		this.udid = "";
		this.expirationDate = "";
		//this.qrCode = "";
		this.familyId = 0;
		this.directionId = 0;
		this.f_data = "";
		this.f_extra = 0;
		this.status = 1;
	}
}

export class ProductDataSave{
	udid: string;
	expirationDate: string
	//qrCode: string;
	familyProductId: number;
	directionId: number;

	constructor(){
		this.udid = "";
		this.expirationDate = "";
		//this.qrCode = "";
		this.familyProductId = 0;
		this.directionId = 0;
	}
}

export class ProcessResponse {
	messageEng: string;
	messageEsp: string;

	constructor() {
		this.messageEng = "";
		this.messageEsp = "";
	}
}

export class ProductIdList{
	idProducts: number[];
	
	constructor(){
		this.idProducts = [];
	}
}