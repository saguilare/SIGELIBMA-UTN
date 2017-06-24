

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
    public class RolService
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public RolService() {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Role> GetAll() {
            try
            {
                List<Role> roles = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}
                roles = unitOfWork.Repository<Role>().GetAll().ToList();
                
                return roles;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Role GetById(Role rolp)
        {
            try
            {
                Role rol = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}
                rol = (Role) unitOfWork.Repository<Role>().GetById(rolp);

                return rol;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Delete(Role rolp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}
                unitOfWork.Repository<Role>().Delete(rolp);
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Update(Role rolp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}
                unitOfWork.Repository<Role>().Update(rolp);
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
