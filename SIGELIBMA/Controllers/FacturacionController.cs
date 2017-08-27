
using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using SIGELIBMA.Filters;
using SIGELIBMA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IMANA.SIGELIBMA.MVC.Controllers
{
    [ValidateSessionFilter]
    [ExceptionFilter]
    public class FacturacionController : Controller
    {

        private FacturaServicio servicioFactura = new FacturaServicio();
        private UsuarioServicio servicioUsuario = new UsuarioServicio();
        private CajaServicio servicioCaja = new CajaServicio();
        private MovimientoCajaServicio servicioMovimientos = new MovimientoCajaServicio();
        private LibroServicio servicioLibro = new LibroServicio();
        private TipoPagoServicio servicioTipoPago = new TipoPagoServicio();
        private CajaUsuarioServicio servicioCajaUsuario = new CajaUsuarioServicio();
        private int CajaVirtual = Convert.ToInt32(ConfigurationManager.AppSettings["CajaVirtual"]);
      
        public ActionResult Index()
        {

            return View();
        }

        
        public ActionResult ImprimirFactura(int factura)
        {
            Factura fact = servicioFactura.ObtenerPorId(new Factura { Numero = factura });
            ViewBag.Fecha = DateTime.Now.ToString();
            return View(fact);
        }

        [HttpGet]
        public JsonResult Init() {
            try
            {
                var libros = ObtenerLibros();
                var cajas = ObtenerCajas();
                var tipospago = ObtenerTiposPago();
                Usuario user = null;
                CajaModel caja = null;
                int sesionCaja = 0;

                if (Session != null && Session["SesionSistema"] != null)
                {
                    SesionModel sesion = Session["SesionSistema"] as SesionModel;
                    user = sesion.Usuario;
                    sesionCaja = sesion.SesionCaja;
                }
                
                if (sesionCaja > 0)
                {
                    CajaUsuario cu = servicioCajaUsuario.ObtenerPorId(new CajaUsuario { Sesion = sesionCaja });
                    caja = new CajaModel();
                    caja.Sesion = cu.Sesion;
                    caja.Codigo = cu.Caja1.Codigo;
                    caja.Estado = cu.Caja1.Estado;
                }
                
                
                if (libros == null ||  cajas== null || tipospago== null || user == null)  
                {
                    throw new Exception("No se cuenta con la informacion necesaria para esta pagina, vuelva a recargar."); 
                }
                return Json(new { EstadoOperacion = true, Libros = libros, Cajas = cajas,Caja = caja,TiposPago = tipospago, Mensaje = "Operacion exitosa" }, JsonRequestBehavior.AllowGet);
   
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
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
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }
       
        [HttpPost]
        public JsonResult ProcesarCompra(FacturaModel fatura)
        {
            try
            {

                var agotados = VerificarInvetario(fatura.Productos);
                if (agotados != null)
                {
                    return Json(new { EstadoOperacion = false, Agotados = agotados, Confirmacion = "", Mensaje = "La factura no se proceso, no hay articulos en existencia" });
                }

                fatura.Cliente.Nombre2 = (fatura.Cliente.Nombre1 != "" && fatura.Cliente.Nombre1.Contains(" ")) ? fatura.Cliente.Nombre1.Split(' ')[1] : "";
                fatura.Cliente.Apellido2 = (fatura.Cliente.Apellido1 != "" && fatura.Cliente.Apellido1.Contains(" ")) ? fatura.Cliente.Apellido1.Split(' ')[1] : "";
                Factura factura = CrearFactura(fatura);
                if (factura != null && factura.Numero > 0)
                {
                    SendEmail(factura);
                    return Json(new { EstadoOperacion = true, Factura = factura.Numero, Mensaje = "Transaccion Exitosa" });
                }
                else
                {
                    return Json(new { EstadoOperacion = false, Mensaje = "Error: No se registro la factura" });
                }

            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }
     
        [HttpPost]
        public JsonResult BuscarCedula(ClienteModel cliente)
        {
            try
            {
                Usuario user = servicioUsuario.ObtenerPorId(new Usuario { Cedula = cliente.Cedula });
                if (user != null)
                {
                    var usuarioLimpio = new
                    {
                        Cedula = user.Cedula,
                        Nombre = user.Segundo_Nombre != "" ? user.Nombre + " " + user.Segundo_Nombre : user.Nombre,
                        Apellidos = user.Apellido2 != "" ? user.Apellido1 + " " + user.Apellido2 : user.Apellido1,
                        Correo = user.Correo,
                        Telefono = user.Telefono
                    };
                    return Json(new { EstadoOperacion = true, Usuario = usuarioLimpio, Mensaje = "Operacion OK" });
                }


                return Json(new { EstadoOperacion = false, Usuario = "", Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }
     
        [HttpPost]
        public JsonResult MovimientosCaja(CajaModel caja)
        {
            try
            {
                SesionModel sesion = Session["SesionSistema"] as SesionModel;
                int sesionCaja = sesion.SesionCaja;
                List<MovimientoCaja> movimientos = servicioMovimientos.ObtenerTodos().Where(x => x.Caja == caja.Codigo && x.Fecha.Date == DateTime.Today).ToList();
                //remove child elements to avoid circular dependency errors
                var newList = movimientos.Select(item => new
                {
                    Fecha = item.Fecha.ToString(),
                    Descripcion = item.Descripcion,
                    Monto = item.Monto,
                    Tipo = new { Codigo = item.TipoMovimientoCaja.Codigo, Descripcion = item.TipoMovimientoCaja.Descripcion }
                });

                return Json(new { EstadoOperacion = true, Movimientos = newList, Mensaje = "Operacion Exitosa" });

            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }

        [HttpPost]
        public JsonResult RetirarAbonarCaja(CajaModel caja)
        {
            try
            {
                SesionModel sesion = Session["SesionSistema"] as SesionModel;
                int sesionCaja = sesion.SesionCaja;
                Caja cajaDb = servicioCaja.ObtenerPorId(new Caja { Codigo = caja.Codigo });
                if (cajaDb != null)
                {
                    MovimientoCaja movimiento = new MovimientoCaja();
                    movimiento.Descripcion = caja.Razon;
                    movimiento.Caja = cajaDb.Codigo;
                    movimiento.Fecha = DateTime.Now;
                    movimiento.Monto = Convert.ToDecimal(caja.Monto);
                    movimiento.MontoReal = Convert.ToDecimal(caja.MontoReal);
                    movimiento.Tipo = caja.Movimiento == 1 ? 4 : 3;
                    movimiento.SesionId = sesionCaja;
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
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }


        private bool SendEmail(Factura factura)
        {

            try
            {
               
                factura.Usuario = servicioUsuario.ObtenerPorId(new Usuario { Cedula = factura.Cliente });
                foreach (DetalleFactura item in factura.DetalleFactura)
                {
                    item.Libro = servicioLibro.ObtenerPorId(new Libro { Codigo = item.Articulo });
                }
                string from = ConfigurationManager.AppSettings["from"];
                var fromAddress = new MailAddress(from, "Librería Iglesia Mana");
                var toAddress = new MailAddress(factura.Usuario.Correo, factura.Usuario.Correo);
                string fromPassword = ConfigurationManager.AppSettings["password"];
                string subject = "Confirmación Compra - #" + factura.Numero;
                StringBuilder str = new StringBuilder();
                str.AppendLine("Estimado(a): " + factura.Usuario.Nombre);
                str.AppendLine("<br/>");
                str.AppendLine("Adjuntamos el detalle de su compra, le recordamos que puede pasar a retirar su producos de Lunes a Viernes de 8am a 7pm");
                str.AppendLine("<br/><br/>");
                str.AppendLine("<style>td{border-bottom: 1px solid black;}</style>");
                str.AppendLine("<div style='min-width: 300px'>");
                str.AppendLine(" <table >");
                str.AppendLine("<thead>");
                str.AppendLine("<tr>");
                str.AppendLine("<th><a>Código</a></th><th><a>Titulo</a></th><th><a>Cantidad</a></th><th><a>Precio Unitario</a></th><th><a>Precio Total</a></th>");
                str.AppendLine("</tr>");
                str.AppendLine("</thead>");
                str.AppendLine("<tbody>");
                foreach (DetalleFactura detail in factura.DetalleFactura)
                {
                    str.Append("<tr>\n" +
                                "<td>" + detail.Libro.Codigo + "</td>\n" +
                                "<td>" + detail.Libro.Titulo + "</td>\n" +
                                "<td>" + detail.Cantidad + "</td>\n" +
                                "<td>&#162;" + detail.Libro.PrecioVentaSinImpuestos + "</td>\n" +
                                "<td>&#162;" + (detail.Cantidad * detail.Libro.PrecioVentaSinImpuestos) + "</td>\n" +
                                "</tr>"
                        );
                }
                str.AppendLine("<tr>");
                str.AppendLine("<td></td><td></td><td></td>");
                str.AppendLine("<td>Subtotal: </td>");
                str.AppendLine("<td>&#162;" + factura.Subtotal + "</td>");
                str.AppendLine("</tr>");
                str.AppendLine("<tr>");
                str.AppendLine("<td></td><td></td><td></td>");
                str.AppendLine("<td>Impuestos (I.V.A): </td>");
                str.AppendLine("<td>&#162;" + factura.Impuestos + "</td>");
                str.AppendLine("</tr>");
                str.AppendLine("<tr>");
                str.AppendLine("<td></td><td></td><td></td>");
                str.AppendLine("<td>Total (I.V.I): </td>");
                str.AppendLine("<td>&#162;" + factura.Total + "</td>");
                str.AppendLine("</tr>");
                str.AppendLine("</tbody>");
                str.AppendLine("</table> ");
                str.AppendLine("</div>");

                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["server"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };
                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(str.ToString(), null, "text/html");
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = str.ToString()
                })
                {
                    message.AlternateViews.Add(htmlView);
                    smtp.Send(message);

                }

                return true;

            }
            catch (Exception)
            {
                //LOG ERROR
                return false;
            }

        }

        private object ObtenerLibros()
        {
            try
            {

                LibroServicio servicio = new LibroServicio();
                List<Libro> libros = servicio.ObtenerTodos();
                //transform and simplify list to avoid circular dependency issues 
                var newList = libros.Where(x => x.Inventario != null && x.Estado == 1).Select(item => new
                {
                    Codigo = item.Codigo,
                    Autor = item.Autor1.Apellidos + ", " + item.Autor1.Nombre,
                    Precio = item.PrecioVentaConImpuestos,
                    PrecioSinImp = item.PrecioVentaSinImpuestos,
                    Descripcion = item.Descripcion,
                    Image = item.Imagen,
                    Titulo = item.Titulo,
                    Stock = item.Inventario.CantidadStock


                });


                return newList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private object ObtenerCategorias()
        {
            try
            {
                CategoriasLibroServicio servicio = new CategoriasLibroServicio();
                List<Categoria> cats = servicio.ObtenerTodos().Where(x => x.Estado == 1).ToList();
                //remove child elements to avoid circular dependency errors
                var newList = cats.Where(x => x.Estado == 1 && x.Libro != null && x.Libro.Count > 0).Select(item => new
                {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    Libros = item.Libro.Where(x => x.Inventario != null && x.Estado == 1).Select(libro => new
                    {
                        Codigo = libro.Codigo,
                        Autor = libro.Autor1.Apellidos + ", " + libro.Autor1.Nombre,
                        Precio = libro.PrecioVentaConImpuestos,
                        PrecioSinImp = libro.PrecioVentaSinImpuestos,
                        Descripcion = libro.Descripcion,
                        Image = libro.Imagen,
                        Titulo = libro.Titulo,
                        Stock = libro.Inventario.CantidadStock
                    })
                });


                return newList;
            }
            catch (Exception e)
            {
                throw e;
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
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool CambiarEstadoCaja(CajaModel caja)
        {
            try
            {
                if (caja.Estado == 1)
                {
                    SesionModel sesion = Session["SesionSistema"] as SesionModel;
                    int sesioncaja = servicioCaja.Abrir(new Caja { Codigo = caja.Codigo }, sesion.Usuario, caja.Monto);
                    sesion.SesionCaja = sesioncaja;
                    Session["SesionSistema"] = sesion;
                    return true;
                }
                else
                {
                    SesionModel sesion = Session["SesionSistema"] as SesionModel;
                    servicioCaja.Cerrar(new Caja { Codigo = caja.Codigo }, sesion.SesionCaja, caja.Monto, caja.MontoReal);
                    sesion.SesionCaja = 0;
                    Session["SesionSistema"] = sesion;
                    return true;
                }
                
              
            }
            catch (Exception e)
            {
                throw e;
            }
        }
               
        private object ObtenerCajas()
        {
            try
            {

                List<Caja> cajas = servicioCaja.ObtenerTodos().Where(x => x.Codigo != CajaVirtual).ToList();
                //foreach (Caja caja in cajas)
                //{
                //    //remover cajas donde hay algun otro ususario conectado
                //    if (caja.CajaUsuario != null && caja.CajaUsuario.Count > 0)
                //    {
                //        foreach (CajaUsuario cu in caja.CajaUsuario)
                //        {

                //            if (cu.Apertura.Date == DateTime.Today && cu.Cierre == null)
                //            {
                //                cajas.Remove(caja);
                //                break;
                //            }
                //        }
                //    }
                //}
                //remove child elements to avoid circular dependency errors
                var newList = cajas.Select(item => new
                {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    Monto = 0
                });


                return newList;
            }
            catch (Exception e)
            {
                throw e;
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

                SesionModel sesionSistema = Session["SesionSistema"] as SesionModel;
                int sesionCaja = sesionSistema.SesionCaja;
                MovimientoCaja mov = new MovimientoCaja();
                mov.Caja = compra.Caja;
                mov.Fecha = factura.FechaCreacion;
                mov.Monto = factura.Total;
                mov.Tipo = factura.TipoPago == 1 ? 5 : 6;
                mov.Descripcion = "Factura: "+ factura.Numero;
                mov.SesionId = sesionCaja;

                if (!servicioMovimientos.Agregar(mov))
                {
                    //todo log if error
                }
                

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
            catch (Exception e)
            {
                throw e;
            }
        }

        private Usuario ObtenerUsuario(ClienteModel clientep)
        {

            try
            {
                UsuarioRolesServicio serv = new UsuarioRolesServicio();
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
                        Telefono = clientep.Telefono

                    };
                    servicioUsuario.Agregar(cliente);

                    serv.Agregar(new UsuarioRoles { Usuario = cliente.Cedula, Rol = 3, Estado = 1 });
                }
                else
                {
                    bool agregar = true;
                    foreach (UsuarioRoles item in cliente.UsuarioRoles)
                    {
                        if (item.Rol == 3)
                        {
                            agregar = false;
                            break;
                        }
                    }
                    if (agregar)
                    {
                        serv.Agregar(new UsuarioRoles { Usuario = cliente.Cedula, Rol = 3, Estado = 1 });
                    }
                }

                return cliente;

            }
            catch (Exception e)
            {
                throw e;
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

        private object VerificarInvetario(List<ProductoModel> productos)
        {
            List<Libro> agotados = new List<Libro>();
            foreach (ProductoModel item in productos)
            {
                Libro l = servicioLibro.ObtenerPorId(new Libro { Codigo = item.Codigo });
                if (l.Inventario == null || l.Inventario.CantidadStock <= 0 || l.Inventario.CantidadStock < item.Cantidad)
                {
                    agotados.Add(l);
                }
            }
            if (agotados == null || agotados.Count <= 0)
            {
                return null;
            }

            var transformados = agotados.Select(x => new
            {
                Codigo = x.Codigo,
                Titulo = x.Titulo,
                Existencia = x.Inventario != null ? x.Inventario.CantidadStock : 0
            });
            return transformados;
        }
    }
}