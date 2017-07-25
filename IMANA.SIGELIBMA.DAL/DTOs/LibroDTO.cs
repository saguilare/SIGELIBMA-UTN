using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.DAL.Repository;

namespace IMANA.SIGELIBMA.DAL.DTOs
{
    public class LibroDTO
    {
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public virtual AutorDTO Autor1 { get; set; }
        public virtual CategoriaDTO Categoria1 { get; set; }
        public virtual ProveedorDTO Proveedor1 { get; set; }
        public string Fecha { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public decimal PrecioVentaSinImpuestos { get; set; }
        public decimal PrecioVentaConImpuestos { get; set; }
        public string Imagen { get; set; }
        public Nullable<int> Estado { get; set; }

    }
}