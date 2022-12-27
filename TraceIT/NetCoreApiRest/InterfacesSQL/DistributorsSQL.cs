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
	public class DistributorsSQL : DBHelperDapper
	{
		#region Properties
		public int groupId;

		//Datos Distribuidor
		public int distributorId;
		public string distributorNumber;
		public string distributorName;
		public string businessName;
		public string email;
		public string phone;
		public bool status;
		public int userId;
		public bool opc;
		public int topc;

		// Asociar Productos
		public string rawMaterial;
		public int productId;
		public int companyId;
		public int packagingId;

		//Nombre SP a ejecutar
		public string sp;

		//Datos del filtro para Empaques y Etiquetas
		public List<DistributorsDataSQL> distributors;

		//Datos para asociar productos
		public List<ProductDistributorDataSQL> products;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("DistributorsSQL");
		#endregion

		#region Constructor
		public DistributorsSQL()
		{
			this.groupId = 0;

			this.distributorId = 0;
			this.distributorNumber = String.Empty;
			this.distributorName = String.Empty;
			this.businessName = String.Empty;
			this.email = String.Empty;
			this.phone = String.Empty;
			this.status = false;
			this.userId = 0;
			this.opc = false;
			this.topc = 0;

			this.rawMaterial = String.Empty;
			this.productId = 0;
			this.companyId = 0;
			this.packagingId = 0;

			this.sp = String.Empty;

			this.distributors = new List<DistributorsDataSQL>();
			this.products = new List<ProductDistributorDataSQL>();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar los distribuidores
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<DistributorsDataSQL> SearchDistributors()
		{
			log.trace("SearchDistributorsSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_distributorName", distributorName, DbType.String);
				parameters.Add("_businessName", businessName, DbType.String);
				parameters.Add("_opc", opc, DbType.Boolean);
				parameters.Add("_userId", userId, DbType.Int32);

				List<DistributorsDataSQL> response = Consulta<DistributorsDataSQL>("spc_consultaDistribuidoresEmpaqEtiq", parameters);
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
		/// Metodo para consultar los datos de un distribuidor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<DistributorsDataSQL> SearchDistributorData()
		{
			log.trace("SearchDistributorDataSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_distributorId", distributorId, DbType.Int32);

				List<DistributorsDataSQL> response = Consulta<DistributorsDataSQL>("spc_consultaDistribuidorEmpaqEtiqDatos", parameters);
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
		/// Metodo para guardar / actualizar distribuidores
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SaveDistributor()
		{
			log.trace("SaveDistributorSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				if (distributorId > 0)
					parameters.Add("_distributorId", distributorId, DbType.Int32);
				parameters.Add("_distributorNumber", distributorNumber, DbType.String);
				parameters.Add("_distributorName", distributorName, DbType.String);
				parameters.Add("_businessName", businessName, DbType.String);
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_phone", phone, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(sp, parameters);

				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP de " + (distributorId > 0 ? "editar" : "agregar") + " distribuidor");
				else if (response == -1)
					throw new Exception("Ya existe un distribuidor con el mismo número de distribuidor");
				else if (response == -2)
					throw new Exception("Ya existe un distribuidor con el mismo nombre");
				else if (response == -3)
					throw new Exception("Ya existe un distribuidor con la misma razón social");

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
		/// Metodo para eliminar distribuidores
		/// Desarollador: Iván Gutiérrez
		/// </summary>
		public void DeleteDistributor()
		{
			log.trace("DeleteDistributorSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_distributorId", distributorId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarDistributorEmpaqEtiq", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("No se puede eliminar un distribuidor con estatus activo");

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
					parameters.Add("_distributorId", product.distributorId, DbType.Int32);
					parameters.Add("_productId", product.productId, DbType.Int32);
					parameters.Add("_companyId", product.companyId, DbType.Int32);
					parameters.Add("_packagingId", product.packagingId, DbType.Int32);
					parameters.Add("_rawMaterial", product.rawMaterial.Trim(), DbType.String);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarAsociarProductosDistribuidor", parameters);

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
		/// Metodo para consultar los productos asociados de un distribuidor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ProductDistributorDataSQL> SearchProductDistributor()
		{
			log.trace("SearchProductDistributorSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_distributorId", distributorId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_packagingId", packagingId, DbType.Int32);
				parameters.Add("_rawMaterial", rawMaterial, DbType.String);
				parameters.Add("_opc", topc, DbType.Int32);

				List<ProductDistributorDataSQL> response = Consulta<ProductDistributorDataSQL>("spc_consultaAsociarProductosDistribuidor", parameters);
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
}
