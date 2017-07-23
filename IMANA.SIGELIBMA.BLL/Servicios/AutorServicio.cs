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
    public class AutorServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public AutorServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Autor> ObtenerTodos() {
            try
            {
                List<Autor> autores = null;
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    autores = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                autores = unitOfWork.Repository<Autor>().GetAll().ToList();
                
                return autores;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Autor ObtenerPorId(Autor autorp)
        {
            try
            {
                Autor autor = null;

                autor = (Autor) unitOfWork.Repository<Autor>().GetById(autorp.Codigo);

                return autor;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Autor autorp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    autores = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Autor>().Add(autorp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Desabilitar(Autor autorp)
        {
            try
            {
 
                unitOfWork.Repository<Autor>().Update(autorp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Autor autorp)
        {
            try
            {
                // using (var unitOfWork = (UnitOfWork)factory.CreateNew())
                // {
                //    autores = unitOfWork.Repository<Role>().ObtenerTodos().ToList();
                //}
                unitOfWork.Repository<Autor>().Update(autorp);
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
