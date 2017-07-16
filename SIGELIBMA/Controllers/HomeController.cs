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

            ViewBag.Title = "Principal";
            return View();
        }


        [HttpGet]
        public JsonResult Init()
        {

            try
            {
                var books = GetBooks();
                var categories = GetCategories();
                return Json(new { OperationStatus = true, Categories =categories, Books = books, Message = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult ProcessPayment(string param)
        {
            try
            {
              
                return Json(new { OperationStatus = true, ConfirmationCode = "AZ-5456",  Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false,  Message = "Operation FAILED" });
            }
        }

        private object GetBooks()
        {
            try 
	        {	        
                
		        BookService service = new BookService();
                List<Libro> books = service.GetAll();
                //transform and simplify list to avoid circular dependency issues 
                var newList = books.Select(item => new {
                    Codigo = item.Codigo,
                    Autor = item.Autor1.Apellidos + ", " + item.Autor1.Nombre,
                    Precio = item.PrecioVentaConImpuestos,
                    Descripcion = item.Descripcion,
                    Image = item.Imagen,
                    Titulo =item.Titulo
                    
                });


                return newList;
	        }
	        catch (Exception)
	        {
		
		        throw;
	        }
        }

        private object GetCategories()
        {
            try
            {
                BookCatService service = new BookCatService();
                List<Categoria> cats = service.GetAll().Where(x => x.Estado == 1).ToList();
                //remove child elements to avoid circular dependency errors
                var newList = cats.Select(item =>new {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    Libros = item.Libro.Select(libro => new {
                        Codigo = libro.Codigo,
                        Autor = libro.Autor1.Apellidos + ", " + libro.Autor1.Nombre,
                        Precio = libro.PrecioVentaConImpuestos,
                        Descripcion = libro.Descripcion,
                        Image = libro.Imagen,
                        Titulo =libro.Titulo
                    })
                });


                return newList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}