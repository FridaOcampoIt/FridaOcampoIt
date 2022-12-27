using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
	public class TrackingSQL : DBHelperDapper
	{
		#region Properties
		public int trackingId;
		public string searchCode;
		public string searchCIUCode;
		public int phase;
		public int opc;
		public int userId;

		// Info del movimiento general
		public List<TrackingEventInfoSQL> eventInfo;
		// Info del remitente
		public List<TrackingEventSenderInfoSQL> eventSenderInfo;
		// Info de la info legal
		public List<TrackingEventLegalInfoSQL> eventLegalInfo;
		public List<TrackingEventLegalDocsInfoSQL> eventLegalDocsInfo;
		// Info del destinatario
		public List<TrackingEventRecipientInfoSQL> eventRecipientInfo;
		// Info de productos del movimiento
		public List<TrackingEventProductsInfoSQL> eventProductsInfo;

		public List<TrackingEventCarriersInfoSQL> eventTransportInfo;
		public List<TrackingEventProuctDetailInfoSQL> eventProdDetailInfo;
		public List<TrackingEventTotalProdInfoSQL> eventTotalProdInfo;
		public List<TrackingEventTotalPalletInfoSQL> eventTotalPalletInfo;
		public List<TrackingEventTotalBoxInfoSQL> eventTotalBoxInfo;
		public List<TrackingEventTotalQuantityInfoSQL> eventTotalQuantityInfo;
		public List<TrackingEventTotalWeightInfoSQL> eventTotalWeightInfo;
		public List<TrackingEventDateMinInfoSQL> eventDateMinInfo;

		public TrackingStockDocs datosStockDocs;
		public List<documentoStock> documentoStocks;
		public List<TrackingDocumentosFamilia> documentosFamilias;
		public List<alertas> listaAlertas;

		public InfoFamilia infoFamilia;

		public int movementID;

		public string ciu;
		public string lat;
		public string lon;
		public string json;
		public int tipo;

		public string nombre;
		public string apellido;
		public string cargo;
		public string fecha;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("TrackingSQL");
		#endregion

		#region Constructor
		public TrackingSQL()
		{
			this.trackingId = 0;
			this.searchCode = String.Empty;
			this.searchCIUCode = String.Empty;
			this.phase = 0;
			this.opc = 0;
			this.userId = 0;

			this.eventInfo = new List<TrackingEventInfoSQL>();
			this.eventSenderInfo = new List<TrackingEventSenderInfoSQL>();
			this.eventLegalInfo = new List<TrackingEventLegalInfoSQL>();
			this.eventLegalDocsInfo = new List<TrackingEventLegalDocsInfoSQL>();
			this.eventRecipientInfo = new List<TrackingEventRecipientInfoSQL>();
			this.eventProductsInfo = new List<TrackingEventProductsInfoSQL>();

			this.eventTransportInfo = new List<TrackingEventCarriersInfoSQL>();
			this.eventProdDetailInfo = new List<TrackingEventProuctDetailInfoSQL>();
			this.eventTotalProdInfo = new List<TrackingEventTotalProdInfoSQL>();
			this.eventTotalPalletInfo = new List<TrackingEventTotalPalletInfoSQL>();
			this.eventTotalBoxInfo = new List<TrackingEventTotalBoxInfoSQL>();
			this.eventTotalQuantityInfo = new List<TrackingEventTotalQuantityInfoSQL>();
			this.eventDateMinInfo = new List<TrackingEventDateMinInfoSQL>();

			this.datosStockDocs = new TrackingStockDocs();
			this.documentosFamilias = new List<TrackingDocumentosFamilia>();
			this.documentosFamilias = new List<TrackingDocumentosFamilia>();
			this.listaAlertas = new List<alertas>();
			this.infoFamilia = new InfoFamilia();

			this.movementID = 0;

			this.ciu = String.Empty;
			this.lat = String.Empty;
			this.lon = String.Empty;
			this.json = String.Empty;
			this.tipo = 0;
		}
	#endregion

	#region Public methods

	#region BackOffice
		/// <summary>
		/// Metodo para consultar los rastreos
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<TrackingDataSQL> SearchTrackingList()
		{
			log.trace("SearchTrackingListSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				bool _isHex = false;
				string range = "";
				if (!String.IsNullOrEmpty(searchCode) && String.IsNullOrEmpty(searchCIUCode))
				{
					_isHex = isHexa(searchCode);
					if (_isHex)
					{
						long numbah = Convert.ToInt64(searchCode, 16);
						//Anidar a ranges los últimos 10 dígitos del long , los cuales pertenecen a un rango de ids de producto
						string numrango = numbah.ToString().Substring(numbah.ToString().Length - 10);
						numrango = numrango.TrimStart('0');
						range = numrango.ToString();
						parameters.Add("_searchCode", searchCode, DbType.String);
						parameters.Add("_searchCIUCode", searchCIUCode, DbType.String);
						parameters.Add("_typeCIU", 2, DbType.Int32); //Si es un ciu nuevo
					}
					else
					{
						parameters.Add("_searchCode", searchCode, DbType.String);
						parameters.Add("_searchCIUCode", searchCIUCode, DbType.String);
						parameters.Add("_typeCIU", 1, DbType.Int32); //Si es un ciu anterior
						
					}
				}
				else
				{
					_isHex = isHexa(searchCIUCode);
					if (_isHex)
					{
						long numbah = Convert.ToInt64(searchCIUCode, 16);
						//Anidar a ranges los últimos 10 dígitos del long , los cuales pertenecen a un rango de ids de producto
						string numrango = numbah.ToString().Substring(numbah.ToString().Length - 10);
						numrango = numrango.TrimStart('0');
						range = numrango.ToString();
						parameters.Add("_searchCode", searchCode, DbType.String);
						parameters.Add("_searchCIUCode", searchCIUCode, DbType.String);
						parameters.Add("_typeCIU", 2, DbType.Int32); //Si es un ciu nuevo
					}
					else
					{
						parameters.Add("_searchCode", searchCode, DbType.String);
						parameters.Add("_searchCIUCode", searchCIUCode, DbType.String);
						parameters.Add("_typeCIU", 1, DbType.Int32); //Si es un ciu anterior

					}
				}

				parameters.Add("_phase", phase, DbType.Int32);
				//parameters.Add("_userId", userId, DbType.Int32);

				List<TrackingDataSQL> response = Consulta<TrackingDataSQL>("spc_consultaHistoricoRastreos", parameters);

                if (response.Count > 0)
                    if (response[0].ciu == "-1")
                    {
						throw new Exception("El Codigo brindado no es rastreable.");
                    }

				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
				CerrarConexion();
				throw ex;
			}
		}

		public List<TrackingDataSQL> SearchTackingListBox()
		{
			log.trace("SearchTackingListBox");
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				DynamicParameters parameters = new DynamicParameters();
				bool _isHex = false;
				string range = "";

				//obtener el ciu
				parameters.Add("_searchCode", searchCode, DbType.String);
				parameters.Add("_searchCIUCode", searchCIUCode, DbType.String);
				parameters.Add("_phase", phase, DbType.Int32);
				parameters.Add("_typeCIU", 2, DbType.Int32); //Si es un ciu nuevo

				List<TrackingDataSQL> response = Consulta<TrackingDataSQL>("spc_consultaHistoricoRastreosCaja", parameters);

				CerrarConexion();
				return response;

			}
            catch (Exception ex)
            {
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;

			}
		}

		public bool isHexa(string number)
		{
			try
			{
				if (number.Length == 13)
				{
					ulong a = ulong.Parse(number, System.Globalization.NumberStyles.HexNumber);
					return a.ToString()[0] == '1' ? true : a.ToString()[0] == '2' ? true : false;
				}
				else
				{

					return false;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		/// <summary>
		/// Metodo para consultar la info de un evento (movimiento) en rastreo
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public void GetTrackingInfo()
		{
			log.trace("GetTrackingInfoSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				DynamicParameters parametersX = new DynamicParameters();
				DynamicParameters parametersrem = new DynamicParameters();
				parameters.Add("_movimientosId", trackingId, DbType.Int32);
				parametersrem.Add("_movimientosId", trackingId, DbType.Int32);
				parametersrem.Add("_userId", 0, DbType.Int32);
				parametersX.Add("_movimientosId", trackingId, DbType.Int32);
				parametersX.Add("_codigoI", "", DbType.String);
				parametersX.Add("_codigoF", "", DbType.String);
				parametersX.Add("_codigoTipo", "", DbType.String);
				parametersX.Add("_codigoIHEXA", "", DbType.String);
				parametersX.Add("_codigoFHEXA", "", DbType.String);
				parametersX.Add("_numeroC", "", DbType.String);
				parametersX.Add("_isHexa", null, DbType.Boolean);

				if (opc == 1) {
					eventInfo = Consulta<TrackingEventInfoSQL>("spc_ConsultaDataMovimientoGeneral", parameters);
				}

				if (opc == 2) {
					eventSenderInfo = Consulta<TrackingEventSenderInfoSQL>("spc_ConsultaDataMovimientoRemitente", parametersrem);
				}

				if (opc == 3) {
					eventLegalInfo = Consulta<TrackingEventLegalInfoSQL>("spc_ConsultaDataMovimientoInfoLegal", parameters);
				}

				if (opc == 4) {
					eventRecipientInfo = Consulta<TrackingEventRecipientInfoSQL>("spc_ConsultaDataMovimientoDestinatario", parameters);
				}

				if (opc == 5) {
					eventProductsInfo = Consulta<TrackingEventProductsInfoSQL>("spc_ConsultaDataMovimientoGeneralProductos", parameters);
				}

				if (opc == 6) {
					eventInfo = Consulta<TrackingEventInfoSQL>("spc_ConsultaDataMovimientoGeneral", parameters);
					eventSenderInfo = Consulta<TrackingEventSenderInfoSQL>("spc_ConsultaDataMovimientoRemitente", parametersrem);
					eventLegalInfo = Consulta<TrackingEventLegalInfoSQL>("spc_ConsultaDataMovimientoInfoLegal", parameters);
					eventLegalDocsInfo = Consulta<TrackingEventLegalDocsInfoSQL>("spc_consultaDocumentosInfoLegal", parameters);
					eventRecipientInfo = Consulta<TrackingEventRecipientInfoSQL>("spc_ConsultaDataMovimientoDestinatario", parameters);

					eventProductsInfo = Consulta<TrackingEventProductsInfoSQL>("spc_ConsultaDataMovimientoGeneralProductos", parametersX);
					eventTransportInfo = Consulta<TrackingEventCarriersInfoSQL>("spc_ConsultaDataMovimientoTransportista", parameters);
					eventProdDetailInfo = Consulta<TrackingEventProuctDetailInfoSQL>("spc_ConsultaDocDetalleProductos", parameters);
					eventTotalProdInfo = Consulta<TrackingEventTotalProdInfoSQL>("spc_ConsultaDocDetalleTotalProd", parameters);
					eventTotalPalletInfo = Consulta<TrackingEventTotalPalletInfoSQL>("spc_ConsultaDocDetalleTotalPallet", parameters);
					eventTotalBoxInfo = Consulta<TrackingEventTotalBoxInfoSQL>("spc_ConsultaDocDetalleTotalCajas", parameters);
					eventTotalQuantityInfo = Consulta<TrackingEventTotalQuantityInfoSQL>("spc_ConsultaDocDetalleTotalCantidad", parameters);
					eventTotalWeightInfo = Consulta<TrackingEventTotalWeightInfoSQL>("spc_ConsultaDocDetalleTotalPeso", parameters);
					eventDateMinInfo = Consulta<TrackingEventDateMinInfoSQL>("spc_ConsultaDocDetalleFechaMin", parameters);
				}

				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		public void searchMovementId (string ciu, int tipo)
		{
			string ciuT = ciu;
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				// buscar en el rango de Iniciales (caso de rango positivo)

				int envio = 0;
				//Obtenemos la cadena en decimal
				string data = ulong.Parse(ciu, System.Globalization.NumberStyles.HexNumber).ToString();
				//Evaluamos si es código nuevo o viejo
				string nuevoViejo = data.Substring(0, 1);
				//Con esto obtenemos el embalaje o la familia (según sea el caso)
				string familiaEmbalaje = data.Substring(1, 5);
				familiaEmbalaje = familiaEmbalaje.Trim(new char[] {'0'});
				string aux = data.Substring(data.Length - 10);
				//Quitamos el auxiliar
				string familia = data.Substring(1);
				//Rescatamos la familia (eliminando el rango)
				familia = familia.Substring(0,5);
				if (tipo == 3)
                {
					int rango = 0;
					//OBtener el rango del ciu
					rango = getCIUid(ciu);
					// buscar el ciu de la caja a la que pertenece el ciu , por probabilidad de absolutos
					//ciuT = ConsultaCommand<string>($"(SELECT od.RangoMin FROM his_048_operacionDetalleEscaneados ode INNER JOIN his_047_operacionDetalle od ON od.DetalleId = ode.OperacionDetalleId WHERE ((select RIGHT(CONV(Codigo,16,10),10)) - {rango}) <= 0 ORDER BY ({rango} - (select RIGHT(CONV(Codigo,16,10),10))) LIMIT 1);").FirstOrDefault();
					//Corregímos la consulta a lo webon, buscamos por "probabilidad" (buscamos por rangos concretos y familias ó embalajes, no dejamos nada al aíre 🙄🙄

				ciuT = ConsultaCommand<string>(
							$"select " +
							$" RangoMin " +
							$"from( " +
							$"	select " +
							$"	* " +
							$"	from( " +
							$"		select  " +
							$"			REPLACE(LTRIM(REPLACE(RIGHT(CONV(od.RangoMin,16,10),10),'0',' ')),' ','0') RangoMinSinCero, " +
							$"			REPLACE(LTRIM(REPLACE(RIGHT(CONV(od.RangoMax ,16,10),10),'0',' ')),' ','0') RangoMaxSinCero, " +
							$"			SUBSTRING(LEFT(CONV(od.RangoMin ,16,10),6), 2, 6) familiaEmbalajeMin, " +
							$"			REPLACE(LTRIM(REPLACE(SUBSTRING(LEFT(CONV(od.RangoMin ,16,10),6), 2, 6),'0',' ')),' ','0') familiaoEmabalajeSinCerosMin, " +
							$"			RIGHT(CONV(od.RangoMin ,16,10),10) rangoMinConverter, " +
							$"			RIGHT(CONV(od.RangoMax ,16,10),10) rangoMax, " +
							$"			SUBSTRING(LEFT(CONV(od.RangoMin ,16,10),6), 1, 1) auxiliarMin, " +
							$"			SUBSTRING(LEFT(CONV(od.RangoMax ,16,10),6), 1, 1) auxiliarMax, " +
							$"			CONV(od.RangoMin ,16,10) codigoCompletoMin, " +
							$"			CONV(od.RangoMax  ,16,10) codigoCompletoMax, " +
							$"			od.RangoMin  " +
							$"		from his_048_operacionDetalleEscaneados ode  " +
							$"		INNER JOIN his_047_operacionDetalle od ON od.DetalleId = ode.OperacionDetalleId " +
							$"		where  " +
							$"			(od.RangoMin is not null) and (od.RangoMin is not null) " +
							$"		order by ode.EscaneadoId  desc " +
							$"	)busquedaTipoCodigo " +
							$"	where " +
							$"		busquedaTipoCodigo.auxiliarMin = {nuevoViejo}  " +
							$"		and " +
							$"		busquedaTipoCodigo.familiaoEmabalajeSinCerosMin = {familiaEmbalaje} " +
							$")busquedaRango " +
							$"	where " +
							$"		{rango} BETWEEN busquedaRango.RangoMinSinCero and busquedaRango.RangoMaxSinCero " +
							$"	order by {rango} - busquedaRango.RangoMinSinCero limit 1;").FirstOrDefault();
				}
				// buscar el movimiento ID
				this.movementID = ConsultaCommand<int>($"SELECT IFNULL(MovimientosId,0) FROM rel_021_productomovimiento WHERE CajaId LIKE '%{ciuT}%' ORDER BY MovimientosId DESC LIMIT 1;").FirstOrDefault();


				// si no se encontró en el rango de positivos, buscarlo en el de finales (caso de rango negativo)
				if (this.movementID == 0)
                {
					this.movementID = ConsultaCommand<int>($"SELECT IFNULL(MovimientoId,0) FROM rel_023_movimientoAgrupacion WHERE CajaId LIKE '%{ciu}%' ORDER BY MovimientoId DESC LIMIT 1;").FirstOrDefault();
				}

				// buscar si es envio/recep
                if (this.movementID != 0)
                {

					envio = ConsultaCommand<int>($"SELECT IFNULL(MovimientoParcialId, MovimientosId) FROM cat_027_movimientos WHERE MovimientosId = {this.movementID};").FirstOrDefault();

					if (envio != 0)
					{
						this.movementID = envio;
					}

				}
				

				CerrarConexion();
			}
            catch (Exception ex)
            {
				this.movementID = 0;
            }
        }

		public void searchDocs (string ciu, int movimientoId, string tipoBusqueda)
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				/*
				var consulta = $"select cf.FamiliaProductoId FamiliaProductoId from cat_027_movimientos cm " +
													 $"inner join rel_021_productomovimiento rp on rp.MovimientosId = cm.MovimientosId " +
													 $"inner join cat_001_familiaproducto cf on cf.FamiliaProductoId = rp.AgruMovimientosId " +
													 $"where cm.CodigoQR  like '%{ciu}%' and cm.MovimientosId = {movimientoId};";
				*/
				//Buscar la familia
				int familiaId = ConsultaCommand<int>($"select cf.FamiliaProductoId FamiliaProductoId from cat_027_movimientos cm " +  
													 $"inner join rel_021_productomovimiento rp on rp.MovimientosId = cm.MovimientosId " + 
													 $"inner join cat_001_familiaproducto cf on cf.FamiliaProductoId = rp.AgruMovimientosId " + 
													 $"where  cm.MovimientosId = {movimientoId};").FirstOrDefault();

				//Buscar los documentos PDFs de la familia
				if (familiaId > 0)
                {
					documentosFamilias = ConsultaCommand<TrackingDocumentosFamilia>($"SELECT Titulo, Url FROM cat_007_vinculo " + 
																					$"WHERE TipoVinculoId = 1 and Status = 1 and FamiliaProductoId = {familiaId};").ToList();
					infoFamilia = ConsultaCommand<InfoFamilia>($"SELECT fp.Nombre, fp.Modelo, c.Nombre as Compania FROM cat_001_familiaproducto as fp " + 
															   $"INNER JOIN cat_003_compania as c ON c.CompaniaId = fp.CompaniaId WHERE fp.FamiliaProductoId = {familiaId};").FirstOrDefault();
                }

                //Si hay movimientoId buscar los archivos
                if (movimientoId > 0)
                {
					//Obtener el envioId y el infoLegal de ese envioMovimiento
					datosStockDocs = ConsultaCommand<TrackingStockDocs>($"SELECT IFNULL(MovimientoParcialId, MovimientosId) as movimientoId, InfoLegalId FROM cat_027_movimientos " + 
																		$"where MovimientosId = {movimientoId};").FirstOrDefault();

					//Obtener los documentos de Info  Legal
					documentoStocks = ConsultaCommand<documentoStock>($"SELECT NombreDoc, Documento, TipoArchivo FROM cat_026_docinfolegal " + 
																	  $" WHERE InfoLegalId = {datosStockDocs.infoLegalId};").ToList();

				}

				//Obtener las Alertas
				DynamicParameters parameters = new DynamicParameters();

				// Para definir todo el listado de valores
				List<BusquedaCiu> reporteDePalletsAgrupaciones = new List<BusquedaCiu>();

				var codigoBuscar = ciu.ToUpper().ToString();

				// Si es una caja, evaluamos los Pallets y las Agrupaciones, para encontar alguna alerta
				var buscarCaja = true;

				//Obtenemos la cadena en decimal
				string data = ulong.Parse(ciu, System.Globalization.NumberStyles.HexNumber).ToString();
				//Evaluamos si es código nuevo o viejo
				string nuevoViejo = data.Substring(0, 1);
				//Familia 
				string familia = data.Substring(1, 5);
				Console.WriteLine($"data: {data} , NuevoViejo: {nuevoViejo}, Familia: {Convert.ToInt32(familia)}");
				var reportePorFamilia = ConsultaCommand<string>($"" +
												$"SELECT " +
												$"	RoboProductoId " +
												$"FROM his_004_robo hr " +
												$"WHERE " +
												$"	FK_CMMTipoReporte = 1000009 " +
												$"	AND " +
												$"	Estatus = 1000004 " +
												$"	AND FK_FamiliaId = {Convert.ToInt32(familia)}; ").FirstOrDefault();
				if (reportePorFamilia != null)
				{

					parameters.Add("_idReporte", reportePorFamilia, DbType.Int32);
					listaAlertas = Consulta<alertas>("spc_busquedaAlertas", parameters);
				}
				else
				{
					if (tipoBusqueda.Equals("C"))
					{
						//Buscamos en los pallets/Agrupaciones la caja que se esta activando
						//Traemos todas las alertas que se encuentre activas (Solo si son de Pallet o agrupaciones)
						reporteDePalletsAgrupaciones = ConsultaCommand<BusquedaCiu>($"" +
																					$"SELECT " +
																					$"	*	" +
																					$"FROM	(" +
																					$" SELECT " +
																					$"	RoboProductoId as idReporte, " +
																					$"	CodigoAlerta as codigoAlerta, " +
																					$"	FK_CMMTipoReporte as tipoReporte," +
																					$"	AgrupacionId agrupacionId," +
																					$"	Estatus" +
																					$" FROM his_004_robo " +
																					$" WHERE " +
																					$"	FK_CMMTipoReporte = 1000010 or FK_CMMTipoReporte = 1000013" +
																					$")Agrupacion	" +
																					$"WHERE" +
																					$"	Agrupacion.Estatus = 1000004;").ToList();
						if (reporteDePalletsAgrupaciones != null)
						{
							//Iteramos todas las alerta de pallet o agrupaciones
							foreach (var codigo in reporteDePalletsAgrupaciones)
							{
								//Guardamos el id de la alerta actual (para si se encuentra un resultado, rescatarlo)
								var idReporteActual = codigo.idReporte;
								var palletCajas = ConsultaCommand<string>($"" +
																		$"SELECT " +
																		$"	DISTINCT(SUBSTRING(hod.EtiquetaID, 2, LENGTH(hod.EtiquetaID))) CajaEtiqueta " +
																		$"	FROM rel_021_productomovimiento rp " +
																		$"	INNER JOIN cat_027_movimientos cm on cm.MovimientosId = rp.MovimientosId " +
																		$"	INNER JOIN his_020_movimientos hm on hm.MovimientosFK = cm.MovimientosId " +
																		$"	INNER JOIN rel_022_productooperacion rp2 ON rp2.movimientoId = cm.MovimientosId " +
																		$"	INNER JOIN his_047_operacionDetalle hod on hod.OperacionId = rp2.operacionId " +
																		$"WHERE rp.CajaId  LIKE CONCAT('%__', '{codigo.codigoAlerta}', '%');").ToList();

								if (palletCajas != null)
								{
									foreach (var caja in palletCajas)
									{
										if (codigoBuscar.Equals(caja))
										{
											parameters.Add("_idReporte", idReporteActual, DbType.Int32);
											listaAlertas = Consulta<alertas>("spc_busquedaAlertas", parameters);
											buscarCaja = false;
										}
									}
								}

							}
						}
						if (buscarCaja)
						{
							//Buscamos todas las alertas de caja.
							var busquedaCaja = ConsultaCommand<BusquedaCiu>($"" +
																		$"SELECT " +
																		$"	RoboProductoId idReporte, " +
																		$"	CodigoAlerta codigoAlerta," +
																		$"	FK_CMMTipoReporte as tipoReporte," +
																		$"	AgrupacionId agrupacionId " +
																		$"FROM his_004_robo " +
																		$"WHERE " +
																		$"FK_CMMTipoReporte = 1000011 AND Estatus = 1000004;").ToList();
							if (busquedaCaja != null)
							{
								foreach (var busqueda in busquedaCaja)
								{
									if (busqueda.codigoAlerta.Equals(ciu))
									{
										parameters.Add("_idReporte", busqueda.idReporte, DbType.Int32);
										listaAlertas = Consulta<alertas>("spc_busquedaAlertas", parameters);
									}
								}
							}
						}

					}
					//Busqueda directamente en Pallet/Agrupación
					if (tipoBusqueda.Equals("A") || tipoBusqueda.Equals("P"))
					{
						//Traemos todas las alertas que se encuentre activas (Solo si son de Pallet o agrupaciones)
						var reporte = ConsultaCommand<BusquedaCiu>($"" +
																	$"SELECT " +
																	$"	* " +
																	$"FROM( " +
																	$" SELECT " +
																	$"	RoboProductoId as idReporte, " +
																	$"	CodigoAlerta as codigoAlerta, " +
																	$"	FK_CMMTipoReporte as tipoReporte," +
																	$"	AgrupacionId agrupacionId," +
																	$"	Estatus" +
																	$" FROM his_004_robo " +
																	$" WHERE " +
																	$"	(FK_CMMTipoReporte = 1000010 or FK_CMMTipoReporte = 1000013) " +
																	$"	AND " +
																	$"	CodigoAlerta  like '%{ciu}%' " +
																	$")Agupacion	" +
																	$"WHERE " +
																	$"	Agrupacion.Estatus = 1000004;").FirstOrDefault();
						if (reporte != null)
						{
							parameters.Add("_idReporte", reporte.idReporte, DbType.Int32);
							listaAlertas = Consulta<alertas>("spc_busquedaAlertas", parameters);
						}
					}
				}
				CerrarConexion();
			}
            catch (Exception ex)
            {
				
				CerrarConexion();
            }
        }

		public bool mismaCompania(string ciu, int compania)
		{

			int companiadb = 0;

            if (ciu.Length > 13)
            {
				ciu = ciu.Substring(1);
            }

            try
            {

				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				int rango = 0;
				//OBtener el rango del ciu
				rango = getCIUid(ciu);
				// buscar el ciu de la caja a la que pertenece el ciu , por probabilidad de absolutos
				companiadb = ConsultaCommand<int>($"(SELECT fa.CompaniaId FROM cat_048_producto p INNER JOIN cat_001_familiaproducto fa ON fa.FamiliaProductoId = p.FamiliaProductoId WHERE (select RIGHT(CONV('{ciu}',16,10),10)) BETWEEN Inicio AND Fin LIMIT 1);").FirstOrDefault();

				CerrarConexion();

                if (companiadb == 0)
                {
					throw new Exception("El ciu ingresado no existe.");
                }

				if (compania == companiadb)
                {
					return true;
                } else	{
					return false;
                }

			}
            catch (Exception ex)
            {
				
				throw new Exception("El ciu no tiene el formato correcto, no pertenece a una familia o no existe.");
			}
			
			return false;
        }

		public void recepcionTracking()
		{
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();

				parameters.Add("_movimientoId", this.movementID, DbType.Int32);
				parameters.Add("_nombre", nombre, DbType.String);
				parameters.Add("_apellido", apellido, DbType.String);
				parameters.Add("_cargo", cargo, DbType.String);
				parameters.Add("_fecha", fecha, DbType.String);
				parameters.Add("_lat", lat, DbType.String);
				parameters.Add("_lon", lon, DbType.String);

				Consulta<string>("spi_guardarRecepcionTracking", parameters);

				CerrarConexion();
			}
            catch (Exception ex)
            {

				throw new Exception(ex.Message);
            }
        }

		public static int getCIUid(string QR)
		{
			try
			{
				string data = ulong.Parse(QR, System.Globalization.NumberStyles.HexNumber).ToString();
				Console.WriteLine(int.Parse(data.Substring(6)));
				return (int.Parse(data.Substring(6)));
			}
			catch
			{
				return (-1);
			}
		}

		public void registroLog()
		{
            try
            {

				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();

				parameters.Add("_ciu", ciu, DbType.String);
				parameters.Add("_lat", lat, DbType.String);
				parameters.Add("_lon", lon, DbType.String);
				parameters.Add("_json", json, DbType.String);
				parameters.Add("_tipo", tipo, DbType.Int32);

				Consulta<string>("spi_logTracking", parameters);


				CerrarConexion();

            }
            catch (Exception ex)
            {

				throw ex;
            }
        }
	#endregion

	#endregion
	}
}
