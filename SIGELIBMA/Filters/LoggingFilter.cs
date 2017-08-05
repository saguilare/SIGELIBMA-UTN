using NLog;
using SIGELIBMA.Helpers;
using System;
using System.Web.Http;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using System.Web.Mvc.Filters;


namespace SIGELIBMA.Filters
{

    //public class LoggingFilter : FilterAttribute
    //{
        
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        NLogger logger = new NLogger();
    //        logger.Trace(filterContext.HttpContext.Request, "Controller : " + filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action : " + filterContext.ActionDescriptor.ActionName, "JSON", new Exception { });
    //    }
    //}   
}