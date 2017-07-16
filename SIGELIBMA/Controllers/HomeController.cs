using IMANA.SIGELIBMA.BLL.Servicios;
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
                var books = ObtenerLibros();
                var categories = ObtenerCategorias();
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

        private object ObtenerLibros()
        {
            try 
	        {	        
                
		        LibroServicio service = new LibroServicio();
                List<Libro> books = service.ObtenerTodos();
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

        private object ObtenerCategorias()
        {
            try
            {
                CategoriasLibroServicio service = new CategoriasLibroServicio();
                List<Categoria> cats = service.ObtenerTodos().Where(x => x.Estado == 1).ToList();
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