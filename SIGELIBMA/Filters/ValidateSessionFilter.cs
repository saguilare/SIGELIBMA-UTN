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
                filterContext.ActionDescriptor.ActionName.ToString() != "ValidarLogin"))
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
            }
            
            
        }

    }
}