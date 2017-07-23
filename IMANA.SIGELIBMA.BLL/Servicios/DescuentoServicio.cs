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
    public class DescuentoServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public DescuentoServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Descuento> ObtenerTodos() {
            try
            {
                List<Descuento> descuentos = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    descuentos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                descuentos = unitOfWork.Repository<Descuento>().GetAll().ToList();
                
                return descuentos;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Descuento ObtenerPorId(Descuento descuentop)
        {
            try
            {
                Descuento descuento = null;

                descuento = (Descuento) unitOfWork.Repository<Descuento>().GetById(descuentop.Codigo);

                return descuento;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Descuento descuentop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    descuentos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Descuento>().Add(descuentop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Descuento descuentop)
        {
            try
            {
 
                unitOfWork.Repository<Descuento>().Update(descuentop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Descuento descuentop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    descuentos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Descuento>().Update(descuentop);
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
