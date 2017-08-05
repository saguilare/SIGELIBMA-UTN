using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using SIGELIBMA.Filters;
using SIGELIBMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGELIBMA.Controllers
{
    [ValidateSessionFilter]
    [ExceptionFilter]
    public class InventarioController : Controller
    {
        private InventarioServicio servicioInventario = new InventarioServicio();
        private LibroServicio servicioLibro = new LibroServicio();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerInitData()
        {

            try
            {

                List<object> estados = new List<object>();
                estados.Add(new { codigo = 0, descripcion = "Inactivo" });
                estados.Add(new { codigo = 1, descripcion = "Activo" });
                var inventarios = Inventarios();
                var libros = Libros();
                if (inventarios != null)
                {
                    return Json(new { EstadoOperacion = true, 
                                    Inventarios = inventarios, 
                                    Estados = estados,
                                    Libros = libros,
                                    Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
                }
                throw new Exception("No se obtuvo la informacion necesaria para esta pagina.");
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ObtenerInventarios()
        {

            try
            {
                var inv = Inventarios();
                return Json(new { EstadoOperacion = true, Inventarios = inv, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Agregar(InventarioModel inventario)
        {

            try
            {
                if (inventario != null)
                {
                    Inventario inv = new Inventario();
                    inv.CodigoLibro = inventario.Libro;
                    inv.CantidadStock = inventario.Stock;
                    inv.CantidadMinima = inventario.Minimo;
                    inv.CantidadMaxima = inventario.Maximo;
                    inv.Estado = inventario.Estado;
                    if (servicioInventario.Agregar(inv))
                    {
                        return Json(new { EstadoOperacion = true, Mensaje = "La operacion se ejecuto con exito" });        
                    }
                }

                throw new Exception("La operacion no se ejecuto.");
                
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Modificar(InventarioModel inventario)
        {

            try
            {
                if (inventario != null)
                {
                    Inventario inv = servicioInventario.ObtenerPorId(new Inventario{CodigoLibro = inventario.Libro });
                    if (inv != null)
                    {
                        inv.CantidadStock = inventario.Stock;
                        inv.CantidadMinima = inventario.Minimo;
                        inv.CantidadMaxima = inventario.Maximo;
                        inv.Estado = inventario.Estado;
                        if (servicioInventario.Modificar(inv))
                        {
                            return Json(new { EstadoOperacion = true, Mensaje = "La operacion se ejecuto con exito" });    
                        }
                        
                    }
                }

                throw new Exception("La operacion no se ejecuto.");

            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        private object Inventarios() {

            var inventarions = servicioInventario.ObtenerTodos().Select(x => new
            {
                libro = new { codigo = x.Libro.Codigo, titulo = x.Libro.Titulo },
                stock = x.CantidadStock,
                minimo = x.CantidadMinima,
                maximo = x.CantidadMaxima,
                estado = (int)x.Estado == 0 ? new { codigo = 0, descripcion = "Inactivo" } : new { codigo = 1, descripcion = "Activo" }
            });

            return inventarions;
        }

        private object Libros() {
            var libros = servicioLibro.ObtenerTodos().Where(x => x.Estado > 0).Select(x => new{
                codigo = x.Codigo,
                titulo = x.Titulo
            });
            return libros;
        }

        
    }
}