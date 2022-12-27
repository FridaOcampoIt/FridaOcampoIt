export class LoginUserRequest {
    user: string;
    password: string;
    id: string;

    constructor() {
        this.password = "";
        this.user = "";
        this.id = "";
    }
}

export class LoginUserResponse {
    token: string;
    messageEng: string;
    messageEsp: string;
    userData: UserLoginData;

    constructor() {
        this.messageEng = "";
        this.messageEsp = "";
        this.token = "";
        this.userData = new UserLoginData();
    }
}

export class UserLoginData {
    userData: UserDataLogin;
    userPermissions: UserPermissionLogin[];

    constructor() {
        this.userData = new UserDataLogin();
        this.userPermissions = [];
    }
}

export class UserDataLogin {
    idUser: number;
    name: string;
    company: number;
    isType: number;
    origen: number;

    constructor() {
        this.idUser = 0;
        this.name = "";
        this.company = 0;
        this.isType = 0;
        this.origen = 0;
    }
}

export class UserPermissionLogin {
    permissionId: number;
    namePermission: string;

    constructor() {
        this.permissionId = 0;
        this.namePermission = "";
    }
}