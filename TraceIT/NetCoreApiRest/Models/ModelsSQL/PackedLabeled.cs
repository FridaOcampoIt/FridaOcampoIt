using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra los datos necesarios para los combos del catálogo de empacado y etiquetado
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class PackedLabeledComboSQL
	{
		public int id { get; set; }
		public string data { get; set; }

		#region Constructor
		public PackedLabeledComboSQL()
		{
			this.id = 0;
			this.data = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contendra los datos necesarios para los combos del catálogo de empacado y etiquetado
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class RawMaterialsSQL
	{
		public string providerName { get; set; }
		public string rawMaterial { get; set; }
		
		#region Constructor
		public RawMaterialsSQL()
		{
			this.providerName = String.Empty;
			this.rawMaterial = String.Empty;
		}
		#endregion
	}

	public class ExisteCIUSQL
	{
		public string existe { get; set; }

        #region Constructor
		public ExisteCIUSQL()
		{
			this.existe = "";
        }
        #endregion
    }

	/// <summary>
	/// Clase para mapear la información de los pallets escaneados
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class InfoQRCodeSQL
	{
		public int opId { get; set; }
		public string product { get; set; }
		public int productId { get; set; }
		public string pallet { get; set; }
		public int packagingId { get; set; }
		public int quantity { get; set; }

		#region Constructor
		public InfoQRCodeSQL()
		{
			this.opId = 0;
			this.product = String.Empty;
			this.productId = 0;
			this.pallet = String.Empty;
			this.packagingId = 0;
			this.quantity = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase para mapear las cajas de los pallets escaneados
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class InfoOperacionPalletsSQL
	{
		public int OperacionId { get; set; }
		public int CompaniaId { get; set; }
		public int ProveedorId { get; set; }
		public int OperadorId { get; set; }
		public int LineaId { get; set; }
		public int ProductoId { get; set; }
		public int EmbalajeId { get; set; }
		public int unidadesPorCaja { get; set; }
		public int CajasPallet { get; set; }
		public int DetalleId { get; set; }
		public string Pallet { get; set; }
		public string Linea { get; set; }
		public string RangoMin { get; set; }
		public string RangoMax { get; set; }
		public string EtiquetaID { get; set; }

		#region Constructor
		public InfoOperacionPalletsSQL()
		{
			this.OperacionId = 0;
			this.CompaniaId = 0;
			this.ProveedorId = 0;
			this.OperadorId = 0;
			this.LineaId = 0;
			this.ProductoId = 0;
			this.EmbalajeId = 0;
			this.unidadesPorCaja = 0;
			this.CajasPallet = 0;
			this.DetalleId = 0;
			this.Linea = String.Empty;
			this.RangoMin = String.Empty;
			this.RangoMax = String.Empty;
			this.EtiquetaID = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase para mapear las cajas de los pallets escaneados
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class InfoOperacionPalletScannedSQL
	{
		public int OperacionDetalleId { get; set; }
		public int Orden { get; set; }
		public string Codigo { get; set; }

		#region Constructor
		public InfoOperacionPalletScannedSQL()
		{
			this.OperacionDetalleId = 0;
			this.Orden = 0;
			this.Codigo = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase para mapear los movimientos por operación del pallet
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class InfoMovOperacionPalletSQL
	{
		public int MovimientosId { get; set; }
		public int TipoMovimientoId { get; set; }
		public int OperacionId { get; set; }
		public string Pallet { get; set; }
		public int nPalletsOp { get; set; }
		public string Latitud { get; set; }
		public string Longitud { get; set; }
		public int PK_Usuario { get; set; }

		#region Constructor
		public InfoMovOperacionPalletSQL()
		{
			this.MovimientosId = 0;
			this.TipoMovimientoId = 0;
			this.OperacionId = 0;
			this.Pallet = String.Empty;
			this.nPalletsOp = 0;
			this.Latitud = String.Empty;
			this.Longitud = String.Empty;
			this.PK_Usuario = 0;
		}
		#endregion
	}
	#endregion
}
