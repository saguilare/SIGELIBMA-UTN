using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using SIGELIBMA.Filters;
using SIGELIBMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGELIBMA.Controllers
{
    [ValidateSessionFilter]
    public class EntregasController : Controller
    {
        private FacturaServicio servicioFactura = new FacturaServicio();
        private EstadoFacturaServicio servicioEstado = new EstadoFacturaServicio();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerInitData()
        {

            try
            {
                var entregas = Entregas();
                var estados = Estados();
                if (estados != null && entregas != null)
                {
                    return Json(new { EstadoOperacion = true, Entregas = entregas,Estados=estados, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);    
                }
                throw new Exception("No se obtuvo la informacion necesaria para esta pagina.");
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ObtenerEntregas()
        {

            try
            {
                var entregas = Entregas();
                return Json(new { EstadoOperacion = true, Entregas = entregas, Mensaje = "Operacion OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Entregar(EntregaModel entrega) {
            try
            {
                Factura factura = servicioFactura.ObtenerPorId(new Factura { Numero = Convert.ToInt32(entrega.NumeroFactura) });
                if (factura != null)
                {
                    factura.Estado = entrega.Estado;
                    if (servicioFactura.Modificar(factura))
                    {
                        return Json(new { EstadoOperacion = true, Mensaje = "La operacion se efectuo con exito." });
                    }
                    return Json(new { EstadoOperacion = false, Mensaje = "El pedido no se modifico, intente nuevamente" });
                }
                return Json(new { EstadoOperacion = false, Mensaje = "No se encontro el pedido" });
            }
            catch (Exception)
            {
                Response.StatusCode = 400;
                throw;
            }

        }

        private object Estados(){
            var estados = servicioEstado.ObtenerTodos().Select(x=> new {
                codigo = x.Codigo,
                descripcion = x.Descripcion
            });


            return estados;
        }

        private object Entregas() {
            var entregas = servicioFactura.ObtenerTodos().Where(x => x.TipoPago == 3).Select(x => new
            {
                master = new
                {
                    numero = x.Numero,
                    fechaCreacion = x.FechaCreacion.ToString(),
                    fechaCancelacion = x.FechaCancelacion.ToString(),
                    subtotal = x.Subtotal,
                    impuestos = x.Impuestos,
                    total = x.Total,
                    deposito = x.Referencia,
                    estado = x.Estado
                }
                ,
                cliente = new { cedula = x.Usuario.Cedula, nombre = x.Usuario.Nombre + ", " + x.Usuario.Apellido1, telefono = x.Usuario.Telefono, correo = x.Usuario.Correo }
                ,
                deposito = new { numero = x.Deposito.FirstOrDefault().Referencia, fecha = x.Deposito.FirstOrDefault().Fecha.ToString(), bancoEmisor = x.Deposito.FirstOrDefault().BancoEmisor, bancoReceptor = x.Deposito.FirstOrDefault().BancoReceptor }
                ,
                estado = new { codigo = x.EstadoFactura.Codigo, descripcion = x.EstadoFactura.Descripcion }
                ,
                detalles = x.DetalleFactura.Select(d => new
                {
                    cantidad = d.Cantidad,
                    libro = new { codigo = d.Libro.Codigo, titulo = d.Libro.Titulo, autor = d.Libro.Autor1.Nombre + ", " + d.Libro.Autor1.Apellidos, precio = d.Libro.PrecioVentaSinImpuestos, precioIva = d.Libro.PrecioVentaConImpuestos }

                })


            });

            return entregas;
        
        }
    }


    
}