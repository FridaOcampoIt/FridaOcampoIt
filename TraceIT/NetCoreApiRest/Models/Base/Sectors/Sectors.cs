using Microsoft.CodeAnalysis.Classification;
using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Companies;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Models.Base.Sectors

/// <summary>
/// Creacion del modelo de sectores
/// Desarrollador:  Roberto Ortega
/// </summary>
{
    public class Sectors
    {
        public int sectorId { get; set; }
        public string nombre { get; set; }
        public string descripcionCorta { get; set; }
        public int usuarioCreadorId { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusId { get; set; }

        public Sectors()
        {
            this.sectorId = 0;
            this.nombre = String.Empty;
            this.descripcionCorta = String.Empty;
            this.usuarioCreadorId = 0;
            this.usuarioModificadorId = 0;
            this.fechaCreacion = DateTime.Now;
            this.fechaModificacion = DateTime.Now;
            this.estatusId = 0;

        }
    }
    /// <summary>
    /// ´Peticiones (Request) de sectores
    /// Desarrollador:  Roberto Ortega
    /// </summary>
    public class deleteSectorsRequest
    {
        public int sectorId { get; set; }
    }
    public class saveSectorsRequest
    {
        public int sectorId { get; set; }
        public string nombre { get; set; }
        public string descripcionCorta { get; set; }
        public int usuarioCreadorId { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusId { get; set; }
    }

    public class searchSectorsRequest
    {
        public int sectorId { get; set; }
        public string nombre { get; set; }
    }

    public class updateSectorsRequest
    {
        public int sectorId { get; set; }
        public string nombre { get; set; }
        public string descripcionCorta { get; set; }
        public int usuarioModificadorId { get; set; }
        public int estatusId { get; set; }
    }
    /// <summary>
    /// Respuestas (response)  de sectores
    /// Desarrollador:  Roberto Ortega
    /// </summary>
    public class deleteSectorsResponse : TraceITResponse
    {
        public deleteSectorsResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }
    public class saveSectorsResponse : TraceITResponse
    {
        public saveSectorsResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    public class searchSectorsByIdResponse : TraceITResponse
    {
        public int sectorId { get; set; }
        public string nombre { get; set; }
        public string descripcionCorta { get; set; }
        public int usuarioCreadorId { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusId { get; set; }
        public searchSectorsByIdResponse()
        {
            this.sectorId = 0;
            this.nombre = String.Empty;
            this.descripcionCorta = String.Empty;
            this.usuarioCreadorId = 0;
            this.usuarioModificadorId = 0;
            this.fechaCreacion = DateTime.Now;
            this.fechaModificacion = DateTime.Now;
            this.estatusId = 0;
        }
    }

    public class SearchSectorResponse : TraceITResponse
    {
        public List<Sectors> sectorsDataList { get; set; }

        public SearchSectorResponse()
        {
            this.sectorsDataList = new List<Sectors>();
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }
    

    public class updateSectorsResponse : TraceITResponse
    {
        public updateSectorsResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

}
       

    

