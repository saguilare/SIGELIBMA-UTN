﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Models
{
    public class CajaModel
    {
        [Required]
        public int Codigo { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public int Estado { get; set; }
        public int Movimiento { get; set; }
        public decimal Monto { get; set; }
        public string Razon { get; set; }
    }
}