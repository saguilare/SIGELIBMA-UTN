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
    public class DepositoServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public DepositoServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Deposito> ObtenerTodos() {
            try
            {
                List<Deposito> depositos = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    depositos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                depositos = unitOfWork.Repository<Deposito>().GetAll().ToList();
                
                return depositos;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Deposito ObtenerPorId(Deposito depositop)
        {
            try
            {
                Deposito deposito = null;

                deposito = (Deposito) unitOfWork.Repository<Deposito>().GetById(depositop.Codigo);

                return deposito;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Deposito depositop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    depositos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Deposito>().Add(depositop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Deposito depositop)
        {
            try
            {
 
                unitOfWork.Repository<Deposito>().Update(depositop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Deposito depositop)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    depositos = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Deposito>().Update(depositop);
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
