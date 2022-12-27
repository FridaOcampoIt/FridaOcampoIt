using Dapper;
using DWUtils;
using NetCoreApiRest.Utils;
using System.Data;
using System;
using System.Linq;
using WSTraceIT.Models.Response;
using WSTraceIT.Models.Base.CategoriesForms;

namespace WSTraceIT.InterfacesSQL
{
    public class CategoriesFormSQL: DBHelperDapper
    {
        #region propiedades
        public int categoriaFormularioId;
        public int formularioId;
        public string nombreCategoria;
        public int usuarioCreadorId;
        public DateTime fechaCreacion;
        public int usuarioModificadorId;
        public DateTime fechaModificacion;
        public int estatusCategoriaId;
        private LoggerD4 log = new LoggerD4("CategoriesFormSQL");


        #endregion

        #region Constructor
        public CategoriesFormSQL()
        { 
            this.categoriaFormularioId = 0;
            this.formularioId = 0;
            this.nombreCategoria = String.Empty;
            this.usuarioCreadorId = 0;
            this.fechaCreacion = DateTime.Now;
            this.usuarioModificadorId = 0;
            this.fechaModificacion = DateTime.Now;
            this.estatusCategoriaId = 0;

        }
        #endregion

        #region Public method save
        /// <summary>
        /// Metodo para guardar los categoria formulario
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void SaveCategoriesForm()
        {
            log.trace("SaveCategoriesForm");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_categoriaFormularioId", categoriaFormularioId, DbType.Int32);
                parameters.Add("_formularioId", formularioId, DbType.Int32);
                parameters.Add("_nombreCategoria", nombreCategoria, DbType.String);
                parameters.Add("_usuarioCreadorId", usuarioCreadorId, DbType.Int32);

                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spi_guardarCategoriesForm", parameters);
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
        /// Metodo para consultar el formulario por id
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public searchCategoriesFormsByIdResponse searchCategoriesFormById()
        {
            log.trace("searchCategoriesFormById");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_categoriaFormularioId", categoriaFormularioId, DbType.Int32);

                searchCategoriesFormsByIdResponse resp = Consulta<searchCategoriesFormsByIdResponse>("spc_consultaCategoriesFormById", parameters).FirstOrDefault();
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
        public void upadateCategoriesForm()

        {
            log.trace("upadateCategoriesForm");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters(); 

                parameters.Add("_categoriaFormularioId", categoriaFormularioId, DbType.Int32);
                parameters.Add("_nombreCategoria", nombreCategoria, DbType.String);
                parameters.Add("_usuarioModificadorId", usuarioModificadorId, DbType.Int32);
                parameters.Add("_fechaModificacion", fechaModificacion, DbType.DateTime);
                parameters.Add("_estatusCategoriaId", estatusCategoriaId, DbType.Int32);

                parameters = EjecutarSPOutPut("spu_updateCategoriesForm", parameters);
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
        /// Metodo para eliminar el Formulario, (involucra formularios asociados al sector)
        /// Desarrollador: Roberto Ortega
        /// </summary>
        public void deleteCategoriesForm()
        {
            log.trace("deleteCategoriesForm");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_categoriaFormularioId", categoriaFormularioId, DbType.Int32);

                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spd_eliminaCategoriesForm", parameters);
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
