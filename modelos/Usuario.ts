export class Usuario {
    public usuarioId:number;
    public nombre:string;
    public contrasena:string;
    public fechaUltimaSession:Date ;
    public creadoPorId:number;
    public modificadoPorId:number;
    public fechaModificacion:Date; 

    constructor(){
        this.usuarioId=0;
        this.nombre="";
        this.contrasena="";
        this.fechaUltimaSession= new Date() ;
        this.creadoPorId=0;
        this.modificadoPorId=0;
        this.fechaModificacion=new Date(); 
    }
}