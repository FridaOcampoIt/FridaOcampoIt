using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSTraceIT.Models.Request
{
    #region Busquedas de datos para tabla movimientos
    //Busqueda de movimientos
    public class SearchMovimientoRequest
    {
        public string producto { get; set; }
        public int tipoMovimientoId { get; set; }
        public string fechaCaducidad { get; set; }
        public string fechaIngresoDe { get; set; }
        public string fechaIngresoHasta { get; set; }
        public int usuario { get; set; }
        public string acopiosId { get; set; }
    }
    #endregion

    #region Busquedas de datos para tabla movimientos por acopio
    //Busqueda de movimientos
    public class SearchMovimientoByAcopioIdRequest
    {
        public string producto { get; set; }
        public int tipoMovimientoId { get; set; }
        public string fechaCaducidad { get; set; }
        public string fechaIngresoDe { get; set; }
        public string fechaIngresoHasta { get; set; }
        public int acopioId { get; set; }
    }
    #endregion

    #region Busqueda de datos para los combos de filtros de la tabla movimientos
    //Busqueda para los combos tipo de movimientos y productos
    public class SearchComboMovimientoRequest
    {
        public int tipoMovimiento { get; set; }
    }
    #endregion

    #region Request para buscar datos de un solo movimiento (Separados)
    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDataMovimientoGeneralRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDataEtiquetaRequest
    {
        public string nombreAgru { get; set; }
        public int referenciaInt { get; set; }
        public int referenciaExt { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDataMovimientoGeneralRecepRequest
    {
        public string codigoQR { get; set; }
        public string codigoI { get; set; }
        public string codigoF { get; set; }
        public bool isAgro { get; set; }

        #region Constructor
        public SearchDataMovimientoGeneralRecepRequest()
        {
            this.codigoQR = String.Empty;
            this.codigoI = String.Empty;
            this.codigoF = String.Empty;
            this.isAgro = false;
        }
        #endregion
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDataMovimientoGeneralProdRequest
    {
        public int movimientoId { get; set; }
        public string codigoI { get; set; }
        public string codigoF { get; set; }
        public string codigoTipo { get; set; }
        public string codigoIHEXA { get; set; }
        public string codigoFHEXA { get; set; }
        public int totalProductosQR { get; set; }
        public bool isHexa { get; set; }
        public bool isAgro { get; set; }

        #region Constructor
        public SearchDataMovimientoGeneralProdRequest()
        {
            this.movimientoId = 0;
            this.codigoTipo = String.Empty;
            this.codigoI = String.Empty;
            this.codigoF = String.Empty;
            this.codigoIHEXA = String.Empty;
            this.codigoFHEXA = String.Empty;
            this.totalProductosQR = 0;
            this.isHexa = false;
            this.isAgro = false;
        }
        #endregion
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDataMovimientoGeneralProdRecepRequest
    {
        public string codigoQR { get; set; }
        public string codigoI { get; set; }
        public string codigoF { get; set; }
        public string codigoIHEXA { get; set; }
        public string codigoFHEXA { get; set; }
        public int totalProductosQR { get; set; }
        public bool isHexa { get; set; }
        public int familiaProductoId { get; set; }
        public int productoMovimientoId { get; set; }
        public bool isAgro { get; set; }

        #region Constructor
        public SearchDataMovimientoGeneralProdRecepRequest()
        {
            this.codigoQR = String.Empty;
            this.codigoI = String.Empty;
            this.codigoF = String.Empty;
            this.totalProductosQR = 0;
            this.isHexa = false;
            this.familiaProductoId = 0;
            this.productoMovimientoId = 0;
            this.isAgro = false;
        }
        #endregion
    }

    //Busqueda observaciones de un solo dato de la tabla movimientos
    public class SearchDataMovimientoObservacionesRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDataMovimientoObservacionesRecepRequest
    {
        public string codigoQR { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDataMovimientoNombreRecepRequest
    {
        public string codigoCompleto { get; set; }
    }

    //Busqueda remitente de un solo dato de la tabla movimientos
    public class SearchDataMovimientoRemitenteRequest
    {
        public int movimientoId { get; set; }
        public int company { get; set; }
    }

    //Busqueda remitente de un solo dato de la tabla movimientos
    public class SearchDataMovimientoRemitenteRecepRequest
    {
        public string codigoQR { get; set; }
    }

    //Busqueda destinatario de un solo dato de la tabla movimientos
    public class SearchDataMovimientoDestinatarioRequest
    {
        public int movimientoId { get; set; }
        public string numeroC { get; set; }

        #region Contructor
        public SearchDataMovimientoDestinatarioRequest()
        {
            this.numeroC = String.Empty;
        }
        #endregion
    }

    //Busqueda destinatario de un solo dato de la tabla movimientos
    public class SearchDataMovimientoDestinatarioRecepRequest
    {
        public string codigoQR { get; set; }
    }


    //Busqueda transportista de un solo dato de la tabla movimientos
    public class SearchDataMovimientoTransportistaRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda transportista de un solo dato de la tabla movimientos
    public class SearchDataMovimientoTransportistaRecepRequest
    {
        public string codigoQR { get; set; }
    }


    //Busqueda info legal de un solo dato de la tabla movimientos
    public class SearchDataMovimientoInfoLegalRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda info legal de un solo dato de la tabla movimientos
    public class SearchDataMovimientoInfoLegalRecepRequest
    {
        public string codigoQR { get; set; }
    }

    //Busqueda etiqueta de un solo dato de la tabla movimientos
    public class SearchDataMovimientoInfoEtiquetaRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda info legal de un solo dato de la tabla movimientos
    public class SearchDataMovimientoMermaRequest
    {
        public string codigoId { get; set; }
    }

    //Busqueda de cajas por pallet
    public class SearchDataMovimientoGeneralProdsPalletRequest
    {
        public string codigoI { get; set; }
        public string codigoF { get; set; }
        public int nPallet { get; set; }
        public string tipoC { get; set; }
        public bool isAgro { get; set; }

        #region Constructor
        public SearchDataMovimientoGeneralProdsPalletRequest()
        {
            this.codigoI = String.Empty;
            this.codigoF = String.Empty;
            this.nPallet = 0;
            this.tipoC = String.Empty;
            this.isAgro = false;
        }
        #endregion
    }

    // Busqueda de un movimiento por texto o id
    public class SearchMovimientoCodeRequest
    {
        public string codigoQR { get; set; }
        public bool isAgro { get; set; }
        public bool isRecibe { get; set; }

        #region Constructor
        public SearchMovimientoCodeRequest()
        {
            this.codigoQR = String.Empty;
            this.isAgro = false;
            this.isRecibe = false;
        }
        #endregion
    }
    #endregion

    #region Busqeuda de datos para los comobos que poseen paises y estados
    //Busqueda para los combos paises y estados
    public class SearchComboPaisEstadoRequest
    {
        public int paisId { get; set; }
    }
    #endregion

    #region Busqueda de nombres de los tipo de información legal para mostrarse en radios buttons
    //Busqueda para los radio button tipo info legal
    public class SearchRadioTipoInfoRequest
    {
        public int tipoInfoLegalId { get; set; }
    }
    #endregion

    #region Edición de datos de un solo movimiento (Separados)
    //Edición general de un solo dato de la tabla movimientos
    public class EditDataMovimientoGeneralRequest
    {
        public int movimientoId { get; set; }
        public string nombreAgrupacion { get; set; }
        public int referenciaInterna { get; set; }
        public int referenciaExterna { get; set; }
        public int numeroPallet { get; set; }
        public int numeroCajas { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }
        public int tipoMovimiento { get; set; }
        public string fechaIngreso { get; set; }
        public string fechaCaducidad { get; set; }
    }

    //Edición de observacion de un solo dato de la tabla movimientos
    public class EditDataMovimientoObservacionRequest
    {
        public int movimientoId { get; set; }
        public string observacion { get; set; }
        #region Constructor
        public EditDataMovimientoObservacionRequest()
        {
            this.movimientoId = 0;
            this.observacion = String.Empty;
        }
        #endregion
    }

    //Edición de observacion de un solo dato de la tabla movimientos
    public class EditDataMovimientoNombreRequest
    {
        public string codigoCompleto { get; set; }
        public string nombre { get; set; }
    }

    //Edición de remitente de un solo dato de la tabla movimientos
    public class EditDataMovimientoRemitenteRequest
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
        public string cpR { get; set; }
        public string domicilioR { get; set; }
        public string ranchoR { get; set; }
        public string sectorR { get; set; }
    }

    //Edición de remitente de un solo dato de la tabla movimientos
    public class EditDataMovimientoDestinatarioRequest
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
        public string cpD { get; set; }
        public string domicilioD { get; set; }
        public string numeroC { get; set; }
        public string ranchoD { get; set; }
        public string sectorD { get; set; }
    }

    public class DataMovimientoDestinatarioRequest : EditDataMovimientoDestinatarioRequest
    {
        #region Constructor
        public DataMovimientoDestinatarioRequest()
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
            this.cpD = String.Empty;
            this.domicilioD = String.Empty;
            this.numeroC = String.Empty;
            this.ranchoD = String.Empty;
            this.sectorD = String.Empty;
        }
        #endregion
    }

    //Edición de remitente de un solo dato de la tabla movimientos
    public class EditDataMovimientoTransportistaRequest
    {
        public int movimientoId { get; set; }
        public string transportista { get; set; }
        public string numReferencia { get; set; }
        public string fechaEmbarque { get; set; }
    }

    //Edición de remitente de un solo dato de la tabla movimientos
    public class EditDataMovimientoInfoLegalRequest
    {
        public int movimientoId { get; set; }
        public int tipoInfo { get; set; }
        public string nombreInfo { get; set; }
        public string direccionInfo { get; set; }
        public string contactoInfo { get; set; }
        public string nombreInfoExp { get; set; }
        public string direccionInfoExp { get; set; }
        public string contactoInfoExp { get; set; }
    }

    //Edición de remitente de un solo dato de la tabla movimientos
    public class EditDataMovimientoMermaRequest
    {
        public string codigoId { get; set; }
        public int merma { get; set; }
        public int productoMovimientoId { get; set; }
    }
    #endregion

    #region Busqueda de documentos de la info legal
    //Busqueda de documentos de la info legal
    public class SearchDocsInfoLegalRequest
    {
        public int movimientoId { get; set; }
    }
    #endregion

    #region Busqueda de datos para tabla de existencia estiamda
    //Busqueda para tabla de existencia estimada
    public class SearchExistenciaEstimadaRequest
    {
        public string nombreFamiliaCIU { get; set; }
        public string fechaCaducidad { get; set; }
        public string fechaIngresoDe { get; set; }
        public string fechaIngresoHasta { get; set; }
        public int ordenar { get; set; }
        public int usuario { get; set; }
    }
    #endregion

    #region Registrar nuevos documentos de info legal
    //Guardar documentos de info legal
    public class SaveDocsInfoLegalRequest
    {
        public int movimientoId { get; set; }
        public int docId { get; set; }
        public string doc { get; set; }
        public string fecha { get; set; }
        public int infoLegalId { get; set; }
        public string nombreDoc { get; set; }
    }
    #endregion

    #region Registrar un nuevo movimiento y cambio de su status
    //Guardar movimiento
    public class SaveMovimientoRequest
    {
        public string nombreAgrupacion { get; set; }
        public string fechaIngreso { get; set; }
        public int referenciaInt { get; set; }
        public int referenciaExt { get; set; }
        public int usuario { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
    }
    

    //Editar tipo de movimineto de agrupacion a envio
    public class EditDataAgruAEnvioRequest
    {
        public int movimientoId { get; set; }
        public int usuario { get; set; }
        public string fecha { get; set; }
        public int referenciaInterna { get; set; }
        public int referenciaExterna { get; set; }
        public bool noEsnormal { get; set; }
        public string codigoI { get; set; }
        public string codigoF { get; set; }
        public string codigoTipo { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string codigoCompleto { get; set; }
        public string nombre { get; set; }
        public string caja { get; set; }
        public string codigoIHEXA { get; set; }
        public string codigoFHEXA { get; set; }
        public bool isHexa { get; set; }
        public int tipoRecepcion { get; set; }
        public string cosechero { get; set; }
        public string sector { get; set; }
        public DateTime fechaProduccion { get; set; }
        public int productosRecibidos { get; set; }
        public int nCajasRecibidas { get; set; }
        public int nPalletsRecibidos { get; set; }
        public int nProductosMerma { get; set; }
        public string numeroC { get; set; }
        public int totalProductosQR { get; set; }
        public int isGroup { get; set; }
        public List<productosAgrupacion> productosAgrupacion { get; set; }
        public int productoMovimientoId { get; set; }
        public bool domicilioUp { get; set; }
        public DataMovimientoDestinatarioRequest domicilio { get; set; }
        public bool domicilioUpRem { get; set; }
        public EditDataMovimientoRemitenteRequest domicilioRem { get; set; }
        public bool infoUpTras { get; set; }
        public EditDataMovimientoTransportistaRequest infoTransportista { get; set; }
        public bool infoUpObs { get; set; }
        public EditDataMovimientoObservacionRequest infoObservaciones { get; set; }
        public bool isAgro { get; set; }
        public bool force { get; set; }
        public bool isUpName { get; set; }
        public bool isPallet { get; set; }
        public int embalajeId { get; set; }

        //Acopios, sera la sección que se encargara de "mejorar la trasabilidad"
        public int acopioId { get; set; }
        public int productorId { get; set; }
        #region Contructor
        public EditDataAgruAEnvioRequest()
        {
            this.referenciaInterna = 0;
            this.referenciaExterna = 0;
            this.fechaProduccion = default(DateTime);
            this.cosechero = String.Empty;
            this.sector = String.Empty;
            this.productosRecibidos = 0;
            this.nCajasRecibidas = 0;
            this.nPalletsRecibidos = 0;
            this.nProductosMerma = 0;
            this.numeroC = String.Empty;
            this.totalProductosQR = 0;
            this.isGroup = 0;
            this.productosAgrupacion = new List<productosAgrupacion>();
            this.productoMovimientoId = 0;
            this.domicilioUp = false;
            this.domicilio = new DataMovimientoDestinatarioRequest();
            this.domicilioUpRem = false;
            this.domicilioRem = new EditDataMovimientoRemitenteRequest();
            this.infoUpTras = false;
            this.infoTransportista = new EditDataMovimientoTransportistaRequest();
            this.infoUpObs = false;
            this.infoObservaciones = new EditDataMovimientoObservacionRequest();
            this.isAgro = false;
            this.force = false;
            this.isUpName = false;
            this.isPallet = false;
            this.embalajeId = 0;
        }
        #endregion
    }
    public class productosAgrupacion
    {
        public int id { get; set; }
        public string codigoQR { get; set; }
        public string codigoI { get; set; }
        public string codigoF { get; set; }
        public int cantidad { get; set; }
        public string productoNombre { get; set; }
        public int familiaProductoId { get; set; }
        public int embalajeId { get; set; }
        public productosAgrupacion ()
        {
            this.id = 0;
            this.codigoQR = String.Empty;
            this.codigoI = String.Empty;
            this.codigoF = String.Empty;
            this.cantidad = 0;
            this.productoNombre = String.Empty;
            this.familiaProductoId = 0;
            this.embalajeId = 0;
        }
    }
    #endregion

    #region Crear cuenta de un nuevo usuario dentro del consolidador de mercancias
    //Guardar usuario
    public class SaveUsuarioRequest
    {
        public string nombreComp { get; set; }
        public string razonSocial { get; set; }
        public string correoComp { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string nombrePais { get; set; }

        public string codigoPostal { get; set; }
        public string ciudad { get; set; }
        public string nombreEstado { get; set; }

        public string email1 { get; set; }
        public string pass1 { get; set; }

        public string email2 { get; set; }
        public string pass2 { get; set; }

        public string email3 { get; set; }
        public string pass3 { get; set; }

        public string email4 { get; set; }
        public string pass4 { get; set; }

        public string email5 { get; set; }
        public string pass5 { get; set; }

        public int usuario { get; set; }
    }

    //Guardar usuario Invitado
    public class SaveUsuarioInvitadoRequest
    {
        public string email1 { get; set; }
        public string pass1 { get; set; }
    }

    //Crear evento de eliminar usuario Invitado
    public class DeleteUsuarioInvitadoRequest
    {
        public string email1 { get; set; }
        public int usuario { get; set; }
    }
    #endregion

    #region Busqueda de info para rellenar documento de detalle al seleccionar un movimiento desde envío
    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleProductosRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleProductosIndiRequest
    {
        public string movimientoId { get; set; }
        public string codigoTipo { get; set; }
        public string codigoI { get; set; }
        public string codigoF { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalProdRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalPalletRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalCajasRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalCantidadRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalPesoRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleFechaMinRequest
    {
        public int movimientoId { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleProductosRecepRequest
    {
        public string codigoQR { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleProductosAgruRequest
    {
        public int idFicha { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalProdRecepRequest
    {
        public string codigoQR { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalPalletRecepRequest
    {
        public string codigoQR { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalCajasRecepRequest
    {
        public string codigoQR { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalCantidadRecepRequest
    {
        public string codigoQR { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleTotalPesoRecepRequest
    {
        public string codigoQR { get; set; }
    }

    //Busqueda general de un solo dato de la tabla movimientos
    public class SearchDocDetalleFechaMinRecepRequest
    {
        public string codigoQR { get; set; }
    }
    #endregion

    #region Busqueda de destinatarios de un usuario para autocomplete
    //Busqueda de movimientos
    public class SearchListDestinatariosRequest
    {
        public int usuario { get; set; }
    }

    //Busqueda de movimientos
    public class SearchListDestinatariosRequest2
    {
        public int usuario { get; set; }
    }
    #endregion

    #region Insersion de estados no existentes en los combos
    //Guardar documentos de info legal
    public class SaveEstadosSiNoExisteRequest
    {
        public int pais { get; set; }
        public string nombreEstado { get; set; }
    }
    #endregion

    #region Busqueda de existencia de estado
    //Guardar documentos de info legal
    public class SearchExistenciaEstadoRequest
    {
        public int pais { get; set; }
        public string nombreEstado { get; set; }
    }
    #endregion

    #region Busqueda de embalajes por familia en el envio de un movimiento
    // Busqueda de embalajes por familia
    public class SearchPackagingByfamilyRequest
    {
        public string ids { get; set; }
        public bool isAgro { get; set; }

        #region Constructor
        public SearchPackagingByfamilyRequest()
        {
            this.ids = String.Empty;
            this.isAgro = false;
        }
        #endregion
    }
    #endregion

    #region Busqueda de Consecutivo Etiqueta para imprimir desde Pallet o Caja
    // Busqueda de Consecutivo Etiqueta por familia
    public class SearchConsecutiveByfamilyRequest
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int consecutiveBox { get; set; }

        #region Constructor
        public SearchConsecutiveByfamilyRequest()
        {
            this.id = 0;
            this.productId = 0;
            this.consecutiveBox = 0;
        }
        #endregion
    }
    #endregion
}
