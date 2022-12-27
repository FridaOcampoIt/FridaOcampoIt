using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base.User;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
	public class UserMobileSQL: DBHelperDapper
	{
		#region Properties
		public string name;
		public string lastname;
		public string gender;
		public int age;
		public string country;
		public string city;
		public string postalCode;
		public string imageUrl;
		public string facebookId;
		public string googleId;
		public string email;
		public string password;
		public string idLogin;
		public int type;
		public int userId;
		public int familyId;
		public int productId;
		public int productUserMobile;
		public decimal qualification;
		public int linkId;
		public string dateBuy;
		public string placeBuy;
		public string photoTicket;
		public int daysNotification;
		public string expiration;
		public int periodMonth;
		public string serialNumber;
		public string registerName;
		public string lastNameRegister;
		public string emailRegister;
		public bool sendNotification;
		public int warrantyId;
		public string recoveryCode;
		public int mobileId;
		public string model;
		public string tokenFCM;
		public string imei;
		public string logName;
		public string latitude;
		public string longitude;
        public int configNotification;
        public int idioma; //0 = español, 1 = inglés
		private LoggerD4 log = new LoggerD4("UserMobileSQL");
		#endregion

		#region Constructor
		public UserMobileSQL()
		{
			this.name = String.Empty;
			this.lastname = String.Empty;
			this.gender = String.Empty;
			this.age = 0;
			this.country = String.Empty;
			this.city = String.Empty;
			this.postalCode = String.Empty;
			this.imageUrl = String.Empty;
			this.facebookId = String.Empty;
			this.googleId = String.Empty;
			this.email = String.Empty;
			this.password = String.Empty;
			this.idLogin = String.Empty;
			this.type = 0;
			this.userId = 0;
			this.familyId = 0;
			this.productId = 0;
			this.productUserMobile = 0;
			this.qualification = 0;
			this.linkId = 0;
			this.dateBuy = String.Empty;
			this.placeBuy = String.Empty;
			this.photoTicket = String.Empty;
			this.daysNotification = 0;
			this.expiration = String.Empty;
			this.periodMonth = 0;
			this.serialNumber = String.Empty;
			this.registerName = String.Empty;
			this.lastNameRegister = String.Empty;
			this.emailRegister = String.Empty;
			this.sendNotification = false;
			this.warrantyId = 0;
			this.recoveryCode = String.Empty;
			this.mobileId = 0;
			this.model = String.Empty;
			this.tokenFCM = String.Empty;
			this.imei = String.Empty;
			this.latitude = String.Empty;
			this.longitude = String.Empty;
			this.logName = String.Empty;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Metodo para consumir el sp para la autenticación de un usuario móvil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public DataUserMobile LoginMobile()
		{
			log.trace("LoginMobile");
			DataUserMobile response = new DataUserMobile();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_password", password, DbType.String);
                parameters.Add("_idioma", idioma, DbType.Int32);

				response = Consulta<DataUserMobile>("spc_loginMovil", parameters).FirstOrDefault();
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consumir el sp para la autenticación de un usuario móvil por medio de facebook y google
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public DataUserMobile LoginMobileById()
		{
			log.trace("LoginMobileById");
			DataUserMobile response = new DataUserMobile();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_loginId", idLogin, DbType.String);
				parameters.Add("_type", type, DbType.Int32);
                parameters.Add("_idioma", idioma, DbType.Int32);

                response = Consulta<DataUserMobile>("spc_loginMovilById", parameters).FirstOrDefault();
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para registrar los datos de un usuario móvil
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveUserMobile()
		{
			log.trace("SaveUserMobile");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_lastname", lastname, DbType.String);
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_gender", gender, DbType.String);
				parameters.Add("_age", age, DbType.Int32);
				parameters.Add("_country", country, DbType.String);
				parameters.Add("_city", city, DbType.String);
				parameters.Add("_postalCode", postalCode, DbType.String);
				parameters.Add("_password", password, DbType.String);
				parameters.Add("_imageUrl", imageUrl, DbType.String);
				parameters.Add("_facebookId", facebookId, DbType.String);
				parameters.Add("_googleId", googleId, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarUsuarioMovil", parameters);
				int response = parameters.Get<int>("_response");

				if (response == -1)
					throw new Exception("Ya existe un usuario con ese correo electronico");
				else if(response == 0)
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
		/// Metodo para editar los datos de un usuario móvil
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateUserMobile()
		{
			log.trace("UpdateUserMobile");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_lastname", lastname, DbType.String);
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_gender", gender, DbType.String);
				parameters.Add("_age", age, DbType.Int32);
				parameters.Add("_country", country, DbType.String);
				parameters.Add("_city", city, DbType.String);
				parameters.Add("_postalCode", postalCode, DbType.String);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionUsuarioMovil", parameters);
				int response = parameters.Get<int>("_response");

				if (response == -1)
					throw new Exception("Ya existe un usuario con ese correo electronico");
				else if (response == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");
			
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
		/// Metodo para consultar la imagen del usuario movil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public UserImageSQL SearchImage()
		{
			log.trace("SearchImage");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_imageProfileUrl", imageUrl, DbType.String);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_option", 1, DbType.Int32);				
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				UserImageSQL resp = Consulta<UserImageSQL>("spu_edicionImagenUsuarioMovil", parameters).FirstOrDefault();
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
		/// Metodo para actualizar la imagen del usuario móvil
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateImageUserMobile()
		{
			log.trace("UpdateImageUserMobile");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_imageProfileUrl", imageUrl, DbType.String);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_option", 2, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionImagenUsuarioMovil", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");

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
		/// Metodo para cargar las listas que seran consultadas al iniciar la aplicación móvil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public ListSystemSQL SearchListSystems()
		{
			ListSystemSQL response = new ListSystemSQL();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(CategoriesSQL));
				types.Add(typeof(SectionTypesSQL));
				types.Add(typeof(LinkType));
                types.Add(typeof(Countries));

				var responseSQL = ConsultaMultiple("spc_consultaInicialMovil", types);

				response.categoriesSQL = responseSQL[0].Cast<CategoriesSQL>().ToList();
				response.sectionTypesSQL = responseSQL[1].Cast<SectionTypesSQL>().ToList();
				response.linkTypes = responseSQL[2].Cast<LinkType>().ToList();
                response.countries = responseSQL[3].Cast<Countries>().ToList();

				CerrarConexion();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}

			return response;
		}

		/// <summary>
		/// Metodo para mandar el listado de produtos y familias que tiene un usuario
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public List<ProductFamilyUsers> SearchFamilyProductUser()
		{
			List<ProductFamilyUsers> response = new List<ProductFamilyUsers>();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_userMobileId", userId, DbType.Int32);

				response = Consulta<ProductFamilyUsers>("spc_consultaProductoFamiliaMovil", parameters);
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para guardar una familia o producto al catalogo
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveFamilyProductUser()
		{
			log.trace("SaveFamilyProductUser");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_familyId", familyId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				parameters.Add("_userMobileId", userId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarProductoFamiliaMovil", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");
				else if (response == -1)
					throw new Exception("No se puede agregar la familia al catálogo porque el usuario tiene agregado un producto perteneciente al mismo");

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
		/// Metodo para eliminar un producto o familia del catalogo
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteFamilyProductUser()
		{
			log.trace("DeleteFamilyProductUser");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_productUserMobileId", productUserMobile, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarProductoFamiliaMovil", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");

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
		/// Metodo para guardar las calificaciones de las familias
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveQualificationFamily()
		{
			log.trace("SaveQualificationFamily");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_userMobileId", userId, DbType.Int32);
				parameters.Add("_familyProductId", familyId, DbType.Int32);
				parameters.Add("_qualification", qualification, DbType.Decimal);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarCalificacionFamiliaMovil", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");

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
		/// Metodo para registrar una calificacion de link
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveQualificationLink()
		{
			log.trace("SaveQualificationLink");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_userMobileId", userId, DbType.Int32);
				parameters.Add("_linkId", linkId, DbType.Int32);
				parameters.Add("_qualification", qualification, DbType.Decimal);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarCalificacionVinculo", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");

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
		/// Metodo para registrar la garantia del móvil
		/// Desarrollador: David Martinez
		/// </summary>
		public WarrantiesResponseSQL SaveWarrantyUser()
		{
			log.trace("SaveWarrantyUser");
			WarrantiesResponseSQL responseSQL = new WarrantiesResponseSQL();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_dateBuy", dateBuy, DbType.String);
				parameters.Add("_placeBuy", placeBuy, DbType.String);
				parameters.Add("_photoTicket", photoTicket, DbType.String);
				parameters.Add("_daysNotification", daysNotification, DbType.Int32);
				parameters.Add("_expiration", expiration, DbType.String);
				parameters.Add("_periodMonth", periodMonth, DbType.Int32);
				parameters.Add("_serialNumber", serialNumber, DbType.String);
				parameters.Add("_registerName", registerName, DbType.String);
				parameters.Add("_lastNameRegister", lastNameRegister, DbType.String);
				parameters.Add("_emailRegister", emailRegister, DbType.String);
				parameters.Add("_sendNotification", sendNotification, DbType.Boolean);
				parameters.Add("_warrantyId", warrantyId, DbType.Int32);
				parameters.Add("_userMobileId", userId, DbType.Int32);
				parameters.Add("_country", country, DbType.String);
				parameters.Add("_city", city, DbType.String);
				parameters.Add("_age", age, DbType.Int32);
				parameters.Add("_gender", gender, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarGarantiaUsuario", parameters);
				productId = parameters.Get<int>("_response");
				log.trace("productId after saving warranty: " + productId);
				if (productId == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");
				else if (productId == -1)
					throw new Exception("El producto no existe");
				else if (productId == -2)
					throw new Exception("El producto ya contiene una garantía regístrada");

				parameters = new DynamicParameters();
				parameters.Add("_option", 1, DbType.Int32);
				parameters.Add("_idReference", productId, DbType.Int32);

				CompanyIdSQL idCompanyReference = Consulta<CompanyIdSQL>("spc_consultaId", parameters).FirstOrDefault();

				parameters = new DynamicParameters();
				parameters.Add("_option", type, DbType.Int32);
				parameters.Add("_id", idCompanyReference.id, DbType.Int32);
				log.debug("Args to spc_consultaCorreosEnvio: _option:" + type + ", idCompany: " + idCompanyReference);
				EmailUserSQL emailReference = Consulta<EmailUserSQL>("spc_consultaCorreosEnvio", parameters).FirstOrDefault();

				if (emailReference != null)
				{
					if(emailReference.email != "")
						responseSQL.correos.Add(emailReference.email);
				}

				responseSQL.productId = productId;
				responseSQL.qrCode = idCompanyReference.qrCode;

				TransComit();
				CerrarConexion();

				return responseSQL;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los emails para el envio de correos
		/// Desarrollador: David Martine
		/// </summary>
		/// <returns></returns>
		public List<string> searchEmail()
		{
			log.trace("Searching addresses for email");
			List<string> email = new List<string>();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_option", type, DbType.Int32);
				parameters.Add("_id", userId, DbType.Int32);

				EmailUserSQL emailResponse = Consulta<EmailUserSQL>("spc_consultaCorreosEnvio", parameters).FirstOrDefault();
				

				if(email != null)
				{
					if (emailResponse.email != null || emailResponse.email != "")
						email = emailResponse.email.Split(',').ToList();
				}
                CerrarConexion();

                return email;
			}
			catch (Exception ex)
			{
				log.error("Error geting address for the email with the type "+type+" for the user "+userId+": " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para guardar y actualizar el código de recuperación
		/// Desarrollador: David Martinez
		/// </summary>
		public void saveCodeRecovery()
		{
			log.trace("saveCodeRecovery");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_recoveryCode", recoveryCode, DbType.String);
				parameters.Add("_password", password, DbType.String);
				parameters.Add("_option", type, DbType.Int32);
				parameters.Add("_userType", 1, DbType.Int32);//for mobile user
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionCodigoRecuperacion", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");
				else if (response == -1)
					throw new Exception("Usuario inexistente");
				else if (response == -2)
					throw new Exception("Código de recuperación incorrecto");

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
		/// Metodo para consultar los datos del usuario
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public UserMobileData searchDataUserMobile()
		{
			log.trace("searchDataUserMobile");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_userId", userId, DbType.Int32);

				UserMobileData response = Consulta<UserMobileData>("spc_consultaDatosUsuarioMovil", parameters).FirstOrDefault();
				if(response == null) {
					response = new UserMobileData();
				}
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los datos de la garantia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public WarrantiesDataUser searchWarrantiesData()
		{
			log.trace("searchWarrantiesData");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_productId", productId, DbType.Int32);

				WarrantiesDataUser response = Consulta<WarrantiesDataUser>("spc_consultaDatosGarantiasMovil", parameters).FirstOrDefault();
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para el registro y actualización del dispositivo móvil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public int registerMobile()
		{
			log.trace("registerMobile");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_mobileId", mobileId, DbType.Int32);
				parameters.Add("_model", model, DbType.String);
				parameters.Add("_tokenFCM", tokenFCM, DbType.String);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_imei", imei, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarDispositivoMovil", parameters);
				mobileId = parameters.Get<int>("_response");

				if (mobileId == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");
				else if (mobileId == -1)
					throw new Exception("Ya existe el dispositivo móvil");

				TransComit();
				CerrarConexion();

				return mobileId;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo que registra los logs en la base de datos
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveLog()
		{
			log.trace("SaveLog");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_latitude", latitude, DbType.String);
				parameters.Add("_longitude", longitude, DbType.String);
				parameters.Add("_logName", logName, DbType.String);
				parameters.Add("_userMobileId", mobileId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				parameters.Add("_familyProductId", familyId, DbType.Int32);
				parameters.Add("_linkId", linkId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarLogs", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");

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
        /// Metodo para actualizar el estado de la configuración de notificaciones
        /// configNotification = 0 "Notificaciones inactivas por configuración"
        /// configNotification = 1 "Notificaciones activas"
        /// configNotification = 2 "Notificaciones inactivas por logout"
        /// Desarrollador: David Martinez
        /// </summary>
        public void updateNotificationConfig()
        {
            log.trace("updateNotificationConfig");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_usuarioId", userId, DbType.Int32);
                parameters.Add("_estadoNotificacion", configNotification, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spu_estatusNotificaciones", parameters);
                int response = parameters.Get<int>("_response");

                if (response == 0)
                    throw new Exception("Ocurrio un error ejecutar el sp");
                else if (response == -1)
                    throw new Exception("Usuario inexistente");

                TransComit();
                CerrarConexion();
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
TransRollback();
                CerrarConexion();
                throw ex;
            }
        }

        public List<NotificationData> consultaNotificacionesGarantia()
        {
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                List<NotificationData> response = Consulta<NotificationData>("spc_consultaNotificacionesGarantia");
                CerrarConexion();

                return response;
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
CerrarConexion();
                throw ex;
            }
        }

        #endregion
    }
}