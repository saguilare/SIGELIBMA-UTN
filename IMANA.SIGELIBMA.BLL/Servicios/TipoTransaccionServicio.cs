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
    public class TipoTransaccionServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public TipoTransaccionServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<TipoTransaccion> ObtenerTodos() {
            try
            {
                List<TipoTransaccion> tipos = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    tipos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                tipos = unitOfWork.Repository<TipoTransaccion>().GetAll().ToList();
                
                return tipos;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public TipoTransaccion ObtenerPorId(TipoTransaccion tipop)
        {
            try
            {
                TipoTransaccion tipo = null;

                tipo = (TipoTransaccion) unitOfWork.Repository<TipoTransaccion>().GetById(tipop.Codigo);

                return tipo;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(TipoTransaccion tipop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    tipos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<TipoTransaccion>().Add(tipop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(TipoTransaccion tipop)
        {
            try
            {
 
                unitOfWork.Repository<TipoTransaccion>().Update(tipop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(TipoTransaccion tipop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    tipos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<TipoTransaccion>().Update(tipop);
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
