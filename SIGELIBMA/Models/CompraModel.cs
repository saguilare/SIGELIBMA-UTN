using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class CompraModel
    {
        [Required]
        public ClienteModel Cliente { get; set; }
        [Required]
        public List<ProductoModel> Productos { get; set; }
        [Required]
        public DepositoModel Deposito { get; set; }
    }
}