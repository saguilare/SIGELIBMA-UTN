using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class DepositoModel
    {
        public DateTime Fecha { get; set; }
        public string Referencia { get; set; }
        public string BancoEmisor { get; set; }
        public string BancoReceptor { get; set; }
    }
}