
export class Seccion{
    public categoriaId?:number;
    public nombreEN?:string;
    public nombreES?:string;
    public formularioId?:number;
    public PadreId?:number;
    public estatusId?:number;
    public creadoPorId?:number;
    public fechaCreacion?:Date;
    public fechaModificacion?:Date;
    public moodificadoPorId?:number;
        
    constructor(){
        this.categoriaId=0;
        this.formularioId=0;
        this.nombreEN="";
        this.nombreES="";
        this.estatusId=0;
        this.PadreId=0;
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
        this.creadoPorId=0;
        this.moodificadoPorId=0;
       
    }
}