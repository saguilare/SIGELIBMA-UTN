
using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMANA.SIGELIBMA.MVC.Controllers
{
    public class FacturacionController : Controller
    {
        public ActionResult Index()
        {
            //if (System.Web.HttpContext.Current.Session["session"] != null)
            //{ 
            //    Sesion session = System.Web.HttpContext.Current.Session["session"] as Sesion;
            //    if (session != null)
            //    {
            //        ViewBag.Title = "Facturacion";
            //        return View();
            //    }
            //}
            //return RedirectToAction("Login", "Login");

            return View();
        }

        [HttpGet]
        public JsonResult Init() {
            try
            {
                var libros = ObtenerLibros();
                //var categories = ObtenerCategorias();

                return Json(new { EstadoOperacion = true, Libros = libros, Mensaje = "Operacion exitosa" }, JsonRequestBehavior.AllowGet);
   
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        [HttpPost]
        public JsonResult AbrirCaja(string param) {
            try
            {
                string caja = "45";
                if (AbrirCaja())
                {
                    return Json(new { EstadoOperacion = true, Mensaje = string.Format("Caja {0} inicializada", caja) });
                }
                else
                {
                    return Json(new { EstadoOperacion = false, Mensaje = string.Format("No se pudo inicializar la Caja {0} ", caja) });
                }
                
            }
            catch (Exception ex)
            {
                //TODO
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ProcesarCompra(string param)
        {
            try
            {
                bool resultado = true;
                if (resultado)
                {
                    return Json(new { EstadoOperacion = true,Factura = 456456, Mensaje = "Transaccion Exitosa"});
                }
                else
                {
                    return Json(new { EstadoOperacion = false, Mensaje = "Error: No se registro la factura" });
                }

            }
            catch (Exception ex)
            {
                //TODO
                throw ex;
            }
        }

        private bool AbrirCaja() {
            return true;
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
                    Titulo = item.Titulo

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
                var newList = cats.Select(item => new
                {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    Libros = item.Libro.Select(libro => new
                    {
                        Codigo = libro.Codigo,
                        Autor = libro.Autor1.Apellidos + ", " + libro.Autor1.Nombre,
                        Precio = libro.PrecioVentaConImpuestos,
                        Descripcion = libro.Descripcion,
                        Image = libro.Imagen,
                        Titulo = libro.Titulo
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