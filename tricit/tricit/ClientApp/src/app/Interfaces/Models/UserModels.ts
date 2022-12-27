import { TraceITListDropDown } from './TraceITBaseModels';

//Request and response del web method "searchUser"
export class SearchUserRequest {
    name: string;
    rol: number;
    company: number;

    constructor() {
        this.company = 0;
        this.name = "";
        this.rol = 0;
    }
}

export class SearchUserResponse {
    messageEng: string;
    messageEsp: string;
    dataUser: DataUserBackOffice[];

    constructor() {
        this.dataUser = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class DataUserBackOffice {
    name: string;
    email: string;
    profile: string;
    rol: string;
    company: string;
    idUser: number;

    constructor() {
        this.company = "";
        this.email = "";
        this.idUser = 0;
        this.name = "";
        this.profile = "";
        this.rol = "";
    }
}

//Request and response del web method "searchUserDropDown"
export class SearchUserDropDownRequest {
    company: number;
    user: number;
    option: number;

    constructor() {
        this.company = 0;
        this.option = 0;
        this.user = 0;
    }
}

export class SearchUserDropDownResponse {
    dropDown: UserDropDown;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.dropDown = new UserDropDown();
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class UserDropDown {
    rols: TraceITListDropDown[];
    companies: TraceITListDropDown[];
    profiles: TraceITListDropDown[];

    constructor() {
        this.companies = [];
        this.profiles = [];
        this.rols = [];
    }
}

//Request and response del web method "searchUserData"
export class SearchUserDataRequest {
    idUser: number;

    constructor() {
        this.idUser = 0;
    }
}

export class SearchUserDataResponse {
    dataUser: DataUserBackOfficeDatas;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.dataUser = new DataUserBackOfficeDatas();
        this.messageEsp = "";
        this.messageEng = "";
    }
}

export class DataUserBackOfficeDatas {
    idUser: number;
    name: string;
    lastName: string;
    email: string;
    position: string;
    company: number;
    rol: number;
    profile: number;
    acopiosIds: Array<number>;
    auxAcopiosIds: Array<number>;
    constructor() {
        this.company = 0;
        this.email = "";
        this.idUser = 0;
        this.lastName = "";
        this.name = "";
        this.position = "";
        this.profile = 0;
        this.rol = 0;
        this.acopiosIds = [];
        this.auxAcopiosIds = [];
    }
}

//Request and response del web method "saveUser"
export class SaveUserRequest {
    name: string;
    lastName: string;
    email: string;
    password: string;
    position: string;
    companyId: number;
    rolId: number;
    profile: number;
    acopiosIds: Array<number>;
    constructor() {
        this.companyId = 0;
        this.email = "";
        this.lastName = "";
        this.name = "";
        this.password = "";
        this.position = "";
        this.profile = 0;
        this.rolId = 0;
        this.acopiosIds = [];
    }
}

export class EditUserRequest {
    idUser: number;
    name: string;
    lastName: string;
    email: string;
    password: string;
    position: string;
    companyId: number;
    rolId: number;
    profile: number;
    acopiosIds: Array<number>;
    auxAcopiosIds: Array<number>;
    constructor() {
        this.idUser = 0;
        this.companyId = 0;
        this.email = "";
        this.lastName = "";
        this.name = "";
        this.password = "";
        this.position = "";
        this.profile = 0;
        this.rolId = 0;
        this.acopiosIds = [];
        this.auxAcopiosIds = [];
    }
}

export class DeleteUserRequest {
    idUser: number;

    constructor() {
        this.idUser = 0;
    }
}

export class UserProcessResponse {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}