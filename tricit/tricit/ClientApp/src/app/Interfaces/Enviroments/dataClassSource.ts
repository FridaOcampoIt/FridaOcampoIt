export interface FlatNode {
    expandable: boolean;
    name: string;
    level: number;
    id: number;
    idParent?: number;
    parent: boolean;
}

export class DataTreeEspecification {
    title: string;
    id?: number;
    idPaaarent?: number;
    parent?: boolean;
    children?: DataTreeEspecification[];

    constructor() {
        this.title = "";
        this.id = 0;
        this.idPaaarent = 0;
        this.parent = false;
        this.children = [];
    }
}

export interface FlatNodeProfile {
    expandable: boolean;
    name: string;
    level: number;
    parent: boolean;
    id: number;
    idParent?: number;
    check?: boolean;
}

export class DataTreeProfile {
    name: string;
    id?: number;
    idParent?: number;
    parent?: boolean;
	check?: boolean;
	isForCompany?: boolean;
    children?: DataTreeProfile[];

    constructor() {
        this.name = "";        
        this.id = 0;
        this.idParent = 0;
        this.parent = false;
        this.check = false;
		this.children = [];
		this.isForCompany = false;
    }
}