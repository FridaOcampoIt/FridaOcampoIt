using Dapper;
using DWUtils;
using NetCoreApiRest.Utils;
using System.Data;
using System;
using WSTraceIT.Models.Response;
using WSTraceIT.Models.Base.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSTraceIT.Models.Request;
using System.Linq;
using WSTraceIT.Models.Base.Sectors;
using System.Collections.Generic;
using WSTraceIT.Models.ModelsSQL;
using System.ComponentModel.Design;

namespace WSTraceIT.InterfacesSQL
{
    public class FormsSQL : DBHelperDapper
    {
        #region propiedades
        public int formularioId;
        public string nombre;
        public string descripcionCorta;
        public int usuarioCreadorId;
        public int usuarioModificadorId;
        public DateTime fechaCreacion;
        public DateTime fechaModificacion;
        public int estatusFormularioId;
        public int sectorId;
        private LoggerD4 log = new LoggerD4("FormsSQL");


        #endregion

        #region Constructor
        public FormsSQL()
        {
            this.formularioId = 0;
            this.nombre = String.Empty;
            this.descripcionCorta = String.Empty;
            this.usuarioCreadorId = 0;
            this.fechaCreacion = DateTime.Now;
            this.usuarioModificadorId = 0;
            this.fechaModificacion = DateTime.Now;
            this.estatusFormularioId = 0;
            this.sectorId = 0;


        }
        #endregion

        #region Public save
        /// <summary>
        /// Metodo para guardar los formularios
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void saveForms()
        {
            log.trace("saveForms");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("_nombre", nombre, DbType.String);
                parameters.Add("_descripcionCorta", descripcionCorta, DbType.String);
                parameters.Add("_usuarioCreadorId", usuarioCreadorId, DbType.Int32);

                parameters.Add("_estatusId", estatusFormularioId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spi_guardarFormulario", parameters);
                int response = parameters.Get<int>("_response");

                if (response == 0)
                    throw new Exception("Error al ejecutar el sp");
                if (response != 0)
                {
                    //insercion al agregar un nuevo formulario
                    string query = $"INSERT INTO rel_054_formulario_sector (FK_FormularioId, FK_SectorId) VALUES({response},{sectorId});";
                    ConsultaCommand<string>(query);
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
        #endregion 

        #region search
        /// <summary>
        /// Metodo para consultar el formulario sector por id
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public searchFormsByIdResponse searchFormsById()
        {
            log.trace("searchSectorById");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_formularioId", formularioId, DbType.Int64);

                searchFormsByIdResponse resp = Consulta<searchFormsByIdResponse>("spc_consultarFormulariosById", parameters).FirstOrDefault();
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
        /// Metodo para actualizar los sectores
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void updateForm()
        {
            log.trace("updateForm");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_formularioId", formularioId, DbType.Int32);
                parameters.Add("_usuarioModificadorId", usuarioModificadorId, DbType.Int32);
                parameters.Add("_nombre", nombre, DbType.String);
                parameters.Add("_descripcionCorta", descripcionCorta, DbType.String);;
                parameters.Add("_estatusFormularioId", estatusFormularioId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spu_editarFormulario", parameters);
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
        public void deleteForm()
        {
            log.trace("deleteForm");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_formularioId", formularioId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spd_eliminarFormulario", parameters);
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

        #region list forms
        /// <summary>
        /// Metodo para consultar los acopios por compañia
        /// Desarrollador: Hernán Gómez
        /// </summary>
        public List<Forms> searchforms()
        {
            log.trace("searchListForms");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
               
                parameters.Add("_sectorId", sectorId, DbType.String);

                List<Forms> resp = Consulta<Forms>("spc_consultarFormulario", parameters);
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
