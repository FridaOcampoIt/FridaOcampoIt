using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using WSTraceIT.Interfaces;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Families;
using WSTraceIT.Models.Base.User;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using WS.Interfaces;

using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace WSTraceIT.Controllers
{
    [Route("Families")]
    [ApiController]
    public class FamiliesController : ControllerBase
    {

		 private LoggerD4 log = new LoggerD4("FamiliesController");
		
		/// <summary>
		/// Web Method para la aplicación móvil que realizar la consulta de la informacion de familias 
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchFamily")]
		[HttpPost]
		
		public IActionResult SearchFamily([FromBody]SearchFamilyRequest request)
		{
			
			log.trace("SearchFamily");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchFamilyResponse response = new SearchFamilyResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.barCode = request.barCode;
				sql.userMobileId = request.idUser;
				sql.latitude = request.latitude;
				sql.longitude = request.longitude;
				

				if (request.barCode != null)
				{
					FamiliesSQL donnee = new FamiliesSQL();
					donnee.barCode = request.barCode;
					StatusProd status = donnee.UneFamile();
					response.est_company = status.st_company;
					response.est_familia = status.st_familia;
					//response.est_prod = status.st_product;
					response.IdProduct2 = status.Idprod;
					if (status.Idprod < 1) {
						response.est_prod = -4;
						response.IdProduct2 = -4;
					}
				}

				if (response.est_company > 0) 
				{
					if (response.est_familia > 0)
					{
						InformationFamily infoFamily = sql.SearchFamilyMobile();

						if (infoFamily.FamilyInformation != null)
						{


							RobberySQL rSql = new RobberySQL();
							RobberyRegistryInfo responseSQL = new RobberyRegistryInfo();
							responseSQL = rSql.ObtenerProductoReportado(infoFamily.FamilyInformation.IdProduct);

							if (responseSQL.ProductId != 0 && responseSQL.ProductId == infoFamily.FamilyInformation.IdProduct)
							{
								log.info("Product stolen scanned: " + infoFamily.FamilyInformation.IdProduct);

								string username = "genérico";
								if (request.idUser != 0)
								{
									UserMobileData responseUser = new UserMobileData();
									try
									{
										UserMobileSQL userSql = new UserMobileSQL();
										userSql.userId = request.idUser;

										responseUser = userSql.searchDataUserMobile();
										username = responseUser.name + " " + responseUser.lastName;
										//Se manda el correo electronico

									}
									catch (Exception ex)
									{
										log.error("There was an error trying to get the info of the user to send the mail. " + ex.Message);
										response.messageEng = "An error occurred: " + ex.Message;
										response.messageEsp = "Ocurrio un error: " + ex.Message;
									}
								}
								try
								{
									UserMobileSQL emailList = new UserMobileSQL();
									emailList.type = 6;//to TraceIt and the company
									emailList.userId = infoFamily.FamilyInformation.IdFamily;
									List<string> correosListado = emailList.searchEmail();
									//log.trace("Sending email");
									EMAILClient correo = new EMAILClient();
									string geoPosMsg = ((request.latitude == 0) ? "Datos de geolocalización no disponibles." : "Escaneado en latitud: " + request.latitude + ", longitud: " + request.longitude + ".");
									String mensaje = "El usuario " + username + " escaneó el producto de la familia " + infoFamily.FamilyInformation.Name + " que tiene reporte de robo con CIU " + request.barCode + ".\r\n" + geoPosMsg;
									log.debug("The message: " + mensaje);
									correo.EnviarCorreo(correosListado.ToArray(), mensaje, "Escaneo de producto con reporte de robo (" + request.barCode + ")");
								}
								catch (Exception ex)
								{
									log.error("Could not send email: " + ex.Message);
								}
							}

                            List<alertas> listaAlertas = new List<alertas>();

                            try
                            {
                                listaAlertas = sql.buscarAlertas(request.barCode, infoFamily.FamilyInformation.IdFamily);
                            }
                            catch (Exception ex)
                            {
                                log.error("Error al buscar las alertas en app. " + ex.Message);
                                response.messageEng = "An error occurred: " + ex.Message;
                                response.messageEsp = "Ocurrio un error: " + ex.Message;
                            }

							response.IdFamily = infoFamily.FamilyInformation.IdFamily;
							response.IdProduct = infoFamily.FamilyInformation.IdProduct;
							response.UDID = infoFamily.FamilyInformation.UDID;
							response.Name = infoFamily.FamilyInformation.Name;
							response.Description = infoFamily.FamilyInformation.Description;
							response.DescriptionEnglish = infoFamily.FamilyInformation.DescriptionEnglish;
							response.Image = ConfigurationSite._cofiguration["Paths:urlImagesFamily"] + infoFamily.FamilyInformation.Image;
							response.Model = infoFamily.FamilyInformation.Model;
							response.Rating = (float)infoFamily.FamilyInformation.Rating;
							response.AddTicket = infoFamily.FamilyInformation.AddTicket;
							response.AllowExpiration = infoFamily.FamilyInformation.AllowExpiration;
							response.AllowWarranty = infoFamily.FamilyInformation.AllowWarranty;
							response.SKU = infoFamily.FamilyInformation.SKU;
							response.GTIN = infoFamily.FamilyInformation.GTIN;
							response.CategoryID = infoFamily.FamilyInformation.CategoryID;
							response.AggregateFamily = infoFamily.FamilyInformation.AggregateFamily;
							response.IsRated = infoFamily.FamilyInformation.IsRated;
							response.IsWarranty = infoFamily.FamilyInformation.IsWarranty;
							response.IsStolen = infoFamily.FamilyInformation.IsStolen;
							response.AddProductReference = infoFamily.FamilyInformation.AddProductReference;
							response.phoneCompany = infoFamily.FamilyInformation.phoneCompany;
							if(infoFamily.originList.Count > 0)
							{
								response.Origin = (from or in infoFamily.originList
												   select new Origin
												   {
													   Lat = or.Lat,
													   Lon = or.Lon,
													   Manufacturer = or.Manufacturer,
													   Address = or.Address,
													   Country = or.Country,
													   Ciu = or.Ciu,
													   Nombre = or.Nombre,
													   Presentacion = or.Presentacion,
													   Marca = or.Marca,
													   NumSerie = or.NumSerie,
													   Lote = or.Lote,
													   Caducidad = or.Caducidad,
													   GTIN = or.GTIN,
													   Empresa = or.Empresa,
													   Pais = or.Pais,
													   Ciudad = or.Ciudad,
													   Estado = or.Estado,
													   Planta = or.Planta,
													   LineaProduccion = or.LineaProduccion,
													   FechaProduccion = or.FechaProduccion,
													   CosecheroNombre = or.CosecheroNombre,
													   CosecheroPuesto = or.CosecheroPuesto,
													   CosecheroDescripcion = or.CosecheroDescripcion,
													   CosecheroImagen = or.CosecheroImagen == null || or.CosecheroImagen == "" ? "" : ConfigurationSite._cofiguration["Paths:urlImagesOperatorsEmpEtq"] + "1/" + or.CosecheroImagen,
													   Cosecha = or.Cosecha,
													   Sector = or.Sector,
													   Rancho = or.Rancho
												   }).ToList()[0]; //aquí se agregaron nuevos campos a la respuesta del origin.
							}
							
							response.TechnicalSpecifications = (from ts in infoFamily.TechnicalSpecification
																select new TechnicalSpecificationGroup
																{
																	Title = ts.Title,
																	TitleEnglish = ts.TitleEnglish,
																	TechnicalSpecifications = (from tsd in infoFamily.TechnicalSpecificationDetails
																							   where tsd.TechnicalSpecificationId == ts.TechnicalSpecificationId
																							   select new TechnicalSpecification
																							   {
																								   Description = tsd.Description,
																								   DescriptionEnglish = tsd.DescriptionEnglish,
																								   Name = tsd.Subtitle,
																								   NameEnglish = tsd.SubtitleEnglish,
																								   Image = tsd.Image != "" ? ConfigurationSite._cofiguration["Paths:urlImagesTecnicalSpecification"] + tsd.Image : ""

																							   }).ToList()
																}).ToList();
							response.AlternateNormalUses = (from an in infoFamily.AlternateNormalUsesList
															select new Link
															{
																IdLink = an.IdLink,
																Title = an.Title,
																Image = an.Image,
																//URL = an.Type == 1 ? ConfigurationSite._cofiguration["Paths:urlImagesLinks"] + an.URL : an.URL,
																URL = an.URL,
																Author = an.Author,
																Rate = (float)an.Rate,
																RecommendedBy = an.RecommendedBy,
																RecommendedByEnglish = an.RecommendedByEnglish,
																Type = an.Type,
																IsRated = an.IsRated

															}).ToList();
							response.InstallationGuide = (from ig in infoFamily.InstallationGuideList
														  select new Link
														  {
															  IdLink = ig.IdLink,
															  Title = ig.Title,
															  Image = ig.Image,
															  //URL = ig.Type == 1 ? ConfigurationSite._cofiguration["Paths:urlImagesLinks"] + ig.URL : ig.URL,
															  URL = ig.URL,
															  Author = ig.Author,
															  Rate = (float)ig.Rate,
															  RecommendedBy = ig.RecommendedBy,
															  RecommendedByEnglish = ig.RecommendedByEnglish,
															  Type = ig.Type,
															  IsRated = ig.IsRated

														  }).ToList();
							response.RelatedProduct = (from rp in infoFamily.RelatedProductList
													   select new Link
													   {
														   Title = rp.Title,
														   Image = ConfigurationSite._cofiguration["Paths:urlImagesLinks"] + rp.Image,
														   URL = rp.URL

													   }).ToList();
							response.ServiceCenters = (from sc in infoFamily.ServiceCenters
													   select new ServiceCenter
													   {
														   Name = sc.Name,
														   Address = sc.Address,
														   Phone = sc.Phone,
														   Country = sc.Country,
														   State = sc.State,
														   City = sc.City,
														   PostalCode = sc.PostalCode,
														   Lat = Convert.ToDouble(sc.Lat),
														   Lon = Convert.ToDouble(sc.Lon)

													   }).ToList();
							response.FAQs = (from fa in infoFamily.FAQsList
											 select new FAQ
											 {
												 Question = fa.Question,
												 QuestionEnglish = fa.QuestionEnglish,
												 Response = fa.Response,
												 ResponseEnglish = fa.ResponseEnglish

											 }).ToList();
							response.Warranties = (from wa in infoFamily.WarrantiesList
												   select new Warranty
												   {
													   Id = wa.Id,
													   Country = wa.Country,
													   PdfUrl = ConfigurationSite._cofiguration["Paths:urlImagesWarranties"] + wa.PdfUrl,
													   PeriodMonths = wa.PeriodMonths

												   }).ToList();
							response.SocialMediaLinks = (from sm in infoFamily.SocialMediaLinks
														 select new SocialMediaLinks
														 {
															 Image = ConfigurationSite._cofiguration["Paths:urlImagesSocialNetwork"] + sm.Image,
															 URL = sm.URL

														 }).ToList();

                            if (listaAlertas.Count > 0)
                            {
                                foreach (alertas alerta in listaAlertas)
                                {
                                    alerta.NombreArchivo = $"AlertFiles/{alerta.NombreArchivo}";

                                    response.listAlertas.Add(alerta);
                                }
                            }
						}
					}
					else
					{
						response.messageEng = "An error occurred the family does not active ";
						response.messageEsp = "Ocurrio un error, la familia no esta activa";
					}
				} 
				else 
				{
					response.messageEng = "An error occurred the comany does not active ";
					response.messageEsp = "Ocurrio un error, la compañia no esta activa";
				}
								
			}
			catch (Exception ex)
			{
				log.error("There was an error while scanning the code "+ request.barCode + ": " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			
			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que guarda los datos de la familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveFamily")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarFamilia)]
		public IActionResult saveFamily([FromBody]SaveFamilyRequest request)
		{
			log.trace("saveFamily");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveFamilyResponse response = new SaveFamilyResponse();
			ArchivesData archives = new ArchivesData();
			try
			{
				if(request.familyData.imageBaseFamily != null && request.familyData.imageBaseFamily != "")
				{
					Random random = new Random();

					//Se guarda la imagen de la familia del producto
					archives.archiveName = request.familyData.gtin.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + ".png";
					archives.base64 = request.familyData.imageBaseFamily;
					request.familyData.imageBaseFamily = archives.archiveName;
				}
				

				FamiliesSQL sql = new FamiliesSQL();
				sql.name = request.familyData.name;
				sql.model = request.familyData.model;
				sql.imageFamily = request.familyData.imageBaseFamily;
				sql.sku = request.familyData.sku;
				sql.barCode = request.familyData.gtin;
				sql.status = request.familyData.status;
				sql.warranty = request.familyData.warranty;
				sql.expiration = request.familyData.expiration;
				sql.addTicket = request.familyData.addTicket;
				sql.category = request.familyData.category;
				sql.company = request.familyData.company;
				sql.directionFamilies = request.directionFamily;
				sql.lifeDays = request.familyData.lifeDays;
				sql.autoLote = request.familyData.autoLote;
				sql.editLote = request.familyData.editLote;
				sql.consecutiveLote = request.familyData.consecutiveLote;
				sql.colorFamilia = request.familyData.colorFamilia;
				sql.prefix = request.familyData.prefix;

				sql.SaveFamily();

				if(archives.base64 != "" && archives.base64 != null)
				{
					ProcesarImagen image = new ProcesarImagen();
					image.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesFamily"]);
				}					
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesFamily"] + archives.archiveName))
					System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesFamily"] + archives.archiveName);

				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que guarda la especificacion tecnica
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveTechnicalSpecification")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarEspecificacion)]
		public IActionResult saveTechnicalSpecification([FromBody]SaveTechnicalSpecificationRequest request)
		{
			log.trace("saveTechnicalSpecification");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveTechnicalSpecificationResponse response = new SaveTechnicalSpecificationResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.technicalSpecifications.Add(request.technicalSpecification);
				sql.familyId = request.familyId;
				sql.option = 1;

				sql.SaveTechnicalSpecification();
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
		/// Web Method para el backoffice que guarda los detalles de la especificacion tecnica
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveTechnicalSpecificationDetails")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarEspecificacion)]
		public IActionResult saveTechnicalSpecificationDetails([FromBody]SaveTechnicalSpecificationDetailsRequest request)
		{
			log.trace("saveTechnicalSpecificationDetails");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveTechnicalSpecificationDetailsResponse response = new SaveTechnicalSpecificationDetailsResponse();
			ArchivesData archives = new ArchivesData();
			try
			{
				if (request.technicalSpecificationDetails.imageBase != null && request.technicalSpecificationDetails.imageBase != "")
				{
					Random random = new Random();

					archives.archiveName = random.Next() + ".png";					
					archives.base64 = request.technicalSpecificationDetails.imageBase;
					request.technicalSpecificationDetails.imageBase = archives.archiveName;
				}

				FamiliesSQL sql = new FamiliesSQL();
				sql.technicalSpecificationDetails.Add(request.technicalSpecificationDetails);
				sql.familyId = request.technicalSpecificationId;
				sql.option = 2;

				sql.SaveTechnicalSpecificationDetails();

				if(archives.base64 != "" && archives.base64 != null)
				{
					ProcesarImagen image = new ProcesarImagen();
					image.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"]);
				}
					
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + archives.archiveName))
					System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + archives.archiveName);

				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que guarda la garantia de la familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveWarranty")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarGarantias)]
		public IActionResult saveWarranty([FromBody]SaveWarrantyRequest request)
		{
			log.trace("saveWarranty");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveWarrantyResponse response = new SaveWarrantyResponse();
			ArchivesData archives = new ArchivesData();
			try
			{
				//ProcesarImagen image = new ProcesarImagen();

				/*if(request.warranty.pdfBase != null && request.warranty.pdfBase != "")
				{
					Random random = new Random();

					archives.archiveName = request.warranty.country.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + ".pdf";					
					archives.base64 = request.warranty.pdfBase;
					request.warranty.pdfBase = archives.archiveName;
				}*/

				FamiliesSQL sql = new FamiliesSQL();
				sql.familyId = request.familyId;
				sql.warranties.Add(request.warranty);

				sql.SaveWarranty();
				
				/*if(archives.base64 != "" && archives.base64 != null)
					image.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesWarranties"]);*/
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + archives.archiveName))
					System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + archives.archiveName);

				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que guarda las preguntas frecuentes
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveFrequentQuestions")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarPreguntas)]
		public IActionResult saveFrequentQuestions([FromBody]SaveFrequentQuestionsRequest request)
		{
			log.trace("saveFrequentQuestions");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveFrequentQuestionsResponse response = new SaveFrequentQuestionsResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.frequentQuestions.Add(request.frequentQuestions);
				sql.familyId = request.familyId;
				sql.SaveFrequentQuestions();
			}
			catch (Exception ex)
			{	
				log.error("Exception: " + ex.Message);	
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que guarda los vinculos de familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveLink")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public IActionResult saveLink([FromBody]SaveLinkRequest request)
		{
			log.trace("saveLink");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveLinkResponse response = new SaveLinkResponse();
			List<ArchivesData> archives = new List<ArchivesData>();
			try
			{
				foreach (var link in request.linkData)
				{
					/*if (link.linkTypeId == 1)
					{
						if(link.url != null && link.url != "")
						{
							Random random = new Random();
							ArchivesData dateImage = new ArchivesData();
							dateImage.archiveName = link.title.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + ".pdf";
							dateImage.base64 = link.url;
							link.url = dateImage.archiveName;
							archives.Add(dateImage);
						}
					}
					else*/ if(link.linkTypeId == 2)
					{
						if (link.thumbailUrl != null && link.thumbailUrl != "")
						{
							Random random = new Random();
							ArchivesData dateImage = new ArchivesData();
							dateImage.archiveName = link.title.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + ".png";
							dateImage.base64 = link.thumbailUrl;
							link.thumbailUrl = dateImage.archiveName;
							archives.Add(dateImage);
						}
					}
				}

				FamiliesSQL sql = new FamiliesSQL();
				sql.links = request.linkData;
				sql.sectionType = request.sectionType;
				sql.familyId = request.idFamily;
				sql.category = request.recommendedById;
				sql.userCompanyId = request.userCompanyId;

				sql.SaveLink();

				foreach (var archive in archives)
				{
					if (archive.base64 != "" && archive.base64 != null)
					{
						ProcesarImagen image = new ProcesarImagen();
						image.SaveImage(archive.base64, archive.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesLinks"]);
					}
				}				
			}
			catch (Exception ex)
			{	
				log.error("Exception: " + ex.Message);	
				foreach (var archive in archives)
				{
					//Se elimina el archivo que se intento eliminar
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + archive.archiveName))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + archive.archiveName);
				}				

				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que actualiza los datos de la familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("updateFamily")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarFamilia)]
		public IActionResult updateFamily([FromBody]UpdateFamilyRequest request)
		{
			log.trace("updateFamily");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			UpdateFamilyResponse response = new UpdateFamilyResponse();
			ArchivesData archives = new ArchivesData();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();				
				ProcesarImagen image = new ProcesarImagen();
				Random random = new Random();

				//Se obtiene el archivo actual
				sql.familyId = request.familyData.familyId;
				sql.option = 1;
				ArchivesFamiliesSQL archivesFamiliesSQL = sql.SearchArchivesFamilies().FirstOrDefault();

				//Se valida que se esta intentando actualizar la imagen para asignar un nombre al archivo
				if (request.familyData.imageBaseFamily != null && request.familyData.imageBaseFamily != "")
				{
					archives.archiveDeleteName = archivesFamiliesSQL.image;
					archives.base64 = request.familyData.imageBaseFamily;
					archives.archiveName = request.familyData.name.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + ".png";
					request.familyData.imageBaseFamily = archives.archiveName;
				}
				else
					request.familyData.imageBaseFamily = archivesFamiliesSQL.image;

				sql.familyId = request.familyData.familyId;
				sql.name = request.familyData.name;
				sql.model = request.familyData.model;
				sql.description = request.familyData.description;
				sql.descriptionEnglish = request.familyData.descriptionEnglish;
				sql.imageFamily = request.familyData.imageBaseFamily;
				sql.sku = request.familyData.sku;
				sql.barCode = request.familyData.gtin;
				sql.status = request.familyData.status;
				sql.warranty = request.familyData.warranty;
				sql.expiration = request.familyData.expiration;
				sql.addTicket = request.familyData.addTicket;
				sql.category = request.familyData.category;
				sql.company = request.familyData.company;
				sql.lifeDays = request.familyData.lifeDays;
				sql.autoLote = request.familyData.autoLote;
				sql.editLote = request.familyData.editLote;
				sql.consecutiveLote = request.familyData.consecutiveLote;
				sql.prefix = request.familyData.prefix;
				sql.directionFamilies = request.directionFamily;
				sql.colorFamilia = request.familyData.colorFamilia;
				sql.option = request.option;

				sql.UpdateFamily();

				//Se valida que se esta intentando guardar una nueva imagen
				if (archives.base64 != null && archives.base64 != "")
				{
					//Se elimina el archivo anterior
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesFamily"] + archives.archiveDeleteName))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesFamily"] + archives.archiveDeleteName);
			
					image.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesFamily"]);
				}
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
//Se elimina el archivo que se intento eliminar
				if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesFamily"] + archives.archiveName))
					System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesFamily"] + archives.archiveName);

				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que actualiza las especificaciones tecnicas
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("updateTechnicalSpecification")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarEspecificacion)]
		public IActionResult updateTechnicalSpecification([FromBody]UpdateTechnicalSpecificationRequest request)
		{
			log.trace("updateTechnicalSpecification");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			UpdateTechnicalSpecificationResponse response = new UpdateTechnicalSpecificationResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();

				//Se realizan las modificaciones en la base de datos
				sql.technicalSpecifications.Add(request.technicalSpecification);
				sql.option = 1;
				sql.UpdateTechnicalSpecification();
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
		/// Web Method para el backoffice que actualiza los datos de la especificacion tecnica detalle
		/// Desarrollador:  David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("updateTechnicalSpecificationDetails")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarEspecificacion)]
		public ActionResult updateTechnicalSpecificationDetails([FromBody]UpdateTechnicalSpecificationDetailsRequest request)
		{
			log.trace("updateTechnicalSpecificationDetails");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			UpdateTechnicalSpecificationDetailsResponse response = new UpdateTechnicalSpecificationDetailsResponse();
			ArchivesData archives = new ArchivesData();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();

				//Se revisa el nombre de la imagen actual de la especificacion tecnica
				sql.familyId = request.technicalSpecificationDetails.specificationTechnicalDetailId;
				sql.option = 2;
				ArchivesFamiliesSQL archivesFamilies = sql.SearchArchivesFamilies().FirstOrDefault();

				//Se valida si se esta tratando de guardar una imagen
				if (request.technicalSpecificationDetails.imageBase != null && request.technicalSpecificationDetails.imageBase != "")
				{
					//Se valida si tenia imagen guardado
					if (archivesFamilies != null)
						archives.archiveDeleteName = archivesFamilies.image == null ? "" : archivesFamilies.image;

					//Se guarda el base 64 en el objeto de archives
					Random random = new Random();
					archives.archiveName = random.Next() + ".png";
					archives.base64 = request.technicalSpecificationDetails.imageBase;
					request.technicalSpecificationDetails.imageBase = archives.archiveName;
				}
				//Se valida si tenia anteriormente una imagen y no actualiza la imagen
				else if (archivesFamilies != null && !request.imagenEliminado)
					request.technicalSpecificationDetails.imageBase = archivesFamilies.image == null ? "" : archivesFamilies.image;
				//Se valida que tenia una imagen anteriormente y la quiere eliminar
				else if(archivesFamilies != null && request.imagenEliminado)
				{
					archives.archiveDeleteName = archivesFamilies.image == null ? "" : archivesFamilies.image;
					request.technicalSpecificationDetails.imageBase = "";
				}
				//Se valida que no tiene una imagen anteriormente
				else
					request.technicalSpecificationDetails.imageBase = "";

				sql.technicalSpecificationDetails.Add(request.technicalSpecificationDetails);
				sql.option = 3;
				sql.UpdateTechnicalSpecificationDetails();

				//Se valida si se tiene que eliminar algun archivo
				if (archives.archiveDeleteName != "")
				{
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + archives.archiveDeleteName))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + archives.archiveDeleteName);
				}

				//Se valida que se quiera escribir un archivo
				if (archives.base64 != "")
				{
					ProcesarImagen image = new ProcesarImagen();
					image.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"]);
				}
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
//Se valida si se tiene que eliminar algun archivo
				if (archives.archiveName != "")
				{
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + archives.archiveName))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + archives.archiveName);
				}

				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que actualiza las garantias de una familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("updateWarranty")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarGarantias)]
		public IActionResult updateWarranty([FromBody]UpdateWarrantyRequest request)
		{
			log.trace("updateWarranty");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			UpdateWarrantyResponse response = new UpdateWarrantyResponse();
			ArchivesData archives = new ArchivesData();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();				

				//Se obtiene el archivo actual
				sql.familyId = request.warrantyData.warrantyId;
				sql.option = 3;
				ArchivesFamiliesSQL archivesFamiliesSQL = sql.SearchArchivesFamilies().FirstOrDefault();

				//Se valida que se esta intentando actualizar el pdf para asignar un nombre al archivo
				if (request.warrantyData.pdfBase != null && request.warrantyData.pdfBase != "")
				{
					Random random = new Random();

					archives.archiveName = request.warrantyData.country.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + ".pdf";
					archives.archiveDeleteName = archivesFamiliesSQL.image;
					archives.base64 = request.warrantyData.pdfBase;
					request.warrantyData.pdfBase = archives.archiveName;
				}
				else
					request.warrantyData.pdfBase = archivesFamiliesSQL.image;

				sql.warranties.Add(request.warrantyData);
				sql.UpdateWarranties();

				//Se valida que se esta intentando guardar una nueva imagen
				if (archives.base64 != null || archives.base64 != "")
				{
					ProcesarImagen image = new ProcesarImagen();

					//Se elimina el archivo anterior
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + archives.archiveDeleteName))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + archives.archiveDeleteName);
				
					image.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesWarranties"]);
				}
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
//Se elimina el archivo que se intento guardar
				if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + archives.archiveName))
					System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + archives.archiveName);

				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que actualiza las preguntas frecuentes
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("updateFrequentQuestions")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarPreguntas)]
		public IActionResult updateFrequentQuestions([FromBody]UpdateFrequentQuestionRequest request)
		{
			log.trace("updateFrequentQuestions");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			UpdateFrequentQuestionResponse response = new UpdateFrequentQuestionResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.frequentQuestions.Add(request.frequentQuestions);
				sql.UpdateFrequentQuestions();
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
		/// Web Method para el backoffice que actualiza un link de familias
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("updateLink")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public IActionResult updateLink([FromBody]UpdateLinkRequest request)
		{
			log.trace("updateLink");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			UpdateLinkResponse response = new UpdateLinkResponse();
			ArchivesData archives = new ArchivesData();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
			
				//Se valida si es un vinculo de tipo PDF
				if (request.linkData.linkTypeId == 1)
				{
					//Se obtiene el archivo actual
					sql.familyId = request.linkData.linkId;
					sql.option = 4;
					ArchivesFamiliesSQL archivesFamiliesSQL = sql.SearchArchivesFamilies().FirstOrDefault();

					//Se valida que se esta intentando actualizar el PDF para asignar un nombre al archivo
					if (request.linkData.url != null && request.linkData.url != "")
					{
						Random random = new Random();

						archives.archiveName = request.linkData.title.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + ".pdf";
						archives.base64 = request.linkData.url;
						archives.archiveDeleteName = archivesFamiliesSQL.image;
						request.linkData.url = archives.archiveName;
					}
					else
						request.linkData.thumbailUrl = archivesFamiliesSQL.image;
				}
				//tipo link 
				else if(request.linkData.linkTypeId == 2)
				{
					//Se obtiene el archivo actual
					sql.familyId = request.linkData.linkId;
					sql.option = 9;
					ArchivesFamiliesSQL archivesFamiliesSQL = sql.SearchArchivesFamilies().FirstOrDefault();

					//Se valida que se esta intentando actualizar el PDF para asignar un nombre al archivo
					if (request.linkData.thumbailUrl != null && request.linkData.thumbailUrl != "")
					{
						Random random = new Random();

						archives.archiveName = request.linkData.title.Replace("/", "").Replace(":", "").Replace(".", "").Replace(" ", "") + "-" + random.Next() + ".png";
						archives.base64 = request.linkData.thumbailUrl;
						archives.archiveDeleteName = archivesFamiliesSQL.image;
						request.linkData.thumbailUrl = archives.archiveName;
					}
					else
						request.linkData.thumbailUrl = archivesFamiliesSQL.image;
				}

				sql.links.Add(request.linkData);
				sql.category = request.recommendedById;
				sql.UpdateLink();

				//Se valida que se esta intentando guardar un nuevo pdf
				if (archives.base64 != null && archives.base64 != "")
				{
					ProcesarImagen image = new ProcesarImagen();

					//Se elimina el archivo anterior
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + archives.archiveDeleteName))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + archives.archiveDeleteName);
					
					image.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesLinks"]);
				}
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
if(request.linkData.linkTypeId == 1)
				{
					//Se elimina el archivo que se intento eliminar
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + archives.archiveName))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + archives.archiveName);
				}

				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que elimina la familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteFamily")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarFamilia)]
		public IActionResult deleteFamily([FromBody]DeleteFamilyRequest request)
		{
			log.trace("deleteFamily");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteFamilyResponse response = new DeleteFamilyResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.familyId = request.familyId;
				
				//se recupera los archivos de especificaciones tecnicas
				sql.option = 5;
				List<ArchivesFamiliesSQL> tecnicalEspecification = sql.SearchArchivesFamilies();

				//se recupera los archivos de vinculos
				sql.option = 6;
				List<ArchivesFamiliesSQL> link = sql.SearchArchivesFamilies();

				//se recupera los archivos de garantias
				sql.option = 7;
				List<ArchivesFamiliesSQL> warranties = sql.SearchArchivesFamilies();

				sql.DeleteFamily();

				foreach (var tec in tecnicalEspecification)
				{
					//Se elimina los archivo de especificación tecnica
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + tec.image))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + tec.image);
				}

				foreach (var li in link)
				{
					//Se elimina los archivos de los links
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + li.image))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + li.image);
				}

				foreach (var wa in warranties)
				{
					//Se elimina los archivos de las garantias
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + wa.image))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + wa.image);
				}
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			//if (response.messageEng == "") { response.messageEng = "Action done "; }
			//if (response.messageEsp == "") { response.messageEsp = "Accion realizada"; }
			
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			
			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que elimina las garantias
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteWarranties")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarGarantias)]
		public IActionResult deleteWarranties([FromBody]DeleteWarrantiesRequest request)
		{
			log.trace("deleteWarranties");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteWarrantiesResponse response = new DeleteWarrantiesResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.familyId = request.warrantyId;
				sql.option = 3;

				ArchivesFamiliesSQL archives = sql.SearchArchivesFamilies().FirstOrDefault();

				sql.DeleteWarranties();

				//Se elimina el archivo de la garantia
				if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + archives.image))
					System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesWarranties"] + archives.image);
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
		/// Web Method para el backoffice que elimina las especificaciones técnica
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteTechnicalSpecification")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarEspecificacion)]
		public IActionResult deleteTechnicalSpecification([FromBody]DeleteTechnicalSpecificationRequest request)
		{
			log.trace("deleteTechnicalSpecification");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteTechnicalSpecificationResponse response = new DeleteTechnicalSpecificationResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.option = 8;
				sql.familyId = request.technicalSpecificationId;

				List<ArchivesFamiliesSQL> archives = sql.SearchArchivesFamilies();
				sql.DeleteTechnicalSpecification();

				foreach (var tec in archives)
				{
					//Se elimina los archivo de especificación tecnica
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + tec.image))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + tec.image);
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
		/// Web Method para el backoffice que realiza la eliminacion del detalle de la especificacion tecnica
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteTechnicalSpecificationDetails")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarEspecificacion)]
		public ActionResult deleteTechnicalSpecificationDetails([FromBody]DeleteTechnicalSpecificationDetailsRequest request)
		{
			log.trace("deleteTechnicalSpecificationDetails");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteTechnicalSpecificationDetailsResponse response = new DeleteTechnicalSpecificationDetailsResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();

				sql.familyId = request.TechnicalSpecificationDetailsId;
				sql.option = 2;
				ArchivesFamiliesSQL archivesFamilies = sql.SearchArchivesFamilies().FirstOrDefault();

				sql.deleteTechnicalSpecification.Add(request.TechnicalSpecificationDetailsId);
				sql.option = 2;
				sql.DeleteTechnicalSpecificationDetails();

				//Se valida si se tiene que eliminar algun archivo
				if (archivesFamilies != null)
				{
					if(archivesFamilies.image != null && archivesFamilies.image != "")
					{
						if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + archivesFamilies.image))
							System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesTecnicalSpecification"] + archivesFamilies.image);
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
		/// Web Method para el backoffice que elimina los links
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteLink")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public IActionResult deleteLink([FromBody]DeleteLinkRequest request)
		{
			log.trace("deleteLink");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteLinkResponse response = new DeleteLinkResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.option = 4;
				sql.familyId = request.linkId;

				ArchivesFamiliesSQL archives = sql.SearchArchivesFamilies().FirstOrDefault();
				sql.DeleteLink();

				//Se elimina el archivo de link
				if(archives != null)
				{
					if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + archives.image))
						System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathImagesLinks"] + archives.image);
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
		/// Web Method para el backoffice que elimina las preguntas frecuentes
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteFrequentQuestions")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarPreguntas)]
		public IActionResult deleteFrequentQuestions([FromBody]DeleteFrequentQuestionsRequest request)
		{
			log.trace("deleteFrequentQuestions");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteFrequentQuestionsResponse response = new DeleteFrequentQuestionsResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.familyId = request.faqId;
				sql.DeleteFrequentQuestions();
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
		/// Web Method para el backoffice que consulta los datos generales de una familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchFamilyProduct")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public IActionResult searchFamilyProduct([FromBody]SearchFamilyProductRequest request)
		{
			log.trace("searchFamilyProduct");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchFamilyProductResponse response = new SearchFamilyProductResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.name = request.name;
				sql.company = request.companyId;

				response.productFamilyData = sql.SearchProductFamily();
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
		/// Web Method para el backoffice que consulta los datos para la edicion de la familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchFamilyProductDate")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarFamilia)]
		public IActionResult searchFamilyProductDate([FromBody]SearchFamilyProductDateRequest request)
		{
			log.trace("searchFamilyProductDate");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchFamilyProductDateResponse response = new SearchFamilyProductDateResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.familyId = request.familyId;
				response.productFamily = sql.SearchProductFamilyData();

				response.productFamily.productFamilyData.image = ConfigurationSite._cofiguration["Paths:urlImagesFamily"] + response.productFamily.productFamilyData.image;
			}
			catch (Exception ex)
			{	
				log.error("Exception: " + ex.Message);	
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que consulta la especificación tecnica de una familia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchTechnicalSpecificationFamily")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.visualizarEspecificacion)]
		public IActionResult searchTechnicalSpecificationFamily([FromBody]SearchTechnicalSpecificationFamilyRequest request)
		{
			log.trace("searchTechnicalSpecificationFamily");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchTechnicalSpecificationFamilyResponse response = new SearchTechnicalSpecificationFamilyResponse();

			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.familyId = request.familyId;
				TecnicalSpecificationSQL respSQL = sql.SearchTechnicalSpecification();

				response.technicalSpecifications = (from ts in respSQL.tecnicalSpecificationData
												   select new TechnicalSpecificationFamilyData
												   {
													   specificationTecnicalId = ts.specificationTecnicalId,
													   title = ts.title,
													   titleEnglish = ts.titleEnglish,
													   technicalSpecificationDetails = (from tsd in respSQL.tecnicalSpecificationDetails
																					   where tsd.specificationTecnicalId == ts.specificationTecnicalId
																					   select new TechnicalSpecificationDetailsFamilyData
																					   {
																						   description = tsd.description,
																						   descriptionEnglish = tsd.descriptionEnglish,
																						   specificationTechnicalDetailId = tsd.especificationTecnicalDetailId,
																						   image = tsd.image == null || tsd.image == "" ? "" : ConfigurationSite._cofiguration["Paths:urlImagesTecnicalSpecification"] + tsd.image,
																						   subtitle = tsd.subtitle,
																						   subtitleEnglish = tsd.subtitleEnglish
																					   }).ToList()
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
		/// Web Method para el backOffice que consulta los tipos de links de diferentes secciones
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchLinkFamily")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public IActionResult searchLinkFamily([FromBody]SearchLinkFamilyRequest request)
		{
			log.trace("searchLinkFamily");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchLinkFamilyResponse response = new SearchLinkFamilyResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.familyId = request.familyId;
				sql.option = request.sectionType;
				sql.category = request.linkType;

				LinksData sqlResp = new LinksData();
				sqlResp = sql.SearchLink();

				response.linkFamilies.limitsFamily = sqlResp.limitsFamily;
				response.linkFamilies.linkData = (from lin in sqlResp.linkData
										 select new LinkFamilyData
										 {
											 author = lin.author,
											 linkId = lin.linkId,
											 linkTypeId = lin.linkTypeId,
											 recommendedById = lin.recommendedById,
											 status = lin.status,
											 thumbailUrl = request.sectionType == 5 ? ConfigurationSite._cofiguration["Paths:urlImagesLinks"] + lin.thumbailUrl: lin.thumbailUrl,
											 title = lin.title,
											 //url = lin.linkTypeId == 1 ? ConfigurationSite._cofiguration["Paths:urlImagesLinks"] + lin.url : lin.url, //no longer a local url
											 url = lin.url,
											 recommendedBy = lin.recommendedBy
										 }).ToList();
			}
			catch (Exception ex)
			{	
				log.error("Exception: " + ex.Message);	
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que consulta las garantias y preguntas frecuentes
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchWarrantiesFaq")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.visualizarGarantiasServicio)]
		public IActionResult searchWarrantiesFaq([FromBody]SearchWarrantiesFaqRequest request)
		{
			log.trace("searchWarrantiesFaq");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchWarrantiesFaqResponse response = new SearchWarrantiesFaqResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.familyId = request.familyId;

				WarrantiesFaqFamily responseSQL = sql.SearchWarrantiesFaq();
				response.warrantiesFaq.frequentQuestions = responseSQL.frequentQuestions;
				response.warrantiesFaq.limitsFamily = responseSQL.limitsFamily;
				response.warrantiesFaq.warranties = (from war in responseSQL.warranties
													 select new WarrantiesFamilyData
													 {
														 country = war.country,
														 periodMonths = war.periodMonths,
														 urlPdf = ConfigurationSite._cofiguration["Paths:urlImagesWarranties"] + war.urlPdf,
														 warrantyId = war.warrantyId
													 }).ToList();
			}
			catch (Exception ex)
			{	
				log.error("Exception: " + ex.Message);	
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que consulta todos los combos que seran utilizados dentro del modulo
		/// Desarollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchDropDownListFamily")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public IActionResult searchDropDownListFamily([FromBody]SearchDropDownListFamilyRequest request)
		{
			log.trace("searchDropDownListFamily");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDropDownListFamilyResponse response = new SearchDropDownListFamilyResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.company = request.idCompany;
				sql.option = request.option;

				FamilyListDropDownSQL respSQL = sql.SearchFamilyListDropDown();
				response.addressData = respSQL.addressData;
				response.categoryData = respSQL.categoryData;
				response.companyData = respSQL.companyData;
				response.recommendedBy = respSQL.recommendedBy;
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
		/// Web Method para el backoffice que consulta la api de youtube para la consulta de videos
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("youtubeSearch")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public ActionResult youtubeSearch([FromBody]YoutubeSearchRequest request)
		{
			log.trace("youtubeSearch");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			YoutubeSearchResponse response = new YoutubeSearchResponse();
			try
			{
				var youtubeService = new YouTubeService(new BaseClientService.Initializer()
				{
					ApiKey = ConfigurationSite._cofiguration["GoogleAPI:ApiKey"],
					ApplicationName = this.GetType().ToString()
				});

				var searchListRequest = youtubeService.Search.List("snippet");
				searchListRequest.Q = request.dataFilter;
				searchListRequest.MaxResults = 50;
				searchListRequest.Type = "video";

				var searchListResponse = searchListRequest.Execute();

				foreach (var searchResult in searchListResponse.Items)
				{
					response.youtubeData.Add(new YoutubeData
					{
						channelTitle = searchResult.Snippet.ChannelTitle,
						thumbnails = searchResult.Snippet.Thumbnails.Default__.Url,
						title = searchResult.Snippet.Title,
						videoId = searchResult.Id.VideoId
					});
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
		/// Web Method para el backoffice y móvil para consultar los paises existentes
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("searchCountries")]
		[HttpPost]
		[Authorize]
		public ActionResult searchCountries()
		{
			log.trace("searchCountries");
			SearchCountriesResponse response = new SearchCountriesResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				response.countriesData = sql.searchCountries();
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
		/// Web Method para el backoffice y móvil para consultar los estados existentes
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		[Route("searchEstadoByPaisId")]
		[HttpPost]
		[Authorize]
		public ActionResult searchEstados([FromBody] SearchEstadosByPaisIdRequest request)
		{
			log.trace("searchEstadoByPaisId");
			SearchEstadosByPaisIdResponse response = new SearchEstadosByPaisIdResponse();
			try
			{
				FamiliesSQL sql = new FamiliesSQL();
				sql.paisId = request.paisId;
				response.estadosData = sql.searchEstadobyPaisId();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que obtiene los embalajes de una familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("getPackaging")]
		[HttpPost]
		public IActionResult getPackaging([FromBody] SearchPackagingRequest request)
		{
			log.trace("getPackaging");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchPackagingResponse response = new SearchPackagingResponse();
			try
			{
				PackagingSQL sql = new PackagingSQL();
				sql.familyId = request.familyId;
				sql.packagingId = request.packagingId;

				response.packagingList = sql.SearchPackagingFamilies();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice para guardar un embalaje de una familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("savePackaging")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public IActionResult SavePackaging([FromBody] SavePackagingRequest request)
		{
			log.trace("savePackaging");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SavePackagingResponse response = new SavePackagingResponse();
			try
			{
				if (request.packagingType.Trim() == "") {
					response.messageEng = "The field Packaging type is required";
					response.messageEsp = "El campo Tipo de empaque es requerido";
				} else { 
					PackagingSQL sql = new PackagingSQL();
					sql.packagingId = request.packagingId;
					sql.packagingType = request.packagingType;
					sql.readingType = request.readingType;
					sql.boxLabelType = request.boxLabelType;
					sql.boxLabelPallet = request.boxLabelPallet;
					sql.unitsPerBox = request.unitsPerBox;
					sql.copiesPerBox = request.copiesPerBox;
					sql.linesPerBox = request.linesPerBox;
					sql.grossWeightPerBox = request.grossWeightPerBox;
					sql.dimensionsWeightPerBox = request.dimensionsWeightPerBox;
					sql.boxesPerPallet = request.boxesPerPallet;
					sql.copiesPerPallet = request.copiesPerPallet;
					sql.grossWeightPerPallet = request.grossWeightPerPallet;
					sql.dimensionsPerPallet = request.dimensionsPerPallet;
					sql.instructionsWarnings = request.instructionsWarnings;
					sql.familyId = request.familyId;
					sql.empacadoresId = request.empacadoresId;
					sql.auxEmpacadoresId = request.auxEmpacadoresId;
					sql.sp = sql.packagingId > 0 ? "spu_edicionEmbalajeFamilia" : "spi_guardarEmbalajeFamilia";

					sql.SavePackagingFamily();
				}
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para eliminar un embalaje de una familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deletePackaging")]
		[HttpPost]
		[Authorize]
		public IActionResult deletePackaging([FromBody] DeletePackagingRequest request)
		{
			log.trace("deletePackaging");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeletePackagingResponse response = new DeletePackagingResponse();
			try
			{
				PackagingSQL sql = new PackagingSQL();
				sql.packagingId = request.packagingId;

				sql.DeletePackaging();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice que obtiene los embalajes de reproceso de una familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns>List</returns>
		[Route("getPackagingReprocess")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public IActionResult getPackagingReprocess([FromBody] SearchPackagingRequest request)
		{
			log.trace("getPackagingReprocess");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchPackagingResponse response = new SearchPackagingResponse();
			try
			{
				PackagingSQL sql = new PackagingSQL();
				sql.familyId = request.familyId;
				sql.packagingId = request.packagingId;
				sql.isReproceso = true;

				response.packagingList = sql.SearchPackagingFamilies();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para el backoffice para guardar un embalaje de reproceso de una familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns>response</returns>
		[Route("savePackagingReprocess")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloFamilia)]
		public IActionResult savePackagingReprocess([FromBody] SavePackagingRequest request)
		{
			log.trace("savePackagingReprocess");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SavePackagingResponse response = new SavePackagingResponse();
			try
			{
				if (request.packagingType.Trim() == "")
				{
					response.messageEng = "The field Packaging type is required";
					response.messageEsp = "El campo Tipo de empaque es requerido";
				}
				else
				{
					PackagingSQL sql = new PackagingSQL();
					sql.packagingId = request.packagingId;
					sql.packagingType = request.packagingType;
					sql.readingType = request.readingType;
					sql.boxLabelType = request.boxLabelType;
					sql.boxLabelPallet = request.boxLabelPallet;
					sql.unitsPerBox = request.unitsPerBox;
					sql.copiesPerBox = request.copiesPerBox;
					sql.linesPerBox = request.linesPerBox;
					sql.grossWeightPerBox = request.grossWeightPerBox;
					sql.dimensionsWeightPerBox = request.dimensionsWeightPerBox;
					sql.boxesPerPallet = request.boxesPerPallet;
					sql.copiesPerPallet = request.copiesPerPallet;
					sql.grossWeightPerPallet = request.grossWeightPerPallet;
					sql.dimensionsPerPallet = request.dimensionsPerPallet;
					sql.instructionsWarnings = request.instructionsWarnings;
					sql.familyId = request.familyId;
					sql.sp = sql.packagingId > 0 ? "spu_edicionEmbalajeReprocesoFamilia" : "spi_guardarEmbalajeReprocesoFamilia";

					sql.SavePackagingFamily();
				}
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para eliminar un embalaje de reproceso de una familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns>response</returns>
		[Route("deletePackagingReprocess")]
		[HttpPost]
		[Authorize]
		public IActionResult deletePackagingReprocess([FromBody] DeletePackagingRequest request)
		{
			log.trace("deletePackagingReprocess");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeletePackagingResponse response = new DeletePackagingResponse();
			try
			{
				PackagingSQL sql = new PackagingSQL();
				sql.packagingId = request.packagingId;
				sql.isReproceso = true;

				sql.DeletePackaging();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}
	}
}