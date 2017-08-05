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
    public class MantProveedorController : Controller
    {
        private ProveedorServicio proveedorServicio = new ProveedorServicio();


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Proveedores";
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

                var proveedores = proveedorServicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    nombre = x.Nombre,
                    telefono = x.Telefono,
                    correo = x.Correo,
                    estado = x.Estado
                });

                return Json(new { EstadoOperacion = true, Proveedores = proveedores, Estados = estados, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
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

                var proveedores = proveedorServicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    nombre = x.Nombre,
                    telefono = x.Telefono,
                    correo = x.Correo,
                    estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Proveedores = proveedores, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObtenerPorId(ProveedorModel proveedorp)
        {
            try
            {
                Proveedor proveedor = proveedorServicio.ObtenerPorId(new Proveedor { Codigo = proveedorp.Codigo });
                return Json(new { EstadoOperacion = true, Proveedor = proveedor, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Desabilitar(ProveedorModel proveedorp)
        {
            try
            {
                bool resultado = false;
                resultado = proveedorServicio.Desabilitar(new Proveedor
                {
                    Codigo = proveedorp.Codigo,
                    Nombre = proveedorp.Nombre,
                    Telefono = proveedorp.Telefono,
                    Correo = proveedorp.Correo,
                    Estado = proveedorp.Estado
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
        public JsonResult Modificar(ProveedorModel proveedorp)
        {
            try
            {
                bool resultado = false;
                resultado = proveedorServicio.Modificar(new Proveedor
                {
                    Codigo = proveedorp.Codigo,
                    Nombre = proveedorp.Nombre,
                    Telefono = proveedorp.Telefono,
                    Correo = proveedorp.Correo,
                    Estado = proveedorp.Estado
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
        public JsonResult Agregar(ProveedorModel proveedorp)
        {
            try
            {
                bool resultado = false;
                resultado = proveedorServicio.Agregar(new Proveedor
                {
                    Codigo = proveedorp.Codigo,
                    Nombre = proveedorp.Nombre,
                    Telefono = proveedorp.Telefono,
                    Correo = proveedorp.Correo,
                    Estado = proveedorp.Estado
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