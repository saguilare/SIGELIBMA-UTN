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
                var libros = ObtenerLibros();
                var categorias = ObtenerCategorias();
                return Json(new { EstadoOperacion = true, Categorias =categorias, Libros = libros, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult ProcesarCompra(string param)
        {
            try
            {
              
                return Json(new { EstadoOperacion = true, Confirmacion = "AZ-5456",  Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false,  Mensaje = "Operation FAILED" });
            }
        }

        private object ObtenerLibros()
        {
            try 
	        {	        
                
		        LibroServicio servicio = new LibroServicio();
                List<Libro> libros = servicio.ObtenerTodos();
                //transform and simplify list to avoid circular dependency issues 
                var newList = libros.Select(item => new
                {
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
                CategoriasLibroServicio servicio = new CategoriasLibroServicio();
                List<Categoria> cats = servicio.ObtenerTodos().Where(x => x.Estado == 1).ToList();
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