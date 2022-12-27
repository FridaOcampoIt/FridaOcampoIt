using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.ModelsSQL
{
    public class ListaRobo : TraceITResponse
    {
        public List<Robo> listaRobo { get; set; }

        public ListaRobo()
        {
            listaRobo = new List<Robo>();
            messageEng = "";
            messageEsp = "";
        }
    }


    public class SaveRoboResponse : TraceITResponse
    {
        public SaveRoboResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    public class SearchListadoRobo
    {
        public string companiaFamilia { get; set; }
        public SearchListadoRobo()
        {
            this.companiaFamilia = String.Empty;
        }
    }
    public class SearchRoboRequest
    {
        public string companiaFamilia { get; set; }
    }


    public class Caja
    {
        public string caja { get; set; }
    }
    public class BusquedaCiu
    {
        public int idReporte { get; set; }
        public string codigoAlerta { get; set; }
        public string tipoReporte { get; set; }
        public bool agrupacionId { get; set; }
    }

    public class Robo
    {
        public int roboId { get; set; }
        public string tituloAlertaES { get; set; }
        public string descripcionES { get; set; }
        public string tituloAlertaEN { get; set; }
        public string descripcionEN { get; set; }
        public bool notificacionMovilCiu { get; set; }
        public bool notificacionTracking { get; set; }
        public int usuarioCreadorId { get; set; }
        public int companiaId { get; set; }
        public int usuarioSolicitoId { get; set; }
        public int tipoAlertaId { get; set; }
        public string nombreArchivo { get; set; }
        public int tipoReporteId { get; set; }
        public int familiaId { get; set; }
        public string codigoAlerta { get; set; }
        public string nombreUsuario { get; set; }
        public DateTime fechaRobo { get; set; }
        public string compania { get; set;  }
        public string familia { get; set; }
        public bool agrupacionId { get; set; }
        public string usuarioSolicitud { get; set; }
        public string tipoReporteNombre { get; set; }
        public string tipoAlertaNombre  { get; set; }
        public string detalle { get; set; }
        public SaveRoboResponse saveRoboResponse { get; set; }
    }

    public class FamiliasProductosSelector
    {
        public string udid { get; set; }
        public bool notificacion { get; set; }
        public int productoId { get; set; }
        public string ciu { get; set; }
        public string descripcion { get; set; }
		public string tituloAlerta { get; set; }
		public string descripcionEN { get; set; }
		public string tituloAlertaEN { get; set; }
        public int tipoAlerta { get; set; }
        public string codigoAlerta { get; set; }
        public string nombreArchivo { get; set; }
        public int companiaId { get; set; }
        public int familiaId { get; set; }
        public int tipoReporte { get; set; }
        public bool NotificarTracking { get; set; }
        public string usuarioSolicitud { get; set; }
    }

    public class CajasList
    {
        public string Cajas { get; set; }

        public CajasList()
        {
            this.Cajas = String.Empty;
        }
    }

    public class ProductosRes
    {
        public int ProductoId { get; set; }
        public string ciu { get; set; }
    }

    public class FamiliasProductosSelectorRes
    {
        public string UDID { get; set; }
        public bool notificacion { get; set; }
        public string descripcion { get; set; }
        public List<ProductosRes> productos { get; set; }
        public FamiliasProductosSelectorRes()
        {
            productos = new List<ProductosRes>();
        }
    }

    public class ListFamiliasProductosRes: TraceITResponse
    {
        public List<FamiliasProductosSelectorRes> ListaUdidsCius { get; set; }
        public bool notificacion { get; set; }
        public string descripcion { get; set; }
		public string tituloAlerta { get; set; }
		public string descripcionEN { get; set; }
		public string tituloAlertaEN { get; set; }
        public int tipoAlerta { get; set; }
        public string codigoAlerta { get; set; }
        public string nombreArchivo { get; set; }
        public int companiaId { get; set; }
        public int familiaId { get; set; }
        public int tipoReporte { get; set; }
        public bool NotificarTracking { get; set; }
        public string usuarioSolicitud { get; set; }
        public List<CajasList> listaCajas { get; set; }

        public ListFamiliasProductosRes()
        {
            ListaUdidsCius = new List<FamiliasProductosSelectorRes>();
            tipoAlerta = 0;
            messageEng = "";
            messageEsp = "";
        }
    }

    public class RobberyRegistryInfo
    {
        public int RobberyId { get; set; }
        public int ProductId { get; set; }
        public string ImageUri { get; set; }
        public string FamilyName { get; set; }
        public bool notifyMe { get; set; }
        public string descripcion { get; set; }
        public string TituloAlerta { get; set; }
        public string descripcionEN { get; set; }
        public string TituloAlertaEN { get; set; }
        public string messageEng { get; set; }
        public string messageEsp { get; set; }
    }

    public class saveDocResponse : TraceITResponse
    {
        public int Accion { get; set; }

        public saveDocResponse() {
            this.Accion = 0;
            this.messageEng = "";
            this.messageEsp = "";
        }
    }

    public class eliminarAlertaResponse : TraceITResponse
    {
        public int accion { get; set; }

        public eliminarAlertaResponse()
        {
            this.accion = 0;
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    public class alertaProducto
    {
        public int alertaId { get; set; }

        public alertaProducto()
        {
            this.alertaId = 0;
        }

    }
}
