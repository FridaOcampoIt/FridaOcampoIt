export class Pregunta{
    public preguntaId?:number;
    public categoriaId?:number;

    public nombre?:string;
    public descripcionCorta:string;
    public placeholder?:string;
    
    public tipoPregunta?:string;
    public validacion?:string;
    
    public fechaCreacion?:Date;
    public fechaModificacion?:Date;
    
    public estatus?:number;

    public usuarioCreador?:number;
    public usuarioModificador?:number;
    constructor(){
        this.preguntaId=0;
        this.categoriaId=0;
        this.nombre="";
        this.tipoPregunta="";
        this.estatus=0;
        this.placeholder="";
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
        this.usuarioCreador=0;
        this.usuarioModificador=0;
    }

}

export class Uusario {
    public usuarioId?:number;
    public usuarioCreador?:number;
    public usuarioModificador?:number;
    constructor(){
        this.usuarioCreador=0;
        this.usuarioModificador=0;

    }
}