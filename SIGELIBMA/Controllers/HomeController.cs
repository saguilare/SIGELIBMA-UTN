using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.MVC.Controllers;
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

namespace SIGELIBMA.Controllers
{
    [ExceptionFilter]
    public class HomeController : Controller
    {
        private FacturaServicio servicioFactura = new FacturaServicio();
        private UsuarioServicio servicioUsuario = new UsuarioServicio();
        private CajaServicio servicioCaja = new CajaServicio();
        private LibroServicio servicioLibro = new LibroServicio();
        //private SesionServicio servicioSesion = new SesionServicio();
        //private TransaccionServicio servicioTransaccion = new TransaccionServicio();
        private decimal IVA = Convert.ToDecimal(ConfigurationManager.AppSettings["IVA"]);
        private int CajaVirtual = Convert.ToInt32(ConfigurationManager.AppSettings["CajaVirtual"]);
        private List<string> Bancos = ConfigurationManager.AppSettings["Bancos"].Split(',').ToList();
        private string RazonSocial = ConfigurationManager.AppSettings["RazonSocial"];
        private string CedulaJuridica = ConfigurationManager.AppSettings["CedulaJuridica"];
        private List<string> Cuentas = ConfigurationManager.AppSettings["accts"].Split(',').ToList();

        
        

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
                string date = DateTime.Today.ToString("MM/dd/yyyy"); 
                return Json(new { EstadoOperacion = true,
                                  Categorias =categorias, 
                                  Libros = libros, 
                                  RazonSocial = RazonSocial,
                                  CedulaJuridica = CedulaJuridica,
                                  BancosEmisores = Bancos,
                                  BancosReceptores = Cuentas,
                                  Date = date,
                                  Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }


        [HttpPost]
        public JsonResult ProcesarCompra(CompraModel compra)
        {
            try
            {
                var agotados = VerificarInvetario(compra.Productos);
                if (agotados != null )
                {
                    return Json(new { EstadoOperacion = false, Agotados = agotados, Confirmacion = "", Mensaje = "La factura no se proceso, no hay articulos en existencia" });
                }

                compra.Cliente.Nombre2 = (compra.Cliente.Nombre1 != "" && compra.Cliente.Nombre1.Contains(" ")) ? compra.Cliente.Nombre1.Split(' ')[1] : "";
                compra.Cliente.Apellido2 = (compra.Cliente.Apellido1 != "" && compra.Cliente.Apellido1.Contains(" ")) ? compra.Cliente.Apellido1.Split(' ')[1] : "";
                Factura factura = CrearFactura(compra);

                bool resultado = RetirarInvetario(compra.Productos);

                return Json(new { EstadoOperacion = true, Confirmacion = factura.Numero, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {
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
                    var usuarioLimpio = new {
                     Cedula = user.Cedula,
                     Nombre = user.Segundo_Nombre != ""? user.Nombre + " " + user.Segundo_Nombre : user.Nombre,
                     Apellidos = user.Apellido2 != ""? user.Apellido1 + " " + user.Apellido2 : user.Apellido1,
                     Correo = user.Correo,
                     Telefono = user.Telefono
                    };
                    return Json(new { EstadoOperacion = true, Usuario = usuarioLimpio, Mensaje = "Operacion OK" });
                }


                return Json(new { EstadoOperacion = false, Usuario = "", Mensaje = "Operacion OK" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private object ObtenerLibros()
        {
            try 
	        {	        
                
		        LibroServicio servicio = new LibroServicio();
                List<Libro> libros = servicio.ObtenerTodos();
                //transform and simplify list to avoid circular dependency issues 
                var newList = libros.Where(x => x.Inventario != null &&  x.Estado == 1).Select(item => new
                {
                    Codigo = item.Codigo,
                    Autor = item.Autor1.Apellidos + ", " + item.Autor1.Nombre,
                    Precio = item.PrecioVentaConImpuestos,
                    Descripcion = item.Descripcion,
                    Image = item.Imagen,
                    Titulo =item.Titulo,
                    Stock = item.Inventario != null && item.Inventario.CantidadStock > 0 ? item.Inventario.CantidadStock : 0


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
                List<Categoria> cats = servicio.ObtenerTodos().Where(x => x.Estado == 1 && x.Libro != null && x.Libro.Count > 0).ToList();
                //remove child elements to avoid circular dependency errors

                
                List<Categoria> cleanCats = new List<Categoria>();
                if (cats != null && cats.Count > 0)
                {
                    foreach (Categoria cat in cats)
                    {
                        foreach (Libro libro in cat.Libro)
                        {
                            if (libro.Estado == 1 && libro.Inventario != null && libro.Inventario.CantidadStock > 0)
                            {
                                cleanCats.Add(cat);
                                break;
                            }
                        }
                    }
                }

                var newList = cleanCats.Where(x => x.Estado == 1 && x.Libro != null && x.Libro.Count > 0).Select(item => new
                {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    Libros = item.Libro.Where(x => x.Inventario != null && x.Estado == 1).Select(libro => new {
                        Codigo = libro.Codigo,
                        Autor = libro.Autor1.Apellidos + ", " + libro.Autor1.Nombre,
                        Precio = libro.PrecioVentaConImpuestos,
                        Descripcion = libro.Descripcion,
                        Image = libro.Imagen,
                        Titulo =libro.Titulo,
                        Stock = libro.Inventario != null && libro.Inventario.CantidadStock > 0 ? libro.Inventario.CantidadStock : 0
                    })
                });


                return newList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private Factura CrearFactura(CompraModel compra) {
            try
            {
                
                Sesion sesion = new Sesion();
                sesion.Inicio = DateTime.Now;

                Factura factura = new Factura();
                factura.Cliente = ObtenerUsuario(compra.Cliente).Cedula;
                factura.Caja = ObtenerCaja(CajaVirtual).Codigo;
                factura.FechaCreacion = DateTime.Now;
                factura.FechaCancelacion = null;
                factura.TipoPago = 3;
                List<Deposito> depositos = new List<Deposito>();
                depositos.Add(new Deposito{ Referencia = compra.Deposito.Referencia,
                    Fecha = compra.Deposito.Fecha,
                    BancoEmisor = compra.Deposito.BancoEmisor,
                    BancoReceptor = compra.Deposito.BancoReceptor,
                    Descripcion = compra.Deposito.Descripcion == null ? "": compra.Deposito.Descripcion
                    
                });
                factura.Deposito = depositos;
                factura.Referencia = depositos != null && depositos.Count > 0 ? depositos.FirstOrDefault().Referencia.ToString() : string.Empty;
                AgregarDetallesFactura(compra.Productos,ref factura);
                CalcularMontosFactura(ref factura);
                factura.Estado = 4;

                servicioFactura.Agregar(factura);

                bool send = Convert.ToBoolean(ConfigurationManager.AppSettings["sendEmail"]);
                if (send)
                {
                    SendEmail(factura.Numero);
                }
               

                return factura;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private Usuario ObtenerUsuario(ClienteModel clientep) {

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
                   
                    serv.Agregar(new UsuarioRoles { Usuario = cliente.Cedula, Rol = 3,Estado =1 });
	            }else
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
                   
                    DetalleFactura detalle = new DetalleFactura { Articulo = prod.Codigo, Cantidad = prod.Cantidad};

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

        private bool SendEmail(int facturaNum)
        {

            try
            {
                Factura factura = servicioFactura.ObtenerPorId(new Factura { Numero = facturaNum });
                factura.Usuario = servicioUsuario.ObtenerPorId(new Usuario { Cedula = factura.Cliente});
                foreach (DetalleFactura item in factura.DetalleFactura)
                {
                    item.Libro = servicioLibro.ObtenerPorId(new Libro {Codigo = item.Articulo });
                }
                string from = ConfigurationManager.AppSettings["from"];
                var fromAddress = new MailAddress(from, "Libreria Iglesia Mana");
                var toAddress = new MailAddress(factura.Usuario.Correo, factura.Usuario.Correo);
                string fromPassword = ConfigurationManager.AppSettings["password"];
                string subject = "Confirmacion Compra - #"+factura.Numero;
                StringBuilder str = new StringBuilder();
                str.AppendLine("Estimado(a): "+ factura.Usuario.Nombre);
                str.AppendLine("<br/>");
                str.AppendLine("Adjuntamos el detalle de su compra, le recordamos que puede pasar a retirar su producos de Lunes a Viernes de 8am a 7pm");
                str.AppendLine("<br/><br/>");
                str.AppendLine("<style>td{border-bottom: 1px solid black;}</style>");
                str.AppendLine("<div style='min-width: 300px'>");
                str.AppendLine(" <table >");
                str.AppendLine("<thead>");
                str.AppendLine("<tr>");
                str.AppendLine("<th><a>Codigo</a></th><th><a>Titulo</a></th><th><a>Cantidad</a></th><th><a>Precio Unitario</a></th><th><a>Precio Total</a></th>");
                str.AppendLine("</tr>");
                str.AppendLine("</thead>");
                str.AppendLine("<tbody>");
                foreach (DetalleFactura detail in factura.DetalleFactura)
	            {
		            str.Append("<tr>\n"+
                                "<td>"+detail.Libro.Codigo+"</td>\n"+
                                "<td>"+detail.Libro.Titulo+"</td>\n"+
                                "<td>"+detail.Cantidad+"</td>\n"+
                                "<td>&#162;"+detail.Libro.PrecioVentaSinImpuestos+"</td>\n"+
                                "<td>&#162;"+(detail.Cantidad * detail.Libro.PrecioVentaSinImpuestos)+"</td>\n"+
                                "</tr>"
                        );
	            }
                str.AppendLine("<tr>");
                str.AppendLine("<td></td><td></td><td></td>");
                str.AppendLine("<td>Subtotal: </td>");
                str.AppendLine("<td>&#162;"+factura.Subtotal+"</td>");
                str.AppendLine("</tr>");
                str.AppendLine("<tr>");
                str.AppendLine("<td></td><td></td><td></td>");
                str.AppendLine("<td>Impuestos (I.V.A): </td>");
                str.AppendLine("<td>&#162;"+factura.Impuestos+"</td>");
                str.AppendLine("</tr>");
                str.AppendLine("<tr>");
                str.AppendLine("<td></td><td></td><td></td>");
                str.AppendLine("<td>Total (I.V.I): </td>");
                str.AppendLine("<td>&#162;"+factura.Total+"</td>");
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

        private object VerificarInvetario(List<ProductoModel> productos)
        {
            List<Libro> agotados = new List<Libro>();
            foreach (ProductoModel item in productos)
            {
                Libro l = servicioLibro.ObtenerPorId(new Libro { Codigo = item.Codigo});
                if (l.Inventario == null || l.Inventario.CantidadStock <= 0 || l.Inventario.CantidadStock < item.Cantidad)
                {
                    agotados.Add(l);
                }
            }
            if (agotados == null || agotados.Count <= 0 ) {
                return null;
            }

            var transformados = agotados.Select(x => new { 
                Codigo = x.Codigo,
                Titulo = x.Titulo,
                Existencia = x.Inventario != null ? x.Inventario.CantidadStock : 0
            });
            return transformados;
        }

        private bool RetirarInvetario(List<ProductoModel> productos)
        {
            List<Libro> agotados = new List<Libro>();
            foreach (ProductoModel item in productos)
            {
                Libro l = servicioLibro.ObtenerPorId(new Libro { Codigo = item.Codigo });
                l.Inventario.CantidadStock -= item.Cantidad;
                servicioLibro.Modificar(l);
            }

            return true;
           
        }
    }
}