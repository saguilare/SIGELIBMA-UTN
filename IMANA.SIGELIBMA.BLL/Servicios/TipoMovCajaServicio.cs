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
    public class TipoMovCajaServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public TipoMovCajaServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<TipoMovimientoCaja> ObtenerTodos() {
            try
            {
                List<TipoMovimientoCaja> tiposMovCaja = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    tiposMovCaja = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                tiposMovCaja = unitOfWork.Repository<TipoMovimientoCaja>().GetAll().ToList();
                
                return tiposMovCaja;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public TipoMovimientoCaja ObtenerPorId(TipoMovimientoCaja tipo)
        {
            try
            {
                TipoMovimientoCaja tipoMovCaja = null;

                tipoMovCaja = (TipoMovimientoCaja) unitOfWork.Repository<TipoMovimientoCaja>().GetById(tipo.Codigo);

                return tipoMovCaja;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(TipoMovimientoCaja tipo)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    tiposMovCaja = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<TipoMovimientoCaja>().Add(tipo);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(TipoMovimientoCaja tipo)
        {
            try
            {
 
                unitOfWork.Repository<TipoMovimientoCaja>().Update(tipo);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(TipoMovimientoCaja tipo)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    tiposMovCaja = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<TipoMovimientoCaja>().Update(tipo);
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
