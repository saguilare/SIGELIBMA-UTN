

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.DAL.Repository;
using System.Data.Entity;

namespace IMANA.SIGELIBMA.BLL.Servicios
{
    public class RolServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public RolServicio() {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Rol> ObtenerTodos() {
            try
            {
                List<Rol> roles = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                roles = unitOfWork.Repository<Rol>().GetAll().ToList();
                
                return roles;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Rol ObtenerPorId(Rol rolp)
        {
            try
            {
                Rol rol = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                rol = (Rol) unitOfWork.Repository<Rol>().GetById(rolp.Codigo);

                return rol;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Rol rolp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
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

        public bool Desabilitar(Rol rolp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
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

        public bool Modificar(Rol rolp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    roles = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
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
