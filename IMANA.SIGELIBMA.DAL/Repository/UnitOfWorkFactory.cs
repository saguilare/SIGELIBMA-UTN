using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;



namespace IMANA.SIGELIBMA.DAL.Repository
{
    /// <summary>
    /// Note: It's assumed that this class is being injected to Services (in BL) using a DI container
    /// 
    /// Usage Example:
    /// 
    /// using (var unitOfWork = factory.CreateNew())
    ///     {
    ///            switchList = unitOfWork.Repository<Switch>().GetAll().ToList();
    ///     }
    /// </summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork CreateNew()
        {            //#error Add an existing Entity Data Model (remove this line).

            // For instance:
            return new UnitOfWork(new SIGELIBMAEntities());
        
        }
    }
}
