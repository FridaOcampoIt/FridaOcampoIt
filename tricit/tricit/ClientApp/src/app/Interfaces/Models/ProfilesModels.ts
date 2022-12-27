import { TraceITListDropDown } from './TraceITBaseModels';

//Request and response del web method "searchProfile"
export class SearchProfileRequest {
    name: string;
    company: number;

    constructor() {
        this.company = 0;
        this.name = "";
    }
}

export class SearchProfileResponse {
    profiles: Profiles[];
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.profiles = [];
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class Profiles {
    profileId: number;
    name: string;
    company: string;

    constructor() {
        this.company = "";
        this.name = "";
        this.profileId = 0;
    }
}

//Request and response del web method "searchDropDownPermission"
export class SearchDropDownPermissionRequest {
    company: number;

    constructor() {
        this.company = 0;
    }
}

export class SearchDropDownPermissionResponse {
    dropDownProfilesPermission: DropDownProfilesPermission;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.dropDownProfilesPermission = new DropDownProfilesPermission();
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class DropDownProfilesPermission {
    companyList: TraceITListDropDown[];
    permissions: PermissionEstructure[];

    constructor() {
        this.companyList = [];
        this.permissions = [];
    }
}

export class PermissionEstructure {
    idPermission: number;
    name: string;
    permission: PermissionChildren[];

    constructor() {
        this.idPermission = 0;
        this.name = "";
        this.permission = [];
    }
}

export class PermissionChildren {
    idPermission: number;
    name: string;
    check: boolean;
	isForCompany: boolean;
    constructor() {
        this.idPermission = 0;
        this.name = "";
		this.check = false;
		this.isForCompany = false;
    }
}

//Request and response del web method "searchProfileData"
export class SearchProfileDataRequest {
    profileId: number;

    constructor() {
        this.profileId = 0;
    }
}

export class SearchProfileDataResponse {
    permissionProfileData: PermissionProfileData;
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.permissionProfileData = new PermissionProfileData();
        this.messageEsp = "";
        this.messageEng = "";
    }
}

export class PermissionProfileData {
    profileData: ProfilesData;
    permissions: PermissionData[];

    constructor() {
        this.permissions = [];
        this.profileData = new ProfilesData();
    }
}

export class ProfilesData {
    profileId: number;
    name: string;
    company: number;

    constructor() {
        this.company = 0;
        this.name = "";
        this.profileId = 0;
    }
}

export class PermissionData {
    permission: number;

    constructor() {
        this.permission = 0;
    }
}

//Request and response del web method "saveProfile", "editProfile", "deleteProfile"
export class SaveProfileRequest {
    name: string;
    company: number;
    permission: number[];

    constructor() {
        this.company = 0;
        this.name = "";
        this.permission = [];
    }
}

export class EditProfileRequest {
    profileId: number;
    name: string;
    company: number;
    permission: number[];

    constructor() {
        this.profileId = 0;
        this.company = 0;
        this.name = "";
        this.permission = [];
    }
}

export class SaveUpdateProccess {
    messageEng: string;
    messageEsp: string;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
    }
}

export class DeleteProfileRequest {
    profileId: number;

    constructor() {
        this.profileId = 0;
    }
}