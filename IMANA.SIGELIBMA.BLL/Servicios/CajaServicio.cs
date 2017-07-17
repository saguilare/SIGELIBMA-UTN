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
    public class CajaServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public CajaServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Caja> ObtenerTodos() {
            try
            {
                List<Caja> cajas = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajas = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                cajas = unitOfWork.Repository<Caja>().GetAll().ToList();
                
                return cajas;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Caja ObtenerPorId(Caja cajap)
        {
            try
            {
                Caja caja = null;

                caja = (Caja) unitOfWork.Repository<Caja>().GetById(cajap.Codigo);

                return caja;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Caja cajap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajas = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Caja>().Add(cajap);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Caja cajap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajas = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                cajap.Estado = 0;
                unitOfWork.Repository<Caja>().Update(cajap);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Caja cajap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    cajas = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Caja>().Update(cajap);
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
