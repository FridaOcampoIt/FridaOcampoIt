export class Autonumericos {
    public autoNumericoId:number;
    public nombre:string;
    public prefijo:string;
    public siguiente:number;
    public digitos:number;
    public activo:boolean;

    constructor(){
        this.autoNumericoId=0;
        this.nombre="";
        this.prefijo="";
        this.siguiente=0;
        this.digitos=0;
        this.activo=true;
 
    }
}