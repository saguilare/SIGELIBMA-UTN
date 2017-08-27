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

namespace SIGELIBMA.Controllers
{
    
    [ExceptionFilter]
    public class LoginController : Controller
    {

        public ActionResult Index(int? code)
        {
            if (Session == null || Session["SesionSistema"] == null) {
                int errorCode = Convert.ToInt32((code != null) ? code : 3);

                ViewBag.code = errorCode;
                ViewBag.Title = "Login";
                return View();

            }
            else
            {

                return RedirectToAction("Index", "Facturacion");
            }
            
        }

        public ActionResult AccesoRestringido() {
            return View();
        }

        [ValidateSessionFilter]
        [HttpGet]
        public JsonResult GetSesion() {
            try
            {
                if (Session != null && Session["SesionSistema"] != null)
                {
                    SesionModel sesion = Session["SesionSistema"] as SesionModel;
                    var s = new { id = sesion.Id, usuario = sesion.Usuario.Apellido1+", "+sesion.Usuario.Nombre, username = sesion.Usuario.Usuario1};
                    return Json(new { EstadoOperacion = true, Sesion = s , Mensaje = "Operacion OK"},JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { EstadoOperacion = false, redirectUrl = Url.Action("Index", "Login") , Mensaje = "Operacion OK"},JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }

        [ValidateSessionFilter]
        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                if (Session != null && Session["SesionSistema"] != null)
                {
                    SesionServicio serv = new SesionServicio();
                    SesionModel s = Session["SesionSistema"] as SesionModel;
                    Sesion sesDB = serv.ObtenerPorId(new Sesion { Id = s.Id });
                    sesDB.Finalizacion = DateTime.Now;
                    serv.Modificar(sesDB);
                    Session.Clear();
                    Session.Abandon();
                }

                return RedirectToAction("Index", new {code = 2 });
                
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }
        }

        [HttpPost]
        public ActionResult ValidarLogin(UserLoginModel login)
        {

            try
            {

                if (ValidarUsuario(login))
                {
                    
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Facturacion");
                    return Json(new { EstadoOperacion = true, Url = redirectUrl });
                }
                else
                {
                    return Json(new { EstadoOperacion = false, Mensaje = "Acceso denegado, por favor verifique sus credenciales." });
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                throw e;
            }

        }

        private bool ValidarUsuario(UserLoginModel login) {
            try
            {
                int rolCliente = Convert.ToInt32(ConfigurationManager.AppSettings["RolClienteCode"]);
                int rolAdmin = Convert.ToInt32(ConfigurationManager.AppSettings["RolAdminCode"]);
                int rolVentas = Convert.ToInt32(ConfigurationManager.AppSettings["RolVentasCode"]);

                UsuarioServicio servicio = new UsuarioServicio();
                Usuario usuario = servicio.Validar(new Usuario { Usuario1 = login.Username, Clave = login.Password });
                if (usuario != null && usuario.Estado != 0 && usuario.UsuarioRoles != null && usuario.UsuarioRoles.Count > 0)
                {
                    SesionServicio servicioSesion = new SesionServicio();
                    Sesion sesion = new Sesion { Usuario = usuario.Cedula, Inicio = DateTime.Now , Finalizacion = null};
                    servicioSesion.Agregar(sesion);
                    SesionModel s = new SesionModel();
                    s.Id = sesion.Id;
                    s.Usuario = usuario;
                    s.Inicio = sesion.Inicio;
                    Session.Add("SesionSistema", s);
                    return true;
                }
        
                return false;
                
                
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}