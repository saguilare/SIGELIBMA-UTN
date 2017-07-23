using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class ProductoModel
    {
        [Required]
        public string Codigo { get; set; }
        [Required]
        public int Cantidad { get; set; }
    }
}