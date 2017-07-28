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
    public class TransaccionServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public TransaccionServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public bool RegistrarTransaccion<T>(T bitacora) where T : class 
        {
            try
            {
                unitOfWork.Repository<T>().Add(bitacora);
                return true;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public List<Transaccion> ObtenerTodos() {
            try
            {
                List<Transaccion> transacciones = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    transacciones = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                transacciones = unitOfWork.Repository<Transaccion>().GetAll().ToList();
                
                return transacciones;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Transaccion ObtenerPorId(Transaccion tx)
        {
            try
            {
                Transaccion caja = null;

                caja = (Transaccion) unitOfWork.Repository<Transaccion>().GetById(tx.Codigo);

                return caja;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Transaccion tx)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    transacciones = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Transaccion>().Add(tx);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Transaccion tx)
        {
            try
            {
 
                unitOfWork.Repository<Transaccion>().Update(tx);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Transaccion tx)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    transacciones = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Transaccion>().Update(tx);
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
