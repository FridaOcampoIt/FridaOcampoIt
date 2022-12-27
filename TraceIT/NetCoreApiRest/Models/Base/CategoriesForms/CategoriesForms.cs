using System;

namespace WSTraceIT.Models.Base.CategoriesForms
{
    public class CategoriesForms
    {
        public int categoriaFormularioId { get; set; }
        public int formularioId { get; set; }
        public string nombreCategoria { get; set; }
        public int usuarioCreadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusCategoriaId { get; set; }
        public CategoriesForms()
        {
            this.categoriaFormularioId = 0;
            this.formularioId = 0;
            this.nombreCategoria = String.Empty;
            this.usuarioCreadorId = 0;
            this.fechaCreacion = DateTime.Now;
            this.usuarioModificadorId = 0;
            this.fechaModificacion = DateTime.Now;
            this.estatusCategoriaId = 0;

        }
    }
    /// <summary>
    /// ´Peticiones (Request) de categorias formulario
    /// Desarrollador:  Roberto Ortega
    /// </summary>
    public class deleteCategoriesFormsRequest
    {
        public int categoriaFormularioId { get; set; }
    }
    public class saveCategoriesFormsRequest
    {
        public int categoriaFormularioId { get; set; }
        public int formularioId { get; set; }
        public string nombreCategoria { get; set; }
        public int usuarioCreadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusCategoriaId { get; set; }
    }

    public class searchCategoriesFormsByIdRequest
    {
        public int 
            categoriaFormularioId { get; set; }
    }

    public class updateCategoriesFormsRequest
    {
        public int categoriaFormularioId { get; set; }      
        public int formularioId { get; set; }
        public string nombreCategoria { get; set; }
        public int usuarioCreadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusCategoriaId { get; set; }
    }
    /// <summary>
    /// Respuestas (response)  de categorias formulario
    /// Desarrollador:  Roberto Ortega
    /// </summary> 

    public class deleteCategoriesFormsResponse : TraceITResponse
    {
        public deleteCategoriesFormsResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }
    public class saveCategoriesFormsResponse : TraceITResponse
    {
        public saveCategoriesFormsResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }

    public class searchCategoriesFormsByIdResponse : TraceITResponse
    {
        public int categoriaFormularioId { get; set; }
        public int formularioId { get; set; }
        public string nombreCategoria { get; set; }
        public int usuarioCreadorId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificadorId { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int estatusCategoriaId { get; set; }
        public searchCategoriesFormsByIdResponse()
        {
            this.categoriaFormularioId = 0;
            this.formularioId = 0;
            this.nombreCategoria = String.Empty;
            this.usuarioCreadorId = 0;
            this.fechaCreacion = DateTime.Now;
            this.usuarioModificadorId = 0;
            this.fechaModificacion = DateTime.Now;
            this.estatusCategoriaId = 0;
        }
    }

    public class updateCategoriesFormsResponse : TraceITResponse
    {
        public updateCategoriesFormsResponse()
        {
            this.messageEng = String.Empty;
            this.messageEsp = String.Empty;
        }
    }
}
