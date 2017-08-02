using IMANA.SIGELIBMA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class SesionModel
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Finalizacion { get; set; }
        public int SesionCaja { get; set; }
    }
}