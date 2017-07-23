﻿
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
        private UsuarioServicio servicioUsuario = new UsuarioServicio();
        private CajaServicio servicioCaja = new CajaServicio();
        private MovimientoCajaServicio servicioMovimientos = new MovimientoCajaServicio();
        private LibroServicio servicioLibro = new LibroServicio();
        private TipoPagoServicio servicioTipoPago = new TipoPagoServicio();
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
                var tipospago = ObtenerTiposPago();
                if (libros == null ||  cajas== null || tipospago== null)  
                {
                    throw new Exception("No se cuenta con la informacion necesaria para esta pagina, vuelva a recargar."); 
                }
                return Json(new { EstadoOperacion = true, Libros = libros, Cajas = cajas,TiposPago = tipospago, Mensaje = "Operacion exitosa" }, JsonRequestBehavior.AllowGet);
   
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
        public JsonResult ProcesarCompra(FacturaModel fatura)
        {
            try
            {
       

                fatura.Cliente.Nombre2 = (fatura.Cliente.Nombre1 != "" && fatura.Cliente.Nombre1.Contains(" ")) ? fatura.Cliente.Nombre1.Split(' ')[1] : "";
                fatura.Cliente.Apellido2 = (fatura.Cliente.Apellido1 != "" && fatura.Cliente.Apellido1.Contains(" ")) ? fatura.Cliente.Apellido1.Split(' ')[1] : "";
                Factura factura = CrearFactura(fatura);
                if (factura != null && factura.Numero > 0)
                {
                    return Json(new { EstadoOperacion = true, Factura = factura.Numero, Mensaje = "Transaccion Exitosa" });
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

        private object ObtenerTiposPago()
        {
            try
            {

                List<TipoPago> tipos = servicioTipoPago.ObtenerTodos();
                //remove child elements to avoid circular dependency errors
                var newList = tipos.Select(item => new
                {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                   
                });


                return newList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CambiarEstadoCaja(CajaModel caja)
        {
            try
            {
                Caja cajaDb = servicioCaja.ObtenerPorId(new Caja { Codigo = caja.Codigo });
                if (cajaDb != null)
                {
                    cajaDb.Estado = caja.Estado;
                    if (servicioCaja.Modificar(cajaDb))
                    {
                        MovimientoCaja movimiento = new MovimientoCaja();
                        movimiento.Descripcion = cajaDb.Estado == 1 ? "Apertura" : "Cierre";
                        movimiento.Caja = cajaDb.Codigo;
                        movimiento.Fecha = DateTime.Now;
                        movimiento.Monto = Convert.ToDecimal(caja.Monto);
                        movimiento.Tipo = cajaDb.Estado == 1 ? 1 : 2;
                        servicioMovimientos.Agregar(movimiento);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        [HttpPost]
        public JsonResult RetirarAbonarCaja(CajaModel caja )
        {
            try
            {
                Caja cajaDb = servicioCaja.ObtenerPorId(new Caja { Codigo = caja.Codigo });
                if (cajaDb != null)
                {
                    MovimientoCaja movimiento = new MovimientoCaja();
                    movimiento.Descripcion = caja.Razon;
                    movimiento.Caja = cajaDb.Codigo;
                    movimiento.Fecha = DateTime.Now;
                    movimiento.Monto = Convert.ToDecimal(caja.Monto);
                    movimiento.Tipo = caja.Movimiento == 1 ? 4 : 3;
                    if (servicioMovimientos.Agregar(movimiento)) 
                    {
                        return Json(new { EstadoOperacion = true, Mensaje = "Operacion Exitosa" });
                    }
                    return Json(new { EstadoOperacion = false, Mensaje = "La operacion no se completo" });
                }
                else
                {
                    throw new Exception("Parametros invalidos/incompletos");
                }
                
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

                List<Caja> cajas = servicioCaja.ObtenerTodos().Where(x => x.Codigo != CajaVirtual && x.Estado == 2).ToList();
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

        [HttpPost]
        public JsonResult MovimientosCaja(CajaModel caja)
        {
            try
            {
                
                List<MovimientoCaja> movimientos = servicioMovimientos.ObtenerTodos().Where(x => x.Caja == caja.Codigo && x.Fecha.Date == DateTime.Today.Date).ToList();
                //remove child elements to avoid circular dependency errors
                var newList = movimientos.Select(item => new
                {
                    Fecha = item.Fecha.ToString(),
                    Descripcion = item.Descripcion,
                    Monto = item.Monto,
                    Tipo = item.TipoMovimientoCaja.Descripcion
                });
                
                return Json(new { EstadoOperacion = true, Movimientos = newList, Mensaje = "Operacion Exitosa" });
              
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void Facturar(Factura factura) {
            bool result = servicioFactura.Agregar(factura);
        }

        private Factura CrearFactura(FacturaModel compra)
        {
            try
            {
                Sesion sesion = new Sesion();
                sesion.Inicio = DateTime.Now;

                Factura factura = new Factura();
                factura.Cliente = ObtenerUsuario(compra.Cliente).Cedula;
                factura.Caja = ObtenerCaja(compra.Caja).Codigo;
                factura.FechaCreacion = DateTime.Now;
                factura.FechaCancelacion = DateTime.Now;
                AgregarDetallesFactura(compra.Productos, ref factura);
                CalcularMontosFactura(ref factura);
                factura.Estado = 2;
                factura.TipoPago = compra.TipoPago.Codigo;
                factura.Referencia = compra.Referencia;
                servicioFactura.Agregar(factura);
                

                //sesion.Usuario = factura.Cliente;
                //sesion.Finalizacion = DateTime.Now;
                //servicioSesion.Agregar(sesion);

                //Transaccion tx = new Transaccion();
                //tx.Tipo = 1;
                //tx.Sesion = sesion.Id;
                //tx.Tabla = "Login";
                //tx.TuplaAnterior = "";
                //tx.TuplaNueva = "";

                //servicioTransaccion.Agregar(tx);

                //tx.Tipo = 2;
                //tx.Sesion = sesion.Id;
                //tx.Tabla = "Login";
                //tx.TuplaAnterior = "";
                //tx.TuplaNueva = "";

                //servicioTransaccion.Agregar(tx);


                return factura;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private Usuario ObtenerUsuario(ClienteModel clientep)
        {

            try
            {
                Usuario cliente = servicioUsuario.ObtenerPorId(new Usuario { Cedula = clientep.Cedula });
                if (cliente == null)
                {
                    cliente = new Usuario
                    {
                        Usuario1 = clientep.Nombre1[0] + clientep.Apellido1 + (clientep.Apellido2 != "" ? clientep.Apellido2[0].ToString() : ""),
                        Clave = "",
                        Cedula = clientep.Cedula,
                        Nombre = clientep.Nombre1,
                        Segundo_Nombre = clientep.Nombre2,
                        Apellido1 = clientep.Apellido1,
                        Apellido2 = clientep.Apellido2,
                        Correo = clientep.Email,
                        Estado = 1,
                        Telefono = clientep.Telefono

                    };
                    servicioUsuario.Agregar(cliente);
                }

                return cliente;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private Caja ObtenerCaja(int id)
        {
            try
            {
                return servicioCaja.ObtenerPorId(new Caja { Codigo = id });
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void AgregarDetallesFactura(List<ProductoModel> productos, ref Factura factura)
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

        private void CalcularMontosFactura(ref Factura factura)
        {
            try
            {
                foreach (DetalleFactura detalle in factura.DetalleFactura)
                {
                    Libro libro = servicioLibro.ObtenerPorId(new Libro { Codigo = detalle.Articulo });
                    factura.Impuestos += ((detalle.Cantidad * libro.PrecioVentaSinImpuestos) * 13) / 100;
                    factura.Subtotal += detalle.Cantidad * libro.PrecioVentaSinImpuestos;

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