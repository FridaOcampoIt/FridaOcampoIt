using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.ModelsSQL
{
    public class Agrupacion
    {
    }

    /// <summary>
    /// Modelo para traer los datos de todas las fichas seleccionadas por un usuario
    /// Desarrollador: Javier Ramirez
    /// </summary>
    //Busqueda de datos de fichas seleccionadas
    public class FichasSeleccionadasData
    {
        public List<TraceITListFichas> fichasSeleccionadasDataList { get; set; }
        public List<TraceITListFichas> fichasDesconocidasDataList { get; set; }
        public List<TraceITListFichas> fichasUnicasDataList { get; set; }
        public List<TraceITListFichas> fichasMovimientosDataList { get; set; }


        public FichasSeleccionadasData()
        {
            this.fichasSeleccionadasDataList = new List<TraceITListFichas>();
            this.fichasDesconocidasDataList = new List<TraceITListFichas>();
            this.fichasUnicasDataList = new List<TraceITListFichas>();
            this.fichasMovimientosDataList = new List<TraceITListFichas>();
        }
    }
}
