using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class EstadoModel
    {
        [Required]
        public int Codigo { get; set; }
        [Required]
        public int Estado { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}