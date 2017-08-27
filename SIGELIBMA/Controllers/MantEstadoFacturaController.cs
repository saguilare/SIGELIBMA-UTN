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
    public class MantEstadoFacturaController : Controller
    {
        private EstadoFacturaServicio servicio = new EstadoFacturaServicio();


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Estados Factura";
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
                Response.StatusCode = 400;
                throw e;
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
                Response.StatusCode = 400;
                throw e;
            }
        }

        [HttpPost]
        public JsonResult ObtenerPorId(EstadoModel param)
        {
            try
            {
                EstadoFactura tipo = servicio.ObtenerPorId(new EstadoFactura { Codigo = param.Codigo });
                return Json(new { EstadoOperacion = true, Tipo = tipo, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }

        [HttpPost]
        public JsonResult Desabilitar(EstadoModel param)
        {
            try
            {
                bool resultado = false;
                resultado = servicio.Desabilitar(new EstadoFactura
                {
                    Codigo = param.Codigo,
                    Descripcion = param.Descripcion,
                    Estado = param.Estado
                });
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }

        [HttpPost]
        public JsonResult Modificar(EstadoModel param)
        {
            try
            {
                bool resultado = false;
                resultado = servicio.Modificar(new EstadoFactura
                {
                    Codigo = param.Codigo,
                    Descripcion = param.Descripcion,
                    Estado = param.Estado
                });
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }

        [HttpPost]
        public JsonResult Agregar(EstadoModel param)
        {
            try
            {
                bool resultado = false;
                resultado = servicio.Agregar(new EstadoFactura
                {
                    Codigo = param.Codigo,
                    Descripcion = param.Descripcion,
                    Estado = param.Estado
                });
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }


    }
}