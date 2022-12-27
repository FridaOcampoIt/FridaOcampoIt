using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using System.Reflection.Emit;

namespace WSTraceIT.InterfacesSQL
{
	public class LabelsQRSQL : DBHelperDapper
	{
		#region Properties
		//Datos Etiqueta qr
		public int labelId;
		public string name;
		public int orientation;
		public bool grouper;
		public int nChildren;
		public int topPrimary;
		public int topSecondary;
		public int rightPrimary;
		public int rightSecondary;
		public int bottomPrimary;
		public int bottomSecondary;
		public int leftPrimary;
		public int leftSecondary;
		public int companyId;

		public int userId;
		public int opc;

		//Nombre SP a ejecutar
		public string sp;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("LabelsQRSQL");
		#endregion

		#region Constructor
		public LabelsQRSQL()
		{
			this.labelId = 0;
			this.name = String.Empty;
			this.orientation = 0;
			this.grouper = false;
			this.nChildren = 0;
			this.topPrimary = 0;
			this.topSecondary = 0;
			this.rightPrimary = 0;
			this.rightSecondary = 0;
			this.bottomPrimary = 0;
			this.bottomSecondary = 0;
			this.leftPrimary = 0;
			this.leftSecondary = 0;
			this.companyId = 0;

			this.userId = 0;
			this.opc = 0;

			this.sp = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar las etiquetas qr
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<LabelsQRDataSQL> SearchLabelsQR()
		{
			log.trace("SearchLabelsQRSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_opc", opc, DbType.Int32);

				List<LabelsQRDataSQL> response = Consulta<LabelsQRDataSQL>("spc_consultaEtiquetasQR", parameters);
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
		/// Metodo para consultar las etiquetas qr para los combos (selects)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<LabelsQRDataComboSQL> SearchLabelsQRCombo()
		{
			log.trace("SearchLabelsQRComboSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_opc", opc, DbType.Int32);

				List<LabelsQRDataComboSQL> response = Consulta<LabelsQRDataComboSQL>("spc_consultaEtiquetasQR", parameters);
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
		/// Metodo para consultar los datos de una etiqueta qr
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<LabelsQRDataSQL> SearchLabelQRData()
		{
			log.trace("SearchLabelQRDataSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_labelId", labelId, DbType.Int32);

				List<LabelsQRDataSQL> response = Consulta<LabelsQRDataSQL>("spc_consultaEtiquetasQRDatos", parameters);
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
		/// Metodo para guardar / actualizar etiquetas qr
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SaveLabelQR()
		{
			log.trace("SaveLabelQRSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				if (labelId > 0)
					parameters.Add("_labelId", labelId, DbType.Int32);
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_orientation", orientation, DbType.Int32);
				parameters.Add("_grouper", grouper, DbType.Boolean);
				parameters.Add("_nChildren", nChildren, DbType.Int32);
				parameters.Add("_topPrimary", topPrimary, DbType.Int32);
				parameters.Add("_topSecondary", topSecondary, DbType.Int32);
				parameters.Add("_rightPrimary", rightPrimary, DbType.Int32);
				parameters.Add("_rightSecondary", rightSecondary, DbType.Int32);
				parameters.Add("_bottomPrimary", bottomPrimary, DbType.Int32);
				parameters.Add("_bottomSecondary", bottomSecondary, DbType.Int32);
				parameters.Add("_leftPrimary", leftPrimary, DbType.Int32);
				parameters.Add("_leftSecondary", leftSecondary, DbType.Int32);
				parameters.Add("_companyId", companyId, DbType.Int32);
				//parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(sp, parameters);

				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP de " + (labelId > 0 ? "editar" : "agregar") + " etiquetas qr");
				else if (response == -1)
					throw new Exception("Ya existe una etiqueta con el mismo qr");

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
		/// Metodo para eliminar etiquetas qr
		/// Desarollador: Iván Gutiérrez
		/// </summary>
		public void DeleteLabelQR()
		{
			log.trace("DeleteLabelQRSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_labelId", labelId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarEtiquetaQR", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("No se puede eliminar una etiqueta con dependecias de otros registros");

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
}
