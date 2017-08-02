using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using SIGELIBMA.Filters;
using SIGELIBMA.Models;

namespace SIGELIBMA.Controllers
{   
    
    [ValidateSessionFilter]
    public class ReporteInventarioController : Controller
    {
        InventarioServicio servicioInventario = new InventarioServicio();

        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult InitData()
        {
            try
            {
                List<Inventario> inventario = null;


                inventario = servicioInventario.ObtenerTodos();


                if (inventario != null)
                {
                    var cleanList = inventario.Select(x => new
                    {
                        codigoLibro = x.Libro.Codigo,
                        tituloLibro = x.Libro.Titulo,
                        minimo = x.CantidadMinima,
                        maximo = x.CantidadMaxima,
                        stock = x.CantidadStock
                    });


                    return Json(new { EstadoOperacion = true, Inventario = cleanList, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { EstadoOperacion = true, Inventario = inventario, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
                }




            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Operation FAILED" });
            }
        }

        [HttpPost]
        public JsonResult Inventario(ReporteVentasModelo modelo)
        {
            try
            {
                List<Inventario> inventario = null;


                inventario = servicioInventario.ObtenerTodos();


                if (inventario != null)
                {
                    var cleanList = inventario.Select(x => new
                    {
                        codigoLibro = x.Libro.Codigo,
                        tituloLibro = x.Libro.Titulo,
                        minimo = x.CantidadMinima,
                        maximo = x.CantidadMaxima,
                        stock = x.CantidadStock
                    });


                    return Json(new { EstadoOperacion = true, Inventario = cleanList, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { EstadoOperacion = true, Inventario = inventario, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
                }
                

                
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Operation FAILED" });
            }
        }
    }
}