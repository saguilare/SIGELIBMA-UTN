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
    
    public partial class Sesion
    {
        public Sesion()
        {
            this.Transaccion = new HashSet<Transaccion>();
        }
    
        public int Id { get; set; }
        public string Usuario { get; set; }
        public System.DateTime Inicio { get; set; }
        public Nullable<System.DateTime> Finalizacion { get; set; }
    
        public virtual Usuario Usuario1 { get; set; }
        public virtual ICollection<Transaccion> Transaccion { get; set; }
    }
}
