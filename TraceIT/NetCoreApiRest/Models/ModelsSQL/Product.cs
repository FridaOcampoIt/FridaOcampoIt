using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Models.ModelsSQL
{
    public class SaveProductSQL
    {
        public int idProduct { get; set; }
        public string udid { get; set; }
        public string expirationDate { get; set; }
        public int familyProductId { get; set; }
        public int directionId { get; set; }
        public int status { get; set; }
        public int cantidad { get; set; }

        public SaveProductSQL()
        {
            this.idProduct = 0;
            this.directionId = 0;
            this.expirationDate = String.Empty;
            this.familyProductId = 0;
            this.udid = String.Empty;
            this.status = 1;
            this.cantidad = 0;
        }
    }

    public class SearchImportProductActive
    {
        public int ImportProduct { get; set; }

        public SearchImportProductActive()
        {
            this.ImportProduct = 0;
        }
    }

    public class SearchImportProductData
    {
        public string fileName { get; set; }
        public int importId { get; set; }
        public int familyProductId { get; set; }
        public int directionId { get; set; }

        public SearchImportProductData()
        {
            this.directionId = 0;
            this.familyProductId = 0;
            this.fileName = String.Empty;
            this.importId = 0;
        }
    }

    public class DropDownFront
    {
        public List<familyDropDown> familyDropDown { get; set; }
        public List<TraceITListDropDown> originDropDown { get; set; }
        public List<TraceITListDropDown> companyDropDown { get; set; }

        public DropDownFront()
        {
            this.familyDropDown = new List<familyDropDown>();
            this.originDropDown = new List<TraceITListDropDown>();
            this.companyDropDown = new List<TraceITListDropDown>();
        }
    }

    public class ProductEmailData
    {
        public string familyName { get; set; }
        public string originName { get; set; }
        public string userName { get; set; }

        public ProductEmailData()
        {
            this.familyName = "";
            this.originName = "";
            this.userName = "";
        }
    }


    public class ProductListId
    {
        public List<int> idProducts { get; set; }
    }

    public class productDataSQL
    {
        public string ciu { get; set; }
        public string pais { get; set; }
        public string ciudad { get; set; }
        public string planta { get; set; }
        public string sector { get; set; }
        public string cosecha { get; set; }
        public string empresa { get; set; }
        public string cosecheroNombre { get; set; }
        public string cosecheroPuesto { get; set; }
        public string cosecheroDescripcion { get; set; }
        public string cosecheroImagen { get; set; }

        public productDataSQL()
        {
            this.ciu = "";
            this.pais = "";
            this.ciudad = "";
            this.planta = "";
            this.sector = "";
            this.cosecha = "";
            this.empresa = "";
            this.cosecheroNombre = "";
            this.cosecheroPuesto = "";
            this.cosecheroDescripcion = "";
            this.cosecheroImagen = "";
        }
    }
}
