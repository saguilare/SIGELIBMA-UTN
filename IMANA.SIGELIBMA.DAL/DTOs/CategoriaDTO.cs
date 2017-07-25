using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMANA.SIGELIBMA.DAL.DTOs
{
    public class CategoriaDTO
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Estado { get; set; }

    }
}
