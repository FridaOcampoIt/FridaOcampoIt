using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Agrupaciones;

namespace WSTraceIT.Models.Response
{
    //Busqueda de un solo dato de la tabla movimientos general
    public class SearchDataFichaResponse : TraceITResponse
    {

        public SearchDataFichaResponse()
        {

            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    public class SearchDataFichaResponse2 : TraceITResponse
    {
        public List<FichaData> fichasDataList { get; set; }

        public SearchDataFichaResponse2()
        {
            this.fichasDataList = new List<FichaData>();
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    //Guardar ficha temporal
    public class SaveFichaTempResponse : TraceITResponse
    {
        public SaveFichaTempResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    //Busqueda de datos de fichas seleccionadas
    public class SearchDataFichasSeleccionadasResponse : TraceITResponse
    {
        public List<TraceITListFichas> fichasSeleccionadasDataList { get; set; }
        public List<TraceITListFichas> fichasDesconocidasDataList { get; set; }
        public List<TraceITListFichas> fichasUnicasDataList { get; set; }
        public List<TraceITListFichas> fichasMovimientosDataList { get; set; }

        public SearchDataFichasSeleccionadasResponse()
        {
            this.fichasSeleccionadasDataList = new List<TraceITListFichas>();
            this.fichasDesconocidasDataList = new List<TraceITListFichas>();
            this.fichasUnicasDataList = new List<TraceITListFichas>();
            this.fichasMovimientosDataList = new List<TraceITListFichas>();
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    //Eliminar una de las fichas seleccionadas
    public class DeleteFichaSeleccionadaResponse : TraceITResponse
    {
        public DeleteFichaSeleccionadaResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    //Guardar producto desconocido
    public class SaveProdDescResponse : TraceITResponse
    {
        public SaveProdDescResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }
}
