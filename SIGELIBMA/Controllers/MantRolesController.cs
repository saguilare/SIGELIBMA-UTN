using IMANA.SIGELIBMA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMANA.SIGELIBMA.BLL.Servicios;

namespace SIGELIBMA.Controllers
{
    public class MantRolesController : Controller
    {
        private RolServicio rolServicio = new RolServicio();


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
                var Roles =  rolServicio.ObtenerTodos().Select(x => new
                { 
                                    Codigo = x.Codigo, 
                                    Descripcion = x.Descripcion, 
                                    Estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Roles = Roles, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
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

                var Roles = rolServicio.ObtenerTodos().Select(x => new
                {
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Roles = Roles, Mensaje = "Operacion OK" },JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObtenerPorId(Rol rolp)
        {
            try
            {
                Rol rol = rolServicio.ObtenerPorId(rolp);
                return Json(new { EstadoOperacion = true, Rol = rol, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Desabilitar(Rol rolp)
        {
            try
            {
                bool resultado = false;
                resultado = rolServicio.Desabilitar(rolp);
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
        public JsonResult Modificar(Rol rolp)
        {
            try
            {
                bool resultado = false;
                resultado = rolServicio.Modificar(rolp);
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
        public JsonResult Agregar(Rol rolp)
        {
            try
            {
                bool resultado = false;
                resultado = rolServicio.Agregar(rolp);
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