using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using DWUtils;
using WSTraceIT.Models.Response;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Base.Production;
using NetCoreApiRest.Utils;
using WSTraceIT.Models.ModelsSQL;
using Google.Apis.YouTube.v3;

namespace WSTraceIT.InterfacesSQL
{
	public class ProductionSQL : DBHelperDapper
	{
		LoggerD4 log = new LoggerD4("ProductionSQL");

		public ProductionLineResponses saveOperationData(OperationDataRequest data, string lat, string lon) {
			log.debug("saveOperationData");
			ProductionLineResponses response = new ProductionLineResponses();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_startDate", data.startDate, System.Data.DbType.DateTime);
				parameters.Add("_endDate", data.endDate, System.Data.DbType.DateTime);
				parameters.Add("_idProveedor", data.idProvider, System.Data.DbType.Int32);// --
				parameters.Add("_idCompania", data.idCompany, System.Data.DbType.Int32);// --
				parameters.Add("_idOperador", data.idOperator, System.Data.DbType.Int32);// --  cosechero
				parameters.Add("_idLinea", data.idLine, System.Data.DbType.Int32); // - Sector
				parameters.Add("_idProducto", data.idProduct, System.Data.DbType.Int32); //
				parameters.Add("_idEmbalaje", data.idPackage, System.Data.DbType.Int32);
				parameters.Add("_agrupacion", data.grouping, System.Data.DbType.String);
				parameters.Add("_totalUnidades", data.totalUnits, System.Data.DbType.Int32);
				parameters.Add("_rango", data.range, System.Data.DbType.String); // -- ciu split primer primer leido - split final final leido
				parameters.Add("_unidadesEscaneadas", data.unitsScanned, System.Data.DbType.String);
				parameters.Add("_isGroup", 0, System.Data.DbType.Boolean);
				parameters.Add("_idOperacion", data.idOperation, System.Data.DbType.Int32);
				parameters.Add("_localId", data.localId, System.Data.DbType.String);
				parameters.Add("_unitsPerBox", data.unitsPerBox, System.Data.DbType.Int32);
				parameters.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

				string[] cius = data.scannedCIUs.Split('-');
				string ranges = "";
				bool isHex = false;
				bool globalHex = allHexa(cius);
				bool isNewCode = false;
				foreach (string ciu in cius)
                {
					isHex = isHexa(ciu);

                    if (isHex)
                    {
						
						long numbah = Convert.ToInt64(ciu, 16);
                        if (numbah > 2000000000000000)
                        {
							isNewCode = true;
                        }
						//Anidar a ranges los últimos 10 dígitos del long , los cuales pertenecen a un rango de ids de producto
						string numrango = numbah.ToString().Substring(numbah.ToString().Length - 10);
						numrango = numrango.TrimStart('0');
						ranges += $"{numrango}-";
					} else 
					{
						ranges += $"{ciu}-";
                    }
                }

				ranges = ranges.Remove(ranges.Length - 1);

				parameters.Add("_allCIUs", ranges, System.Data.DbType.String);// -- todos los cius leidos ciu1-ciu1-ciu3-ciu4...
				parameters.Add("_isHexa", globalHex == true ? 1 : 0, System.Data.DbType.Int32);
				parameters.Add("_isNewCode", isNewCode, System.Data.DbType.Boolean);
				log.trace("Executing spi_guardarOperacion");
				log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(parameters));

				parameters = EjecutarSPOutPut("spi_guardarOperacion", parameters);

				int operationID = parameters.Get<int>("_response");
				log.trace("opration id: " + operationID);
                if (operationID <= 0)
                {
					throw new Exception("Hubo un error al generar la operación.");
                }

				//CrearTransaccion();
				foreach (var rawMaterial in data.rawMaterials) {
                    if (rawMaterial.lot.Length != 0 && rawMaterial.lot != null && rawMaterial.lot != "")
                    {
						DynamicParameters rawMatParams = new DynamicParameters();
						rawMatParams.Add("_idOperacion", operationID, System.Data.DbType.Int32);
						rawMatParams.Add("_producto", rawMaterial.product, System.Data.DbType.String);
						rawMatParams.Add("_proveedor", rawMaterial.provider, System.Data.DbType.String);
						rawMatParams.Add("_lote", rawMaterial.lot, System.Data.DbType.String);
						log.trace("Executing spi_guardarOperacionMateriaPrima");
						EjecutarSPOutPut("spi_guardarOperacionMateriaPrima", rawMatParams);
					}
				}

				string etiquetaDetalleId = "";

				foreach (var operationDetail in data.operation)
				{
					string[] rangos = operationDetail.range.Split('-');

					DynamicParameters detailParams = new DynamicParameters();
					detailParams.Add("_idOperacion", operationID, System.Data.DbType.Int32);
					detailParams.Add("_pallet", operationDetail.pallet, System.Data.DbType.String);
					detailParams.Add("_caja", operationDetail.box, System.Data.DbType.String);
					detailParams.Add("_linea", operationDetail.line, System.Data.DbType.String);
					detailParams.Add("_merma", operationDetail.merma, System.Data.DbType.Int32);
					detailParams.Add("_etiquetaId", operationDetail.etiquetaID, System.Data.DbType.String);
					detailParams.Add("_rangoMin", rangos[0].Trim(), System.Data.DbType.String);
					detailParams.Add("_rangoMax", rangos[1].Trim(), System.Data.DbType.String);
					detailParams.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);
					etiquetaDetalleId += etiquetaDetalleId == "" ? "X" + operationDetail.etiquetaID : ",X" + operationDetail.etiquetaID;

					log.trace("Executing spi_detalleOperacion");
					parameters = EjecutarSPOutPut("spi_detalleOperacion", detailParams);
					int operationdetailId = parameters.Get<int>("_response");
					//log.trace("opration id: " + operationID);


					for (int i = 0; i < operationDetail.scanned.Count; i++) {
						string query = "INSERT INTO his_048_operacionDetalleEscaneados VALUES (NULL,'" + operationdetailId + "','" + i + "','" + operationDetail.scanned[i] + "');";
						ConsultaCommand<string>(query);
					}
				}

                //INICIO STOCK-------------------------------------------------------------------
                #region Generar Envío y Recepción en STOCK
                /*Desarrollador: Javier Ramirez, Iván Daniel Gutiérrez Rodríguez*/
                string latitud = "", longitud = "", codigoQRStok = "{\"T\":\"A\",\"P\":" + (data.unitsPerBox * data.operation.Count).ToString() + ",\"I\":\""+ data.range.Split('-')[0].Trim() + "\",\"F\":\""+ data.range.Split('-')[1].Trim() + "\",\"ID\":\"Agrupacion\"}";

				//Registro de la operacion en un nuevo movimiento de envio de Stock
				DynamicParameters parametersStock = new DynamicParameters();
				parametersStock.Add("_Usuario", data.idUser, System.Data.DbType.Int32);
				parametersStock.Add("_Operacion", operationID, System.Data.DbType.Int32);
				parametersStock.Add("_boxes", data.operation.Count, System.Data.DbType.Int32);
				parametersStock.Add("_totalProductosQR", (data.unitsPerBox * data.operation.Count), System.Data.DbType.Int32);
				parametersStock.Add("_CodigoQROperacion", codigoQRStok, System.Data.DbType.String); // -- Obtener último texto del código QR
				parametersStock.Add("_Latitud", lat, System.Data.DbType.String);
				parametersStock.Add("_Longitud", lon, System.Data.DbType.String);
				parametersStock.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

				parametersStock = EjecutarSPOutPut("spi_GuardarOperacionStock", parametersStock);
				int responseStock = parametersStock.Get<int>("_response");

				if (responseStock <= 0)
					throw new Exception("Error al ejecutar el sp");

				string queryStock = "INSERT INTO rel_021_productomovimiento(ProductoId, MovimientosId, Cantidad, NumIndPallet, NumIndCajas, DesconocidoId, ProductoUnico, AgruMovimientosId, Merma, ProductosRecibidos, CajaId) "
					+ " VALUES (NULL, " + responseStock.ToString() + ", " + (data.unitsPerBox * data.operation.Count).ToString() + ", 0, "+ data.operation.Count.ToString() + ", NULL, NULL, " + data.idProduct.ToString()  + ", 0, 0, '" + etiquetaDetalleId + "');";
				ConsultaCommand<string>(queryStock);
				queryStock = "";
				log.trace("Executing DetailAgrupacionQRs");
				foreach (var operationDetail in data.operation) {
					string[] rangos = operationDetail.range.Split('-');
					queryStock = "INSERT INTO rel_022_productooperacion(operacionId, movimientoId, caja) "
						+ " VALUES(" + operationID.ToString() + ", " + responseStock.ToString() + ", IFNULL('" + operationDetail.etiquetaID + "', ''));";
					ConsultaCommand<string>(queryStock);
					queryStock = "";

					queryStock = "INSERT INTO rel_023_movimientoAgrupacion (CodigoQR, MovimientoId, CodigoI, CodigoF, CajaId) "
						+ " VALUES ('{\"T\":\"C\",\"P\":"+ data.unitsPerBox.ToString() + ",\"I\":\"" + rangos[0].Trim() + "\",\"F\":\"" + rangos[1].Trim() + "\",\"ID\":\""+ data.grouping + "-"+operationDetail.pallet+"-"+operationDetail.box+"\"}',"
								  + "" + responseStock.ToString() + ", '"+ rangos[0].Trim() + "', '" + rangos[1].Trim() + "', CONCAT('X', '" + operationDetail.etiquetaID + "'));";
					ConsultaCommand<string>(queryStock);
					queryStock = "";
				}

				parameters = new DynamicParameters();
				parameters.Add("_movimientosId", responseStock, System.Data.DbType.Int32);
				parameters.Add("_usuario", data.idUser, System.Data.DbType.Int32);
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
				parameters.Add("_recepcionStock", false, System.Data.DbType.Boolean);
				parameters.Add("_acopioId", 0, System.Data.DbType.Int32);
				parameters.Add("_productorId", 0, System.Data.DbType.Int32);
				parameters.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionEnvioARecepcion", parameters);
                #endregion
				//FIN STOCK-------------------------------------------------------------------

				//por cada guardo de rollo
				foreach (var cambiorollo in data.cambiosRollo)
				{
					DynamicParameters cambioParameters = new DynamicParameters();
					cambioParameters.Add("_pallet", cambiorollo.pallet, System.Data.DbType.String);
					cambioParameters.Add("_caja", cambiorollo.caja, System.Data.DbType.String);
					cambioParameters.Add("_operacionId", operationID, System.Data.DbType.Int32);
					cambioParameters.Add("_ciuFin", cambiorollo.ultimo, System.Data.DbType.String);
					cambioParameters.Add("_ciuIn", cambiorollo.ultimo, System.Data.DbType.String);
					cambioParameters.Add("_rangoFin", cambiorollo.iUltimo, System.Data.DbType.Int32);
					cambioParameters.Add("_rangoIni", cambiorollo.iPrimero, System.Data.DbType.Int32);
					cambioParameters.Add("_embalaje", cambiorollo.iPackId, System.Data.DbType.Int32);
					EjecutarSP("spi_guardarCambioRollo", cambioParameters);
				}
				response.success = true;
				//TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				response.success = false;
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
			return response;
		}

		public List<OperationDetail> getDayOperation(int company, int provider) {
			List<OperationDetail> response = new List<OperationDetail>();
			log.trace("getDayOperation");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_fecha", DateTime.Now , System.Data.DbType.DateTime);
				parameters.Add("_companiaId", company, System.Data.DbType.Int32);
				parameters.Add("_proveedorId", provider, System.Data.DbType.Int32);

				response = Consulta<OperationDetail> ("spc_consultaOperacionesPorDia", parameters);

				CerrarConexion();
				return response;

			} catch (Exception ex) {
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		public WasteMaterial getScannedDetail(int idCompany, string code) {
			WasteMaterial response = new WasteMaterial();
			log.trace("getScannedDetail");
			try {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idCompania", idCompany, System.Data.DbType.Int32);
				parameters.Add("_codigo", code, System.Data.DbType.String);

				List<WasteMaterial> scannedData = Consulta<WasteMaterial>("spc_ConsultaProductoEscaneado", parameters);
				if (scannedData.Count > 0)
					response = scannedData[0];

				CerrarConexion();
			}
			catch(Exception ex){
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
			return response;
		}

		public bool saveWastageMaterial(int idCompany, List<WasteMaterial> materialData)
		{
			log.trace("getDayOperation");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				foreach (var material in materialData)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_agrupacion", material.grouping, System.Data.DbType.String);
					parameters.Add("_pallet", material.pallet, System.Data.DbType.String);
					parameters.Add("_caja", material.box, System.Data.DbType.String);
					parameters.Add("_codigo", material.ciu, System.Data.DbType.String);
					parameters.Add("_companiaId", idCompany, System.Data.DbType.Int32);
					parameters.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarOperacionMermas", parameters);
					int wasteId = parameters.Get<int>("_response");
					log.trace("Wastage material stored, id: " + wasteId);
				}
				CerrarConexion();
				//return response;
				return true;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		public int saveChangeRoll(SaveRollChangeRequest data)
		{
			log.trace("saveChangeRollSQL");
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_operacionId", data.operacionId, System.Data.DbType.Int32);
				parameters.Add("_pallet", data.pallet, System.Data.DbType.String);
				parameters.Add("_caja", data.caja, System.Data.DbType.String);
				parameters.Add("_ciufin", data.caja, System.Data.DbType.String);
				parameters.Add("_ciuin", data.caja, System.Data.DbType.String);
				parameters.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarCambioRollo", parameters);

				int operacionId = parameters.Get<int>("_response");
				log.trace("Operacion Change roll con id" + operacionId);

				CerrarConexion();
				return operacionId;
			}
            catch (Exception ex)
            {
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
            }
        }

		public OperationDatas getOperation(OperationRequest data, bool isHex, string pallet, string type)
		{
			log.trace("getOperation");
			OperationDatas response = new OperationDatas();
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_ciu",data.ciu, System.Data.DbType.String);
				parameters.Add("_isHex", isHex == true ? 1 : 0, System.Data.DbType.Int32);
				parameters.Add("_pallet", pallet.ToUpper(), System.Data.DbType.String);
				parameters.Add("_type", type, System.Data.DbType.String);

				List<Type> types = new List<Type>();
				types.Add(typeof(GetOperationDataSQL));
				types.Add(typeof(RawMaterialSQL));
				types.Add(typeof(OperationDetailSQL));

				var responseSQL = ConsultaMultiple("spc_consultaOperacion", types, parameters);

				log.trace("Operation response" + responseSQL);

                response.operationInfo = responseSQL[0].Cast<GetOperationDataSQL>().FirstOrDefault();
				response.rawMaterialsInfo = responseSQL[1].Cast<RawMaterialSQL>().ToList();
				response.operationDetailsInfo = responseSQL[2].Cast<OperationDetailSQL>().ToList();

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

		public void getLabelData(LabelDataRequest data)
		{
			log.trace("getLabelData");
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();

				CerrarConexion();
            }
            catch (Exception ex)
            {
				log.trace("Exception: " + ex.Message);
				CerrarConexion();
                throw ex;
            }
        }

		public int findOperationJob(string data)
		{
			log.trace("findOperationJobSQL");
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_localId", data, System.Data.DbType.String);
				parameters.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spc_consultaOperacionJob", parameters);

				int existsOp = parameters.Get<int>("_response");
				
				CerrarConexion();
				return existsOp;
			}
            catch (Exception ex)
            {
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
            }
        }

		public void getLabelDataCode(LabelDataCodeResponse response, string code, string type)
		{
			log.trace("getLabelDataCode");
            try
            {

				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_code", code, System.Data.DbType.String);

				List<Type> types = new List<Type>();
				types.Add(typeof(OperationDatase));
				types.Add(typeof(OperationDetailse));
				types.Add(typeof(DetailsOrderse));

				var res = ConsultaMultiple("spc_consultaOperacionesCode", types, parameters);

				CerrarConexion();

				if (res.Count <= 2)
				{
					response.messageEsp = "Error al buscar la información.";
					return;
				}

				OperationDatase operationDatase = res[0].Cast<OperationDatase>().First();
				List<OperationDetailse> operationDetailse = res[1].Cast<OperationDetailse>().ToList();
				List<DetailsOrderse> DetailsOrderse = res[2].Cast<DetailsOrderse>().ToList();

				var operationScanned = 0;
				string lecturas = "";


				response.company = operationDatase.Company;
				response.line = operationDatase.Linea;
				//response.ranges = operationDatase.Range;
				response.grouping = operationDatase.Grouping;
				response.product = operationDatase.Product;
				response.Operator = operationDatase.Operator;
				response.unitBox = operationDatase.unitsBox;
				response.startDate = operationDatase.fechaRegistro;
				response.endDate = operationDatase.endDate;

				string iniIni = "";
				string finIni = "";
				string iniFin = "";
				string finFin = "";

				switch (type)
                {
					case "Pallet":
						string palletOrigen = "";

						//Obtener el pallet en el que se encuentra el código de la etiqueta de la caja
						palletOrigen = operationDetailse.Find(x => x.EtiquetaID == code).Pallet;
						//obtener todos las cajas y armar el pallet.
						List<OperationDetailse> tempOperaciones = operationDetailse.FindAll(x => x.Pallet == palletOrigen).ToList();

                        foreach (var operacion in tempOperaciones)
                        {
							response.ranges += $"{operacion.RangoMin} - {operacion.RangoMax};";
                        }

						response.firstScanned = tempOperaciones[0].RangoMin;
						response.lastScanned = tempOperaciones[tempOperaciones.Count - 1].RangoMax;
						response.unitBox = operationDatase.unitsBox;
						response.pallet = tempOperaciones[0].Pallet;
						response.box = tempOperaciones[tempOperaciones.Count - 1].Caja;

						iniIni = tempOperaciones[0].RangoMin; // primer lectura de la primer caja
						finIni = tempOperaciones[0].RangoMax; // segunda/ultima l ectura de la primer caja
						iniFin = tempOperaciones.Last().RangoMin; // primer lectura de la última caja
						finFin = tempOperaciones.Last().RangoMax; // ultima lectura de la última caja

						//if (getCIUid(iniIni) > getCIUid(finIni))
						//{
						//	response.firstScanned = finIni;
						//} else {
						//	response.firstScanned = iniIni;
						//}

						//if (getCIUid(iniFin) > getCIUid(finFin))
						//{
						//	response.lastScanned = finFin;
						//}
						//else
						//{
						//	response.lastScanned = iniFin;
						//}

                        if (getCIUid(finFin) < getCIUid(iniIni)) // El último ciu leido es menor al primer leido de caja, indicando que fue una lectura negativa (mayor a menor) de algun modo.
                        {
							response.firstScanned = finIni;
							response.lastScanned = iniFin;
                        } 
						else 
						{
							// si el último ciu es mayor al ciu inicial, es decir que fue una lectura positiva (de menor a mayor) de algun modo.
							response.firstScanned = iniIni;
							response.lastScanned = finFin;
                        }

                        if (getCIUid(response.firstScanned) > getCIUid(response.lastScanned))
                        {
							string first = response.firstScanned;
							response.firstScanned = response.lastScanned;
							response.lastScanned = first;
                        }

						break;
					case "Caja":

						OperationDetailse caja = operationDetailse.Find(x => x.EtiquetaID == code);

                        if (caja.EtiquetaID != "")
                        {
							response.ranges = $"{caja.RangoMin} - {caja.RangoMax}";

							response.firstScanned = caja.RangoMin;
							response.lastScanned = caja.RangoMax;
							response.pallet = caja.Pallet;
							response.box = caja.Caja;

						}
                        else
                        {
							throw new Exception("No se encontró el registro de la caja con el código brindado.");
                        }

					break;
                    default:
                        break;
                }

            }
            catch (Exception es)
            {
				response.messageEsp = es.Message;
                throw es;
            }
        }

		public int getWasteByCompany(int companyId, int idUser)
		{
			log.trace("getWasteByCompany");
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, System.Data.DbType.Int32);
				parameters.Add("_packedId", idUser, System.Data.DbType.Int32);
				parameters.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spc_consultaMermaPorCompania", parameters);

				int merma = parameters.Get<int>("_response");
				
				CerrarConexion();
				return merma;
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
					ulong.Parse(number, System.Globalization.NumberStyles.HexNumber);
					return true;

                } else {
					return false;
                }
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool allHexa(string[] cius)
		{
			List<bool> hexas = new List<bool>();
            foreach (string ciu in cius)
            {
				hexas.Add(isHexa(ciu));
            }

			// al primer registro de no hexadecimal que encuentre significa que algun ciu leido no es hexa
            foreach (bool ishex in hexas)
            {
                if (ishex == false)
                {
					return false;
                }
            }
			return true;
        }

		public static int getCIUid(string QR)
		{
			try
			{
				string data = ulong.Parse(QR, System.Globalization.NumberStyles.HexNumber).ToString();
				return (int.Parse(data.Substring(6)));
			}
			catch
			{
				return (-1);
			}
		}
		public static int getCIUpid(string QR)
		{
			try
			{
				string data = ulong.Parse(QR, System.Globalization.NumberStyles.HexNumber).ToString();
				return (int.Parse(data.Substring(1, 5)));
			}
			catch
			{
				return (-1);
			}
		}
	}
}
