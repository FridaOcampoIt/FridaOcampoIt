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
	public class ProvidersSQL: DBHelperDapper
	{
		#region Properties
		public int groupId;

		//Datos Provider
		public int providerId;
		public string providerNumber;
		public string providerName;
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
		public List<ProvidersDataSQL> providers;

		//Datos para asociar productos
		public List<ProductProviderDataSQL> products;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("ProvidersSQL");

		#endregion

		#region Constructor
		public ProvidersSQL()
		{
			this.groupId = 0;

			this.providerId = 0;
			this.providerNumber = String.Empty;
			this.providerName = String.Empty;
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

			this.providers = new List<ProvidersDataSQL>();
			this.products = new List<ProductProviderDataSQL>();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar los proveedores realizadas en la Linea de Producción
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ProvidersDataSQL> SearchProviders()
		{
			log.trace("SearchProvidersSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_providerName", providerName, DbType.String);
				parameters.Add("_businessName", businessName, DbType.String);
				parameters.Add("_opc", opc, DbType.Boolean);
				parameters.Add("_userId", userId, DbType.Int32);

				List<ProvidersDataSQL> response = Consulta<ProvidersDataSQL>("spc_consultaProveedoresEmpaqEtiq", parameters);
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
		/// Metodo para consultar los datos de un proveedor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ProvidersDataSQL> SearchProviderData()
		{
			log.trace("SearchProviderDataSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_providerId", providerId, DbType.Int32);

				List<ProvidersDataSQL> response = Consulta<ProvidersDataSQL>("spc_consultaProveedorEmpaqEtiqDatos", parameters);
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
		/// Metodo para guardar / actualizar provedores
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SaveProvider()
		{
			log.trace("SaveProviderSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				if (providerId > 0)
					parameters.Add("_providerId", providerId, DbType.String);
				parameters.Add("_providerNumber", providerNumber, DbType.String);
				parameters.Add("_providerName", providerName, DbType.String);
				parameters.Add("_businessName", businessName, DbType.String);
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_phone", phone, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(sp, parameters);

				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP de " + (providerId > 0 ? "editar" : "agregar" ) + " provedor");
				else if (response == -1)
					throw new Exception("Ya existe un proveedor con el mismo número de proveedor");
				else if (response == -2)
					throw new Exception("Ya existe un proveedor con el mismo nombre");
				else if (response == -3)
					throw new Exception("Ya existe un proveedor con la misma razón social");

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
		/// Metodo para eliminar provedores
		/// Desarollador: Iván Gutiérrez
		/// </summary>
		public void DeleteProvider()
		{
			log.trace("DeleteProviderSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_providerId", providerId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarProveedorEmpaqEtiq", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("No se puede eliminar un proveedor con estatus activo");

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
					parameters.Add("_providerId", product.providerId, DbType.Int32);
					parameters.Add("_productId", product.productId, DbType.Int32);
					parameters.Add("_companyId", product.companyId, DbType.Int32);
					parameters.Add("_packagingId", product.packagingId, DbType.Int32);
					parameters.Add("_rawMaterial", product.rawMaterial.Trim(), DbType.String);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarAsociarProductosProveedor", parameters);

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
		/// Metodo para consultar los productos asociados de un proveedor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<ProductProviderDataSQL> SearchProductProvider()
		{
			log.trace("SearchProductProviderSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_providerId", providerId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_packagingId", packagingId, DbType.Int32);
				parameters.Add("_rawMaterial", rawMaterial, DbType.String);
				parameters.Add("_opc", topc, DbType.Int32);

				List<ProductProviderDataSQL> response = Consulta<ProductProviderDataSQL>("spc_consultaAsociarProductosProveedor", parameters);
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
