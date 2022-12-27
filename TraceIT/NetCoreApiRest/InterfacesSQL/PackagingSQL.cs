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
	public class PackagingSQL : DBHelperDapper
	{
		#region Properties

		//Datos Embalaje
		public int packagingId;
		public string packagingType;
		public string readingType;
		public int boxLabelType;
		public int boxLabelPallet;
		public int unitsPerBox;
		public int copiesPerBox;
		public int linesPerBox;
		public decimal grossWeightPerBox;
		public string dimensionsWeightPerBox;
		public int boxesPerPallet;
		public int copiesPerPallet;
		public decimal grossWeightPerPallet;
		public string dimensionsPerPallet;
		public string instructionsWarnings;
		public List<int> empacadoresId;
		public List<int> auxEmpacadoresId;

		public int familyId;

		//Tipos de embalaje reproceso
		public bool isReproceso;

		//Nombre SP a ejecutar
		public string sp;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("PackagingSQL");
		#endregion

		#region Constructor
		public PackagingSQL()
		{
			this.packagingId = 0;

			this.packagingType = String.Empty;
			this.readingType = String.Empty;
			this.boxLabelType = 0;
			this.boxLabelPallet = 0;
			this.unitsPerBox = 0;
			this.copiesPerBox = 0;
			this.linesPerBox = 0;
			this.grossWeightPerBox = 0;
			this.dimensionsWeightPerBox = "";
			this.boxesPerPallet = 0;
			this.copiesPerPallet = 0;
			this.grossWeightPerPallet = 0;
			this.dimensionsPerPallet = "";
			this.instructionsWarnings = String.Empty;

			this.familyId = 0;

			this.isReproceso = false;


			this.sp = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar los embalajes de una familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<PackagingFamilySQL> SearchPackagingFamilies()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_familyId", familyId, DbType.Int32);
				parameters.Add("_packagingId", packagingId, DbType.Int32);

				sp = !isReproceso ? "spc_consultaEmbalajeFamilia" : "spc_consultaEmbalajeReprocesoFamilia";
				Console.WriteLine($"ESTE ES EL SP: {sp}");
				List<PackagingFamilySQL> response = Consulta<PackagingFamilySQL>(sp, parameters);
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
		/// Metodo para guardar / actualizar embalaje de una familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SavePackagingFamily()
		{
			log.trace("SavePackagingFamilySQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				if (packagingId > 0)
					parameters.Add("_packagingId", packagingId, DbType.Int32);
				parameters.Add("_packagingType", packagingType, DbType.String);
				parameters.Add("_readingType", readingType, DbType.String);
				parameters.Add("_boxLabelType", boxLabelType, DbType.Int32);
				parameters.Add("_boxLabelPallet", boxLabelPallet, DbType.Int32);
				parameters.Add("_unitsPerBox", unitsPerBox, DbType.Int32);
				parameters.Add("_copiesPerBox", copiesPerBox, DbType.Int32);
				parameters.Add("_linesPerBox", linesPerBox, DbType.String); 
				parameters.Add("_grossWeightPerBox", grossWeightPerBox, DbType.Decimal);
				parameters.Add("_dimensionsWeightPerBox", dimensionsWeightPerBox, DbType.String);
				parameters.Add("_boxesPerPallet", boxesPerPallet, DbType.Int32);
				parameters.Add("_copiesPerPallet", copiesPerPallet, DbType.Int32);
				parameters.Add("_grossWeightPerPallet", grossWeightPerPallet, DbType.Decimal);
				parameters.Add("_dimensionsPerPallet", dimensionsPerPallet, DbType.String);
				parameters.Add("_instructionsWarnings", instructionsWarnings, DbType.String);
				parameters.Add("_familyId", familyId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(sp, parameters);

				Console.WriteLine($"ESTE ES EL SP: {sp}");
				int response = parameters.Get<int>("_response");
				if(packagingId > 0 && (auxEmpacadoresId  != null || empacadoresId != null)) {
					//Nos regresa un array con las compañias que ya no se encuentren listadas en la petición
					if(auxEmpacadoresId != null) {
						//Si tiene algo eliminamos los viejos
						var viejos = auxEmpacadoresId.Except(empacadoresId);
						//Eliminamos los registros de la tabla pivote (Eliminados por usuario TraceIt)
						foreach (int viejo in viejos)
						{
							string query = $"DELETE FROM rel_050_empacadorEmbalaje  WHERE EmpacadorId = {viejo} and EmbalajeId = {packagingId};";
							ConsultaCommand<string>(query);
						}
					}
					//Nos regresa un array con las compañias nuevas que se agregaron en la petición
					var nuevos = auxEmpacadoresId != null ? empacadoresId.Except(auxEmpacadoresId) : empacadoresId;
					foreach (int nuevo in nuevos)
					{
						//Creamos los registros para las nuevas compañias que seran ligadas (Solo usuario TraceIt)
						string query = $"INSERT INTO rel_050_empacadorEmbalaje (EmpacadorId, EmbalajeId) VALUES({ nuevo },{packagingId});";
						ConsultaCommand<string>(query);
					}

				}
				else if((response > 0 && sp.Equals("spi_guardarEmbalajeFamilia")) && (auxEmpacadoresId != null || empacadoresId != null))
				{
					foreach (int empacador in empacadoresId)
					{
						string query = $"INSERT INTO rel_050_empacadorEmbalaje (EmpacadorId, EmbalajeId) VALUES({empacador},{response});";
						ConsultaCommand<string>(query);
					}
				}
				else if (response == 0)
					throw new Exception("Error al ejecutar el SP de " + (packagingId > 0 ? "editar" : "agregar" ) + " embalaje");
				else if (response == -1)
					throw new Exception("Ya existe un embalaje con el mismo nombre");

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
		/// Metodo para eliminar embalajes
		/// Desarollador: Iván Gutiérrez
		/// </summary>
		public void DeletePackaging()
		{
			log.trace("DeletePackagingSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packagingId", packagingId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				sp = !isReproceso ? "spd_eliminarEmbalajeFamilia" : "spd_eliminarEmbalajeReprocesoFamilia";

				parameters = EjecutarSPOutPut(sp, parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("No se puede eliminar un embalaje tiene dependencias con otros registros");

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
