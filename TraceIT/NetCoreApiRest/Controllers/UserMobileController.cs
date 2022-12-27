using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Models.Response;
using NetCoreApiRest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WS.Interfaces;
using WSTraceIT.Interfaces;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Families;
using WSTraceIT.Models.Base.User;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;

namespace NetCoreApiRest.Controllers
{
	[Route("UserMobile")]
	[ApiController]
	public class UserMobileController : ControllerBase
	{
		private LoggerD4 log = new LoggerD4("UserMobileController");

		/// <summary>
		/// Web Method para la autenticacion con el usuario base para la generacion del token provisional 
		/// para que sea utilizada en los consumos de los Web Method cuando aun no se tenga una sesión iniciada
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("loginTraceIT")]
		[HttpPost]
		public IActionResult loginTraceIT([FromBody]LoginMobileRequest request)
		{
			LoginTraceITResponse response = new LoginTraceITResponse();

			if(ConfigurationSite._cofiguration["Credenciales:Username"] != request.email || ConfigurationSite._cofiguration["Credenciales:Password"] != request.password)
			{
				response.messageEng = "Incorrect email or password";
				response.messageEsp = "Correo electrónico o contraseña incorrectos";
			}
			else
			{
				//Variables que conformaran el token para la autorizacion
				var claimsData = new[]
				{
					new Claim("Name", ConfigurationSite._cofiguration["Credenciales:Username"])
				};

				//Si tiene acceso se llena la clase del usuario y se manda al metodo para la generacion del token
				response.token = ConfigurationSite.GenerateToken(claimsData);
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		/// <summary>
		/// Web Method para la autenticación del usuario móvil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("loginMobile")]
		[HttpPost]
		public IActionResult loginMobile([FromBody]LoginMobileRequest request)
		{
			LoginMobileResponse response = new LoginMobileResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.email = request.email;
				sql.password = request.password;
                sql.idioma = request.idioma;

				response.UserData = sql.LoginMobile();

				//Se valida la existencia del usuario en la base de datos
				if(response.UserData != null)
				{
					#region Generacion del Token
					//Variables que conformaran el token para la autorizacion
					var claimsData = new[]
					{
						new Claim("Name", response.UserData.name),
						new Claim("lastname", response.UserData.lastname),
						new Claim("Email", response.UserData.email),
						new Claim("Pais", response.UserData.country),
						new Claim("Ciudad", response.UserData.city)
					};

					//Si tiene acceso se llena la clase del usuario y se manda al metodo para la generacion del token
					response.token = ConfigurationSite.GenerateToken(claimsData);
					response.UserData.image = ConfigurationSite._cofiguration["Paths:urlImagesUser"] + response.UserData.image;
					#endregion
				}
				else
				{
					response.messageEng = "Incorrect email or password";
					response.messageEsp = "Correo electrónico o contraseña incorrectos";
				}				
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			// Retornamos el token
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		/// <summary>
		/// Web Method para la autenticación del usuario móvil por medio de facebook o google
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("loginMobileById")]
		[HttpPost]
		public IActionResult loginMobileById([FromBody]LoginMobileByIdRequest request)
		{
			LoginMobileResponse response = new LoginMobileResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.idLogin = request.idLogin;
				sql.type = request.type;
                sql.idioma = request.idioma;

				response.UserData = sql.LoginMobileById();

				//Se valida la existencia del usuario en la base de datos
				if (response.UserData != null)
				{
					#region Generacion del Token
					//Variables que conformaran el token para la autorizacion
					var claimsData = new[]
					{
						new Claim("Name", response.UserData.name),
						new Claim("lastname", response.UserData.lastname),
						new Claim("Email", response.UserData.email),
						new Claim("Pais", response.UserData.country),
						new Claim("Ciudad", response.UserData.city)
					};

					//Si tiene acceso se llena la clase del usuario y se manda al metodo para la generacion del token
					response.token = ConfigurationSite.GenerateToken(claimsData);
					response.UserData.image = ConfigurationSite._cofiguration["Paths:urlImagesUser"] + response.UserData.image;
					#endregion
				}
				else
				{
					response.messageEng = "Incorrect email or password";
					response.messageEsp = "Correo electrónico o contraseña incorrectos";
				}
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			// Retornamos el token
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		/// <summary>
		/// Web Method para registrar un usuario de tipo movil
		/// NOTA: Para este web method se utilizara un token provicional generado con usuario provisional de
		///		  WSTraceIT para realizar este procedimiento se utilizara el web method loginTraceIT
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("registerMobileUser")]
		[HttpPost]
		[Authorize]
		public IActionResult registerMobileUser([FromBody]RegisterMobileUserRequest request)
		{
			log.trace("registerMobileUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			RegisterMobileUserResponse response = new RegisterMobileUserResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.name = request.name;
				sql.lastname = request.lastName;
				sql.email = request.email;
				sql.gender = request.gender;
				sql.age = request.age;
				sql.country = request.country;
				sql.city = request.city;
				sql.postalCode = request.postalCode;
				sql.password = request.password;
				sql.facebookId = request.facebookId;
				sql.googleId = request.googleId;

				sql.SaveUserMobile();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
//Se valida si es un error que manda el SP o el proceso del registro de usuario
				if(ex.Message == "Ya existe un usuario con ese correo electronico")
				{
					response.messageEng = "There is already a user with that email";
					response.messageEsp = "Ya existe un usuario con ese correo electronico";
				}
				else
				{
					response.messageEng = "An error occurred: " + ex.Message;
					response.messageEsp = "Ocurrio un error: " + ex.Message;
				}
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para editar un usuario de tipo móvil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editMobileUser")]
		[HttpPost]
		[Authorize]
		public IActionResult editMobileUser([FromBody]EditMobileUserRequest request)
		{
			log.trace("editMobileUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditMobileUserResponse response = new EditMobileUserResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.userId = request.userId;
				sql.name = request.name;
				sql.lastname = request.lastName;
				sql.email = request.email;
				sql.age = request.age;
				sql.gender = request.gender;
				sql.country = request.country;
				sql.city = request.city;
				sql.postalCode = request.postalCode;

				sql.UpdateUserMobile();

			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
//Se valida si es un error que manda el SP o el proceso del registro de usuario
				if (ex.Message == "Ya existe un usuario con ese correo electronico")
				{
					response.messageEng = "There is already a user with that email";
					response.messageEsp = "Ya existe un usuario con ese correo electronico";
				}
				else
				{
					response.messageEng = "An error occurred: " + ex.Message;
					response.messageEsp = "Ocurrio un error: " + ex.Message;
				}
				throw;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para editar la imagen del usuario móvil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("userMobileImage")]
		[HttpPost]
		[Authorize]
		public IActionResult userMobileImage([FromBody]UserMobileImageRequest request)
		{
			log.trace("userMobileImage");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			UserMobileImageResponse response = new UserMobileImageResponse();
			string nombreImagen = String.Empty;
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.userId = request.userId;

				UserImageSQL image = sql.SearchImage();

				if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesUser"] + image.nameImage))
					System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesUser"] + image.nameImage);

				Random random = new Random();
				nombreImagen = random.Next() + ".jpg";
				ProcesarImagen procesar = new ProcesarImagen();
				procesar.SaveImage(request.image, nombreImagen, ConfigurationSite._cofiguration["Paths:pathImagesUser"]);

				sql.userId = request.userId;
				sql.imageUrl = nombreImagen;
				sql.UpdateImageUserMobile();

				response.image = ConfigurationSite._cofiguration["Paths:urlImagesUser"] + nombreImagen;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesUser"] + nombreImagen))
					System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesUser"] + nombreImagen);

				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		/// <summary>
		/// Web Method para cargar los listados iniciales en la aplicación
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchList")]
		[HttpPost]
		[Authorize]
		public IActionResult searchList([FromBody]SearchListRequest request)
		{
			log.trace("searchList");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchListResponse response = new SearchListResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				ListSystemSQL responseSQL = sql.SearchListSystems();

				response.listSystem.categories = (from cat in responseSQL.categoriesSQL
												  select new Categories
												  {
													  id = cat.id,
													  image = cat.image != "" ? ConfigurationSite._cofiguration["Paths:urlImagesCategory"] + cat.image : "",
													  imageBanner = cat.imageBanner != "" ? ConfigurationSite._cofiguration["Paths:urlImagesCategory"] + cat.imageBanner : "",
													  name = request.language == "ES" ? cat.name : cat.nameEnglish,
													  namePersonalized = request.language == "ES" ? cat.namePersonalized : cat.namePersonalizedEnglish

												  }).ToList();
				response.listSystem.sectionTypes = (from st in responseSQL.sectionTypesSQL
													select new SectionTypes
													{
														id = st.id,
														name = request.language == "ES" ? st.name : st.nameEnglish

													}).ToList();
				response.listSystem.linkTypes = responseSQL.linkTypes;
                response.listSystem.countries = responseSQL.countries;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para buscar las familias y productos en el catalogo del usuario
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchFamilyProductUser")]
		[HttpPost]
		[Authorize]
		public IActionResult searchFamilyProductUser([FromBody]SearchFamilyProductUserRequest request)
		{
			log.trace("searchFamilyProductUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchFamilyProductUserResponse response = new SearchFamilyProductUserResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.userId = request.userMobileId;

				List<ProductFamilyUsers> respSQL = sql.SearchFamilyProductUser();

				response.productFamilyUsers = (from pfU in respSQL
											   select new ProductFamilyUsers
											   {
												   categoryId = pfU.categoryId,
												   expirationDate = pfU.expirationDate,
												   code = pfU.code,
												   familyId = pfU.familyId,
												   id = pfU.id,
												   image = ConfigurationSite._cofiguration["Paths:urlImagesFamily"] + pfU.image,
												   model = pfU.model,
												   name = pfU.name,
												   productId = pfU.productId
											   }).ToList();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		/// <summary>
		/// Web Method para guardar las familias y productos en el catalogo del usuario
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveFamilyProductUser")]
		[HttpPost]
		[Authorize]
		public IActionResult saveFamilyProductUser([FromBody]SaveFamilyProductUserRequest request)
		{
			log.trace("saveFamilyProductUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveFamilyProductUserResponse response = new SaveFamilyProductUserResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.familyId = request.familyId;
				sql.productId = request.productId;
				sql.userId = request.userMobileId;

				sql.SaveFamilyProductUser();

				List<ProductFamilyUsers> respSQL = sql.SearchFamilyProductUser();

				response.productFamilyUsers = (from pfU in respSQL
											   select new ProductFamilyUsers
											   {
												   categoryId = pfU.categoryId,
												   expirationDate = pfU.expirationDate,
												   code = pfU.code,
												   familyId = pfU.familyId,
												   id = pfU.id,
												   image = ConfigurationSite._cofiguration["Paths:urlImagesFamily"] + pfU.image,
												   model = pfU.model,
												   name = pfU.name,
												   productId = pfU.productId
											   }).ToList();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
if(ex.Message == "No se puede agregar la familia al catálogo porque el usuario tiene agregado un producto perteneciente al mismo")
				{
					response.messageEng = "You can not add the family to the catalog because the user has added a product belonging to it";
					response.messageEsp = "No se puede agregar la familia al catálogo porque el usuario tiene agregado un producto perteneciente al mismo";
				}
				else
				{
					response.messageEng = "An error occurred: " + ex.Message;
					response.messageEsp = "Ocurrio un error: " + ex.Message;
				}
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		/// <summary>
		/// Web Method para eliminar las familias y productos en el catalogo del usuario
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteFamilyProductUser")]
		[HttpPost]
		[Authorize]
		public IActionResult deleteFamilyProductUser([FromBody]DeleteFamilyProductUserRequest request)
		{
			log.trace("deleteFamilyProductUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteFamilyProductUserResponse response = new DeleteFamilyProductUserResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.productUserMobile = request.productUserMobile;

				sql.DeleteFamilyProductUser();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		/// <summary>
		/// Web Method para guardar las calificaciones de la familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveQualificationFamily")]
		[HttpPost]
		[Authorize]
		public IActionResult saveQualificationFamily([FromBody]SaveQualificationFamilyRequest request)
		{
			log.trace("saveQualificationFamily");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveQualificationFamilyResponse response = new SaveQualificationFamilyResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.userId = request.userMobileId;
				sql.familyId = request.familyProductId;
				sql.qualification = request.qualification;

				sql.SaveQualificationFamily();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para guardar o registrar la calificaciónes del link
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveQualificationLink")]
		[HttpPost]
		[Authorize]
		public ActionResult saveQualificationLink([FromBody]SaveQualificationLinkRequest request)
		{
			log.trace("saveQualificationLink");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveQualificationLinkResponse response = new SaveQualificationLinkResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.linkId = request.linkId;
				sql.userId = request.userMobileId;
				sql.qualification = request.qualification;

				sql.SaveQualificationLink();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el registro de garantia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveWarrantyUser")]
		[HttpPost]
		[Authorize]
		public IActionResult saveWarrantyUser([FromBody]SaveWarrantyUserRequest request)
		{
			log.trace("saveWarrantyUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveWarrantyUserResponse response = new SaveWarrantyUserResponse();
			ArchivesData archives = new ArchivesData();
			try
			{
				if(request.photoTicket != null && request.photoTicket != "")
				{
					Random random = new Random();

					archives.archiveName = request.serialNumber.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + ".jpg";
                    archives.tmbName = request.serialNumber.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + "_tmb.jpg";
                    archives.base64 = request.photoTicket;
					request.photoTicket = archives.archiveName;
				}

				UserMobileSQL sql = new UserMobileSQL();
				sql.dateBuy = request.dateBuy;
				sql.placeBuy = request.placeBuy;
				sql.photoTicket = request.photoTicket;
				sql.daysNotification = request.daysNotification;
				sql.expiration = request.expiration;
				sql.periodMonth = request.periodMonth;
				sql.serialNumber = request.serialNumber;
				sql.registerName = request.registerName;
				sql.lastNameRegister = request.lastNameRegister;
				sql.emailRegister = request.emailRegister;
				sql.sendNotification = request.sendNotification;
				sql.warrantyId = request.warrantyId;
				sql.userId = request.userMobileId;
				sql.country = request.country;
				sql.city = request.city;
				sql.age = request.age;
				sql.gender = request.gender;
				sql.type = 1;

				WarrantiesResponseSQL responseSQL = sql.SaveWarrantyUser();
				response.productId = responseSQL.productId;
				response.qrCode = responseSQL.qrCode;
				log.debug("WarrantiesResponseSQL: " + Newtonsoft.Json.JsonConvert.SerializeObject(responseSQL));

				if (responseSQL.correos != null)
				{
					if (responseSQL.correos.Count > 0)
					{
						//responseSQL.correos.Add();
						EMAILClient email = new EMAILClient();
						String mensaje = "Se ha registrado una nueva garantia";
						//responseSQL.correos.Add(request.emailRegister);
						log.trace("send email to: " + responseSQL.correos + ".");
						email.EnviarCorreo(responseSQL.correos.ToArray(), mensaje, "Registro de garantia");
						WarrantiesResponseSQL responseSQL2 = new WarrantiesResponseSQL();
						log.trace("send second email to: " + request.emailRegister + ".");
						responseSQL2.correos.Add(request.emailRegister);
						email.EnviarCorreo(responseSQL2.correos.ToArray(), mensaje, "Registro de garantia");
					}
				}


				if (archives.base64 != "" && archives.base64 != null)
				{
					ProcesarImagen image = new ProcesarImagen();
					image.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesWarranties"]);
                    image.SaveTmbn(archives.base64, archives.tmbName, ConfigurationSite._cofiguration["Paths:pathImagesWarranties"]);
                }

				
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
				if(ex.Message == "El producto no existe")
				{
					response.messageEng = "The product does not exist";
					response.messageEsp = "El producto no existe";
				}
				else if(ex.Message == "El producto ya contiene una garantía regístrada")
				{
					response.messageEng = "The product already contains a registered guarantee";
					response.messageEsp = "El producto ya contiene una garantía regístrada";
				}
				else if (ex.Message == "Error 1 al guardar la imagen")
				{
					response.messageEng = "The image was not saved";
					response.messageEsp = "La imagen no fue almacenada";
				}
				else if (ex.Message == "Error 2 al guardar la imagen")
				{
					response.messageEng = "The image couldn´t not be save";
					response.messageEsp = "La imagen no fue almacenada";
				}
				else if (ex.Message == "Failure sending mail.")
				{
					response.messageEng = "Error send email.";
					response.messageEsp = "Error al mandar correo";
				}
				else
				{
					response.messageEng = "An error occurred: " + ex.Message;
					response.messageEsp = "Ocurrio un error: " + ex.Message;
				}				
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el envio de preguntas y comentarios al fabricante
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("sendQuestionCompany")]
		[HttpPost]
		[Authorize]
		public IActionResult sendQuestionCompany([FromBody]SendQuestionCompanyRequest request)
		{
			log.trace("sendQuestionCompany");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SendQuestionCompanyResponse response = new SendQuestionCompanyResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.userId = request.idFamily;
				sql.type = 2;

				List<string> emailCompany = sql.searchEmail();

				if(emailCompany.Count > 0)
				{
					if(emailCompany[0] != "")
					{
						sql.userId = request.idUserMobile;
						sql.type = 3;
						List<string> emailUser = sql.searchEmail();
						
						EMAILClient email = new EMAILClient();
						String Mensaje = request.question;
						String CorreoReply = emailUser.Count > 0 ? "" : emailUser[0];

						email.EnviarCorreo(emailCompany.ToArray(), Mensaje, "Pregunta TraceIT", CorreoReply);
					}
				}
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el envio de comentarios a TraceIT
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("sendCommentsTraceIT")]
		[HttpPost]
		[Authorize]
		public IActionResult sendCommentsTraceIT([FromBody]SendCommentsTraceITRequest request)
		{
			log.trace("sendCommentsTraceIT");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SendCommentsTraceITResponse response = new SendCommentsTraceITResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.type = 4;

				List<string> emailCompany = sql.searchEmail();

				if (emailCompany.Count > 0)
				{
					if (emailCompany[0] != "")
					{
						sql.userId = request.idUserMobile;
						sql.type = 3;
						List<string> emailUser = sql.searchEmail();

						EMAILClient email = new EMAILClient();
						String Mensaje = request.comments;
						String CorreoReply = emailUser.Count > 0 ? "" : emailUser[0];

						email.EnviarCorreo(emailCompany.ToArray(), Mensaje, "Comentarios TraceIT", CorreoReply);
					}
				}
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para generar el código de recuperación
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("recoveryPassword")]
		[HttpPost]
		[Authorize]
		public IActionResult recoveryPassword([FromBody]RecoverPasswordRequest request)
		{
			log.trace("recoveryPassword");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			RecoverPasswordResponse response = new RecoverPasswordResponse();
			log.info("Recovery password for " + request.email);
			try
			{
				Random random = new Random();
				String codeRecovery = random.Next(0, 1000000).ToString("D6");
				log.trace("codeRecovry generated successfully");
				UserMobileSQL sql = new UserMobileSQL();
				sql.email = request.email;
				sql.recoveryCode = codeRecovery;
				sql.type = 1;

				sql.saveCodeRecovery();
				log.trace("recoveryCode saved to DB");

				EMAILClient email = new EMAILClient();
				String Mensaje = "Su código de recuperación es el siguiente: " + codeRecovery;
				List<string> destinatarios = new List<string>();
				destinatarios.Add(request.email);
				email.EnviarCorreo(destinatarios.ToArray(), Mensaje, "Recuperación de Contraseña");
				log.trace("Recovery email send");
				
			}
			catch (Exception ex)
			{
				log.error("There was an exception sending the recovery email: " + ex.Message);
				if(ex.Message == "Usuario inexistente")
				{
					response.messageEng = "Username does not exist";
					response.messageEsp = "El usuario no existe";
				}
				else
				{
					response.messageEng = "An error occurred: " + ex.Message;
					response.messageEsp = "Ocurrio un error: " + ex.Message;
				}
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para realizar el cambio de contraseña
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("restorePassword")]
		[HttpPost]
		[Authorize]
		public IActionResult restorePassword([FromBody]RestorePasswordRequest request)
		{
			log.trace("restorePassword");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			RestorePasswordResponse response = new RestorePasswordResponse();

			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.email = request.email;
				sql.recoveryCode = request.recoveryCode;
				sql.password = request.password;

				sql.saveCodeRecovery();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
if(ex.Message == "Usuario inexistente")
				{
					response.messageEng = "Username does not exist";
					response.messageEsp = "El usuario no existe";
				}
				else if(ex.Message == "Código de recuperación incorrecto")
				{
					response.messageEng = "Incorrect recovery code";
					response.messageEsp = "Código de recuperación incorrecto";
				}
				else
				{
					response.messageEng = "An error occurred: " + ex.Message;
					response.messageEsp = "Ocurrio un error: " + ex.Message;
				}				
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para la busqueda de los datos de los usuarios móviles
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchUserMobileData")]
		[HttpPost]
		[Authorize]
		public IActionResult searchUserMobileData([FromBody]SearchUserMobileDataRequest request)
		{
			log.trace("searchUserMobileData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchUserMobileDataResponse response = new SearchUserMobileDataResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.userId = request.userId;

				response.dataUserMobile = sql.searchDataUserMobile();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para la busqueda de los datos de las garantias registradas
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("searchWarrantyProductData")]
		[HttpPost]
		[Authorize]
		public IActionResult searchWarrantyProductData([FromBody]SearchWarrantyProductDataRequest request)
		{
			log.trace("searchWarrantyProductData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchWarrantyProductDataResponse response = new SearchWarrantyProductDataResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.productId = request.productId;

				response.warrantiesData = sql.searchWarrantiesData();
				if(response.warrantiesData != null)
					response.warrantiesData.photoTicket = response.warrantiesData.photoTicket == null ? "" : ConfigurationSite._cofiguration["Paths:urlImagesWarranties"] + response.warrantiesData.photoTicket.Replace(".jpg", "_tmb.jpg");
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el registro y actualización del dispositivo móvil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("registerMobile")]
		[HttpPost]
		[Authorize]
		public IActionResult registerMobile([FromBody]RegisterMobileRequest request)
		{
			log.trace("registerMobile");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			RegisterMobileResponse response = new RegisterMobileResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.mobileId = request.mobileId;
				sql.model = request.model;
				sql.tokenFCM = request.tokenFCM;
				sql.userId = request.userId;
				sql.imei = request.imei;

				response.mobileId = sql.registerMobile();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para guardar los logs en la base de datos
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveLog")]
		[HttpPost]
		[Authorize]
		public IActionResult saveLog([FromBody]SaveLogRequest request)
		{
			log.trace("saveLog");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveLogResponse response = new SaveLogResponse();
			try
			{
				UserMobileSQL sql = new UserMobileSQL();
				sql.latitude = request.latitude;
				sql.longitude = request.longitude;
				sql.logName = request.logName;
				sql.mobileId = request.mobileId;
				sql.productId = request.productId;
				sql.linkId = request.linkId;
				sql.familyId = request.familyProductId;

				sql.SaveLog();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

        /// <summary>
		/// Web Method para actualizar el estado de la configuración de notificaciones
		/// Desarrollador: Oscar Ruesga
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("updateNotificationConfig")]
        [HttpPost]
        [Authorize]
        public IActionResult updateNotificationConfig([FromBody]UpdateNotificationConfigRequest request)
        {
            log.trace("updateNotificationConfig");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SaveLogResponse response = new SaveLogResponse();
            try
            {
                UserMobileSQL sql = new UserMobileSQL();
                sql.userId = request.userId;
                sql.configNotification = request.configNotification;

                sql.updateNotificationConfig();
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
response.messageEng = "An error occurred: " + ex.Message;
                response.messageEsp = "Ocurrio un error: " + ex.Message;
            }

            log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

            return Ok(response);
        }

        /// <summary>
		/// Web Method para actualizar el estado de la configuración de notificaciones
		/// Desarrollador: Oscar Ruesga
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SendNotification")]
        [HttpPost]
        public IActionResult SendNotification()
        {
            log.trace("SendNotification");
            SaveLogResponse response = new SaveLogResponse();
            try
            {
                new NotificacionesPush();
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
response.messageEng = "An error occurred: " + ex.Message;
                response.messageEsp = "Ocurrio un error: " + ex.Message;
            }

            log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

            return Ok(response);
        }
    }
}
