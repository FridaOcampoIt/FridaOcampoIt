import { TraceITListDropDown } from './TraceITBaseModels'

//Request y response del web method "SearchFamilyProduct"
export class SearchFamilyProductRequest {
    name: string;
    companyId: number;

    constructor() {
        this.name = "";
        this.companyId = 0;
    }
}

export class SearchFamilyProductResponse {
    productFamilyData: ProductFamily[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.productFamilyData = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class ProductFamily {
    familyProductId: number;
    name: string;
    model: string;
    sku: string;
    gtin: string;
    companyId: number;

    constructor() {
        this.familyProductId = 0;
        this.name = "";
        this.model = "";
        this.sku = "";
        this.gtin = "";
        this.companyId = 0;
    }
}

//Request y response del web method "searchDropDownListFamily"
export class SearchDropDownListFamilyRequest {
    idCompany: number;
    option: number;

    constructor() {
        this.idCompany = 0;
        this.option = 0;
    }
}

export class SearchDropDownListFamilyResponse {
    companyData: TraceITListDropDown[];
    categoryData: TraceITListDropDown[];
    addressData: TraceITListDropDown[];
    recommendedBy: TraceITListDropDown[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.companyData = [];
        this.addressData = [];
        this.categoryData = [];
        this.recommendedBy = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}

//Request y response del web method "searchFamilyProductDate"
export class SearchFamilyProductDateRequest {
    familyId: number;

    constructor() {
        this.familyId = 0;
    }
}

export class SearchFamilyProductDateResponse {
    productFamily: FamilyDataEdition;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
        this.productFamily = new FamilyDataEdition();
    }
}

export class FamilyDataEdition {
    productFamilyData: ProductFamilyData;
	directionFamily: DirectionFamilyData[];
	limitsFamily: LimitsFamilyData;
	specCharUse: LimitUseFamily;
	imageUse: LimitUseFamily;

    constructor() {
        this.directionFamily = [];
		this.productFamilyData = new ProductFamilyData();
		this.limitsFamily = new LimitsFamilyData();
		this.specCharUse = new LimitUseFamily();
		this.imageUse = new LimitUseFamily();
    }
}

export class ProductFamilyData {
    familyProductId: number;
    name: string;
    model: string;
    description: string;
    descriptionEnglish: string;
    image: string;
    sku: string;
    gtin: string;
    status: boolean;
    warranty: boolean;
    expiration: boolean;
    addTicket: boolean;
    category: number;
    company: number;
    lifeDays: number;
    autoLote: boolean;
    editLote: boolean;
    consecutiveLote: number;
    prefix: string;
    colorFamilia: string;

    constructor() {
        this.addTicket = false;
        this.category = 0;
        this.company = 0;
        this.description = "";
        this.descriptionEnglish = "";
        this.expiration = false;
        this.familyProductId = 0;
        this.gtin = "";
        this.image = "";
        this.model = "";
        this.name = "";
        this.sku = "";
        this.status = true;
        this.warranty = false;
        this.autoLote = false;
        this.editLote = false;
        this.consecutiveLote = 100;
        this.prefix = "";
        this.colorFamilia = "";
    }
}

export class LimitsFamilyData {
	npdf : number;
	nCharEspec: number;
	nCharFAQ: number ;
	nImg: number;
	nVid: number;
	nProductosRelacionados: number;

	constructor() {
		this.npdf = 0;
		this.nCharEspec = 0;
		this.nCharFAQ = 0;
		this.nImg = 0;
		this.nVid = 0;
		this.nProductosRelacionados = 0;
	}
}

export class LimitUseFamily{
	totalUsed: number;
	constructor()
	{
		this.totalUsed = 0;
	}
}

export class DirectionFamilyData {
    directionId: number;

    constructor() {
        this.directionId = 0;
    }
}

//Request y response del web method "saveFamily" y "updateFamily"
export class SaveFamilyRequest {
    familyData: FamilyData;
    directionFamily: DirectionFamilyData[];

    constructor() {
        this.directionFamily = [];
        this.familyData = new FamilyData();
    }
}

export class UpdateFamilyRequest {
    familyData: FamilyData;
    directionFamily: DirectionFamilyData[];
    option: number;

    constructor() {
        this.directionFamily = [];
        this.familyData = new FamilyData();
        this.option = 0;
    }
}

export class FamilyProcessResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class FamilyData {
    familyId: number;
    name: string;
    model: string;
    description: string;
    descriptionEnglish: string;
    imageBaseFamily: string;
    sku: string;
    gtin: string;
    status: boolean;
    warranty: boolean;
    expiration: boolean;
    addTicket: boolean;
    category: number;
    company: number;
    lifeDays: number;
    autoLote: boolean;
    editLote: boolean;
    consecutiveLote: number;
    prefix: string;
    colorFamilia: string;
    constructor() {
        this.addTicket = false;
        this.category = 0;
        this.company = 0;
        this.description = "";
        this.descriptionEnglish = "";
        this.expiration = false;
        this.familyId = 0;
        this.gtin = "";
        this.imageBaseFamily = "";
        this.model = "";
        this.name = "";
        this.sku = "";
        this.status = false;
        this.warranty = false;
        this.lifeDays = 0;
        this.autoLote = false;
        this.editLote = false;
        this.consecutiveLote = 100;
        this.prefix = "";
        this.colorFamilia = "";
    }
}

export class DeleteFamilyRequest {
    familyId: number;

    constructor() {
        this.familyId = 0;
    }
}

//Request y response del web method "SearchTechnicalSpecificationFamily"
export class SearchTechnicalSpecificationFamilyRequest {
    familyId: number;

    constructor() {
        this.familyId = 0;
    }
}

export class SearchTechnicalSpecificationFamilyResponse {
    technicalSpecifications: TechnicalSpecificationFamilyData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
        this.technicalSpecifications = [];
    }
}

export class TechnicalSpecificationFamilyData {
    specificationTecnicalId: number;
    title: string;
    titleEnglish: string;
    technicalSpecificationDetails: TechnicalSpecificationDetailsFamilyData[];

    constructor() {
        this.specificationTecnicalId = 0;
        this.title = "";
        this.titleEnglish = "";
        this.technicalSpecificationDetails = [];
    }
}

export class TechnicalSpecificationDetailsFamilyData {
    specificationTechnicalDetailId: number;
    subtitle: string;
    subtitleEnglish: string;
    description: string;
    descriptionEnglish: string;
    image: string;

    constructor() {
        this.specificationTechnicalDetailId = 0;
        this.subtitle = "";
        this.subtitleEnglish = "";
        this.description = "";
        this.descriptionEnglish = "";
        this.image = "";
    }
}

//Request del web method "saveTechnicalSpecification",
export class SaveTechnicalSpecificationRequest {
    technicalSpecification: TechnicalSpecificationData;
    familyId: number;

    constructor() {
        this.familyId = 0;
        this.technicalSpecification = new TechnicalSpecificationData();
    }
}

//Request del web method "updateTechnicalSpecification"
export class UpdateTechnicalSpecificationRequest {
    technicalSpecification: TechnicalSpecificationData;


    constructor() {
        this.technicalSpecification = new TechnicalSpecificationData();
    }
}

//Request del web method "saveTechnicalSpecificationDetails"
export class SaveTechnicalSpecificationDetailsRequest {
    technicalSpecificationDetails: TechnicalSpecificationDetailsData;
    technicalSpecificationId: number;

    constructor() {
        this.technicalSpecificationDetails = new TechnicalSpecificationDetailsData();
        this.technicalSpecificationId = 0;
    }
}

//Request del web method "updateTechnicalSpecificationDetails"
export class UpdateTechnicalSpecificationDetailsRequest {
    technicalSpecificationDetails: TechnicalSpecificationDetailsData;
    imagenEliminado: boolean;

    constructor() {
        this.technicalSpecificationDetails = new TechnicalSpecificationDetailsData();
        this.imagenEliminado = false;
    }
}

//Request para el Web method "deleteTechnicalSpecification"
export class DeleteTechnicalSpecificationRequest {
    technicalSpecificationId: number;

    constructor() {
        this.technicalSpecificationId = 0;
    }
}

//Request para el web method "deleteTechnicalSpecificationDetails"
export class DeleteTechnicalSpecificationDetailsRequest {
    TechnicalSpecificationDetailsId: number;

    constructor() {
        this.TechnicalSpecificationDetailsId = 0
    }
}

export class TechnicalSpecificationData {
    specificationTechnicalId: number;
    title: string;
    titleEnglish: string;
    technicalEspecificationDetails: TechnicalSpecificationDetailsData[];

    constructor() {
        this.specificationTechnicalId = 0;
        this.title = "";
        this.titleEnglish = "";
        this.technicalEspecificationDetails = [];
    }
}

export class TechnicalSpecificationDetailsData {
    specificationTechnicalDetailId: number;
    subtitle: string;
    subtitleEnglish: string;
    description: string;
    descriptionEnglish: string;
    imageBase: string;

    constructor() {
        this.description = "";
        this.descriptionEnglish = "";
        this.specificationTechnicalDetailId = 0;
        this.imageBase = "";
        this.subtitle = "";
        this.subtitleEnglish = "";
    }
}

//Request y response para el web method "searchLinkFamily"
export class SearchLinkFamilyRequest {
    familyId: number;
    sectionType: number;
    linkType: number;

    constructor() {
        this.familyId = 0;
        this.linkType = 0;
        this.sectionType = 0;
    }
}

export class SearchLinkFamilyResponse {
	linkFamilies: SearchLinskData

    messageEsp: string;
    messageEng: string;

    constructor() {
		this.linkFamilies = new SearchLinskData();
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class SearchLinskData {
	linkData: LinkFamilyData[];
	limitsFamily: LimitsFamilyData;
	constructor() {
		this.linkData = [];
		this.limitsFamily = new LimitsFamilyData();
	}
}

export class LinkFamilyData {
    linkId: number;
    title: string;
    url: string;
    thumbailUrl: string;
    author: string;
    status: boolean;
    recommendedById: number;
    linkTypeId: number;
    recommendedBy: string;

    constructor() {
        this.author = "";
        this.linkId = 0;
        this.linkTypeId = 0;
        this.recommendedById = 0;
        this.status = false;
        this.thumbailUrl = "";
        this.title = "";
        this.url = "";
        this.recommendedBy = "";
    }
}

//Request para el web method "saveLink", "updateLink"
export class SaveLinkRequest {
    linkData: linkData[];
    idFamily: number;
    recommendedById: number;
	sectionType: number;
	userCompanyId: number;

    constructor() {
        this.idFamily = 0;
        this.linkData = [];
        this.recommendedById = 0;
		this.sectionType = 0;
		this.userCompanyId = -1;
    }
}

export class UpdateLinkRequest {
    linkData: linkData;
    recommendedById: number;

    constructor() {
        this.linkData = new linkData();
        this.recommendedById = 0;
    }
}

export class linkData {
    linkId: number;
    title: string;
    url: string;
    thumbailUrl: string;
    author: string;
    status: boolean;
    linkTypeId: number;

    constructor() {
        this.author = "";
        this.linkId = 0;
        this.linkTypeId = 0;
        this.status = false;
        this.thumbailUrl = "";
        this.title = "";
        this.url = "";
    }
}

//Request y response para el web method "youtubeSearch"
export class YoutubeSearchRequest{
    dataFilter: string;

    constructor() {
        this.dataFilter = "";
    }

}

export class YoutubeSearchResponse {
    youtubeData: YoutubeData[];
    messageEsp: string;
    messageEng: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
        this.youtubeData = [];
    }
}

export class YoutubeData {
    title: string;
    videoId: string;
    thumbnails: string;
    channelTitle: string;
    recommendedBy: string;

    constructor() {
        this.channelTitle = "";
        this.thumbnails = "";
        this.title = "";
        this.videoId = "";
        this.recommendedBy = "";
    }
}

//Request el web method "deleteLink";
export class DeleteLinkRequest {
    linkId: number

    constructor() {
        this.linkId = 0;
    }
}

//Request y response del web mthod "searchWarrantiesFaq"
export class SearchWarrantiesFaqRequest {
    familyId: number;

    constructor() {
        this.familyId = 0;
    }
}

export class SearchWarrantiesFaqResponse {
    warrantiesFaq: WarrantiesFaqFamily;

    constructor() {
        this.warrantiesFaq = new WarrantiesFaqFamily();
    }
}

export class WarrantiesFaqFamily {
    warranties: WarrantiesFamilyData[];
	frequentQuestions: FrequentQuestionsFamilyData[];
	limitsFamily: LimitsFamilyData;

    constructor() {
        this.frequentQuestions = [];
		this.warranties = [];
		this.limitsFamily = new LimitsFamilyData();
    }
}

export class WarrantiesFamilyData {
    warrantyId: number;
    country: string;
    urlPdf: string;
    periodMonths: number;

    constructor() {
        this.country = "";
        this.periodMonths = 0;
        this.urlPdf = "";
        this.warrantyId = 0;
    }
}

export class FrequentQuestionsFamilyData {
    faqId: number;
    questionSpanish: string;
    responseSpanish: string;
    questionEnglish: string;
    responseEnglish: string;

    constructor() {
        this.faqId = 0;
        this.questionEnglish = "";
        this.questionSpanish = "";
        this.responseEnglish = "";
        this.responseSpanish = "";
    }
}


//Request del web method "saveWarranty"
export class SaveWarrantyRequest {
    warranty: WarrantyData;
    familyId: number;

    constructor() {
        this.familyId = 0;
        this.warranty = new WarrantyData();
    }
}

export class WarrantyData {
    warrantyId: number;
    country: string;
    pdfBase: string;
    periodMonth: number;

    constructor() {
        this.country = "";
        this.pdfBase = "";
        this.periodMonth = 0;
        this.warrantyId = 0;
    }
}

//Request del web method "deleteWarranties"
export class DeleteWarrantiesRequest {
    warrantyId: number;

    constructor() {
        this.warrantyId = 0;
    }
}

//Request del web method "saveFrequentQuestions"
export class SaveFrequentQuestionsRequest {
    frequentQuestions: FrequentQuestions;
    familyId: number;

    constructor() {
        this.familyId = 0;
        this.frequentQuestions = new FrequentQuestions();
    }
}

export class FrequentQuestions {
    questionId: number;
    question: string;
    questionEnglish: string;
    response: string;
    responseEnglish: string;

    constructor() {
        this.question = "";
        this.questionEnglish = "";
        this.questionId = 0;
        this.response = "";
        this.responseEnglish = "";
    }
}

//Request del web method "updateFrequentQuestions"
export class UpdateFrequentQuestionRequest {
    frequentQuestions: FrequentQuestions;

    constructor() {
        this.frequentQuestions = new FrequentQuestions();
    }
}

//Request del web method "deleteFrequentQuestions"
export class DeleteFrequentQuestionsRequest {
    faqId: number;

    constructor() {
        this.faqId = 0;
    }
}