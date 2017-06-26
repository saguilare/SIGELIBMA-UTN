

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

        public List<Rol> GetAll() {
            try
            {
                List<Rol> roles = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}
                roles = unitOfWork.Repository<Rol>().GetAll().Where(x => x.Estado == 1).ToList();
                
                return roles;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Rol GetById(Rol rolp)
        {
            try
            {
                Rol rol = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}
                rol = (Rol) unitOfWork.Repository<Rol>().GetById(rolp);

                return rol;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Add(Rol rolp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}
                unitOfWork.Repository<Rol>().Add(rolp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public bool Delete(Rol rolp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}
                rolp.Estado = 0;
                unitOfWork.Repository<Rol>().Update(rolp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Update(Rol rolp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().GetAll().ToList();
                //}
                unitOfWork.Repository<Rol>().Update(rolp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
