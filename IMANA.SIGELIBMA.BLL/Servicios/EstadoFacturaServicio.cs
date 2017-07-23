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
    public class EstadoFacturaServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public EstadoFacturaServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<EstadoFactura> ObtenerTodos() {
            try
            {
                List<EstadoFactura> estados = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    estados = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                estados = unitOfWork.Repository<EstadoFactura>().GetAll().ToList();
                
                return estados;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public EstadoFactura ObtenerPorId(EstadoFactura estadop)
        {
            try
            {
                EstadoFactura estadoFact = null;

                estadoFact = (EstadoFactura) unitOfWork.Repository<EstadoFactura>().GetById(estadop.Codigo);

                return estadoFact;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(EstadoFactura estadop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    estados = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<EstadoFactura>().Add(estadop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(EstadoFactura estadop)
        {
            try
            {
 
                unitOfWork.Repository<EstadoFactura>().Update(estadop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(EstadoFactura estadop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    estados = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<EstadoFactura>().Update(estadop);
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
