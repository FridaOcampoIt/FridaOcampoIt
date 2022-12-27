using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using NetCoreApiRest.Utils;
using Microsoft.AspNetCore.Authorization;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Production;

namespace WSTraceIT.Controllers
{
	[Route("Production")]
	[ApiController]
	public class ProductionController : ControllerBase
	{

		private LoggerD4 log = new LoggerD4("ProductionController");


		[Route("saveOperationData")]
		[HttpPost]
		
		public IActionResult saveOperationData([FromBody]OperationDataRequest request)
		{
			log.trace("saveOperationData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));

			ProductionLineResponses response = new ProductionLineResponses();
			try
			{
				string lat = User.Claims.FirstOrDefault(u => u.Type == "Latitude").Value.ToString();
				string lon = User.Claims.FirstOrDefault(u => u.Type == "Longitude").Value.ToString();
				ProductionSQL production = new ProductionSQL();
				response = production.saveOperationData(request, lat, lon);

			}
			catch (Exception e)
			{
				log.error("Exception: " + e.Message);
				response.success = false;
				response.messageEng = e.Message;
				response.messageEsp = e.Message;
			}

			return Ok(response);
		}

		/// <summary>
		/// Web Method para realizar el guardado de operaciones mediante un job
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveOperationDataJob")]
		[HttpPost]
		
		public IActionResult saveOperationDataJob([FromBody]OperationDataRequest request)
		{
			log.trace("saveOperationDataJob");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));

			ProductionLineResponses response = new ProductionLineResponses();
			try
			{
				ProductionSQL production = new ProductionSQL();
				string lat = User.Claims.FirstOrDefault(u => u.Type == "Latitude").Value.ToString();
				string lon = User.Claims.FirstOrDefault(u => u.Type == "Longitude").Value.ToString();

				// Validar la existencia de la operación
				int findOperation = production.findOperationJob(request.localId);
				
				if(findOperation == 0) {
					response = production.saveOperationData(request, lat, lon);
                }
				response.success = true;
			}
			catch (Exception e)
			{
				response.success = false;
				response.messageEng = e.Message;
				response.messageEsp = e.Message;
			}

			return Ok(response);
		}

		[Route("getSummaryOperation")]
		[HttpPost]
		[Authorize]
		public IActionResult getSummaryOperation([FromBody]OperationDaySummaryRequest request)
		{
			log.trace("getSummaryOperation");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			ProductionLineSummaryResponse response = new ProductionLineSummaryResponse();
			try
			{
				ProductionSQL production = new ProductionSQL();
				response.summary = production.getDayOperation(request.idCompany, request.idProvider);
			}
			catch (Exception e)
			{
				response.messageEng = e.Message;
				response.messageEsp = e.Message;
			}
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);

		}

		[Route("getScannedDetail")]
		[HttpPost]
		[Authorize]
		public IActionResult getScannedDetail([FromBody]ScannedDetailsRequest request)
		{
			log.trace("getScannedDetail");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			ScannedDetailResponse response = new ScannedDetailResponse();
			try
			{
				ProductionSQL production = new ProductionSQL();
				response.details = production.getScannedDetail(request.idCompany, request.code);
			}
			catch (Exception e)
			{
				response.messageEng = e.Message;
				response.messageEsp = e.Message;
			}
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);

		}

		[Route("saveWastageData")]
		[HttpPost]
		[Authorize]
		public IActionResult saveWastageData([FromBody]SaveWastageRequest request)
		{
			log.trace("saveWastageData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			ProductionLineResponses response = new ProductionLineResponses();
			try
			{
				ProductionSQL production = new ProductionSQL();
				production.saveWastageMaterial(request.idCompany, request.wasteMaterial);
				response.success = true;
			}
			catch (Exception e)
			{
				response.messageEng = e.Message;
				response.messageEsp = e.Message;
			}
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);

		}

		/// <summary>
		/// Guardar datos de cambio de rollo.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveRollChange")]
		[HttpPost]
		[Authorize]
		public IActionResult saveRollChange([FromBody] SaveRollChangeRequest request)
		{
			log.trace("saveRollChange");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			ProductionLineChangeRollResponse response = new ProductionLineChangeRollResponse();
            try
            {
				ProductionSQL production = new ProductionSQL();
				response.operacionId = production.saveChangeRoll(request);
            }
            catch (Exception ex)
            {
				response.messageEng = ex.Message;
				response.messageEsp = ex.Message;
                
            }

			return Ok(response);
        }


		/// <summary>
		/// Solicitar datos de una operacion
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("getOperationData")]
		[HttpPost]
        //[Authorize]
        public IActionResult getOperationData([FromBody] OperationRequest request)
		{
			log.trace("getOperationData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			OperationDataResponse response = new OperationDataResponse();
			try
			{
				ProductionSQL production = new ProductionSQL();

				//long ciu = Convert.ToInt64(request.ciu, 16);
				bool isHex = false;
                try
                {
					ulong.Parse(request.ciu, System.Globalization.NumberStyles.HexNumber);
					isHex = true;
                }
                catch (Exception ex)
                {
					isHex = false;
                }

				OperationDatas operationDatas = production.getOperation(request, isHex, request.pallet, request.type);

				//Armado de respuestaJSON para ID
				response.startDate = operationDatas.operationInfo.startDate;
				response.endDate = operationDatas.operationInfo.endDate;
				response.Company = operationDatas.operationInfo.Company;
				response.Operator = operationDatas.operationInfo.Operator;
				response.Line = operationDatas.operationInfo.Line;
				response.Product = operationDatas.operationInfo.Product;
				response.Package = operationDatas.operationInfo.Package;
				response.grouping = operationDatas.operationInfo.grouping;
				response.totalUnits = operationDatas.operationInfo.totalUnits;
				response.range = operationDatas.operationInfo.range;
				response.unitsScanned = operationDatas.operationInfo.unitsScanned;
				response.isGroup = operationDatas.operationInfo.isGroup;
				response.idOperation = operationDatas.operationInfo.idOperation;
				response.etiquetaId = operationDatas.operationInfo.EtiquetaID;

				RawMaterial a;
				foreach (var raw in operationDatas.rawMaterialsInfo)
                {
					a = new RawMaterial();
					a.lot = raw.lot;
					a.product = raw.product;
					a.provider = raw.provider;
					response.rawMaterials.Add(a);
                }


				OperationDetail op;
				var count = 1;
				foreach (var ope in operationDatas.operationDetailsInfo)
                {
					op = new OperationDetail();
					op.box = ope.box;
					op.grouping = "";
					op.line = ope.line;
					op.operatorName = ope.operatorName;
					op.pallet = ope.pallet;
					op.range = ope.range;
					string[] scanneds = ope.scanned.Split(',');
                    foreach (string scann in scanneds)
                    {
                        if (count == 1)
                        {
							var first = op.range.Split('-')[0];
							op.scanned.Add(first);
                        }
						op.scanned.Add(scann);
						count++;
					}
					response.operation.Add(op);
                }


			}
            catch (Exception ex)
            {

				response.messageEng = ex.Message;
				response.messageEsp = ex.Message;

			}
			return Ok(response);
        }

		[Route("getOperationDataCode")]
		[HttpPost]
		//[Authorize]
		public IActionResult getOperationDataCode([FromBody] OperationCodeRequest request)
		{
			log.trace("getOperationData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			LabelDataCodeResponse response = new LabelDataCodeResponse();
			try
            {
				ProductionSQL production = new ProductionSQL();

				production.getLabelDataCode(response, request.code, request.type);

			}
            catch (Exception ex)
            {
				response.messageEsp = ex.Message;
            }
			return Ok(response);
        }

		public IActionResult getLabelData([FromBody] LabelDataRequest request)
		{

			log.trace("getLabelData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			LabelDataResponse response = new LabelDataResponse();
            try
            {
				ProductionSQL production = new ProductionSQL();

            }
            catch (Exception ex)
            {
				response.messageEng = ex.Message;
				response.messageEsp = ex.Message;
			}

			return Ok(response);
        }

		/// <summary>
		/// Obtener la merma del usuario de acuerdo a la compañia seleccionada
		/// </summary>
		/// <param name="request"></param>
		/// <returns>Object</returns>
		[Route("getWasteByCompany")]
		[HttpPost]
		[Authorize]
		public IActionResult getWasteByCompany([FromBody] CompanyWasteRequest request)
		{
			log.trace("getWasteByCompany");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			CompanyWasteResponse response = new CompanyWasteResponse();
			try
			{
				ProductionSQL production = new ProductionSQL();
				int idUser = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);// id del usuario logueado
				response.waste = production.getWasteByCompany(request.companyId, idUser);
			}
			catch (Exception e)
			{
				response.waste = 0;
				log.trace("getWasteByCompany error: " + e.Message);
			}
			return Ok(response);

		}
	}
}