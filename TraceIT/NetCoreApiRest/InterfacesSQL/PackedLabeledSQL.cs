using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Response;
using NetCoreApiRest.Utils;
using System.Reflection.Emit;

namespace WSTraceIT.InterfacesSQL
{
	public class PackedLabeledSQL : DBHelperDapper
	{
		#region Properties
		//Datos Empacado y Etiquetado
		public int id;
		public int userId;
		public int opc;
		public string typeCompany;

		// Materias Primas
		public int companyId;
		public int productId;
		public int packagingId;
		public int providerId;

		//Nombre SP a ejecutar
		public string sp;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("PackedLabeledSQL");

		//variable para validar existencia de CIU
		public string ciu;

		// Variables para la lectura de Pallets
		public int opeId;
		public string ciuF;
		public int init;
		public int pallet;
		public List<InfoQRCodeSQL> pallets;
		public int totalP;
		#endregion

		#region Constructor
		public PackedLabeledSQL()
		{
			this.id = 0;
			this.userId = 0;
			this.opc = 0;


			this.companyId = 0;
			this.productId = 0;
			this.packagingId = 0;
			this.providerId = 0;
			this.typeCompany = "";
			this.sp = String.Empty;

			// Variables para la lectura de Pallets
			this.opeId = 0;
			this.ciuF = String.Empty;
			this.init = 0;
			this.pallet = 0;
			this.pallets = new List<InfoQRCodeSQL>();
			this.totalP = 0;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar las compañias para los combos (selects)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<PackedLabeledComboSQL> SearchCompanyCombo()
		{
			log.trace("SearchCompanyComboSQL");
			try {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_id", 0, DbType.Int32);
				parameters.Add("_opc", opc, DbType.Int32);

				List<PackedLabeledComboSQL> response = Consulta<PackedLabeledComboSQL>("spc_consultaCombosEmpEtq", parameters);
				CerrarConexion();

				return response;
			} catch (Exception ex) {
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los productos para los combos (selects)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<PackedLabeledComboSQL> SearchProductCombo()
		{
			log.trace("SearchProductComboSQL");
			try {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_id", id, DbType.Int32);
				parameters.Add("_opc", opc, DbType.Int32);

				List<PackedLabeledComboSQL> response = Consulta<PackedLabeledComboSQL>("spc_consultaCombosEmpEtq", parameters);
				CerrarConexion();

				return response;
			} catch (Exception ex) {
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los embalajes para los combos (selects)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<PackedLabeledComboSQL> SearchPackagingCombo()
		{
			log.trace("SearchPackagingComboSQL");
			try {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_id", id, DbType.Int32);
				parameters.Add("_opc", opc, DbType.Int32);

				List<PackedLabeledComboSQL> response = Consulta<PackedLabeledComboSQL>("spc_consultaCombosEmpEtq", parameters);
				CerrarConexion();

				return response;
			} catch (Exception ex) {
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los compañias, productos y embalajes para los combos (selects)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public SearchComProdPackComboResponse SearchComProdPackCombos()
		{
			log.trace("SearchComProdPackCombosSQL");
			try {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TraceITListDropDown));
				types.Add(typeof(TraceITListDropDown));
				types.Add(typeof(TraceITListDropDown));

				SearchComProdPackComboResponse response = new SearchComProdPackComboResponse();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_id", 0, DbType.Int32);
				parameters.Add("_opc", opc, DbType.Int32);

				var respSQL = ConsultaMultiple("spc_consultaCombosEmpEtq", types, parameters);

				response.companiescombo = respSQL[0].Cast<TraceITListDropDown>().ToList();
				response.productscombo = respSQL[1].Cast<TraceITListDropDown>().ToList();
				response.packagingcombo = respSQL[2].Cast<TraceITListDropDown>().ToList();

				CerrarConexion();

				return response;
			} catch (Exception ex) {
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los productos para los combos (selects)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<PackedLabeledComboSQL> SearchFamilyProductCombo()
		{
			log.trace("SearchFamilyProductComboSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_id", id, DbType.Int32);
				parameters.Add("_opc", opc, DbType.Int32);

				List<PackedLabeledComboSQL> response = Consulta<PackedLabeledComboSQL>("spc_consultaCombosEmpEtq", parameters);
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

		/// <summary>
		/// Metodo para consultar los operadores y lineas de producción (selects) de una compañia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<PackedLabeledComboSQL> SearchCompanyInfoCombo()
		{
			log.trace("SearchCompanyInfoComboSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_id", id, DbType.Int32);
				parameters.Add("_opc", opc, DbType.Int32);

				List<PackedLabeledComboSQL> response = Consulta<PackedLabeledComboSQL>("spc_consultaCompaniaInfoCombos", parameters);
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

		public List<PackedLabeledComboSQL> SearchCompanyInfoComboEmbalaje()
		{
			log.trace("SearchCompanyInfoComboSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_id", id, DbType.Int32);
				parameters.Add("_opc", opc, DbType.Int32);
				parameters.Add("_familiaId", productId, DbType.Int32);
				parameters.Add("_providerId", providerId, DbType.Int32);

				List<PackedLabeledComboSQL> response = Consulta<PackedLabeledComboSQL>("spc_consultaCompaniaInfoCombosEmbalaje", parameters);
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

		/// <summary>
		/// Metodo para consultar las materias primas de un proveedor de acuerdo al filtro aplicado
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<RawMaterialsSQL> SearchRawMaterialsList()
		{
			log.trace("SearchRawMaterialsListSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				parameters.Add("_packagingId", packagingId, DbType.Int32);
				parameters.Add("_providerId", providerId, DbType.Int32);

				List<RawMaterialsSQL> response = Consulta<RawMaterialsSQL>("spc_consultaMateriasPrimas", parameters);
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

		/// <summary>
		/// Metodo para consultar las direcciones para un combo (select) de acuerdo a la compañia del usuario autenticado
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<PackedLabeledComboSQL> SearchAddressComboList()
		{
			log.trace("SearchAddressComboListSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyType", id, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);

				List<PackedLabeledComboSQL> response = Consulta<PackedLabeledComboSQL>("spc_consultaDireccionesProvPorCompania", parameters);
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

		public List<ExisteCIUSQL> SearchExisteCIU() 
		{
			log.trace("SearchExisteCIU");
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_ciu", ciu, DbType.String);

				List<ExisteCIUSQL> response = Consulta<ExisteCIUSQL>("spc_consultaCIU", parameters);
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

		/// <summary>
		/// Metodo para consultar para obtener las cajas e info de la operación de un pallet / caja
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<InfoQRCodeSQL> SearchInfoQRCodeCIU()
		{
			log.trace("SearchInfoQRCodeCIU");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_ciuI", ciu, DbType.String);
				parameters.Add("_ciuF", ciuF, DbType.String);
				parameters.Add("_init", init, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				parameters.Add("_packagingId", packagingId, DbType.Int32);

				List<InfoQRCodeSQL> response = Consulta<InfoQRCodeSQL>("spc_consultaOperacionPallet", parameters);
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

		/// <summary>
		/// Metodo para consultar para obtener las cajas e info de la operación de un pallet / caja
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public saveOperationPalletResponse saveOperationPallet(string latmv, string longmv, int user_idmv)
		{
			log.trace("saveOperationPallet");
			try
			{
				saveOperationPalletResponse response = new saveOperationPalletResponse();

				DateTime startDate = DateTime.Now;

				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				opeId = pallets.First().opId;
				pallet = Convert.ToInt32(pallets.First().pallet);
				parameters.Add("_operacionId", opeId, DbType.Int32);
				parameters.Add("_pallet", pallet, DbType.Int32);
				parameters.Add("_init", 1, DbType.Int32);

				// Obtener el detalle del primer pallet
				List<InfoOperacionPalletsSQL> fullBoxes = Consulta<InfoOperacionPalletsSQL>("spc_consultaOperacionPalletCajas", parameters);
				CerrarConexion();

				// Datos operación
				int operationID = 0;
				int companiaId = fullBoxes.First().CompaniaId;
				int proveedorId = fullBoxes.First().ProveedorId;
				int operadorId = fullBoxes.First().OperadorId;
				int lineaId = fullBoxes.First().LineaId;
				int productoId = fullBoxes.First().ProductoId;
				int embalajeId = fullBoxes.First().EmbalajeId;
				string rango = "";
				int unidadesPorCaja = fullBoxes.First().unidadesPorCaja;
				int cajasPorPallet = fullBoxes.First().CajasPallet;
				int totalUnidades = 0;
				var mes = DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();
				var dia = DateTime.Now.Day.ToString().Length > 1 ? DateTime.Now.Day.ToString() : "0" + DateTime.Now.Day.ToString();
				var hora = DateTime.Now.Hour.ToString().Length > 1 ? DateTime.Now.Hour.ToString() : "0" + DateTime.Now.Hour.ToString();
				var minuto = DateTime.Now.Minute.ToString().Length > 1 ? DateTime.Now.Minute.ToString() : "0" + DateTime.Now.Minute.ToString();
				var segundo = DateTime.Now.Second.ToString().Length > 1 ? DateTime.Now.Second.ToString() : "0" + DateTime.Now.Second.ToString();
				string nombreOperacion = "Operacion-" + DateTime.Now.Year.ToString() + mes + dia + hora + minuto + segundo;
				string localId = DateTime.Now.Year.ToString() + mes + dia + hora + minuto + segundo;

				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				
				// Obtener el detalle del resto de pallets
				int init = 0;
				foreach (InfoQRCodeSQL item in pallets) {
					totalUnidades += item.quantity;

					if (init > 0) {
						parameters = new DynamicParameters();
						parameters.Add("_operacionId", item.opId, DbType.Int32);
						parameters.Add("_pallet", Convert.ToInt32(item.pallet), DbType.Int32);
						parameters.Add("_init", 2, DbType.Int32);

						List<InfoOperacionPalletsSQL> restBoxes = Consulta<InfoOperacionPalletsSQL>("spc_consultaOperacionPalletCajas", parameters);
						if(restBoxes.Count > 0) {
							foreach (InfoOperacionPalletsSQL box in restBoxes) {
								fullBoxes.Add(box);
							}
                        }
					}
					init++;
				}

				// Rangos de la nueva operación
				if(fullBoxes.Count > 0)
					rango = fullBoxes.First().RangoMin + "-" + fullBoxes.Last().RangoMax;

				List<InfoOperacionPalletScannedSQL> fullScanned = new List<InfoOperacionPalletScannedSQL>();
				// Obtener los escaneados por cada uno de los pallets
				foreach (InfoOperacionPalletsSQL item in fullBoxes) {
					parameters = new DynamicParameters();
					parameters.Add("_operacionId", item.DetalleId, DbType.Int32);
					parameters.Add("_pallet", 0, DbType.Int32);
					parameters.Add("_init", 3, DbType.Int32);

					List<InfoOperacionPalletScannedSQL> restScanned = Consulta<InfoOperacionPalletScannedSQL>("spc_consultaOperacionPalletCajas", parameters);
					if(restScanned.Count > 0) {
						foreach (InfoOperacionPalletScannedSQL scan in restScanned) {
							fullScanned.Add(scan);
						}
                    }
				}

				/*
				 * Guardar Operación
				 */
				parameters.Add("_startDate", startDate, DbType.DateTime);
				parameters.Add("_endDate", startDate, DbType.DateTime);
				parameters.Add("_idProveedor", proveedorId, DbType.Int32);// --
				parameters.Add("_idCompania", companiaId, DbType.Int32);// --
				parameters.Add("_idOperador", operadorId, DbType.Int32);// --  cosechero
				parameters.Add("_idLinea", lineaId, DbType.Int32); // - Sector
				parameters.Add("_idProducto", productoId, DbType.Int32); //
				parameters.Add("_idEmbalaje", embalajeId, DbType.Int32);
				parameters.Add("_agrupacion", nombreOperacion, DbType.String);
				parameters.Add("_totalUnidades", totalUnidades, DbType.Int32);
				parameters.Add("_rango", rango, DbType.String); // -- ciu split primer primer leido - split final final leido
				parameters.Add("_unidadesEscaneadas", fullScanned.Count, DbType.Int32);
				parameters.Add("_isGroup", 0, DbType.Boolean);
				parameters.Add("_localId", localId, DbType.String);
				parameters.Add("_unitsPerBox", unidadesPorCaja, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarOperacionPallet", parameters);
				operationID = parameters.Get<int>("_response");
				log.trace("Operation Pallets id: " + operationID);
				
				if (operationID <= 0) {
					throw new Exception("Hubo un error al generar la operación.");
				}

				/*
				 * Insertar el detalle de la operación (Pallets) con sus escaneados
				 */
				int npallet = 1;
				int nboxes = 1;
				string etiquetaDetalleId = "";
				foreach (var operationDetail in fullBoxes) {
					//if (nboxes > cajasPorPallet) {
					//	nboxes = 1;
					//	npallet++;
					//}
					operationDetail.Pallet = "PL-"+ npallet.ToString();
					DynamicParameters detailParams = new DynamicParameters();
					detailParams.Add("_idOperacion", operationID, System.Data.DbType.Int32);
					detailParams.Add("_pallet", "PL-" + npallet.ToString(), System.Data.DbType.String);
					detailParams.Add("_caja", "Bx-" + nboxes.ToString(), System.Data.DbType.String);
					detailParams.Add("_linea", operationDetail.Linea, System.Data.DbType.String);
					detailParams.Add("_merma", 0, System.Data.DbType.Int32);
					detailParams.Add("_etiquetaId", operationDetail.EtiquetaID, System.Data.DbType.String);
					detailParams.Add("_rangoMin", operationDetail.RangoMin, System.Data.DbType.String);
					detailParams.Add("_rangoMax", operationDetail.RangoMax, System.Data.DbType.String);
					detailParams.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);
					etiquetaDetalleId += etiquetaDetalleId == "" ? "X" + operationDetail.EtiquetaID : ",X" + operationDetail.EtiquetaID;

					detailParams = EjecutarSPOutPut("spi_detalleOperacion", detailParams);
					int operationdetailId = detailParams.Get<int>("_response");

					List<InfoOperacionPalletScannedSQL> tmpScanned = fullScanned.Where( x => x.OperacionDetalleId == operationDetail.DetalleId).ToList();

					foreach(InfoOperacionPalletScannedSQL scan in tmpScanned) {
                        string query = "INSERT INTO his_048_operacionDetalleEscaneados VALUES (NULL,'" + operationdetailId + "','" + scan.Orden + "','" + scan.Codigo + "');";
                        ConsultaCommand<string>(query);
                    }

					nboxes++;
				}
				log.trace("End save new Operation Pallet with scanneds: " + operationID);

				// Obtener los movimientos por cada operación del pallet
				DynamicParameters movParams = new DynamicParameters();
				List<InfoMovOperacionPalletSQL> deleteMovimientos = new List<InfoMovOperacionPalletSQL>();
				int npallets = 0;
				int npalletsMov = 0;
				List<InfoOperacionPalletsSQL> tmplist = fullBoxes.GroupBy(x => new { x.OperacionId, x.Pallet }).Select(grp => grp.FirstOrDefault()).ToList();
				foreach (var operationDetail in tmplist) {
					movParams = new DynamicParameters();
					movParams.Add("_operacionId", operationDetail.OperacionId, DbType.Int32);
					movParams.Add("_pallet", operationDetail.Pallet, DbType.String);
					List<InfoMovOperacionPalletSQL> movimientos = Consulta<InfoMovOperacionPalletSQL>("spc_consultaOperacionMovimientoPallet", movParams);

					if (movimientos.Count > 0)
						npalletsMov = movimientos.First().nPalletsOp;

					npallets = fullBoxes.Where(x => x.OperacionId == operationDetail.OperacionId).GroupBy(x => x.Pallet).ToList().Count();
					
					if(npallets == npalletsMov) {
						if(deleteMovimientos.Where(x => x.OperacionId == operationDetail.OperacionId).ToList().Count() == 0) {
							foreach (InfoMovOperacionPalletSQL item in movimientos) {
								deleteMovimientos.Add(item);
							}
                        }
					}
				}
				/******************************************
				 * Crear nuevo movimiento con su recepción
				 *****************************************/
				log.trace("Start save new Movimiento: " + operationID);
				string lat = latmv;
				string lon = longmv;
				int userId = user_idmv;
				string codigoQRStok = "{\"T\":\"A\",\"P\":" + totalUnidades.ToString() + ",\"I\":\"" + rango.Split('-')[0].Trim() + "\",\"F\":\"" + rango.Split('-')[1].Trim() + "\",\"ID\":\"Agrupacion\"}";

				//Registro de la operacion en un nuevo movimiento de envio de Stock
				DynamicParameters parametersStock = new DynamicParameters();
				parametersStock.Add("_Usuario", userId, System.Data.DbType.Int32);
				parametersStock.Add("_Operacion", operationID, System.Data.DbType.Int32);
				parametersStock.Add("_boxes", fullBoxes.Count, System.Data.DbType.Int32);
				parametersStock.Add("_totalProductosQR", totalUnidades, System.Data.DbType.Int32);
				parametersStock.Add("_CodigoQROperacion", codigoQRStok, System.Data.DbType.String); // -- Obtener último texto del código QR
				parametersStock.Add("_Latitud", lat, System.Data.DbType.String);
				parametersStock.Add("_Longitud", lon, System.Data.DbType.String);
				parametersStock.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

				parametersStock = EjecutarSPOutPut("spi_GuardarOperacionStock", parametersStock);
				int responseStock = parametersStock.Get<int>("_response");

				if (responseStock <= 0)
					throw new Exception("Error al ejecutar el sp");

				string queryStock = "INSERT INTO rel_021_productomovimiento(ProductoId, MovimientosId, Cantidad, NumIndPallet, NumIndCajas, DesconocidoId, ProductoUnico, AgruMovimientosId, Merma, ProductosRecibidos, CajaId) "
					+ " VALUES (NULL, " + responseStock.ToString() + ", " + totalUnidades.ToString() + ", 0, "+ fullBoxes.Count.ToString() + ", NULL, NULL, " + productoId.ToString()  + ", 0, 0, '" + etiquetaDetalleId + "');";
				ConsultaCommand<string>(queryStock);
				queryStock = "";
				nboxes = 1;
				log.trace("Executing DetailAgrupacionQRs");
				foreach (var operationDetail in fullBoxes) {
					queryStock = "INSERT INTO rel_022_productooperacion(operacionId, movimientoId, caja) "
						+ " VALUES(" + operationID.ToString() + ", " + responseStock.ToString() + ", IFNULL('" + operationDetail.EtiquetaID + "', ''));";
					ConsultaCommand<string>(queryStock);
					queryStock = "";

					queryStock = "INSERT INTO rel_023_movimientoAgrupacion (CodigoQR, MovimientoId, CodigoI, CodigoF, CajaId) "
						+ " VALUES ('{\"T\":\"C\",\"P\":"+ unidadesPorCaja.ToString() + ",\"I\":\"" + operationDetail.RangoMin + "\",\"F\":\"" + operationDetail.RangoMax + "\",\"ID\":\""+ nombreOperacion + "-"+operationDetail.Pallet+"-Bx-"+nboxes+"\"}',"
								  + "" + responseStock.ToString() + ", '"+ operationDetail.RangoMin + "', '" + operationDetail.RangoMax + "', CONCAT('X', '" + operationDetail.EtiquetaID + "'));";
					ConsultaCommand<string>(queryStock);
					queryStock = "";
					nboxes++;
				}

				parameters = new DynamicParameters();
				parameters.Add("_movimientosId", responseStock, System.Data.DbType.Int32);
				parameters.Add("_usuario", userId, System.Data.DbType.Int32);
				parameters.Add("_fecha", "", System.Data.DbType.String);
				parameters.Add("_noNormal", false, System.Data.DbType.Boolean);
				parameters.Add("_codigoI", "", System.Data.DbType.String);
				parameters.Add("_codigoF", "", System.Data.DbType.String);
				parameters.Add("_codigoTipo", "", System.Data.DbType.String);
				parameters.Add("_Latitud", lat, System.Data.DbType.String);
				parameters.Add("_Longitud", lon, System.Data.DbType.String);
				parameters.Add("_codigoCompleto", codigoQRStok, System.Data.DbType.String);
				parameters.Add("_nombre", "", System.Data.DbType.String);
				parameters.Add("_caja", "", System.Data.DbType.String);
				parameters.Add("_codigoIHEXA", "", System.Data.DbType.String);
				parameters.Add("_codigoFHEXA", "", System.Data.DbType.String);
				parameters.Add("_isHexa", false, System.Data.DbType.Boolean);
				parameters.Add("_tipoRecepcion", 1, System.Data.DbType.Int32);
				parameters.Add("_cosecheroGral", "", System.Data.DbType.String);
				parameters.Add("_sectorGral", "", System.Data.DbType.String);
				parameters.Add("_fechaProduccion", null, System.Data.DbType.DateTime);
				parameters.Add("_productosRecibidos", 0, System.Data.DbType.Int32);
				parameters.Add("_nCajasRecibidas", 0, System.Data.DbType.Int32);
				parameters.Add("_nPalletsRecibidos", 0, System.Data.DbType.Int32);
				parameters.Add("_nProductosMerma", 0, System.Data.DbType.Int32);
				parameters.Add("_productoMovimientoId", 0, System.Data.DbType.Int32);
				parameters.Add("_isAgro", 1, System.Data.DbType.Boolean);
				parameters.Add("_isUpName", false, System.Data.DbType.Boolean);
				parameters.Add("_isIDRecibo", 2, System.Data.DbType.Int32);
				parameters.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionEnvioARecepcion", parameters);
				
				log.trace("END save new Movimiento: " + operationID);
				/*****************************************/

				string idsDetail = "";

				// Ordenar los movimientos obtenidos, primero las recepciones
				deleteMovimientos = deleteMovimientos.OrderByDescending(x => x.TipoMovimientoId).ToList();
				// Eliminar los movimientos obsoletos
				DynamicParameters itemParams = new DynamicParameters();
				foreach (var movimiento in deleteMovimientos) {
					itemParams = new DynamicParameters();
					itemParams.Add("_movimientoId", movimiento.MovimientosId, DbType.Int32);
					itemParams.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
					itemParams = EjecutarSPOutPut("spd_eliminarOldMovimientos", itemParams);
					int respDel = itemParams.Get<int>("_response");
					if (respDel == 0)
						idsDetail += idsDetail == "" ? movimiento.MovimientosId.ToString() : "," + movimiento.MovimientosId.ToString();
				}
				if (idsDetail != "")
					log.trace("Could not delete old movimientos: " + idsDetail);
				
				idsDetail = "";
				int uescaned = 0;
				// Eliminar los pallets leidos
				itemParams = new DynamicParameters();
				init = 0;
				foreach (var operationDetail in fullBoxes) {
					uescaned = 0;
					itemParams = new DynamicParameters();
					itemParams.Add("_detalleId", operationDetail.DetalleId, DbType.Int32);
					itemParams.Add("_operacionId", operationDetail.OperacionId, DbType.Int32);
					itemParams.Add("_unidadesPorCaja", unidadesPorCaja, DbType.Int32);
					uescaned = fullScanned.Where(x => x.OperacionDetalleId == operationDetail.DetalleId).ToList().Count;
					itemParams.Add("_unidadesEscaneadas", uescaned, DbType.Int32);
					itemParams.Add("_newoperacionId", operationID, DbType.Int32);
					itemParams.Add("_init", init, DbType.Int32);
					itemParams.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
					itemParams = EjecutarSPOutPut("spd_eliminarOldPallets", itemParams);
					int respDel = itemParams.Get<int>("_response");
					init = 1;
					if (respDel == 0)
						idsDetail += idsDetail == "" ? operationDetail.DetalleId.ToString() : "," + operationDetail.DetalleId.ToString();
				}
				
				if(idsDetail != "")
					log.trace("Could not delete old palettes: " + idsDetail);

				CerrarConexion();

				response.success = true;
				response.messageEsp = operationID.ToString() + "," + npallet;

				return response;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}
		#endregion

		#endregion
	}
}
