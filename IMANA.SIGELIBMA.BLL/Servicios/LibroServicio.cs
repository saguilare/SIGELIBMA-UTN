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
    public class LibroServicio
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public LibroServicio()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Libro> ObtenerTodos() {
            try
            {
                List<Libro> libros = null;
                libros = unitOfWork.Repository<Libro>().GetAll().ToList();
                
                return libros;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Libro ObtenerPorId(Libro librop)
        {
            try
            {
                Libro libro = null;
                libro = (Libro)unitOfWork.Repository<Libro>().GetById(librop);

                return libro;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Agregar(Libro librop)
        {
            try
            {
                unitOfWork.Repository<Libro>().Add(librop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
       
        public bool Desabilitar(Libro librop)
        {
            try
            {
                librop.Estado = 0;
                unitOfWork.Repository<Libro>().Update(librop);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Modificar(Libro librop)
        {
            try
            {
                unitOfWork.Repository<Libro>().Update(librop);
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
