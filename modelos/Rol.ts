export class Rol{
    public rolId:number;
    public nombre:string;
    public activo:number;
    public usuarioCreadorId:number;
    public fechaCreacion:Date;
    public modificadoPorId:number;
    public fechaModificacion:Date;
    
    constructor (){
    this.rolId=0;
    this.nombre="";
    this.activo=0;
    this.usuarioCreadorId=0;
    this.fechaCreacion=new Date();
    this.modificadoPorId=0;
    this.fechaModificacion=new Date();
    }
}