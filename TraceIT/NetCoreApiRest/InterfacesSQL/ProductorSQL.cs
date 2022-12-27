using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Address;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using WSTraceIT.Models.Response;

namespace WSTraceIT.InterfacesSQL
{
	public class ProductorSQL: DBHelperDapper
	{
		#region Properties
		public int productorId;
		public Boolean activo;
		public int numeroProductor;
		public string nombreProductor;
		public string nombreContacto;
		public string apellidoContacto;
		public string telefonoContacto;
		public string direccion;
		public string latitud;
		public string longitud;
		public int usuarioCreador;
		public DateTime fechaCreacion;
		public DateTime fechaModificacion;
		public int usuarioModificador;
		public int companiaId;
		public string nombreRancho;
		public Boolean estatus;
		public List<int> acopiosId;
		public List<int> auxAcopiosId; //Son los acopios auxiliares, si se recibe arreglo (futuras funciones)
		public string nombreProductorNumeroNombreAcopio;
		private LoggerD4 log = new LoggerD4("ProductorSQL");
		#endregion

		#region Constructor
		public ProductorSQL()
		{
			this.productorId = 0;
			this.activo = true;
			this.numeroProductor = 0;
			this.nombreProductor = String.Empty;
			this.nombreContacto = String.Empty;
			this.apellidoContacto = String.Empty;
			this.telefonoContacto = String.Empty;
			this.direccion = String.Empty;
			this.latitud = String.Empty;
			this.longitud = String.Empty;
			this.usuarioCreador = 0;
			this.fechaCreacion = new DateTime();
			this.fechaModificacion = new DateTime();
			this.usuarioModificador = 0;
			this.companiaId = 0;
			this.nombreRancho = String.Empty;
			this.estatus = true;
			this.acopiosId = new List<int>();
			this.auxAcopiosId = new List<int>();
			this.nombreProductorNumeroNombreAcopio = String.Empty;
		}
		#endregion

		#region Public method
		/// <summary>
		/// Metodo para guardar los productor
		/// Desarrollador: Hernán Gómez
		/// Fecha: 06-Mayo-2022
		/// </summary>
		public void SaveProductor()
		{
			log.trace("SaveProductor");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();

				parameters.Add("_activo", activo, DbType.Boolean);
				parameters.Add("_numeroProductor", numeroProductor, DbType.Int32);
				parameters.Add("_nombreProductor", nombreProductor, DbType.String);
				parameters.Add("_nombreContacto", nombreContacto, DbType.String);
				parameters.Add("_apellidoContacto", apellidoContacto, DbType.String);
				parameters.Add("_telefonoContacto", telefonoContacto, DbType.String);
				parameters.Add("_direccion", direccion, DbType.String);
				parameters.Add("_latitud", latitud, DbType.String);
				parameters.Add("_longitud", longitud, DbType.String);
				parameters.Add("_usuarioCreador", usuarioCreador, DbType.Int32);
				parameters.Add("_companiaId", companiaId, DbType.Int32);
				parameters.Add("_nombreRancho", nombreRancho, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarProductor", parameters);
				int response = parameters.Get<int>("_response");
				
				if (response > 0){
					foreach (int acopio in acopiosId)
					{
						string query = $"INSERT INTO rel_052_productoracopio (FK_AcopioId, FK_ProductorId) VALUES({acopio},{response});";
						ConsultaCommand<string>(query);
					}
				}
				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
				 
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

		#region Public method
		/// <summary>
		/// Metodo para traer listado de productores, busqueda por numero, nombre, rancho y acopio
		/// Desarrollador: Hernán Gómez
		/// Fecha: 06-Mayo-2022
		/// </summary>
		public List<searchProductorResponse> searchProductores()
		{
			log.trace("searchProductores");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companiaId", companiaId, DbType.Int32);
				parameters.Add("_nombreProductorNumeroNombreAcopio", nombreProductorNumeroNombreAcopio, DbType.String);

				List<searchProductorResponse> resp = Consulta<searchProductorResponse>("spc_consultaProductorCompaniaByIdParams", parameters);
				return resp;
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


		#region Public method
		/// <summary>
		/// Metodo para traer detalle de productor por id
		/// Desarrollador: Hernán Gómez
		/// Fecha: 06-Mayo-2022
		/// </summary>
		public searchProductorByIdResponse searchProductorById()
		{
			log.trace("searchProductorById");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_productorId", productorId, DbType.Int32);

				var resp = Consulta<searchProductorByIdResponse>("spc_consultaProductorById", parameters).FirstOrDefault();
				return resp;
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


		#region Public method
		/// <summary>
		/// Metodo para actualizar productor por id
		/// Desarrollador: Hernán Gómez
		/// Fecha: 06-Mayo-2022
		/// </summary>
		public void UpdateProductorById()
		{
			log.trace("searchProductorById");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();
				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_productorId", productorId, DbType.Int32);
				parameters.Add("_activo", activo, DbType.Boolean);
				parameters.Add("_numeroProductor", numeroProductor, DbType.Int32);
				parameters.Add("_nombreProductor", nombreProductor, DbType.String);
				parameters.Add("_nombreRancho", nombreRancho, DbType.String);
				parameters.Add("_direccion", direccion, DbType.String);
				parameters.Add("_longitud", longitud, DbType.String);
				parameters.Add("_latitud", latitud, DbType.String);
				parameters.Add("_usuarioModificador", usuarioModificador, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_actualizaProductorById", parameters);
				int response = parameters.Get<int>("_response");
				if(response != 0)
                {
						//Nos regresa un array con los que ya no se encuentren asociados a este productor
						var viejos = auxAcopiosId.Except(acopiosId);
						//Nos regresa un array con los acopios nuevos al que pertenece
						var nuevos = acopiosId.Except(auxAcopiosId);
						if(nuevos != null) {
							foreach (int nuevo in nuevos)
							{
								//Creamos los registros para los nuevos productores que seran ligados a los acopios
								string query = $"INSERT INTO rel_052_productoracopio (FK_AcopioId, FK_ProductorId) VALUES({ nuevo },{productorId});";
								ConsultaCommand<string>(query);
							}
						}
						if (viejos != null)
						{
							//Eliminamos los registros de la tabla pivote
							foreach (int viejo in viejos)
							{
								string query = $"DELETE FROM rel_052_productoracopio  WHERE FK_AcopioId = {viejo} and FK_ProductorId = {productorId};";
								ConsultaCommand<string>(query);
							}
						}
				}
				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
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


		#region Public method
		/// <summary>
		/// Metodo para eliminar un productor (Tiene que estar inactivo)
		/// Desarrollador: Hernán Gómez
		/// Fecha: 06-Mayo-2022
		/// </summary>
		public void DeleteProductorById()
		{
			log.trace("DeleteProductorById");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();
				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_productorId", productorId, DbType.Int32);
				parameters.Add("_usuarioModificador", usuarioModificador, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarProductorById", parameters);
				int response = parameters.Get<int>("_response");
				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
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
	}
}
