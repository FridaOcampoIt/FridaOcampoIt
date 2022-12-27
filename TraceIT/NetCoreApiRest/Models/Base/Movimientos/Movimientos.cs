using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSTraceIT.Models.Base.Movimientos
{
    public class Movimientos
    {
    }

    #region Modelo para tabla de movimientos
    /// <summary>
    /// Modelo para traer los datos de los movimientos en la consulta
    /// Desarrollador: Javier Ramirez
    /// </summary>
    public class MovimientosData
	{
		public int movimientoId { get; set; }
		public string nombreAgrupacion { get; set; }
		//public int numeroPallet { get; set; }
		public int numeroCajas { get; set; }
		public int cantidad { get; set; }
		public int cantidadRecibida { get; set; }
		public string nombreRemitente { get; set; }
		public string apellidoRemitente { get; set; }
		public string nombreDestinatario { get; set; }
		public string apellidoDestinatario { get; set; }
		public string fechaIngreso { get; set; }
		//Se usan para la búsqueda
		public string producto { get; set; }
		public string tipoMovimiento { get; set; }
		public string fechaCaducidad { get; set; }
		public string fechaIngresoDe { get; set; }
		public string fechaIngresoHasta { get; set; }
		public int cantIndMov { get; set; }
		public string lote { get; set; }
		public string referenciaInterna { get; set; }
		public string referenciaExterna { get; set; }
		public string codigoQR { get; set; }
		public string nombreAcopio { get; set; }


		public MovimientosData()
		{
			//Variables SearchMovimientos
			this.movimientoId = 0;
			this.nombreAgrupacion = String.Empty;
			// this.numeroPallet = 0;
			this.numeroCajas = 0;
			this.cantidad = 0;
			this.cantidadRecibida = 0;
			this.nombreRemitente = String.Empty;
			this.apellidoRemitente = String.Empty;
			this.nombreDestinatario = String.Empty;
			this.apellidoDestinatario = String.Empty;
			this.fechaIngreso = String.Empty;
			//Se usan para la búsqueda
			this.producto = String.Empty;
			this.tipoMovimiento = String.Empty;
			this.fechaCaducidad = String.Empty;
			this.fechaIngresoDe = String.Empty;
			this.fechaIngresoHasta = String.Empty;
			this.cantIndMov = 0;
			this.lote = String.Empty;
			this.referenciaInterna = String.Empty;
			this.referenciaExterna = String.Empty;
			this.codigoQR = String.Empty;
			this.nombreAcopio = String.Empty;
		}
	}
    #endregion

    #region Modelos para datos generales al seleccionar un solo movimiento
    /// <summary>
    /// Modelo para traer los datos generales de un movimiento
    /// Desarrollador: Javier Ramirez OK
    /// </summary>
    public class MovimientosDataGeneral
	{
		public int movimientoId { get; set; }
		public string nombreAgrupacion { get; set; }
		public int referenciaInterna { get; set; }
		public int referenciaExterna { get; set; }
		public int numeroPallet { get; set; }
		public int numeroCajas { get; set; }
		public string producto { get; set; }
		public int cantidad { get; set; }
		public string tipoMovimiento { get; set; }
		public string fechaIngreso { get; set; }
		public string fechaCaducidad { get; set; }
		public string codigoQR { get; set; }
		public string codigoI { get; set; }
		public string codigoF { get; set; }
		public int tipoRecepcion { get; set; }
		public string cosechero { get; set; }
		public string sector { get; set; }
		public DateTime fechaProduccion { get; set; }
		public int productosRecibidos { get; set; }
		public bool isAgro { get; set; }

		public MovimientosDataGeneral()
		{
			this.movimientoId = 0;
			this.nombreAgrupacion = String.Empty;
			this.referenciaInterna = 0;
			this.referenciaExterna = 0;
			this.numeroPallet = 0;
			this.numeroCajas = 0;
			this.producto = String.Empty;
			this.cantidad = 0;
			this.tipoMovimiento = String.Empty;
			this.fechaIngreso = String.Empty;
			this.fechaCaducidad = String.Empty;
			this.codigoQR = String.Empty;
			this.codigoF = String.Empty;
			this.codigoI = String.Empty;
			this.tipoRecepcion = 0;
			this.cosechero = String.Empty;
			this.sector = String.Empty;
			this.fechaProduccion = default(DateTime);
			this.productosRecibidos = 0;
			this.isAgro = false;
		}
	}

	/// <summary>
	/// Modelo para traer los datos generales de un movimiento
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class MovimientosDataObservacion
	{
		public int movimientoId { get; set; }
		public string observacion { get; set; }
		public string dimensionesCaja { get; set; }
		public string pesoCaja { get; set; }

		public MovimientosDataObservacion()
		{
			this.movimientoId = 0;
			this.observacion = String.Empty;
			this.dimensionesCaja = String.Empty;
			this.pesoCaja = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para traer los datos generales de un movimiento
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class MovimientosDataNombre
	{
		public int movimientoId { get; set; }
		public string nombreAgrupacion { get; set; }

		public MovimientosDataNombre()
		{
			this.movimientoId = 0;
			this.nombreAgrupacion = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para traer los datos generales de un movimiento
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class MovimientosDataGeneralProd
	{
		public int movimientoId { get; set; }
		public string producto { get; set; }
		public int cantidad { get; set; }

		public MovimientosDataGeneralProd()
		{
			this.movimientoId = 0;
			this.producto = String.Empty;
			this.cantidad = 0;
		}
	}

	/// <summary>
	/// Modelo para traer los datos de remitente de un movimiento
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class MovimientosDataRemitente
	{
		public int movimientoId { get; set; }
		public string nombreRemitente { get; set; }
		public string apellidoRemitente { get; set; }
		public string nombreCompaniaR { get; set; }
		public string rzCompaniaR { get; set; }
		public string telefonoR { get; set; }
		public int paisR { get; set; }
		public int estadoR { get; set; }
		public string paisNR { get; set; }
		public string estadoNR { get; set; }
		public string ciudadR { get; set; }
		public string cpR { get; set; }
		public string domicilioR { get; set; }
		public string ranchoR { get; set; }
		public string sectorR { get; set; }
		public string nombreCompaniaHeader { get; set; }
		public string atendioUsuario { get; set; }
		public MovimientosDataRemitente()
		{
			this.movimientoId = 0;
			this.nombreRemitente = String.Empty;
			this.apellidoRemitente = String.Empty;
			this.nombreCompaniaR = String.Empty;
			this.rzCompaniaR = String.Empty;
			this.telefonoR = String.Empty;
			this.paisR = 0;
			this.estadoR = 0;
			this.paisNR = String.Empty;
			this.estadoNR = String.Empty;
			this.ciudadR = String.Empty;
			this.cpR = String.Empty;
			this.domicilioR = String.Empty;
			this.ranchoR = String.Empty;
			this.sectorR = String.Empty;
			this.nombreCompaniaHeader = String.Empty;
			this.atendioUsuario = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para traer los datos de destinatario de un movimiento
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class MovimientosDataDestinatario
	{
		public int movimientoId { get; set; }
		public int idDestinatario { get; set; }
		public string nombreDestinatario { get; set; }
		public string apellidoDestinatario { get; set; }
		public string nombreCompaniaD { get; set; }
		public string rzCompaniaD { get; set; }
		public string telefonoD { get; set; }
		public int paisD { get; set; }
		public int estadoD { get; set; }
		public string paisND { get; set; }
		public string estadoND { get; set; }
		public string ciudadD { get; set; }
		public string cpD { get; set; }
		public string domicilioD { get; set; }
		public string numeroC { get; set; }
		public string ranchoD { get; set; }
		public string sectorD { get; set; }

		public MovimientosDataDestinatario()
		{
			this.movimientoId = 0;
			this.idDestinatario = 0;
			this.nombreDestinatario = String.Empty;
			this.apellidoDestinatario = String.Empty;
			this.nombreCompaniaD = String.Empty;
			this.rzCompaniaD = String.Empty;
			this.telefonoD = String.Empty;
			this.paisD = 0;
			this.estadoD = 0;
			this.paisND = String.Empty;
			this.estadoND = String.Empty;
			this.ciudadD = String.Empty;
			this.cpD = String.Empty;
			this.domicilioD = String.Empty;
			this.numeroC = String.Empty;
			this.ranchoD = String.Empty;
			this.sectorD = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para traer los datos de transportista de un movimiento
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class MovimientosDataTransportista
	{
		public int movimientoId { get; set; }
		public string transportista { get; set; }
		public string numReferencia { get; set; }
		public string fechaEmbarque { get; set; }

		public MovimientosDataTransportista()
		{
			this.movimientoId = 0;
			this.transportista = String.Empty;
			this.numReferencia = String.Empty;
			this.fechaEmbarque = String.Empty;

		}
	}

	/// <summary>
	/// Modelo para traer los datos de info legal de un movimiento
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class MovimientosDataInfoLegal
	{
		public int movimientoId { get; set; }
		public int infoLegalId { get; set; }
		public int tipoInfo { get; set; }
		public string nombreInfo { get; set; }
		public string direccionInfo { get; set; }
		public string contactoInfo { get; set; }
		public string nombreInfoExp { get; set; }
		public string direccionInfoExp { get; set; }
		public string contactoInfoExp { get; set; }

		public MovimientosDataInfoLegal()
		{
			this.movimientoId = 0;
			this.infoLegalId = 0;
			this.tipoInfo = 0;
			this.nombreInfo = String.Empty;
			this.direccionInfo = String.Empty;
			this.contactoInfo = String.Empty;
			this.nombreInfoExp = String.Empty;
			this.direccionInfoExp = String.Empty;
			this.contactoInfoExp = String.Empty;

		}
	}

	/// <summary>
	/// Modelo para traer los datos de los documentos en la consulta
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class DocsInfoLegalData
	{
		public int docId { get; set; }
		public string fechaDoc { get; set; }
		public string rutaDoc { get; set; }
		public string Nombredoc { get; set; }
		public int tipoArchivo { get; set; }
		//Se usan para la búsqueda
		public int movimientoId { get; set; }

		public DocsInfoLegalData()
		{
			//Variables SearchMovimientos
			this.docId = 0;
			this.fechaDoc = "";
			this.rutaDoc = "";
			this.Nombredoc = "";
			this.tipoArchivo = 0;
			//Se usan para la búsqueda
			this.movimientoId = 0;
		}
	}

	/// <summary>
	/// Modelo para traer los datos de la etiqueta de un movimiento
	/// Desarrollador: Ivan Gutierrez
	/// </summary>
	public class MovimientosDataInfoLabel
	{
		public string nombreComp { get; set; }
		public string operador { get; set; }
		public string linea { get; set; }
		public string fecha { get; set; }
		public string codigoQR { get; set; }
		public string codigoI { get; set; }
		public string codigoF { get; set; }
		public int cantidad { get; set; }
		public string nombreProducto { get; set; }
		public string cajaId { get; set; }

		public MovimientosDataInfoLabel()
		{
			this.nombreComp = String.Empty;
			this.operador = String.Empty;
			this.linea = String.Empty;
			this.fecha = String.Empty;
			this.codigoQR = String.Empty;
			this.codigoI = String.Empty;
			this.codigoF = String.Empty;
			this.cantidad = 0;
			this.nombreProducto = String.Empty;
			this.cajaId = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para obtener las cajas de un pallet
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class CajasPalletLst
	{
		public string codigoQR { get; set; }
		public string codigoI { get; set; }
		public string codigoF { get; set; }
		public int cantidad { get; set; }
		public string productoNombre { get; set; }
		public int familiaProductoId { get; set; }
		public int exists { get; set; }

		#region Constructor
		public CajasPalletLst()
		{
			this.codigoQR = String.Empty;
			this.codigoI = String.Empty;
			this.codigoF = String.Empty;
			this.cantidad = 0;
			this.productoNombre = String.Empty;
			this.familiaProductoId = 0;
			this.exists = 0;
		}
		#endregion
	}
	#endregion

	#region Modelo para tabla de existencia estimada
	/// <summary>
	/// Modelo para traer los datos de la existencia estimada
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class ExistenciaEstimadaData
	{
		public int movimientoId { get; set; }
		public string nombreAgrupacion { get; set; }
		public string nombreCliente { get; set; }
		public string numCliente { get; set; }
		public string producto { get; set; }
		public string compania { get; set; }
		public int cantidadEnvio { get; set; }
		public int cantidadRecibida { get; set; }
		public string fechaIngreso { get; set; }
		public string fechaCaducidad { get; set; }
		public string lote { get; set; }
		public int envioId { get; set; }
		public int TipoMovimiento { get; set; }

		public ExistenciaEstimadaData()
		{
			//Variables SearchMovimientos
			this.movimientoId = 0;
			this.nombreAgrupacion = "";
			this.nombreCliente = "";
			this.numCliente = "";
			this.producto = "";
			this.compania = "";
			this.cantidadEnvio = 0;
			this.cantidadRecibida = 0;
			this.fechaIngreso = "";
			this.fechaCaducidad = "";
			this.lote = "";
			this.envioId = 0;
			this.TipoMovimiento = 0;
		}
	}
    #endregion

    #region Modelos de datos para el documento de detalles al seleccionar un movimiento
    /// <summary>
    /// Modelo para traer los datos generales de la tabla productos del documento de detalle
    /// Desarrollador: Javier Ramirez
    /// </summary>
    public class DocDetalleProductoData
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
		public  string numSerie  { get; set; }
		public string codigoQR { get; set; }
		public string gtin { get; set; }
		public string tipoEmpaque { get; set; }
		public string codigoPallet { get; set; }
		public string codigoCaja { get; set; }

		public DocDetalleProductoData()
		{
			//Variables SearchMovimientos
			this.producto = String.Empty;
			this.numPallet = 0;
			this.numCajas = 0;
			this.cantidad = 0;
			this.pesoBruto = 0;
			this.dimensiones = String.Empty;
			this.fechaCaducidad = String.Empty;
			this.lote = String.Empty;
			this.ciu = String.Empty;
			this.numSerie = String.Empty;
			this.codigoQR = String.Empty;
			this.gtin = String.Empty;
			this.tipoEmpaque = String.Empty;
			this.codigoPallet = String.Empty;
			this.codigoCaja = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para traer el total de productos de la tabla productos del documento de detalle
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class DocDetalleTotalProdData
	{
		public int totalProducto { get; set; }

		public DocDetalleTotalProdData()
		{
			//Variables SearchMovimientos
			this.totalProducto = 0;
		}
	}

	/// <summary>
	/// Modelo para traer el total de pallets de los productos de la tabla productos del documento de detalle
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class DocDetalleTotalPalletData
	{
		public int totalPallet { get; set; }

		public DocDetalleTotalPalletData()
		{
			//Variables SearchMovimientos
			this.totalPallet = 0;
		}
	}

	/// <summary>
	/// Modelo para traer el total de cajas de los productos de la tabla productos del documento de detalle
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class DocDetalleTotalCajasData
	{
		public int totalCajas { get; set; }

		public DocDetalleTotalCajasData()
		{
			//Variables SearchMovimientos
			this.totalCajas = 0;
		}
	}

	/// <summary>
	/// Modelo para traer el total de unidades (cantidad) de los productos de la tabla productos del documento de detalle
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class DocDetalleTotalCantidadData
	{
		public int totalCantidad { get; set; }

		public DocDetalleTotalCantidadData()
		{
			//Variables SearchMovimientos
			this.totalCantidad = 0;
		}
	}

	/// <summary>
	/// Modelo para traer el total del peso de los productos de la tabla productos del documento de detalle
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class DocDetalleTotalPesoData
	{
		public int totalPeso { get; set; }

		public DocDetalleTotalPesoData()
		{
			//Variables SearchMovimientos
			this.totalPeso = 0;
		}
	}

	/// <summary>
	/// Modelo para traer la fecha minima de caducidad de productos de la tabla productos del documento de detalle
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class DocDetalleFechaMinData
	{
		public string fechaMin { get; set; }

		public DocDetalleFechaMinData()
		{
			//Variables SearchMovimientos
			this.fechaMin = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para traer los datos del usuario invitado
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class usuarioInvitadoData
	{
		public string email { get; set; }
		public string password { get; set; }
		public int usuario { get; set; }

		public usuarioInvitadoData()
		{
			this.email = String.Empty;
			this.password = String.Empty;
			this.usuario = 0;
		}
	}
	#endregion

	#region Modelo para lista de autocomplete de Destinatarios
	/// <summary>
	/// Modelo para traer los datos de los movimientos en la consulta
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class ListDestinatariosData
	{
		public string razonSocial { get; set; }
		public string nombreDes { get; set; }
		public string apellidoDes { get; set; }
		public string telefono { get; set; }
		public int pais { get; set; }
		public int estado { get; set; }
		public string npais { get; set; }
		public string nestado { get; set; }
		public string ciudad { get; set; }
		public int codigoPostal { get; set; }
		public string direccion { get; set; }
		public int usuario { get; set; }
		public string numeroC { get; set; }
		public string rancho { get; set; }
		public string sector { get; set; }

		public ListDestinatariosData()
		{
			//Variables SearchMovimientos
			this.razonSocial = String.Empty;
			this.nombreDes = String.Empty;
			this.apellidoDes = String.Empty;
			this.telefono = String.Empty;
			this.pais = 0;
			this.estado = 0;
			this.nestado = String.Empty;
			this.npais = String.Empty;
			this.ciudad = String.Empty;
			this.codigoPostal = 0;
			this.direccion = String.Empty;
			this.usuario = 0;
			this.rancho = String.Empty;
			this.sector = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para traer los datos de los movimientos en la consulta
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class ListDestinatariosData2
	{
		public string razonSocial { get; set; }
		public string nombreDes { get; set; }
		public string apellidoDes { get; set; }
		public string telefono { get; set; }
		public int pais { get; set; }
		public int estado { get; set; }
		public string npais { get; set; }
		public string nestado { get; set; }
		public string ciudad { get; set; }
		public int codigoPostal { get; set; }
		public string direccion { get; set; }
		public int usuario { get; set; }
		public string numeroC { get; set; }
		public string rancho { get; set; }
		public string sector { get; set; }

		public ListDestinatariosData2()
		{
			//Variables SearchMovimientos
			this.razonSocial = String.Empty;
			this.nombreDes = String.Empty;
			this.apellidoDes = String.Empty;
			this.telefono = String.Empty;
			this.pais = 0;
			this.estado = 0;
			this.nestado = String.Empty;
			this.npais = String.Empty;
			this.ciudad = String.Empty;
			this.codigoPostal = 0;
			this.direccion = String.Empty;
			this.usuario = 0;
			this.rancho = String.Empty;
			this.sector = String.Empty;
		}
	}
	#endregion

	#region Modelos para existencia de estado
	/// <summary>
	/// Modelo para traer los datos generales de un movimiento
	/// Desarrollador: Javier Ramirez OK
	/// </summary>
	public class EstadoExistencia
	{
		public bool bandera { get; set; }

		public EstadoExistencia()
		{
			this.bandera = false;
		}
	}
	#endregion

	#region Modelos para obtener los embalajes por familia
	/// <summary>
	/// Modelo para obtener los embalajes por familia
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class PackagingByFamily
	{
		public int id { get; set; }
		public string data { get; set; }
		public int unitsPerBox { get; set; }
		public int boxesPerPallet { get; set; }
		public int copiesBox { get; set; }
		public int copiesPallet { get; set; }
		public int productId { get; set; }
		public bool autoLote { get; set; }
		public string prefix { get; set; }
		public int consecutiveLote { get; set; }
		public bool editLote { get; set; }

		#region Constructor
		public PackagingByFamily()
		{
			this.id = 0;
			this.data = String.Empty;
			this.unitsPerBox = 0;
			this.boxesPerPallet = 0;
			this.copiesBox = 0;
			this.copiesPallet = 0;
			this.productId = 0;
			this.autoLote = false;
			this.prefix = String.Empty;
			this.consecutiveLote = 0;
			this.editLote = false;
		}
		#endregion
	}
	#endregion

	#region Modelos para obtener la información de los productos (familias) y las cajas de un movimiento movimiento
	/// <summary>
	/// Modelo para obtener la información de la familia de un movimiento
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class FamilyTypeInfoMov
	{
		public int id { get; set; }
		public int productId { get; set; }
		public string boxId { get; set; }

		#region Constructor
		public FamilyTypeInfoMov()
		{
			this.id = 0;
			this.productId = 0;
			this.boxId = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Modelo para obtener la información de las cajas de un movimiento
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class BoxInfoMov
	{
		public int id { get; set; }
		public string codeI { get; set; }
		public string codeF { get; set; }
		public string boxId { get; set; }
		public string codeQR { get; set; }

		#region Constructor
		public BoxInfoMov()
		{
			this.id = 0;
			this.codeI = String.Empty;
			this.codeF = String.Empty;
			this.boxId = String.Empty;
			this.codeQR = String.Empty;
		}
		#endregion
	}
	#endregion
}
