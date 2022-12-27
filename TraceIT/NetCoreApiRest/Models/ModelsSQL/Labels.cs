using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.ModelsSQL
{
    public class ListaSolicitudEtiquetas: TraceITResponse
    {
        public List<SolicitudesEtiquetas> listaSolicitudEtiquetas { get; set; }

        public ListaSolicitudEtiquetas()
        {
            this.listaSolicitudEtiquetas = new List<SolicitudesEtiquetas>();
            this.messageEng = "";
            this.messageEsp = "";
        }
    }

    public class SolicitudesEtiquetas
    {
        public int solicitudId { get; set; }
        public string folio { get; set; }
        public DateTime fechaGeneracion { get; set;}
        public int estatusSolicitudId { get; set; }
        public string status { get; set; }
        public int familiaId { get; set; }
        public string familia { get; set; }
        public string modelo { get; set; }
        public int companiaId { get; set; }
        public string companiaNombre { get; set; }
        public string razonSocial { get; set; }
        public int usuarioId { get; set; }
        public int direccionId { get; set; }
        public string udid { get; set; }
        public DateTime? caducidad { get; set; }
        public int cantidad { get; set; }
        public bool byFile { get; set; }
        public string Base64File { get; set; }
    }

    public class ListaCompania : TraceITResponse
    {
        public List<Companias> listaCompanias { get; set; }

        public ListaCompania()
        {
            this.listaCompanias = new List<Companias>();
            this.messageEng = "";
            this.messageEsp = "";
        }
    }

    public class Companias
    {
        public int companiaId { get; set; }
        public string nombre { get; set; }
        public string razonSocial { get; set; }
    }

    public class Bitacora
    {
        public DateTime fechaGeneracion { get; set; }
        public int estatusSolicitudId { get; set; }
        public string status { get; set; }
        public string descripcion { get; set; }
        public int solicitudId { get; set; }
        public int usuarioId { get; set; }
		public string usuario { get; set; }
    }

    public class ListaBitacoras : TraceITResponse
    {
        public List<Bitacora> listaBitacoras{ get; set; }

        public ListaBitacoras()
        {
            this.listaBitacoras = new List<Bitacora>();
            this.messageEng = "";
            this.messageEsp = "";
        }
    }
}
