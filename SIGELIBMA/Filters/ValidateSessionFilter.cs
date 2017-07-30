using IMANA.SIGELIBMA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGELIBMA.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidateSessionFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToString().ToLower() != "home" 
                || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToString().ToLower() != "login" &&
                filterContext.ActionDescriptor.ActionName.ToString() != "ValidarLogin")
                || (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToString().ToLower() != "login" &&
                filterContext.ActionDescriptor.ActionName.ToString() != "Logout")
                )
            {
                var session = filterContext.HttpContext.Session;
                if (session == null || session["SesionSistema"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Login" }, 
                        { "action", "Index" },
                        {"code", 1}
                    });
                }
                else
                {
                    Sesion ses = session["SesionSistema"] as Sesion;
                    string controler = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToString();
                    if (controler.Equals("facturacion", StringComparison.OrdinalIgnoreCase)
                        || controler.Equals("inventario", StringComparison.OrdinalIgnoreCase)
                        || controler.Equals("entregas", StringComparison.OrdinalIgnoreCase)
                        )
                    {  bool flag = false;
                        foreach (UsuarioRoles item in ses.Usuario1.UsuarioRoles)
                        {
                            flag = item.Rol1.Codigo == 1 || item.Rol1.Codigo == 2 ? true : false;
                            break;
                        }
                        if (flag == false)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                            new System.Web.Routing.RouteValueDictionary
                            {
                                { "controller", "Login" }, 
                                { "action", "AccesoRestringido" }
                            });
                        }
                    }
                    else if (controler.Equals("mantautor", StringComparison.OrdinalIgnoreCase)
                        || controler.Equals("mantcategoria", StringComparison.OrdinalIgnoreCase)
                        || controler.Equals("mantestadocaja", StringComparison.OrdinalIgnoreCase)
                        || controler.Equals("mantestadofactura", StringComparison.OrdinalIgnoreCase)
                        || controler.Equals("mantproveedor", StringComparison.OrdinalIgnoreCase)
                        || controler.Equals("mantroles", StringComparison.OrdinalIgnoreCase)
                        || controler.Equals("manttipomovcaja", StringComparison.OrdinalIgnoreCase)
                        || controler.Equals("manttipopago", StringComparison.OrdinalIgnoreCase)
                        )
                    {
                        bool flag = false;
                        foreach (UsuarioRoles item in ses.Usuario1.UsuarioRoles)
                        {
                            flag = item.Rol1.Codigo == 1  ? true : false;
                            break;
                        }
                        if (flag == false)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                            new System.Web.Routing.RouteValueDictionary
                            {
                                { "controller", "Login" }, 
                                { "action", "AccesoRestringido" }
                            });
                        }
                    }
                }
            }
            
            
        }

    }
}