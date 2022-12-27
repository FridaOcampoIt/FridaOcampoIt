import { TraceITListDropDown } from './TraceITBaseModels';

//Request and response del web method "searchDropDownListAddress"
export class SearchDropDownListAddressRequest {
    idCompany: number;

    constructor() {
        this.idCompany = 0;
    }
}

export class SearchDropDownListAddressResponse {
    companyData: TraceITListDropDown[];
    addressTypeData: TraceITListDropDown[];
    familyData: TraceITListDropDown[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.addressTypeData = [];
        this.companyData = [];
        this.familyData = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}

//Request and response del web method "searchAddress"
export class SearchAddressRequest {
    idFamily: number;
    idCompany: number;

    constructor() {
        this.idCompany = 0;
        this.idFamily = 0;
    }
}

export class SearchAddressResponse {
    addressDataList: AddressesData[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.addressDataList = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class AddressesData {
    serviceCenter: string;
    company: string;
    idAddress: number;
    type: string;

    constructor() {
        this.company = "";
        this.idAddress = 0;
        this.serviceCenter = "";
        this.type = "";
    }
}

//Request and response del web method "searchAddressData"
export class SearchAddressDataRequest {
    idAddress: number;

    constructor() {
        this.idAddress = 0;
    }
}

export class SearchAddressDataResponse {
    addressData: AddressDataEdition;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.addressData = new AddressDataEdition();
        this.messageEng = "";
        this.messageEsp = "";
    }
}


export class AddressDataEdition {
    idAddress: number;
    name: string;
    phone: string
    address: string;
    postalCode: string;
    city: string;
    state: string;
    country: string;
    latitude: string;
    longitude: string;
    status: boolean;
    companyId: number;
    directionType: number;

    constructor() {
        this.idAddress = 0;
        this.address = "";
        this.city = "";
        this.companyId = 0;
        this.country = "";
        this.latitude = "-34.397";
        this.longitude = "150.644";
        this.name = "";
        this.phone = "";
        this.postalCode = "";
        this.state = "";
        this.status = true;
        this.directionType = 0;
    }
}

//Request and response del web method "saveAddres", "editAddress", "deleteAddress"
export class SaveAddressRequest {
    name: string;
    phone: string;
    address: string;
    postalCode: string;
    city: string;
    state: string;
    country: string;
    latitude: string;
    longitude: string;
    status: boolean;
    idCompany: number;
    idTypeAddress: number;

    constructor() {
        this.name = "";
        this.phone = "";
        this.address = "";
        this.postalCode = "";
        this.city = "";
        this.state = "";
        this.country = "";
        this.latitude = "";
        this.longitude = "";
        this.status = false;
        this.idCompany = 0;
        this.idTypeAddress = 0;
    }
}

export class EditAddressRequest {
    idAddress: number;
    name: string;
    phone: string;
    address: string;
    postalCode: string;
    city: string;
    state: string;
    country: string;
    latitude: string;
    longitude: string;
    status: boolean;
    idCompany: number;
    idTypeAddress: number;

    constructor() {
        this.idAddress = 0;
        this.name = "";
        this.phone = "";
        this.address = "";
        this.postalCode = "";
        this.city = "";
        this.state = "";
        this.country = "";
        this.latitude = "";
        this.longitude = "";
        this.status = false;
        this.idCompany = 0;
        this.idTypeAddress = 0;
    }
}

export class DeleteAddressRequest {
    idAddress: number;

    constructor() {
        this.idAddress = 0;
    }
}

export class AddressProcessResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}