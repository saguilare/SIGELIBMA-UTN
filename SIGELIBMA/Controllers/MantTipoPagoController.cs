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
    public class MantTipoPagoController : Controller
    {
        private TipoPagoServicio servicio = new TipoPagoServicio();


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Estados Caja";
            return View();
        }


        [HttpGet]
        public JsonResult ObtenerInitData()
        {

            try
            {
                List<object> estados = new List<object>();
                estados.Add(new { codigo = 1, descripcion = "Activo" });
                estados.Add(new { codigo = 0, descripcion = "Inactivo" });

                var tipos = servicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    descripcion = x.Descripcion,
                    estado = x.Estado
                });

                return Json(new { EstadoOperacion = true, Tipos = tipos, Estados = estados, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
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

                var tipos = servicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    descripcion = x.Descripcion,
                    estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Tipos = tipos, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObtenerPorId(TipoPagoModel param)
        {
            try
            {
                TipoPago tipo = servicio.ObtenerPorId(new TipoPago { Codigo = param.Codigo });
                return Json(new { EstadoOperacion = true, Tipo = tipo, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Desabilitar(TipoPagoModel param)
        {
            try
            {
                bool resultado = false;
                resultado = servicio.Desabilitar(new TipoPago
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
        public JsonResult Modificar(TipoPagoModel param)
        {
            try
            {
                bool resultado = false;
                resultado = servicio.Modificar(new TipoPago
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
        public JsonResult Agregar(TipoPagoModel param)
        {
            try
            {
                bool resultado = false;
                resultado = servicio.Agregar(new TipoPago
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