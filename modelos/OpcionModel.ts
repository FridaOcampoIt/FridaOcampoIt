export class Opcion{
    public opcionId?:number;
    public nombreEN?:string;
    public nombreES?:string;
    public valor?:number;
    public preguntaId?:number;
    public estatusId?:number;
    public creadoPorId?:number;
    public modificadoPorId?:number;
    public fechaCreacion?:Date;
    public fechaModificacion?:Date;
    
    constructor(){
        this.opcionId=0;
        this.nombreEN="";
        this.nombreES="";
        this.valor=0;
        this.estatusId=0;
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
        this.creadoPorId=0;
        this.modificadoPorId=0;
    }
}