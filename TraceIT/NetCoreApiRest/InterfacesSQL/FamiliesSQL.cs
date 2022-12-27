using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Families;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using WSTraceIT.Models.Base.Product;

namespace WSTraceIT.InterfacesSQL
{
	public class FamiliesSQL : DBHelperDapper
	{
		#region Properties
		//WS Mobile
		public int userMobileId;

		//WS BackOffice
		public int option;

		//Datos de la familia
		public int familyId;
		public string name;
		public string model;
		public string description;
		public string descriptionEnglish;
		public string imageFamily;
		public decimal qualification;
		public string sku;
		public string barCode;
		public bool status;
		public bool warranty;
		public bool expiration;
		public bool addTicket;
		public int category;
		public int company;
		public int lifeDays;
		public bool autoLote;
		public bool editLote;
		public int consecutiveLote;
		public string prefix;
		public float latitude;
		public float longitude;
		public int userCompanyId;
		public string colorFamilia;
		public int paisId;

		//Datos de la especificación tecnica
		public List<DirectionFamilyData> directionFamilies;
		public List<TechnicalSpecificationData> technicalSpecifications;
		public List<TechnicalSpecificationDetailsData> technicalSpecificationDetails;
		public List<int> deleteTechnicalSpecification;

		//Datos de los links (guias de uso, guias de instalacion y productos relacionados)
		public int sectionType;
		public List<linkData> links;

		//Datos de garantias
		public List<WarrantyData> warranties;

		//Preguntas frecuentes
		public List<FrequentQuestions> frequentQuestions;

		//Datos de embalajes
		public List<PackagingFamily> packagingFamily;

		private LoggerD4 log = new LoggerD4("FamiliesSQL");
		private bool _isHex = false;

		public bool isgtin;
		public bool isciu;
		public bool ixhex;
		#endregion

		#region Constructor
		public FamiliesSQL()
		{
			this.userMobileId = 0;

			this.option = 0;

			this.familyId = 0;
			this.name = String.Empty;
			this.model = String.Empty;
			this.description = String.Empty;
			this.descriptionEnglish = String.Empty;
			this.imageFamily = String.Empty;
			this.qualification = 0;
			this.sku = String.Empty;
			this.barCode = String.Empty;
			this.status = false;
			this.warranty = false;
			this.expiration = false;
			this.addTicket = false;
			this.category = 0;
			this.company = 0;
			this.lifeDays = 0;
			this.autoLote = false;
			this.editLote = false;
			this.consecutiveLote = 0;
			this.prefix = String.Empty;
			this.latitude = 0;
			this.longitude = 0;

			this.directionFamilies = new List<DirectionFamilyData>();
			this.technicalSpecifications = new List<TechnicalSpecificationData>();
			this.technicalSpecificationDetails = new List<TechnicalSpecificationDetailsData>();
			this.deleteTechnicalSpecification = new List<int>();
			this.links = new List<linkData>();
			this.frequentQuestions = new List<FrequentQuestions>();
			this.packagingFamily = new List<PackagingFamily>();

			this.userCompanyId = -1;

			this.warranties = new List<WarrantyData>();
			this.isgtin = false;
			this.isciu = false;
			this._isHex = false;

			this.paisId = 0;
		}
		#endregion

		#region Public methods

		#region Mobile applications


		/// <summary>
		/// Metodo para consultar la información de una familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public StatusProd UneFamile()
		{
			log.debug("UneFamile est invoque");
			StatusProd response = new StatusProd();

			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				log.debug("indicateur: " + barCode);
				//var GTIN = //traer GTIN == qr si no GTIN null;
				//validacion si gtin null , gtin = barcode

				bool izHex = false;
				izHex = isHexa(barCode);
				List<string> results = new List<string>();
				string GTIN = "";
				if (izHex)
				{
					string range = "";
					long numbah = Convert.ToInt64(barCode, 16);
					//Anidar a ranges los últimos 10 dígitos del long , los cuales pertenecen a un rango de ids de producto
					string numrango = numbah.ToString().Substring(numbah.ToString().Length - 10);
					numrango = numrango.TrimStart('0');

					range = numrango.ToString();
					//Obtener el familaiproducto id
					int fpId = ConsultaCommand<int>($"SELECT familiaproductoId FROM cat_048_producto WHERE {range} BETWEEN Inicio AND Fin;").FirstOrDefault();
					//consultar el gtin directamente del id de la familia producto
					results = ConsultaCommand<string>($"SELECT GTIN FROM cat_001_familiaproducto WHERE familiaProductoId = {fpId};");

					// después de que se encontró en la tabla nueva agrupados, generar su registro en la tabla anterior individual para los procesos siguientes
					// y así no hacer cambios al ciu leido y pasarlo a origen con el hexa

					if (results.Count > 0)
					{
						GTIN = results[0];
						_isHex = izHex; // o sea true.

						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("_rango", range, DbType.String);
						parameters.Add("_hexa", barCode, DbType.String);
						parameters.Add("_response", DbType.Int32, direction: ParameterDirection.InputOutput);

						parameters = EjecutarSPOutPut("spi_generarProducto", parameters);

						ixhex = true;

						if (parameters.Get<int>("_response") == 0)
						{
							throw new Exception("Hubo un error al generar el registro individual del CIU");
						}

					}
					else
					{
						throw new Exception("El CIU no se encuentra registrado en el sistema");
					}
				}
				else
				{

					//buscar primero el gtin
					results = ConsultaCommand<string>("SELECT fp.GTIN " +
													"FROM cat_001_familiaproducto fp " +
													"WHERE fp.GTIN = '" + barCode + "' GROUP BY fp.GTIN  ; ");
					this.isgtin = true;

					//si no encuentra gtin buscar por qr
					if (results.Count == 0)
					{
						this.isgtin = false;
						//Obtener el familia id del producto
						int fpId = ConsultaCommand<int>($"SELECT familiaProductoId FROM cat_004_producto WHERE CodigoQR = '{barCode}';").FirstOrDefault();
						//buscar el gtin de la familia
						results = ConsultaCommand<string>($"SELECT GTIN FROM cat_001_familiaproducto WHERE familiaProductoId = '{fpId}';");
						this.isciu = true;
					}

					if (results.Count > 0)
					{
						GTIN = results[0];
						this._isHex = izHex; // o sea false.
					}
					else
					{
						throw new Exception("El CIU no se encuentra registrado en el sistema");
					}

				}

				if (GTIN == null || GTIN.Length == 0)
				{
					GTIN = barCode;
				}

				var idproduit = "0";
				var idproduitR = ConsultaCommand<string>("select FamiliaProductoId from cat_001_familiaproducto where GTIN = '" + GTIN + "' ;");
				if (idproduitR.Count > 0)
				{
					idproduit = idproduitR[0];
				}
				var status_famile = "100";
				var status_famileR = ConsultaCommand<string>("select Status from cat_001_familiaproducto where GTIN = '" + GTIN + "' ;");
				if (status_famileR.Count > 0)
				{
					status_famile = status_famileR[0];
				}

				var company_id = "0";
				var company_idR = ConsultaCommand<string>("select CompaniaId from cat_001_familiaproducto where GTIN = '" + GTIN + "' ;");
				if (company_idR.Count > 0)
				{
					company_id = company_idR[0];
				}

				var status_company = "100";
				var status_companyR = ConsultaCommand<string>("select Status from cat_003_compania where CompaniaId = '" + company_id + "' ;");
				if (status_companyR.Count > 0)
				{
					status_company = status_companyR[0];
				}
				//var idproduit = ConsultaCommand<string>("select FamiliaProductoId from cat_004_producto where GTIN = '" + barCode + "' limit 1;")[0];
				//var famileid = ConsultaCommand<string>("select FamiliaProductoId from cat_004_producto where ProductoId = " + idproduit + ";");
				//var st_famile = ConsultaCommand<string>("select Status from cat_001_familiaproducto where FamiliaProductoId = " + famileid[0] + ";")[0];
				//var idcompa = ConsultaCommand<string>("select CompaniaId from cat_001_familiaproducto where FamiliaProductoId = " + famileid[0] + ";")[0];
				//var st_compagnie = ConsultaCommand<string>("select Status from cat_003_compania where CompaniaId = " + idcompa + ";")[0];
				response.Idprod = Int32.Parse(idproduit);
				response.st_company = Int32.Parse(status_company);
				response.st_familia = Int32.Parse(status_famile);
				//var st_produit = ConsultaCommand<string>("select estatus from cat_004_producto where ProductoId = " + idproduit + ";")[0];
				//response.st_product = Int32.Parse(st_produit + "");

				//TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				//log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}

			return response;
		}



		/// <summary>
		/// Metodo para consultar la información de la familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public InformationFamily SearchFamilyMobile()
		{
			log.debug("SearchFamilyMobile was invoked");
			InformationFamily response = new InformationFamily();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				log.debug("Code scanned: " + barCode);

				string range = "";
				bool izHex = false;
				izHex = isHexa(barCode);
				List<string> results = new List<string>();
				string GTIN = "";
				if (izHex)
				{

					long numbah = Convert.ToInt64(barCode, 16);
					//Anidar a ranges los últimos 10 dígitos del long , los cuales pertenecen a un rango de ids de producto
					string numrango = numbah.ToString().Substring(numbah.ToString().Length - 10);
					numrango = numrango.TrimStart('0');

					range = numrango.ToString();
					//Obtener el familaiproducto id
					int fpId = ConsultaCommand<int>($"SELECT familiaproductoId FROM cat_048_producto WHERE {range} BETWEEN Inicio AND Fin;")[0];
					//consultar el gtin directamente del id de la familia producto
					results = ConsultaCommand<string>($"SELECT GTIN FROM cat_001_familiaproducto WHERE familiaProductoId = {fpId};");

					// después de que se encontró en la tabla nueva agrupados, generar su registro en la tabla anterior individual para los procesos siguientes
					// y así no hacer cambios al ciu leido y pasarlo a origen con el hexa

					if (results.Count > 0)
					{
						GTIN = results[0];
						izHex = true;
					}
					else
					{
						throw new Exception("El CIU no se encuentra registrado en el sistema");
					}
				}
				else
				{
					//buscar primero el gtin
					results = ConsultaCommand<string>("SELECT fp.GTIN " +
													"FROM cat_001_familiaproducto fp " +
													"WHERE fp.GTIN = '" + barCode + "' GROUP BY fp.GTIN  ; ");
					this.isgtin = true;

					//si no encuentra gtin buscar por qr
					if (results.Count == 0)
					{
						this.isgtin = false;
						//Obtener el familia id del producto
						int fpId = ConsultaCommand<int>($"SELECT familiaProductoId FROM cat_004_producto WHERE CodigoQR = '{barCode}';").FirstOrDefault();
						//buscar el gtin de la familia
						results = ConsultaCommand<string>($"SELECT GTIN FROM cat_001_familiaproducto WHERE familiaProductoId = '{fpId}';");
						this.isciu = true;
					}

					if (results.Count > 0)
					{
						GTIN = results[0];
						this._isHex = izHex; // o sea false.
					}
					else
					{
						throw new Exception("El CIU no se encuentra registrado en el sistema");
					}
				}

				DynamicParameters parametersOrigin = new DynamicParameters();
				DynamicParameters parameters = new DynamicParameters();

				productoId productoId = new productoId();
				familiaProducto familiaProducto = new familiaProducto();
				List<string> origenProductoId = new List<string>();
				List<string> operacionId = new List<string>();

				if (izHex)
				{
					long numbah = Convert.ToInt64(barCode, 16);
					//Anidar a ranges los últimos 10 dígitos del long , los cuales pertenecen a un rango de ids de producto
					long numrango = Int64.Parse(numbah.ToString().Substring(numbah.ToString().Length - 10));
					range = numrango.ToString();

					productoId = ConsultaCommand<productoId>($"SELECT ProductoId, UDID, FechaCaducidad, DireccionId, FamiliaProductoId From cat_004_producto WHERE CodigoQR = '{barCode}';").FirstOrDefault();
					familiaProducto = ConsultaCommand<familiaProducto>($"SELECT FamiliaProductoId, ImagenUrl, Nombre, Modelo, GTIN FROM cat_001_familiaproducto WHERE FamiliaProductoId = {productoId.FamiliaProductoId}").FirstOrDefault();
					origenProductoId = ConsultaCommand<string>($"SELECT * FROM tra_003_OrigenProducto WHERE ProductoId = {productoId.ProductoId};");

					parametersOrigin.Add("_ciu", range, DbType.String);
					parametersOrigin.Add("_isHex", izHex == true ? 1 : 0, DbType.Int16);
					parametersOrigin.Add("_direccionId", productoId.DireccionId, DbType.String);
					parametersOrigin.Add("_familiaProducto", familiaProducto.FamiliaProductoId, DbType.Int32);
					parametersOrigin.Add("_type", 0, DbType.Int16);
				}
				else
				{

					//obtener los ids que se utiliza en los NO provenientes de producto


					if (isgtin)
					{
						//si es un gtin no consulta otra cosa mas que la información de la familia, y de origen nel.
						parametersOrigin.Add("_familiaProducto", 0, DbType.Int16);
						parametersOrigin.Add("_direccionId", 0, DbType.String);
						parametersOrigin.Add("_type", 2, DbType.Int16);
					}
					else if (isciu)
					{

						productoId = ConsultaCommand<productoId>($"SELECT ProductoId, UDID, FechaCaducidad, DireccionId, FamiliaProductoId From cat_004_producto WHERE CodigoQR = '{barCode}';").FirstOrDefault();
						familiaProducto = ConsultaCommand<familiaProducto>($"SELECT FamiliaProductoId, Nombre, ImagenUrl, Modelo, GTIN FROM cat_001_familiaproducto WHERE FamiliaProductoId = {productoId.FamiliaProductoId}").FirstOrDefault();
						parametersOrigin.Add("_familiaProducto", familiaProducto.FamiliaProductoId, DbType.Int32);
						parametersOrigin.Add("_direccionId", productoId.DireccionId, DbType.String);
						parametersOrigin.Add("_type", 1, DbType.Int16);
					}

					parametersOrigin.Add("_ciu", barCode, DbType.String);
					parametersOrigin.Add("_isHex", izHex == true ? 1 : 0, DbType.Int16);
				}

				if (isgtin)
				{
					parameters.Add("_type", 0, DbType.Int16);
					parameters.Add("_familiaId", 0, DbType.Int32);
					parameters.Add("_productoId", 0, DbType.Int32);
				}
				else
				{
					parameters.Add("_type", 1, DbType.Int16);
					parameters.Add("_familiaId", familiaProducto.FamiliaProductoId, DbType.Int32);
					parameters.Add("_productoId", productoId.ProductoId, DbType.Int32);
				}

				parameters.Add("_code", barCode, DbType.String);
				parameters.Add("_userMobileId", userMobileId, DbType.Int32);
				parameters.Add("_latitude", latitude, DbType.Double);
				parameters.Add("_longitude", longitude, DbType.Double);


				List<Type> types = new List<Type>();
				types.Add(typeof(FamilySQL));
				types.Add(typeof(TechnicalSpecificationGroupSQL));
				types.Add(typeof(TechnicalSpecificationSQL));
				types.Add(typeof(LinkSQL));
				types.Add(typeof(LinkSQL));
				types.Add(typeof(RelatedProductSQL));
				types.Add(typeof(ServiceCenterSQL));
				types.Add(typeof(FAQsSQL));
				types.Add(typeof(WarrantiesSQL));
				types.Add(typeof(SocialMediaLinkSQL));
				//types.Add(typeof(OriginSQL));
				//types.Add(typeof(string));

				var responseSQL = ConsultaMultiple("spc_consultaInformacionFamilia", types, parameters);

				response.FamilyInformation = responseSQL[0].Cast<FamilySQL>().FirstOrDefault();
				response.FamilyInformation.IdProduct = productoId.ProductoId;
				response.FamilyInformation.UDID = productoId.UDID;

				response.TechnicalSpecification = responseSQL[1].Cast<TechnicalSpecificationGroupSQL>().ToList();
				response.TechnicalSpecificationDetails = responseSQL[2].Cast<TechnicalSpecificationSQL>().ToList();
				response.AlternateNormalUsesList = responseSQL[3].Cast<LinkSQL>().ToList(); //tipo de vinculo
				response.InstallationGuideList = responseSQL[4].Cast<LinkSQL>().ToList();
				response.RelatedProductList = responseSQL[5].Cast<RelatedProductSQL>().ToList();
				response.ServiceCenters = responseSQL[6].Cast<ServiceCenterSQL>().ToList();
				response.FAQsList = responseSQL[7].Cast<FAQsSQL>().ToList();
				response.WarrantiesList = responseSQL[8].Cast<WarrantiesSQL>().ToList();
				response.SocialMediaLinks = responseSQL[9].Cast<SocialMediaLinkSQL>().ToList();
				//response.originList = responseSQL[10].Cast<OriginSQL>().ToList();
				//response.username= responseSQL[11].Cast<String>().ToList()[0];



				//Obtener por separado el id y datos




				List<OriginSQL> respOrigen = Consulta<OriginSQL>("spc_ConsultaOrigenProducto", parametersOrigin);

				if (izHex || isciu)
				{
					respOrigen[0].Ciu = barCode;
					respOrigen[0].Caducidad = productoId.FechaCaducidad;
					respOrigen[0].NumSerie = productoId.UDID;
					respOrigen[0].Lote = productoId.UDID;

					respOrigen[0].Nombre = familiaProducto.Nombre;
					respOrigen[0].Presentacion = familiaProducto.Modelo;
					respOrigen[0].GTIN = familiaProducto.GTIN;

				}

				response.originList = respOrigen;
				CerrarConexion();
                
				try {
					// Obtener ultimo movimiento registrado
					int movimientoId = searchMovementId(barCode);

					Models.Response.SearchDocDetalleProductosResponse infoProdMovimiento = new Models.Response.SearchDocDetalleProductosResponse();

					MovimientosSQL sql = new MovimientosSQL();
					sql.movimientoId = movimientoId;

					infoProdMovimiento.docDetalleProductoList = sql.SearchDocDetalleProducto();

					string fechaCaducidad = infoProdMovimiento.docDetalleProductoList.Where(x => x.producto == familiaProducto.Nombre).Select(x => x.fechaCaducidad).FirstOrDefault();

					response.originList[0].Caducidad = String.IsNullOrEmpty(fechaCaducidad) ? "" : fechaCaducidad;
				}
				catch (Exception ex) {
					response.originList[0].Caducidad = "";
				}
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
			return response;
		}

		public int searchMovementId (string ciu)
		{
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				// buscar en el rango de Iniciales (caso de rango positivo)

				int envio = 0;
				int movementID = 0;

				int rango = 0;
				//OBtener el rango del ciu
				rango = InterfacesSQL.TrackingSQL.getCIUid(ciu);
				// buscar el ciu de la caja a la que pertenece el ciu , por probabilidad de absolutos
				string ciuT = ciuT = ConsultaCommand<string>($"(SELECT od.RangoMin FROM his_048_operacionDetalleEscaneados ode INNER JOIN his_047_operacionDetalle od ON od.DetalleId = ode.OperacionDetalleId WHERE ((select RIGHT(CONV(Codigo,16,10),10)) - {rango}) <= 0 ORDER BY ({rango} - (select RIGHT(CONV(Codigo,16,10),10))) LIMIT 1);").FirstOrDefault();

				movementID = ConsultaCommand<int>($"SELECT IFNULL(MovimientosId,0) FROM rel_021_productomovimiento WHERE CajaId LIKE '%{ciuT}%' ORDER BY MovimientosId DESC LIMIT 1;").FirstOrDefault();


				// si no se encontró en el rango de positivos, buscarlo en el de finales (caso de rango negativo)
				if (movementID == 0) {
					movementID = ConsultaCommand<int>($"SELECT IFNULL(MovimientoId,0) FROM rel_023_movimientoAgrupacion WHERE CajaId LIKE '%{ciu}%' ORDER BY MovimientoId DESC LIMIT 1;").FirstOrDefault();
				}

				// buscar si es envio/recep
                if (movementID != 0) {
					envio = ConsultaCommand<int>($"SELECT IFNULL(MovimientoParcialId, MovimientosId) FROM cat_027_movimientos WHERE MovimientosId = {movementID};").FirstOrDefault();

					if (envio != 0) {
						movementID = envio;
					}
				}
				
				CerrarConexion();

				return movementID;
			}
            catch (Exception ex) {
				return 0;
            }
        }

        public List<alertas> buscarAlertas(string codigo, int familyId)
        {
            List<alertas> response = new List<alertas>();
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                log.debug("Code scanned: " + barCode);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_familiaId", familyId, DbType.Int32);
                parameters.Add("_movimientoId", 0, DbType.Int32);
                parameters.Add("_codigo", codigo, DbType.String);
                parameters.Add("_tipo", 2, DbType.Int32);
                parameters.Add("_isHexa", isHexa(codigo), DbType.Boolean);

                response = Consulta<alertas>("spc_ConsultaAlertasTrackit", parameters);

                CerrarConexion();
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al buscar las alertas: {ex.Message}");
            }

            return response;
        }

		#endregion

		#region BackOffice
		/// <summary>
		/// Metodo para guardar todos los datos provenientes de familias
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveFamily()
		{
			log.trace("SaveFamily");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_model", model, DbType.String);
				parameters.Add("_image", imageFamily, DbType.String);
				parameters.Add("_sku", sku, DbType.String);
				parameters.Add("_gtin", barCode, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_warranty", warranty, DbType.Boolean);
				parameters.Add("_expiration", expiration, DbType.Boolean);
				parameters.Add("_addTicket", addTicket, DbType.Boolean);
				parameters.Add("_category", category, DbType.Int32);
				parameters.Add("_company", company, DbType.Int32);
				parameters.Add("_lifeDays", lifeDays, DbType.Int32);
				parameters.Add("_autoLote", autoLote, DbType.Boolean);
				parameters.Add("_editLote", editLote, DbType.Boolean);
				parameters.Add("_consecutiveLote", consecutiveLote, DbType.Int32);
				parameters.Add("_prefix", prefix, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
				parameters.Add("_colorFamilia", colorFamilia, DbType.String);

				parameters = EjecutarSPOutPut("spi_guardarFamiliaProducto", parameters);

				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP de agregar familia general");
				else if (response == -1)
					throw new Exception("Ya existe una familia con el mismo codigo de barras");

				foreach (var dir in directionFamilies)
				{
					parameters = new DynamicParameters();
					parameters.Add("_idDirection", dir.directionId, DbType.Int32);
					parameters.Add("_idFamily", response, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarDireccionesFamilias", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP de agregar direcciones");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para guardar las nuevas especificaciones tecnicas al estar en el modo edicion
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveTechnicalSpecification()
		{
			log.trace("SaveTechnicalSpecification");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				//Especificación técnica
				foreach (var et in technicalSpecifications)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_especificationTecnicalId", 0, DbType.Int32);
					parameters.Add("_title", et.title, DbType.String);
					parameters.Add("_titleEnglish", et.titleEnglish, DbType.String);
					parameters.Add("_familyProductId", familyId, DbType.Int32);
					parameters.Add("_subtitle", "", DbType.String);
					parameters.Add("_description", "", DbType.String);
					parameters.Add("_subtitleEnglish", "", DbType.String);
					parameters.Add("_descriptionEnglish", "", DbType.String);
					parameters.Add("_image", "", DbType.String);
					parameters.Add("_option", option, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarEspecificacionTecnica", parameters);

					int reponse = parameters.Get<int>("_response");

					if (reponse == 0)
						throw new Exception("Error al ejecutar el SP");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para guardar las nuevas especificaciones de detalle al estar en el modo edicion
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveTechnicalSpecificationDetails()
		{
			log.trace("SaveTechnicalSpecificationDetails");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				//Especificación tecnica detalle
				foreach (var etd in technicalSpecificationDetails)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_especificationTecnicalId", familyId, DbType.Int32);
					parameters.Add("_title", "", DbType.String);
					parameters.Add("_titleEnglish", "", DbType.String);
					parameters.Add("_familyProductId", 0, DbType.Int32);
					parameters.Add("_subtitle", etd.subtitle, DbType.String);
					parameters.Add("_description", etd.description, DbType.String);
					parameters.Add("_subtitleEnglish", etd.subtitleEnglish, DbType.String);
					parameters.Add("_descriptionEnglish", etd.descriptionEnglish, DbType.String);
					parameters.Add("_image", etd.imageBase, DbType.String);
					parameters.Add("_option", option, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarEspecificacionTecnica", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para guardar las nuevas garantias al estar en el modo edición
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveWarranty()
		{
			log.trace("SaveWarranty");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				foreach (var war in warranties)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_country", war.country, DbType.String);
					parameters.Add("_pdfUrl", war.pdfBase, DbType.String);
					parameters.Add("_periodMonth", war.periodMonth, DbType.Int32);
					parameters.Add("_familyProductId", familyId, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarGarantiasFamilias", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para guardar las nuevas preguntas frecuentes al estar en el modo edición
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveFrequentQuestions()
		{
			log.trace("SaveFrequentQuestions");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				//Preguntas frecuentes
				foreach (var faq in frequentQuestions)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_questionSpanish", faq.question, DbType.String);
					parameters.Add("_responseSpanish", faq.response, DbType.String);
					parameters.Add("_questionEnglish", faq.questionEnglish, DbType.String);
					parameters.Add("_responseEnglish", faq.responseEnglish, DbType.String);
					parameters.Add("_familyProductId", familyId, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarPreguntasFrecuentes", parameters);


					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP de preguntas frecuentes de familias");
					else if (parameters.Get<int>("_response") == -1)
						throw new Exception("overLimit");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para guardar el link de preguntas frecuentes al estar en el modo edición
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveLink()
		{
			log.trace("SaveLink");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				foreach (var link in links)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_title", link.title, DbType.String);
					parameters.Add("_url", link.url, DbType.String);
					parameters.Add("_thumbailUrl", link.thumbailUrl, DbType.String);
					parameters.Add("_author", link.author, DbType.String);
					parameters.Add("_status", link.status, DbType.Boolean);
					parameters.Add("_recommendedById", category, DbType.Int32);
					parameters.Add("_sectionTypeId", sectionType, DbType.Int32);
					parameters.Add("_linkTypeId", link.linkTypeId, DbType.Int32);
					parameters.Add("_familyProductId", familyId, DbType.Int32);
					parameters.Add("_userCompanyId", userCompanyId, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarVinculosSeccion", parameters);

					if (parameters.Get<int>("_response") == -2)
						throw new Exception("overLimit");
					else if (parameters.Get<int>("_response") == -1)
						throw new Exception("Se ha alcanzado el límite de registros");
					else if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para actualizar los datos de la familia
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateFamily()
		{
			log.trace("UpdateFamily");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_famililyProductId", familyId, DbType.Int32);
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_model", model, DbType.String);
				parameters.Add("_description", description, DbType.String);
				parameters.Add("_descriptionEnglish", descriptionEnglish, DbType.String);
				parameters.Add("_image", imageFamily, DbType.String);
				parameters.Add("_sku", sku, DbType.String);
				parameters.Add("_gtin", barCode, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_warranty", warranty, DbType.Boolean);
				parameters.Add("_expiration", expiration, DbType.Boolean);
				parameters.Add("_addTicket", addTicket, DbType.Boolean);
				parameters.Add("_category", category, DbType.Int32);
				parameters.Add("_company", company, DbType.Int32);
				parameters.Add("_lifeDays", lifeDays, DbType.Int32);
				parameters.Add("_autoLote", autoLote, DbType.Boolean);
				parameters.Add("_editLote", editLote, DbType.Boolean);
				parameters.Add("_consecutiveLote", consecutiveLote, DbType.Int32);
				parameters.Add("_prefix", prefix, DbType.String);
				parameters.Add("_option", option, DbType.Int32);
				parameters.Add("_colorFamilia", colorFamilia, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionFamiliaProducto", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("Ya existe una familia con el mismo codigo de barras");
				else if (parameters.Get<int>("_response") == -2)
					throw new Exception("El campo Consecutivo Lote es menor al actual, ingrese un valor mayor a " + consecutiveLote.ToString());

				foreach (var dir in directionFamilies)
				{
					parameters = new DynamicParameters();
					parameters.Add("_idDirection", dir.directionId, DbType.Int32);
					parameters.Add("_idFamily", familyId, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarDireccionesFamilias", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP de agregar direcciones");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para actualizar los datos de la especificación técnica
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateTechnicalSpecification()
		{
			log.trace("UpdateTechnicalSpecification");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				//Se actualizan los datos de especificación tecnica
				foreach (var et in technicalSpecifications)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_especificationTecnicalId", et.specificationTechnicalId, DbType.Int32);
					parameters.Add("_especificationTecnicalDetailId", 0, DbType.Int32);
					parameters.Add("_title", et.title, DbType.String);
					parameters.Add("_titleEnglish", et.titleEnglish, DbType.String);
					parameters.Add("_subtitle", "", DbType.String);
					parameters.Add("_description", "", DbType.String);
					parameters.Add("_subtitleEnglish", "", DbType.String);
					parameters.Add("_descriptionEnglish", "", DbType.String);
					parameters.Add("_image", "", DbType.String);
					parameters.Add("_option", option, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spu_edicionEspecificacionTecnica", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP");
					else if (parameters.Get<int>("_response") == -1)
						throw new Exception("overLimit");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para actualizar la especificacion tecnica de detalle
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateTechnicalSpecificationDetails()
		{
			log.trace("UpdateTechnicalSpecificationDetails");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				//Se actualiza el detalle de la especificacion tecnica
				foreach (var etd in technicalSpecificationDetails)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_especificationTecnicalId", 0, DbType.Int32);
					parameters.Add("_especificationTecnicalDetailId", etd.specificationTechnicalDetailId, DbType.Int32);
					parameters.Add("_title", "", DbType.String);
					parameters.Add("_titleEnglish", "", DbType.String);
					parameters.Add("_subtitle", etd.subtitle, DbType.String);
					parameters.Add("_description", etd.description, DbType.String);
					parameters.Add("_subtitleEnglish", etd.subtitleEnglish, DbType.String);
					parameters.Add("_descriptionEnglish", etd.descriptionEnglish, DbType.String);
					parameters.Add("_image", etd.imageBase, DbType.String);
					parameters.Add("_option", option, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spu_edicionEspecificacionTecnica", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP");
					else if (parameters.Get<int>("_response") == -1)
						throw new Exception("overLimit");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para actualizar los datos de las preguntas frecuentes
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateFrequentQuestions()
		{
			log.trace("UpdateFrequentQuestions");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				foreach (var faq in frequentQuestions)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_faqId", faq.questionId, DbType.Int32);
					parameters.Add("_questionSpanish", faq.question, DbType.String);
					parameters.Add("_responseSpanish", faq.response, DbType.String);
					parameters.Add("_questionEnglish", faq.questionEnglish, DbType.String);
					parameters.Add("_responseEnglish", faq.responseEnglish, DbType.String);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spu_edicionPreguntasFrecuentes", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP");
					else if (parameters.Get<int>("_response") == -1)
						throw new Exception("overLimit");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para actualizar las garantias de una familia
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateWarranties()
		{
			log.trace("UpdateWarranties");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				foreach (var war in warranties)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_warrantyId", war.warrantyId, DbType.Int32);
					parameters.Add("_country", war.country, DbType.String);
					parameters.Add("_pdfUrl", war.pdfBase, DbType.String);
					parameters.Add("_periodMonth", war.periodMonth, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spu_edicionGarantiasFamilias", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para actualizar los vinculos de una familia
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateLink()
		{
			log.trace("UpdateLink");
			try
			{
				CrearConexion(TipoConexion.Mysql, 1);
				AbrirConexion();
				CrearTransaccion();

				foreach (var link in links)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_linkId", link.linkId, DbType.Int32);
					parameters.Add("_title", link.title, DbType.String);
					parameters.Add("_url", link.url, DbType.String);
					parameters.Add("_thumbailUrl", link.thumbailUrl, DbType.String);
					parameters.Add("_author", link.author, DbType.String);
					parameters.Add("_status", link.status, DbType.Boolean);
					parameters.Add("_recommendedById", category, DbType.Int32);
					parameters.Add("_linkTypeId", link.linkTypeId, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spu_edicionVinculosSeccion", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para eliminar las familias
		/// Desarollador: David Martinez
		/// </summary>
		public void DeleteFamily()
		{
			log.trace("DeleteFamily");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_familyProductId", familyId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarFamiliaProducto", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("La familia contiene productos relacionados");
				else if (parameters.Get<int>("_response") == -2)
					throw new Exception("Existen usuarios móviles que tienen agregada la familia a 'Mi Catálogo'");
				else if (parameters.Get<int>("_response") == -3)
					throw new Exception("La familia tiene solicitudes de altas de productos");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para eliminar las garantias
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteWarranties()
		{
			log.trace("DeleteWarranties");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_warrantyId", familyId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarGarantiasFamilias", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para eliminar la especificación tecnica
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteTechnicalSpecification()
		{
			log.trace("DeleteTechnicalSpecification");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_especificationTecnicalId", familyId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarEspecificacionTecnica", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para realizar la eliminacion de los detalles de especificacion tecnica
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteTechnicalSpecificationDetails()
		{
			log.trace("DeleteTechnicalSpecificationDetails");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				//Se elimina los datos de la especificacion tecnica
				foreach (int etdd in deleteTechnicalSpecification)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_especificationTecnicalId", 0, DbType.Int32);
					parameters.Add("_especificationTecnicalDetailId", etdd, DbType.Int32);
					parameters.Add("_title", "", DbType.String);
					parameters.Add("_titleEnglish", "", DbType.String);
					parameters.Add("_subtitle", "", DbType.String);
					parameters.Add("_description", "", DbType.String);
					parameters.Add("_subtitleEnglish", "", DbType.String);
					parameters.Add("_descriptionEnglish", "", DbType.String);
					parameters.Add("_image", "", DbType.String);
					parameters.Add("_option", option, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spu_edicionEspecificacionTecnica", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP de eliminación de detalles de especificación técnica de familias");
					else if (parameters.Get<int>("_response") == -1)
						throw new Exception("overLimit");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para eliminar los vinculos de una familia
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteLink()
		{
			log.trace("DeleteLink");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_linkId", familyId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarVinculoSeccion", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para eliminar las preguntas frecuentes
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteFrequentQuestions()
		{
			log.trace("DeleteFrequentQuestions");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_faqId", familyId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarPreguntasFrecuentes", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los archivos de imagen o pdf de una familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public List<ArchivesFamiliesSQL> SearchArchivesFamilies()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_id", familyId, DbType.Int32);
				parameters.Add("_option", option, DbType.Int32);

				List<ArchivesFamiliesSQL> response = Consulta<ArchivesFamiliesSQL>("spc_consultaArchivosFamilias", parameters);
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
		/// Metodo para consultar los datos de la familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public List<ProductFamily> SearchProductFamily()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_companyId", company, DbType.Int32);

				List<ProductFamily> response = Consulta<ProductFamily>("spc_consultaFamiliaProducto", parameters);

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
		/// Metodo para consultar los datos de la familia para su edición
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public FamilyDataEdition SearchProductFamilyData()
		{
			log.trace("SearchProductFamilyData");
			FamilyDataEdition response = new FamilyDataEdition();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(ProductFamilyData));
				types.Add(typeof(DirectionFamilyData));
				types.Add(typeof(LimitsFamilyData));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_familyId", familyId, DbType.Int32);

				log.trace("Calling spc_consultaFamiliaProductoDatos");
				var respSQL = ConsultaMultiple("spc_consultaFamiliaProductoDatos", types, parameters);

				response.productFamilyData = respSQL[0].Cast<ProductFamilyData>().FirstOrDefault();
				response.directionFamily = respSQL[1].Cast<DirectionFamilyData>().ToList();
				response.limitsFamily = respSQL[2].Cast<LimitsFamilyData>().FirstOrDefault();

				DynamicParameters limitParams = new DynamicParameters();
				limitParams.Add("_familyID", familyId, DbType.String);
				limitParams.Add("_excepSpec", 0, DbType.Int32);//no need to exclude, we need all

				log.trace("Calling spc_consultarCantidadCharEspecs");
				List<LimitUsedFamily> responseLimits = Consulta<LimitUsedFamily>("spc_consultarCantidadCharEspecs", limitParams);
				response.specCharUse = responseLimits.Cast<LimitUsedFamily>().FirstOrDefault();

				DynamicParameters limitImgParams = new DynamicParameters();
				limitImgParams.Add("_familyID", familyId, DbType.String);

				log.trace("Calling spc_consultarCantidadImagenesEspec");
				List<LimitUsedFamily> responseImgLimits = Consulta<LimitUsedFamily>("spc_consultarCantidadImagenesEspec", limitImgParams);
				response.imageUse = responseImgLimits.Cast<LimitUsedFamily>().FirstOrDefault();


				CerrarConexion();
				return response;
			}
			catch (Exception ex)
			{
				log.error("Exception at SearchProductFamilyData: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para buscar las especificaciones tecnicas de una familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public TecnicalSpecificationSQL SearchTechnicalSpecification()
		{
			log.trace("SearchTechnicalSpecification");
			TecnicalSpecificationSQL response = new TecnicalSpecificationSQL();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TecnicalSpecificationDataSQL));
				types.Add(typeof(TecnicalSpecificationDetailsDataSQL));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_familyProductId", familyId, DbType.Int32);

				var respSQL = ConsultaMultiple("spc_consultaEspecificacionTecnica", types, parameters);

				response.tecnicalSpecificationData = respSQL[0].Cast<TecnicalSpecificationDataSQL>().ToList();
				response.tecnicalSpecificationDetails = respSQL[1].Cast<TecnicalSpecificationDetailsDataSQL>().ToList();
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
		/// Metodo para consultar los tipos de links que tiene una familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public LinksData SearchLink()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				LinksData response = new LinksData();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_familyProductId", familyId, DbType.Int32);
				parameters.Add("_sectionType", option, DbType.Int32);
				parameters.Add("_linkType", category, DbType.Int32);

				List<Type> types = new List<Type>();
				types.Add(typeof(LinkFamilyData));
				types.Add(typeof(LimitsFamilyData));

				var responseSQL = ConsultaMultiple("spc_consultaVinculosSeccion", types, parameters);

				response.linkData = responseSQL[0].Cast<LinkFamilyData>().ToList();
				response.limitsFamily = responseSQL[1].Cast<LimitsFamilyData>().FirstOrDefault();

				//List<LinkFamilyData> response = Consulta<LinkFamilyData>("spc_consultaVinculosSeccion", parameters);
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
		/// Metodo para consultarlas garantias y preguntas frecuentes
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public WarrantiesFaqFamily SearchWarrantiesFaq()
		{
			log.trace("SearchWarrantiesFaq");
			WarrantiesFaqFamily response = new WarrantiesFaqFamily();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_familyProductId", familyId, DbType.Int32);

				List<Type> types = new List<Type>();
				types.Add(typeof(WarrantiesFamilyData));
				types.Add(typeof(FrequentQuestionsFamilyData));
				types.Add(typeof(LimitsFamilyData));

				var responseSQL = ConsultaMultiple("spc_consultaGarantiasFamilias", types, parameters);

				response.warranties = responseSQL[0].Cast<WarrantiesFamilyData>().ToList();
				response.frequentQuestions = responseSQL[1].Cast<FrequentQuestionsFamilyData>().ToList();
				response.limitsFamily = responseSQL[2].Cast<LimitsFamilyData>().FirstOrDefault();

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
		/// Metodo para realizar la busqueda de los combos que se utilizaran en el modulo
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public FamilyListDropDownSQL SearchFamilyListDropDown()
		{
			log.trace("SearchFamilyListDropDown");
			try
			{
				FamilyListDropDownSQL response = new FamilyListDropDownSQL();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				if (option == 1)
				{
					List<Type> types = new List<Type>();
					types.Add(typeof(TraceITListDropDown));
					types.Add(typeof(TraceITListDropDown));
					types.Add(typeof(TraceITListDropDown));

					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_idCompany", company, DbType.Int32);
					parameters.Add("_option", option, DbType.Int32);

					var respSQL = ConsultaMultiple("spc_consultaCombosFamilia", types, parameters);

					response.companyData = respSQL[0].Cast<TraceITListDropDown>().ToList();
					response.categoryData = respSQL[1].Cast<TraceITListDropDown>().ToList();
					response.recommendedBy = respSQL[2].Cast<TraceITListDropDown>().ToList();
				}
				else
				{
					List<Type> types = new List<Type>();
					types.Add(typeof(TraceITListDropDown));

					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_idCompany", company, DbType.Int32);
					parameters.Add("_option", option, DbType.Int32);

					var respSQL = ConsultaMultiple("spc_consultaCombosFamilia", types, parameters);

					response.addressData = respSQL[0].Cast<TraceITListDropDown>().ToList();
				}

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
		#endregion

		#region Generics
		/// <summary>
		/// Metodo para consultar los paises que existen en el sistema de TraceIT
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public List<TraceITListDropDown> searchCountries()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<TraceITListDropDown> resp = Consulta<TraceITListDropDown>("spc_consultaPaises");
				CerrarConexion();

				return resp;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}
		#endregion

		#region Generics
		/// <summary>
		/// Metodo para consultar los estados por id de pais TraceIT
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<TraceITListDropDown> searchEstadobyPaisId()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_paisId", paisId, DbType.Int32);
				List<TraceITListDropDown> resp = Consulta<TraceITListDropDown>("spc_consultaEstados", parameters);
				CerrarConexion();

				return resp;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}
		#endregion

		public bool isHexa(string number)
		{
			try
			{
				if (number.Length == 13)
				{
					ulong a = ulong.Parse(number, System.Globalization.NumberStyles.HexNumber);
					return a.ToString()[0] == '1' ? true : false;
				}
				else
				{

					return false;
				}

			}
			catch (Exception ex)
			{
				return false;
			}
		}
		#endregion
	}
}
