export class ConstrolesMaestros {
    public controlMaestroId:number;
    public nombre:string;
    public valor:string;
    public sistema:boolean;
    public fechaCreacion:Date;
    public fechaModificacion:Date;

    constructor(){
        this.controlMaestroId=0;
        this.nombre="";
        this.valor="";
        this.nombre="";
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
 
    }
}