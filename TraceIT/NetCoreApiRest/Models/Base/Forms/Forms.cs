using System;
using System.Collections.Generic;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Models.Base.Forms
{
    /// <summary>
    /// Creacion del modelo de formularios
    /// Desarrollador:  Roberto Ortega
    /// </summary>
    public class Forms
    {
        public int formularioId { get; set; }
        public string nombre { get; set; }
        public string descripcionCorta { get; set; }
        public int usuarioCreadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusFormularioId { get; set; }
        public int sectorId { get; set; }

        public Forms()
        {
            this.formularioId = 0;
            this.nombre = String.Empty;
            this.descripcionCorta = String.Empty;
            this.usuarioCreadorId = 0;
            this.fechaCreacion = DateTime.Now;
            this.usuarioModificadorId = 0;
            this.fechaModificacion = DateTime.Now;
            this.estatusFormularioId = 0;
            this.sectorId = 0;

        }
    }
    /// <summary>
    /// ´Peticiones (Request) de Formularios
    /// Desarrollador:  Roberto Ortega
    /// </summary>
    public class deleteFormsRequest
    {
        public int formularioId { get; set; }
    }
    public class saveFormsRequest
    {
        public int formularioId { get; set; }
        public string nombre { get; set; }
        public string descripcionCorta { get; set; }
        public int usuarioCreadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusFormularioId { get; set; }
        public int sectorId { get; set; }
    }

    public class searchFormsRequest
    {
        public int formularioId { get; set; }
        public int sectorId { get; set; }
    }

    public class updateFormsRequest
    {
        public int formularioId { get; set; }
        public string nombre { get; set; }
        public string descripcionCorta { get; set; }
        public int usuarioModificadorId { get; set; }
        public int estatusFormularioId { get; set; }
    }
    /// <summary>
    /// Respuestas (response)  de Formularios
    /// Desarrollador:  Roberto Ortega
    /// </summary>
    public class deleteFormsResponse : TraceITResponse
    {
        public deleteFormsResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }
    public class saveFormsResponse : TraceITResponse
    {
        public saveFormsResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    public class searchFormsByIdResponse : TraceITResponse
    {
        public int formularioId { get; set; }
        public string nombre { get; set; }
        public string descripcionCorta { get; set; }
        public int usuarioCreadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusFormularioId { get; set; }
        public searchFormsByIdResponse()
        {
            this.formularioId = 0;
            this.nombre = String.Empty;
            this.descripcionCorta = String.Empty;
            this.usuarioCreadorId = 0;
            this.fechaCreacion = DateTime.Now;
            this.usuarioModificadorId = 0;
            this.fechaModificacion = DateTime.Now;
            this.estatusFormularioId = 0;
        }
    }

    public class searchListFormResponse : TraceITResponse
    {
        public List<Forms> formsDataList { get; set; }
        public searchListFormResponse()
        {
            this.formsDataList = new List<Forms>();
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    public class updateFormsResponse : TraceITResponse
    {
        public updateFormsResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }
}
