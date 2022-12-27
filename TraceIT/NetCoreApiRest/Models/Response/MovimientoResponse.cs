using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Movimientos;

namespace WSTraceIT.Models.Response
{
	#region Busqueda para tabla del catalogo movimientos
	//Busqueda de movimientos
	public class SearchMovimientoResponse : TraceITResponse
    {
		public List<MovimientosData> movimientosDataList { get; set; }

		public SearchMovimientoResponse()
		{
			this.movimientosDataList = new List<MovimientosData>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de datos para los combos de filtros de la tabla movimientos
	//Busqueda para los combos tipo de movimientos y productos
	public class SearchComboMovimientoResponse : TraceITResponse
	{
		public List<TraceITListDropDown> tiposMovimientosDataComboList { get; set; }
		public List<TraceITListDropDown> productosDataComboList { get; set; }

		public SearchComboMovimientoResponse()
		{
			this.tiposMovimientosDataComboList = new List<TraceITListDropDown>();
			this.productosDataComboList = new List<TraceITListDropDown>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Request para buscar datos de un solo movimiento (Separados)
	//Busqueda de un solo dato de la tabla movimientos general
	public class SearchDataMovimientoGeneralResponse : TraceITResponse
    {
		public MovimientosDataGeneral movimientosDataGeneralRecepList { get; set; }

		public SearchDataMovimientoGeneralResponse()
		{
			this.movimientosDataGeneralRecepList = new MovimientosDataGeneral();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos general
	public class SearchDataMovimientoGeneralRecepResponse : TraceITResponse
	{
		public MovimientosDataGeneral movimientosDataGeneralRecepList { get; set; }

		public SearchDataMovimientoGeneralRecepResponse()
		{
			this.movimientosDataGeneralRecepList = new MovimientosDataGeneral();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos general
	public class SearchDataMovimientoGeneralRecepResponse2 : TraceITResponse
	{
		public List<MovimientosDataGeneral> movimientosDataGeneralRecepList { get; set; }

		public SearchDataMovimientoGeneralRecepResponse2()
		{
			this.movimientosDataGeneralRecepList = new List<MovimientosDataGeneral>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos general pero solo los productos
	public class SearchDataMovimientoGeneralProdResponse : TraceITResponse
	{
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralProdRecepList { get; set; }
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralDesProdRecepList { get; set; }
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralUnicoProdRecepList { get; set; }
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralOperaProdRecepList { get; set; }
		//public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralReagrupadoProdRecepList { get; set; }

		public SearchDataMovimientoGeneralProdResponse()
		{
			this.movimientosDataGeneralProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.movimientosDataGeneralDesProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.movimientosDataGeneralUnicoProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.movimientosDataGeneralOperaProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			//this.movimientosDataGeneralReagrupadoProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos general pero solo los productos
	public class SearchDataMovimientoGeneralProdRecepResponse : TraceITResponse
	{
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralProdRecepList { get; set; }
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralDesProdRecepList { get; set; }
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralUnicoProdRecepList { get; set; }
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralOperaProdRecepList { get; set; }

		public SearchDataMovimientoGeneralProdRecepResponse()
		{
			this.movimientosDataGeneralProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.movimientosDataGeneralDesProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.movimientosDataGeneralUnicoProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.movimientosDataGeneralOperaProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos observaciones
	public class SearchDataMovimientoObservacionResponse : TraceITResponse
	{
		public MovimientosDataObservacion movimientosDataObservacionRecepList { get; set; }

		public SearchDataMovimientoObservacionResponse()
		{
			this.movimientosDataObservacionRecepList = new MovimientosDataObservacion();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos observaciones
	public class SearchDataMovimientoNombreResponse : TraceITResponse
	{
		public MovimientosDataNombre movimientosDataNombreRecepList { get; set; }

		public SearchDataMovimientoNombreResponse()
		{
			this.movimientosDataNombreRecepList = new MovimientosDataNombre();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos remitente
	public class SearchDataMovimientoRemitenteResponse : TraceITResponse
	{
		public MovimientosDataRemitente movimientosDataRemitenteRecepList { get; set; }

		public SearchDataMovimientoRemitenteResponse()
		{
			this.movimientosDataRemitenteRecepList = new MovimientosDataRemitente();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos remitente
	public class SearchDataMovimientoRemitenteRecepResponse : TraceITResponse
	{
		public MovimientosDataRemitente movimientosDataRemitenteRecepList { get; set; }

		public SearchDataMovimientoRemitenteRecepResponse()
		{
			this.movimientosDataRemitenteRecepList = new MovimientosDataRemitente();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos destinatario
	public class SearchDataMovimientoDestinatarioResponse : TraceITResponse
	{
		public MovimientosDataDestinatario movimientosDataDestinatarioRecepList { get; set; }

		public SearchDataMovimientoDestinatarioResponse()
		{
			this.movimientosDataDestinatarioRecepList = new MovimientosDataDestinatario();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos destinatario
	public class SearchDataMovimientoDestinatarioRecepResponse : TraceITResponse
	{
		public MovimientosDataDestinatario movimientosDataDestinatarioRecepList { get; set; }

		public SearchDataMovimientoDestinatarioRecepResponse()
		{
			this.movimientosDataDestinatarioRecepList = new MovimientosDataDestinatario();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos transportista
	public class SearchDataMovimientoTransportistaResponse : TraceITResponse
	{
		public MovimientosDataTransportista movimientosDataTransportistaList { get; set; }

		public SearchDataMovimientoTransportistaResponse()
		{
			this.movimientosDataTransportistaList = new MovimientosDataTransportista();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos info legal
	public class SearchDataMovimientoInfoLegalResponse : TraceITResponse
	{
		public MovimientosDataInfoLegal movimientosDataInfoLegalRecepList { get; set; }

		public SearchDataMovimientoInfoLegalResponse()
		{
			this.movimientosDataInfoLegalRecepList = new MovimientosDataInfoLegal();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos info legal
	public class SearchDataMovimientoInfoLegalRecepResponse : TraceITResponse
	{
		public MovimientosDataInfoLegal movimientosDataInfoLegalRecepList { get; set; }

		public SearchDataMovimientoInfoLegalRecepResponse()
		{
			this.movimientosDataInfoLegalRecepList = new MovimientosDataInfoLegal();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos info legal
	public class SearchDataMovimientoInfoEtiquetaResponse : TraceITResponse
	{
		public MovimientosDataInfoLabel movimientosDataInfoEtiquetaList { get; set; }

		public SearchDataMovimientoInfoEtiquetaResponse()
		{
			this.movimientosDataInfoEtiquetaList = new MovimientosDataInfoLabel();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de un solo dato de la tabla movimientos merma
	public class SearchDataMovimientoMermaResponse : TraceITResponse
	{
		public List<TraceITMovimientosDataMerma> movimientosDataMermaList { get; set; }

		public SearchDataMovimientoMermaResponse()
		{
			this.movimientosDataMermaList = new List<TraceITMovimientosDataMerma>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de cajas para un pallet
	public class SearchDataMovimientoGeneralProdsPalletResponse : TraceITResponse
	{
		public List<CajasPalletLst> cajasPalletList { get; set; }

		public SearchDataMovimientoGeneralProdsPalletResponse()
		{
			this.cajasPalletList = new List<CajasPalletLst>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	// Busqueda de un movimiento por texto o id
	public class SearchMovimientoCodeResponse : TraceITResponse
	{
		public string codigoQR { get; set; }

		public SearchMovimientoCodeResponse()
		{
			this.codigoQR = String.Empty;
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqeuda de datos para los comobos que poseen paises y estados
	//Busqueda para los combos pais y estado
	public class SearchComboPaisEstadoResponse : TraceITResponse
	{
		public List<TraceITListDropDown> paisesDataComboList { get; set; }
		public List<TraceITListDropDown> estadosDataComboList { get; set; }

		public SearchComboPaisEstadoResponse()
		{
			this.paisesDataComboList = new List<TraceITListDropDown>();
			this.estadosDataComboList = new List<TraceITListDropDown>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de nombres de los tipo de información legal para mostrarse en radios buttons
	//Busqueda para los radio button tipo info legal
	public class SearchRadioTipoInfoResponse : TraceITResponse
	{
		public List<TraceITListDropDown> tipoInfoRadioList { get; set; }

		public SearchRadioTipoInfoResponse()
		{
			this.tipoInfoRadioList = new List<TraceITListDropDown>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Edición de datos de un solo movimiento (Separados)
	//Edicion de un solo dato de la tabla movimientos general
	public class EditDataMovimientoGeneralResponse : TraceITResponse
	{
		public EditDataMovimientoGeneralResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Edicion de un solo dato de la tabla movimientos general
	public class EditDataMovimientoObservacionResponse : TraceITResponse
	{
		public EditDataMovimientoObservacionResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Edicion de un solo dato de la tabla movimientos general
	public class EditDataMovimientoNombreResponse : TraceITResponse
	{
		public EditDataMovimientoNombreResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}


	//Edicion de un solo dato de la tabla movimientos general
	public class EditDataMovimientoRemitenteResponse : TraceITResponse
	{
		public EditDataMovimientoRemitenteResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Edicion de un solo dato de la tabla movimientos general
	public class EditDataMovimientoDestinatarioResponse : TraceITResponse
	{
		public EditDataMovimientoDestinatarioResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Edicion de un solo dato de la tabla movimientos general
	public class EditDataMovimientoTransportistaResponse : TraceITResponse
	{
		public EditDataMovimientoTransportistaResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Edicion de un solo dato de la tabla movimientos general
	public class EditDataMovimientoInfoLegalResponse : TraceITResponse
	{
		public EditDataMovimientoInfoLegalResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Edicion de un solo dato de la tabla movimientos general
	public class EditDataMovimientoMermaResponse : TraceITResponse
	{
		public EditDataMovimientoMermaResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de documentos de la info legal
	//Busqueda de documentos
	public class SearchDocsInfoLegaloResponse : TraceITResponse
	{
		public List<DocsInfoLegalData> docsInfoLegalDataList { get; set; }

		public SearchDocsInfoLegaloResponse()
		{
			this.docsInfoLegalDataList = new List<DocsInfoLegalData>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de datos para tabla de existencia estiamda
	//Busqueda de existencia estimada
	public class SearchExistenciaEstimadaResponse : TraceITResponse
	{
		public List<ExistenciaEstimadaData> existenciaEstimadaDataList { get; set; }

		public SearchExistenciaEstimadaResponse()
		{
			this.existenciaEstimadaDataList = new List<ExistenciaEstimadaData>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Registrar nuevos documentos de info legal
	//Guardar docs info legal
	public class SaveDocsInfoLegalResponse : TraceITResponse
	{
		public SaveDocsInfoLegalResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Registrar un nuevo movimiento y cambio de su status
	//Guardar movimientos
	public class SaveMovimientosResponse : TraceITResponse
	{
		public SaveMovimientosResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Edicion de un tipo de movimiento de agrupación a envío
	public class EditDataAgruAEnvioResponse : TraceITResponse
	{
		public int movimientoId { get; set; }
		public EditDataAgruAEnvioResponse()
		{
			this.movimientoId = 0;
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de info para rellenar documento de detalle al seleccionar un movimiento desde envío
	//Guardar USUARIOS
	public class SaveUsuarioResponse : TraceITResponse
	{
		public SaveUsuarioResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Guardar USUARIOS
	public class SaveUsuarioInvitadoResponse : TraceITResponse
	{

		public usuarioInvitadoData usuarioInvitadoList { get; set; }

		public SaveUsuarioInvitadoResponse()
		{
			this.usuarioInvitadoList = new usuarioInvitadoData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Guardar USUARIOS
	public class DeleteUsuarioInvitadoResponse : TraceITResponse
	{
		public DeleteUsuarioInvitadoResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de info para rellenar documento de detalle al seleccionar un movimiento desde envío
	//Busqueda de datos generales de la tabla productos del documento detalles
	public class SearchDocDetalleProductosResponse : TraceITResponse
	{
		public List<DocDetalleProductoData> docDetalleProductoList { get; set; }

		public SearchDocDetalleProductosResponse()
		{
			this.docDetalleProductoList = new List<DocDetalleProductoData>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de datos generales de la tabla productos del documento detalles
	public class SearchDocDetalleProductosIndiResponse : TraceITResponse
	{
		public List<DocDetalleProductoData> docDetalleProductoIndiList { get; set; }

		public SearchDocDetalleProductosIndiResponse()
		{
			this.docDetalleProductoIndiList = new List<DocDetalleProductoData>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de datos generales de la tabla productos del documento detalles
	public class SearchDocDetalleProductosCajasResponse : TraceITResponse
	{
		public List<DocDetalleProductoData> docDetalleProductoCajasList { get; set; }

		public SearchDocDetalleProductosCajasResponse()
		{
			this.docDetalleProductoCajasList = new List<DocDetalleProductoData>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de cantidad total de productos de la tabla productos del documento detalles
	public class SearchDocDetalleTotalProdResponse : TraceITResponse
	{
		public DocDetalleTotalProdData docDetalleTotalProdList { get; set; }

		public SearchDocDetalleTotalProdResponse()
		{
			this.docDetalleTotalProdList = new DocDetalleTotalProdData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de datos de cantidad total de pallest de la tabla productos del documento detalles
	public class SearchDocDetalleTotalPalletResponse : TraceITResponse
	{
		public DocDetalleTotalPalletData docDetalleTotalPalletList { get; set; }

		public SearchDocDetalleTotalPalletResponse()
		{
			this.docDetalleTotalPalletList = new DocDetalleTotalPalletData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de datos de cantidad total de cajas de la tabla productos del documento detalles
	public class SearchDocDetalleTotalCajasResponse : TraceITResponse
	{
		public DocDetalleTotalCajasData docDetalleTotalCajasList { get; set; }

		public SearchDocDetalleTotalCajasResponse()
		{
			this.docDetalleTotalCajasList = new DocDetalleTotalCajasData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de datos de la cantidad total de unidades de la tabla productos del documento detalles
	public class SearchDocDetalleTotalCantidadResponse : TraceITResponse
	{
		public DocDetalleTotalCantidadData docDetalleTotalCantidadList { get; set; }

		public SearchDocDetalleTotalCantidadResponse()
		{
			this.docDetalleTotalCantidadList = new DocDetalleTotalCantidadData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de datos de la cantidad total del peso de los productos de la tabla productos del documento detalles
	public class SearchDocDetalleTotalPesoResponse : TraceITResponse
	{
		public DocDetalleTotalPesoData docDetalleTotalPesoList { get; set; }

		public SearchDocDetalleTotalPesoResponse()
		{
			this.docDetalleTotalPesoList = new DocDetalleTotalPesoData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de datos de la cantidad total del peso de los productos de la tabla productos del documento detalles
	public class SearchDocDetalleFechaMinResponse : TraceITResponse
	{
		public DocDetalleFechaMinData docDetalleFechaMinList { get; set; }

		public SearchDocDetalleFechaMinResponse()
		{
			this.docDetalleFechaMinList = new DocDetalleFechaMinData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de datos de usuario invitado
	//Busqueda de datos de la cantidad total del peso de los productos de la tabla productos del documento detalles
	public class SearchDataUsuarioInvitadoResponse : TraceITResponse
	{
		public usuarioInvitadoData dataUsuarioInvitadoList { get; set; }

		public SearchDataUsuarioInvitadoResponse()
		{
			this.dataUsuarioInvitadoList = new usuarioInvitadoData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de destinatarios de un usuario para autocomplete
	//Busqueda de movimientos
	public class SearchListDestinatariosResponse : TraceITResponse
	{
		public List<ListDestinatariosData> destinatariosDataList { get; set; }

		public SearchListDestinatariosResponse()
		{
			this.destinatariosDataList = new List<ListDestinatariosData>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	//Busqueda de movimientos
	public class SearchListDestinatariosResponse2 : TraceITResponse
	{
		public List<ListDestinatariosData2> destinatariosDataList2 { get; set; }

		public SearchListDestinatariosResponse2()
		{
			this.destinatariosDataList2 = new List<ListDestinatariosData2>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Insersion de estados no existentes en los combos
	//Guardar docs info legal
	public class SaveEstadosSiNoExisteResponse : TraceITResponse
	{
		public SaveEstadosSiNoExisteResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de Existencia de Estadp
	//Guardar docs info legal
	public class SearchExistenciaEstadoResponse : TraceITResponse
	{
		public EstadoExistencia existenciaEstadoList { get; set; }

		public SearchExistenciaEstadoResponse()
		{
			this.existenciaEstadoList = new EstadoExistencia();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de Embalajes por familia en el envio de un movimiento
	// Busqueda de embalajes por familia
	public class SearchPackagingByfamilyResponse : TraceITResponse
	{
		public List<PackagingByFamily> packagingLst { get; set; }

		public SearchPackagingByfamilyResponse()
		{
			this.packagingLst = new List<PackagingByFamily>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de Consecutivo Etiqueta para imprimir desde Pallet o Caja
	// Busqueda de Consecutivo Etiqueta por familia
	public class SearchConsecutiveByfamilyResponse : TraceITResponse
	{
		public int consecutiveStart { get; set; }
		public int consecutiveEnd { get; set; }

		public SearchConsecutiveByfamilyResponse()
		{
			this.consecutiveStart = 0;
			this.consecutiveEnd = 0;
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion

	#region Busqueda de Cajas de un movimiento agrupado necesarios para imprimir desde Pallet o Caja
	// Busqueda de Cajas de un movimiento
	public class SearchBoxesInfoByMovResponse : TraceITResponse
	{
		public List<FamilyTypeInfoMov> familyInfo { get; set; }
		public List<BoxInfoMov> boxesInfo { get; set; }

		public SearchBoxesInfoByMovResponse()
		{
			this.familyInfo = new List<FamilyTypeInfoMov>();
			this.boxesInfo = new List<BoxInfoMov>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
	#endregion
}
