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
    public class MantEstadoCajaController : Controller
    {
        private EstadoCajaServicio estadoServicio = new EstadoCajaServicio();


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

                var estadosCaja = estadoServicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    descripcion = x.Descripcion,
                    estado = x.Estado
                });

                return Json(new { EstadoOperacion = true, EstadosCaja = estadosCaja, Estados = estados, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
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

                var estadosCaja = estadoServicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    descripcion = x.Descripcion,
                    estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, EstadosCaja = estadosCaja, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
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
                EstadoCaja autor = estadoServicio.ObtenerPorId(new EstadoCaja { Codigo = param.Codigo });
                return Json(new { EstadoOperacion = true, EstadoCaja = autor, Mensaje = "Operacion OK" });
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
                resultado = estadoServicio.Desabilitar(new EstadoCaja
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
                resultado = estadoServicio.Modificar(new EstadoCaja
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
                resultado = estadoServicio.Agregar(new EstadoCaja
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