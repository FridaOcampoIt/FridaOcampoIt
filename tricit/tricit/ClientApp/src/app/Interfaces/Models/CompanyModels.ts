//Request and response del web method "searchCompany"
export class SearchCompanyRequest {
    name: string;
    businessName: string;
    packedId: number; //Valor, para buscar las compañias ligadas ha un empacador
    constructor() {
        this.businessName = "";
        this.name = "";
        this.packedId = 0;
    };
}

export class SearchCompanyResponse {
    companiesDataList: CompaniesData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.companiesDataList = [];
        this.messageEng = "";
        this.messageEsp = "";
    };
}

export class CompaniesData {
    idCompany: number;
    name: string;
    businessName: string;
    phone: string;

    constructor() {
        this.idCompany = 0;
        this.name = "";
        this.businessName = "";
        this.phone = "";
    }
}

//Request and response del web method "searchCompanyData"
export class SearchCompanyDataRequest {
    idCompany: number;

    constructor() {
        this.idCompany = 0;
    }
}

export class SearchCompanyDataResponse {
    companyData: CompanyDataEdition;
    contactData: ContactCompaniesData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.companyData = new CompanyDataEdition();
        this.contactData = new ContactCompaniesData();
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class CompanyDataEdition {
    idCompany: number;
    name: string;
    businessName: string;
    email: string;
    webSite: string;
    phone: string;
    country: string;
    address: string;
    status: boolean;
	facebook: string;
	youtube: string;
    linkedin: string;
    clientNumber: string;
    tipoGiro: number;
    

    constructor() {
        this.idCompany = 0;
        this.address = "";
        this.businessName = "";
        this.country = "";
        this.email = "";
        this.name = "";
        this.phone = "";
        this.status = true;
        this.webSite = "";
		this.facebook = "";
		this.youtube = "";
        this.linkedin = "";
        this.clientNumber = "";
        this.tipoGiro=0;
    }
}

export class ContactCompaniesData {
    idContactFirst: number;
    contactNameFirst: string;
    contactPhoneFirst: string;
    contactEmailFirst: string;
    defaultFirst: boolean;
    idContactSecond: number;
    contactNameSecond: string;
    contactPhoneSecond: string;
    contactEmailSecond: string;
    defaultSecond: boolean;

    constructor() {
        this.idContactFirst = 0;
        this.contactNameFirst = "";
        this.contactPhoneFirst = "";
        this.contactEmailFirst = "";
        this.defaultFirst = false;
        this.idContactSecond = 0;
        this.contactNameSecond = "";
        this.contactPhoneSecond = "";
        this.contactEmailSecond = "";
        this.defaultSecond = false;
    }
}

//Request and response del web method "saveCompany", "editCompany", "deleteCompany"
export class SaveCompanyRequest {
    name: string;
    businessName: string;
    email: string;
    webSite: string;
    phone: string;
    country: string;
    address: string;
    status: boolean;
	facebook: string;
	youtube: string;
    linkedin: string;
    clientNumber: string;
    contactCompanies: ContactCompaniesData;
    tipoGiro: number;

    constructor() {
        this.name = "";
        this.businessName = "";
        this.email = "";
        this.webSite = "";
        this.phone = "";
        this.country = "";
        this.address = "";
        this.status = false;
		this.facebook = "";
		this.youtube = "";
        this.linkedin = "";
        this.clientNumber = "";
        this.contactCompanies = new ContactCompaniesData();
        this.tipoGiro=0;
    }
}

export class EditCompanyRequest {
    idCompany: number;
    name: string;
    businessName: string;
    email: string;
    webSite: string;
    phone: string;
    country: string;
    address: string;
    status: boolean;
	facebook: string;
	youtube: string;
    linkedin: string;
    clientNumber: string;
    contactCompanies: ContactCompaniesData;
    tipoGiro: number;

    constructor() {
        this.name = "";
        this.businessName = "";
        this.email = "";
        this.webSite = "";
        this.phone = "";
        this.country = "";
        this.address = "";
        this.status = false;
		this.facebook = "";
		this.youtube = "";
        this.linkedin = "";
        this.clientNumber = "";
        this.contactCompanies = new ContactCompaniesData();
        this.tipoGiro=0;
    }
}

export class DeleteCompanyRequest {
    idCompany: number;

    constructor() {
        this.idCompany = 0;
    }
}

export class CompanyProcessResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}
