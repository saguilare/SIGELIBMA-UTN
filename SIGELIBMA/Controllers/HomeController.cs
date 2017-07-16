using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.MVC.Controllers;
using SIGELIBMA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGELIBMA.Controllers
{
    public class HomeController : Controller
    {
        private FacturaServicio servicioFactura = new FacturaServicio();
        private UsuarioServicio servicioUsuario = new UsuarioServicio();
        private CajaServicio servicioCaja = new CajaServicio();
        private decimal IVA = Convert.ToDecimal(ConfigurationManager.AppSettings["IVA"]);
        private int CajaVirtual = Convert.ToInt32(ConfigurationManager.AppSettings["CajaVirtual"]);

        // GET: Home
        public ActionResult Index()
        {

            ViewBag.Title = "Principal";
            return View();
        }


        [HttpGet]
        public JsonResult Init()
        {

            try
            {
                var libros = ObtenerLibros();
                var categorias = ObtenerCategorias();
                return Json(new { EstadoOperacion = true, Categorias =categorias, Libros = libros, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult ProcesarCompra(CompraModel compra)
        {
            try
            {
                compra.Cliente.Nombre2 = (compra.Cliente.Nombre1 != "" && compra.Cliente.Nombre1.Contains(" ")) ? compra.Cliente.Nombre1.Split(' ')[1] : "";
                compra.Cliente.Apellido2 = (compra.Cliente.Apellido1 != "" && compra.Cliente.Apellido1.Contains(" ")) ? compra.Cliente.Apellido1.Split(' ')[1] : "";
                CrearFactura(compra);
                

                return Json(new { EstadoOperacion = true, Confirmacion = "AZ-5456",  Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false,  Mensaje = "Operation FAILED" });
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
                    Precio = item.PrecioVentaConImpuestos,
                    Descripcion = item.Descripcion,
                    Image = item.Imagen,
                    Titulo =item.Titulo
                    
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
                var newList = cats.Select(item =>new {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    Libros = item.Libro.Select(libro => new {
                        Codigo = libro.Codigo,
                        Autor = libro.Autor1.Apellidos + ", " + libro.Autor1.Nombre,
                        Precio = libro.PrecioVentaConImpuestos,
                        Descripcion = libro.Descripcion,
                        Image = libro.Imagen,
                        Titulo =libro.Titulo
                    })
                });


                return newList;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private bool CrearFactura(CompraModel compra) {
            try
            {
                

                Factura factura = new Factura();
                factura.Usuario = ObtenerUsuario(compra.Cliente);
                factura.Cliente = factura.Usuario.Cedula;
                factura.Caja = CajaVirtual;
                factura.Caja1 = ObtenerCaja(CajaVirtual);
                factura.FechaCreacion = DateTime.Now;
                factura.FechaCancelacion = null;
                AgregarDetallesFactura(compra.Productos,ref factura);
                CalcularMontosFactura(ref factura);
                factura.Estado = 2;
         
      
                return  servicioFactura.Agregar(factura);

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private Usuario ObtenerUsuario(ClienteModel clientep) {

            try
            {
                Usuario cliente = servicioUsuario.ObtenerPorId(new Usuario { Cedula = clientep.Cedula });
                if (cliente == null)
	            {
                    Usuario usuario = new Usuario {
                        Usuario1 = clientep.Nombre1[0] + clientep.Apellido1 + clientep.Apellido2[0],
                        Clave = "",
                        Cedula = clientep.Cedula,
                        Nombre = clientep.Nombre1,
                        Segundo_Nombre = clientep.Nombre2,
                        Apellido1 = clientep.Apellido1,
                        Apellido2 = clientep.Apellido2,
                        Correo = clientep.Email

                    };

	            }

                return cliente;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private Caja ObtenerCaja(int id) {
            try
            {
                return servicioCaja.ObtenerPorId(new Caja{Codigo = id});
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void AgregarDetallesFactura(List<ProductoModel> productos,ref Factura factura)
        {
            try
            {
                List<DetalleFactura> detalles = new List<DetalleFactura>();
                foreach (ProductoModel prod in productos)
                {
                    DetalleFactura detalle = new DetalleFactura { Articulo = prod.Codigo, Cantidad = prod.Cantidad };
                    detalles.Add(detalle);
                }

                factura.DetalleFactura = detalles;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void CalcularMontosFactura( ref Factura factura)
        {
            try
            {
                foreach (DetalleFactura detalle in factura.DetalleFactura)
                {
                    factura.Impuestos += ((detalle.Cantidad * detalle.Libro.PrecioVentaSinImpuestos) * 13) /100;
                    factura.Subtotal += detalle.Cantidad * detalle.Libro.PrecioVentaSinImpuestos;
                    
                    factura.Pendiente = 0;
                }
                factura.Total += factura.Subtotal + factura.Impuestos;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}