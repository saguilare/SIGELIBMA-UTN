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
    public class TipoPagoServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public TipoPagoServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<TipoPago> ObtenerTodos() {
            try
            {
                List<TipoPago> tipos = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    tipos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                tipos = unitOfWork.Repository<TipoPago>().GetAll().ToList();
                
                return tipos;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public TipoPago ObtenerPorId(TipoPago tiposp)
        {
            try
            {
                TipoPago tipoPago = null;

                tipoPago = (TipoPago) unitOfWork.Repository<TipoPago>().GetById(tiposp.Codigo);

                return tipoPago;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(TipoPago tiposp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    tipos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<TipoPago>().Add(tiposp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(TipoPago tiposp)
        {
            try
            {
 
                unitOfWork.Repository<TipoPago>().Update(tiposp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(TipoPago tiposp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    tipos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<TipoPago>().Update(tiposp);
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
