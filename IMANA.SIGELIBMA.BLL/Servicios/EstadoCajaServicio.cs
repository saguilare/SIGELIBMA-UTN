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
    public class EstadoCajaServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public EstadoCajaServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<EstadoCaja> ObtenerTodos() {
            try
            {
                List<EstadoCaja> estados = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    estados = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                estados = unitOfWork.Repository<EstadoCaja>().GetAll().ToList();
                
                return estados;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public EstadoCaja ObtenerPorId(EstadoCaja estadop)
        {
            try
            {
                EstadoCaja estado = null;

                estado = (EstadoCaja) unitOfWork.Repository<EstadoCaja>().GetById(estadop.Codigo);

                return estado;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(EstadoCaja estadop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    estados = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<EstadoCaja>().Add(estadop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(EstadoCaja estadop)
        {
            try
            {
 
                unitOfWork.Repository<EstadoCaja>().Update(estadop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(EstadoCaja estadop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    estados = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<EstadoCaja>().Update(estadop);
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
