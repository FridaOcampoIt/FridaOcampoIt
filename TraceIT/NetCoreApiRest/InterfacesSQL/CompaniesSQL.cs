using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base.Companies;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using WSTraceIT.Models.Base;
using System.Numerics;

namespace WSTraceIT.InterfacesSQL
{
	public class CompaniesSQL: DBHelperDapper
	{
		#region Properties
		public int idCompany;
		public string name;
		public string businessName;
		public string email;
		public string webSite;
		public string phone;
		public string country;
		public string address;
		public bool status;
		public string facebook;
		public string youtube;
		public string linkedin;
		public string clientNumber;
		public int packedId;
        public int tipoGiro;

        public int idContactFirst;
		public bool defaultFirst;
		public string contactNameFirst;
		public string contactPhoneFirst;
		public string contactEmailFirst;
		public int idContactSecond;
		public bool defaultSecond;
		public string contactNameSecond;
		public string contactPhoneSecond;
		public string contactEmailSecond;
		private LoggerD4 log = new LoggerD4("CompaniesSQL");
		#endregion

		#region Constructor
		public CompaniesSQL()
		{
			this.idCompany = 0;
			this.name = String.Empty;
			this.businessName = String.Empty;
			this.email = String.Empty;
			this.webSite = String.Empty;
			this.phone = String.Empty;
			this.country = String.Empty;
			this.address = String.Empty;
			this.status = false;
			this.tipoGiro = 0;

            this.idContactFirst = 0;
			this.defaultFirst = false;
			this.contactNameFirst = String.Empty;
			this.contactPhoneFirst = String.Empty;
			this.contactEmailFirst = String.Empty;
			this.idContactSecond = 0;
			this.defaultSecond = false;
			this.contactNameSecond = String.Empty;
			this.contactPhoneSecond = String.Empty;
			this.contactEmailSecond = String.Empty;
			this.facebook = String.Empty;
			this.youtube = String.Empty;
			this.linkedin = String.Empty;
			this.clientNumber = String.Empty;
			this.tipoGiro = 0;

        }
		#endregion

		#region Public method
		/// <summary>
		/// Metodo para guardar los datos de la compañia 
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveCompany()
		{
			log.trace("SaveCompany");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_businessName", businessName, DbType.String);
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_webSite", webSite, DbType.String);
				parameters.Add("_phone", phone, DbType.String);
				parameters.Add("_country", country, DbType.String);
				parameters.Add("_address", address, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_facebook", facebook, DbType.String);
				parameters.Add("_youtube", youtube, DbType.String);
				parameters.Add("_linkedin", linkedin, DbType.String);
				parameters.Add("_clientNumber", clientNumber, DbType.String);
                parameters.Add("_FK_CMMTipoGiro", tipoGiro, DbType.String);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarCompania", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");

				parameters = new DynamicParameters();
				parameters.Add("_contactName", contactNameFirst, DbType.String);
				parameters.Add("_contactEmail", contactEmailFirst, DbType.String);
				parameters.Add("_contactPhone", contactPhoneFirst, DbType.String);
				parameters.Add("_contactDefault", defaultFirst, DbType.Boolean);
				parameters.Add("_companyId", response, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarContactoCompania", parameters);
				int responseContact = parameters.Get<int>("_response");

				if (responseContact == 0)
					throw new Exception("Error al ejecutar el sp");

				parameters = new DynamicParameters();
				parameters.Add("_contactName", contactNameSecond, DbType.String);
				parameters.Add("_contactEmail", contactEmailSecond, DbType.String);
				parameters.Add("_contactPhone", contactPhoneSecond, DbType.String);
				parameters.Add("_contactDefault", defaultSecond, DbType.Boolean);
				parameters.Add("_companyId", response, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarContactoCompania", parameters);
				responseContact = parameters.Get<int>("_response");

				if (responseContact == 0)
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
		
		/// <summary>
		/// Metodo para actualizar los datos de una compañia
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateCompany()
		{
			log.trace("UpdateCompany");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idCompany", idCompany, DbType.Int32);
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_businessName", businessName, DbType.String);
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_webSite", webSite, DbType.String);
				parameters.Add("_phone", phone, DbType.String);
				parameters.Add("_country", country, DbType.String);
				parameters.Add("_address", address, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_facebook", facebook, DbType.String);
				parameters.Add("_youtube", youtube, DbType.String);
				parameters.Add("_linkedin", linkedin, DbType.String);
				parameters.Add("_clientNumber", clientNumber, DbType.String);
                parameters.Add("_FK_CMMTipoGiro", tipoGiro, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionCompania", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");

				parameters = new DynamicParameters();
				parameters.Add("_idContact", idContactFirst, DbType.Int32);
				parameters.Add("_contactName", contactNameFirst, DbType.String);
				parameters.Add("_contactEmail", contactEmailFirst, DbType.String);
				parameters.Add("_contactPhone", contactPhoneFirst, DbType.String);
				parameters.Add("_contactDefault", defaultFirst, DbType.Boolean);				
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionContactoCompania", parameters);
				int responseContact = parameters.Get<int>("_response");

				if (responseContact == 0)
					throw new Exception("Error al ejecutar el sp");

				parameters = new DynamicParameters();
				parameters.Add("_idContact", idContactSecond, DbType.Int32);
				parameters.Add("_contactName", contactNameSecond, DbType.String);
				parameters.Add("_contactEmail", contactEmailSecond, DbType.String);
				parameters.Add("_contactPhone", contactPhoneSecond, DbType.String);
				parameters.Add("_contactDefault", defaultSecond, DbType.Boolean);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionContactoCompania", parameters);
				responseContact = parameters.Get<int>("_response");

				if (responseContact == 0)
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

		/// <summary>
		/// Metodo para eliminar la compañia
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteCompany()
		{
			log.trace("DeleteCompany");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idCompany", idCompany, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarCompania", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
				else if (response == -1)
					throw new Exception("La compañía contiene familias relacionadas");
				else if (response == -2)
					throw new Exception("La compañía contiene direcciones relacionadas");

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
		///	Metodo para consultar las compañias
		///	Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public List<CompaniesData> SeachCompanies()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_businessName", businessName, DbType.String);
			   
				List<CompaniesData> resp = Consulta<CompaniesData>("spc_consultaCompanias", parameters);
				CerrarConexion();

				return resp;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}


		/// <summary>
		///	Metodo para consultar las compañias por empacador
		///	Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<CompaniesData> searchCompanyEmpacador()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_packedId", packedId, DbType.Int32);

				List<CompaniesData> resp = Consulta<CompaniesData>("spc_consultaCompaniasEmpacador", parameters);
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



		#region Generics
		/// <summary>
		/// Metodo para traer el nombre de la compañia logeada
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public CompaniesData searchCompanyName()
		{
			log.trace("searchCompanyName");
			try
			{
				CompaniesData response = new CompaniesData();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companiaId", idCompany, DbType.Int32);
				response = Consulta<CompaniesData>("spc_consultaNombreCompania", parameters).FirstOrDefault();
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


		/// <summary>
		/// Metodo para realizar la consulta de los datos de la compañia para su edicion
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public CompanyDataSQL SearchCompaniesData()
		{			
			log.trace("SearchCompaniesData");			
			try
			{
				CompanyDataSQL response = new CompanyDataSQL();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(CompanyDataEditionSQL));
				types.Add(typeof(ContactCompaniesDataSQL));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idCompany", idCompany, DbType.Int32);

				var resp = ConsultaMultiple("spc_consultaCompaniasDatos", types, parameters);

				response.companyDataEdition = resp[0].Cast<CompanyDataEditionSQL>().FirstOrDefault();
				response.contactCompaniesData = resp[1].Cast<ContactCompaniesDataSQL>().ToList();

				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
				CerrarConexion();
				throw ex;
			}
		}
		#endregion
	}
}
