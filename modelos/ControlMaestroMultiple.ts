export class ConstrolesMaestrosMultiples {
    public controlId:number;
    public control:string;
    public valorES:string;
    public valorEN:string;
    public activo:boolean;
    public sistema:boolean;
    public fechaCreacion:Date;
    public fechaModificacion:Date;
    public usuarioCreadorPortId:number;
    public usuarioModificadorPorId:number;
    constructor(){
        this.controlId=0;
        this.control="";
        this.valorES="";
        this.valorEN="";
        this.activo=true;
        this.sistema=true;
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
        this.usuarioCreadorPortId=0;
        this.usuarioModificadorPorId=0;
    }
}