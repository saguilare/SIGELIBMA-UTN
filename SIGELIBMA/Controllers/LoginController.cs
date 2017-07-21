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
    //[ValidateSessionFilter]
    public class LoginController : Controller
    {

        public ActionResult Index(int? code)
        {
            int errorCode = Convert.ToInt32((code != null) ? code : 1);
           
            ViewBag.code = errorCode;
            ViewBag.Title = "Login";
            return View();
        }

        [HttpPost]
        public ActionResult ValidarLogin(UserLoginModel login)
        {

            try
            {

                if (ValidarUsuario(login))
                {
                    SesionServicio servicio = new SesionServicio();
                    Sesion sesion = new Sesion { Usuario = "206370927", Inicio = DateTime.Now };
                    servicio.Agregar(sesion);
                    Session.Add("SesionSistema", sesion);
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
                return Json(new { EstadoOperacion = false, Mensaje = "System error,validate login ex thrown" });
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
                    foreach (UsuarioRoles rol in usuario.UsuarioRoles)
                    {
                        if (rol.Rol == rolAdmin || rol.Rol == rolVentas)
                        {
                            return true;
                        } 
                    }
                   
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