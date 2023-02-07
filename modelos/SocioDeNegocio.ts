export class SociosDeNegocios{
    public socioDeNegocioId:number;
    public nombre:string;
    public razonSocial:string;
    public RFC:string;
    public creadoporId:number;
    public fechaCreacion:Date;
    public modificadoporId:number;
    public fechaModificacion:Date;
    public estatusId:number;
    constructor(){
        this.socioDeNegocioId=0;
        this.nombre="";
        this.razonSocial="";
        this.RFC="";
        this.creadoporId=0;
        this.fechaCreacion=new Date();
        this.modificadoporId=0;
        this.fechaModificacion=new Date();
        this.estatusId=0;
    }
}