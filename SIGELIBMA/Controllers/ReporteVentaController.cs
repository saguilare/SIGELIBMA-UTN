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
    [ExceptionFilter]
    public class ReporteVentaController : Controller
    {
        FacturaServicio servicioFactura = new FacturaServicio();

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
                List<Factura> facturas = null;
                
            
                facturas = servicioFactura.ObtenerTodos().Where(x => x.FechaCreacion.Year == DateTime.Today.Year).ToList();
                List<object> Filtros = new List<object>();
                Filtros.Add(new { codigo = 1, descripcion = "Año" });
                Filtros.Add(new { codigo = 2, descripcion = "Rango" });
               
                if (facturas != null)
                {
                    var cleanList = facturas.Select(x => new
                    {
                        numero = x.Numero,
                        fecha = x.FechaCreacion.ToString(),
                        tipo = x.TipoPago1.Descripcion,
                        caja = x.Caja1.Descripcion,
                        subtotal = x.Subtotal,
                        total = x.Total,
                        impuestos = x.Impuestos
                    });

                    decimal total = 0; decimal subtotal = 0; decimal impuestos = 0;
                    foreach (var venta in cleanList)
                    {
                        total += venta.total;
                        subtotal += venta.subtotal;
                        impuestos += venta.impuestos;
                    }

                    var totales = new { total = total, subtotal = subtotal, impuestos = impuestos };
                    return Json(new { EstadoOperacion = true, Ventas = cleanList, Totales = totales, Filtros = Filtros, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { EstadoOperacion = true, Ventas = facturas, Totales = 0, Filtros = Filtros, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
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
        public JsonResult Ventas(ReporteVentasModelo modelo)
        {
            try
            {
                List<Factura> facturas = null;
                //Filtro == 1 busqueda por anno
                if (modelo.Filtro == 1)
                {
                    facturas = servicioFactura.ObtenerTodos().Where(x => x.FechaCreacion.Year == modelo.FechaInicio.Year).ToList();
                }
                //filtro 2 == range defult date == current year
                else
                {
                    facturas = servicioFactura.ObtenerTodos().Where(x => x.FechaCreacion.Date >= modelo.FechaInicio.Date && x.FechaCreacion.Date <= modelo.FechaFinal.Date).ToList();
                }

                if (facturas != null)
                {
                    var cleanList = facturas.Select(x => new { 
                        numero = x.Numero,
                        fecha = x.FechaCreacion.ToString(),
                        tipo = x.TipoPago1.Descripcion,
                        caja = x.Caja1.Descripcion,
                        subtotal = x.Subtotal,
                        total = x.Total,
                        impuestos = x.Impuestos
                    });

                    decimal total = 0; decimal subtotal = 0; decimal impuestos = 0;
                    foreach (var venta in cleanList)
                    {
                        total += venta.total;
                        subtotal += venta.subtotal;
                        impuestos += venta.impuestos;
                    }

                    var totales = new { total = total, subtotal = subtotal, impuestos = impuestos };
                    return Json(new { EstadoOperacion = true, Ventas = cleanList, Totales = totales, Mensaje = "Operation OK" });
                }
                else
                {
                    return Json(new { EstadoOperacion = true, Ventas = facturas,Totales =0, Mensaje = "Operation OK" });
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