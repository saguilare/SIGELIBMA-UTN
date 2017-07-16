//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMANA.SIGELIBMA.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Libro
    {
        public Libro()
        {
            this.DetalleFactura = new HashSet<DetalleFactura>();
        }
    
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public int Categoria { get; set; }
        public int Autor { get; set; }
        public int Proveedor { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public decimal PrevioVenta { get; set; }
        public string Imagen { get; set; }
        public Nullable<int> Estado { get; set; }
        public decimal PrecioVenta { get; set; }
    
        public virtual Autor Autor1 { get; set; }
        public virtual Categoria Categoria1 { get; set; }
        public virtual ICollection<DetalleFactura> DetalleFactura { get; set; }
        public virtual Inventario Inventario { get; set; }
        public virtual Proveedor Proveedor1 { get; set; }
    }
}
