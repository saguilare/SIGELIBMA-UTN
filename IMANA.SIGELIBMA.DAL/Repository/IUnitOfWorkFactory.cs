using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMANA.SIGELIBMA.DAL.Repository
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateNew();
    }
}
