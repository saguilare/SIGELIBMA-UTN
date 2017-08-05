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
    public class MantCategoriaController : Controller
    {
        private CategoriasLibroServicio catServicio = new CategoriasLibroServicio();


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Categorias";
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

                var categorias = catServicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    descripcion = x.Descripcion,
                    estado = x.Estado
                });

                return Json(new { EstadoOperacion = true, Categorias = categorias, Estados = estados, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
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

                var categorias = catServicio.ObtenerTodos().Select(x => new
                {
                    codigo = x.Codigo,
                    descripcion = x.Descripcion,
                    estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Categorias = categorias, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObtenerPorId(CategoriaModel catp)
        {
            try
            {
                Categoria autor = catServicio.ObtenerPorId(new Categoria { Codigo = catp.Codigo });
                return Json(new { EstadoOperacion = true, Categoria = autor, Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Desabilitar(CategoriaModel catp)
        {
            try
            {
                bool resultado = false;
                resultado = catServicio.Desabilitar(new Categoria
                {
                    Codigo = catp.Codigo,
                    Descripcion = catp.Descripcion,
                    Estado = catp.Estado
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
        public JsonResult Modificar(CategoriaModel catp)
        {
            try
            {
                bool resultado = false;
                resultado = catServicio.Modificar(new Categoria
                {
                    Codigo = catp.Codigo,
                    Descripcion = catp.Descripcion,
                    Estado = catp.Estado
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
        public JsonResult Agregar(CategoriaModel catp)
        {
            try
            {
                bool resultado = false;
                resultado = catServicio.Agregar(new Categoria
                {
                    Codigo = catp.Codigo,
                    Descripcion = catp.Descripcion,
                    Estado = catp.Estado
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