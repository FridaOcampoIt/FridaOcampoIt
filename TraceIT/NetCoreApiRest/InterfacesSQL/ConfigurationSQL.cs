using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Configuration;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
	public class ConfigurationSQL: DBHelperDapper
	{
		#region Propiedades
		public int company;
		public int nUseGuides;
		public int nInstalationGuides;
		public int nRelatedProduct;
		public string notifyComments;
		public string notifyWarranty;
		public string notifyStolen;
		public int nPDF;
		public int nCharSpec;
		public int nImg;
		public int nCharFAQ;
		public int nVid;

		public int option;
		public List<SaveGeneralConfiguration> configurations;
		private LoggerD4 log = new LoggerD4("ConfigurationSQL");
		#endregion

		#region Constructor
		public ConfigurationSQL()
		{
			this.company = 0;
			this.nUseGuides = 0;
			this.nInstalationGuides = 0;
			this.nRelatedProduct = 0;
			this.notifyComments = String.Empty;
			this.notifyWarranty = String.Empty;
			this.notifyStolen = String.Empty;
			this.nPDF = 0;
			this.nCharFAQ = 0;
			this.nCharSpec = 0;
			this.nImg = 0;
			this.nVid = 0;

			this.configurations = new List<SaveGeneralConfiguration>();
			this.option = 0;
		}
		#endregion

		#region Metodos publicos
		/// <summary>
		/// Metodo para consultar los combos para el modulo
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public ConfigurationDropDownSQL SearchDropDown()
		{
			log.trace("SearchDropDown");
			ConfigurationDropDownSQL response = new ConfigurationDropDownSQL();

			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(GeneralConfigurationDataSQL));
				types.Add(typeof(TraceITListDropDown));
				types.Add(typeof(TraceITListDropDown));

				var respSQL = ConsultaMultiple("spc_consultaConfiguracionGral", types);

				response.configurationGenerals = respSQL[0].Cast<GeneralConfigurationDataSQL>().ToList();
				response.users = respSQL[1].Cast<TraceITListDropDown>().ToList();
				response.companies = respSQL[2].Cast<TraceITListDropDown>().ToList();

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
		/// Metodo para consultar las configuraciones de una compañia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public CompanyConfigurationData SearchConfigurationCompany()
		{
			log.trace("SearchConfigurationCompany");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_company", company, DbType.Int32);

				CompanyConfigurationData response = Consulta<CompanyConfigurationData>("spc_ConsultaConfiguracionCompania", parameters).FirstOrDefault();

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
		/// Metodo para guardar las configuracion general
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveGeneralConfiguration()
		{
			log.trace("SaveGeneralConfiguration");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				foreach (var config in configurations)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_configuration", config.configuration, DbType.String);
					parameters.Add("_value", config.value, DbType.String);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spu_edicionConfiguracionGral", parameters);
					int response = parameters.Get<int>("_response");

					if (response == 0)
						throw new Exception("Error al ejecutar el sp");
				}			

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para guardar la configuración por compañia
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveCompanyConfiguration()
		{
			log.trace("SaveCompanyConfiguration");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_company", company, DbType.Int32);
				parameters.Add("_nUseGuides", nUseGuides, DbType.Int32);
				parameters.Add("_nInstalationGuides", nInstalationGuides, DbType.Int32);
				parameters.Add("_nRelatedProduct", nRelatedProduct, DbType.Int32);
				parameters.Add("_notifyComments", notifyComments, DbType.String);
				parameters.Add("_notifyWarranty", notifyWarranty, DbType.String);
				parameters.Add("_notifyStolen", notifyStolen, DbType.String);
				parameters.Add("_nPDF", nPDF, DbType.Int32);
				parameters.Add("_nCharE", nCharSpec, DbType.Int32);
				parameters.Add("_nCharQ", nCharFAQ, DbType.Int32);
				parameters.Add("_nImg", nImg, DbType.Int32);
				parameters.Add("_nVid", nVid, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionConfiguracionCompania", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		}
		#endregion
	}
}
