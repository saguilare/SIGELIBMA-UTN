using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class FacturaModel
    {
        [Required]
        public int Caja { get; set; }

        public string Referencia { get; set; }
        [Required]
        public ClienteModel Cliente { get; set; }
        [Required]
        public List<ProductoModel> Productos { get; set; }
        [Required]
        public TipoPagoModel TipoPago { get; set; }
        
    }
}