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
    
    public partial class BitacoraUsuario
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Segundo_Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Correo { get; set; }
        public Nullable<int> Estado { get; set; }
        public string Telefono { get; set; }
        public int TransaccionId { get; set; }
    
        public virtual Transaccion Transaccion { get; set; }
    }
}
