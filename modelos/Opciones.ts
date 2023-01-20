export class Opciones{
    public opcionId?:number;
    public nombre?:string;
    public valor?:number;
    public estatus?:number;
    public fechaCreacion?:Date;
    public fechaModificacion?:Date;
    public usuarioCreador?:number;
    public usuarioModificador?:number;

    constructor(){
        this.opcionId=0;
        this.nombre="";
        this.valor=0;
        this.estatus=0;
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
        this.usuarioCreador=0;
        this.usuarioModificador=0;
    }
}