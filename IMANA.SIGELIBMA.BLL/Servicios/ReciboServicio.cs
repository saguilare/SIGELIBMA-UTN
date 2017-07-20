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
    public class ReciboServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public ReciboServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Recibo> ObtenerTodos()
        {
            try
            {
                List<Recibo> recibos = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    recibos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                recibos = unitOfWork.Repository<Recibo>().GetAll().ToList();
                
                return recibos;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Recibo ObtenerPorId(Recibo recibop)
        {
            try
            {
                Recibo recibo = null;

                recibo = (Recibo) unitOfWork.Repository<Recibo>().GetById(recibop.Codigo);

                return recibo;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Recibo recibop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    recibos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Recibo>().Add(recibop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Recibo recibop)
        {
            try
            {
 
                unitOfWork.Repository<Recibo>().Update(recibop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Recibo recibop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    recibos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Recibo>().Update(recibop);
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
