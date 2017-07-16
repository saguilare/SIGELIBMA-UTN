using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class CompraModel
    {
        public ClienteModel Cliente { get; set; }
        public List<ProductoModel> Productos { get; set; }
        public DepositoModel Deposito { get; set; }
    }
}