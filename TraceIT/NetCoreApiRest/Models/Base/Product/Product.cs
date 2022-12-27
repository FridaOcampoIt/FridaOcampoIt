using System;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Base.Product
{
    public class ProductList
    {
        public string ciu { get; set; }
        public int productId { get; set; }
        public string family { get; set; }
        public string udid { get; set; }
    }

    public class CIUList
    {
        public string ciu { get; set; }
    }

    public class ProductDetails
    {
        public int idProduct { get; set; }
        public string udid { get; set; }
        public string expirationDate { get; set; }
        public int familyId { get; set; }
        public int directionId { get; set; }
        public string f_data { get; set; }
        public int f_extra { get; set; }
        public int status { get; set; }
    }

    public class ProductDetailsCIU
    {
        public string Ciu { get; set; } // Nuevo origen, obtenido a partir de CIU
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ProductoImagen { get; set; }
        public string Presentacion { get; set; }
        public string Marca { get; set; }
        public string NumSerie { get; set; }
        public string Lote { get; set; }
        public string Caducidad { get; set; }
        public string GTIN { get; set; }
        public string Empresa { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Planta { get; set; }
        public string LineaProd { get; set; }
        public string LineaProduccion { get; set; }
        public string FechaProduccion { get; set; }
        public string CosecheroNombre { get; set; }
        public string CosecheroPuesto { get; set; }
        public string CosecheroDescripcion { get; set; }
        public string CosecheroImagen { get; set; }
        public string Cosecha { get; set; }
        public string Sector { get; set; }
        public string Rancho { get; set; }
        public string Operador { get; set; }
        public alertas listaAlerta { get; set; }
    }

}
