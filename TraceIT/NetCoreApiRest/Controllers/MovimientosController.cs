using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreApiRest.Utils;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSTraceIT.Models.Base.Movimientos;

using System.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using WS.Interfaces;
using WSTraceIT.Interfaces;
using WSTraceIT.Models.Base.Families;
using Google.Apis.Util;

namespace WSTraceIT.Controllers
{
	[Route("Movimientos")]
	[ApiController]
	public class MovimientosController : ControllerBase
	{
		LoggerD4 log = new LoggerD4("MovimientosController");

		#region Busqueda para tabla del catalogo movimientos
		/// <summary>
		/// Web Method para buscar un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchMovimiento")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchMovimiento([FromBody] SearchMovimientoRequest request)
		{
			log.trace("searchMovimiento");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchMovimientoResponse response = new SearchMovimientoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.producto = request.producto;
				sql.tipoMovimiento = request.tipoMovimientoId;
				sql.fechaCaducidad = request.fechaCaducidad;
				sql.fechaIngresoDe = request.fechaIngresoDe;
				sql.fechaIngresoHasta = request.fechaIngresoHasta;
				sql.usuario = request.usuario;
				sql.acopiosId = request.acopiosId;

				response.movimientosDataList = sql.SearchMovimientos();
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
		#endregion


		#region Busqueda para tabla del catalogo movimientos por acopio
		/// <summary>
		/// Web Method para buscar un movimiento
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		[Route("searchMovimientoByAcopioId")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchMovimientoByAcopioId([FromBody] SearchMovimientoByAcopioIdRequest request)
		{
			log.trace("searchMovimiento");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchMovimientoResponse response = new SearchMovimientoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.producto = request.producto;
				sql.tipoMovimiento = request.tipoMovimientoId;
				sql.fechaIngresoDe = request.fechaIngresoDe;
				sql.fechaIngresoHasta = request.fechaIngresoHasta;
				sql.acopioId = request.acopioId;

				response.movimientosDataList = sql.SearchMovimientosByAcopioId();
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
		#endregion

		#region Busqueda de los datos para los combos que servirán de filtros para el catalogo movimientos
		/// <summary>
		/// Web Method para buscar los tipos de movimientos y productos para el combo del catalogo movimientos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>

		[Route("searchMovimientoDropDown")]
		[HttpPost]
		public IActionResult searchMovimientoDropDown([FromBody] SearchComboMovimientoRequest request)
		{
			log.trace("searchMovimientoDropDown");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchComboMovimientoResponse response = new SearchComboMovimientoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.tipoMovimiento = request.tipoMovimiento;

				TipoMovimientoDataCombo respSQL = sql.SearchCombosMovimiento();
				response.tiposMovimientosDataComboList = respSQL.tiposMovimientosDataComboList;
				response.productosDataComboList = respSQL.productosDataComboList;
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
		#endregion

		#region Busqueda de datos pertenecientes a un solo movimento (separados)
		/// <summary>
		/// Web Method para buscar un movimiento etiqueta creada
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataEtiqueta")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataEtiqueta([FromBody] SearchDataEtiquetaRequest request)
		{
			log.trace("searchDataEtiqueta");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoGeneralResponse response = new SearchDataMovimientoGeneralResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.nombreAgrupacion = request.nombreAgru;
				sql.referenciaInterna = request.referenciaInt;
				sql.referenciaExterna = request.referenciaExt;

				response.movimientosDataGeneralRecepList = sql.SearchMovimientoEtiqueta();
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
		/// Web Method para buscar un movimiento general
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoGeneral")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		public IActionResult searchDataMovimientoGeneral([FromBody] SearchDataMovimientoGeneralRequest request)
		{
			log.trace("searchDataMovimientoGeneral");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoGeneralResponse response = new SearchDataMovimientoGeneralResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.movimientosDataGeneralRecepList = sql.SearchMovimientoGeneral();
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
		/// Web Method para buscar un movimiento general
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoGeneralRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataMovimientoGeneralRecep([FromBody] SearchDataMovimientoGeneralRecepRequest request)
		{
			log.trace("searchDataMovimientoGeneralRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoGeneralRecepResponse response = new SearchDataMovimientoGeneralRecepResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;
				sql.codigoI = request.codigoI;
				sql.codigoF = request.codigoF;

				response.movimientosDataGeneralRecepList = sql.SearchMovimientoGeneralRecep();
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
		/// Web Method para buscar un movimiento general
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoGeneralRecep2")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataMovimientoGeneralRecep2([FromBody] SearchDataMovimientoGeneralRecepRequest request)
		{
			log.trace("searchDataMovimientoGeneralRecep2");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoGeneralRecepResponse2 response = new SearchDataMovimientoGeneralRecepResponse2();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;
				sql.codigoI = request.codigoI;
				sql.codigoF = request.codigoF;
				sql.isAgro = request.isAgro;

				response.movimientosDataGeneralRecepList = sql.SearchMovimientoGeneralRecep2();
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
		/// Web Method para buscar las cajas de un pallet
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param>request</param>
		/// <returns>ListQr</returns>
		[Route("searchDataMovimientoGeneralProdsPallet")]
		[HttpPost]
		[Authorize]
		public IActionResult searchDataMovimientoGeneralProdsPallet([FromBody] SearchDataMovimientoGeneralProdsPalletRequest request)
		{
			log.trace("searchDataMovimientoGeneralProdsPallet");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoGeneralProdsPalletResponse response = new SearchDataMovimientoGeneralProdsPalletResponse();
			Console.Write($"CodigoI:{request.codigoI}, CodigoF:{request.codigoF}, npallet:{request.nPallet}, isAgro:{request.isAgro} \n \n \n");
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoI = request.codigoI;
				sql.codigoF = request.codigoF;
				sql.npallet = request.nPallet;
				sql.isAgro = request.isAgro;

				response.cajasPalletList = sql.SearchMovimientoGeneralCajasPallet();
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
		/// Web Method para buscar un movimiento por algún texto del codigo QR
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param>request</param>
		/// <returns>codigoQR</returns>
		[Route("searchMovimientoByCode")]
		[HttpPost]
		[Authorize]
		public IActionResult searchMovimientoByCode([FromBody] SearchMovimientoCodeRequest request)
		{
			log.trace("searchMovimientoByCode");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			Console.WriteLine($"codigoQR:{request.codigoQR}, isAgro:{request.isAgro}, isRecibe:{request.isRecibe} \n \n \n");
			SearchMovimientoCodeResponse response = new SearchMovimientoCodeResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;
				sql.isAgro = request.isAgro;
				sql.isRecibe = request.isRecibe;

				response.codigoQR = sql.searchMovimientoByCode();
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
		/// Web Method para obtener los embalajes por familia en el envío
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param>request</param>
		/// <returns>List</returns>
		[Route("searchPackagingByProduct")]
		[HttpPost]
		[Authorize]
		public IActionResult searchPackagingByProduct([FromBody] SearchPackagingByfamilyRequest request)
		{
			log.trace("searchPackagingByProduct");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchPackagingByfamilyResponse response = new SearchPackagingByfamilyResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.ids = request.ids;
				sql.isAgro = request.isAgro;

				response.packagingLst = sql.SearchEmbalajesPorFamilia();
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
		/// Web Method para buscar los producstos de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoGeneralProducto")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataMovimientoGeneralProducto([FromBody] SearchDataMovimientoGeneralProdRequest request)
		{
			log.trace("searchDataMovimientoGeneralProducto");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoGeneralProdResponse response = new SearchDataMovimientoGeneralProdResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.codigoI = request.codigoI;
				sql.codigoF = request.codigoF;
				sql.codigoTipo = request.codigoTipo;
				sql.codigoIHEXA = request.codigoIHEXA;
				sql.codigoFHEXA = request.codigoFHEXA;
				sql.totalProductosQR = request.totalProductosQR;
				sql.isHexa = request.isHexa;
				sql.isAgro = request.isAgro;

				SearchDataMovimientoGeneralProdData respSQL = sql.SearchMovimientoGeneralProd();
				response.movimientosDataGeneralProdRecepList = respSQL.movimientosDataGeneralProdRecepList;
				//response.movimientosDataGeneralDesProdRecepList = respSQL.movimientosDataGeneralDesProdRecepList;
				//response.movimientosDataGeneralUnicoProdRecepList = respSQL.movimientosDataGeneralUnicoProdRecepList;
				//response.movimientosDataGeneralOperaProdRecepList = respSQL.movimientosDataGeneralOperaProdRecepList;
				//response.movimientosDataGeneralReagrupadoProdRecepList = respSQL.movimientosDataGeneralReagrupadoProdRecepList;
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
		/// Web Method para buscar los producstos de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoGeneralProductoRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataMovimientoGeneralProductoRecep([FromBody] SearchDataMovimientoGeneralProdRecepRequest request)
		{
			log.trace("searchDataMovimientoGeneralProductoRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoGeneralProdRecepResponse response = new SearchDataMovimientoGeneralProdRecepResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;
				sql.codigoI = request.codigoI;
				sql.codigoF = request.codigoF;
				sql.isHexa = request.isHexa;
				sql.codigoIHEXA = request.codigoIHEXA;
				sql.codigoFHEXA = request.codigoFHEXA;
				sql.familiaProductoId = request.familiaProductoId;
				sql.productoMovimientoId = request.productoMovimientoId;
				sql.isAgro = request.isAgro;

				SearchDataMovimientoGeneralProdData respSQL = sql.SearchMovimientoGeneralProdRecep();
				response.movimientosDataGeneralProdRecepList = respSQL.movimientosDataGeneralProdRecepList;
				response.movimientosDataGeneralDesProdRecepList = respSQL.movimientosDataGeneralDesProdRecepList;
				response.movimientosDataGeneralUnicoProdRecepList = respSQL.movimientosDataGeneralUnicoProdRecepList;
				response.movimientosDataGeneralOperaProdRecepList = respSQL.movimientosDataGeneralOperaProdRecepList;

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
		/// Web Method para buscar un movimiento general
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataObservcion")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDataObservcion([FromBody] SearchDataMovimientoObservacionesRequest request)
		{
			log.trace("searchDataObservcion");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoObservacionResponse response = new SearchDataMovimientoObservacionResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.movimientosDataObservacionRecepList = sql.SearchMovimientoObservaciones();
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
		/// Web Method para buscar un movimiento general
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataObservcionRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataObservcionRecep([FromBody] SearchDataMovimientoObservacionesRecepRequest request)
		{
			log.trace("searchDataObservcionRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoObservacionResponse response = new SearchDataMovimientoObservacionResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.movimientosDataObservacionRecepList = sql.SearchMovimientoObservacionesRecep();
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
		/// Web Method para buscar un movimiento general
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataNombreRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataNombreRecep([FromBody] SearchDataMovimientoNombreRecepRequest request)
		{
			log.trace("searchDataNombreRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoNombreResponse response = new SearchDataMovimientoNombreResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoCompleto = request.codigoCompleto;

				response.movimientosDataNombreRecepList = sql.SearchMovimientoNombreRecep();
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
		/// Web Method para buscar un movimiento remitente
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoRemitente")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDataMovimientoRemitente([FromBody] SearchDataMovimientoRemitenteRequest request)
		{
			log.trace("searchDataMovimientoRemitente");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoRemitenteResponse response = new SearchDataMovimientoRemitenteResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				//Comento la compañia, porque parece ser que este valor esta cuasando lentitud en el back
				//sql.usuario = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
				sql.company = request.company;
				Console.WriteLine(request.company);
				response.movimientosDataRemitenteRecepList = sql.SearchMovimientoRemitente();
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
		/// Web Method para buscar un movimiento remitente
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoRemitenteRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataMovimientoRemitenteRecep([FromBody] SearchDataMovimientoRemitenteRecepRequest request)
		{
			log.trace("searchDataMovimientoRemitenteRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoRemitenteRecepResponse response = new SearchDataMovimientoRemitenteRecepResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.movimientosDataRemitenteRecepList = sql.SearchMovimientoRemitenteRecep();
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
		/// Web Method para buscar un movimiento destinatario
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoDestinatario")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDataMovimientoDestinatario([FromBody] SearchDataMovimientoDestinatarioRequest request)
		{
			log.trace("searchDataMovimientoDestinatario");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoDestinatarioResponse response = new SearchDataMovimientoDestinatarioResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.numeroC = request.numeroC.Trim();
				sql.usuario = User.Identity.IsAuthenticated ? Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value) : 0;

				response.movimientosDataDestinatarioRecepList = sql.SearchMovimientoDestinatario();
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
		/// Web Method para buscar un movimiento destinatario
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoDestinatarioRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataMovimientoDestinatarioRecep([FromBody] SearchDataMovimientoDestinatarioRecepRequest request)
		{
			log.trace("searchDataMovimientoDestinatarioRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoDestinatarioRecepResponse response = new SearchDataMovimientoDestinatarioRecepResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.movimientosDataDestinatarioRecepList = sql.SearchMovimientoDestinatarioRecep();
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
		/// Web Method para buscar un movimiento transportista
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoTransportista")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDataMovimientoTransportista([FromBody] SearchDataMovimientoTransportistaRequest request)
		{
			log.trace("searchDataMovimientoTransportista");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoTransportistaResponse response = new SearchDataMovimientoTransportistaResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.movimientosDataTransportistaList = sql.SearchMovimientoTransportista();
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
		/// Web Method para buscar un movimiento transportista
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoTransportistaRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataMovimientoTransportistaRecep([FromBody] SearchDataMovimientoTransportistaRecepRequest request)
		{
			log.trace("searchDataMovimientoTransportistaRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoTransportistaResponse response = new SearchDataMovimientoTransportistaResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.movimientosDataTransportistaList = sql.SearchMovimientoTransportistaRecep();
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
		/// Web Method para buscar un movimiento transportista
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoInfoLegal")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDataMovimientoInfoLegal([FromBody] SearchDataMovimientoInfoLegalRequest request)
		{
			log.trace("searchDataMovimientoInfoLegal");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoInfoLegalResponse response = new SearchDataMovimientoInfoLegalResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.movimientosDataInfoLegalRecepList = sql.SearchMovimientoInfoLegal();
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
		/// Web Method para buscar un movimiento transportista
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoInfoLegalRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataMovimientoInfoLegalRecep([FromBody] SearchDataMovimientoInfoLegalRecepRequest request)
		{
			log.trace("searchDataMovimientoInfoLegalRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoInfoLegalRecepResponse response = new SearchDataMovimientoInfoLegalRecepResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.movimientosDataInfoLegalRecepList = sql.SearchMovimientoInfoLegalRecep();
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
		/// Web Method para buscar un movimiento transportista
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoInfoEtiqueta")]
		[HttpPost]
		[Authorize]
		public IActionResult searchDataMovimientoInfoEtiqueta([FromBody] SearchDataMovimientoInfoEtiquetaRequest request)
		{
			log.trace("searchDataMovimientoInfoEtiqueta");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoInfoEtiquetaResponse response = new SearchDataMovimientoInfoEtiquetaResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.movimientosDataInfoEtiquetaList = sql.SearchMovimientoInfoEtiqueta();
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
		/// Web Method para buscar la merma de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDataMovimientoMerma")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDataMovimientoMerma([FromBody] SearchDataMovimientoMermaRequest request)
		{
			log.trace("searchDataMovimientoMerma");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDataMovimientoMermaResponse response = new SearchDataMovimientoMermaResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoId = request.codigoId;

				SearchDataMovimientoMermaData respSQL = sql.SearchMovimientoMerma();
				response.movimientosDataMermaList = respSQL.movimientosDataMermaList;
				//response.movimientosDataGeneralReagrupadoProdRecepList = respSQL.movimientosDataGeneralReagrupadoProdRecepList;
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
		#endregion

		#region Busqueda de los datos para los combos de pais y estado de remitente y destinatario
		/// <summary>
		/// Web Method para buscar los paises y estados para combos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>

		[Route("searchPaisEstadoDropDown")]
		[HttpPost]
		public IActionResult searchPaisEstadoDropDown([FromBody] SearchComboPaisEstadoRequest request)
		{
			log.trace("searchPaisEstadoDropDown");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchComboPaisEstadoResponse response = new SearchComboPaisEstadoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.paisId = request.paisId;

				PaisEstadoDataCombo respSQL = sql.SearchCombosPaisEstado();
				response.paisesDataComboList = respSQL.paisesDataComboList;
				response.estadosDataComboList = respSQL.estadosDataComboList;
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
		#endregion

		#region Busqueda de los datos para los radio button de tipo de información legal
		/// <summary>
		/// Web Method para buscar los tipos de información legal para radio button
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>

		[Route("searchTipoInfoRadioB")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchTipoInfoRadioB([FromBody] SearchRadioTipoInfoRequest request)
		{
			log.trace("searchTipoInfoRadioB");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchRadioTipoInfoResponse response = new SearchRadioTipoInfoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.paisId = request.tipoInfoLegalId;

				InfoLegalTipoRadioB respSQL = sql.SearchRadioBInfoLegal();
				response.tipoInfoRadioList = respSQL.tipoInfoRadioList;
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
		#endregion

		#region Edición de datos pertenecientes a un solo movimiento (separados)
		/// <summary>
		/// Web Method para actualizar los datos generales de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataMovimientoGeneral")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataMovimientoGeneral([FromBody] EditDataMovimientoGeneralRequest request)
		{
			log.trace("editDataMovimientoGeneral");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataMovimientoGeneralResponse response = new EditDataMovimientoGeneralResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.nombreAgrupacion = request.nombreAgrupacion;
				sql.referenciaInterna = request.referenciaInterna;
				sql.referenciaExterna = request.referenciaExterna;
				sql.numeroPallet = request.numeroPallet;
				sql.numeroCajas = request.numeroCajas;
				sql.producto = request.producto;
				sql.cantidad = request.cantidad;
				sql.tipoMovimiento = request.tipoMovimiento;
				sql.fechaIngreso = request.fechaIngreso;
				sql.fechaCaducidad = request.fechaCaducidad;


				sql.UpdateMovimientoGeneral();
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
		/// Web Method para actualizar los datos de la observación de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataMovimientoObservacion")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataMovimientoObservacion([FromBody] EditDataMovimientoObservacionRequest request)
		{
			log.trace("editDataMovimientoObservacion");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataMovimientoObservacionResponse response = new EditDataMovimientoObservacionResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.observacion = request.observacion;

				sql.UpdateMovimientoObservacion();
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
		/// Web Method para actualizar los datos de la observación de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataMovimientoNombre")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataMovimientoNombre([FromBody] EditDataMovimientoNombreRequest request)
		{
			log.trace("editDataMovimientoNombre");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataMovimientoNombreResponse response = new EditDataMovimientoNombreResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoCompleto = request.codigoCompleto;
				sql.nombreAgrupacion = request.nombre;

				sql.UpdateMovimientoNombre();
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
		/// Web Method para actualizar los datos del remitente de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataMovimientoRemitente")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataMovimientoRemitente([FromBody] EditDataMovimientoRemitenteRequest request)
		{
			log.trace("editDataMovimientoRemitente");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataMovimientoRemitenteResponse response = new EditDataMovimientoRemitenteResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.nombreRemitente = request.nombreRemitente;
				sql.apellidoRemitente = request.apellidoRemitente;
				sql.nombreCompaniaR = request.nombreCompaniaR;
				sql.rzCompaniaR = request.rzCompaniaR;
				sql.telefonoR = request.telefonoR;
				sql.paisR = request.paisR;
				sql.estadoR = request.estadoR;
				sql.ciudadR = request.ciudadR;
				sql.cpR = request.cpR;
				sql.domicilioR = request.domicilioR;
				sql.rancho = request.ranchoR;
				sql.sector = request.sectorR;

				sql.UpdateMovimientoRemitente();
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
		/// Web Method para actualizar los datos del destinatario  de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataMovimientoDestinatario")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataMovimientoDestinatario([FromBody] EditDataMovimientoDestinatarioRequest request)
		{
			log.trace("editDataMovimientoDestinatario");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataMovimientoDestinatarioResponse response = new EditDataMovimientoDestinatarioResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.nombreDestinatario = request.nombreDestinatario;
				sql.apellidoDestinatario = request.apellidoDestinatario;
				sql.nombreCompaniaD = request.nombreCompaniaD;
				sql.rzCompaniaD = request.rzCompaniaD;
				sql.telefonoD = request.telefonoD;
				sql.paisD = request.paisD;
				sql.estadoD = request.estadoD;
				sql.ciudadD = request.ciudadD;
				sql.cpD = request.cpD;
				sql.domicilioD = request.domicilioD;
				sql.numeroC = request.numeroC;
				sql.rancho = request.ranchoD;
				sql.sector = request.sectorD;

				sql.UpdateMovimientoDestinatario();
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
		/// Web Method para actualizar los datos del transportista de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataMovimientoTransportista")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataMovimientoTransportista([FromBody] EditDataMovimientoTransportistaRequest request)
		{
			log.trace("editDataMovimientoTransportista");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataMovimientoTransportistaResponse response = new EditDataMovimientoTransportistaResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.transportista = request.transportista;
				sql.numReferencia = request.numReferencia;
				sql.fechaEmbarque = request.fechaEmbarque;

				sql.UpdateMovimientoTransportista();
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
		/// Web Method para actualizar los datos de info legal de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataMovimientoInfoLegal")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataMovimientoInfoLegal([FromBody] EditDataMovimientoInfoLegalRequest request)
		{
			log.trace("editDataMovimientoInfoLegal");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataMovimientoInfoLegalResponse response = new EditDataMovimientoInfoLegalResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.tipoInfo = request.tipoInfo;
				sql.nombreInfo = request.nombreInfo;
				sql.direccionInfo = request.direccionInfo;
				sql.contactoInfo = request.contactoInfo;
				sql.nombreInfoExp = request.nombreInfoExp;
				sql.direccionInfoExp = request.direccionInfoExp;
				sql.contactoInfoExp = request.contactoInfoExp;

				sql.UpdateMovimientoInfoLegal();
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
		/// Web Method para actualizar los datos de info legal de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataMovimientoMerma")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataMovimientoMerma([FromBody] EditDataMovimientoMermaRequest request)
		{
			log.trace("editDataMovimientoMerma");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataMovimientoMermaResponse response = new EditDataMovimientoMermaResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoId = request.codigoId;
				sql. merma = request.merma;
				sql.productoId = request.productoMovimientoId;

				sql.UpdateMovimientoMerma();
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
		#endregion

		#region Busqueda para tabla de documento de info legal de un movimiento
		/// <summary>
		/// Web Method para buscar documentos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocsInfoLegal")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDocsInfoLegal([FromBody] SearchDocsInfoLegalRequest request)
		{
			log.trace("searchDocsInfoLegal");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocsInfoLegaloResponse response = new SearchDocsInfoLegaloResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.docsInfoLegalDataList = sql.SearchDocsInfoLegal();
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
		#endregion

		#region Busqueda para tabla de existencia estimada
		/// <summary>
		/// Web Method para buscar datos de existencia estimada
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchExistenciaEstimada")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchExistenciaEstimada([FromBody] SearchExistenciaEstimadaRequest request)
		{
			log.trace("searchExistenciaEstimada");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExistenciaEstimadaResponse response = new SearchExistenciaEstimadaResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.nombreFamiliaCIU = request.nombreFamiliaCIU;
				sql.fechaCaducidad = request.fechaCaducidad;
				sql.fechaIngresoDe = request.fechaIngresoDe;
				sql.fechaIngresoHasta = request.fechaIngresoHasta;
				sql.ordenar = request.ordenar;
				sql.usuario = request.usuario;

				response.existenciaEstimadaDataList = sql.SearchExistenciaEstimada();
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
		#endregion

		#region Insersion de documentos para info legal
		/// <summary>
		/// Web Method para guardar un documento seleccionado
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveDocsInfoLegal")]
		[HttpPost]
		[Authorize]
		public IActionResult saveDocsInfoLegal()
		{
			log.trace("saveDocsInfoLegal");
			SaveDocsInfoLegalResponse response = new SaveDocsInfoLegalResponse();
			try
			{
				var httpRequest = HttpContext.Request;
				
				MovimientosSQL sql = new MovimientosSQL();
				sql.doc = httpRequest.Form["doc"].ToString().Trim();
				sql.fecha = httpRequest.Form["fecha"];
				sql.infoLegalId = Convert.ToInt32(httpRequest.Form["infoLegalId"]);
				sql.nombreDoc = httpRequest.Form["nombreDoc"].ToString().Trim();
				sql.tipoArchivo = Convert.ToInt32(httpRequest.Form["tipoArchivo"]);


				// Guardar archivo
				if (httpRequest.Form.Files.Count > 0) {
					if(((httpRequest.Form.Files[0].Length / 1024) / 1024) > 10) {
						throw new Exception("El archivo debe ser menor a 10 MB");
					}
					string fileName = httpRequest.Form.Files[0].FileName;
					string filepath = ConfigurationSite._cofiguration["Paths:pathFilesInfoLegal"] + httpRequest.Form["movimientoId"] + "/";
					string fullPath = Path.Combine(filepath, fileName);

					if (!Directory.Exists(filepath))
						Directory.CreateDirectory(filepath);

					using (var stream = new FileStream(fullPath, FileMode.Create)) {
						httpRequest.Form.Files[0].CopyTo(stream);
					}
				}

				sql.SaveDocsInfo();
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
		/// Web Method para descargar documento seleccionado, Sin authorize para descargar los datos en trackit sin loguear
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("DownloadDocsInfoLegal")] 
		[HttpPost]
		public IActionResult DownloadDocsInfoLegal([FromBody] SaveDocsInfoLegalRequest request)
		{
			log.trace("DownloadDocsInfoLegal");
			SaveDocsInfoLegalResponse response = new SaveDocsInfoLegalResponse();
			try
			{
				// Buscar el archivo si existe en el path
				if(System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:urlFilesInfoLegal"] + request.movimientoId.ToString() + "/" + request.nombreDoc)) {
					var net = new System.Net.WebClient();
					var data = net.DownloadData(ConfigurationSite._cofiguration["Paths:urlFilesInfoLegal"] + request.movimientoId.ToString() + "/" + request.nombreDoc);
					var content = new System.IO.MemoryStream(data);
					var contentType = "application/octet-stream";
					var fileName = request.nombreDoc;
					return File(content, contentType, fileName);
                } else {
					throw new Exception("El archivo no existe");
				}
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			return Ok(response);
		}
		/// <summary>
		/// Web Method para eliminar un documento seleccionado
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("DeleteDocsInfoLegal")]
		[HttpPost]
		[Authorize]
		public IActionResult DeleteDocsInfoLegal([FromBody] SaveDocsInfoLegalRequest request)
		{
			log.trace("DeleteDocsInfoLegal");
			SaveDocsInfoLegalResponse response = new SaveDocsInfoLegalResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.docId = request.docId;
				sql.infoLegalId = request.infoLegalId;
				sql.nombreDoc = request.nombreDoc.ToString().Trim();

				// Si no existe el archivo mandar error
				if(System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:urlFilesInfoLegal"] + request.movimientoId.ToString() + "/" + request.nombreDoc)) {
					System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:urlFilesInfoLegal"] + request.movimientoId.ToString() + "/" + request.nombreDoc);
                }

				// Eliminar el archivo
				sql.DeleteDocsInfo();
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
		#endregion

		#region Insersion de movimiento
		/// <summary>
		/// Web Method para guardar un movimiento
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveMovimiento")]
		[HttpPost]
		[Authorize]
		public IActionResult saveMovimiento([FromBody] SaveMovimientoRequest request)
		{
			log.trace("saveMovimiento");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveMovimientosResponse response = new SaveMovimientosResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.nombreAgrupacion = request.nombreAgrupacion;
				sql.fechaIngreso = request.fechaIngreso;
				sql.referenciaInterna = request.referenciaInt;
				sql.referenciaExterna = request.referenciaExt;
				sql.usuario = request.usuario;
				sql.latitud = request.latitud;
				sql.longitud = request.longitud;

				sql.SaveMovimiento();
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
		#endregion

		#region Edición de tipo de movimiento de agrupación a envío
		/// <summary>
		/// Web Method para actualizar tipo de movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataAgruAEnvio")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataAgruAEnvio([FromBody] EditDataAgruAEnvioRequest request)
		{
			log.trace("editDataAgruAEnvio");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataAgruAEnvioResponse response = new EditDataAgruAEnvioResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.codigoCompleto = request.codigoCompleto;
				sql.codigoI = request.codigoI;
				sql.codigoF = request.codigoF;
				sql.fechaIngreso = request.fecha;
				sql.numeroC = request.numeroC;
				sql.totalProductosQR = request.totalProductosQR;
				sql.isGroup = request.isGroup;
				sql.productosAgrupacion = request.productosAgrupacion;
				sql.domicilioUp = request.domicilioUp;
				sql.domicilioUpRem = request.domicilioUpRem;
				sql.infoUpTras = request.infoUpTras;
				sql.infoUpObs = request.infoUpObs;
				sql.isAgro = request.isAgro;
				sql.referenciaInterna = request.referenciaInterna;
				sql.referenciaExterna = request.referenciaExterna;
				sql.embalajeId = request.embalajeId;
				sql.acopioId = request.acopioId;
				sql.productorId = request.productorId;

				// Validar el domicilio del Remitente cuando se envía por primera vez
				/*
				 * Quitamos estas cosas
				if (request.domicilioUpRem) {
					sql.nombreRemitente = request.domicilioRem.nombreRemitente;
					sql.apellidoRemitente = request.domicilioRem.apellidoRemitente;
					sql.nombreCompaniaR = request.domicilioRem.nombreCompaniaR;
					sql.rzCompaniaR = request.domicilioRem.rzCompaniaR;
					sql.telefonoR = request.domicilioRem.telefonoR;
					sql.paisR = request.domicilioRem.paisR == 0 ? 241 : request.domicilioRem.paisR;
					sql.estadoR = request.domicilioRem.estadoR == 0 ? 36 : request.domicilioRem.estadoR;
					sql.ciudadR = request.domicilioRem.ciudadR;
					sql.cpR = request.domicilioRem.cpR;
					sql.domicilioR = request.domicilioRem.domicilioR;
					sql.ranchoR = request.domicilioRem.ranchoR;
					sql.sectorR = request.domicilioRem.sectorR;
				}

				// Validar el domicilio del Destinatario cuando se envía por primera vez
				if (request.domicilioUp) {
					sql.paisD = request.domicilio.paisD == 0 ? 241 : request.domicilio.paisD;
					sql.estadoD = request.domicilio.estadoD == 0 ? 36 : request.domicilio.estadoD;
					sql.ciudadD = request.domicilio.ciudadD;
					sql.cpD = request.domicilio.cpD;
					sql.domicilioD = request.domicilio.domicilioD;
					sql.nombreDestinatario = request.domicilio.nombreDestinatario;
					sql.apellidoDestinatario = request.domicilio.apellidoDestinatario;
					sql.nombreCompaniaD = request.domicilio.nombreCompaniaD;
					sql.rzCompaniaD = request.domicilio.rzCompaniaD;
					sql.telefonoD = request.domicilio.telefonoD;
					sql.numeroC = request.domicilio.numeroC;
					sql.rancho = request.domicilio.ranchoD;
					sql.sector = request.domicilio.sectorD;
				}
				*/

				// Validar la info del transportista cuando se envía por primera vez
				if (request.infoUpTras) {
					sql.transportista = request.infoTransportista.transportista.Trim();
					sql.numReferencia = request.infoTransportista.numReferencia.Trim();
					sql.fechaEmbarque = request.infoTransportista.fechaEmbarque;
				}

				// Validar la info de observacion cuando se envía por primera vez
				if (request.infoUpObs) {
					sql.observacion = request.infoObservaciones.observacion.Trim();
				}

				sql.latitud = User.Claims.FirstOrDefault(u => u.Type == "Latitude").Value != null ? User.Claims.FirstOrDefault(u => u.Type == "Latitude").Value : "";
				sql.longitud = User.Claims.FirstOrDefault(u => u.Type == "Longitude").Value != null ? User.Claims.FirstOrDefault(u => u.Type == "Longitude").Value : "";
				sql.usuario = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);

				response.movimientoId = sql.UpdateAgruAEnvio();

				// Heredar los movimientos de un envío a otro
				if(sql.movimientoId != 0 && sql.isAgro) {
					sql.movimientoId = sql.getEnvioId(sql.movimientoId.ToString());
					string fullPath = ConfigurationSite._cofiguration["Paths:pathFilesInfoLegal"] + sql.movimientoId.ToString();
					string filepath = ConfigurationSite._cofiguration["Paths:pathFilesInfoLegal"] + response.movimientoId.ToString();
					string newFileName = "";

					if (Directory.Exists(fullPath)) {
						if (!Directory.Exists(filepath))
							Directory.CreateDirectory(filepath);

						// Mover los archivos del envío anterior al nuevo envío
						foreach (var file in new DirectoryInfo(fullPath).GetFiles()) {
							newFileName = Path.Combine(filepath + "/", file.Name);
							file.MoveTo(newFileName);
						}
					}
                }
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.movimientoId = 0;
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}
		#endregion

		#region Edición de tipo de movimiento de envío a recepcion
		/// <summary>
		/// Web Method para actualizar ltipo de movimientoSearchListDestinatarios
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editDataEnvioARecep")]
		[HttpPost]
		[Authorize]
		public IActionResult editDataEnvioARecep([FromBody] EditDataAgruAEnvioRequest request)
		{
			log.trace("editDataEnvioARecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditDataAgruAEnvioResponse response = new EditDataAgruAEnvioResponse();
			try
			{
			MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;
				sql.usuario = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
				sql.latitud = User.Claims.FirstOrDefault(u => u.Type == "Latitude").Value != null ? User.Claims.FirstOrDefault(u => u.Type == "Latitude").Value : "";
				sql.longitud = User.Claims.FirstOrDefault(u => u.Type == "Longitude").Value != null ? User.Claims.FirstOrDefault(u => u.Type == "Longitude").Value : "";

				if (request.movimientoId == 0 || request.force || request.isPallet)
				{
					#region Datos del Envío para una recepción sin envío
					sql.codigoCompleto = request.codigoCompleto;
					sql.codigoI = request.codigoI;
					sql.codigoF = request.codigoF;
					sql.fechaIngreso = request.isPallet ? "" : request.fecha;
					sql.numeroC = "";
					sql.totalProductosQR = request.totalProductosQR;
					sql.nCajasRecibidas = request.nCajasRecibidas;
					sql.isGroup = request.isPallet ? 1 : 0;
					sql.productosAgrupacion = new List<productosAgrupacion>();
					sql.domicilioUp = request.domicilioUp;
					sql.domicilioUpRem = request.domicilioUpRem;
					sql.infoUpTras = request.infoUpTras;
					sql.infoUpObs = request.infoUpObs;
					sql.isAgro = request.isAgro;
					sql.referenciaInterna = 0;
					sql.referenciaExterna = 0;
					sql.isReciboEnvio = true;
					sql.isPallet = request.isPallet;
					sql.acopioId = request.acopioId;
					sql.productorId = request.productorId;

					// Validar el domicilio del Remitente cuando se envía por primera vez
					/*if (request.domicilioUpRem)
					{
						sql.nombreRemitente = request.domicilioRem.nombreRemitente;
						sql.apellidoRemitente = request.domicilioRem.apellidoRemitente;
						sql.nombreCompaniaR = request.domicilioRem.nombreCompaniaR;
						sql.rzCompaniaR = request.domicilioRem.rzCompaniaR;
						sql.telefonoR = request.domicilioRem.telefonoR;
						sql.paisR = request.domicilioRem.paisR == 0 ? 241 : request.domicilioRem.paisR;
						sql.estadoR = request.domicilioRem.estadoR == 0 ? 36 : request.domicilioRem.estadoR;
						sql.ciudadR = request.domicilioRem.ciudadR;
						sql.cpR = request.domicilioRem.cpR;
						sql.domicilioR = request.domicilioRem.domicilioR;
						sql.ranchoR = request.domicilioRem.ranchoR;
						sql.sectorR = request.domicilioRem.sectorR;
					}
					*/
					// Validar el domicilio del Destinatario cuando se envía por primera vez
					/*if (request.domicilioUp)
					{
						sql.paisD = request.domicilio.paisD == 0 ? 241 : request.domicilio.paisD;
						sql.estadoD = request.domicilio.estadoD == 0 ? 36 : request.domicilio.estadoD;
						sql.ciudadD = request.domicilio.ciudadD;
						sql.cpD = request.domicilio.cpD;
						sql.domicilioD = request.domicilio.domicilioD;
						sql.nombreDestinatario = request.domicilio.nombreDestinatario;
						sql.apellidoDestinatario = request.domicilio.apellidoDestinatario;
						sql.nombreCompaniaD = request.domicilio.nombreCompaniaD;
						sql.rzCompaniaD = request.domicilio.rzCompaniaD;
						sql.telefonoD = request.domicilio.telefonoD;
						sql.numeroC = request.domicilio.numeroC;
						sql.rancho = request.domicilio.ranchoD;
						sql.sector = request.domicilio.sectorD;
					}
					*/
					// Validar la info del transportista cuando se envía por primera vez
					if (request.infoUpTras)
					{
						sql.transportista = request.infoTransportista.transportista.Trim();
						sql.numReferencia = request.infoTransportista.numReferencia.Trim();
						sql.fechaEmbarque = request.infoTransportista.fechaEmbarque;
					}

					// Validar la info de observacion cuando se envía por primera vez
					if (request.infoUpObs)
					{
						sql.observacion = request.infoObservaciones.observacion.Trim();
					}

					sql.movimientoId = sql.UpdateAgruAEnvio();

					// Heredar los movimientos de un envío a otro
					if (sql.movimientoId != 0 && sql.isAgro && request.force)
					{
						string fullPath = ConfigurationSite._cofiguration["Paths:pathFilesInfoLegal"] + request.movimientoId.ToString();
						string filepath = ConfigurationSite._cofiguration["Paths:pathFilesInfoLegal"] + sql.movimientoId.ToString();
						string newFileName = "";

						if (Directory.Exists(fullPath))
						{
							if (!Directory.Exists(filepath))
								Directory.CreateDirectory(filepath);

							// Mover los archivos del envío anterior al nuevo envío
							foreach (var file in new DirectoryInfo(fullPath).GetFiles())
							{
								newFileName = Path.Combine(filepath + "/", file.Name);
								file.MoveTo(newFileName);
							}
						}
					}
					#endregion
				}

				#region Recibir
				//sql.movimientoId = request.movimientoId;
				//sql.usuario = request.usuario;
				sql.fechaIngreso = request.fecha;
				sql.noEsnormal = request.noEsnormal;
				sql.codigoI = request.codigoI;
				sql.codigoF = request.codigoF;
				sql.codigoTipo = request.codigoTipo;
				sql.codigoCompleto = request.codigoCompleto;
				sql.nombreAgrupacion = request.nombre;
				sql.caja = request.caja;
				sql.isHexa = request.isHexa;
				sql.codigoFHEXA = request.codigoFHEXA;
				sql.codigoIHEXA = request.codigoIHEXA;
				sql.tipoRecepcion = request.tipoRecepcion;
				sql.cosecheroGral = request.cosechero;
				sql.sectorGral = request.sector;
				sql.fechaProduccion = request.fechaProduccion;
				sql.productosRecibidos = request.productosRecibidos;
				sql.nCajasRecibidas = request.nCajasRecibidas;
				sql.nPalletsRecibidos = request.nPalletsRecibidos;
				sql.nProductosMerma = request.nProductosMerma;
				sql.productoMovimientoId = request.productoMovimientoId;
				sql.isAgro = request.isAgro;
				sql.isUpName = request.isUpName;
				sql.acopioId = request.acopioId;
				sql.productorId = request.productorId;

				sql.UpdateEnvioARecep();
				#endregion
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = ex.Message == "El nombre ingresado ya se encuentra en otro movimiento" ? ex.Message : "An error occurred: " + ex.Message;
				response.messageEsp = ex.Message == "El nombre ingresado ya se encuentra en otro movimiento" ? ex.Message : "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}
		#endregion

		#region Busqueda y Actualiza el Consecutivo de la etiqueta al imprimir por Pallet o Caja
		/// <summary>
		/// Web Method para actualizar el Consecutivo de Etiqueta de Caja por Familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param>request</param>
		/// <returns>Int</returns>
		[Route("SaveConsecutiveBoxProduct")]
		[HttpPost]
		[Authorize]
		public IActionResult SaveConsecutiveBoxProduct([FromBody] SearchConsecutiveByfamilyRequest request)
		{
			log.trace("SaveConsecutiveBoxProduct");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchConsecutiveByfamilyResponse response = new SearchConsecutiveByfamilyResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.id;
				sql.productoId = request.productId;

				// Obtener el consecutivo actual
				response.consecutiveStart = sql.SearchConsecutiveByFamily();

				//Incrementar consecutivo
				response.consecutiveEnd = response.consecutiveStart + request.consecutiveBox;

				// Actualizar el consecutivo
				sql.consecutivoCaja = response.consecutiveEnd;
				sql.SaveConsecutiveByFamily();
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
		/// Web Method para actualizar el Consecutivo de Etiqueta por Familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param>request</param>
		/// <returns>List</returns>
		[Route("SaveConsecutiveGenericProduct")]
		[HttpPost]
		[Authorize]
		public IActionResult SaveConsecutiveGenericProduct([FromBody] SearchConsecutiveByfamilyRequest request)
		{
			log.trace("SaveConsecutiveGenericProduct");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchConsecutiveByfamilyResponse response = new SearchConsecutiveByfamilyResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.id;
				sql.productoId = request.productId;

				// Actualizar y Obtener el consecutivo
				response.consecutiveEnd = sql.SearchConsecutiveByFamily(1);
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
		/// Web Method para obtener las cajas con su QR de un movimiento agrupado por familia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param>request</param>
		/// <returns>List</returns>
		[Route("ListBoxesPerFamily")]
		[HttpPost]
		[Authorize]
		public IActionResult ListBoxesPerFamily([FromBody] SearchConsecutiveByfamilyRequest request)
		{
			log.trace("SaveConsecutiveBoxProduct");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchBoxesInfoByMovResponse response = new SearchBoxesInfoByMovResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.id;

				response = sql.SearchBoxesInfoByMovimiento();
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
		#endregion

		#region Insersion de USUARIOS AL SISTEMA
		/// <summary>
		/// Web Method para guardar un usuario
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveUsuario")]
		[HttpPost]
		public IActionResult saveUsuario([FromBody] SaveUsuarioRequest request)
		{
			log.trace("saveUsuario");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveUsuarioResponse response = new SaveUsuarioResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.nombreComp = request.nombreComp;
				sql.razonSocial = request.razonSocial;
				sql.correoComp = request.correoComp;
				sql.telefono = request.telefono;
				sql.direccion = request.direccion;
				sql.nombrePais = request.nombrePais;

				sql.codigoPostal = request.codigoPostal;
				sql.ciudad = request.ciudad;
				sql.nombreEstado = request.nombreEstado;

				sql.email1 = request.email1;
				sql.pass1 = request.pass1;

				sql.email2 = request.email2;
				sql.pass2 = request.pass2;

				sql.email3 = request.email3;
				sql.pass3 = request.pass3;

				sql.email4 = request.email4;
				sql.pass4 = request.pass4;

				sql.email5 = request.email5;
				sql.pass5 = request.pass5;

				sql.usuario = request.usuario;

				sql.SaveUsuario();
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
		/// Web Method para guardar un usuario invitado
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveUsuarioInvitado")]
		[HttpPost]
		public IActionResult saveUsuarioInvitado([FromBody] SaveUsuarioInvitadoRequest request)
		{
			log.trace("saveUsuarioInvitado");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveUsuarioInvitadoResponse response = new SaveUsuarioInvitadoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.email1 = request.email1;
				sql.pass1 = request.pass1;

				sql.SaveUsuarioInvitado();
				response.usuarioInvitadoList.email = "" + sql.email1;

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
		/// Web Method crear evento para eliminar datos despues de 10 hrs
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteEventoInvitado")]
		[HttpPost]
		public IActionResult deleteEventoInvitado([FromBody] DeleteUsuarioInvitadoRequest request)
		{
			log.trace("deleteEventoInvitado");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteUsuarioInvitadoResponse response = new DeleteUsuarioInvitadoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.email1 = request.email1;
				sql.usuario = request.usuario;

				sql.createEventDeleteInvitado();
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
		#endregion

		#region Búsqueda de Info para Doc Detalle de un movimiento (separados)
		/// <summary>
		/// Web Method para buscar los datos general de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleProductos")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDocDetalleProductos([FromBody] SearchDocDetalleProductosRequest request)
		{
			log.trace("searchDocDetalleProductos");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleProductosResponse response = new SearchDocDetalleProductosResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.docDetalleProductoList = sql.SearchDocDetalleProducto();
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
		/// Web Method para buscar los datos general de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleProductosIndi")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDocDetalleProductosIndi([FromBody] SearchDocDetalleProductosIndiRequest request)
		{
			log.trace("searchDocDetalleProductosIndi");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleProductosIndiResponse response = new SearchDocDetalleProductosIndiResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId2 = request.movimientoId;

				response.docDetalleProductoIndiList = sql.SearchDocDetalleProductoIndi();
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
		/// Web Method para buscar los datos general de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleProductosCajas")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDocDetalleProductosCajas([FromBody] SearchDocDetalleProductosIndiRequest request)
		{
			log.trace("searchDocDetalleProductosCajas");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleProductosCajasResponse response = new SearchDocDetalleProductosCajasResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId2 = request.movimientoId;
				sql.codigoTipo = request.codigoTipo;
				sql.codigoI = request.codigoI;
				sql.codigoF = request.codigoF;

				response.docDetalleProductoCajasList = sql.SearchDocDetalleProductoCajas();
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
		/// Web Method para buscar los datos general de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleProductosRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDocDetalleProductosRecep([FromBody] SearchDocDetalleProductosRecepRequest request)
		{
			log.trace("searchDocDetalleProductosRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleProductosResponse response = new SearchDocDetalleProductosResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.docDetalleProductoList = sql.SearchDocDetalleProductoRecep();
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
		/// Web Method para buscar los datos general de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleProductosAgru")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDocDetalleProductosAgru([FromBody] SearchDocDetalleProductosAgruRequest request)
		{
			log.trace("searchDocDetalleProductosAgru");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleProductosResponse response = new SearchDocDetalleProductosResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.idFicha = request.idFicha;

				response.docDetalleProductoList = sql.SearchDocDetalleProductoAgru();
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
		/// Web Method para buscar el total de los productos de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalProd")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDocDetalleTotalProd([FromBody] SearchDocDetalleTotalProdRequest request)
		{
			log.trace("searchDocDetalleTotalProd");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalProdResponse response = new SearchDocDetalleTotalProdResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.docDetalleTotalProdList = sql.SearchDocDetalleTotalProd();
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
		/// Web Method para buscar el total de los productos de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalProdRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDocDetalleTotalProdRecep([FromBody] SearchDocDetalleTotalProdRecepRequest request)
		{
			log.trace("searchDocDetalleTotalProdRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalProdResponse response = new SearchDocDetalleTotalProdResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.docDetalleTotalProdList = sql.SearchDocDetalleTotalProdRecep();
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
		/// Web Method para buscar el total de los pallets de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalPallet")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDocDetalleTotalPallet([FromBody] SearchDocDetalleTotalPalletRequest request)
		{
			log.trace("searchDocDetalleTotalPallet");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalPalletResponse response = new SearchDocDetalleTotalPalletResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.docDetalleTotalPalletList = sql.SearchDocDetalleTotalPallet();
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
		/// Web Method para buscar el total de los pallets de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalPalletRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDocDetalleTotalPalletRecep([FromBody] SearchDocDetalleTotalPalletRecepRequest request)
		{
			log.trace("searchDocDetalleTotalPalletRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalPalletResponse response = new SearchDocDetalleTotalPalletResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.docDetalleTotalPalletList = sql.SearchDocDetalleTotalPalletRecep();
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
		/// Web Method para buscar el total de los cajas de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalCajas")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDocDetalleTotalCajas([FromBody] SearchDocDetalleTotalCajasRequest request)
		{
			log.trace("searchDocDetalleTotalCajas");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalCajasResponse response = new SearchDocDetalleTotalCajasResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.docDetalleTotalCajasList = sql.SearchDocDetalleTotalCajas();
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
		/// Web Method para buscar el total de los cajas de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalCajasRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDocDetalleTotalCajasRecep([FromBody] SearchDocDetalleTotalCajasRecepRequest request)
		{
			log.trace("searchDocDetalleTotalCajasRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalCajasResponse response = new SearchDocDetalleTotalCajasResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.docDetalleTotalCajasList = sql.SearchDocDetalleTotalCajasRecep();
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
		/// Web Method para buscar el total de las unidades de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalCantidad")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDocDetalleTotalCantidad([FromBody] SearchDocDetalleTotalCantidadRequest request)
		{
			log.trace("searchDocDetalleTotalCantidad");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalCantidadResponse response = new SearchDocDetalleTotalCantidadResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.docDetalleTotalCantidadList = sql.SearchDocDetalleTotalCantidad();
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
		/// Web Method para buscar el total de las unidades de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalCantidadRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDocDetalleTotalCantidadRecep([FromBody] SearchDocDetalleTotalCantidadRecepRequest request)
		{
			log.trace("searchDocDetalleTotalCantidadRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalCantidadResponse response = new SearchDocDetalleTotalCantidadResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.docDetalleTotalCantidadList = sql.SearchDocDetalleTotalCantidadRecep();
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
		/// Web Method para buscar el total del peso de los productos de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalPeso")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDocDetalleTotalPeso([FromBody] SearchDocDetalleTotalPesoRequest request)
		{
			log.trace("searchDocDetalleTotalPeso");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalPesoResponse response = new SearchDocDetalleTotalPesoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.docDetalleTotalPesoList = sql.SearchDocDetalleTotalPeso();
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
		/// Web Method para buscar el total del peso de los productos de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleTotalPesoRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDocDetalleTotalPesoRecep([FromBody] SearchDocDetalleTotalPesoRecepRequest request)
		{
			log.trace("searchDocDetalleTotalPesoRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleTotalPesoResponse response = new SearchDocDetalleTotalPesoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.docDetalleTotalPesoList = sql.SearchDocDetalleTotalPesoRecep();
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
		/// Web Method para buscar la fecha de caducidad minima de los productos de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleFechaMin")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		//[Authorize]
		public IActionResult searchDocDetalleFechaMin([FromBody] SearchDocDetalleFechaMinRequest request)
		{
			log.trace("searchDocDetalleFechaMin");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleFechaMinResponse response = new SearchDocDetalleFechaMinResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.movimientoId = request.movimientoId;

				response.docDetalleFechaMinList = sql.SearchDocDetalleFechaMin();
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
		/// Web Method para buscar la fecha de caducidad minima de los productos de la tabla detalle productos
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchDocDetalleFechaMinRecep")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchDocDetalleFechaMinRecep([FromBody] SearchDocDetalleFechaMinRecepRequest request)
		{
			log.trace("searchDocDetalleFechaMinRecep");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDocDetalleFechaMinResponse response = new SearchDocDetalleFechaMinResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.codigoQR = request.codigoQR;

				response.docDetalleFechaMinList = sql.SearchDocDetalleFechaMinRecep();
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
		#endregion

		#region Busqueda de destinatarios de un usuario para autocomplete
		/// <summary>
		/// Web Method para buscar un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchListDestinatarios")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchListDestinatarios([FromBody] SearchListDestinatariosRequest request)
		{
			log.trace("searchListDestinatarios");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchListDestinatariosResponse response = new SearchListDestinatariosResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.usuario = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);

				response.destinatariosDataList = sql.SearchListDestinatarios();
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
		/// Web Method para buscar un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchListDestinatariosNum")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		[Authorize]
		public IActionResult searchListDestinatariosNum([FromBody] SearchListDestinatariosRequest2 request)
		{
			log.trace("searchListDestinatariosNum");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchListDestinatariosResponse2 response = new SearchListDestinatariosResponse2();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.usuario = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);

				response.destinatariosDataList2 = sql.SearchListDestinatariosNum();
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
		#endregion

		#region Insersion de estados no existentes en los combos
		/// <summary>
		/// Web Method para guardar un producto desconocido
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveEstadosSiNoExiste")]
		[HttpPost]
		//[Authorize]
		public IActionResult saveEstadosSiNoExiste([FromBody] SaveEstadosSiNoExisteRequest request)
		{
			log.trace("saveEstadosSiNoExiste");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveEstadosSiNoExisteResponse response = new SaveEstadosSiNoExisteResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.paisE = request.pais;
				sql.estadoE = request.nombreEstado;

				sql.SaveEstadoSiNoExiste();
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
		#endregion

		#region Busqueda de existencia de estado
		/// <summary>
		/// Web Method para buscar un movimiento etiqueta creada
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <param nombreAgrupacion="request"></param>
		/// <returns></returns>
		[Route("searchExistenciaEstado")]
		[HttpPost]
		/*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
		public IActionResult searchExistenciaEstado([FromBody] SearchExistenciaEstadoRequest request)
		{
			log.trace("searchExistenciaEstado");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExistenciaEstadoResponse response = new SearchExistenciaEstadoResponse();
			try
			{
				MovimientosSQL sql = new MovimientosSQL();
				sql.paisE = request.pais;
				sql.estadoE = request.nombreEstado;

				response.existenciaEstadoList = sql.SearchEstadoExistencia();
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
        #endregion
    }

}
