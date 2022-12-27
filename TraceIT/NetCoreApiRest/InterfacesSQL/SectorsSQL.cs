using Dapper;
using DWUtils;
using NetCoreApiRest.Utils;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base.Sectors;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Response;

namespace WSTraceIT.InterfacesSQL
{
    public class SectorsSQL : DBHelperDapper
    {
        #region propiedades
        public int sectorId;
        public string nombre;
        public string descripcionCorta;
        public int usuarioCreadorId;
        public int usuarioModificadorId;
        public DateTime fechaCreacion;
        public DateTime fechaModificacion;
        public int estatusId;
        private LoggerD4 log = new LoggerD4("SectorsSQL");


        #endregion

        #region Constructor
        public  SectorsSQL()
        {
            this.sectorId = 0;
            this.nombre = String.Empty;
            this.descripcionCorta = String.Empty;
            this.usuarioCreadorId = 0;
            this.usuarioModificadorId = 0;
            this.fechaCreacion = DateTime.Now;
            this.fechaModificacion = DateTime.Now;
            this.estatusId = 0;

        }
        #endregion

        #region Public method save y update
        /// <summary>
        /// Metodo para guardar los sectores
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void SaveUpdateSectors()
        {
            log.trace("saveSectors");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_nombre", nombre, DbType.String);
                parameters.Add("_descripcionCorta", descripcionCorta, DbType.String);
                parameters.Add("_usuarioCreadorId", usuarioCreadorId, DbType.Int32);
                
                parameters.Add("_estatusId", estatusId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spi_guardarSectores", parameters);
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

        #region Public method search
        /// <summary>
        /// Metodo para consultar el sector por id
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public searchSectorsByIdResponse searchSectorById()
        {
            log.trace("searchSectorById");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_sectorId", sectorId, DbType.Int32);

                searchSectorsByIdResponse resp = Consulta<searchSectorsByIdResponse>("spc_consultarSectoresById", parameters).FirstOrDefault();
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

        #region Public method update
        /// <summary>
        /// Metodo para actualizar los sectores
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void upadateSectors()
        {
            log.trace("upadateSectors");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_sectorId", sectorId, DbType.Int32);
                parameters.Add("_usuarioModificadorId", usuarioModificadorId, DbType.Int32);
                parameters.Add("_nombre", nombre, DbType.String);
                parameters.Add("_descripcionCorta", descripcionCorta, DbType.String);
                parameters.Add("_estatusId", estatusId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spu_editarSector", parameters);
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

        #region Public method delete
        /// <summary>
        /// Metodo para eliminar el sector, (involucra formularios asociados al sector)
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void deleteSectors()
        {
            log.trace("deleteSectors");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_sectorId", sectorId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spd_eliminarSector", parameters);
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

        #region Public method
        /// <summary>
        /// Metodo para consultar los acopios por compañia
        /// Desarrollador: Hernán Gómez
        /// </summary>
        public List<Sectors> searchSectors()
        {
            log.trace("searchListSectors");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_sectorId", sectorId, DbType.Int32);
                parameters.Add("_nombre", nombre, DbType.String);


                List<Sectors> resp = Consulta<Sectors>("spc_consultarSectores", parameters);
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
    }
}
