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
    
    public partial class BitacoraDescuento
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public Nullable<System.DateTime> FechaFinalizacion { get; set; }
        public Nullable<int> Estado { get; set; }
        public int TransaccionId { get; set; }
    
        public virtual Transaccion Transaccion { get; set; }
    }
}
