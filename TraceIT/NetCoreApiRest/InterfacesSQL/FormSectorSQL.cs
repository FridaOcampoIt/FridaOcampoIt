using Dapper;
using NetCoreApiRest.Utils;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using static DWUtils.DBHelperDapper;
using System.Data;
using WSTraceIT.Models.Response;
using System.Linq;
using DWUtils;
using WSTraceIT.Models.Base.FormSector;

namespace WSTraceIT.InterfacesSQL
{
    public class FormSectorSQL: DBHelperDapper
    {
        #region propiedades
        public int formularioSectorId;
        public int sectorId;
        public int formularioId;
        private LoggerD4 log = new LoggerD4("FormSectorSQL");


        #endregion

        #region Constructor
        public FormSectorSQL()
        {
            this.formularioSectorId = 0;
            this.sectorId = 0;
            this.formularioId = 0;

        }
        #endregion

        #region save
        /// <summary>
        /// Metodo para guardar los formularios
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void SaveFormSector()
        {
            log.trace("SaveFormSector");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
              /*  parameters.Add("_FormularioSectorId", nombre, DbType.Int32);

                parameters.Add("_nombreFormuario", nombreSector, )
                    parameters.Add("_usuarioCreadorId", usuarioCreadorId)
                    "_sectorId" */
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spi_guardarFormSector", parameters);
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
        #endregion

        #region search
        /// <summary>
        /// Metodo para consultar el formulario por id
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public searchFormSectorByIdResponse searchFormSectorById()
        {
            log.trace("searchFormSectorById");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_formularioSectorId", formularioSectorId, DbType.Int64);

                searchFormSectorByIdResponse resp = Consulta<searchFormSectorByIdResponse>("spc_consultaFormSectorById", parameters).FirstOrDefault();
                return resp;
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
        #endregion

        #region update
        /// <summary>
        /// Metodo para actualizar los formulario sector
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void updateFormSector()
        {
            log.trace("updateFormSector");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
               /* "_sectorId"
                parameters.Add("_nombre")
                    "_status",
                    "_usuarioModifacadorId",*/
                parameters.Add("_formularioSectorId", formularioSectorId, DbType.Int32);
                parameters.Add("_formularioSectorId", formularioSectorId, DbType.Int32);
                parameters.Add("_formularioSectorId", formularioSectorId, DbType.Int32);


                parameters = EjecutarSPOutPut("spu_updateFormSector", parameters);
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
        #endregion

        #region delete
        /// <summary>
        /// Metodo para eliminar el Formulario, (involucra formularios asociados al sector)
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void deleteFormSector()
        {
            log.trace("deleteFormSector");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_formularioSectorId", formularioSectorId, DbType.Int32);

                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spd_eliminarFormSector", parameters);
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
        #endregion
    }
}
