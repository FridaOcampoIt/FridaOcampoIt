using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Product;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Response;
using NetCoreApiRest.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WSTraceIT.InterfacesSQL
{
    public class ProductSQL : DBHelperDapper
    {
        #region Propiedades
        public List<SaveProductSQL> productList;
        public int idProduct;
        public string udid;
        public int familyProductId;
        public int? company;
        public int packagingId;
        public int cantidad;

        public string fileName;
        public int directionId;
        public int status;
        public int importId;
        public bool isgtin;
        public bool isciu;
        public bool ixhex;
        private LoggerD4 log = new LoggerD4("ProductSQL");
        #endregion

        #region Constructor
        public ProductSQL()
        {
            this.productList = new List<SaveProductSQL>();
            this.idProduct = 0;
            this.udid = String.Empty;
            this.familyProductId = 0;
            this.company = 0;
            this.packagingId = 0;
            this.cantidad = 0;

            this.fileName = String.Empty;
            this.directionId = 0;
            this.status = 0;
            this.importId = 0;
            this.isgtin = false;
            this.isciu = false;
            this.ixhex = false;
        }
        #endregion

        #region Metodos públicos
        /// <summary>
        /// Metodo para guardar los productos
        /// Desarrollador: David Martinez
        /// </summary>
        public List<string> SaveProductsImport()
        {
            List<string> QR = new List<string>();
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                //consultar el último registro (no de id si no de ciu) insertado (o sea que lo tendrás que descomponer)
                var ultimoRegistro = $"SELECT IFNULL(MAX(CAST(Fin AS UNSIGNED)),0) FROM cat_048_producto where FamiliaProductoId  = {productList[0].familyProductId };";

                int maximoRegistro = ConsultaCommand<int>(ultimoRegistro)[0];

                int inicio = 0; //ultimo registro insertado en la tabla
                int final = 0;

                string query = "insert into cat_048_producto(UDID,FechaCaducidad,Inicio,Fin,EmbalajeId,FamiliaProductoId,TipoProductoId,DireccionId) values ";
                string strDirId = "";
                //por cada cantidad solicitada
                foreach (var product in productList)
                {

                    inicio = (maximoRegistro + 1); //donde va a iniciar el siguiente rango de cius
                    final = (maximoRegistro + product.cantidad); // final del rango de los cius solicitados
                    strDirId = product.directionId > 0 ? product.directionId.ToString() : "NULL";
                    query += $"('{product.udid}','{product.expirationDate}','{inicio}','{final}',NULL,'{product.familyProductId}','1','{strDirId}'),";

                    maximoRegistro = final;

                    QR.Add($"{inicio}-{final}");
                }

                query = query.TrimEnd(','); //Quitar la coma final paa el query;
                log.debug(query);

                ConsultaCommand<string>(query);

                TransComit();
                CerrarConexion();

                //System.IO.File.Delete(path + filename + ".tmp.txt");//delete the file after import
                return QR;
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
        /// Metodo para guardar los productos
        /// Desarrollador: David Martinez
        /// </summary>
        public List<string> SaveProduct()
        {
            List<string> QR = new List<string>();
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                foreach (var product in productList)
                {
                    bool bandera = true;
                    while (bandera)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("_udid", product.udid, DbType.String);
                        parameters.Add("_expirationDate", product.expirationDate, DbType.String);
                        parameters.Add("_qrCode", GenerateQR.QR(13, packagingId), DbType.String);
                        parameters.Add("_familyProductId", product.familyProductId, DbType.Int32);
                        parameters.Add("_directionId", product.directionId, DbType.Int32);
                        parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                        parameters = EjecutarSPOutPut("spi_guardarProducto", parameters);
                        int response = parameters.Get<int>("_response");

                        if (response == 0)
                            throw new Exception("Error al ejecutar el sp");
                        else if (response == -1)
                            bandera = true;
                        else
                        {
                            QR.Add(parameters.Get<string>("_qrCode"));
                            bandera = false;
                        }
                    }
                }

                TransComit();
                CerrarConexion();

                return QR;
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
        /// Metodo para actualizar el producto
        /// Desarrollador: David Martinez
        /// </summary>
        public void UpdateProduct()
        {
            log.trace("UpdateProduct");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                foreach (var product in productList)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("_idProduct", product.idProduct, DbType.Int32);
                    parameters.Add("_udid", product.udid, DbType.String);
                    parameters.Add("_expirationDate", product.expirationDate, DbType.String);
                    parameters.Add("_familyProductId", product.familyProductId, DbType.Int32);
                    parameters.Add("_directionId", product.directionId, DbType.Int32);
                    parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                    parameters = EjecutarSPOutPut("spu_edicionProducto", parameters);
                    int response = parameters.Get<int>("_response");
                    ConsultaCommand<string>("UPDATE cat_004_producto SET estatus = " + product.status + " WHERE ProductoId = " + product.idProduct + ";");

                    if (response == 0)
                        throw new Exception("Error al ejecutar el sp");
                }

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
        /// Metodo para eliminar un producto que esta registrado
        /// Desarrollador: David Martinez
        /// </summary>
        public void DeleteProduct()
        {
            log.trace("DeleteProduct");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_idProduct", idProduct, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spd_eliminarProductos", parameters);
                int response = parameters.Get<int>("_response");

                if (response == 0)
                    throw new Exception("Error al ejecutar el sp");
                else if (response == -1)
                    throw new Exception("No se puede eliminar el producto porque tiene una garantia registrada");
                else if (response == -2)
                    throw new Exception("No se puede eliminar el producto porque tiene un registro de robo");
                else if (response == -3)
                    throw new Exception("No se puede eliminar el producto porque un usuario móvil lo tiene agregado en su catálogo");

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
        /// Metodo para realizar la busqueda de productos
        /// Desarrollador: David Martinez
        /// </summary>
        /// <returns></returns>
        public List<ProductList> SearchProduct()
        {
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_udid", udid, DbType.String);
                parameters.Add("_idFamily", familyProductId, DbType.Int32);

                List<ProductList> response = Consulta<ProductList>("spc_ConsultaProductos", parameters);
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
        /// Metodo para realizar la consulta del detalle del producto
        /// Desarrollador: David Martinez
        /// </summary>
        /// <returns></returns>
        public ProductDetails SearchProductDetails()
        {
            log.trace("SearchProductDetails");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_idProduct", idProduct, DbType.Int32);

                ProductDetails response = Consulta<ProductDetails>("spc_ConsultaProductosDatos", parameters).FirstOrDefault();
                var status_p = ConsultaCommand<string>("select estatus from cat_004_producto where ProductoId = '" + idProduct + "' ;")[0];
                response.status = Int32.Parse(status_p);
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
        /// Metodo para consultar los combos para el modulo de productos
        /// Desarrollador: David Martinez
        /// </summary>
        /// <returns></returns>
        public DropDownFront SearchDropDownProduct(int opc = 0)
        {
            log.trace("SearchDropDownProduct");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_company", company, DbType.Int32);

                List<Type> types = new List<Type>();
                types.Add(typeof(familyDropDown));
                if (opc == 0)
                    types.Add(typeof(TraceITListDropDown));
                if(opc == 0)
                    types.Add(typeof(TraceITListDropDown));

                var respSQL = ConsultaMultiple(opc == 0 ? "spc_ConsultaCombosProductos" : "spc_ConsultaCombosProductosImport", types, parameters);

                DropDownFront resp = new DropDownFront();

                resp.familyDropDown = respSQL[0].Cast<familyDropDown>().ToList();
                if (opc == 0)
                    resp.originDropDown = respSQL[1].Cast<TraceITListDropDown>().ToList();
                if(opc == 0)
                    resp.companyDropDown = respSQL[2].Cast<TraceITListDropDown>().ToList();

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

        /// <summary>
        /// Metodo para consultar origenes de la familia especificada 
        /// Desarrollador: David Martinez
        /// </summary>
        /// <returns></returns>
        public DropDownFront SearchDropDownOrigen(int familia, int opc = 0)
        {
            log.trace("SearchDropDownOrigen");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_familiaId", familia, DbType.Int32);

                List<Type> types = new List<Type>();
                types.Add(typeof(TraceITListDropDown));

                var respSQL = ConsultaMultiple(opc == 0 ? "spc_ConsultaCombosProductosOrigen" : "spc_ConsultaCombosProductosOrigenImport", types, parameters);

                DropDownFront resp = new DropDownFront();

                resp.originDropDown = respSQL[0].Cast<TraceITListDropDown>().ToList();

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

        /// <summary>
        /// Metodo para registrar el archivo para que pueda ser procesado
        /// Desarrollador: David Martinez
        /// </summary>
        public void SaveImportProduct()
        {
            log.trace("SaveImportProduct");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_fileName", fileName, DbType.String);
                parameters.Add("_familyProductId", familyProductId, DbType.Int32);
                parameters.Add("_directionId", directionId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spi_guardarImportacionProductos", parameters);
                int response = parameters.Get<int>("_response");

                if (response == 0)
                    throw new Exception("Error al ejecutar el sp");

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
        /// Metodo para saber si existe un proceso activo de importacion
        /// Desarrollador: David Martinez
        /// </summary>
        /// <returns></returns>
        public bool SearchImportProductActive()
        {
            log.trace("SearchImportProductActive");
            bool exists = false;
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                SearchImportProductActive product = Consulta<SearchImportProductActive>("spc_consultaImportacionActiva").FirstOrDefault();
                CerrarConexion();

                if (product.ImportProduct > 0)
                    exists = true;

                return exists;
            }
            catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                CerrarConexion();
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para traerse las importaciones pendientes
        /// Desarrollador: David Martinez
        /// </summary>
        /// <returns></returns>
        public SearchImportProductData SearchImportProduct()
        {
            log.trace("SearchImportProduct");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                SearchImportProductData response = Consulta<SearchImportProductData>("spc_consultaImportacionProductos").FirstOrDefault();
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
        /// Metodo para actualizar la importacion de productos
        /// Desarrollador: David Martinez
        /// </summary>
        public void UpdateImportProduct()
        {
            log.trace("UpdateImportProduct");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_status", status, DbType.Int32);
                parameters.Add("_importId", directionId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spu_actualizarImportacion", parameters);
                int response = parameters.Get<int>("_response");

                if (response == 0)
                    throw new Exception("Error al ejecutar el sp");

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
        /// Regeneracion de las etiquetas
        /// Desarrollador: Daniel Rodríguez
        /// </summary>
        public List<string> getProductQRCode(List<int> ids)
        {
            List<string> QR = new List<string>();
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();
                string idParam = "";
                foreach (var id in ids)
                {
                    idParam += id + ",";
                }
                idParam = idParam.TrimEnd(',');
                log.trace("About to send a list of id to the SP: " + idParam);
                //ConsultaCommand<string>("LOAD DATA INFILE '"+ path + filename + ".tmp.txt" + "' INTO TABLE cat_004_producto FIELDS TERMINATED BY ',' (UDID,FechaCaducidad,CodigoQR,FamiliaProductoId,TipoProductoId,DireccionId)");
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("idProducts", idParam, DbType.String);

                List<CIUList> cius = Consulta<CIUList>("spc_ConsultaProductosCIU", parameters);
                foreach (var ciu in cius)
                {
                    QR.Add(ciu.ciu);
                }


                TransComit();
                CerrarConexion();

                //System.IO.File.Delete(path + filename + ".tmp.txt");//delete the file after import
                return QR;
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
        /// Metodo para obtener los datos para el correo de confirmación
        /// Desarrollador: Daniel Rodríguez
        /// </summary>
        /// <returns></returns>
        public ProductEmailData SearchEmailData(int familyId, int originId, int userId)
        {
            log.trace("Search for the email data names");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_familyId", familyId, DbType.Int32);
                parameters.Add("_directionId", originId, DbType.Int32);
                parameters.Add("_userId", userId, DbType.Int32);

                List<Type> types = new List<Type>
                {
                    typeof(string),
                    typeof(string),
                    typeof(string)
                };

                var respSQL = ConsultaMultiple("sps_productoEmailDatos", types, parameters);

                ProductEmailData resp = new ProductEmailData();

                //resp.familyName = respSQL[0].Cast<string>().ToString();
                resp.familyName = (respSQL[0].Cast<string>().ToArray())[0];  //respSQL[0].Cast<string>().ToA;
                resp.originName = (respSQL[1].Cast<string>().ToArray())[0];
                resp.userName = (respSQL[2].Cast<string>().ToArray())[0];

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

        /// <summary>
        /// Metodo para obtener los datos para nueva vista de Origin
        /// Desarrollador: Omar Larrion
        /// </summary>
        /// <returns></returns>
        public List<ProductDetailsCIU> searchDataCIU(string ciu)
        {
            log.trace("searchDataCIU");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parametersOrigin = new DynamicParameters();

                //verificar si es hexa
                string range = "";
                bool izHex = false;
                izHex = isHexa(ciu);
                List<string> results = new List<string>();
                string GTIN = "";
                string numero = Convert.ToInt64(ciu, 16).ToString();
                Console.WriteLine($"CiuRange: {numero}");

                List<BusquedaCiu> listadoCodigoAlertasPallets = new List<BusquedaCiu>();
                List<BusquedaCiu> listadoCodigoAlertasAgrupaciones = new List<BusquedaCiu>();
                List<string> codigoCajas = new List<string>();
                //Obtenemos la cadena en decimal
                string data = ulong.Parse(ciu, System.Globalization.NumberStyles.HexNumber).ToString();
                //Evaluamos si es código nuevo o viejo
                string nuevoViejo = data.Substring(0, 1);
                //Con esto obtenemos el embalaje o la familia (según sea el caso)
                string familiaEmbalaje = data.Substring(1, 5);
                familiaEmbalaje = familiaEmbalaje.Trim(new char[] { '0' });
                string aux = data.Substring(data.Length - 10);
                //Quitamos el auxiliar
                string familia = data.Substring(1);
                //Rescatamos la familia (eliminando el rango)
                familia = familia.Substring(0, 5).TrimStart('0');
                Console.WriteLine($"Este es el aux: {aux} \n Familia: {familia}, \n CIU: {ciu}");
                int fpId = 0;
                alertas auxiliarAlerta = new alertas();
                //Consulta de reporte por ciu  solo si son nuevos cius 
                if (nuevoViejo.Equals("2"))
                {
                    //Buscamos la compañia respecto a la familia que se esta consultando
                    var compania = ConsultaCommand<int>($"SELECT CompaniaId FROM cat_001_familiaproducto WHERE FamiliaProductoId  = {familia};").FirstOrDefault();
                    //Console.WriteLine($"Compañia {compania}");
                    //Buscamos un reporte por ciu primero.
                    var reporteFamilia = ConsultaCommand<BusquedaCiu>($"" +
                                                           $"SELECT " +
                                                           $"	hr.RoboProductoId as idReporte, " +
                                                           $"	hr.CodigoAlerta as codigoAlerta, " +
                                                           $"	hr.FK_CMMTipoReporte as tipoReporte," +
                                                           $"	hr.AgrupacionId agrupacionId " +
                                                           $"FROM his_004_robo AS hr " +
                                                           $" WHERE " +
                                                           $"   hr.CompaniaId = {compania} " +
                                                           $"   AND " +
                                                           $"   hr.Estatus = 1000004 " +
                                                           $"   AND " +
                                                           $"   hr.FK_CMMTipoReporte = 1000009 " +
                                                           $"   AND " +
                                                           $"   hr.FK_FamiliaId = {familia};").FirstOrDefault();
                    //Retornamos la alerta si esta por familia
                    if (reporteFamilia != null)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("_idReporte", reporteFamilia.idReporte, DbType.Int32);
                        auxiliarAlerta = Consulta<alertas>("spc_busquedaAlertas", parameters).FirstOrDefault();
                    }
                    else
                    {
                        //Buscamos un reporte por ciu primero.
                        var reporte = ConsultaCommand<BusquedaCiu>($"" +
                                                               $"SELECT " +
                                                               $"	hr.RoboProductoId as idReporte, " +
                                                               $"	hr.CodigoAlerta as codigoAlerta, " +
                                                               $"	hr.FK_CMMTipoReporte as tipoReporte," +
                                                               $"	hr.AgrupacionId agrupacionId " +
                                                               $"FROM his_004_robo AS hr " +
                                                               $" WHERE " +
                                                               $"   hr.CompaniaId = {compania} " +
                                                               $"   AND " +
                                                               $"   hr.Estatus = 1000004 " +
                                                               $"   AND " +
                                                               $"   hr.FK_CMMTipoReporte = 1000012 " +
                                                               $"   AND " +
                                                               $"   hr.CodigoAlerta = '%{ciu.ToUpper()}%'; ").FirstOrDefault();
                        //Retornamos el reporte de la alerta si existe en un ciu espeficifico
                        if (reporte != null)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("_idReporte", reporte.idReporte, DbType.Int32);
                            auxiliarAlerta = Consulta<alertas>("spc_busquedaAlertas", parameters).FirstOrDefault();
                        }
                        else
                        {
                            //Buscamos en los pallets/Agrupaciones la caja que se esta activando
                            //Traemos todas las alertas que se encuentre activas (Solo si son de Pallet o agrupaciones)

                            listadoCodigoAlertasPallets = ConsultaCommand<BusquedaCiu>($"" +
                                $"SELECT" +
                                $"  busqueda.idReporte, " +
                                $"  busqueda.codigoAlerta, " +
                                $"  busqueda.tipoReporte, " +
                                $"  busqueda.agrupacionId " +
                                $"FROM( " +
                                $"  SELECT " +
                                $"      RoboProductoId as idReporte, " +
                                $"      CodigoAlerta as codigoAlerta, " +
                                $"      FK_CMMTipoReporte as tipoReporte, " +
                                $"      AgrupacionId agrupacionId, " +
                                $"      Estatus " +
                                $"  FROM his_004_robo " +
                                $"  WHERE " +
                                $"      FK_CMMTipoReporte = 1000010 or FK_CMMTipoReporte = 1000013 or  FK_CMMTipoReporte = 1000011 " +
                                $"      AND " +
                                $"      CompaniaId = {compania} " +
                                $")busqueda " +
                                $"WHERE busqueda.Estatus = 1000004; ").ToList();
                            if (listadoCodigoAlertasPallets != null)
                            {
                                //Iteramos todas las alerta de pallet o agrupaciones
                                foreach (var codigo in listadoCodigoAlertasPallets)
                                {
                                    Console.WriteLine($"codigo {codigo.idReporte}, {codigo.codigoAlerta}");
                                    //Guardamos el id de la alerta actual (para si se encuentra un resultado, rescatarlo)
                                    var idReporteActual = codigo.idReporte;
                                    var resultadoEncontrado = ConsultaCommand<string>($"" +
                                               $"SELECT" +
                                               $"  *   " +
                                               $"FROM(" +
                                               $"  SELECT" +
                                               $"      DISTINCT(hod.EtiquetaID),   " +
                                               $"      REPLACE(LTRIM(REPLACE(RIGHT(CONV(hod.RangoMin, 16, 10), 10), '0', ' ')), ' ', '0') RangoMinSinCero,     " +
                                               $"      REPLACE(LTRIM(REPLACE(RIGHT(CONV(hod.RangoMax, 16, 10), 10), '0', ' ')), ' ', '0') RangoMaxSinCero,     " +
                                               $"      rp.ProductoMovimientoId,    " +
                                               $"      rp.CajaId,  " +
                                               $"      cm.MovimientosId,   " +
                                               $"      rp2.operacionId,    " +
                                               $"      hod.RangoMin, " +
                                               $"      hod.RangoMax" +
                                               $"  FROM rel_021_productomovimiento rp  " +
                                               $"  INNER JOIN cat_027_movimientos cm on cm.MovimientosId = rp.MovimientosId    " +
                                               $"  INNER JOIN his_020_movimientos hm on hm.MovimientosFK = cm.MovimientosId    " +
                                               $"  INNER JOIN rel_022_productooperacion rp2 ON rp2.movimientoId = cm.MovimientosId     " +
                                               $"  INNER JOIN his_047_operacionDetalle hod on hod.OperacionId = rp2.operacionId    " +
                                               $"  WHERE " +
                                               $"      rp.CajaId like '%__{codigo.codigoAlerta}%'  " +
                                               $"      AND     " +
                                               $"      cm.isID = {codigo.agrupacionId} " +
                                               $")rangos   " +
                                               $"  WHERE" +
                                               $"  {Convert.ToInt32(aux)} BETWEEN rangos.RangoMinSinCero AND rangos.RangoMaxSinCero; ").ToList();
                                    if (resultadoEncontrado.Count > 0)
                                    {
                                        DynamicParameters parameters = new DynamicParameters();
                                        parameters.Add("_idReporte", idReporteActual, DbType.Int32);
                                        auxiliarAlerta = Consulta<alertas>("spc_busquedaAlertas", parameters).FirstOrDefault();
                                    }
                                }
                             }
                           
                        }
                    }
                }

                if (izHex)
                {

                    long numbah = Convert.ToInt64(ciu, 16);
                    //Anidar a ranges los últimos 10 dígitos del long , los cuales pertenecen a un rango de ids de producto
                    string numrango = numbah.ToString().Substring(numbah.ToString().Length - 10);
                    numrango = numrango.TrimStart('0');

                    range = numrango.ToString();
                    //Obtener el familaiproducto id
                    //int fpId = ConsultaCommand<int>($"SELECT familiaproductoId FROM cat_048_producto WHERE {range} BETWEEN Inicio AND Fin;").FirstOrDefault();
                    //Solucionamos el problema de los rangos
                    if(nuevoViejo.Equals("1"))
                    {
                        fpId = ConsultaCommand<int>($"" +
                        $"SELECT " +
                        $"  familiaproductoId " +
                        $"FROM cat_048_producto " +
                        $"WHERE {range} BETWEEN Inicio AND Fin " +
                        $"AND " +
                        $" EmbalajeId = {familia};").FirstOrDefault();
                    }
                    else if(nuevoViejo.Equals("2"))
                    {
                        fpId = ConsultaCommand<int>($"" +
                        $"SELECT " +
                        $"  familiaproductoId " +
                        $"FROM cat_048_producto " +
                        $"WHERE {range} BETWEEN Inicio AND Fin " +
                        $"AND " +
                        $"FamiliaProductoId = {familia} " +
                        $";").FirstOrDefault();
                    }


                    //consultar el gtin directamente del id de la familia producto
                    results = ConsultaCommand<string>($"SELECT GTIN FROM cat_001_familiaproducto WHERE familiaProductoId = {fpId};");

                    // después de que se encontró en la tabla nueva agrupados, generar su registro en la tabla anterior individual para los procesos siguientes
                    // y así no hacer cambios al ciu leido y pasarlo a origen con el hexa

                    if (results.Count > 0)
                    {
                        GTIN = results[0];
                        izHex = true;
                    }
                    else
                    {
                        throw new Exception("El CIU no se encuentra registrado en el sistema");
                    }
                }
                else
                {
                    //buscar primero el gtin
                    results = ConsultaCommand<string>("SELECT fp.GTIN " +
                                                    "FROM cat_001_familiaproducto fp " +
                                                    "WHERE fp.GTIN = '" + ciu + "' GROUP BY fp.GTIN  ; ");
                    this.isgtin = true;

                    //si no encuentra gtin buscar por qr
                    if (results.Count == 0)
                    {
                        this.isgtin = false;
                        //Obtener el familia id del producto
                        fpId = ConsultaCommand<int>($"SELECT familiaProductoId FROM cat_004_producto WHERE CodigoQR = '{ciu}';").FirstOrDefault();

                        //if (fpId == 0)
                        //{
                        //    throw new Exception("El CIU no se encuentra registrado en el sistema");
                        //}

                        //buscar el gtin de la familia
                        results = ConsultaCommand<string>($"SELECT GTIN FROM cat_001_familiaproducto WHERE familiaProductoId = '{fpId}';");
                        this.isciu = true;
                    }

                    if (results.Count > 0)
                    {
                        GTIN = results[0];
                        izHex = izHex; // o sea false.
                    }
                    else
                    {
                        throw new Exception("El CIU no se encuentra registrado en el sistema");
                    }
                }

                productoId productoId = new productoId();
                familiaProducto familiaProducto = new familiaProducto();
                List<string> origenProductoId = new List<string>();
                List<string> operacionId = new List<string>();

                if (izHex)
                {
                    long numbah = Convert.ToInt64(ciu, 16);
                    //Anidar a ranges los últimos 10 dígitos del long , los cuales pertenecen a un rango de ids de producto
                    long numrango = Int64.Parse(numbah.ToString().Substring(numbah.ToString().Length - 10));
                    range = numrango.ToString();
                    //Una vez más arreglamos las pendejadas
                    //productoId = ConsultaCommand<productoId>($"SELECT ProductoId, UDID, FechaCaducidad, DireccionId, FamiliaProductoId From cat_048_producto WHERE {range} BETWEEN Inicio AND Fin;").FirstOrDefault();
                    //Solucionamos el problema de los rangos
                    if (nuevoViejo.Equals("1"))
                    {
                        productoId = ConsultaCommand<productoId>($"" +
                        $"SELECT " +
                        $"  ProductoId, UDID, FechaCaducidad, DireccionId, FamiliaProductoId  " +
                        $"FROM cat_048_producto " +
                        $"WHERE {range} BETWEEN Inicio AND Fin " +
                        $"AND " +
                        $" EmbalajeId = {familia};").FirstOrDefault();
                    }
                    else if (nuevoViejo.Equals("2"))
                    {
                        productoId = ConsultaCommand<productoId>($"" +
                        $"SELECT " +
                        $"  ProductoId, UDID, FechaCaducidad, DireccionId, FamiliaProductoId " +
                        $"FROM cat_048_producto " +
                        $"WHERE {range} BETWEEN Inicio AND Fin " +
                        $"AND " +
                        $"FamiliaProductoId = {familia} " +
                        $";").FirstOrDefault();
                    }
                    familiaProducto = ConsultaCommand<familiaProducto>($"SELECT FamiliaProductoId, Descripcion, ImagenUrl, Nombre, Modelo, GTIN FROM cat_001_familiaproducto WHERE FamiliaProductoId = {productoId.FamiliaProductoId}").FirstOrDefault();
                    origenProductoId = ConsultaCommand<string>($"SELECT * FROM tra_003_OrigenProducto WHERE ProductoId = {productoId.ProductoId};");

                    parametersOrigin.Add("_ciu", range, DbType.String);
                    parametersOrigin.Add("_isHex", izHex == true ? 1 : 0, DbType.Int16);
                    parametersOrigin.Add("_direccionId", productoId.DireccionId, DbType.String);
                    parametersOrigin.Add("_familiaProducto", int.Parse(familia), DbType.Int32);
                    parametersOrigin.Add("_type", 0, DbType.Int16);
                }
                else
                {

                    //obtener los ids que se utiliza en los NO provenientes de producto


                    if (isgtin)
                    {
                        //si es un gtin no consulta otra cosa mas que la información de la familia, y de origen nel.
                        parametersOrigin.Add("_familiaProducto", 0, DbType.Int16);
                        parametersOrigin.Add("_direccionId", 0, DbType.String);
                        parametersOrigin.Add("_type", 2, DbType.Int16);
                    }
                    else if (isciu)
                    {

                        productoId = ConsultaCommand<productoId>($"SELECT ProductoId, UDID, FechaCaducidad, DireccionId, FamiliaProductoId From cat_004_producto WHERE CodigoQR = '{ciu}';").FirstOrDefault();
                        familiaProducto = ConsultaCommand<familiaProducto>($"SELECT FamiliaProductoId, Nombre, ImagenUrl, Modelo, GTIN FROM cat_001_familiaproducto WHERE FamiliaProductoId = {productoId.FamiliaProductoId}").FirstOrDefault();
                        parametersOrigin.Add("_familiaProducto", int.Parse(familia), DbType.Int32);
                        parametersOrigin.Add("_direccionId", productoId.DireccionId, DbType.String);
                        parametersOrigin.Add("_type", 1, DbType.Int16);
                    }

                    parametersOrigin.Add("_ciu", ciu, DbType.String);
                    parametersOrigin.Add("_isHex", izHex == true ? 1 : 0, DbType.Int16);
                }
                parametersOrigin.Add("_isNewOrOld", int.Parse(nuevoViejo), DbType.Int32);
                List<ProductDetailsCIU> response = Consulta<ProductDetailsCIU>("spc_ConsultaOrigenProducto", parametersOrigin);
                if (izHex || isciu)
                {
                    response[0].Ciu = ciu;
                    response[0].Caducidad = productoId.FechaCaducidad;
                    response[0].NumSerie = productoId.UDID;
                    response[0].Lote = productoId.UDID;

                    response[0].Nombre = familiaProducto.Nombre;
                    response[0].Presentacion = familiaProducto.Modelo;
                    response[0].GTIN = familiaProducto.GTIN;
                    response[0].Descripcion = familiaProducto.Descripcion;
                }
                 response[0].listaAlerta = auxiliarAlerta;

                foreach (ProductDetailsCIU item in response) {
                    item.Caducidad = "";
                };

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

        public bool isHexa(string number)
        {
            try
            {
                if (number.Length == 13)
                {
                    ulong a = ulong.Parse(number, System.Globalization.NumberStyles.HexNumber);
                    return a.ToString()[0] == '1' ? true : a.ToString()[0] == '2' ? true : false;
                }
                else
                {

                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }



    public static class GenerateQR
    {
        /// <summary>
        /// Metodo para la generacion de qr aleatorio para el producto
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="longitud"></param>
        /// <returns></returns>
        public static string QR(int packagingId, int rango)
        {
            //const string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder token = new StringBuilder();
            Random rnd = new Random();
            //for (int i = 0; i < longitud; i++)
            //{
            //    int indice = rnd.Next(alfabeto.Length);
            //    token.Append(alfabeto[indice]);
            //}


            // complementar a 5 digitos con 0.
            var len = packagingId.ToString().Length;
            var embalajeCompleto = "";
            while (len < 5)
            {
                embalajeCompleto += "0";
                len++;
            }

            var lenRango = rango.ToString().Length;
            var rangoCompleto = "";
            while (lenRango < 10)
            {
                rangoCompleto += "0";
                lenRango++;
            }

            // 5 dígitos 00000; máx 99999
            embalajeCompleto += packagingId;
            //10 dígitos 0;
            rangoCompleto += rango;

            token.Append("2");
            token.Append(embalajeCompleto);
            token.Append(rangoCompleto);

            long ciu = (long)Convert.ToInt64(token.ToString());

            return ciu.ToString("X");
        }


    }

}
