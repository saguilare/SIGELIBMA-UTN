using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using SIGELIBMA.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGELIBMA.Controllers
{
    [ValidateSessionFilter]
    [ExceptionFilter]
    public class LibroController : Controller
    {
        LibroServicio LibroServicio = new LibroServicio();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Libros";
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerTodos()
        {

            try
            {
                List<Libro> libros = LibroServicio.ObtenerTodos();
                return Json(new { EstadoOperacion = true, Libros = libros, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObtenerPorId(Libro librop)
        {
            try
            {
                Libro libro = LibroServicio.ObtenerPorId(librop);
                return Json(new { EstadoOperacion = true, Libro = libro, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Desabilitar(Libro librop)
        {
            try
            {
                bool resultado = false;
                resultado = LibroServicio.Desabilitar(librop);
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Modificar(Libro librop)
        {
            try
            {
                bool resultado = false;
                resultado = LibroServicio.Modificar(librop);
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Agregar(Libro librop)
        {
            try
            {
                bool resultado = false;
                resultado = LibroServicio.Agregar(librop);
                return Json(new { EstadoOperacion =  resultado, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }
    }
}