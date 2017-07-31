using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.DAL.Repository;
using System.Drawing;

namespace SIGELIBMA.Models
{
    public class LibroModel
    {
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public virtual AutorModel Autor1 { get; set; }
        public virtual CategoriaModel Categoria1 { get; set; }
        public virtual ProveedorModel Proveedor1 { get; set; }
        public string Fecha { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public decimal PrecioVentaSinImpuestos { get; set; }
        public decimal PrecioVentaConImpuestos { get; set; }
        public string NombreImagen { get; set; }
        public string Imagen { get; set; }

        public Nullable<int> Estado { get; set; }

    }
}