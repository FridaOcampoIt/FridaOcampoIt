using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSTraceIT.Models.ModelsSQL
{
    public class OperationDatas
    {
        public GetOperationDataSQL operationInfo { get; set; }
        public List<RawMaterialSQL> rawMaterialsInfo { get; set; }
        public List<OperationDetailSQL> operationDetailsInfo { get; set; }

        public OperationDatas()
        {
            operationInfo = new GetOperationDataSQL();
            rawMaterialsInfo = new List<RawMaterialSQL>();
            operationDetailsInfo = new List<OperationDetailSQL>();
        }
    }

    public class OperationDataSQL
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? idUser { get; set; }
        public int idCompany { get; set; }
        public int idProvider { get; set; }
        public int idOperator { get; set; }
        public int idLine { get; set; }
        public int idProduct { get; set; }
        public int idPackage { get; set; }
        public string grouping { get; set; }
        public int totalUnits { get; set; }
        public string range { get; set; }
        public string scannedCIUs { get; set; }
        public int unitsScanned { get; set; }
        public bool isGroup { get; set; }
        public int idOperation { get; set; }
    }

    public class GetOperationDataSQL
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? idUser { get; set; }
        public string Company { get; set; }
        public string Provider { get; set; }
        public string Operator { get; set; }
        public string Line { get; set; }
        public string Product { get; set; }
        public string Package { get; set; }
        public string grouping { get; set; }
        public int totalUnits { get; set; }
        public string range { get; set; }
        public string scannedCIUs { get; set; }
        public int unitsScanned { get; set; }
        public bool isGroup { get; set; }
        public int idOperation { get; set; }
        public string EtiquetaID { get; set; }
    }

    public class RawMaterialListSQL
    {
        public List<RawMaterialSQL> rawMaterials { get; set; }

        public RawMaterialListSQL()
        {
            rawMaterials = new List<RawMaterialSQL>();
        }
    }

    public class RawMaterialSQL
    {
        public string provider { get; set; }
        public string product { get; set; }
        public string lot { get; set; }

        public RawMaterialSQL()
        {
            provider = "";
            product = "";
            lot = "";
        }
    }

    public class OperationDetailListSQL
    {
        public List<OperationDetailSQL> operationDetails { get; set; }

        public OperationDetailListSQL()
        {
            operationDetails = new List<OperationDetailSQL>();
        }
    }

    public class OperationDetailSQL
    {
        public string pallet { get; set; }
        public string box { get; set; }
        public string range { get; set; }
        public string line { get; set; }
        public string operatorName { get; set; }
        public string scanned { get; set; }

        public OperationDetailSQL()
        {
            pallet = "";
            box = "";
            range = "";
            line = "";
            operatorName = "";
            scanned = "";
        }
    }
}
