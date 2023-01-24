
export class Formulario{
    public formularioId?:number;
    public nombre?:string;
    public estatusId?:number;
    public creadoPorId?:number;
    public fechaCreacion?:Date;
    public moodificadoPorId?:number;
    public fechaModificacion?:Date;
        
    constructor(){
        this.formularioId=0;
        this.nombre="";
        this.estatusId=0;
        this.fechaCreacion=new Date();
        this.fechaModificacion=new Date();
        this.creadoPorId=0;
        this.moodificadoPorId=0;
       
    }
}