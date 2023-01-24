export class ConstrolMaestroMultiple {
    public controlId:number;
    public control:string;
    public valorES:string;
    public valorEN:string;
    public archivo:boolean;
    public fechaCreacion:Date;
    public fechaModificacion:Date;
    public usuarioCreadorPortId:number;
    public usuarioModificadorPorId:number;
    constructor(){
        this.controlId=0;
        this.control="";
        this.valorES="";
        this.valorEN="";
        this.archivo=true;
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
        this.usuarioCreadorPortId=0;
        this.usuarioModificadorPorId=0;

    }
}