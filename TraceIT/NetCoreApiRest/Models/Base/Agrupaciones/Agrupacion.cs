using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSTraceIT.Models.Base.Agrupaciones
{
    public class Agrupacion
    {
    }

    /// <summary>
    /// Modelo para traer los datos de una ficha
    /// Desarrollador: Javier Ramirez
    /// </summary>
    public class FichaData
    {
        public int idFicha { get; set; }
        public string nombreFicha { get; set; }
        public int numeroPallets { get; set; }
        public int numeroCajas { get; set; }
        public int producto { get; set; }
        public int cantidad { get; set; }
        public int tipoFicha { get; set; }
        public string lote { get; set; }
        public string fechaCaducidad { get; set; }
        public string numSerie { get; set; }
        public string codigo { get; set; }

        public FichaData()
        {
            this.idFicha = 0;
            this.nombreFicha = String.Empty;
            this.numeroPallets = 0;
            this.numeroCajas = 0;
            this.producto = 0;
            this.cantidad = 0;
            this.tipoFicha = 0;
            this.lote = String.Empty;
            this.fechaCaducidad = String.Empty;
            this.numSerie = String.Empty;
            this.codigo = String.Empty;
        }
    }

    /// <summary>
    /// Modelo para traer los datos de todas las fichas seleccionadas por un usuario
    /// Desarrollador: Javier Ramirez
    /// </summary>
    public class FichasSeleccionadasData2
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

        public FichasSeleccionadasData2()
        {
            this.movfichaId = 0;
            this.idFicha = 0;
            this.nombreFicha = String.Empty;
            this.numeroPallets = 0;
            this.numeroCajas = 0;
            this.producto = String.Empty;
            this.cantidad = 0;
            this.tipoFicha = 0;
            this.nombreFicha = String.Empty;
            this.lote = String.Empty;
            this.fechaCaducidad = String.Empty;
            this.numSerie = String.Empty;
            this.usuario = 0;
        }
    }
}
