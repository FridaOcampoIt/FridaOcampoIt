using System;

namespace WSTraceIT.Models.Base.FormSector
{
    /// <summary>
    /// Creacion del modelo de formulario sector
    /// Desarrollador:  Roberto Ortega
    /// </summary>
    public class FormSector
    {
        public int formularioSectorId { get; set; }
        public int sectorId { get; set; }
        public int formularioId { get; set; }

        public FormSector()
        {
            this.formularioSectorId = 0;
            this.sectorId = 0;
            this.formularioId = 0;
        }
    }
    /// <summary>
    /// ´Peticiones (Request) de Formulario sector
    /// Desarrollador:  Roberto Ortega
    /// </summary>
    public class deleteFormSectorRequest
    {
        public int formularioSectorId { get; set; }
    }
    public class saveFormSectorRequest
    {
        public int formularioSectorId { get; set; }
        public int sectorId { get; set; }
        public int formularioId { get; set; }
    }

    public class searchFormSectorRequest
    {
        public int formularioSectorId { get; set; }
    }

    public class updateFormSectorRequest
    {
        public int formularioSectorId { get; set; }
        public int sectorId { get; set; }
        public int formularioId { get; set; }
    }
    /// <summary>
    /// Respuestas (response)  de Formulario sector
    /// Desarrollador:  Roberto Ortega
    /// </summary> 
    public class deleteFormSectorResponse : TraceITResponse
    {
         public deleteFormSectorResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    public class saveFormSectorResponse : TraceITResponse
    {
        public saveFormSectorResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    public class searchFormSectorByIdResponse : TraceITResponse
    {
        public int formularioSectorId { get; set; }
        public int sectorId { get; set; }
        public int formularioId { get; set; }
        public searchFormSectorByIdResponse()
        {
            this.formularioSectorId = 0;
            this.sectorId = 0;
            this.formularioId = 0;
        }
    }

    public class updateFormSectorResponse : TraceITResponse
    {
        public updateFormSectorResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }
}
