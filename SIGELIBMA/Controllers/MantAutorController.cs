using IMANA.SIGELIBMA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMANA.SIGELIBMA.BLL.Servicios;
using SIGELIBMA.Filters;
using SIGELIBMA.Models;

namespace SIGELIBMA.Controllers
{
    [ValidateSessionFilter]
    [ExceptionFilter]
    public class MantAutorController : Controller
    {
        private AutorServicio autorServicio = new AutorServicio();


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Roles";
            return View();
        }


        [HttpGet]
        public JsonResult ObtenerInitData()
        {

            try
            {
                List<object> estados = new List<object>();
                estados.Add(new {codigo=1,descripcion="Activo" });
                estados.Add(new { codigo = 0, descripcion = "Inactivo" });

                var autores = autorServicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    nombre = x.Nombre,
                    apellidos = x.Apellidos,
                    estado = x.Estado
                });

                return Json(new { EstadoOperacion = true,Autores= autores, Estados = estados, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ObtenerTodos() {

            try
            {

                var autores = autorServicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    nombre = x.Nombre,
                    apellidos = x.Apellidos,
                    estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Autores = autores, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObtenerPorId(AutorModel autorp)
        {
            try
            {
                Autor autor = autorServicio.ObtenerPorId(new Autor {Codigo = autorp.Codigo });
                return Json(new { EstadoOperacion = true, Autor = autor, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Desabilitar(AutorModel autorp)
        {
            try
            {
                bool resultado = false;
                resultado = autorServicio.Desabilitar(new Autor { Codigo = autorp.Codigo,
                                                                  Nombre = autorp.Nombre,
                                                                  Apellidos = autorp.Apellidos,
                                                                  Estado = autorp.Estado
                                                      });
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Modificar(AutorModel autorp)
        {
            try
            {
                bool resultado = false;
                resultado = autorServicio.Modificar(new Autor
                {
                    Codigo = autorp.Codigo,
                    Nombre = autorp.Nombre,
                    Apellidos = autorp.Apellidos,
                    Estado = autorp.Estado
                });
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Agregar(AutorModel autorp)
        {
            try
            {
                bool resultado = false;
                resultado = autorServicio.Agregar(new Autor
                {
                    Codigo = autorp.Codigo,
                    Nombre = autorp.Nombre,
                    Apellidos = autorp.Apellidos,
                    Estado = autorp.Estado
                });
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}