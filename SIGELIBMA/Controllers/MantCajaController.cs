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
    public class MantCajaController : Controller
    {
        private CajaServicio servicio = new CajaServicio();


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Caja";
            return View();
        }


        [HttpGet]
        public JsonResult ObtenerInitData()
        {

            try
            {
                List<object> estados = new List<object>();
                estados.Add(new { codigo = 1, descripcion = "Abierta" });
                estados.Add(new { codigo = 2, descripcion = "Cerrada" });

                var cajas = servicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    descripcion = x.Descripcion,
                    estado = x.Estado
                });

                return Json(new { EstadoOperacion = true, Cajas = cajas, Estados = estados, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ObtenerTodos()
        {

            try
            {

                var cajas = servicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    descripcion = x.Descripcion,
                    estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Cajas = cajas, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObtenerPorId(EstadoModel param)
        {
            try
            {
                Caja caja = servicio.ObtenerPorId(new Caja { Codigo = param.Codigo });
                return Json(new { EstadoOperacion = true, Caja = caja, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Desabilitar(EstadoModel param)
        {
            try
            {
                bool resultado = false;
                resultado = servicio.Desabilitar(new Caja
                {
                    Codigo = param.Codigo,
                    Descripcion = param.Descripcion,
                    Estado = param.Estado
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
        public JsonResult Modificar(EstadoModel param)
        {
            try
            {
                bool resultado = false;
                resultado = servicio.Modificar(new Caja
                {
                    Codigo = param.Codigo,
                    Descripcion = param.Descripcion,
                    Estado = param.Estado
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
        public JsonResult Agregar(EstadoModel param)
        {
            try
            {
                bool resultado = false;
                resultado = servicio.Agregar(new Caja
                {
                    Codigo = param.Codigo,
                    Descripcion = param.Descripcion,
                    Estado = param.Estado
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