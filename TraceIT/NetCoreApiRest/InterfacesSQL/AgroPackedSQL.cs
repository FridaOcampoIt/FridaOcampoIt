using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using WSTraceIT.Models.Response;

namespace WSTraceIT.InterfacesSQL
{
	public class AgroPackedSQL: DBHelperDapper
	{
		#region Properties
		//Datos Productor
		public int producerId;
		public string producerNumber;
		public int userId;
		public int companyId;

		// Datos para el filtrado del reporte de embalaja reproceso
		public int productId;
		public string dateStart;
		public string dateEnd;
		
		//Nombre SP a ejecutar
		public string sp;

		//Datos del filtro para los Productores
		public List<AgroPackedDataSQL> producerList;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("AgroPackedSQL");
		#endregion

		#region Constructor
		public AgroPackedSQL()
		{
			this.producerId = 0;
			this.producerNumber = String.Empty;
			this.userId = 0;
			this.companyId = 0;

			this.productId = 0;
			this.dateStart = String.Empty;
			this.dateStart = String.Empty;

			this.sp = String.Empty;

			this.producerList = new List<AgroPackedDataSQL>();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar los productores
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<AgroPackedDataSQL> SearchProducerList()
		{
			log.trace("SearchAgroProducerList");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_producerNumber", producerNumber, DbType.String);
				parameters.Add("_companyId", companyId, DbType.Int32);

				List<AgroPackedDataSQL> response = Consulta<AgroPackedDataSQL>("spc_consultaEmpacadorAgroEmpaqEtiq", parameters);
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
		/// Metodo para consultar los datos necesarios para el reporte de embalaje reproceso
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public SearchAgroPackedPackagingReproReportResponse SearchReprocessingPackagingReport()
		{
			log.trace("SearchReprocessingPackagingReport");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(AgroPackedPackagingReproReporDataSQL));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_producerNumber", producerNumber, DbType.String);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				parameters.Add("_dateStart", dateStart, DbType.String);
				parameters.Add("_dateEnd", dateEnd, DbType.String);

				SearchAgroPackedPackagingReproReportResponse response = new SearchAgroPackedPackagingReproReportResponse();

				var responseSQL = ConsultaMultiple("spc_consultaReporteEmbalajeReprocesoEmpacadoAgro", types, parameters);
				CerrarConexion();


				/*
				 * Envio
				 */
				var tem = (from ts in responseSQL[0].Cast<AgroPackedPackagingReproReporDataSQL>().ToList()
						   orderby ts.dateMov
						   where ts.typeMov == 2
						   select new AgroPackedPackagingReporDataCountSQL
						   {
							   total = (from tsbox in responseSQL[0].Cast<AgroPackedPackagingReproReporDataSQL>().ToList()
										where tsbox.dateMov.ToString("yyyy-MM-dd") == ts.dateMov.ToString("yyyy-MM-dd") && tsbox.typeMov == 2
										select tsbox.quantity).Sum(),
							   dateMov = ts.dateMov.ToString("yyyy-MM-dd"),
							   id = ts.id
						   }).ToList();

				tem = tem.GroupBy(g => new { g.dateMov }).Select(grp => grp.FirstOrDefault()).ToList();
				response.infoDataShipment = tem;


				/*
				 * Recepcion
				 */
				tem = (from ts in responseSQL[0].Cast<AgroPackedPackagingReproReporDataSQL>().ToList()
						   orderby ts.dateMov
						   where ts.typeMov == 3
						   select new AgroPackedPackagingReporDataCountSQL
						   {
							   total = (from tsbox in responseSQL[0].Cast<AgroPackedPackagingReproReporDataSQL>().ToList()
										where tsbox.dateMov.ToString("yyyy-MM-dd") == ts.dateMov.ToString("yyyy-MM-dd") && tsbox.typeMov == 3
										select tsbox.received).Sum(),
							   dateMov = ts.dateMov.ToString("yyyy-MM-dd"),
							   id = ts.parentId
						   }).ToList();

				tem = tem.GroupBy(g => new { g.dateMov }).Select(grp => grp.FirstOrDefault()).ToList();
				response.infoDataReceived = tem;

				/*
				 * Merma
				 */
				/*tem = (from ts in responseSQL[0].Cast<AgroPackedPackagingReproReporDataSQL>().ToList()
						   orderby ts.dateMov
						   where ts.typeMov == 3
						   select new AgroPackedPackagingReporDataCountSQL
						   {
							   total = (from tsbox in responseSQL[0].Cast<AgroPackedPackagingReproReporDataSQL>().ToList()
										where tsbox.dateMov.ToString("yyyy-MM-dd") == ts.dateMov.ToString("yyyy-MM-dd") && tsbox.typeMov == 3
										select tsbox.waste).Sum(),
							   dateMov = ts.dateMov.ToString("yyyy-MM-dd")
						   }).ToList();

				tem = tem.GroupBy(g => new { g.dateMov }).Select(grp => grp.FirstOrDefault()).ToList();
				response.infoDataWaste = tem;*/

				/*
				 * Merma sin reporte
				 */
				tem = (from ts in response.infoDataShipment.ToList()
					   orderby ts.dateMov
					   select new AgroPackedPackagingReporDataCountSQL
					   {
						   total = ts.total - (from tsbox in response.infoDataReceived.ToList()
									where tsbox.id == ts.id
									select tsbox.total).Sum(),
						   dateMov = ts.dateMov
					   }).ToList();

				tem = tem.GroupBy(g => new { g.dateMov }).Select(grp => grp.FirstOrDefault()).ToList();
				response.infoDataWasteReport = tem;

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

		#endregion
	}
}
