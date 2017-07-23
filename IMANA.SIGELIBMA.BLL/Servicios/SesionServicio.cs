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
    public class SesionServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public SesionServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Sesion> ObtenerTodos() {
            try
            {
                List<Sesion> sesiones = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    sesiones = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                sesiones = unitOfWork.Repository<Sesion>().GetAll().ToList();
                
                return sesiones;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Sesion ObtenerPorId(Sesion sesionp)
        {
            try
            {
                Sesion caja = null;

                caja = (Sesion) unitOfWork.Repository<Sesion>().GetById(sesionp.Id);

                return caja;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Sesion sesionp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    sesiones = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Sesion>().Add(sesionp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Sesion sesionp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    sesiones = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
             
                unitOfWork.Repository<Sesion>().Update(sesionp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Sesion sesionp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    sesiones = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Sesion>().Update(sesionp);
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
