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
    
    public partial class CajaUsuario
    {
        public CajaUsuario()
        {
            this.MovimientoCaja = new HashSet<MovimientoCaja>();
        }
    
        public int Sesion { get; set; }
        public int Caja { get; set; }
        public string Usuario { get; set; }
        public System.DateTime Ingreso { get; set; }
        public Nullable<System.DateTime> Salida { get; set; }
    
        public virtual Caja Caja1 { get; set; }
        public virtual Usuario Usuario1 { get; set; }
        public virtual ICollection<MovimientoCaja> MovimientoCaja { get; set; }
    }
}
