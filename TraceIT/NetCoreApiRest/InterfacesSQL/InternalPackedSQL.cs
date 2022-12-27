using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Response;
using NetCoreApiRest.Utils;
using Microsoft.EntityFrameworkCore.Internal;

namespace WSTraceIT.InterfacesSQL
{
	public class InternalPackedSQL: DBHelperDapper
	{
		#region Properties
		//Datos Empacador
		public int packedId;
		public string packedNumber;
		public string packedName;
		public int merma;
		public string email;
		public string password;
		public string phone;
		public bool status;
		public int userId;
		public int companyId;
		public bool opc;
		public int type;
		public int typeUsuario;
		public List<int> companiasId;
		public int companyIdSearch;
		public List<int> auxGetCompany;

		//Nombre SP a ejecutar
		public string sp;

		//Datos del filtro para los Empacadores
		public List<InternalPackedDataSQL> packedList;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("InternalPackedSQL");
		#endregion

		#region Constructor
		public InternalPackedSQL()
		{
			this.packedId = 0;
			this.packedNumber = String.Empty;
			this.packedName = String.Empty;
			this.merma = 0;
			this.email = String.Empty;
			this.password = String.Empty;
			this.phone = String.Empty;
			this.status = false;
			this.userId = 0;
			this.companyId = 0;
			this.opc = false;
			this.type = 0;

			this.sp = String.Empty;
			this.companiasId = new List<int>();
			this.packedList = new List<InternalPackedDataSQL>();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar los empacadores
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		/// Modificación de consultar empacdores (Se dejan variables comentadas, pero se comentan, para no tener afectaciones) 
		public List<InternalPackedDataSQL> SearchPackedList()
		{
			log.trace("SearchInternalPackedList");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				//Pasamos el nombre del empacador, para buscarlo por ese valor (es opcional en el sp) 
				parameters.Add("_packedName", packedName, DbType.String);
				//parameters.Add("_merma", merma, DbType.String);
				//No sirve para nada
				//parameters.Add("_opc", opc, DbType.Boolean); //fase
				
				//Pasamos el tipo de empacador 1 = Externo / 2 = interno
				parameters.Add("_type", 2, DbType.Int32);

				//Pasamos el tipo de compañia (si tiene se pasa el id, si no se manda 0 y regresa el listado para usuario TraceIT) 
				parameters.Add("_companyId", companyId, DbType.Int32);

				//Asignamos el id de la compañia que se esta buscando
				parameters.Add("_companyIdSearch", companyIdSearch, DbType.Int32);

				//Tipo de usuario
				parameters.Add("_typeUsuario", typeUsuario, DbType.Int32);
				//Usuario de empacador
				parameters.Add("_userId", userId, DbType.Int32);

				//Tampoco es necesario
				//parameters.Add("_userId", userId, DbType.Int32); //11
				//Tampoco es necesario
				//parameters.Add("_isEmpc", type, DbType.Int32); //0

				List<InternalPackedDataSQL> response = Consulta<InternalPackedDataSQL>("spc_consultaEmpacadorEmpaqEtiq", parameters);
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
		/// Mejora e implementación de arrays en la consulta de un registro (compañias que pertenece el empacador)
		public List<InternalPackedDataSQL> SearchPackedData()
		{
			log.trace("SearchInternalPackedDataSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				List<InternalPackedDataSQL> response = Consulta<InternalPackedDataSQL>("spc_consultaEmpacadorEmpaqEtiqDatos", parameters);
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
			log.trace("SaveInternalPackedSQL");
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
				parameters.Add("_phone", "", DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_type", 2, DbType.Int32);
				if (packedId == 0)
					parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(sp, parameters);
				int response = parameters.Get<int>("_response");
				if(packedId > 0 && companyId > 0){
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
				else if (sp.Equals("spu_edicionEmpacadorEmpaqEtiq")){
					//Nos regresa un array con las compañias que ya no se encuentren listadas en la petición
					var viejos = auxGetCompany.Except(companiasId);
					//Nos regresa un array con las compañias nuevas que se agregaron en la petición
					var nuevos = companiasId.Except(auxGetCompany);
					foreach (int nuevo in nuevos){
						//Creamos los registros para las nuevas compañias que seran ligadas (Solo usuario TraceIt)
						string query = $"INSERT INTO rel_051_empacadorCompania (CompaniaId, EmpacadorId) VALUES({ nuevo },{packedId});";
						ConsultaCommand<string>(query);
					}
					//Eliminamos los registros de la tabla pivote (Eliminados por usuario TraceIt)
					foreach (int viejo in viejos){
						string query = $"DELETE FROM rel_051_empacadorCompania  WHERE CompaniaId = {viejo} and EmpacadorId = {packedId};";
						ConsultaCommand<string>(query);
					}

				}
				else if (response > 0){
					foreach(int compania in companiasId){
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
			log.trace("DeleteInternalPackedSQL");
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

	public class InternalExternalPackedSQL : DBHelperDapper
	{
		#region Properties
		public int packedId;
		public string packedName;
		public int companyId;
		public int type;

		//Nombre SP a ejecutar
		public string sp;

		//Datos del filtro para los Empacadores
		public List<InternalExternalPackedDataSQL> packedList;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("InternalPackedSQL");
		#endregion

		#region Constructor
		public InternalExternalPackedSQL()
        {
			this.packedId = 0;
			this.packedName = String.Empty;
			this.type = 0;
			this.companyId = 0;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar los empacadores Internos/Externos por compañia 
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<InternalExternalPackedDataSQL> SearchInternalExternalPackedList()
		{
			log.trace("SearchInternalPackedList");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				//Pasamos el id de la compañia para traernos empacadores Internos/Externos de empacadores 
				parameters.Add("_companyId", companyId, DbType.String);

				List<InternalExternalPackedDataSQL> response = Consulta<InternalExternalPackedDataSQL>("spc_consultaEmpacadoresCompania", parameters);
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

	public class InternalPackedOperatorSQL : DBHelperDapper
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
		private LoggerD4 log = new LoggerD4("InternalPackedOperatorSQL");
		#endregion

		#region Constructor
		public InternalPackedOperatorSQL()
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
		public List<InternalPackedOperatorDataSQL> SearchPackedOperatorsList()
		{
			log.trace("SearchPackedOperatorsList");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_addressId", addressId, DbType.Int32);
				parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);

				List<InternalPackedOperatorDataSQL> response = Consulta<InternalPackedOperatorDataSQL>("spc_consultaOperadoresEmpaqEtiq", parameters);
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
		public List<InternalPackedOperatorDataSQL> SearchPackedOperatorData()
		{
			log.trace("SearchPackedOperatorDataSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_operatorId", operatorId, DbType.Int32);

				List<InternalPackedOperatorDataSQL> response = Consulta<InternalPackedOperatorDataSQL>("spc_consultaOperadoresEmpaqEtiqDatos", parameters);
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
					parameters.Add("_type", 2, DbType.Int32);
				}
				parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);

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

	public class InternalPackedProductionLineSQL : DBHelperDapper
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
		private LoggerD4 log = new LoggerD4("InternalPackedProdLineSQL");
		#endregion

		#region Constructor
		public InternalPackedProductionLineSQL()
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
		public List<InternalPackedProdLineDataSQL> SearchPackedProdLinesList()
		{
			log.trace("SearchPackedProdLinesList");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedId", packedId, DbType.Int32);

				List<InternalPackedProdLineDataSQL> response = Consulta<InternalPackedProdLineDataSQL>("spc_consultaLineasProduccionEmpaqEtiq", parameters);
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
		public List<InternalPackedProdLineDataSQL> SearchPackedProdLineData()
		{
			log.trace("SearchPackedProdLineDataSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_lineId", lineId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);

				List<InternalPackedProdLineDataSQL> response = Consulta<InternalPackedProdLineDataSQL>("spc_consultaLineaProduccionEmpaqEtiqDatos", parameters);
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
			log.trace("SavePackedProdLineSQL");
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
					parameters.Add("_type", 2, DbType.Int32);
				}
				parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);

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
			log.trace("DeletePackedProdLineSQL");
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

	public class ProductInternalPackedSQL : DBHelperDapper
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
		public List<ProductInternalPackedDataSQL> products;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ProductExternalPackedSQL");
		#endregion

		#region Constructor
		public ProductInternalPackedSQL()
		{
			this.packedId = 0;
			this.userId = 0;
			this.topc = 0;

			this.rawMaterial = String.Empty;
			this.productId = 0;
			this.companyId = 0;
			this.packagingId = 0;

			this.products = new List<ProductInternalPackedDataSQL>();
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
		public List<ProductInternalPackedDataSQL> SearchProductPacked()
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

				List<ProductInternalPackedDataSQL> response = Consulta<ProductInternalPackedDataSQL>("spc_consultaAsociarProductosEmpacadoIE", parameters);
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

	public class InternalPackedBoxManagementSQL : DBHelperDapper
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
		public List<IntPackedBoxManagDataSQL> boxesData;
		public List<IntPackedBoxManagDataDetailSQL> boxesDataDetail;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("InternalPackedBoxManagementSQL");
		#endregion

		#region Constructor
		public InternalPackedBoxManagementSQL()
		{
			this.packedId = 0;
			this.userId = 0;
			this.topc = 0;
			this.companiaId = 0;

			this.groupingId = 0;
			this.addressId = 0;
			this.productId = 0;
			this.pallet = 0;
			this.box = 0;
			this.typeView = 0;
			this.dateStart = default(DateTime);
			this.dateEnd = default(DateTime);
			this.searchfield = String.Empty;

			this.boxesData = new List<IntPackedBoxManagDataSQL>();
			this.boxesDataDetail = new List<IntPackedBoxManagDataDetailSQL>();
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
			log.trace("SearchPackedBoxManagementSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedId", packedId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				DateTime _dateStart = Convert.ToDateTime(dateStart);
				parameters.Add("_dateStart", _dateStart.ToUniversalTime().ToString("u"), DbType.String);
				DateTime _dateEnd = Convert.ToDateTime(dateEnd);
				parameters.Add("_dateEnd", _dateEnd.ToUniversalTime().ToString("u"), DbType.String);
				parameters.Add("_companiaId", companiaId, DbType.Int32);
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
		public List<IntPackedBoxManagDataSQL> SearchPackedBoxManagementFilter()
		{
			log.trace("SearchPackedBoxManagementFSQL");
			try
			{
				var result = new List<IntPackedBoxManagDataSQL>();
				var tempresult = new List<IntPackedBoxManagDataSQL>();
				var tem = new List<IntPackedBoxManagDataSQL>();

				/*if (searchfield != "") {
					tem = (from ts in boxesData
						   select new IntPackedBoxManagDataSQL
						   {
							   groupingId = ts.groupingId,
							   groupingName = ts.groupingName,
							   groupingType = 1,
							   pallet = (from ts2 in boxesData
										 where ts2.groupingId == ts.groupingId
										 group ts2 by ts2.pallet into newGroup
										 select newGroup).ToList().Count.ToString(),
							   box = (from tsbox in boxesData
									  where tsbox.groupingId == ts.groupingId
									  select new IntPackedBoxManagDataSQL
									  {
										  box = tsbox.box
									  }).ToList().Count.ToString(),
							   productId = ts.productId,
							   productName = ts.productName,
							   quantity = ts.quantity,
							   isGroup = ts.isGroup
						   }).ToList();

					tem = tem.GroupBy(g => g.groupingId).Select(grp => grp.FirstOrDefault()).ToList();
					
					result = (from ts in tem
							  where ts.groupingName.Contains(searchfield)
									|| ts.pallet == searchfield
									|| ts.box == searchfield
									|| ts.productName.Contains(searchfield)
									|| ts.quantity.ToString() == searchfield
							  select ts).ToList();

					if (result.Count > 0) {
						return result;
					} else {
						tem = (from ts in boxesData
									  select new IntPackedBoxManagDataSQL
									  {
										  groupingId = ts.groupingId,
										  groupingName = ts.groupingName,
										  groupingType = 2,
										  pallet = ts.pallet,
										  box = (from tsbox in boxesData
												 where tsbox.groupingId == ts.groupingId
												 select new IntPackedBoxManagDataSQL
												 {
													 box = tsbox.box
												 }).ToList().Count.ToString(),
										  productId = ts.productId,
										  productName = ts.productName,
										  quantity = ts.quantity,
										  isGroup = ts.isGroup
									  }).ToList();

						tem = tem.GroupBy(g => new { g.groupingId, g.pallet }).Select(grp => grp.FirstOrDefault()).ToList();

						result = (from ts in tem
								  where ts.pallet.Contains(searchfield)
										|| ts.box == searchfield
										|| ts.quantity.ToString() == searchfield
								  select ts).ToList();

						if (result.Count > 0) {
							return result;
						}
						else {
							tem = (from ts in boxesData
								   select new IntPackedBoxManagDataSQL
								   {
									   groupingId = ts.groupingId,
									   groupingName = ts.groupingName,
									   groupingType = 3,
									   pallet = ts.pallet,
									   box = ts.box,
									   productId = ts.productId,
									   productName = ts.productName,
									   quantity = ts.quantity,
									   isGroup = ts.isGroup
								   }).ToList();

							return result = (from ts in tem
											  where ts.box.Contains(searchfield)
											  select ts).ToList();
							
						}
					}
				}*/

				/*
				 * Agrupación
				 */
				if (typeView == 1)
				{
					tempresult = (from ts in boxesData
								  select new IntPackedBoxManagDataSQL
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
											 select new IntPackedBoxManagDataSQL
											 {
												 box = tsbox.box
											 }).ToList().Count.ToString(),
									  productId = ts.productId,
									  productName = ts.productName,
									  quantity = boxesData.Where(x => x.groupingId == ts.groupingId).Select(x => x.quantity).Sum(),
									  isGroup = ts.isGroup
								  }).ToList();

					result = tempresult.GroupBy(g => g.groupingId).Select(grp => grp.FirstOrDefault()).ToList();
				}

				/*
				 * Pallet
				 */
				if (typeView == 2)
				{
					tempresult = (from ts in boxesData
								  select new IntPackedBoxManagDataSQL
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
								  select new IntPackedBoxManagDataSQL
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
		public void SearchPackedBoxManagementDetail(SearchIntPackedBoxManagementDetailResponse response)
		{
			log.trace("SearchPackedBoxManagementDetailSQL");
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
		public List<IntPackedBoxManagDataDetailSQL> SearchPackedBoxManagementDetailFilter()
		{
			log.trace("SearchPackedBoxManagementDetailFSQL");
			try
			{
				var result = new List<IntPackedBoxManagDataDetailSQL>();
				var tempresult = new List<IntPackedBoxManagDataDetailSQL>();
				var tem = new List<IntPackedBoxManagDataDetailSQL>();

				/*
				 * Agrupación
				 */
				if (typeView == 1)
				{
					tempresult = (from ts in boxesDataDetail
								  select new IntPackedBoxManagDataDetailSQL
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
								  select new IntPackedBoxManagDataDetailSQL
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
								  select new IntPackedBoxManagDataDetailSQL
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

	public class InternalPackedArmingReportSQL : DBHelperDapper
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
		private LoggerD4 log = new LoggerD4("InternalPackedArmingReportSQL");
		#endregion

		#region Constructor
		public InternalPackedArmingReportSQL()
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
		public SearchIntPackedArmingReportResponse SearchArmingReport()
		{
			log.trace("SearchArmingReportSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(IntPackedArmingReporDataSQL));
				types.Add(typeof(IntPackedArmingReporDataSQL));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_typeCompany", typeCompany, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_addressId", addressId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				//parameters.Add("_searchGeneric", searchGeneric, DbType.String);
				parameters.Add("_dateStart", dateStart, DbType.String);
				parameters.Add("_dateEnd", dateEnd, DbType.String);parameters.Add("_auxInfo", "CAJA", DbType.String);
				SearchIntPackedArmingReportResponse response = new SearchIntPackedArmingReportResponse();
				var responseSQLCaja = ConsultaMultiple("spc_consultaReporteArmadosEmpacadoIE", types, parameters);

				/* 
				 *CAJAS
				*/
				response.infoDataBoxes = responseSQLCaja[0].Cast<IntPackedArmingReporDataCountSQL>().ToList();


				/*
				 * PALLET
				*/
				parameters.Add("_auxInfo", "PALLET", DbType.String);
				var responseSQLPallet = ConsultaMultiple("spc_consultaReporteArmadosEmpacadoIE", types, parameters);
				response.infoDataPallet = responseSQLPallet[0].Cast<IntPackedArmingReporDataCountSQL>().ToList();


				/*
				 * Mermas
				 */
				parameters.Add("_auxInfo", "MERMA", DbType.String);
				var responseSQLMerma = ConsultaMultiple("spc_consultaReporteArmadosEmpacadoIE", types, parameters);
				response.infoDataWaste = responseSQLMerma[0].Cast<IntPackedArmingReporDataCountSQL>().ToList();

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
