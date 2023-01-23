export class Pregunta{
    public preguntaId?:number;
    public nombreES?:string;
    public nombreEN?:string;
    public descripcionES:string;
    public descripcionEN:string;
    public placeholder?:string;
    public sectorId?:number;
    public tipoPreguntaId?:number;
    public validaciones?:string;
    public CreadoPorId?:number;
    public fechaCreacion?:Date;
    public ModificadoPorId?:number;
    public fechaModificacion?:Date;
    

    public categoriaId?:number;
    public estatus?:number;
    constructor(){
        this.preguntaId=0;
        this.nombreES="";
        this.nombreEN="";
        this.descripcionES="";
        this.descripcionEN=""
        this.sectorId=0;
        this.tipoPreguntaId=0;
        this.placeholder="";
        this.validaciones="";
        this.fechaCreacion=new Date();
        this.CreadoPorId=0;
        this.fechaModificacion=new Date();
        this.ModificadoPorId=0;
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