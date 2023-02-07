export class Direcciones{
    public direcionioId:number;
    public socioDeNegocioId:number;
    public companiaId:number;
    public usuarioId:number;
    public pais:number;
    public estatusId:number;
    public ciudadProvidencia:string;
    public dirrecion:String;
    public numeroExterio:string;
    public numeroInterior:string;
    public latitud:number;
    public logitud:number;
    public estadoId:number;
    public creadorPorId:number;
    public modificadoPorId:number;
    public fechaCreacion:Date;
    public fechaModificacion:Date;

    
    constructor (){
        this.direcionioId=0;
        this.socioDeNegocioId=0;
        this.companiaId=0;
        this.usuarioId=0;
        this.pais=0;
        this.estatusId=0;
        this.ciudadProvidencia="";
        this.dirrecion="";
        this.numeroExterio="";
        this.numeroInterior="";
        this.latitud=0;
        this.logitud=0;
        this.estadoId=0;
        this.creadorPorId=0;
        this.modificadoPorId=0;
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
    }
}