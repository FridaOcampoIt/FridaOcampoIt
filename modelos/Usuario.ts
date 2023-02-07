export class Usuarios {
    public usuarioId:number;
    public contrasena:string;
    public nombre:string;
    public primerApellido:string; 
    public segundoApellido:string; 
    public rolId:number;
    public companiaId:number;
    public estatusId:number;
    public tipoUsuarioId:number;
    public tokenRecuperacion:string;
    public fechaUltimaSession:Date ;
    public creadoPorId:number;
    public modificadoPorId:number;
    public fechaCreacion:Date; 
    public fechaModificacion:Date; 
    

    constructor(){
        this.usuarioId=0;
        this.contrasena="";
        this.nombre="";
        this.primerApellido=""; 
        this.segundoApellido=""; 
        this.rolId=0;
        this.companiaId=0;
        this.estatusId=0;
        this.tipoUsuarioId=0;
        this.tokenRecuperacion="";
        this.fechaUltimaSession=new Date() ;
        this.creadoPorId=0;
        this.modificadoPorId=0;
        this.fechaCreacion=new Date(); 
        this.fechaModificacion=new Date(); 
    }
}