using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class ClienteModel
    {
        [Required]
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        [Required]
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        [Required]
        public string Cedula { get; set; }  
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}