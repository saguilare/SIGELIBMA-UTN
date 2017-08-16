using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMANA.SIGELIBMA.BLL.Servicios
{
    public class BitacoraServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public BitacoraServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public bool Insert<T>(T Origen) where T : class
        { 
            return true;
        }

    }
}
