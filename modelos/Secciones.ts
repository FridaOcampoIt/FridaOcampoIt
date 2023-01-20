
export class Secciones{
    public categoriaId?:number;
    public formularioId?:number;
    public nombre?:string;
    public estatus?:number;
    public descripcionCorta?:string;
    public fechaCreacion?:Date;
    public fechaModificacion?:Date;
    public usuarioCreador?:number;
    public usuarioModificador?:number;
        
    constructor(){
        this.categoriaId=0;
        this.formularioId=0;
        this.nombre="";
        this.estatus=0;
        this.descripcionCorta="";
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
        this.usuarioCreador=0;
        this.usuarioModificador=0;
       
    }
}