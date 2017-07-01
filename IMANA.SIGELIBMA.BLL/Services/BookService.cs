using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMANA.SIGELIBMA.BLL.Services
{
    public class BookService
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public BookService()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Libro> GetAll() {
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

        public Libro GetById(Libro librop)
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

        public bool Add(Libro librop)
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
       
        public bool Delete(Libro librop)
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

        public bool Update(Libro librop)
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
