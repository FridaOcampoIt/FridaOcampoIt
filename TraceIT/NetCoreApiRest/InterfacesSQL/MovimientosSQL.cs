using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base.Companies;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using Dapper;
using DWUtils;
using System.Data;
using WSTraceIT.Models.Base.Movimientos;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Response;
using WSTraceIT.Models.Request;

namespace WSTraceIT.InterfacesSQL
{
    public class MovimientosSQL : DBHelperDapper
    {
		#region Properties
		//Datos de busquedas
		public int movimientoId;
		public string movimientoId2;
		public string acopiosId;
		public string producto;
		public int tipoMovimiento;
		public string fechaIngresoDe;
		public string fechaIngresoHasta;
		public string fechaCaducidad;

		public int paisId;
		public int tipoInfoLegalId;
		

		//Datos generales edicion
		public string nombreAgrupacion;
		public int referenciaInterna;
		public int referenciaExterna;
		public int numeroPallet;
		public int numeroCajas;
		public int cantidad;
		public string fechaIngreso;
		public int tipoRecepcion;
		public string cosecheroGral;
		public string sectorGral;
		public DateTime fechaProduccion;
		public int productosRecibidos;
		public int nCajasRecibidas;
		public int nPalletsRecibidos;
		public int nProductosMerma;
		public int totalProductosQR;

		//Datos para las agrupaciones
		public int isGroup;
		public List<productosAgrupacion> productosAgrupacion;
		public int familiaProductoId;
		public int productoMovimientoId;

		//Datos para la lectura de pallets
		public int npallet;

		//Datos para saber si el movimiento aplica para agro o no
		public bool isAgro;

		//Datos para saber si el movimiento aplica para recibir sin enviar
		public bool isReciboEnvio;
		public bool isRecibe; // buscar por cajaId la recepcion sin envio

		//Dato para actualizar el nombre de un movimiento
		public bool isUpName;

		//Dato para actualizar el nombre de un movimiento
		public bool isPallet;

		//Dato para agregar compañia al remitente
		public int company;

		//Datos de remitente edicion
		public bool domicilioUpRem;
		public string nombreRemitente;
		public string apellidoRemitente;
		public string nombreCompaniaR;
		public string rzCompaniaR;
		public string telefonoR;
		public int paisR;
		public int estadoR;
		public string ciudadR;
		public string cpR;
		public string domicilioR;
		public string ranchoR;
		public string sectorR;

		//Datos de destinatario edicion
		public bool domicilioUp;
		public string nombreDestinatario;
		public string apellidoDestinatario;
		public string nombreCompaniaD;
		public string rzCompaniaD;
		public string telefonoD;
		public int paisD;
		public int estadoD;
		public string ciudadD;
		public string cpD;
		public string domicilioD;
		public string numeroC;

		//Datos de transportista edicion
		public bool infoUpTras;
		public string transportista;
		public string numReferencia;
		public string fechaEmbarque;

		//Datos de info legal edicion
		public int tipoInfo;
		public string nombreInfo;
		public string direccionInfo;
		public string contactoInfo;
		public string nombreInfoExp;
		public string direccionInfoExp;
		public string contactoInfoExp;

		//Datos de tabla existencia estimada
		public string nombreFamiliaCIU;
		public int ordenar;

		//Archivos Info Legal
		public int docId;
		public string doc;
		public string fecha;
		public int infoLegalId;
		public string nombreDoc;
		public int tipoArchivo;

		public int usuario;
		public string latitud;
		public string longitud;

		public string codigoQR;

		//Datos para registro de usuario
		public string nombreComp;
		public string razonSocial;
		public string correoComp;
		public string telefono;
		public string direccion;
		public string nombrePais;

		public string codigoPostal;
		public string ciudad;
		public string nombreEstado;

		public string email1;
		public string pass1;

		public string email2;
		public string pass2;

		public string email3;
		public string pass3;

		public string email4;
		public string pass4;

		public string email5;
		public string pass5;

		public int idFicha;

		public bool infoUpObs;
		public string observacion;

		public bool noEsnormal;
		public string codigoI;
		public string codigoF;
		public string codigoIHEXA;
		public string codigoFHEXA;
		public bool isHexa;
		public string codigoTipo;
		public string codigoCompleto;

		public string caja;
		//Consecutivo Caja para etiquetas Pallet o Caja
		public int consecutivoCaja;

		public int paisE;
		public string estadoE;
		public string rancho;
		public string sector;

		//Merma
		public string codigoId;
		public int merma;
		public int productoId;

		//Embalajes por Familia
		public string ids;
		public int embalajeId;

		//Acopios Información
		public int acopioId;
		public int productorId;

		private LoggerD4 log = new LoggerD4("MovimientosSQL");
		#endregion

		#region Constructor
		public MovimientosSQL()
		{
			//Datos de busquedas
			this.movimientoId = 0;
		
			this.producto = String.Empty;
			this.tipoMovimiento = 0;
			this.fechaCaducidad = String.Empty;
			this.fechaIngresoDe = String.Empty;
			this.fechaIngresoHasta = String.Empty;

			this.paisId = 0;
			this.tipoInfoLegalId = 0;

			//Datos generales edicion
			this.nombreAgrupacion = String.Empty;
			this.referenciaInterna = 0;
			this.referenciaExterna = 0;
			this.numeroPallet = 0;
			this.numeroCajas = 0;
			this.cantidad = 0;
			this.tipoRecepcion = 0;
			this.cosecheroGral = String.Empty;
			this.sectorGral = String.Empty;
			this.fechaProduccion = default(DateTime);
			this.productosRecibidos = 0;
			this.nCajasRecibidas = 0;
			this.nPalletsRecibidos = 0;
			this.nProductosMerma = 0;
			this.totalProductosQR = 0;

			//Datos para las agrupaciones
			this.isGroup = 0;
			this.productosAgrupacion = new List<productosAgrupacion>();
			this.familiaProductoId = 0;
			this.productoMovimientoId = 0;

			//Datos para la lectura de pallets
			this.npallet = 0;

			//Datos para saber si el movimiento aplica para agro o no
			this.isAgro = false;

			//Datos para saber si el movimiento aplica para recibir sin enviar
			this.isReciboEnvio = false;
			this.isRecibe = false; // buscar por cajaId la recepcion sin envio

			//Dato para actualizar el nombre de un movimiento
			this.isUpName = false;

			//Dato para actualizar el nombre de un movimiento
			this.isPallet = false;

			//Datos de remitente edicion
			this.domicilioUpRem = false;
			this.nombreRemitente = String.Empty;
			this.apellidoRemitente = String.Empty;
			this.nombreCompaniaR = String.Empty;
			this.rzCompaniaR = String.Empty;
			this.telefonoR = String.Empty;
			this.paisR = 0;
			this.estadoR = 0;
			this.ciudadR = String.Empty;
			this.cpR = String.Empty;
			this.domicilioR = String.Empty;
			this.ranchoR = String.Empty;
			this.sectorR = String.Empty;

			//Datos de destinatario edicion
			this.domicilioUp = false;
			this.nombreDestinatario = String.Empty;
			this.apellidoDestinatario = String.Empty;
			this.nombreCompaniaD = String.Empty;
			this.rzCompaniaD = String.Empty;
			this.telefonoD = String.Empty;
			this.paisD = 0;
			this.estadoD = 0;
			this.ciudadD = String.Empty;
			this.cpD = String.Empty;
			this.domicilioD = String.Empty;
			this.numeroC = String.Empty;

			//Datos de transportista edicion
			this.infoUpTras = false;
			this.transportista = String.Empty;
			this.numReferencia = String.Empty;
			this.fechaEmbarque = String.Empty;

			//Datos de info legal edicion
			this.tipoInfo = 0;
			this.nombreInfo = String.Empty;
			this.direccionInfo = String.Empty;
			this.contactoInfo = String.Empty;
			this.nombreInfoExp = String.Empty;
			this.direccionInfo = String.Empty;
			this.contactoInfo = String.Empty;

			//Datos de tabla existencia estimada
			this.nombreFamiliaCIU = String.Empty;
			this.ordenar = 0;

			//Archivos Info Legal
			this.docId = 0;
			this.doc = String.Empty;
			this.fecha = String.Empty;
			this.infoLegalId = 0;
			this.nombreDoc = String.Empty;
			this.tipoArchivo = 0;

			this.latitud = String.Empty;
			this.longitud = String.Empty;
			this.usuario = 0;

			this.codigoQR = String.Empty;

			//Datos para registro de usuario
			this.nombreComp = String.Empty;
			this.razonSocial = String.Empty;
			this.correoComp = String.Empty;
			this.telefono = String.Empty;
			this.direccion = String.Empty;
			this.nombrePais = String.Empty;

			this.codigoPostal = String.Empty;
			this.ciudad = String.Empty;
			this.nombreEstado = String.Empty;

			this.email1 = String.Empty;
			this.pass1 = String.Empty;

			this.email2 = String.Empty;
			this.pass2 = String.Empty;

			this.email3 = String.Empty;
			this.pass3 = String.Empty;

			this.email4 = String.Empty;
			this.pass4 = String.Empty;

			this.email5 = String.Empty;
			this.pass5 = String.Empty;

			this.idFicha = 0;

			this.infoUpObs = false;
			this.observacion = String.Empty;

			this.noEsnormal = false;
			this.codigoI = String.Empty;
			this.codigoF = String.Empty;
			this.codigoIHEXA = String.Empty;
			this.codigoFHEXA = String.Empty;
			this.isHexa = false;
			this.codigoTipo = String.Empty;
			this.codigoCompleto = String.Empty;

			this.caja = String.Empty;
			//Consecutivo Caja para etiquetas Pallet o Caja
			this.consecutivoCaja = 0;

			this.paisE = 0;
			this.estadoE = String.Empty;
			this.rancho = String.Empty;
			this.sector = String.Empty;

			//Merma
			this.codigoId = String.Empty;
			this.merma = 0;
			this.productoId = 0;

			//Embalajes por Familia
			this.ids = String.Empty;
			this.embalajeId = 0;
		}
        #endregion

        #region Public method

        #region Busqueda para tabla del catalogo movimientos
        /// <summary>
        ///	Metodo para consultar los movimientos
        ///	Desarrollador: Javier Ramirez
        /// </summary>
        /// <returns></returns>
        public List<MovimientosData> SearchMovimientos()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_Nombre", producto, DbType.String);
				parameters.Add("_Tipo", tipoMovimiento, DbType.String);
				parameters.Add("_FechaCaducidad", fechaCaducidad, DbType.String);
				parameters.Add("_FechaIngresoDe", fechaIngresoDe, DbType.String);
				parameters.Add("_FechaIngresoHasta", fechaIngresoHasta, DbType.String);
				parameters.Add("_Usuario", usuario, DbType.String);
				parameters.Add("_arrayacopio", acopiosId, DbType.String);

				List<MovimientosData> resp = Consulta<MovimientosData>("spc_consultaMovimientos", parameters);
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
		#endregion

		#region Busqueda para tabla del catalogo movimientos
		/// <summary>
		///	Metodo para consultar los movimientos
		///	Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<MovimientosData> SearchMovimientosByAcopioId()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_Nombre", producto, DbType.String); //Esto recibe un ID
				parameters.Add("_Tipo", tipoMovimiento, DbType.String); // Recibe un entero
				parameters.Add("_FechaIngresoDe", fechaIngresoDe, DbType.String);
				parameters.Add("_FechaIngresoHasta", fechaIngresoHasta, DbType.String);
				parameters.Add("_acopioId", acopioId, DbType.String);

				List<MovimientosData> resp = Consulta<MovimientosData>("spc_consultaMovimientosByAcopioId", parameters);
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
		#endregion

		#region Busqueda para tabla del catalogo movimientos
		/// <summary>
		///	Metodo para consultar los movimientos
		///	Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<MovimientosData> SearchMovimientosByCompaniaId()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_productId", producto, DbType.String); //Esto recibe un ID
				parameters.Add("_FechaIngresoDe", fechaIngresoDe, DbType.String);
				parameters.Add("_FechaIngresoHasta", fechaIngresoHasta, DbType.String);
				parameters.Add("_companiaId", company, DbType.String);

				List<MovimientosData> resp = Consulta<MovimientosData>("spc_consultaMovimientosByCompaniaId", parameters);
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
		#endregion

		#region Busqueda de los datos para los combos que servirán de filtros para el catalogo movimientos
		/// <summary>
		///	Metodo para consultar los combos de tipo de movimientos
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public TipoMovimientoDataCombo SearchCombosMovimiento()
		{
			log.trace("SearchCombosMovimiento");
			try
			{
				TipoMovimientoDataCombo resp = new TipoMovimientoDataCombo();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TraceITListDropDown));
				types.Add(typeof(TraceITListDropDown));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_tipoMovimiento", tipoMovimiento, DbType.Int32);

				var respSQL = ConsultaMultiple("spc_ConsultaCombosMovimientos", types, parameters);

				resp.tiposMovimientosDataComboList = respSQL[0].Cast<TraceITListDropDown>().ToList();
				resp.productosDataComboList = respSQL[1].Cast<TraceITListDropDown>().ToList();

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
		#endregion

		#region Busqueda de datos pertenecientes a un solo movimento (separados)
		/// <summary>
		///	Metodo para consultar los datos generales de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataGeneral SearchMovimientoGeneral()
		{
			log.trace("SearchMovimientoGeneral");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				MovimientosDataGeneral resp = Consulta<MovimientosDataGeneral>("spc_ConsultaDataMovimientoGeneral", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos generales de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataGeneral SearchMovimientoEtiqueta()
		{
			log.trace("SearchMovimientoEtiqueta");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_nombreAgru", nombreAgrupacion, DbType.String);
				parameters.Add("_referenciaInt", referenciaInterna, DbType.Int32);
				parameters.Add("_referenciaExt", referenciaExterna, DbType.Int32);

				MovimientosDataGeneral resp = Consulta<MovimientosDataGeneral>("spc_ConsultaDataEtiqueta", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos generales de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataGeneral SearchMovimientoGeneralRecep()
		{
			log.trace("SearchMovimientoGeneralRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.String);
				parameters.Add("_codigoI", codigoI, DbType.String);
				parameters.Add("_codigoF", codigoF, DbType.String);

				MovimientosDataGeneral resp = Consulta<MovimientosDataGeneral>("spc_ConsultaDataMovimientoGeneralRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos generales de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<MovimientosDataGeneral> SearchMovimientoGeneralRecep2()
		{
			log.trace("SearchMovimientoGeneralRecep2");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.String);
				parameters.Add("_codigoI", codigoI, DbType.String);
				parameters.Add("_codigoF", codigoF, DbType.String);
				parameters.Add("_isAgro", isAgro, DbType.Boolean);

				List<MovimientosDataGeneral> resp = Consulta<MovimientosDataGeneral>("spc_ConsultaDataMovimientoGeneralRecep", parameters);
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
		///	Metodo para consultar las cajas de un pallet
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<CajasPalletLst> SearchMovimientoGeneralCajasPallet()
		{
			log.trace("SearchMovimientoGeneralCajasPallet");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_ciu", codigoI, DbType.String);
				parameters.Add("_pallet", npallet, DbType.Int32);
				parameters.Add("_isAgro", isAgro, DbType.Boolean);

				List<CajasPalletLst> resp = Consulta<CajasPalletLst>("spc_ConsultaQRs", parameters);
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
		///	Web Method para buscar un movimiento por algún texto del codigo QR
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public string searchMovimientoByCode()
		{
			log.trace("SearchMovimientoGeneralCajasPallet");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.String);
				parameters.Add("_isAgro", isAgro, DbType.Boolean);
				parameters.Add("_isRecibe", isRecibe, DbType.Boolean);
				parameters.Add("_response", dbType: DbType.String, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spc_ConsultaMovimientoByCode", parameters);
				string response = parameters.Get<string>("_response"); // se obtiene el qr
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
		///	Metodo para consultar los embalajes por familia
		///	Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns>List</returns>
		public List<PackagingByFamily> SearchEmbalajesPorFamilia()
		{
			log.trace("SearchEmbalajesPorFamilia");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_familyId", ids, DbType.String);

				List<PackagingByFamily> resp = Consulta<PackagingByFamily>("spc_consultaEmbalajePorFamilia", parameters);
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
		///	Metodo para consultar los productos de un solo movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		public SearchDataMovimientoGeneralProdData SearchMovimientoGeneralProd()
		{
			log.trace("SearchMovimientoGeneralProd");
			try
			{
				SearchDataMovimientoGeneralProdData resp = new SearchDataMovimientoGeneralProdData();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TraceITMovimientosDataGeneralProd));
				//types.Add(typeof(TraceITMovimientosDataGeneralProd));
				//types.Add(typeof(TraceITMovimientosDataGeneralProd));
				//types.Add(typeof(TraceITMovimientosDataGeneralProd));

				//types.Add(typeof(TraceITMovimientosDataGeneralProd));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.String);
				parameters.Add("_codigoI", codigoI, DbType.String);
				parameters.Add("_codigoF", codigoF, DbType.String);
				parameters.Add("_codigoTipo", codigoTipo, DbType.String);
				parameters.Add("_codigoIHEXA", codigoIHEXA, DbType.String);
				parameters.Add("_codigoFHEXA", codigoFHEXA, DbType.String);
				parameters.Add("_totalProductosQR", totalProductosQR, DbType.Int32);
				parameters.Add("_isHexa", isHexa, DbType.Boolean);

				var respSQL = ConsultaMultiple("spc_ConsultaDataMovimientoGeneralProductos", types, parameters);

				resp.movimientosDataGeneralProdRecepList = respSQL[0].Cast<TraceITMovimientosDataGeneralProd>().ToList();
				//resp.movimientosDataGeneralDesProdRecepList = respSQL[1].Cast<TraceITMovimientosDataGeneralProd>().ToList();
				//resp.movimientosDataGeneralUnicoProdRecepList = respSQL[2].Cast<TraceITMovimientosDataGeneralProd>().ToList();
				//resp.movimientosDataGeneralOperaProdRecepList = respSQL[3].Cast<TraceITMovimientosDataGeneralProd>().ToList();

				//resp.movimientosDataGeneralReagrupadoProdRecepList = respSQL[4].Cast<TraceITMovimientosDataGeneralProd>().ToList();

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
		///	Metodo para consultar los datos de observaciones de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataObservacion SearchMovimientoObservaciones()
		{
			log.trace("SearchMovimientoObservaciones");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				MovimientosDataObservacion resp = Consulta<MovimientosDataObservacion>("spc_ConsultaDataMovimientoObservacion", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos de observaciones de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataObservacion SearchMovimientoObservacionesRecep()
		{
			log.trace("SearchMovimientoObservacionesRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.String);

				MovimientosDataObservacion resp = Consulta<MovimientosDataObservacion>("spc_ConsultaDataMovimientoObservacionRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos de observaciones de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataNombre SearchMovimientoNombreRecep()
		{
			log.trace("SearchMovimientoNombreRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoCompleto", codigoCompleto, DbType.String);

				MovimientosDataNombre resp = Consulta<MovimientosDataNombre>("spc_ConsultaDataMovimientoNombreRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar los productos de un solo movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public SearchDataMovimientoGeneralProdData SearchMovimientoGeneralProdRecep()
		{
			log.trace("SearchMovimientoGeneralProdRecep");
			try
			{
				SearchDataMovimientoGeneralProdData resp = new SearchDataMovimientoGeneralProdData();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TraceITMovimientosDataGeneralProd));
				types.Add(typeof(TraceITMovimientosDataGeneralProd));
				//types.Add(typeof(TraceITMovimientosDataGeneralProd));
				//types.Add(typeof(TraceITMovimientosDataGeneralProd));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.String);
				parameters.Add("_codigoI", codigoI, DbType.String);
				parameters.Add("_codigoF", codigoF, DbType.String);
				parameters.Add("_codigoIHEXA", codigoIHEXA, DbType.String);
				parameters.Add("_codigoFHEXA", codigoFHEXA, DbType.String);
				parameters.Add("_isHexa", isHexa, DbType.Boolean);
				parameters.Add("_familiaProductoId", familiaProductoId, DbType.Int32);
				parameters.Add("_productoMovimientoId", productoMovimientoId, DbType.Int32);
				parameters.Add("_isAgro", isAgro, DbType.Boolean);

				var respSQL = Consulta<TraceITMovimientosDataGeneralProd>("spc_ConsultaDataMovimientoGeneralProductosRecep", parameters);

				resp.movimientosDataGeneralProdRecepList = respSQL;
				//resp.movimientosDataGeneralProdRecepList = respSQL[0].Cast<TraceITMovimientosDataGeneralProd>().ToList();
				//resp.movimientosDataGeneralDesProdRecepList = respSQL[1].Cast<TraceITMovimientosDataGeneralProd>().ToList();
				//resp.movimientosDataGeneralUnicoProdRecepList = respSQL[2].Cast<TraceITMovimientosDataGeneralProd>().ToList();
				//resp.movimientosDataGeneralOperaProdRecepList = respSQL[3].Cast<TraceITMovimientosDataGeneralProd>().ToList();

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
		///	Metodo para consultar los datos remitente de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataRemitente SearchMovimientoRemitente()
		{
			log.trace("SearchMovimientoRemitente");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.String);
				parameters.Add("_userId", usuario, DbType.String);
				parameters.Add("_company", company, DbType.String);
				Console.WriteLine("Parameters", parameters);
				MovimientosDataRemitente resp = Consulta<MovimientosDataRemitente>("spc_ConsultaDataMovimientoRemitente", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos remitente de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataRemitente SearchMovimientoRemitenteRecep()
		{
			log.trace("SearchMovimientoRemitenteRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.String);

				MovimientosDataRemitente resp = Consulta<MovimientosDataRemitente>("spc_ConsultaDataMovimientoRemitenteRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos destinatario de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataDestinatario SearchMovimientoDestinatario()
		{
			log.trace("SearchMovimientoDestinatario");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.String);
				parameters.Add("_numeroC", numeroC, DbType.String);
				parameters.Add("_usuarioId", usuario, DbType.Int32);

				MovimientosDataDestinatario resp = Consulta<MovimientosDataDestinatario>("spc_ConsultaDataMovimientoDestinatario", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos destinatario de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataDestinatario SearchMovimientoDestinatarioRecep()
		{
			log.trace("SearchMovimientoDestinatarioRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.String);

				MovimientosDataDestinatario resp = Consulta<MovimientosDataDestinatario>("spc_ConsultaDataMovimientoDestinatarioRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos transportista de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataTransportista SearchMovimientoTransportista()
		{
			log.trace("SearchMovimientoTransportista");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.String);

				MovimientosDataTransportista resp = Consulta<MovimientosDataTransportista>("spc_ConsultaDataMovimientoTransportista", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos transportista de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataTransportista SearchMovimientoTransportistaRecep()
		{
			log.trace("SearchMovimientoTransportistaRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.String);

				MovimientosDataTransportista resp = Consulta<MovimientosDataTransportista>("spc_ConsultaDataMovimientoTransportistaRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos info legal de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataInfoLegal SearchMovimientoInfoLegal()
		{
			log.trace("SearchMovimientoInfoLegal");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.String);

				MovimientosDataInfoLegal resp = Consulta<MovimientosDataInfoLegal>("spc_ConsultaDataMovimientoInfoLegal", parameters).FirstOrDefault();
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
		///	Metodo para consultar los datos info legal de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataInfoLegal SearchMovimientoInfoLegalRecep()
		{
			log.trace("SearchMovimientoInfoLegalRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.String);

				MovimientosDataInfoLegal resp = Consulta<MovimientosDataInfoLegal>("spc_ConsultaDataMovimientoInfoLegalRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar la merma de un solo movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		public SearchDataMovimientoMermaData SearchMovimientoMerma()
		{
			log.trace("SearchMovimientoMerma");
			try
			{
				SearchDataMovimientoMermaData resp = new SearchDataMovimientoMermaData();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TraceITMovimientosDataMerma));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_CodigoID", codigoId, DbType.String);

				var respSQL = ConsultaMultiple("spc_ConsultaDataMovimientoMerma", types, parameters);

				resp.movimientosDataMermaList = respSQL[0].Cast<TraceITMovimientosDataMerma>().ToList();

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
		///	Metodo para consultar los datos de la etiqueta de un movimiento
		///	Desarrollador: Ivan Gutierrez
		/// </summary>
		/// <returns></returns>
		public MovimientosDataInfoLabel SearchMovimientoInfoEtiqueta()
		{
			log.trace("SearchMovimientoInfoEtiqueta");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.String);

				MovimientosDataInfoLabel resp = Consulta<MovimientosDataInfoLabel>("spc_ConsultaDataMovimientoInfoEtiqueta", parameters).FirstOrDefault();
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
		#endregion

		#region Busqueda de los datos para los combos de paises y estados
		/// <summary>
		///	Metodo para consultar los combos de pais y estado
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public PaisEstadoDataCombo SearchCombosPaisEstado()
		{
			log.trace("SearchCombosPaisEstado");
			try
			{
				PaisEstadoDataCombo resp = new PaisEstadoDataCombo();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TraceITListDropDown));
				types.Add(typeof(TraceITListDropDown));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_paisId", paisId, DbType.Int32);

				var respSQL = ConsultaMultiple("spc_ConsultaCombosPaises", types, parameters);

				resp.paisesDataComboList = respSQL[0].Cast<TraceITListDropDown>().ToList();
				resp.estadosDataComboList = respSQL[1].Cast<TraceITListDropDown>().ToList();

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
		#endregion

		#region Busqueda de los datos para los radio button de tipo de infoLegal
		/// <summary>
		///	Metodo para consultar los combos de pais y estado
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public InfoLegalTipoRadioB SearchRadioBInfoLegal()
		{
			log.trace("SearchRadioBInfoLegal");
			try
			{
				InfoLegalTipoRadioB resp = new InfoLegalTipoRadioB();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TraceITListDropDown));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_tipoInfoLegalId", tipoInfoLegalId, DbType.Int32);

				var respSQL = ConsultaMultiple("spc_ConsultaRadioTipoInfo", types, parameters);

				resp.tipoInfoRadioList = respSQL[0].Cast<TraceITListDropDown>().ToList();

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
		#endregion

		#region Edición de datos pertenecientes a un solo movimiento (separados)

		/// <summary>
		/// Metodo para actualizar los datos generales
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void UpdateMovimientoGeneral()
		{
			log.trace("UpdateMovimientoGeneral");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);
				parameters.Add("_NombreAgrupacion", nombreAgrupacion, DbType.String);
				parameters.Add("_ReferenciaInterna", referenciaInterna, DbType.String);
				parameters.Add("_ReferenciaExterna", referenciaExterna, DbType.String);
				parameters.Add("_NumeroPallet", numeroPallet, DbType.String);
				parameters.Add("_NumeroCajas", numeroCajas, DbType.String);
				parameters.Add("_Producto", producto, DbType.String);
				parameters.Add("_Cantidad", cantidad, DbType.String);
				parameters.Add("_TipoMovimiento", tipoMovimiento, DbType.Boolean);
				parameters.Add("_FechaIngreso", fechaIngreso, DbType.String);
				parameters.Add("_FechaCaducidad", fechaCaducidad, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionDataMovimientoGeneral", parameters);
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
		/// Metodo para actualizar los datos de observacion
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void UpdateMovimientoObservacion()
		{
			log.trace("UpdateMovimientoObservacion");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);
				parameters.Add("_Observacion", observacion, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionDataMovimientoObservacion", parameters);
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
		/// Metodo para actualizar los datos de observacion
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void UpdateMovimientoNombre()
		{
			log.trace("UpdateMovimientoNombre");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoCompleto", codigoCompleto, DbType.Int32);
				parameters.Add("_nombre", nombreAgrupacion, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionDataMovimientoNombreRecep", parameters);
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
		/// Metodo para actualizar los datos de remitente
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void UpdateMovimientoRemitente()
		{
			log.trace("UpdateMovimientoRemitente");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);
				parameters.Add("_NombreRemitente", nombreRemitente, DbType.String);
				parameters.Add("_ApellidoRemitente", apellidoRemitente, DbType.String);
				parameters.Add("_NombreCompaniaR", nombreCompaniaR, DbType.String);
				parameters.Add("_RzCompaniaR", rzCompaniaR, DbType.String);
				parameters.Add("_TelefonoR", telefonoR, DbType.String);
				parameters.Add("_PaisR", paisR, DbType.String);
				parameters.Add("_EstadoR", estadoR, DbType.String);
				parameters.Add("_CiudadR", ciudadR, DbType.String);
				parameters.Add("_CpR", cpR, DbType.String);
				parameters.Add("_DomicilioR", domicilioR, DbType.String);
				parameters.Add("_RanchoR", rancho, DbType.String);
				parameters.Add("_SectorR", sector, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionDataMovimientoRemitente", parameters);
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
		/// Metodo para actualizar los datos de destinatario
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void UpdateMovimientoDestinatario()
		{
			log.trace("UpdateMovimientoDestinatario");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);
				parameters.Add("_NombreDestinatario", nombreDestinatario, DbType.String);
				parameters.Add("_ApellidoDestinatario", apellidoDestinatario, DbType.String);
				parameters.Add("_NombreCompaniaD", nombreCompaniaD, DbType.String);
				parameters.Add("_RzCompaniaD", rzCompaniaD, DbType.String);
				parameters.Add("_TelefonoD", telefonoD, DbType.String);
				parameters.Add("_PaisD", paisD, DbType.String);
				parameters.Add("_EstadoD", estadoD, DbType.String);
				parameters.Add("_CiudadD", ciudadD, DbType.String);
				parameters.Add("_CpD", cpD, DbType.String);
				parameters.Add("_DomicilioD", domicilioD, DbType.String);
				parameters.Add("_NumeroC", numeroC, DbType.String);
				parameters.Add("_RanchoD", rancho, DbType.String);
				parameters.Add("_SectorD", sector, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionDataMovimientoDestinatario", parameters);
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
		/// Metodo para actualizar los datos de transportista
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void UpdateMovimientoTransportista()
		{
			log.trace("UpdateMovimientoTransportista");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);
				parameters.Add("_Transportista", transportista, DbType.String);
				parameters.Add("_NumReferencia", numReferencia, DbType.String);
				parameters.Add("_FechaEmbarque", fechaEmbarque, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionDataMovimientoTransportista", parameters);
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
		/// Metodo para actualizar los datos de transportista
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void UpdateMovimientoInfoLegal()
		{
			log.trace("UpdateMovimientoInfoLegal");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);
				parameters.Add("_TipoInfo", tipoInfo, DbType.String);
				parameters.Add("_NombreInfo", nombreInfo, DbType.String);
				parameters.Add("_DireccionInfo", direccionInfo, DbType.String);
				parameters.Add("_ContactoInfo", contactoInfo, DbType.String);
				parameters.Add("_NombreInfoExp", nombreInfoExp, DbType.String);
				parameters.Add("_DireccionInfoExp", direccionInfoExp, DbType.String);
				parameters.Add("_ContactoInfoExp", contactoInfoExp, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionDataMovimientoInfoLegal", parameters);
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

		#region Edición de merma de un movimiento
		/// <summary>
		/// Metodo para actualizar los datos de transportista
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void UpdateMovimientoMerma()
		{
			log.trace("UpdateMovimientoMerma");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_CodigoID", codigoId, DbType.String);
				parameters.Add("_Merma", merma, DbType.String);
				parameters.Add("_ProductoMovimientoId", productoId, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionDataMovimientoMerma", parameters);
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

		#region Busqueda para tabla de documentos de info legal
		/// <summary>
		///	Metodo para consultar los documentos
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<DocsInfoLegalData> SearchDocsInfoLegal()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_MovimientosId", movimientoId, DbType.String);


				List<DocsInfoLegalData> resp = Consulta<DocsInfoLegalData>("spc_consultaDocumentosInfoLegal", parameters);
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
		#endregion

		#region Busqueda para tabla de existencia estimada
		/// <summary>
		///	Metodo para consultar los movimientos en existencia estimada
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<ExistenciaEstimadaData> SearchExistenciaEstimada()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_NombreFamiliaCIU", nombreFamiliaCIU, DbType.String);
				parameters.Add("_FechaCaducidad", fechaCaducidad, DbType.String);
				parameters.Add("_FechaIngresoDe", fechaIngresoDe, DbType.String);
				parameters.Add("_FechaIngresoHasta", fechaIngresoHasta, DbType.String);
				parameters.Add("_Ordenar", ordenar, DbType.Int32);
				parameters.Add("_Usuario", usuario, DbType.String);


				List<ExistenciaEstimadaData> resp = Consulta<ExistenciaEstimadaData>("spc_consultaExistenciaEstimada", parameters);

				// Recepciones
				List<ExistenciaEstimadaData> recepciones = resp.Where(x => x.TipoMovimiento == 3).ToList();
				// Envios
				List<ExistenciaEstimadaData> envios = resp.Where(x => x.TipoMovimiento == 2).ToList();

				// Unir la cantidad de Envios
                foreach(ExistenciaEstimadaData recep in recepciones) {
					foreach(ExistenciaEstimadaData env in envios) {
						if (env.producto == recep.producto)
							recep.cantidadEnvio += env.cantidadEnvio;
					}
                }

				// Ordenar por Fecha
				recepciones = recepciones.OrderByDescending(x => x.fechaIngreso).ToList();

				CerrarConexion();

				return recepciones;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}
		#endregion

		#region Guardar docs info legal
		/// <summary>
		/// Metodo para guardar documentos de Información Legal
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void SaveDocsInfo()
		{
			log.trace("SaveDocsInfo");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_doc", doc.Trim(), DbType.String);
				parameters.Add("_fecha", fecha, DbType.String);
				parameters.Add("_infoLegalId", infoLegalId, DbType.Int32);
				parameters.Add("_nombreDoc", nombreDoc.Trim(), DbType.String);
				parameters.Add("_tipoArchivo", tipoArchivo, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_GuardarDocsInfoLegal", parameters);
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
		/// Metodo para eliminar documentos de Información Legal
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void DeleteDocsInfo()
		{
			log.trace("SaveDocsInfo");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_archivoId", docId, DbType.Int32);
				parameters.Add("_infoLegalId", infoLegalId, DbType.Int32);
				parameters.Add("_nombreDoc", nombreDoc.Trim(), DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_EliminarDocsInfoLegal", parameters);
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

		#region Guardar movimiento
		/// <summary>
		/// Metodo para guardar los datos de la compañia 
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void SaveMovimiento()
		{
			log.trace("SaveMovimiento");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_NombreAgru", nombreAgrupacion, DbType.String);
				parameters.Add("_FechaIngreso", fechaIngreso, DbType.String);
				parameters.Add("_ReferenciaInt", referenciaInterna, DbType.String);
				parameters.Add("_ReferenciaExt", referenciaExterna, DbType.String);
				parameters.Add("_Usuario", usuario, DbType.String);
				parameters.Add("_Latitud", latitud, DbType.String);
				parameters.Add("_Longitud", longitud, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_GuardarMovimiento", parameters);
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

		#region Edición de tipo de movimiento de agrupación a envío
		/// <summary>
		/// Metodo para actualizar tipo de movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public int UpdateAgruAEnvio()
		{
			log.trace("UpdateAgruAEnvio");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);
				parameters.Add("_codigoQR", codigoCompleto, DbType.String);
				parameters.Add("_codigoI", codigoI, DbType.String);
				parameters.Add("_codigoF", codigoF, DbType.String);
				parameters.Add("_fecha", fechaIngreso, DbType.String);
				parameters.Add("_cosechero", cosecheroGral, DbType.String);
				parameters.Add("_sector", sectorGral, DbType.String);
				parameters.Add("_numeroC", numeroC, DbType.String);
				parameters.Add("_totalProductosQR", totalProductosQR, DbType.Int32);
				if(isPallet)
					parameters.Add("_nCajasRecibidas", nCajasRecibidas, DbType.Int32);
				parameters.Add("_isGroup", isGroup, DbType.Int32);
				parameters.Add("_latitud", latitud, DbType.String);
				parameters.Add("_longitud", longitud, DbType.String);
				parameters.Add("_referenciaInterna", referenciaInterna, DbType.Int32);
				parameters.Add("_referenciaExterna", referenciaExterna, DbType.Int32);
				// Datos para el domicilio del remitente en caso de que se registre manual
				parameters.Add("_domicilioUpRem", domicilioUpRem, DbType.Boolean);
				parameters.Add("_nombreRemitente", nombreRemitente, DbType.String);
				parameters.Add("_apellidoRemitente", apellidoRemitente, DbType.String);
				parameters.Add("_nombreCompaniaR", nombreCompaniaR, DbType.String);
				parameters.Add("_rzCompaniaR", rzCompaniaR, DbType.String);
				parameters.Add("_telefonoR", telefonoR, DbType.String);
				parameters.Add("_paisR", paisR, DbType.String);
				parameters.Add("_estadoR", estadoR, DbType.String);
				parameters.Add("_ciudadR", ciudadR, DbType.String);
				parameters.Add("_cpR", cpR, DbType.String);
				parameters.Add("_domicilioR", domicilioR, DbType.String);
				parameters.Add("_ranchoR", ranchoR, DbType.String);
				parameters.Add("_sectorR", sectorR, DbType.String);
				// Datos para el domicilio destinatario en caso de que se registre manual
				parameters.Add("_domicilioUp", domicilioUp, DbType.Boolean);
				parameters.Add("_paisD", paisD, DbType.Int32);
				parameters.Add("_estadoD", estadoD, DbType.Int32);
				parameters.Add("_ciudadD", ciudadD, DbType.String);
				parameters.Add("_cpD", cpD, DbType.String);
				parameters.Add("_domicilioD", domicilioD, DbType.String);
				parameters.Add("_nombreDestinatario", nombreDestinatario, DbType.String);
				parameters.Add("_apellidoDestinatario", apellidoDestinatario, DbType.String);
				parameters.Add("_nombreCompaniaD", nombreCompaniaD, DbType.String);
				parameters.Add("_rzCompaniaD", rzCompaniaD, DbType.String);
				parameters.Add("_telefonoD", telefonoD, DbType.String);
				parameters.Add("_rancho", rancho, DbType.String);
				parameters.Add("_sector", sector, DbType.String);
				// Datos para transportista en caso de que se registre manual
				parameters.Add("_infoUpTras", infoUpTras, DbType.Boolean);
				parameters.Add("_transportista", transportista, DbType.String);
				parameters.Add("_numReferencia", numReferencia, DbType.String);
				parameters.Add("_fechaEmbarque", fechaEmbarque, DbType.String);
				// Datos para observaciones en caso de que se registre manual
				parameters.Add("_infoUpObs", infoUpObs, DbType.Boolean);
				parameters.Add("_observacion", observacion, DbType.String);

				parameters.Add("_UsuarioId", usuario, DbType.Int32);
				parameters.Add("_isAgro", isAgro, DbType.Boolean);
				parameters.Add("_isReciboEnvio", isReciboEnvio, DbType.Boolean);

				//Acopio y Productor 
				parameters.Add("_acopioId", acopioId, DbType.Int32);
				parameters.Add("_productorId", productorId, DbType.Int32);

				// Datos para guardar el embalaje reproceso cuando es envío de una caja
				if(!isPallet)
					parameters.Add("_embalajeId", embalajeId, DbType.Int32);
				
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(isPallet ? "spu_EdicionAgruAEnvioPallet" : "spu_EdicionAgruAEnvio", parameters);
				int response = parameters.Get<int>("_response"); // se obtiene el id del movimiento

				if(!isPallet) {
					int responseProds = 0;
					if(isGroup == 1 && movimientoId == 0 && response >= 1) {
						//Guardar cada qr en la agrupación
						foreach (var caja in productosAgrupacion) {
							DynamicParameters boxParameters = new DynamicParameters();
							boxParameters.Add("_movimientosId", response, System.Data.DbType.Int32);
							boxParameters.Add("_codigoQR", caja.codigoQR, System.Data.DbType.String);
							boxParameters.Add("_codigoI", caja.codigoI, System.Data.DbType.String);
							boxParameters.Add("_codigoF", caja.codigoF, System.Data.DbType.String);
							boxParameters.Add("_cantidad", caja.cantidad, System.Data.DbType.String);
							boxParameters.Add("_familiaProductoId", caja.familiaProductoId, System.Data.DbType.Int32);
							boxParameters.Add("_embalajeId", caja.embalajeId, System.Data.DbType.Int32);
							boxParameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
							boxParameters = EjecutarSPOutPut("spi_guardarProductosAgrupacion", boxParameters);
							responseProds = boxParameters.Get<int>("_response");
						}
					}
                }
				
				TransComit();
				CerrarConexion();

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
				else
					return response;
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
		/// Metodo para obtener el Id del envío de un movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public int getEnvioId(string movId)
		{
			log.trace("getEnvioId");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				int response = 0;

				response = ConsultaCommand<int>($"SELECT IFNULL(MovimientoParcialId,MovimientosId) FROM cat_027_movimientos WHERE MovimientosId = {movId};").FirstOrDefault();

				CerrarConexion();

				return response;
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

		#region Edición de tipo de movimiento de envío a recepción
		/// <summary>
		/// Metodo para actualizar tipo de movimiento
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void UpdateEnvioARecep()
		{
			log.trace("UpdateEnvioARecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);
				parameters.Add("_usuario", usuario, DbType.Int32);
				parameters.Add("_fecha", fechaIngreso, DbType.String);
				parameters.Add("_noNormal", noEsnormal, DbType.Boolean);
				parameters.Add("_codigoI", codigoI, DbType.String);
				parameters.Add("_codigoF", codigoF, DbType.String);
				parameters.Add("_codigoTipo", codigoTipo, DbType.String);
				parameters.Add("_Latitud", latitud, DbType.String);
				parameters.Add("_Longitud", longitud, DbType.String);
				parameters.Add("_codigoCompleto", codigoCompleto, DbType.String);
				parameters.Add("_nombre", nombreAgrupacion, DbType.String);
				parameters.Add("_caja", caja, DbType.String);
				parameters.Add("_codigoIHEXA", codigoIHEXA, DbType.String);
				parameters.Add("_codigoFHEXA", codigoFHEXA, DbType.String);
				parameters.Add("_isHexa", isHexa, DbType.Boolean);
				parameters.Add("_tipoRecepcion", tipoRecepcion, DbType.Int32);
				parameters.Add("_cosecheroGral", cosecheroGral, DbType.String);
				parameters.Add("_sectorGral", sectorGral, DbType.String);
				parameters.Add("_fechaProduccion", fechaProduccion, DbType.DateTime);
				parameters.Add("_productosRecibidos", productosRecibidos, DbType.Int32);
				parameters.Add("_nCajasRecibidas", nCajasRecibidas, DbType.Int32);
				parameters.Add("_nPalletsRecibidos", nPalletsRecibidos, DbType.Int32);
				parameters.Add("_nProductosMerma", nProductosMerma, DbType.Int32);
				parameters.Add("_productoMovimientoId", productoMovimientoId, DbType.Int32);
				parameters.Add("_isAgro", isAgro, DbType.Boolean);
				parameters.Add("_isUpName", isUpName, DbType.Boolean);
				parameters.Add("_isIDRecibo", (isReciboEnvio ? 1 : 0), System.Data.DbType.Int32);
				parameters.Add("_acopioId", acopioId, DbType.Int32);
				parameters.Add("_productorId", productorId, DbType.Int32);
				parameters.Add("_recepcionStock", true, DbType.Boolean);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_EdicionEnvioARecepcion", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
				if (response == -2)
					throw new Exception("El nombre ingresado ya se encuentra en otro movimiento");

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

		#region Busqueda y Actualiza el Consecutivo de la etiqueta al imprimir por Pallet o Caja
		/// <summary>
		///	Metodo para obtener el Consecutivo de la etiqueta actual por Familia
		///	Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns>Int</returns>
		public int SearchConsecutiveByFamily(int isFamily = 0)
		{
			log.trace("SearchConsecutiveByFamily");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				if(isFamily > 0)
					CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientoId", movimientoId, DbType.Int32);
				parameters.Add("_productoId", productoId, DbType.Int32);
				parameters.Add("_isFamily", isFamily, DbType.Int32);
				
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
				parameters = EjecutarSPOutPut("spc_consultaConsecutivoPorFamilia", parameters);
				if(isFamily > 0)
					TransComit();
				CerrarConexion();
				
				int response = parameters.Get<int>("_response");
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
		///	Metodo para actualizar el Consecutivo de la etiqueta por Familia
		///	Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public void SaveConsecutiveByFamily()
		{
			log.trace("SaveConsecutiveByFamily");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientoId", movimientoId, DbType.Int32);
				parameters.Add("_productoId", productoId, DbType.Int32);
				parameters.Add("_consecutivo", consecutivoCaja, DbType.Int32);

				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
				parameters = EjecutarSPOutPut("spi_GuardarConsecutivoPorFamilia", parameters);

				TransComit();
				CerrarConexion();

				int response = parameters.Get<int>("_response");
				
				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		///	Metodo para consultar la información de los productos (familias) y las cajas de un movimiento movimiento
		///	Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns>List</returns>
		public SearchBoxesInfoByMovResponse SearchBoxesInfoByMovimiento()
		{
			log.trace("SearchBoxesInfoByMovimiento");
			try
			{
				SearchBoxesInfoByMovResponse resp = new SearchBoxesInfoByMovResponse();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(FamilyTypeInfoMov));
				types.Add(typeof(BoxInfoMov));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				var respSQL = ConsultaMultiple("spc_ConsultaCajasMovimientoPorFamilia", types, parameters);

				resp.familyInfo = respSQL[0].Cast<FamilyTypeInfoMov>().ToList();
				resp.boxesInfo = respSQL[1].Cast<BoxInfoMov>().ToList();

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
		#endregion

		#region Guardar USUARIO
		/// <summary>
		/// Metodo para guardar los datos del usuario registrado
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void SaveUsuario()
		{
			log.trace("SaveUsuario");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_NombreComp", nombreComp, DbType.String);
				parameters.Add("_RazonSocial", razonSocial, DbType.String);
				parameters.Add("_CorreoComp", correoComp, DbType.String);
				parameters.Add("_Telefono", telefono, DbType.String);
				parameters.Add("_Direccion", direccion, DbType.String);
				parameters.Add("_NombrePais", nombrePais, DbType.String);

				parameters.Add("_CodigoPostal", codigoPostal, DbType.String);
				parameters.Add("_Ciudad", ciudad, DbType.String);
				parameters.Add("_NombreEstado", nombreEstado, DbType.String);

				parameters.Add("_Email1", email1, DbType.String);
				parameters.Add("_Password1", pass1, DbType.String);

				parameters.Add("_Email2", email2, DbType.String);
				parameters.Add("_Password2", pass2, DbType.String);

				parameters.Add("_Email3", email3, DbType.String);
				parameters.Add("_Password3", pass3, DbType.String);

				parameters.Add("_Email4", email4, DbType.String);
				parameters.Add("_Password4", pass4, DbType.String);

				parameters.Add("_Email5", email5, DbType.String);
				parameters.Add("_Password5", pass5, DbType.String);

				parameters.Add("_Usuario", usuario, DbType.String);

				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_GuardarUsuarioAgrupaciones", parameters);
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
		/// Metodo para guardar los datos de usuario registrado random como invitado
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void SaveUsuarioInvitado()
		{
			log.trace("SaveUsuarioInvitado");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				bool bandera = true;
				while (bandera)
				{
                    if (email1.Equals(""))
                    {
						email1 = GenerateEmail.Email();
					}
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("_Email", email1, DbType.String);
					parameters.Add("_Password", pass1, DbType.String);

					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_GuardarUsuarioAgrupacionesInvitado", parameters);
					int response = parameters.Get<int>("_response");

					if (response == 0)
						throw new Exception("Error al ejecutar el sp");
					else if (response == -1)
						bandera = true;
					else
					{
						bandera = false;
					}
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
		///	Metodo para consultar los datos del invitado recien creados
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public usuarioInvitadoData searchDataInvitado()
		{
			log.trace("SearchDocDetalleTotalProd");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_email", email1, DbType.Int32);

				usuarioInvitadoData resp = Consulta<usuarioInvitadoData>("spc_ConsultaDataInvitado", parameters).FirstOrDefault();
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
		/// Metodo para generar enveto y despues de 10 hrs se borren los registros
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void createEventDeleteInvitado()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();
				string query = " CREATE EVENT invitado_" + usuario+ "_event ON SCHEDULE AT CURRENT_TIMESTAMP + INTERVAL 5 MINUTE DO call traceit_v2.spd_EliminarUsuarioInvitado("+usuario+");";

				query = query.TrimEnd(',');
				log.debug(query);
				ConsultaCommand<string>(query);

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

		#region Búsqueda de Info para Doc Detalle de un movimiento (separados)
		/// <summary>
		///	Metodo para consultar los datos generales de los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<DocDetalleProductoData> SearchDocDetalleProducto()
		{
			log.trace("SearchDocDetalleProducto");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				List<DocDetalleProductoData> resp = Consulta<DocDetalleProductoData>("spc_ConsultaDocDetalleProductos", parameters);
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
		///	Metodo para consultar los datos generales de los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<DocDetalleProductoData> SearchDocDetalleProductoIndi()
		{
			log.trace("SearchDocDetalleProductoIndi");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId2, DbType.String);

				List<DocDetalleProductoData> resp = Consulta<DocDetalleProductoData>("spc_ConsultaDocDetalleProductosIndi", parameters);
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
		///	Metodo para consultar los datos generales de los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<DocDetalleProductoData> SearchDocDetalleProductoCajas()
		{
			log.trace("SearchDocDetalleProductoCajas");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId2, DbType.String);
				parameters.Add("_tipo", codigoTipo, DbType.String);
				parameters.Add("_codigoI", codigoI, DbType.String);
				parameters.Add("_codigoF", codigoF, DbType.String);

				List<DocDetalleProductoData> resp = Consulta<DocDetalleProductoData>("spc_ConsultaDocDetalleProductosCajas", parameters);
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
		///	Metodo para consultar los datos generales de los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<DocDetalleProductoData> SearchDocDetalleProductoRecep()
		{
			log.trace("SearchDocDetalleProductoRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.Int32);

				List<DocDetalleProductoData> resp = Consulta<DocDetalleProductoData>("spc_ConsultaDocDetalleProductosRecep", parameters);
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
		///	Metodo para consultar los datos generales de los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<DocDetalleProductoData> SearchDocDetalleProductoAgru()
		{
			log.trace("SearchDocDetalleProductoAgru");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idFicha", idFicha, DbType.Int32);

				List<DocDetalleProductoData> resp = Consulta<DocDetalleProductoData>("spc_ConsultaDocDetalleProductosAgru", parameters);
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
		///	Metodo para consultar la cantidad total de los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalProdData SearchDocDetalleTotalProd()
		{
			log.trace("SearchDocDetalleTotalProd");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				DocDetalleTotalProdData resp = Consulta<DocDetalleTotalProdData>("spc_ConsultaDocDetalleTotalProd", parameters).FirstOrDefault();
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
		///	Metodo para consultar la cantidad total de los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalProdData SearchDocDetalleTotalProdRecep()
		{
			log.trace("SearchDocDetalleTotalProdRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.Int32);

				DocDetalleTotalProdData resp = Consulta<DocDetalleTotalProdData>("spc_ConsultaDocDetalleTotalProdRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar la cantidad total de pallets los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalPalletData SearchDocDetalleTotalPallet()
		{
			log.trace("SearchDocDetalleTotalPallet");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				DocDetalleTotalPalletData resp = Consulta<DocDetalleTotalPalletData>("spc_ConsultaDocDetalleTotalPallet", parameters).FirstOrDefault();
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
		///	Metodo para consultar la cantidad total de pallets los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalPalletData SearchDocDetalleTotalPalletRecep()
		{
			log.trace("SearchDocDetalleTotalPalletRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.Int32);

				DocDetalleTotalPalletData resp = Consulta<DocDetalleTotalPalletData>("spc_ConsultaDocDetalleTotalPalletRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar la cantidad total de cajas los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalCajasData SearchDocDetalleTotalCajas()
		{
			log.trace("SearchDocDetalleTotalCajas");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				DocDetalleTotalCajasData resp = Consulta<DocDetalleTotalCajasData>("spc_ConsultaDocDetalleTotalCajas", parameters).FirstOrDefault();
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
		///	Metodo para consultar la cantidad total de cajas los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalCajasData SearchDocDetalleTotalCajasRecep()
		{
			log.trace("SearchDocDetalleTotalCajasRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.Int32);

				DocDetalleTotalCajasData resp = Consulta<DocDetalleTotalCajasData>("spc_ConsultaDocDetalleTotalCajasRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar la cantidad total de unidades los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalCantidadData SearchDocDetalleTotalCantidad()
		{
			log.trace("SearchDocDetalleTotalCantidad");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				DocDetalleTotalCantidadData resp = Consulta<DocDetalleTotalCantidadData>("spc_ConsultaDocDetalleTotalCantidad", parameters).FirstOrDefault();
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
		///	Metodo para consultar la cantidad total de unidades los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalCantidadData SearchDocDetalleTotalCantidadRecep()
		{
			log.trace("SearchDocDetalleTotalCantidadRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.Int32);

				DocDetalleTotalCantidadData resp = Consulta<DocDetalleTotalCantidadData>("spc_ConsultaDocDetalleTotalCantidadRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar la cantidad total del peso los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalPesoData SearchDocDetalleTotalPeso()
		{
			log.trace("SearchDocDetalleTotalPeso");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				DocDetalleTotalPesoData resp = Consulta<DocDetalleTotalPesoData>("spc_ConsultaDocDetalleTotalPeso", parameters).FirstOrDefault();
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
		///	Metodo para consultar la cantidad total del peso los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleTotalPesoData SearchDocDetalleTotalPesoRecep()
		{
			log.trace("SearchDocDetalleTotalPesoRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.Int32);

				DocDetalleTotalPesoData resp = Consulta<DocDetalleTotalPesoData>("spc_ConsultaDocDetalleTotalPesoRecep", parameters).FirstOrDefault();
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
		///	Metodo para consultar la fecha de caducidad minima de los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleFechaMinData SearchDocDetalleFechaMin()
		{
			log.trace("SearchDocDetalleFechaMin");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movimientosId", movimientoId, DbType.Int32);

				DocDetalleFechaMinData resp = Consulta<DocDetalleFechaMinData>("spc_ConsultaDocDetalleFechaMin", parameters).FirstOrDefault();
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
		///	Metodo para consultar la fecha de caducidad minima de los productos en la tabla del documento de detalles al seleccionar un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public DocDetalleFechaMinData SearchDocDetalleFechaMinRecep()
		{
			log.trace("SearchDocDetalleFechaMinRecep");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_codigoQR", codigoQR, DbType.Int32);

				DocDetalleFechaMinData resp = Consulta<DocDetalleFechaMinData>("spc_ConsultaDocDetalleFechaMinRecep", parameters).FirstOrDefault();
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
		#endregion

		#region Busqueda de destinatarios de un usuario para autocomplete
		/// <summary>
		///	Metodo para consultar los movimientos
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<ListDestinatariosData> SearchListDestinatarios()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_usuario", usuario, DbType.Int32);

				List<ListDestinatariosData> resp = Consulta<ListDestinatariosData>("spc_ConsultaListDestinatarios", parameters);
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
		///	Metodo para consultar los movimientos
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<ListDestinatariosData2> SearchListDestinatariosNum()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_usuario", usuario, DbType.Int32);

				List<ListDestinatariosData2> resp = Consulta<ListDestinatariosData2>("spc_ConsultaListDestinatariosNum", parameters);
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
		#endregion

		#region Insersion de estados no existentes en los combos
		/// <summary>
		/// Metodo para guardar los datos de la compañia 
		/// Desarrollador: Javier Ramirez
		/// </summary>
		public void SaveEstadoSiNoExiste()
		{
			log.trace("SaveEstadoSiNoExiste");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_nombreEstado", estadoE, DbType.String);
				parameters.Add("_pais", paisE, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_GuardarEstado", parameters);
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

		#region Busqueda existencia de estado
		/// <summary>
		///	Metodo para consultar los datos generales de la selección de un movimiento
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public EstadoExistencia SearchEstadoExistencia()
		{
			log.trace("SearchEstadoExistencia");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_nombreEstado", estadoE, DbType.String);
				parameters.Add("_pais", paisE, DbType.Int32);

				EstadoExistencia resp = Consulta<EstadoExistencia>("spc_ConsultaExistenciaEstado", parameters).FirstOrDefault();
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
        #endregion
        #endregion
    }

    public static class GenerateEmail
	{
		/// <summary>
		/// Metodo para la generacion de qr aleatorio para el producto
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="longitud"></param>
		/// <returns></returns>
		public static string Email()
		{
			const string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			string emailRandom = "";
			Random rnd = new Random();

			for (int i = 0; i < 15; i++)
			{
				emailRandom = emailRandom + rnd.Next(alfabeto.Length);
			}

			emailRandom = emailRandom + "@invitado.mx";

			return emailRandom.ToString();
		}


	}
}
