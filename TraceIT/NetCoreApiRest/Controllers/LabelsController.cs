using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Models.Response;
using NetCoreApiRest.Utils;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using WS.Interfaces;
using WSTraceIT.Interfaces;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Address;
using WSTraceIT.Models.Base.Families;
using WSTraceIT.Models.Base.User;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;

namespace NetCoreApiRest.Controllers
{
    [Route("Labels")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("LabelsController");
		/// <summary>
		/// Web Method para la consulta de las solicitudes de etiqueta
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("buscarSolicitudEtiquetas")]
        [HttpPost]
        [Authorize]
        public IActionResult BuscarSolicitudEtiquetas([FromBody]SolicitudesEtiquetas request)
        {
            ListaSolicitudEtiquetas responseSQL = new ListaSolicitudEtiquetas();
            try
            {
                LabelSQL sql = new LabelSQL();
                responseSQL = sql.BuscarSolicitudEtiquetas(request);
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
responseSQL.messageEng = "An error occurred: " + ex.Message;
                responseSQL.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return Ok(responseSQL);
        }

        /// <summary>
		/// Web Method para la consulta de las compañias
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("buscarCompanias")]
        [HttpPost]
        [Authorize]
        public IActionResult BuscarCompanias([FromBody]int request)
        {
            ListaCompania responseSQL = new ListaCompania();
            try
            {
                LabelSQL sql = new LabelSQL();
                responseSQL = sql.BuscarCompanias(request);
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
responseSQL.messageEng = "An error occurred: " + ex.Message;
                responseSQL.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return Ok(responseSQL);
        }

        /// <summary>
		/// Web Method para la consulta de los detalles de solicitudes de etiqueta
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("detalleSolicitud")]
        [HttpPost]
        [Authorize]
        public IActionResult DetalleSolicitud([FromBody]SolicitudesEtiquetas request)
        {
            ListaSolicitudEtiquetas responseSQL = new ListaSolicitudEtiquetas();
            try
            {
                LabelSQL sql = new LabelSQL();
                responseSQL = sql.DetalleSolicitud(request);
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
responseSQL.messageEng = "An error occurred: " + ex.Message;
                responseSQL.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return Ok(responseSQL);
        }

        /// <summary>
		/// Web Method para el guardado de los comentarios de bitacora
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("guardarBitacoraSolicitud")]
        [HttpPost]
        [Authorize]
        public IActionResult GuardarBitacoraSolicitud([FromBody]Bitacora request)
        {
            ListaSolicitudEtiquetas responseSQL = new ListaSolicitudEtiquetas();
            try
            {
                LabelSQL sql = new LabelSQL();
                sql.GuardarBitacoraSolicitud(request);
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
responseSQL.messageEng = "An error occurred: " + ex.Message;
                responseSQL.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return Ok(responseSQL);
        }

        /// <summary>
		/// Web Method para la consulta el historial de la bitacora
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("historialBitacora")]
        [HttpPost]
        [Authorize]
        public IActionResult HistorialBitacora([FromBody]Bitacora request)
        {
            ListaBitacoras responseSQL = new ListaBitacoras();
            try
            {
                LabelSQL sql = new LabelSQL();
                responseSQL = sql.HistorialBitacora(request);
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
responseSQL.messageEng = "An error occurred: " + ex.Message;
                responseSQL.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return Ok(responseSQL);
        }

        /// <summary>
		/// Web Method para la cactualizacion de status de solicitud
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("actualizarEstatusSolicitud")]
        [HttpPost]
        [Authorize]
        public IActionResult ActualizarEstatusSolicitud([FromBody]SolicitudesEtiquetas request)
        {
            ListaSolicitudEtiquetas responseSQL = new ListaSolicitudEtiquetas();
            try
            {
                LabelSQL sql = new LabelSQL();
                sql.ActualizarEstatusSolicitud(request);
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
responseSQL.messageEng = "An error occurred: " + ex.Message;
                responseSQL.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return Ok(responseSQL);
        }


        /// <summary>
		/// Web Method para guardar el registro de la importacion de productos
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		/// NOTICE: This goes first
		[Route("guardarSolicitud")]
        [HttpPost]
        [Authorize]
        public IActionResult saveImportLabelRequest([FromBody]SolicitudesEtiquetas request)
        {
            ListaSolicitudEtiquetas response = new ListaSolicitudEtiquetas();
            if (request.byFile)
            {
                //log.info("Save file to import");
                ArchivesData archives = new ArchivesData();
                try
                {
                    Random random = new Random();
                    archives.base64 = request.Base64File;
                    archives.archiveName = random.Next() + ".xlsx";
                    request.Base64File = archives.archiveName;
                    //log.debug("The file to save: " + archives.archiveName);

                    /*ProductSQL sql = new ProductSQL();
					sql.fileName = request.fileBase;
					sql.familyProductId = request.familyProductId;
					sql.directionId = request.directionId;
					log.trace("To save the file data to DB");
					sql.SaveImportProduct();*/

                    ProcesarImagen imagen = new ProcesarImagen();
                    //log.trace("Processing file, to be saved at " + ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"]);
                    imagen.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"]);
                    int importStatus = this.generateLabelRequest(archives.archiveName, request);
                    if (importStatus != 0)
                    {
                        response.messageEng = "An error occurred";
                        response.messageEsp = "Ocurrio un error";
                    }
                }
                catch (Exception ex)
                { log.error("Exception: " + ex.Message); 
//log.error("Exception at saving file: " + ex.Message);
                    response.messageEng = "An error occurred: " + ex.Message;
                    response.messageEsp = "Ocurrio un error: " + ex.Message;
                }
            }
            else
            {
                response = this.GuardarSolicitud(request);
            }
            log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
            return Ok(response);
        }

        private int generateLabelRequest(string fileName, SolicitudesEtiquetas par)
        {
            //log.info("Generating products from the file");
            LabelSQL sql = new LabelSQL();
            FileInfo excel = new FileInfo(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName);
            var package = new ExcelPackage(excel);
            ProcesarImagen procesar = new ProcesarImagen();

            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.First();
            //log.debug("Reading the file");
            DataTable datosExcel = procesar.ConvertToDataTable(worksheet);
            package.Dispose();
            workbook.Dispose();
            worksheet.Dispose();
            //log.trace("Processing the data");
            //Se mapean los datos leidos del archivo excel para mandarlos a insertar en la base de datos
            ListaSolicitudEtiquetas lista = new ListaSolicitudEtiquetas();
            foreach (DataRow row in datosExcel.Rows)
            {
                //log.trace("Reading a row");
                    lista.listaSolicitudEtiquetas.Add(new SolicitudesEtiquetas
                    {
                        direccionId = par.direccionId,
                        caducidad = Convert.ToDateTime(row["Caducidad"]),
                        familiaId = par.familiaId,
                        udid = Convert.ToString(row["UDID"]),
                        estatusSolicitudId = par.estatusSolicitudId,
                        cantidad = Convert.ToInt32(row["Cantidad"]),
                        usuarioId = par.usuarioId
                    });
            }
            //log.debug("file read, saving QR data");
			string[] respSolicitud = sql.GuardarSolicitudArchivo(lista);
			string folio = respSolicitud[0];
			string username = respSolicitud[1];
			ListaCompania companias = new ListaCompania();
            if (par.companiaId > 0)
                companias = sql.BuscarCompanias(par.companiaId);
            else
                companias.listaCompanias.Add(new Companias { nombre = "TraceIt" });
            //Se crea el archivo de texto con los CIUS generados para enviarlo por correo
            //System.IO.File.WriteAllLines(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"), QR.ToArray());

            //Se manda el correo electronico
            UserMobileSQL emailList = new UserMobileSQL();
            emailList.type = 5;
            List<string> correosListado = emailList.searchEmail();
            //log.trace("Sending email");
            EMAILClient correo = new EMAILClient();
            String mensaje = "La compañía " + companias.listaCompanias[0].nombre + " realizó la solicitud de las etiquetas adjuntas en el correo. \r\n\rLa solicitud fue registrada con el folio: " + folio + ".\r\n\r\n El usuario que realizó la solicitud es " + username + ".\r\n\rIngrese al BackOffice para el seguimiento."; ;
            correo.EnviarCorreoImportacion(correosListado.ToArray(), mensaje, ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName, "Solicitud de etiqueta Folio " + folio);

            //log.trace("Deleting files");
            //Se borran los archivos txt y Excel
            if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName))
                System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName);

            //if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt")))
            //    System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"));

            //response.messageEng = "Import done successfully";
            //response.messageEsp = "Importación realizada con éxito";
            //log.trace("Import finished");
            return 0;
        }



        /// <summary>
		/// Web Method para guardar una solicitud
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		//[Route("guardarSolicitud")]
        //[HttpPost]
        //[Authorize]
        public /*IActionResult*/ ListaSolicitudEtiquetas GuardarSolicitud([FromBody]SolicitudesEtiquetas request)
        {
            ListaSolicitudEtiquetas responseSQL = new ListaSolicitudEtiquetas();
            try
            {
                LabelSQL sql = new LabelSQL();
				string[] respSolicitud = sql.GuardarSolicitud(request);
				string folio = respSolicitud[0];
				string username = respSolicitud[1];

				ListaCompania companias = new ListaCompania();
                if (request.companiaId > 0)
                    companias = sql.BuscarCompanias(request.companiaId);
                else
                    companias.listaCompanias.Add(new Companias { nombre = "TraceIt" });
                FamiliesSQL sqlFamilia = new FamiliesSQL();
                sqlFamilia.familyId = request.familiaId;
                FamilyDataEdition resFamilia = sqlFamilia.SearchProductFamilyData();

                AddressSQL sqlDireccion = new AddressSQL();
                sqlDireccion.idAddress = request.direccionId;
                AddressDataEdition resDireccion = sqlDireccion.SearchAddresData();
                //Se crea el archivo de texto con los CIUS generados para enviarlo por correo
                //System.IO.File.WriteAllLines(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"), QR.ToArray());

                //Se manda el correo electronico
                UserMobileSQL emailList = new UserMobileSQL();
                emailList.type = 5;
                List<string> correosListado = emailList.searchEmail();
                //log.trace("Sending email");
                EMAILClient correo = new EMAILClient();
                String mensaje = "La compañía " + companias.listaCompanias[0].nombre + " realizó la solicitud de "+request.cantidad+ " etiqueta(s), con los siguientes datos: \r\n\rFamilia: "+resFamilia.productFamilyData.name+ ".\r\n\rOrigen: "+resDireccion.name+ ".\r\n\rUDID: "+request.udid+ ".\r\n\rFecha de Caducidad: "+ String.Format("{0:dd/MM/yyyy}", request.caducidad) + ".\r\n\rLa solicitud fue registrada con el folio: " + folio + ".\r\n\r\n El usuario que realizó la solicitud es " + username + ".\r\n\rIngrese al BackOffice para el seguimiento. "; ;
                correo.EnviarCorreo(correosListado.ToArray(), mensaje, "Solicitud de etiqueta Folio " + folio);

            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
responseSQL.messageEng = "An error occurred: " + ex.Message;
                responseSQL.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return responseSQL;
        }


    } 
}
