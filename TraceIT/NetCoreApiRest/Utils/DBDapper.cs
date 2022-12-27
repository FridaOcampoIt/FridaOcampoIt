using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;
using NetCoreApiRest.Utils;

namespace DWUtils
{
    /// <summary>
    /// Clase de DB Helper utilizando Dapper
    /// Desarrollador: David Martinez
    /// </summary>
    public class DBHelperDapper
    {
        #region Variables utilizadas
        //Instancia de las conexiones y linkedServer existentes
        public static string[] conexion = new string[2];

        //Instancia para el uso de la transacción
        public IDbTransaction transaccion = null;

        //Instancia para el uso de la conexion
        public IDbConnection conection = null;

		//private LoggerD4 log = new LoggerD4("ProfilerController");

		//Tipo de coneccion que soporta el SQLClient
		public enum TipoConexion
		{
			SQL, Mysql
		}
		
        #endregion

        #region Metodos para la conexión
        /// <summary>
        /// Metodo para crear la conexion
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="conection"></param>
        public void CrearConexion(TipoConexion tipoConexion = 0, int conection = 0)
        {
			//log.info("db string: " + conexion[conection]);
			try
			{
				this.transaccion = null;
				if (tipoConexion == TipoConexion.SQL)
					this.conection = new SqlConnection(conexion[conection]);
				else if (tipoConexion == TipoConexion.Mysql){
					this.conection = new MySqlConnection(conexion[conection]);
				}
				else
					throw new Exception("Tipo de conexion no soportada");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        /// <summary>
        /// Metodo para abrir la conexion
        /// Desarrollador: David Martinez
        /// </summary>
        public void AbrirConexion()
        {
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                else if (conection.State == ConnectionState.Open)
                    throw new Exception("La conexión ya se encuentra abierta");

                conection.Open();
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }           
        }

        /// <summary>
        /// Metodo para cerrar la conexion
        /// Desarrollador: David Martinez
        /// </summary>
        public void CerrarConexion()
        {
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                else if (conection.State == ConnectionState.Closed)
                    throw new Exception("La conexión ya se encuentra cerrada");
                else if (conection.State != ConnectionState.Open)
                    throw new Exception("La conexión no se encuentra abierta");

                conection.Close();
                conection.Dispose();
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }            
        }

        /// <summary>
        /// Metodo para crear transaccion
        /// Desarrollador: David Martinez
        /// </summary>
        public void CrearTransaccion()
        {
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                if (transaccion != null)
                    throw new Exception("La conexión ya contiene una transacción creada");

                this.transaccion = conection.BeginTransaction();
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }            
        }

        /// <summary>
        /// Metodo para realizar y confirmar la transacción
        /// Desarrollador: David Martinez
        /// </summary>
        public void TransComit()
        {
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                if (transaccion == null)
                    throw new Exception("La conexión no contiene una transacción creada");

                transaccion.Commit();
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }            
        }

        /// <summary>
        /// Metodo para realizar el rollback de la transacción
        /// Desarrollador: David Martinez
        /// </summary>
        public void TransRollback()
        {
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                if (transaccion == null)
                    throw new Exception("La conexión no contiene una transacción creada");

                transaccion.Rollback();
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }
            
        }
        #endregion

        #region Metodos para ejecutar los SP
        /// <summary>
        /// Metodo para realizar una consulta llamando un SP y regresa un modelo
        /// Desarrollador: David Martinez
        /// </summary>
        /// <typeparam name="T">Modelo para mapear los datos que retorna el SP</typeparam>
        /// <param name="procedimiento">Nombre del Procedimiento almacenado</param>
        /// <param name="parametros">Listado de parametros que recibe el SP</param>
        /// <returns></returns>
        public List<T> Consulta<T>(string procedimiento, DynamicParameters parametros = null)
        {
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                else if (conection.State != ConnectionState.Open)
                    throw new Exception("La conexión no se encuentra abierta");

                return conection.Query<T>(new CommandDefinition(commandText: procedimiento, parameters: parametros, commandType: CommandType.StoredProcedure, transaction: transaccion, flags: CommandFlags.NoCache, commandTimeout: 120)).ToList();
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Metodo para ejecutar un SP que retorna mas de una tabla
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="procedimiento">Nombre del Procedimiento almacenado</param>
        /// <param name="modelos">Listado de modelos que retornará el SP</param>
        /// <param name="parametros">Listado de parametros que recibe el SP</param>
        /// <returns></returns>
        public List<IEnumerable<object>> ConsultaMultiple(string procedimiento, List<Type> modelos, DynamicParameters parametros = null)
        {
            List<IEnumerable<object>> resultado = new List<IEnumerable<object>>();
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                else if (conection.State != ConnectionState.Open)
                    throw new Exception("La conexión no se encuentra abierta");

                var reader = conection.QueryMultiple(new CommandDefinition(commandText: procedimiento, parameters: parametros, commandType: CommandType.StoredProcedure, transaction: transaccion, flags: CommandFlags.NoCache));

                foreach (Type t in modelos)
                    resultado.Add(reader.Read(t));

                return resultado;
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Metodo para ejecutar un sp que contiene parametros de tipo output
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="procedimiento">Nombre del Procedimiento almacenado</param>
        /// <param name="parametros">Listado de parametros que recibe el SP</param>
        /// <returns></returns>
        public DynamicParameters EjecutarSPOutPut(string procedimiento, DynamicParameters parametros)
        {
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                else if (conection.State != ConnectionState.Open)
                    throw new Exception("La conexión no se encuentra abierta");

                conection.Query(sql: procedimiento, param: parametros, commandType: CommandType.StoredProcedure, transaction: transaccion);
                return parametros;
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Metodo para ejecutar el procedimiento sobre un delete, un insert o un update
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="procedimiento">Nombre del Procedimiento almacenado</param>
        /// <param name="parametros">Listado de parametros que recibe el SP</param>
        public void EjecutarSP(string procedimiento, DynamicParameters parametros = null)
        {
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                else if (conection.State != ConnectionState.Open)
                    throw new Exception("La conexión no se encuentra abierta");

                conection.Query(sql: procedimiento, param: parametros, commandType: CommandType.StoredProcedure, transaction: transaccion);
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Metodo para ejecutar el query en comandos de texto
        /// Desarrollador: David Martinez
        /// </summary>
        /// <typeparam name="T">Modelo que retornará el query</typeparam>
        /// <param name="command">query que se ejecutará</param>
        /// <returns></returns>
        public List<T> ConsultaCommand<T>(string command)
        {
            try
            {
                if (conection == null)
                    throw new Exception("La conexión no se encuentra creada");
                else if (conection.State != ConnectionState.Open)
                    throw new Exception("La conexión no se encuentra abierta");

                return conection.Query<T>(new CommandDefinition(commandText: command, commandType: CommandType.Text, transaction: transaccion, flags: CommandFlags.NoCache)).ToList();
            }
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}