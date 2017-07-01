using IMANA.SIGELIBMA.BLL.Services;
using IMANA.SIGELIBMA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGELIBMA.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            
            return View();
        }


        [HttpGet]
        public JsonResult Init()
        {

            try
            {
                List<Libro> books = GetBooks();
                List<Categoria> categories = GetCategories();
                return Json(new { OperationStatus = true, Categories =categories, Books = books, Message = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }
        

        private List<Libro> GetBooks()
        {
            try 
	        {	        
		        BookService service = new BookService();
                List<Libro> books = service.GetAll();
                return books;
	        }
	        catch (Exception)
	        {
		
		        throw;
	        }
        }

        private List<Categoria> GetCategories()
        {
            try
            {
                BookCatService service = new BookCatService();
                List<Categoria> cats = service.GetAll();
                return cats;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}