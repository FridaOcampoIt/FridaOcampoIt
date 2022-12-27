using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Movimientos;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Models.ModelsSQL
{
    public class Movimientos
    {

    }

	public class TipoMovimientoDataCombo
	{
		public List<TraceITListDropDown> tiposMovimientosDataComboList { get; set; }
		public List<TraceITListDropDown> productosDataComboList { get; set; }

		public TipoMovimientoDataCombo()
		{
			this.tiposMovimientosDataComboList = new List<TraceITListDropDown>();
			this.productosDataComboList = new List<TraceITListDropDown>();
		}

	}

	public class PaisEstadoDataCombo
	{
		public List<TraceITListDropDown> paisesDataComboList { get; set; }
		public List<TraceITListDropDown> estadosDataComboList { get; set; }

		public PaisEstadoDataCombo()
		{
			this.paisesDataComboList = new List<TraceITListDropDown>();
			this.estadosDataComboList = new List<TraceITListDropDown>();
		}

	}

	public class InfoLegalTipoRadioB
	{
		public List<TraceITListDropDown> tipoInfoRadioList { get; set; }

		public InfoLegalTipoRadioB()
		{
			this.tipoInfoRadioList = new List<TraceITListDropDown>();
		}

	}

	//Busqueda de un solo dato de la tabla movimientos general pero solo los productos
	public class SearchDataMovimientoGeneralProdData
	{
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralProdRecepList { get; set; }
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralDesProdRecepList { get; set; }
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralUnicoProdRecepList { get; set; }
		public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralOperaProdRecepList { get; set; }
		//public List<TraceITMovimientosDataGeneralProd> movimientosDataGeneralReagrupadoProdRecepList { get; set; }

		public SearchDataMovimientoGeneralProdData()
		{
			this.movimientosDataGeneralProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.movimientosDataGeneralDesProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.movimientosDataGeneralUnicoProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			this.movimientosDataGeneralOperaProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
			//this.movimientosDataGeneralReagrupadoProdRecepList = new List<TraceITMovimientosDataGeneralProd>();
		}
	}

	//Busqueda de un solo dato de la tabla movimientos general pero solo los productos
	public class SearchDataMovimientoMermaData
	{
		public List<TraceITMovimientosDataMerma> movimientosDataMermaList { get; set; }


		public SearchDataMovimientoMermaData()
		{
			this.movimientosDataMermaList = new List<TraceITMovimientosDataMerma>();
		}
	}

}
