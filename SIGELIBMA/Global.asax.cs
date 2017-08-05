using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using Newtonsoft.Json;
using SIGELIBMA.Helpers;
using SIGELIBMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SIGELIBMA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        NLogger logger = new NLogger();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           


            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter
                .SerializerSettings
                .ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        public void Session_OnEnd()
        {

            //closed session at expiration
            if (Session != null || Session["SesionSistema"] != null)
            {
                SesionModel ses = Session["SesionSistema"] as SesionModel;
                if (ses.Id != null && ses.Id > 0)
                {
                    SesionServicio serv = new SesionServicio();

                    Sesion sesDB = serv.ObtenerPorId(new Sesion { Id = ses.Id });
                    sesDB.Finalizacion = DateTime.Now;
                    serv.Modificar(sesDB);
                    Session.Clear();
                    Session.Abandon();

                }
            }
           
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpContext httpContext = ((HttpApplication)sender).Context;
            Exception exception = Server.GetLastError();

            Response.Clear();

            LogException(exception);

            httpContext.ClearError();

            Response.TrySkipIisCustomErrors = true; // Avoid IIS7 getting involved
            Response.ContentType = "application/json";

            if (exception is HttpException)
            {
                Response.StatusCode = ((HttpException)exception).GetHashCode();
                Response.StatusDescription = exception.Message;
            }
            else
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Internal Application Error";
            }

#if DEBUG
            Response.Write(String.Format("An unhandled exception has ocurred: {0}", JsonConvert.SerializeObject(new
            {
                errorMessage = exception.ToString()
            })));
#else
            Response.Write(JsonConvert.SerializeObject(new { error = true,
                        message = "An unexpected application error has occurred."}));
#endif
            Server.ClearError();

        }

        private void LogException(Exception exception)
        {
            logger.LogFatal(exception.ToString());
        }
    }
}
