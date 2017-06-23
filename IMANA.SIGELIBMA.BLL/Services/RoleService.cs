using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.DAL.Repository;
using System.Data.Entity;

namespace IMANA.SIGELIBMA.BLL.Services
{
    public class RoleService
    {
        UnitOfWorkFactory factory = new UnitOfWorkFactory();

        public List<Role> GetAll() {
            try
            {
                List<Role> roles = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}

                var unitOfWork = (UnitOfWork)factory.CreateNew();
                
                    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                
                return roles;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
