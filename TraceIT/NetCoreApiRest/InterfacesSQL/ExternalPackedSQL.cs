using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using WSTraceIT.Models.Response;

namespace WSTraceIT.InterfacesSQL
{
	public class ExternalPackedSQL: DBHelperDapper
	{
		#region Properties
		//Datos Empacador
		public int packedId;
		public string packedNumber;
		public string packedName;
		public string email;
		public string password;
		public string phone;
		public bool status;
		public int userId;
		public int companyId;
		public bool opc;
		public int merma;
		public int type;
		public int typeUsuario;
		public List<int> companiasId;
		public int companyIdSearch;
		public List<int> auxGetCompany;

		//Nombre SP a ejecutar
		public string sp;

		//Datos del filtro para los Empacadores
		public List<ExternalPackedDataSQL> packedList;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ExternalPackedSQL");
		#endregion

		#region Constructor
		public ExternalPackedSQL()
		{
			this.packedId = 0;
			this.packedNumber = String.Empty;
			this.packedName = String.Empty;
			this.email = String.Empty;
			this.password = String.Empty;
			this.phone = String.Empty;
			this.status = false;
			this.userId = 0;
			this.companyId = 0;
			this.merma = 0;
			this.opc = false;
			this.type = 0;

			this.sp = String.Empty;

			this.packedList = new List<ExternalPackedDataSQL>();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar los empacadores
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ExternalPackedDataSQL> SearchPackedList()
		{
			log.trace("SearchExternalPackedList");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedName", packedName, DbType.String);
				parameters.Add("_type", 1, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_companyIdSearch", companyIdSearch, DbType.Int32);
				//Tipo de usuario
				parameters.Add("_typeUsuario", typeUsuario, DbType.Int32);
				//Usuario de empacador
				parameters.Add("_userId", userId, DbType.Int32);

				List<ExternalPackedDataSQL> response = Consulta<ExternalPackedDataSQL>("spc_consultaEmpacadorEmpaqEtiq", parameters);
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
		/// Metodo para consultar los datos de un empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ExternalPackedDataSQL> SearchPackedData()
		{
			log.trace("SearchExternalPackedDataSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				List<ExternalPackedDataSQL> response = Consulta<ExternalPackedDataSQL>("spc_consultaEmpacadorEmpaqEtiqDatos", parameters);
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
		/// Metodo para guardar / actualizar empacadores
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SavePacked()
		{
			log.trace("SaveExternalPackedSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				if (packedId > 0)
					parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_packedNumber", packedNumber, DbType.String);
				parameters.Add("_packedName", packedName, DbType.String);
				parameters.Add("_merma", merma, DbType.String);
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_password", password, DbType.String);
				parameters.Add("_phone", phone, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_type", 1, DbType.Int32);
				if (packedId == 0)
					parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(sp, parameters);

				int response = parameters.Get<int>("_response");

				if (packedId > 0 && companyId > 0)
				{
					//Si la peticiónse hace desde una compañia, solo podra modificar la merma
					string query = $"UPDATE rel_051_empacadorCompania SET porcentajeMerma = {merma} WHERE CompaniaId  = {companyId} and EmpacadorId = {packedId};";
					ConsultaCommand<string>(query);
				}
				else if (response == 0)
					throw new Exception("Error al ejecutar el SP de " + (packedId > 0 ? "editar" : "agregar" ) + " empacador");
				else if (response == -1)
					throw new Exception("Ya existe un empacador con el mismo número de empacador");
				else if (response == -2)
					throw new Exception("Ya existe un empacador con el mismo nombre");
				else if (response == -3)
					throw new Exception("Ya existe un empacador con la misma razón social");
				else if (response == -4)
					throw new Exception("Ya existe un usuario con el mismo correo electrónico");
				else if (sp.Equals("spu_edicionEmpacadorEmpaqEtiq"))
				{
					//Nos regresa un array con las compañias que ya no se encuentren listadas en la petición
					var viejos = auxGetCompany.Except(companiasId);
					//Nos regresa un array con las compañias nuevas que se agregaron en la petición
					var nuevos = companiasId.Except(auxGetCompany);
					foreach (int nuevo in nuevos)
					{
						//Creamos los registros para las nuevas compañias que seran ligadas (Solo usuario TraceIt)
						string query = $"INSERT INTO rel_051_empacadorCompania (CompaniaId, EmpacadorId) VALUES({ nuevo },{packedId});";
						ConsultaCommand<string>(query);
					}
					//Eliminamos los registros de la tabla pivote (Eliminados por usuario TraceIt)
					foreach (int viejo in viejos)
					{
						string query = $"DELETE FROM rel_051_empacadorCompania  WHERE CompaniaId = {viejo} and EmpacadorId = {packedId};";
						ConsultaCommand<string>(query);
					}

				}
				else if (response > 0)
				{
					foreach (int compania in companiasId)
					{
						string query = $"INSERT INTO rel_051_empacadorCompania (CompaniaId, EmpacadorId) VALUES({compania},{response});";
						ConsultaCommand<string>(query);
					}
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
		/// Metodo para eliminar empacadores
		/// Desarollador: Iván Gutiérrez
		/// </summary>
		public void DeletePacked()
		{
			log.trace("DeleteExternalPackedSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarEmpacadorEmpaqEtiq", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("No se puede eliminar un empacador con estatus activo");

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
		#endregion

		#endregion
	}

	public class ExternalPackedOperatorSQL : DBHelperDapper
	{
		#region Properties
		//Datos Operador
		public int operatorId;
		public string code;
		public string name;
		public string image;
		public int addressId;
		public int packedId;
		public int userId;
		public bool opc;
		public int companyId;

		//Nombre SP a ejecutar
		public string sp;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ExternalPackedOperatorSQLSQL");
		#endregion

		#region Constructor
		public ExternalPackedOperatorSQL()
		{
			this.operatorId = 0;
			this.code = String.Empty;
			this.name = String.Empty;
			this.image = String.Empty;
			this.addressId = 0;
			this.packedId = 0;
			this.userId = 0;
			this.opc = false;
			this.companyId = 0;

			this.sp = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar los operadores de un empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ExternalPackedOperatorDataSQL> SearchPackedOperatorsList()
		{
			log.trace("SearchPackedOperatorsList");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedId", packedId, DbType.Int32);

				List<ExternalPackedOperatorDataSQL> response = Consulta<ExternalPackedOperatorDataSQL>("spc_consultaOperadoresEmpaqEtiq", parameters);
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
		/// Metodo para consultar los datos para un operador de un empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ExternalPackedOperatorDataSQL> SearchPackedOperatorData()
		{
			log.trace("SearchPackedOperatorDataSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_operatorId", operatorId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);

				List<ExternalPackedOperatorDataSQL> response = Consulta<ExternalPackedOperatorDataSQL>("spc_consultaOperadoresEmpaqEtiqDatos", parameters);
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
		/// Metodo para guardar / actualizar un operador de un empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SavePackedOperator()
		{
			log.trace("SavePackedOperatorSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				if (operatorId > 0)
					parameters.Add("_operatorId", operatorId, DbType.Int32);
				parameters.Add("_code", code, DbType.String);
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_image", image, DbType.String);
				if (operatorId == 0) {
					parameters.Add("_type", 1, DbType.Int32);
					parameters.Add("_addressId", addressId, DbType.Int32);
				}
					parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);  

				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(sp, parameters);

				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP de " + (operatorId > 0 ? "editar" : "agregar") + " operador");
				else if (response == -1)
					throw new Exception("Ya existe un operador con el mismo nombre en la misma compañia.");

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
		/// Metodo para eliminar un operador de un empacador
		/// Desarollador: Iván Gutiérrez
		/// </summary>
		public void DeletePackedOperator()
		{
			log.trace("DeletePackedOperatorSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_operatorId", operatorId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarOperadorEmpaqEtiq", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("No se puede eliminar un operador con dependecia de otros registros");

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
		#endregion

		#endregion
	}

	public class ExternalPackedProductionLineSQL : DBHelperDapper
	{
		#region Properties
		//Datos Linea de Producción
		public int lineId;
		public string name;
		public int addressId;
		public int packedId;
		public int userId;
		public bool opc;
		public int companyId;

		//Nombre SP a ejecutar
		public string sp;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ExternalPackedProdLineSQL");
		#endregion

		#region Constructor
		public ExternalPackedProductionLineSQL()
		{
			this.lineId = 0;
			this.name = String.Empty;
			this.addressId = 0;
			this.packedId = 0;
			this.userId = 0;
			this.opc = false;
			this.companyId = 0;

			this.sp = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar las lineas de producción de un empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ExternalPackedProdLineDataSQL> SearchPackedProdLinesList()
		{
			log.trace("SearchPackedProdLinesExtList");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedId", packedId, DbType.Int32);

				List<ExternalPackedProdLineDataSQL> response = Consulta<ExternalPackedProdLineDataSQL>("spc_consultaLineasProduccionEmpaqEtiq", parameters);
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
		/// Metodo para consultar los datos para un operador de un empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ExternalPackedProdLineDataSQL> SearchPackedProdLineData()
		{
			log.trace("SearchPackedProdLineDataExtSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_lineId", lineId, DbType.Int32);

				List<ExternalPackedProdLineDataSQL> response = Consulta<ExternalPackedProdLineDataSQL>("spc_consultaLineaProduccionEmpaqEtiqDatos", parameters);
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
		/// Metodo para guardar / actualizar una linea de producción de un empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SavePackedProdLine()
		{
			log.trace("SavePackedProdLineExtSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				if (lineId > 0)
					parameters.Add("_lineId", lineId, DbType.Int32);
				parameters.Add("_name", name, DbType.String);
				if (lineId == 0) {
					parameters.Add("_type", 1, DbType.Int32);
					parameters.Add("_userId", userId, DbType.Int32);
                }

				parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(sp, parameters);

				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP de " + (lineId > 0 ? "editar" : "agregar") + " linea de producción");
				else if (response == -1)
					throw new Exception("Ya existe una linea con el mismo nombre en la misma compañia.");

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
		/// Metodo para eliminar una linea de producción de un empacador
		/// Desarollador: Iván Gutiérrez
		/// </summary>
		public void DeletePackedProdLine()
		{
			log.trace("DeletePackedProdLineExtSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_lineId", lineId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarLineaProduccionEmpaqEtiq", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("No se puede eliminar una linea de producción con dependecia de otros registros");

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
		#endregion

		#endregion
	}

	public class ProductExternalPackedSQL : DBHelperDapper
	{
		#region Properties
		//Datos Empacador
		public int packedId;
		public int userId;
		public int topc;

		// Asociar Productos
		public string rawMaterial;
		public int productId;
		public int companyId;
		public int packagingId;

		//Nombre SP a ejecutar
		public string sp;

		//Datos para asociar productos
		public List<ProductExternalPackedDataSQL> products;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ProductExternalPackedSQL");
		#endregion

		#region Constructor
		public ProductExternalPackedSQL()
		{
			this.packedId = 0;
			this.userId = 0;
			this.topc = 0;

			this.rawMaterial = String.Empty;
			this.productId = 0;
			this.companyId = 0;
			this.packagingId = 0;

			this.products = new List<ProductExternalPackedDataSQL>();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para guardar la lista de productos a asociar
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SaveProduct()
		{
			log.trace("SaveProductSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				foreach (var product in products)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_packedId", product.packedId, DbType.Int32);
					parameters.Add("_productId", product.productId, DbType.Int32);
					parameters.Add("_companyId", product.companyId, DbType.Int32);
					parameters.Add("_packagingId", product.packagingId, DbType.Int32);
					parameters.Add("_rawMaterial", product.rawMaterial.Trim(), DbType.String);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarAsociarProductosEmpacadoIE", parameters);

					if (parameters.Get<int>("_response") == 0)
						throw new Exception("Error al ejecutar el SP");
					else if (parameters.Get<int>("_response") == -1)
						throw new Exception("La materia prima ya se encuentra registrada");
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
		/// Metodo para consultar los productos asociados de un empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ProductExternalPackedDataSQL> SearchProductPacked()
		{
			log.trace("SearchProductPackedSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_packagingId", packagingId, DbType.Int32);
				parameters.Add("_rawMaterial", rawMaterial, DbType.String);
				parameters.Add("_opc", topc, DbType.Int32);

				List<ProductExternalPackedDataSQL> response = Consulta<ProductExternalPackedDataSQL>("spc_consultaAsociarProductosEmpacadoIE", parameters);
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

		#endregion
	}

	public class ExternalPackedBoxManagementSQL : DBHelperDapper
	{
		#region Properties
		//Datos Empacador
		public int packedId;
		public int userId;
		public int topc;

		//Gestionar cajas
		public int groupingId;
		public int addressId;
		public int productId;
		public int pallet;
		public int box;
		public int typeView;
		public DateTime dateStart;
		public DateTime dateEnd;

		public int companiaId;

		public string searchfield;

		// Lista de agrupaciones para la gestión de cajas
		public List<ExtPackedBoxManagDataSQL> boxesData;
		public List<ExtPackedBoxManagDataDetailSQL> boxesDataDetail;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ExternalPackedBoxManagementExtSQL");
		#endregion

		#region Constructor
		public ExternalPackedBoxManagementSQL()
		{
			this.packedId = 0;
			this.userId = 0;
			this.topc = 0;

			this.groupingId = 0;
			this.addressId = 0;
			this.productId = 0;
			this.pallet = 0;
			this.box = 0;

			this.companiaId = 0;

			this.typeView = 0;
			this.dateStart = default(DateTime);
			this.dateEnd = default(DateTime);
			this.searchfield = String.Empty;

			this.boxesData = new List<ExtPackedBoxManagDataSQL>();
			this.boxesDataDetail = new List<ExtPackedBoxManagDataDetailSQL>();
        }
        #endregion

        #region Public methods

        #region BackOffice
        /// <summary>
        /// Metodo para consultar la info para la gestión de cajas
        /// Desarrollador: Iván Gutiérrez
        /// </summary>
        /// <returns></returns>
        public List<GestionCajasPackedBoxManagDataSQL> SearchPackedBoxManagement()
        {
            log.trace("SearchPackedBoxManagementExtSQL");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_packedId", packedId, DbType.Int32); // 16
                parameters.Add("_productId", productId, DbType.Int32); // 0
                parameters.Add("_dateStart", dateStart, DbType.String);
                parameters.Add("_dateEnd", dateEnd, DbType.String);
                parameters.Add("_companiaId", companiaId, DbType.Int32); // 0 - 402
                parameters.Add("_typeView", typeView, DbType.Int32);


                List<GestionCajasPackedBoxManagDataSQL> resp = Consulta<GestionCajasPackedBoxManagDataSQL>("spc_consultaGestionCajasEmpacadoIE", parameters);
                return resp;

                CerrarConexion();
            }
            catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                CerrarConexion();
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para consultar la info para la gestión de cajasde acuerdo al tipo de filtro
        /// Desarrollador: Iván Gutiérrez
        /// </summary>
        /// <returns></returns>
        public List<ExtPackedBoxManagDataSQL> SearchPackedBoxManagementFilter()
		{
			log.trace("SearchPackedBoxManagementExtFSQL");
			try
			{
				var result = new List<ExtPackedBoxManagDataSQL>();
				var tempresult = new List<ExtPackedBoxManagDataSQL>();
				var tem = new List<ExtPackedBoxManagDataSQL>();

								/*
				 * Agrupación
				 */
				if (typeView == 1)
				{
					tempresult = (from ts in boxesData
								  select new ExtPackedBoxManagDataSQL
								  {
									  groupingId = ts.groupingId,
									  groupingName = ts.groupingName,
									  groupingType = typeView,
									  pallet = (from ts2 in boxesData
												where ts2.groupingId == ts.groupingId
												group ts2 by ts2.pallet into newGroup
												select newGroup).ToList().Count.ToString(),
									  box = (from tsbox in boxesData
											 where tsbox.groupingId == ts.groupingId
											 select new ExtPackedBoxManagDataSQL
											 {
												 box = tsbox.box
											 }).ToList().Count.ToString(),
									  productId = ts.productId,
									  productName = ts.productName,
									  quantity = boxesData.Where(x => x.groupingId == ts.groupingId ).Select(x => x.quantity).Sum(),
									  isGroup = ts.isGroup
								  }).ToList();

					result = tempresult.GroupBy( g => g.groupingId).Select(grp => grp.FirstOrDefault()).ToList();
					//result = tempresult.GroupBy( g => new { g.groupingId,  g.productId}).Select(grp => grp.FirstOrDefault()).ToList();
				}

				/*
				 * Pallet
				 */
				if (typeView == 2)
				{
					tempresult = (from ts in boxesData
								  select new ExtPackedBoxManagDataSQL
								  {
									  groupingId = ts.groupingId,
									  groupingName = ts.groupingName,
									  groupingType = typeView,
									  pallet = ts.pallet,
									  box = boxesData.Where(g => g.groupingId == ts.groupingId).Where(g => g.pallet == ts.pallet).ToList().Count.ToString(),
									  productId = ts.productId,
									  productName = ts.productName,
									  quantity = boxesData.Where(g => g.groupingId == ts.groupingId).Where(g => g.pallet == ts.pallet).Select(g => g.quantity).Sum(),
									  isGroup = ts.isGroup
								  }).ToList();

					result = tempresult.GroupBy(g => new { g.groupingId, g.pallet }).Select(grp => grp.FirstOrDefault()).ToList();
				}

				/*
				 * Caja
				 */
				if (typeView == 3)
				{
					tempresult = (from ts in boxesData
								  select new ExtPackedBoxManagDataSQL
								  {
									  groupingId = ts.groupingId,
									  groupingName = ts.groupingName,
									  groupingType = typeView,
									  pallet = ts.pallet,
									  box = ts.box,
									  productId = ts.productId,
									  productName = ts.productName,
									  quantity = ts.quantity,
									  isGroup = ts.isGroup
								  }).ToList();

					result = tempresult;
				}

				// Aplicar filtro de búsqueda
				if(searchfield != "") {
					result = (from ts in boxesData
							  where ts.groupingName.Contains(searchfield)
									|| ts.pallet == searchfield
									|| ts.box == searchfield
									|| ts.productName.Contains(searchfield)
									|| ts.quantity.ToString() == searchfield
							  select ts).ToList();
				}

				return result;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar la info para de una agrupación
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public void SearchPackedBoxManagementDetail(SearchExtPackedBoxManagementDetailResponse response)
		{
			log.trace("SearchPackedBoxManagementDetailExtSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_operationId", groupingId, DbType.Int32);

				List<Type> types = new List<Type>();
				types.Add(typeof(OperationDatase));
				types.Add(typeof(OperationDetailse));
				types.Add(typeof(DetailsOrderse));

				//var result = Consulta<ExtPackedOperationInfoPrintSQL>("spc_consultaOperacionesPrint", parameters);
				var res = ConsultaMultiple("spc_consultaGestionCajasEmpacadoIEDatos", types, parameters);

                if (res.Count < 3)
                {
					response.messageEsp = "Hubo un error en la obtención de los datos de la operación.";
					throw new Exception(response.messageEsp);
                }

				OperationDatase operationDatase = res[0].Cast<OperationDatase>().First();
				List<OperationDetailse> operationDetailse = res[1].Cast<OperationDetailse>().ToList();
				List<DetailsOrderse> DetailsOrderse = res[2].Cast<DetailsOrderse>().ToList();

				response.infoOperation = operationDatase;
				response.OperationDetails = operationDetailse;
				response.detailsOrders = DetailsOrderse;

				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar la info para una agrupación de acuerdo al tipo de filtro
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ExtPackedBoxManagDataDetailSQL> SearchPackedBoxManagementDetailFilter()
		{
			log.trace("SearchPackedBoxManagementDetailFExtSQL");
			try
			{
				var result = new List<ExtPackedBoxManagDataDetailSQL>();
				var tempresult = new List<ExtPackedBoxManagDataDetailSQL>();
				var tem = new List<ExtPackedBoxManagDataDetailSQL>();

				/*
				 * Agrupación
				 */
				if (typeView == 1)
				{
					tempresult = (from ts in boxesDataDetail
								  where ts.productId == productId
								  select new ExtPackedBoxManagDataDetailSQL
								  {
									  groupingId = ts.groupingId,
									  groupingName = ts.groupingName,
									  groupingType = typeView,
									  lineName = ts.lineName,
									  operatorName = ts.operatorName,
									  clamsShells = ts.clamsShells,
									  registerDate = ts.registerDate,
									  range = ts.range,
									  pallet = (from ts2 in boxesDataDetail
												group ts2 by ts2.pallet into newGroup
												select newGroup).ToList().Count.ToString(),
									  box = boxesDataDetail.Count.ToString(),
									  rangeDetail = ts.rangeDetail,
									  productId = ts.productId,
									  productName = ts.productName,
									  quantity = ts.quantity
								  }).ToList();

					result = tempresult.GroupBy(g => g.groupingId).Select(grp => grp.FirstOrDefault()).ToList();
				}

				/*
				 * Pallet
				 */
				if (typeView == 2)
				{
					tempresult = (from ts in boxesDataDetail
								  where ts.productId == productId
								  select new ExtPackedBoxManagDataDetailSQL
								  {

									  groupingId = ts.groupingId,
									  groupingName = ts.groupingName,
									  groupingType = typeView,
									  lineName = ts.lineName,
									  operatorName = ts.operatorName,
									  clamsShells = ts.clamsShells,
									  registerDate = ts.registerDate,
									  range = ts.range,
									  pallet = ts.pallet,
									  box = boxesDataDetail.Count.ToString(),
									  rangeDetail = ts.rangeDetail,
									  productId = ts.productId,
									  productName = ts.productName,
									  quantity = ts.quantity
								  }).ToList();

					result = tempresult.GroupBy(g => new { g.groupingId, g.pallet }).Select(grp => grp.FirstOrDefault()).ToList();
				}

				/*
				 * Caja
				 */
				if (typeView == 3)
				{
					tempresult = (from ts in boxesDataDetail
								  where ts.productId == productId
								  select new ExtPackedBoxManagDataDetailSQL								 
								  {
									  groupingId = ts.groupingId,
									  groupingName = ts.groupingName,
									  groupingType = typeView,
									  lineName = ts.lineName,
									  operatorName = ts.operatorName,
									  clamsShells = ts.clamsShells,
									  registerDate = ts.registerDate,
									  range = ts.range,
									  pallet = ts.pallet,
									  box = ts.box,
									  rangeDetail = ts.rangeDetail,
									  productId = ts.productId,
									  productName = ts.productName,
									  quantity = ts.quantity
								  }).ToList();

					result = tempresult;
				}

				return result;
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

	public class ExternalPackedArmingReportSQL : DBHelperDapper
	{
		#region Properties
		//Datos Filtro
		public int typeCompany;
		public int companyId;
		public int addressId;
		public int productId;
		public string searchGeneric;
		public string dateStart;
		public string dateEnd;

		public int packagingId;
		public int userId;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ExternalPackedArmingReportSQL");
		#endregion

		#region Constructor
		public ExternalPackedArmingReportSQL()
		{
			this.typeCompany = 0;
			this.companyId = 0;
			this.addressId = 0;
			this.productId = 0;
			this.searchGeneric = String.Empty;
			this.dateStart = String.Empty;
			this.dateEnd = String.Empty;

			this.packagingId = 0;
			this.userId = 0;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar los datos necesarios para el reporte de armados
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public SearchExtPackedArmingReportResponse SearchArmingReport()
		{
			log.trace("SearchArmingReportEXTSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(ExtPackedArmingReporDataCountSQL));
				//types.Add(typeof(ExtPackedArmingReporDataCountSQL));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packagingId", packagingId, DbType.Int32);
				parameters.Add("_typeCompany", typeCompany, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_addressId", addressId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				//parameters.Add("_searchGeneric", searchGeneric, DbType.String);
				parameters.Add("_dateStart", dateStart, DbType.String);
				parameters.Add("_dateEnd", dateEnd, DbType.String);
				parameters.Add("_auxInfo", "CAJA", DbType.String);
				SearchExtPackedArmingReportResponse response = new SearchExtPackedArmingReportResponse();
				var responseSQLCaja = ConsultaMultiple("spc_consultaReporteArmadosEmpacadoIE", types, parameters);

				/* 
				 *CAJAS
				*/
				response.infoDataBoxes = responseSQLCaja[0].Cast<ExtPackedArmingReporDataCountSQL>().ToList();


				/*
				 * PALLET
				*/
				parameters.Add("_auxInfo", "PALLET", DbType.String);
				var responseSQLPallet = ConsultaMultiple("spc_consultaReporteArmadosEmpacadoIE", types, parameters);
				response.infoDataPallet = responseSQLPallet[0].Cast<ExtPackedArmingReporDataCountSQL>().ToList();


				/*
				 * Mermas
				 */
				parameters.Add("_auxInfo", "MERMA", DbType.String);
				var responseSQLMerma = ConsultaMultiple("spc_consultaReporteArmadosEmpacadoIE", types, parameters);
				response.infoDataWaste = responseSQLMerma[0].Cast<ExtPackedArmingReporDataCountSQL>().ToList();

				/*
				 * Merma en operaciones
				*/
				parameters.Add("_auxInfo", "MERMAS_OPERACIONES", DbType.String);
				var responseSQLMermaEnOperaciones = ConsultaMultiple("spc_consultaReporteArmadosEmpacadoIE", types, parameters);
				CerrarConexion();
				response.infoDataWasteOperation = responseSQLMermaEnOperaciones[0].Cast<ExtPackedArmingReporDataCountSQL>().ToList();
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

	public class ExternalPackedUnionOperationSQL : DBHelperDapper
	{
		#region Properties
		//Datos Operaciones
		public int groupingPId;
		public int groupingSId;
		public int groupingType;
		public string groupingName;
		public List<int> groupings;
		public int userId;
		public int userType;

		//Nombre SP a ejecutar
		public string sp;

		//Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ExternalPackedUnionOperationSQL");

		//Variable para guardar la nueva union de operaciones
		public ExtPackedOperationUnionSQL newOperation;
		public List<ExtPackedOperationDetailUnionSQL> resultDetail;
		public List<ExtPackedOperationDetailScannedUnionSQL> resultScanned;
		#endregion

		#region Constructor
		public ExternalPackedUnionOperationSQL()
		{
			this.groupingPId = 0;
			this.groupingSId = 0;
			this.groupingType = 0;
			this.groupings = new List<int>();
			this.groupingName = String.Empty;
			this.userId = 0;
			this.userType = 0;
			this.resultDetail = new List<ExtPackedOperationDetailUnionSQL>();
			this.resultScanned = new List<ExtPackedOperationDetailScannedUnionSQL>();

			this.sp = String.Empty;

			this.newOperation = new ExtPackedOperationUnionSQL();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para generar una nueva unión entre operaciones
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void UnionProcessExt(SaveExternalPackedResponse response)
		{
			log.trace("UnionProcessExtSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				List<ExtPackedOperationUnionSQL> datosOperaciones = new List<ExtPackedOperationUnionSQL>();
				types.Add(typeof(ExtPackedOperationUnionSQL));
				types.Add(typeof(ExtPackedOperationUnionSQL));

				//DynamicParameters parameters = new DynamicParameters();
				//parameters.Add("_operationPId", groupingPId, DbType.Int32);
				//parameters.Add("_operationSId", groupingSId, DbType.Int32);
				//parameters.Add("_nboxesPallet", 0, DbType.Int32);
				//parameters.Add("_opc", 1, DbType.Int32);

				//var result = ConsultaMultiple("spc_consultaOperacionesUnion", types, parameters);

				//datosOperaciones = Consulta<ExtPackedOperationUnionSQL>("spc_consultaOperacionesUnion", parameters).ToList();
				DynamicParameters parameters = new DynamicParameters();

                for (int i = 1; i < groupings.Count; i++)
                {
					parameters = new DynamicParameters();
					parameters.Add("_opc", 1, DbType.Int32);
					parameters.Add("_nboxesPallet", 0, DbType.Int32);
                    parameters.Add("_operationPId", groupings[0], DbType.Int32);
                    parameters.Add("_operationSId", groupings[i], DbType.Int32);
					var results = Consulta<ExtPackedOperationUnionSQL>("spc_consultaOperacionesUnion", parameters).ToList();

					if (results != null && results.Count > 0)
					{
                        if (i == 1)
                        {
							datosOperaciones.Add(results[0]);
                        }
						datosOperaciones.Add(results[1]);
					}
				}

				CerrarConexion();
				
				if (datosOperaciones.Count <= 1) {
					response.messageEsp = "Error al unir las agrupaciones";
					throw new Exception(response.messageEsp);
                }

				//List<ExtPackedOperationUnionSQL> operacion1 = result[0].Cast<ExtPackedOperationUnionSQL>().ToList();
				//List<ExtPackedOperationUnionSQL> operacion2 = result[1].Cast<ExtPackedOperationUnionSQL>().ToList();
				
				if(datosOperaciones.Count < 2) {
					response.messageEsp = "Error, en algunas de las agrupaciones no se encontraron resultados";
					throw new Exception(response.messageEsp);
				}

                for (int i = 1; i < datosOperaciones.Count; i++)
                {
                    if (datosOperaciones[0].companyId != datosOperaciones[i].companyId)
                    {
						response.messageEsp = "Error, las agrupaciones no corresponden a la misma compañía";
						throw new Exception(response.messageEsp);
					}

					if (datosOperaciones[0].productId != datosOperaciones[i].productId)
					{
						response.messageEsp = "Error, las agrupaciones no corresponden al mismo producto";
						throw new Exception(response.messageEsp);
					}

					if (datosOperaciones[0].packagingId != datosOperaciones[i].packagingId)
					{
						response.messageEsp = "Error, las agrupaciones no corresponden al mismo embalaje";
						throw new Exception(response.messageEsp);
					}
				}

				//if (operacion1[0].companyId != operacion2[0].companyId | operacion1[0].productId != operacion2[0].productId | operacion1[0].packagingId != operacion2[0].packagingId) {
				//	if (operacion1[0].companyId != operacion2[0].companyId) {
				//		response.messageEsp = "Error, las agrupaciones no corresponden a la misma compañía";
				//		throw new Exception(response.messageEsp);
				//	}
				//	if (operacion1[0].productId != operacion2[0].productId) {
				//		response.messageEsp = "Error, las agrupaciones no corresponden al mismo producto";
				//		throw new Exception(response.messageEsp);
				//	}
				//	if (operacion1[0].packagingId != operacion2[0].packagingId) {
				//		response.messageEsp = "Error, las agrupaciones no corresponden al mismo embalaje";
				//		throw new Exception(response.messageEsp);
				//	}
				//}

				// Datos agrupación Primaria
				var operacionP = datosOperaciones[0];

				// Llenar la nueva agrupación con la primaria
				newOperation.companyId = operacionP.companyId;
				newOperation.proveedorId = operacionP.proveedorId;
				newOperation.operatorId = operacionP.operatorId;
				newOperation.lineId = operacionP.lineId;
				newOperation.productId = operacionP.productId;
				newOperation.packagingId = operacionP.packagingId;
				newOperation.groupingName = groupingName;
				newOperation.totalSize = operacionP.totalSize;
				newOperation.startDate = operacionP.startDate;
				newOperation.endDate = operacionP.endDate;
				newOperation.range = operacionP.range.Split('-')[0];
				newOperation.unitsScanned = operacionP.unitsScanned;
				newOperation.nboxesPallet = operacionP.nboxesPallet;
				newOperation.unitsPerBox = operacionP.unitsPerBox;

				// Datos agrupación Primaria
				for (int i = 1; i < datosOperaciones.Count; i++)
                {
					newOperation.totalSize += datosOperaciones[i].totalSize;
					newOperation.unitsScanned += datosOperaciones[i].unitsScanned;
				}

				newOperation.range += '-' + datosOperaciones[datosOperaciones.Count - 1].range.Split('-')[1];

				//var operacionS = operacion2.First();
				//newOperation.totalSize += operacionS.totalSize;
				//newOperation.unitsScanned += operacionS.unitsScanned;
				//newOperation.range += '-' + operacionS.range.Split('-')[1];

				// Armado del detalle de la operación
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

                for (int i = 1; i < datosOperaciones.Count; i++)
                {
					parameters = new DynamicParameters();
					parameters.Add("_operationPId", datosOperaciones[0].operationId, DbType.Int32);
					parameters.Add("_operationSId", datosOperaciones[i].operationId, DbType.Int32);
					parameters.Add("_nboxesPallet", newOperation.nboxesPallet, DbType.Int32);
					parameters.Add("_opc", 2, DbType.Int32);

					var resultadosDetalles = Consulta<ExtPackedOperationDetailUnionSQL>("spc_consultaOperacionesUnion", parameters);
					//resultDetail = Consulta<ExtPackedOperationDetailUnionSQL>("spc_consultaOperacionesUnion", parameters);

					if (resultadosDetalles.Count == 0)
					{
						response.messageEsp = "Error, No se encontró el detalle de una de las operaciones";
						throw new Exception(response.messageEsp);
					}

					if (resultadosDetalles.GroupBy(x => x.operationId).ToList().Count <= 1)
					{
						response.messageEsp = "Error, Falta el detalle de una de las operaciones";
						throw new Exception(response.messageEsp);
					}

					parameters.Add("_opc", 3, DbType.Int32);
					var resultadosEscaneados = Consulta<ExtPackedOperationDetailScannedUnionSQL>("spc_consultaOperacionesUnion", parameters);
					//resultScanned = Consulta<ExtPackedOperationDetailScannedUnionSQL>("spc_consultaOperacionesUnion", parameters);
					if (resultadosEscaneados.Count == 0)
					{
						response.messageEsp = "Error, No se encontraron productos escaneados en una de las operaciones";
						CerrarConexion();
						throw new Exception(response.messageEsp);
					}

                    foreach (var result in resultadosDetalles)
                    {
                        if (i == 1)
                        {
							resultDetail.Add(result);
						}
						else {

                            if (result.operationId != datosOperaciones[0].operationId)
                            {
								resultDetail.Add(result);
							}

                        }
						
                    }

                    foreach (var result in resultadosEscaneados)
                    {
                        if (i == 1)
                        {
							resultScanned.Add(result);
                        } 
						else 
						{
                            if (result.OperacionId != datosOperaciones[0].operationId)
                            {
								resultScanned.Add(result);
							}
                        }
                    }
				}

				var contadorCaja = 1;
				var contadorPalle = 1;
                foreach (var detalle in resultDetail)
                {
					detalle.box = $"Bx-{contadorCaja}";
					detalle.pallet = $"PL-{contadorPalle}";

                    if (contadorCaja == newOperation.nboxesPallet)
                    {
						contadorCaja = 1;
						contadorPalle++;
                    } else
					{
						contadorCaja++;
                    }
                }

				// Calcular las unidades por caja en caso de que no se indique el valor
				if(newOperation.unitsPerBox == 0) {
					newOperation.unitsPerBox = newOperation.totalSize / resultDetail.Count;
                }

				
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para guardar una Unión de operaciones
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SavePackedUnionOperation(SaveExternalPackedResponse response)
		{
			log.trace("SaveUnionOperationExtSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_operationPId", groupingPId, DbType.Int32);
				parameters.Add("_operationSId", groupingSId, DbType.Int32);
				parameters.Add("_typeView", groupingType, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_userType", userType, DbType.Int32);

				parameters.Add("_companyId", newOperation.companyId, DbType.Int32);
				parameters.Add("_proveedorId", newOperation.proveedorId, DbType.Int32);
				parameters.Add("_operatorId", newOperation.operatorId, DbType.Int32);
				parameters.Add("_lineId", newOperation.lineId, DbType.Int32);
				parameters.Add("_productId", newOperation.productId, DbType.Int32);
				parameters.Add("_packagingId", newOperation.packagingId, DbType.Int32);
				parameters.Add("_groupingName", newOperation.groupingName.Trim(), DbType.String);
				parameters.Add("_totalSize", newOperation.totalSize, DbType.Int32);
				parameters.Add("_startDate", newOperation.startDate, DbType.Int32);
				parameters.Add("_endDate", newOperation.endDate, DbType.Int32);
				parameters.Add("_range", newOperation.range, DbType.Int32);
				parameters.Add("_unitsScanned", newOperation.unitsScanned, DbType.Int32);
				parameters.Add("_nboxesPallet", newOperation.nboxesPallet, DbType.Int32);
				parameters.Add("_unitsPerBox", newOperation.unitsPerBox, DbType.Int32);

				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarUnionOperacion", parameters);

				int result = parameters.Get<int>("_response");

				if (result == 0)
				{
					response.messageEsp = "Error al ejecutar el SP de unir operaciones";
					throw new Exception(response.messageEsp);
				} else if (result == -1) {
					response.messageEsp = "Ya existe una operación con el mismo nombre";
					throw new Exception(response.messageEsp);
				}

                for (int i = 1; i < groupings.Count; i++)
                {
					parameters = new DynamicParameters();
					parameters.Add("_typeView", groupingType, DbType.Int32);
					parameters.Add("_operationId", result, DbType.Int32);
					parameters.Add("_operationPId", groupings[0], DbType.Int32);
					parameters.Add("_operationSId", groupings[i], DbType.Int32);
					parameters.Add("_userId", userId, DbType.Int32);
					parameters.Add("_userType", userType, DbType.Int32);
					parameters.Add("_groupingName", newOperation.groupingName.Trim(), DbType.String);

					var res = Consulta<int>("spi_guardarUnionOperacionDetalles", parameters).FirstOrDefault();

                    if (res != 1)
                    {
						response.messageEsp = $"Hubo un error al unir la operación {groupings[i]}.";
						throw new Exception(response.messageEsp);
					}

				}

				//CerrarConexion();
				//log.trace("EX: Guardar del detalle de la operación, groupingPId: " + groupingPId.ToString() + " - groupingSId: " + groupingSId.ToString());
				//CrearConexion(TipoConexion.Mysql);
				//AbrirConexion();

				string querino = "SELECT ";

				response.groupingId = result;

				foreach (var operationDetail in resultDetail)
				{
					DynamicParameters detailParams = new DynamicParameters();
					detailParams.Add("_idOperacion", result, System.Data.DbType.Int32);
					detailParams.Add("_pallet", operationDetail.pallet, System.Data.DbType.String);
					detailParams.Add("_caja", operationDetail.box, System.Data.DbType.String);
					detailParams.Add("_linea", operationDetail.line, System.Data.DbType.String);
					detailParams.Add("_merma", operationDetail.Merma, DbType.Int32);
					detailParams.Add("_etiquetaId", operationDetail.EtiquetaID, DbType.String);
					detailParams.Add("_rangoMin", operationDetail.rangeMin, System.Data.DbType.String);
					detailParams.Add("_rangoMax", operationDetail.rangeMax, System.Data.DbType.String);
					detailParams.Add("_response", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_detalleOperacion", detailParams);
					int operationdetailId = parameters.Get<int>("_response");
					var scannedList = resultScanned.Where(o => o.detailId == operationDetail.detailId).ToList();
					int x = 0;
					foreach(var i in scannedList) {
						string query = "INSERT INTO his_048_operacionDetalleEscaneados VALUES (NULL,'" + operationdetailId + "','" + x + "','" + i.code + "');";
						x++;
						ConsultaCommand<string>(query);
					}
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
		#endregion

		#endregion
	}

	public class ExternalPackedPrintOperationSQL : DBHelperDapper
	{
		#region Properties
		//Datos Operaciones
		public int groupingId;
		public int groupingType;
		public int box;
		public int pallet;

		//Nombre SP a ejecutar
		public string sp;

		//Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ExternalPackedUnionOperationSQL");

		//Variable para guardar la nueva union de operaciones
		#endregion

		#region Constructor
		public ExternalPackedPrintOperationSQL()
		{
			this.groupingId = 0;
			this.groupingType = 0;
			this.box = 0;
			this.pallet = 0;

			this.sp = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para obtener la info de una operacion al generar pdf de la etiqueta QR
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void PrintProcessExt(PrintOperationExtResponse response)
		{
			log.trace("PrintProcessExt");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_operationId", groupingId, DbType.Int32);
				parameters.Add("_type", groupingType, DbType.Int32);

				List<Type> types = new List<Type>();
				types.Add(typeof(OperationDatase));
				types.Add(typeof(OperationDetailse));
				types.Add(typeof(DetailsOrderse));

				//var result = Consulta<ExtPackedOperationInfoPrintSQL>("spc_consultaOperacionesPrint", parameters);
				var res = ConsultaMultiple("spc_consultaOperacionesPrint", types, parameters);

				CerrarConexion();

				if (res.Count <= 2) {
					response.messageEsp = "Error al reimprimir la agrupación";
					return;
				}

				/*
				 * Generar info para el QR del PDF
				 */

				OperationDatase operationDatase = res[0].Cast<OperationDatase>().First();
				List<OperationDetailse> operationDetailse = res[1].Cast<OperationDetailse>().ToList();
				List<DetailsOrderse> DetailsOrderse = res[2].Cast<DetailsOrderse>().ToList();

				response.company = operationDatase.Company;
				response.productName = operationDatase.Product;
				response.packagin = operationDatase.Packaging;
				response.groupingName = operationDatase.Grouping;
				response.instructions = operationDatase.Instructions;
				response.line = operationDatase.Linea;
				response.date = operationDatase.fechaRegistro.Length > 0 ? operationDatase.fechaRegistro : "";
				var operationScanned = 0;
				string lecturas = "";

				switch (groupingType)
                {
					// No, porque 1 es Agrupación
					case 1:
						break;
					// Pallet
					case 2:
						operationScanned = 0;

						// Obtener la cantidad de cajas que pertenecen al pallet solicitado
						List<OperationDetailse> boxesPallet = operationDetailse.FindAll(x => x.Pallet == $"PL-{pallet}").ToList();

						//Obtener la cantidad de unidades totales en el pallet
						operationScanned = (boxesPallet.Count * operationDatase.unitsBox);

						//Obtener el rango inicial y final del pallet.
						string rangoP = $"{boxesPallet[0].RangoMin}-{boxesPallet[boxesPallet.Count - 1].RangoMax}";
						//Obtener el rango anidado de todas las cajas
						lecturas = "";
                        foreach (var lectura in boxesPallet)
                        {
							lecturas += $"{lectura.RangoMin}-{lectura.RangoMax};";
                        }
						lecturas = lecturas.Remove(lecturas.Length - 1);

						response.etiquetaId = boxesPallet[0].EtiquetaID;
						response.ranges = lecturas;
						response.operationScanned = operationScanned;
						break;
					// Caja
					case 3:

						operationScanned = 0;

						// Obtener la caja que quiere re imprimir
						List<OperationDetailse> boxBox = operationDetailse.FindAll(x => (x.Caja == $"Bx-{box}" || x.Caja == $"BX-{box}") && x.Pallet == $"PL-{pallet}").ToList();

						operationScanned = (1 * operationDatase.unitsBox);

						string rangoBox = $"{boxBox[0].RangoMin}-{boxBox[0].RangoMax}";

						lecturas = rangoBox;

						response.etiquetaId = boxBox[0].EtiquetaID;
						response.ranges = lecturas;
						response.operationScanned = operationScanned;

						break;
                    default:
						
                        break;
                }

    //            int ntotal = 1;
				//string nstr = "";

				//ExtPrintQRDetailSQL qrInfo = new ExtPrintQRDetailSQL();

				//response.groupingName = result.First().groupingName;

				///* Agrupación */
				//if (groupingType == 1) {
				//	qrInfo.T = "A";
				//	qrInfo.P = result.First().quantity.ToString();
				//	if (result.First().groupingPId != 0) {
				//		qrInfo.I = result.First().groupingPId.ToString();
				//		qrInfo.F = result.First().groupingSId.ToString();
				//		ntotal = 2;
				//	} else {
				//		qrInfo.I = result.First().minRange.ToString();
				//		qrInfo.F = result.Last().maxRange.ToString();
				//	}
				//	qrInfo.ID = result.First().groupingId.ToString();
				//	response.range = ntotal;
				//}

				///* Pallet */
				//if (groupingType == 2) {
				//	qrInfo.T = "P";
				//	qrInfo.P = result.First().quantity.ToString();
				//	result = result.GroupBy(g => new { g.detailId }).Select(grp => grp.FirstOrDefault()).ToList();
				//	qrInfo.I = result.First().range.Split('-')[0];
				//	qrInfo.F = result.First().range.Split('-')[1];
				//	qrInfo.ID = result.First().groupingId.ToString();

    //                foreach (var item in result) {
				//		if(nstr != item.pallet) {
				//			nstr = item.pallet;
				//			ntotal++;
    //                    }
    //                }
				//	response.range = ntotal;
				//}

				///* Caja */
				//if (groupingType == 3) {
				//	qrInfo.T = "C";
				//	qrInfo.P = result.First().unitsBox.ToString();
				//	qrInfo.I = result.First().range.Split('-')[0];
				//	qrInfo.F = result.First().range.Split('-')[1];
				//	qrInfo.ID = result.First().groupingId.ToString();
				//	foreach (var item in result) {
				//		if(nstr != item.box) {
				//			nstr = item.box;
				//			ntotal++;
    //                    }
    //                }
				//	response.range = ntotal;
				//}
				//response.QRCode = Newtonsoft.Json.JsonConvert.SerializeObject(qrInfo);
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
