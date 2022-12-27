namespace WSTraceIT.Models.Base
{
	public class TraceITResponse
	{
		public string messageEng { get; set; }
		public string messageEsp { get; set; }
	}

	public class TraceITListDropDown
	{
		public int id { get; set; }
		public string data { get; set; }
	}

	public class TraceITListFichas
	{
		public int movfichaId { get; set; }
		public int idFicha { get; set; }
		public string nombreFicha { get; set; }
		public int numeroPallets { get; set; }
		public int numeroCajas { get; set; }
		public string producto { get; set; }
		public int cantidad { get; set; }
		public int tipoFicha { get; set; }
		public string nombreTipoFicha { get; set; }
		public string lote { get; set; }
		public string fechaCaducidad { get; set; }
		public string numSerie { get; set; }
		public int usuario { get; set; }
	}

	/// <summary>
	/// Modelo para traer los datos generales de un movimiento
	/// Desarrollador: Javier Ramirez
	/// </summary>
	public class TraceITMovimientosDataGeneralProd
	{
		public int movimientoId { get; set; }
		public int productoMovimientoId { get; set; }
		public string producto { get; set; }
		public int cantidad { get; set; }
		public int productosRecibidos { get; set; }
		public int merma { get; set; }
		public int tipoRecepcion { get; set; }
		public int familiaProductoId { get; set; }
		public int numeroCajas { get; set; }
		public int embalajeId { get; set; }

		#region Constructor
		public TraceITMovimientosDataGeneralProd()
        {
			this.movimientoId = 0;
			this.productoMovimientoId = 0;
			this.producto = "";
			this.cantidad = 0;
			this.productosRecibidos = 0;
			this.merma = 0;
			this.tipoRecepcion = 0;
			this.familiaProductoId = 0;
			this.numeroCajas = 0;
			this.embalajeId = 0;
		}
		#endregion
	}


    /// <summary>
    /// Modelo para traer los datos generales de un movimiento
    /// Desarrollador: Javier Ramirez
    /// </summary>
    public class TraceITMovimientosDataMerma
	{
		public string producto { get; set; }
		public int merma { get; set; }
		public int productoMovimiento { get; set; }
		public int total { get; set; }
	}
}
