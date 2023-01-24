export class Direccion{
    public diccionarioId:number;
    public alias:string;
    public pais:number;
    public estadoId:number;
    public ciudad:string;
    public codigoPostal:string;
    public dirrecion:String;
    public exterio:string;
    public interior:string;
    public latitud:number;
    public logitud:number;
    public estatusId:number;
    public creadorPorId:number;
    public fechaCreacion:Date;
    public modificadoPorId:number;
    public fechaModificacion:Date;

    constructor (){
    this.diccionarioId=0;
    this.alias="";
    this.pais=0;
    this.estadoId=0;
    this.ciudad="";
    this.codigoPostal="";
    this.dirrecion="";
    this.exterio="";
    this.interior="";
    this.latitud=0;
    this.logitud=0;
    this.estatusId=0;
    this.creadorPorId=0;
    this.fechaCreacion=new Date();
    this.modificadoPorId=0;
    this.fechaModificacion=new Date();
    }
}