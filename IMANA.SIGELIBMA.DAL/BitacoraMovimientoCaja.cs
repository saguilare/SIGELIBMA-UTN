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
    
    public partial class BitacoraMovimientoCaja
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Caja { get; set; }
        public decimal Monto { get; set; }
        public int Tipo { get; set; }
        public System.DateTime Fecha { get; set; }
        public int Sesion { get; set; }
    
        public virtual Sesion Sesion1 { get; set; }
    }
}
