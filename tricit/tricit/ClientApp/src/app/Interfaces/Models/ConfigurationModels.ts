import { TraceITListDropDown } from "./TraceITBaseModels";

//Request and response del web method "searchDropDownConfiguration"
export class SearchDropDownConfigurationResponse {
    configuration: GeneralConfigurationDropDown;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.configuration = new GeneralConfigurationDropDown();
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class GeneralConfigurationDropDown {
    userTraceIT: TraceITListDropDown[];
    companyTraceIT: TraceITListDropDown[];
    generalConfigurations: GeneralConfigurationData[];

    constructor() {
        this.userTraceIT = [];
        this.companyTraceIT = [];
        this.generalConfigurations = [];
    }
}

export class GeneralConfigurationData {
    configuration: string;
    value: string[];

    constructor() {
        this.configuration = "";
        this.value = [];
    }
}

//Request and response del web method "searchDropDownConfiguration"
export class SearchConfigurationCompanyRequest {
    idCompany: number;

    constructor() {
        this.idCompany = 0;
    }
}

export class SearchConfigurationCompanyResponse {
    companyConfiguration: CompanyConfigurationData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.companyConfiguration = new CompanyConfigurationData();
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class CompanyConfigurationData {
    nUseGuides: number;
    nInstalationGuides: number;
    nRelatedProduct: number;
    notifyComments: string;
    notifyWarranty: string;
	notifyStolen: string;
	nPDF: number;
	nCharEspec: number;
	nImg: number;
	nCharFAQ: number;
	nVid: number;

    constructor() {
        this.nInstalationGuides = 0;
        this.notifyComments = "";
        this.notifyStolen = "";
        this.notifyWarranty = "";
        this.nRelatedProduct = 0;
		this.nUseGuides = 0;
		this.nPDF = 0;
		this.nCharEspec = 0;
		this.nImg = 0;
		this.nCharFAQ = 0;
		this.nVid = 0;
    }
}

//Request and response del web method "saveGeneralConfiguration"
export class SaveGeneralConfigurationRequest {
    generalConfiguration: SaveGeneralConfiguration[];

    constructor() {
        this.generalConfiguration = [];
    }
}

export class SaveGeneralConfigurationResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class SaveGeneralConfiguration {
    configuration: string;
    value: string;

    constructor() {
        this.configuration = "";
        this.value = "";
    }
}

//Request and response del web method "saveCongfigurationCompany"
export class SaveConfigurationCompanyRequest {
    company: number;
    nUseGuides: number;
    nInstalationGuides: number;
	nRelatedProduct: number;
	nPDF: number;
	nCharEspec: number;
	nImg: number;
	nVid: number;
	nCharFAQ: number;
    notifyComments: string;
    notifyWarranty: string;
    notifyStolen: string;

    constructor() {
        this.company = 0;
        this.nInstalationGuides = 0;
        this.notifyComments = "";
        this.notifyStolen = "";
        this.notifyWarranty = "";
        this.nRelatedProduct = 0;
		this.nUseGuides = 0;
		this.nPDF = 0;
		this.nCharEspec = 0;
		this.nImg = 0;
		this.nCharFAQ = 0;
		this.nVid = 0;
    }
}