using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class DepositoModel
    {
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public string Referencia { get; set; }
        [Required]
        public string BancoEmisor { get; set; }
        [Required]
        public string BancoReceptor { get; set; }
    }
}