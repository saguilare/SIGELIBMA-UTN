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
    
    public partial class Transaccion
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Sesion { get; set; }
        public string Tabla { get; set; }
        public string TuplaNueva { get; set; }
        public string TuplaAnterior { get; set; }
        public int Tipo { get; set; }
    
        public virtual Sesion Sesion1 { get; set; }
        public virtual TipoTransaccion TipoTransaccion { get; set; }
    }
}
