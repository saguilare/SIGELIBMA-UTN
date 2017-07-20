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
    public class MovimientoCajaServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public MovimientoCajaServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<MovimientoCaja> ObtenerTodos() {
            try
            {
                List<MovimientoCaja> movimientos = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    movimientos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                movimientos = unitOfWork.Repository<MovimientoCaja>().GetAll().ToList();
                
                return movimientos;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public MovimientoCaja ObtenerPorId(MovimientoCaja cajap)
        {
            try
            {
                MovimientoCaja caja = null;

                caja = (MovimientoCaja) unitOfWork.Repository<MovimientoCaja>().GetById(cajap.Codigo);

                return caja;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(MovimientoCaja cajap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    movimientos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<MovimientoCaja>().Add(cajap);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(MovimientoCaja cajap)
        {
            try
            {
 
                unitOfWork.Repository<MovimientoCaja>().Update(cajap);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(MovimientoCaja cajap)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    movimientos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<MovimientoCaja>().Update(cajap);
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
