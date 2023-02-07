export class SociosDeNegociosCompania{
    public socioDeNegocioCompaniaid:number;
    public companiaId:number;
    public estatusId:number;
    public nombre:string;
    public creadoporid:number;
    public fechaCreacion:Date;
    public modificadoporId:number;
    public fechaModificacion:Date; 

    constructor (){
        this.socioDeNegocioCompaniaid=0;
        this.companiaId=0;
        this.estatusId=0;
        this.nombre="";
        this.creadoporid=0;
        this.fechaCreacion=new Date();
        this.modificadoporId=0;
        this.fechaModificacion=new Date(); 
    }
}