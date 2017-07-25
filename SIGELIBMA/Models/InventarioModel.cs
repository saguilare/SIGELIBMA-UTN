using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class InventarioModel
    {
        [Required]
        public string Libro { get; set; }
        [Required]
        public int Stock { get; set; }
        
        public int Maximo { get; set; }
        [Required]
        public int Minimo { get; set; }
        [Required]
        public int Estado { get; set; }
        
    }
}