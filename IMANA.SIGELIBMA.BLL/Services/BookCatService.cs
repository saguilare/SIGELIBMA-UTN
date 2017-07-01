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
    public class BookCatService
    {
        UnitOfWork unitOfWork  = null;
        DbContext context = null;


        public BookCatService()
        {
            this.context = new SIGELIBMAEntities();
            this.unitOfWork = new UnitOfWork(this.context);
        }

        public List<Categoria> GetAll() {
            try
            {
                List<Categoria> category = null;
                category = unitOfWork.Repository<Categoria>().GetAll().ToList();

                return category;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Categoria GetById(Categoria categoryp)
        {
            try
            {
                Categoria category = null;
                category = (Categoria)unitOfWork.Repository<Categoria>().GetById(categoryp);

                return category;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Add(Categoria categoryp)
        {
            try
            {
                unitOfWork.Repository<Categoria>().Add(categoryp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
       
        public bool Delete(Categoria categoryp)
        {
            try
            {
                categoryp.Estado = 0;
                unitOfWork.Repository<Categoria>().Update(categoryp);
                unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Update(Categoria categoryp)
        {
            try
            {
                unitOfWork.Repository<Categoria>().Update(categoryp);
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
