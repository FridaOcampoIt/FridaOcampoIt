using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
    #region Backoffice
    /// <summary>
    /// Clase que contendra los datos necesarios para el rastreo
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingDataSQL
    {
        public int id { get; set; }
        public int ciuId { get; set; }
        public string ciu { get; set; }
        public string lot { get; set; }
        public string serialN { get; set; }
        public DateTime date { get; set; }
        public string eventName { get; set; }
        public string eventDetail { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int distributorId { get; set; }
        public string distributorName { get; set; }
        public int companyId { get; set; }
        public string companyName { get; set; }
        public string phase { get; set; }
        public string typeGrouping { get; set; }
        public string typeReception { get; set; }
        public string operador { get; set; }
        public string empExt { get; set; }
        public int tipoEmp { get; set; }
        public string lineaOp { get; set; }
        public int userType { get; set; }
        public string destRazonSocial { get; set; }
        public string destNombre { get; set; }
        public string destPais { get; set; }
        public string destEstado { get; set; }
        public string remiRazonSocial { get; set; }
        public string remiNombre { get; set; }
        public string remiPais { get; set; }
        public string remiEstado { get; set; }
        public string stockUserEstado { get; set; }
        public string stockUserPais { get; set; }
        public string stockUserLatitud { get; set; }
        public string stockUserLongitud { get; set; }
        public string movimientoId { get; set; }
        public string CodigoQR { get; set; }

        #region Constructor
        public TrackingDataSQL()
        {
            this.id = 0;
            this.ciuId = 0;
            this.ciu = String.Empty;
            this.lot = String.Empty;
            this.serialN = String.Empty;
            this.date = default(DateTime);
            this.eventName = String.Empty;
            this.eventDetail = String.Empty;
            this.latitude = String.Empty;
            this.longitude = String.Empty;
            this.distributorId = 0;
            this.distributorName = String.Empty;
            this.companyId = 0;
            this.companyName = String.Empty;
            this.phase = String.Empty;
            this.typeGrouping = String.Empty;
            this.typeReception = String.Empty;
            this.operador = String.Empty;
            this.empExt = String.Empty;
            this.tipoEmp = 0;
            this.lineaOp = String.Empty;
            this.userType = 0;
            this.destRazonSocial = String.Empty;
            this.destNombre = String.Empty;
            this.destPais = String.Empty;
            this.destEstado = String.Empty;
            this.remiRazonSocial = String.Empty;
            this.remiNombre = String.Empty;
            this.remiPais = String.Empty;
            this.remiEstado = String.Empty;
            this.stockUserEstado = String.Empty;
            this.stockUserPais = String.Empty;
            this.stockUserLatitud = String.Empty;
            this.stockUserLongitud = String.Empty;
            this.movimientoId = String.Empty;
            this.CodigoQR = String.Empty;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para la información del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventInfoSQL
    {
        public int movimientoId { get; set; }
        public string nombreAgrupacion { get; set; }
        public int referenciaInterna { get; set; }
        public int referenciaExterna { get; set; }
        public int numeroPallet { get; set; }
        public int numeroCajas { get; set; }
        public int tipoMovimiento { get; set; }
        public string fechaIngreso { get; set; }
        public string codigoQR { get; set; }

        #region Constructor
        public TrackingEventInfoSQL()
        {
            this.movimientoId = 0;
            this.nombreAgrupacion = String.Empty;
            this.referenciaInterna = 0;
            this.referenciaExterna = 0;
            this.numeroPallet = 0;
            this.numeroCajas = 0;
            this.tipoMovimiento = 0;
            this.fechaIngreso = String.Empty;
            this.codigoQR = String.Empty;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para la información del remitente del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventSenderInfoSQL
    {
        public int movimientoId { get; set; }
        public string nombreRemitente { get; set; }
        public string apellidoRemitente { get; set; }
        public string nombreCompaniaR { get; set; }
        public string rzCompaniaR { get; set; }
        public string telefonoR { get; set; }
        public int paisR { get; set; }
        public int estadoR { get; set; }
        public string ciudadR { get; set; }
        public int cpR { get; set; }
        public string domicilioR { get; set; }

        #region Constructor
        public TrackingEventSenderInfoSQL()
        {
            this.movimientoId = 0;
            this.nombreRemitente = String.Empty;
            this.apellidoRemitente = String.Empty;
            this.nombreCompaniaR = String.Empty;
            this.rzCompaniaR = String.Empty;
            this.telefonoR = String.Empty;
            this.paisR = 0;
            this.estadoR = 0;
            this.ciudadR = String.Empty;
            this.cpR = 0;
            this.domicilioR = String.Empty;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para la información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventLegalInfoSQL
    {
        public int movimientoId { get; set; }
        public int tipoInfo { get; set; }
        public int infoLegalId { get; set; }
        public string nombreInfo { get; set; }
        public string direccionInfo { get; set; }
        public string contactoInfo { get; set; }

        #region Constructor
        public TrackingEventLegalInfoSQL()
        {
            this.movimientoId = 0;
            this.tipoInfo = 0;
            this.infoLegalId = 0;
            this.nombreInfo = String.Empty;
            this.direccionInfo = String.Empty;
            this.contactoInfo = String.Empty;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para la información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventRecipientInfoSQL
    {
        public int movimientoId { get; set; }
        public string nombreDestinatario { get; set; }
        public string apellidoDestinatario { get; set; }
        public string nombreCompaniaD { get; set; }
        public string rzCompaniaD { get; set; }
        public string telefonoD { get; set; }
        public int paisD { get; set; }
        public int estadoD { get; set; }
        public string ciudadD { get; set; }
        public int cpD { get; set; }
        public string domicilioD { get; set; }

        #region Constructor
        public TrackingEventRecipientInfoSQL()
        {
            this.movimientoId = 0;
            this.nombreDestinatario = String.Empty;
            this.apellidoDestinatario = String.Empty;
            this.nombreCompaniaD = String.Empty;
            this.rzCompaniaD = String.Empty;
            this.telefonoD = String.Empty;
            this.paisD = 0;
            this.estadoD = 0;
            this.ciudadD = String.Empty;
            this.cpD = 0;
            this.domicilioD = String.Empty;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para la información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventProductsInfoSQL
    {
        public int movimientoId { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }

        #region Constructor
        public TrackingEventProductsInfoSQL()
        {
            this.movimientoId = 0;
            this.producto = String.Empty;
            this.cantidad = 0;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para los documentos de información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventLegalDocsInfoSQL
    {
        public int docId { get; set; }
        public string fechaDoc { get; set; }
        public string rutaDoc { get; set; }
        public string nombredoc { get; set; }
        public int movimientoId { get; set; }

        #region Constructor
        public TrackingEventLegalDocsInfoSQL()
        {
            this.movimientoId = 0;
            this.fechaDoc = String.Empty;
            this.rutaDoc = String.Empty;
            this.nombredoc = String.Empty;
            this.movimientoId = 0;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para los documentos de información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventTotalProdInfoSQL
    {
        public int totalProducto { get; set; }

        #region Constructor
        public TrackingEventTotalProdInfoSQL()
        {
            this.totalProducto = 0;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para los documentos de información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventTotalPalletInfoSQL
    {
        public int totalPallet { get; set; }

        #region Constructor
        public TrackingEventTotalPalletInfoSQL()
        {
            this.totalPallet = 0;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para los documentos de información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventTotalBoxInfoSQL
    {
        public int totalCajas { get; set; }

        #region Constructor
        public TrackingEventTotalBoxInfoSQL()
        {
            this.totalCajas = 0;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para los documentos de información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventTotalQuantityInfoSQL
    {
        public int totalCantidad { get; set; }

        #region Constructor
        public TrackingEventTotalQuantityInfoSQL()
        {
            this.totalCantidad = 0;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para los documentos de información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventTotalWeightInfoSQL
    {
        public int totalPeso { get; set; }

        #region Constructor
        public TrackingEventTotalWeightInfoSQL()
        {
            this.totalPeso = 0;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para los documentos de información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventDateMinInfoSQL
    {
        public string fechaMin { get; set; }

        #region Constructor
        public TrackingEventDateMinInfoSQL()
        {
            this.fechaMin = String.Empty;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para los documentos de información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventProuctDetailInfoSQL
    {
        public string producto { get; set; }
        public int numPallet { get; set; }
        public int numCajas { get; set; }
        public int cantidad { get; set; }
        public int pesoBruto { get; set; }
        public string dimensiones { get; set; }
        public string fechaCaducidad { get; set; }
        public string lote { get; set; }
        public string ciu { get; set; }
        public int numSerie { get; set; }

        #region Constructor
        public TrackingEventProuctDetailInfoSQL()
        {
            this.producto = String.Empty;
            this.numPallet = 0;
            this.numCajas = 0;
            this.cantidad = 0;
            this.pesoBruto = 0;
            this.dimensiones = String.Empty;
            this.fechaCaducidad = String.Empty;
            this.lote = String.Empty;
            this.ciu = String.Empty;
            this.numSerie = 0;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contendra los datos necesarios para los documentos de información legal del evento (movimiento)
    /// Desarrollador: Iván Gutiérrez
    /// </summary>
    public class TrackingEventCarriersInfoSQL
    {
        public int movimientoId { get; set; }
        public string transportista { get; set; }
        public string numReferencia { get; set; }
        public string fechaEmbarque { get; set; }

        #region Constructor
        public TrackingEventCarriersInfoSQL()
        {
            this.movimientoId = 0;
            this.transportista = String.Empty;
            this.numReferencia = String.Empty;
            this.fechaEmbarque = String.Empty;
        }
        #endregion
    }

    public class TrackingDocumentosFamilia
    {
        public string Titulo { get; set; }
        public string Url { get; set; }

        public TrackingDocumentosFamilia()
        {
            this.Titulo = String.Empty;
            this.Url = String.Empty;
        }
    }

    public class InfoFamilia
    {
        public string Nombre { get; set; }
        public string Modelo { get; set; }
        public string Compania { get; set; }

        public InfoFamilia()
        {
            this.Nombre = String.Empty;
            this.Modelo = String.Empty;
            this.Compania = String.Empty;
        }

    }

    public class TrackingStockDocs
    {
        public int movimientoId { get; set; }
        public int infoLegalId { get; set; }

        public TrackingStockDocs()
        {
            this.movimientoId = 0;
            this.infoLegalId = 0;
        }
    }

    public class documentoStock
    {
        public string nombreDoc { get; set; }
        public string Documento { get; set; }
        public int TipoArchivo { get; set; }

        public documentoStock()
        {
            this.nombreDoc = String.Empty;
            this.Documento = String.Empty;
            this.TipoArchivo = 0;
        }
    }

    public class alertas
    {
        public string TituloAlerta { get; set; }
        public string TituloAlertaEn { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionEn { get; set; }
        public string NombreArchivo { get; set; }
        public int TipoAlerta { get; set; }
        public int TipoReporte { get; set; }
        public int idReporte { get; set; }
        public alertas()
        {
            this.TituloAlerta = String.Empty;
            this.TituloAlertaEn = String.Empty;
            this.Descripcion = String.Empty;
            this.DescripcionEn = String.Empty;
            this.NombreArchivo = String.Empty;
            this.TipoAlerta = 0;
            this.TipoReporte = 0;

        }

    }

    public class ListaAlertas
    {
        public List<alertas> listAlertas { get; set; }

        public ListaAlertas()
        {
            this.listAlertas = new List<alertas>();
        }
    }

    #endregion
}
