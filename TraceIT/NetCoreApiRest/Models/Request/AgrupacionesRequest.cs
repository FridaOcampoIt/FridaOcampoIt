using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSTraceIT.Models.Request
{
    //Busqueda de datos de una ficha
    public class SearchDataFichaRequest
    {
        public string codigo { get; set; }
        public int usuario { get; set; }
    }

    //Guardar ficha temporal
    public class SaveFichaTempRequest
    {
        public int fichaId { get; set; }
        public int tipoFicha { get; set; }
        public int usuario { get; set; }
        public int desconocidoId { get; set; }
    }

    //Busqueda de datos de fichas seleccionadas
    public class SearchDataFichasSeleccionadasRequest
    {
        public int usuario { get; set; }
    }

    //Eliminar una de las fichas seleccionadas
    public class DeleteFichaSeleccionadasRequest
    {
        public int movfichaId { get; set; }
    }

    //Guardar producto desconocido
    public class SaveProdDescRequest
    {
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public string lote { get; set; }
        public string fechaCaducidad { get; set; }
        public string numSerie { get; set; }
        public string codigo { get; set; }
        public int usuario { get; set; }
    }
}
