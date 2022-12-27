using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using OfficeOpenXml;
using WS.Interfaces;
using WSTraceIT.Interfaces;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Families;
using WSTraceIT.Models.Base.Product;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Controllers
{
    [Route("Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private LoggerD4 log = new LoggerD4("ProductController");
        private string udidFile = String.Empty;

        /// <summary>
        /// Web Method para guardar los productos
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("saveProduct")]
        [HttpPost]
        [Authorize]
        public IActionResult saveProduct([FromBody] SaveProductRequest request)
        {
            log.trace("saveProduct");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SaveProductResponse response = new SaveProductResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                sql.productList.Add(new SaveProductSQL
                {
                    directionId = request.directionId,
                    expirationDate = request.expirationDate,
                    familyProductId = request.familyProductId,
                    udid = request.udid
                });

                sql.SaveProduct();
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
        /// Web Method para editar los productos registrados
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("editProduct")]
        [HttpPost]
        [Authorize]
        public IActionResult editProduct([FromBody] EditProductRequest request)
        {
            log.trace("editProduct");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            EditProductResponse response = new EditProductResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                sql.productList.Add(new SaveProductSQL
                {
                    idProduct = request.idProduct,
                    udid = request.udid,
                    expirationDate = request.expirationDate,
                    directionId = request.directionId,
                    familyProductId = request.familyProductId,
                    status = request.status

                });

                sql.UpdateProduct();
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
        /// Web Method para eliminar el producto
        /// Desarrollador: David Martinez
        /// </summary>
        /// <returns></returns>
        [Route("deleteProduct")]
        [HttpPost]
        [Authorize]
        public IActionResult deleteProduct([FromBody] DeleteProductRequest request)
        {
            log.trace("deleteProduct");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            DeleteProductResponse response = new DeleteProductResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                sql.idProduct = request.idProduct;

                sql.DeleteProduct();
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
        /// Web Method para realizar la busqueda de productos
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchProduct")]
        [HttpPost]
        [Authorize]
        public IActionResult searchProduct([FromBody] SearchProductRequest request)
        {
            log.trace("searchProduct");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchProductResponse response = new SearchProductResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                sql.udid = request.udid;
                sql.familyProductId = request.idFamily;

                response.products = sql.SearchProduct();
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
        /// Web Method para la busqueda del detalle del producto
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchProductEdit")]
        [HttpPost]
        [Authorize]
        public IActionResult searchProductEdit([FromBody] SearchProductEditRequest request)
        {
            log.trace("searchProductEdit");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchProductEditResponse response = new SearchProductEditResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                sql.idProduct = request.idProduct;

                response.productDetails = sql.SearchProductDetails();
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
        /// Web Method para traerse los combos para el modulo de Productos
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchProductDropDown")]
        [HttpPost]
        [Authorize]
        public IActionResult searchProductDropDown([FromBody] SearchProductDropDownRequest request)
        {
            log.trace("searchProductDropDown");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchProductDropDownResponse response = new SearchProductDropDownResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                sql.company = request.company;

                var dropDrownData = sql.SearchDropDownProduct();
                response.familyDropDown = dropDrownData.familyDropDown;
                //response.originDropDown = dropDrownData.originDropDown;
                response.companyDropDown = dropDrownData.companyDropDown;
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
        /// Web Method para traerse los combos para el modulo de Productos Importacion
        /// Desarrollador: Ivan Gutierrez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchProductDropDownImport")]
        [HttpPost]
        [Authorize]
        public IActionResult searchProductDropDownImport([FromBody] SearchProductDropDownRequest request)
        {
            log.trace("searchProductDropDownImport");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchProductDropDownResponse response = new SearchProductDropDownResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                sql.company = request.company;

                var dropDrownData = sql.SearchDropDownProduct(1);
                response.familyDropDown = dropDrownData.familyDropDown;
                //response.originDropDown = dropDrownData.originDropDown;
                //response.companyDropDown = dropDrownData.companyDropDown;
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
        /// Web Method para obtener los origenes de la familia especificada
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchProductDropDownOrigen")]
        [HttpPost]
        [Authorize]
        public IActionResult searchProductDropDownOrigen([FromBody] SearchOriginDropDownRequest request)
        {
            log.trace("searchProductDropDownOrigen");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchProductDropDownResponse response = new SearchProductDropDownResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                int familia = request.filter;
                var dropDrownData = sql.SearchDropDownOrigen(familia);
                response.originDropDown = dropDrownData.originDropDown;
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
        /// Web Method para obtener los origenes de la familia especificada en Importar Productos
        /// Desarrollador: Ivan Gutierrez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchProductDropDownOrigenImport")]
        [HttpPost]
        [Authorize]
        public IActionResult searchProductDropDownOrigenImport([FromBody] SearchOriginDropDownRequest request)
        {
            log.trace("searchProductDropDownOrigenImport");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchProductDropDownResponse response = new SearchProductDropDownResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                int familia = request.filter;
                var dropDrownData = sql.SearchDropDownOrigen(familia, 1);
                response.originDropDown = dropDrownData.originDropDown;
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
        /// Web Method para obtener los datos de un producto por CIU
        /// Desarrollador: Omar Larrion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchDataFromCIUS")]
        [HttpPost]
        public IActionResult searchProductCIUS([FromBody] SearchDataProductCIURequest request)
        {
            log.trace("searchProductDropDownOrigen");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchDataProductResponse response = new SearchDataProductResponse();
            try
            {
                ProductSQL sql = new ProductSQL();
                string ciu = request.ciu;
                var data = sql.searchDataCIU(ciu);
                response.product = (from dat in data
                                    select new ProductDetailsCIU
                                    {
                                        Ciu = dat.Ciu,
                                        Nombre = dat.Nombre,
                                        Descripcion = (dat.Descripcion != null && dat.Descripcion.Length > 0) ? dat.Descripcion : "",
                                        ProductoImagen = dat.ProductoImagen == null || dat.ProductoImagen == "" ? "" : ConfigurationSite._cofiguration["Paths:urlImagesFamily"] + dat.ProductoImagen,
                                        Presentacion = dat.Presentacion,
                                        Marca = dat.Marca,
                                        NumSerie = dat.NumSerie,
                                        Lote = dat.Lote,
                                        Caducidad = dat.Caducidad,
                                        GTIN = dat.GTIN,
                                        Empresa = dat.Empresa,
                                        Pais = dat.Pais,
                                        Ciudad = dat.Ciudad,
                                        Estado = dat.Estado,
                                        Planta = dat.Planta,
                                        LineaProd = dat.LineaProd,
                                        FechaProduccion = dat.FechaProduccion,
                                        LineaProduccion = dat.LineaProduccion,
                                        CosecheroNombre = dat.CosecheroNombre,
                                        CosecheroPuesto = dat.CosecheroPuesto,
                                        CosecheroDescripcion = dat.CosecheroDescripcion,
                                        CosecheroImagen = dat.CosecheroImagen == null || dat.CosecheroImagen == "" ? "" : ConfigurationSite._cofiguration["Paths:urlImagesOperatorsEmpEtq"] + "1/" + dat.CosecheroImagen,
                                        Cosecha = dat.Cosecha,
                                        Sector = dat.Sector,
                                        Rancho = dat.Rancho,
                                        Operador = dat.Operador,
                                        listaAlerta = dat.listaAlerta
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
        /// Web Method para obtener los origenes de la familia especificada
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// NOTICE: This goes first
        [Route("saveImportProduct")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> importacionProductos([FromBody] SaveImportProductRequest request)
        {
            log.trace("saveImportProduct");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SaveImportProductResponse response = new SaveImportProductResponse();
            response.ciusFile = "";
            if (request.byFile)
            {
                log.info("Save file to import");
                ArchivesData archives = new ArchivesData();
                try
                {
                    Random random = new Random();
                    archives.base64 = request.fileBase;
                    archives.archiveName = random.Next() + ".xlsx";
                    request.fileBase = archives.archiveName;
                    log.debug("The file to save: " + archives.archiveName);

                    //ProductSQL sql = new ProductSQL();
                    //sql.fileName = request.fileBase;
                    //sql.familyProductId = request.familyProductId;
                    //sql.directionId = request.directionId;
                    //log.trace("To save the file data to DB");
                    //sql.SaveImportProduct();

                    ProcesarImagen imagen = new ProcesarImagen();
                    log.trace("Processing file, to be saved at " + ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"]);
                    imagen.SaveImage(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"]);
                    int importStatus = this.generateProducts(archives.archiveName, request.familyProductId, request.directionId, request.userId, request.company, request.family, request.packagingId);
                    response.ciusFile = this.udidFile;
                    response.urlFile = ConfigurationSite._cofiguration["Paths:urlFilesImportProduct"];
                    if (importStatus != 0)
                    {
                        response.ciusFile = this.udidFile;
                        response.urlFile = ConfigurationSite._cofiguration["Paths:urlFilesImportProduct"];
                        response.messageEng = "An error occurred";
                        response.messageEsp = "Ocurrio un error";
                        if(importStatus == 1) {
                            response.messageEng = "The amount is wrong";
                            response.messageEsp = "La cantidad es incorrecta";
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.ciusFile = this.udidFile;
                    response.urlFile = ConfigurationSite._cofiguration["Paths:urlFilesImportProduct"];
                    log.error("Exception at saving file: " + ex.Message);
                    response.messageEng = "An error occurred: " + ex.Message;
                    response.messageEsp = "Ocurrio un error: " + ex.Message;
                }
            }
            else
            {
                try
                {
                    int importStatus = this.generateProducts(request.amount, request.expiry, request.udid, request.familyProductId, request.directionId, request.userId, request.company, request.family, request.packagingId, request.columns);
                    response.ciusFile = this.udidFile;
                    response.urlFile = ConfigurationSite._cofiguration["Paths:urlFilesImportProduct"];
                    if (importStatus != 0)
                    {
                        response.urlFile = ConfigurationSite._cofiguration["Paths:urlFilesImportProduct"];
                        response.messageEng = "An error occurred";
                        response.messageEsp = "Ocurrio un error";
                        if(importStatus == 1) {
                            response.messageEng = "The amount is wrong";
                            response.messageEsp = "La cantidad es incorrecta";
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.error("Exception at saving file quantity: " + ex.Message);
                    response.ciusFile = this.udidFile;
                    response.urlFile = ConfigurationSite._cofiguration["Paths:urlFilesImportProduct"];
                    response.messageEng = "An error occurred";
                    response.messageEsp = "Ocurrio un error";
                }
            }
            log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
            return Ok(response);
        }

        private int generateProducts(string fileName, int familyId, int directionId, int userId, string company, string family, int packagingId)
        {
            log.info("Generating products from the file");
            ProductSQL sql = new ProductSQL();
            FileInfo excel = new FileInfo(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName);
            var package = new ExcelPackage(excel);
            ProcesarImagen procesar = new ProcesarImagen();

            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.First();
            log.debug("Reading the file");
            DataTable datosExcel = procesar.ConvertToDataTable(worksheet);
            package.Dispose();
            workbook.Dispose();
            worksheet.Dispose();
            log.trace("Processing the data");
            // var listCiusFile = new List<string>();
            SaveProductSQL product;
            //Se mapean los datos leidos del archivo excel para mandarlos a insertar en la base de datos
            foreach (DataRow row in datosExcel.Rows)
            {
                log.trace("Reading a row");
                //por cada row, hacer un registro de, direccion id, expiration, familyid, udid y la cantidad de esos
                product = new SaveProductSQL();

                sql.productList.Add(new SaveProductSQL
                {
                    directionId = directionId,
                    expirationDate = datosExcel.Columns.Contains("Caducidad") ? Convert.ToString(row["Caducidad"]) : "",
                    familyProductId = familyId,
                    udid = datosExcel.Columns.Contains("UDID") ? Convert.ToString(row["UDID"]) : "",
                    cantidad = Convert.ToInt32(row["Cantidad"])
                });

                if(Convert.ToInt32(row["Cantidad"]) <= 0 || Convert.ToInt32(row["Cantidad"]) > 1000000) {
                    return 1;
                }

                //for (int i = 0; i < Convert.ToInt32(row["Cantidad"]); i++)
                //{

                //    sql.productList.Add(new SaveProductSQL
                //    {
                //        directionId = directionId,
                //        expirationDate = datosExcel.Columns.Contains("Caducidad") ? Convert.ToString(row["Caducidad"]) : "",
                //        familyProductId = familyId,
                //        udid = datosExcel.Columns.Contains("UDID") ? Convert.ToString(row["UDID"]) : ""
                //    });
                //}
                //listCiusFile.Add(Convert.ToString(row["UDID"]));
            }
            //this.udidFile = string.Join(",", listCiusFile.ToArray());
            log.debug("file read, saving QR data");
            packagingId = familyId; // ajuste 24-03-2022: se quita packagingId y se cambia por la familia;
            sql.packagingId = packagingId;

            List<string> QR = sql.SaveProductsImport();


            this.udidFile = string.Join(",", QR);
            string filename = $"{company}_{family}";
            string[] QRArray = QR.ToArray();
            string fileContent = "";

            fileContent += $"{company}_{family}\n";
            fileContent += $"QR,CIU\n";


            ////------ se comenta
            //for (int i = 0; i < QRArray.Length; i++)
            //{
            //    string[] split = QRArray[i].Split("-"); //ciu1 - ciu2
            //    int inicial = Int32.Parse(split[0]); //ciu1
            //    int final = Int32.Parse(split[1]); //ciu2

            //    //obtener los últimos 10 dígitos para contar desde ahí
            //    while (inicial != final+1)
            //    {

            //        fileContent += $"https://data.traceit.net/origin?ciu={GenerateQR.QR(packagingId, inicial)},{GenerateQR.QR(packagingId, inicial)}\n"; //mandar a llamar aquí la generación de ciu concatenado toh
            //        inicial++;
            //    }


            //    //fileContent += $"https://data.traceit.net/origin?ciu={QRArray[0]},{QRArray[i]}\n";
            //}

            DataTable dataTable = new DataTable();
            string columnName = "";

            FileInfo fi = new FileInfo(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".xlsx");
            
            StringBuilder sb = new StringBuilder();
            sb.Append($"{company}_{family}\n");
            sb.Append($"QR,CIU\n");

            using (ExcelPackage libro = new ExcelPackage())
            {
                ExcelWorksheet hoja = libro.Workbook.Worksheets.Add("CIUs");
                hoja.Cells[1, 1].Value = company + "_" + family;
                
		        var rowCols = new List<int>();
                rowCols.Add(sql.productList[0].cantidad);
                
                int lastIndex = 0;
                int countRow = 0;
                int moveCols = 0;
                for (int r = 0; r < rowCols.Count; r++) {
                    //dataTable = new DataTable();
                    //dataTable.Columns.Add("QR", typeof(string));
                    //dataTable.Columns.Add("CIU", typeof(string));
                    
                    //------
                    for (int i = 0; i < QRArray.Length; i++) {
                        string[] split = QRArray[i].Split("-"); //ciu1 - ciu2
                        int inicial = Int32.Parse(split[0]); //ciu1
                        int final = Int32.Parse(split[1]); //ciu2

                        inicial += lastIndex;
                        //obtener los últimos 10 dígitos para contar desde ahí
                        while (inicial != final+1)
                        {
                            //add some rows
                            //dataTable.Rows.Add($"https://data.traceit.net/origin?ciu={GenerateQR.QR(packagingId, inicial)}",$"{GenerateQR.QR(packagingId, inicial)}"); //mandar a llamar aquí la generación de ciu concatenado toh
                            sb.Append($"https://data.traceit.net/origin?ciu={GenerateQR.QR(packagingId, inicial)},{GenerateQR.QR(packagingId, inicial)}\n"); //mandar a llamar aquí la generación de ciu concatenado toh
                            inicial++;
                            lastIndex++;
                            countRow++;
                            if (countRow >= rowCols[r])
                                break;
                        }
                        countRow = 0;

                        //fileContent += $"https://data.traceit.net/origin?ciu={QRArray[0]},{QRArray[i]}\n";
                    }

                    //columnName = getCellName((r+1) + moveCols);
                    //moveCols += 1;
                    //add all the content from the DataTable, starting at cell A2
                    //hoja.Cells[columnName + "2"].LoadFromDataTable(dataTable, true);
                }

                try
                {
                    //libro.Save();
                    
                    libro.Dispose();
                    hoja.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //Si ya existe uno, simplemente eliminarlo.
            if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".csv"))
                System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".csv");

            //Se crea el archivo de texto con los CIUS generados. -- se comenta
            //System.IO.File.WriteAllText(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".csv", fileContent);

            //Se crea el archivo de texto con los CIUS generados para enviarlo por correo
            //System.IO.File.WriteAllText(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"), fileContent);

            //Write string stream data to file
            FileStream fs = new FileStream(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"), FileMode.Create, FileAccess.Write);
            FileStream fscsv = new FileStream(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".csv", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            StreamWriter swcsv = new StreamWriter(fscsv);
            sw.WriteLine(sb);
            swcsv.WriteLine(sb);
            //Refresh the file stream
            sw.Flush();
            sw.Close();
            fs.Close();

            swcsv.Flush();
            swcsv.Close();
            fscsv.Close();

            //obtener los nombres de familia, origen y usuario para agregarlos al cuerpo del correo
            ProductEmailData emailData = sql.SearchEmailData(familyId, directionId, userId);

            //Se manda el correo electronico
            UserMobileSQL emailList = new UserMobileSQL();
            emailList.type = 5;
            List<string> correosListado = emailList.searchEmail();
            log.trace("Sending email");
            EMAILClient correo = new EMAILClient();
            string mensaje = "Se informa que se han creado nuevos productos: <br />" +
             "Familia: <b>" + emailData.familyName.ToString() + "</b><br />" +
             "Origen: <b>" + emailData.originName.ToString() + "</b><br />" +
             "Usuario: <b>" + emailData.userName.ToString() + "</b><br />" +
             "Adjunto encontrarás el archivo con los CIU generados";
            string asunto = "Notificación de importación de productos";
            //String mensaje = "Adjunto encontraras el archivo con los CIU generados";

            try
            {
                correo.EnviarCorreoImportacion(correosListado.ToArray(), mensaje, ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"), asunto);

                log.trace("Deleting files");
                if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt")))
                    System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"));
            }
            catch (Exception ex)
            {
                log.trace("Deleting files");
                if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt")))
                    System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"));
                throw ex;
            }

            //response.messageEng = "Import done successfully";
            //response.messageEsp = "Importación realizada con éxito";
            log.trace("Import finished");
            return 0;
        }

        private int generateProducts(int amount, string expiry, string udid, int familyId, int directionId, int userId, string company, string family, int packagingId, int columns)
        {
            log.info("Generating products from parameter");
            ProductSQL sql = new ProductSQL();

            //for (int i = 0; i < amount; i++)
            //{
            //    sql.productList.Add(new SaveProductSQL
            //    {
            //        directionId = directionId,
            //        expirationDate = expiry,
            //        familyProductId = familyId,
            //        udid = udid
            //    });
            //}

            if(amount > 1000000) {
                return 1;
            }

            sql.productList.Add(new SaveProductSQL
            {
                directionId = directionId,
                expirationDate = expiry,
                familyProductId = familyId,
                udid = udid,
                cantidad = amount
            });

            log.debug("file read, saving QR data");
            packagingId = familyId; // ajuste 24-03-2022: se quita packagingId y se cambia por la familia;
            sql.packagingId = packagingId;
            List<string> QR = sql.SaveProductsImport();
            this.udidFile = string.Join(",", QR);
            //generate a random string for the file name
            string filename = $"{company}_{family}";
            string[] QRArray = QR.ToArray();
            string fileContent = "";

            fileContent += $"{company}_{family}\n";
            fileContent += $"QR,CIU\n";


            //for (int i = 0; i < QRArray.Length; i++) --se comenta
            //{
            //    string[] split = QRArray[i].Split("-"); //ciu1 - ciu2
            //    int inicial = Int32.Parse(split[0]); //ciu1
            //    int final = Int32.Parse(split[1]); //ciu2

            //    //obtener los últimos 10 dígitos para contar desde ahí
            //    while (inicial != final + 1)
            //    {

            //        fileContent += $"https://data.traceit.net/origin?ciu={GenerateQR.QR(packagingId, inicial)},{GenerateQR.QR(packagingId, inicial)}\n"; //mandar a llamar aquí la generación de ciu concatenado toh
            //        inicial++;
            //    }


            //    //fileContent += $"https://data.traceit.net/origin?ciu={QRArray[0]},{QRArray[i]}\n";
            //}

            //Si ya existe uno, simplemente eliminarlo.
            if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".xlsx"))
                System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".xlsx");

            DataTable dataTable = new DataTable();
            string columnName = "";

            FileInfo fi = new FileInfo(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".xlsx");

            using (ExcelPackage libro = new ExcelPackage(fi))
            {
                ExcelWorksheet hoja = libro.Workbook.Worksheets.Add("CIUs");
                hoja.Cells[1, 1].Value = company + "_" + family;
                
                int rowsPerColumn = columns >= amount ? 1 : (amount / columns); // cantidad / columnas
		
		        var rowCols = new List<int>();
                int countCols = columns >= amount ? amount : columns;
                for (int x = 0; x < countCols; x++) {
			        rowCols.Add(rowsPerColumn);
		        }
                
                if(amount > columns)
                    rowCols[columns - 1] += (amount - (rowsPerColumn * columns)); // agregar el sobrante a la última columna
                
                int lastIndex = 0;
                int countRow = 0;
                int moveCols = 0;
                for (int r = 0; r < rowCols.Count; r++) {
                    dataTable = new DataTable();
                    dataTable.Columns.Add("QR" + (r + 1).ToString(), typeof(string));
                    dataTable.Columns.Add("CIU" + (r + 1).ToString(), typeof(string));
                    
                    //------
                    for (int i = 0; i < QRArray.Length; i++) {
                        string[] split = QRArray[i].Split("-"); //ciu1 - ciu2
                        int inicial = Int32.Parse(split[0]); //ciu1
                        int final = Int32.Parse(split[1]); //ciu2

                        inicial += lastIndex;
                        //obtener los últimos 10 dígitos para contar desde ahí
                        while (inicial != final+1)
                        {
                            //add some rows
                            dataTable.Rows.Add($"https://data.traceit.net/origin?ciu={GenerateQR.QR(packagingId, inicial)}",$"{GenerateQR.QR(packagingId, inicial)}"); //mandar a llamar aquí la generación de ciu concatenado toh
                            inicial++;
                            lastIndex++;
                            countRow++;
                            if (countRow >= rowCols[r])
                                break;
                        }
                        countRow = 0;

                        //fileContent += $"https://data.traceit.net/origin?ciu={QRArray[0]},{QRArray[i]}\n";
                    }

                    columnName = getCellName((r+1) + moveCols);
                    moveCols += 1;
                    //add all the content from the DataTable, starting at cell A2
                    hoja.Cells[columnName + "2"].LoadFromDataTable(dataTable, true);
                }

                try
                {
                    libro.Save();
                    
                    libro.Dispose();
                    hoja.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            //for (int i = 0; i < QRArray.Length; i++)
            //{
            //    fileContent += $"https://data.traceit.net/origin?ciu={QRArray[i]},{QRArray[i]}\n";
            //}

            //Si ya existe uno, simplemente eliminarlo. -- se comenta
            //if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".csv"))
            //    System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".csv");


            //Se crea el archivo de texto con los CIUS generados. -- se comenta
            //System.IO.File.WriteAllText(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".csv", fileContent);

            //obtener los nombres de familia, origen y usuario para agregarlos al cuerpo del correo
            /*ProductEmailData emailData = sql.SearchEmailData(familyId, directionId, userId);

			//Se manda el correo electronico
			UserMobileSQL emailList = new UserMobileSQL();
			emailList.type = 5;
			List<string> correosListado = emailList.searchEmail();
			log.trace("Sending email");
			EMAILClient correo = new EMAILClient();
			string mensaje = "Se informa que se han creado nuevos productos: <br />" +
			"Familia: <b>" + emailData.familyName.ToString() + "</b><br />" +
			"Origen: <b>" + emailData.originName.ToString() + "</b><br />" +
			"Usuario: <b>" + emailData.userName.ToString() + "</b><br />" +
			"Adjunto encontrarás el archivo con los CIU generados";
			string asunto = "Notificación de creación de productos";
			correo.EnviarCorreoImportacion(correosListado.ToArray(), mensaje, ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".txt", asunto);

			log.trace("Deleting files");

			if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".txt"))
				System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".txt");*/

            //response.messageEng = "Import done successfully";
            //response.messageEsp = "Importación realizada con éxito";
            log.trace("Import finished");
            return 0;
        }

        static String reverse(String input) {
            char[] reversedString = input.ToCharArray();
            Array.Reverse(reversedString);
            return new String(reversedString);
        }

        private string getCellName(int index)
        {
            string result = "";
            while (index > 0) {
                // Find remainder
                int rem = index % 26;

                // If remainder is 0, then a
                // 'Z' must be there in output
                if (rem == 0) {
                    result += "Z";
                    index = (index / 26) - 1;
                }
                // If remainder is non-zero
                else {
                    result += (char)((rem - 1) + 'A');
                    index = index / 26;
                }
            }

            // Reverse the string
            result = reverse(result);

            return result;
        }

        private int generateProducts(string fileName, int familyId, int directionId)
        {
            log.info("Generating products from the file");
            ProductSQL sql = new ProductSQL();
            FileInfo excel = new FileInfo(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName);
            var package = new ExcelPackage(excel);
            ProcesarImagen procesar = new ProcesarImagen();

            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.First();
            log.debug("Reading the file");
            DataTable datosExcel = procesar.ConvertToDataTable(worksheet);
            package.Dispose();
            workbook.Dispose();
            worksheet.Dispose();
            log.trace("Processing the data");
            //Se mapean los datos leidos del archivo excel para mandarlos a insertar en la base de datos
            foreach (DataRow row in datosExcel.Rows)
            {
                log.trace("Reading a row");
                for (int i = 0; i < Convert.ToInt32(row["Cantidad"]); i++)
                {

                    sql.productList.Add(new SaveProductSQL
                    {
                        directionId = directionId,
                        expirationDate = Convert.ToString(row["Caducidad"]),
                        familyProductId = familyId,
                        udid = Convert.ToString(row["UDID"])
                    });
                }
            }
            log.debug("file read, saving QR data");
            List<string> QR = sql.SaveProductsImport();

            //Se crea el archivo de texto con los CIUS generados para enviarlo por correo
            System.IO.File.WriteAllLines(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"), QR.ToArray());

            //Se manda el correo electronico
            UserMobileSQL emailList = new UserMobileSQL();
            emailList.type = 5;
            List<string> correosListado = emailList.searchEmail();
            log.trace("Sending email");
            EMAILClient correo = new EMAILClient();
            String mensaje = "Adjunto encontraras el archivo con los CIU generados";
            correo.EnviarCorreoImportacion(correosListado.ToArray(), mensaje, ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"));

            log.trace("Deleting files");
            //Se borran los archivos txt y Excel
            if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName))
                System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName);

            if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt")))
                System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName.Replace(".xlsx", ".txt"));

            //response.messageEng = "Import done successfully";
            //response.messageEsp = "Importación realizada con éxito";
            log.trace("Import finished");
            return 0;
        }

        private int generateProducts(int amount, string expiry, string udid, int familyId, int directionId)
        {
            log.info("Generating products from parameter");
            ProductSQL sql = new ProductSQL();

            //for (int i = 0; i < amount; i++)
            //{
            //    sql.productList.Add(new SaveProductSQL
            //    {
            //        directionId = directionId,
            //        expirationDate = expiry,
            //        familyProductId = familyId,
            //        udid = udid
            //    });
            //}

            //log.debug("file read, saving QR data");
            //List<string> QR = sql.SaveProductsImport();
            ////generate a random string for the file name
            //string filename = GenerateQR.QR(10);
            ////Se crea el archivo de texto con los CIUS generados para enviarlo por correo
            //System.IO.File.WriteAllLines(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".txt", QR.ToArray());

            ////Se manda el correo electronico
            //UserMobileSQL emailList = new UserMobileSQL();
            //emailList.type = 5;
            //List<string> correosListado = emailList.searchEmail();
            //log.trace("Sending email");
            //EMAILClient correo = new EMAILClient();
            //String mensaje = "Adjunto encontraras el archivo con los CIU generados";
            //correo.EnviarCorreoImportacion(correosListado.ToArray(), mensaje, ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".txt");

            //log.trace("Deleting files");

            //if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".txt"))
            //    System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + filename + ".txt");

            //response.messageEng = "Import done successfully";
            //response.messageEsp = "Importación realizada con éxito";
            log.trace("Import finished");
            return 0;
        }

        /// <summary>
        /// Web Method para realizar la importacion de los productos
        /// Desarrollador: David Martinez
        /// </summary>
        /// <returns></returns>
        /// //DEPRECATED
        [Route("importProduct")]
        [HttpGet]
        public IActionResult importProduct()
        {
            log.trace("importProduct");
            SimpleProductResponse response = new SimpleProductResponse();
            ProcesarImagen procesar = new ProcesarImagen();
            try
            {



                //*********************************************************************
                ProductSQL sql = new ProductSQL();

                //Se valida si se tiene algun proceso activo de importacion
                if (!sql.SearchImportProductActive())
                {
                    SearchImportProductData datas = sql.SearchImportProduct();

                    //Se valida si se tiene algun proceso de importacion pendiente
                    if (datas != null)
                    {
                        //Se actualiza el proceso para activar la ejecucion del proceso de importacion
                        sql.status = 1;
                        sql.importId = datas.importId;
                        sql.UpdateImportProduct();

                        //Se lee el archivo de excel y se pasa a un DataTable
                        FileInfo excel = new FileInfo(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + datas.fileName);
                        var package = new ExcelPackage(excel);

                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets.First();

                        DataTable datosExcel = procesar.ConvertToDataTable(worksheet);
                        package.Dispose();
                        workbook.Dispose();
                        worksheet.Dispose();

                        //Se mapean los datos leidos del archivo excel para mandarlos a insertar en la base de datos
                        foreach (DataRow row in datosExcel.Rows)
                        {
                            for (int i = 0; i < Convert.ToInt32(row["Cantidad"]); i++)
                            {

                                sql.productList.Add(new SaveProductSQL
                                {
                                    directionId = datas.directionId,
                                    expirationDate = Convert.ToString(row["Caducidad"]),
                                    familyProductId = datas.familyProductId,
                                    udid = Convert.ToString(row["UDID"])
                                });
                            }
                        }
                        List<string> QR = sql.SaveProductsImport();

                        //Se crea el archivo de texto con los CIUS generados para enviarlo por correo
                        //System.IO.File.WriteAllLines(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + datas.fileName.Replace(".xlsx", ".txt"), QR.ToArray());
                        System.IO.File.WriteAllText(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + datas.fileName.Replace(".xlsx", ".txt"), String.Join(",", QR.ToArray()));

                        //Se manda el correo electronico
                        UserMobileSQL emailList = new UserMobileSQL();
                        emailList.type = 5;
                        List<string> correosListado = emailList.searchEmail();

                        EMAILClient correo = new EMAILClient();
                        String mensaje = "Adjunto encontraras el archivo con los CIU generados";
                        correo.EnviarCorreoImportacion(correosListado.ToArray(), mensaje, ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + datas.fileName.Replace(".xlsx", ".txt"));

                        //Se actualiza el proceso para ponerlo como terminado
                        sql.status = 2;
                        sql.importId = datas.importId;
                        sql.UpdateImportProduct();

                        //Se borran los archivos txt y Excel
                        if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + datas.fileName))
                            System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + datas.fileName);

                        if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + datas.fileName.Replace(".xlsx", ".txt")))
                            System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + datas.fileName.Replace(".xlsx", ".txt"));

                        response.messageEng = "Import done successfully";
                        response.messageEsp = "Importación realizada con éxito";
                    }
                    else
                    {
                        response.messageEng = "There are currently no pending imports";
                        response.messageEsp = "Actualmente no hay ninguna importacion pendiente";
                    }
                }
                else
                {
                    response.messageEng = "There is currently an open product import process";
                    response.messageEsp = "Actualmente se encuentra un proceso abierto de importación de productos";
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
        /// Generar archivo de etiquetas
        /// Desarrollador: Daniel Rodríguez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("generateProductCode")]
        [HttpPost]
        [Authorize]
        public IActionResult generateProductCode([FromBody] ProductListId request)
        {
            //log.info("regenerate stickers");
            //SimpleProductResponse response = new SimpleProductResponse();
            //ProductSQL sql = new ProductSQL();
            //log.debug("got a list of " + request.idProducts.Count + "elements");
            //log.debug(Newtonsoft.Json.JsonConvert.SerializeObject(request.idProducts));
            //List<string> QR = sql.getProductQRCode(request.idProducts);
            //log.debug("There is a response: " + QR.Count);
            //log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(QR));
            //string fileName = GenerateQR.QR(10);//create a randon name
            //                                    //Se crea el archivo de texto con los CIUS generados para enviarlo por correo
            //System.IO.File.WriteAllText(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName + ".txt", String.Join(",", QR.ToArray()));

            ////Se manda el correo electronico
            //UserMobileSQL emailList = new UserMobileSQL();
            //emailList.type = 5;
            //List<string> correosListado = emailList.searchEmail();
            //log.trace("Sending email");
            //EMAILClient correo = new EMAILClient();
            //String mensaje = "Adjunto encontraras el archivo con los CIU generados";
            //correo.EnviarCorreoImportacion(correosListado.ToArray(), mensaje, ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName + ".txt");

            ////Se borran los archivos txt y Excel
            //if (System.IO.File.Exists(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName + ".txt"))
            //    System.IO.File.Delete(ConfigurationSite._cofiguration["Paths:pathFilesImportProduct"] + fileName + ".txt");

            ////response.messageEng = "Import done successfully";
            ////response.messageEsp = "Importación realizada con éxito";
            //log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //return Ok(response);
            return Ok("");
        }
    }
}