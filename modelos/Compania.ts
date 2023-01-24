export class Compania {
    public companiaId:number;
    public nombre:string;
    public razonSocial:string;
    public RFC:string;
    public descripcionEN:string;
    public descripcionES:string;
    public estatus:number;
    public licencia:string;
    public inicioContrato:Date;
    public correoContacto:string;
    public telefono:number;
    public sitioURL:string;

    constructor (){
    this.companiaId=0;
    this.nombre="";
    this.razonSocial="";
    this.RFC="";
    this.descripcionEN="";
    this.descripcionES="";
    this.estatus=0;
    this.licencia="";
    this.inicioContrato=new Date();
    this.correoContacto="";
    this.telefono=0;
    this.sitioURL="";
    }
}