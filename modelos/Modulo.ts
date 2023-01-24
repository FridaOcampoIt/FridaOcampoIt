export class Modulo{
    public modoluId:number;
    public moduloPadreId:number;
    public nombreES:string;
    public nombreEN:string;
    public activo:number;
    public icono:string;
    public orden:number;
    public tipo:string;
    public URL:string;

    constructor(){
    this.modoluId=0;
    this.moduloPadreId=0;
    this.nombreES="";
    this.nombreEN="";
    this.activo=0;
    this.icono="";
    this.orden=0;
    this.tipo="";
    this.URL="";
    }
}