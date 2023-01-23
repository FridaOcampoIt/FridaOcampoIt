export class Operacion{
    public operacionId:number; 
    public companiaId:number;
    public operadorId:number;
    public lineaId:number;
    public agrupacionId:number;
    public embalajeId:number;
    public tipoOperacionId:number;
    public codigoInicial:string;
    public codigoFinal:string;
    public cantidad:number;

    constructor (){
    this.operacionId=0; 
    this.companiaId=0;
    this.operadorId=0;
    this.lineaId=0;
    this.agrupacionId=0;
    this.embalajeId=0;
    this.tipoOperacionId=0;
    this.codigoInicial="";
    this.codigoFinal="";
    this.cantidad=0;
    }
}