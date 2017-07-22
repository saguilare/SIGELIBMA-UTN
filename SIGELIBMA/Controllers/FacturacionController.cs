
using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using SIGELIBMA.Filters;
using SIGELIBMA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMANA.SIGELIBMA.MVC.Controllers
{
    [ValidateSessionFilter]
    public class FacturacionController : Controller
    {

        private FacturaServicio servicioFactura = new FacturaServicio();
        private CajaServicio servicioCaja = new CajaServicio();
        private int CajaVirtual = Convert.ToInt32(ConfigurationManager.AppSettings["CajaVirtual"]);

        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public JsonResult Init() {
            try
            {
                var libros = ObtenerLibros();
                var cajas = ObtenerCajas();

                return Json(new { EstadoOperacion = true, Libros = libros, Cajas = cajas, Mensaje = "Operacion exitosa" }, JsonRequestBehavior.AllowGet);
   
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        [HttpPost]
        public JsonResult AbrirCerrarCaja(CajaModel caja) {
            try
            {

                if (CambiarEstadoCaja(caja))
                {
                    return Json(new { EstadoOperacion = true, Mensaje = string.Format("Caja {0} inicializada", caja) });
                }
                else
                {
                    return Json(new { EstadoOperacion = false, Mensaje = string.Format("No se pudo inicializar la Caja {0} ", caja) });
                }
                
            }
            catch (Exception ex)
            {
                //TODO
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ProcesarCompra(string param)
        {
            try
            {
                bool resultado = true;
                if (resultado)
                {
                    return Json(new { EstadoOperacion = true,Factura = 456456, Mensaje = "Transaccion Exitosa"});
                }
                else
                {
                    return Json(new { EstadoOperacion = false, Mensaje = "Error: No se registro la factura" });
                }

            }
            catch (Exception ex)
            {
                //TODO
                throw ex;
            }
        }

        private bool CambiarEstadoCaja(CajaModel caja)
        {
            try
            {
                Caja cajaDb = servicioCaja.ObtenerPorId(new Caja { Codigo = caja.Codigo});
                if (cajaDb != null)
                {
                    cajaDb.Estado = caja.Estado;
                    return servicioCaja.Modificar(cajaDb);
                }
                return false;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private object ObtenerLibros()
        {
            try
            {

                LibroServicio servicio = new LibroServicio();
                List<Libro> libros = servicio.ObtenerTodos();
                //transform and simplify list to avoid circular dependency issues 
                var newList = libros.Select(item => new
                {
                    Codigo = item.Codigo,
                    Autor = item.Autor1.Apellidos + ", " + item.Autor1.Nombre,
                    PrecioSinImp = item.PrecioVentaSinImpuestos,
                    Precio = item.PrecioVentaConImpuestos,
                    Descripcion = item.Descripcion,
                    Image = item.Imagen,
                    Titulo = item.Titulo

                });


                return newList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private object ObtenerCategorias()
        {
            try
            {
                CategoriasLibroServicio servicio = new CategoriasLibroServicio();
                List<Categoria> cats = servicio.ObtenerTodos().Where(x => x.Estado == 1).ToList();
                //remove child elements to avoid circular dependency errors
                var newList = cats.Select(item => new
                {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    Libros = item.Libro.Select(libro => new
                    {
                        Codigo = libro.Codigo,
                        Autor = libro.Autor1.Apellidos + ", " + libro.Autor1.Nombre,
                        Precio = libro.PrecioVentaConImpuestos,
                        Descripcion = libro.Descripcion,
                        Image = libro.Imagen,
                        Titulo = libro.Titulo
                    })
                });


                return newList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private object ObtenerCajas()
        {
            try
            {

                List<Caja> cajas = servicioCaja.ObtenerTodos().Where(x => x.Codigo != CajaVirtual && x.Estado != 1).ToList();
                //remove child elements to avoid circular dependency errors
                var newList = cajas.Select(item => new
                {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                });


                return newList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Facturar(Factura factura) {
            bool result = servicioFactura.Agregar(factura);
        }
    }
}