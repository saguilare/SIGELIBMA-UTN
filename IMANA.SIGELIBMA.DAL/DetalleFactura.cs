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
    
    public partial class DetalleFactura
    {
        public int Codigo { get; set; }
        public int Factura { get; set; }
        public string Articulo { get; set; }
        public int Cantidad { get; set; }
    
        public virtual Factura Factura1 { get; set; }
        public virtual Libro Libro { get; set; }
    }
}
