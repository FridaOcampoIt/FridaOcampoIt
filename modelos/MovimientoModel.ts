export class Movimiento{
    public movimientoId:number;
    public companiaId:number;
    public almacenId:number;
    public agrupacionId:number;
    public provedorId:number;
    public distribuidorId:number;
    public tipoMovimientoId:number;
    public origenMoviemientoId:number
    public cantidad:number; 

    constructor (){
    this.movimientoId=0;
    this.companiaId=0;
    this.almacenId=0;
    this.agrupacionId=0;
    this.provedorId=0;
    this.distribuidorId=0;
    this.tipoMovimientoId=0;
    this.origenMoviemientoId=0;
    this.cantidad=0;
    }
}