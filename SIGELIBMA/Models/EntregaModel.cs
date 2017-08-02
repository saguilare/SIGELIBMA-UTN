using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class EntregaModel
    {
        [Required]
        public string NumeroFactura { get; set; }
        [Required]
        public int Estado { get; set; }
    }
}