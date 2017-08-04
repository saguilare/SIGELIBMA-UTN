using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using SIGELIBMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SIGELIBMA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Session_OnEnd()
        {


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
    }
}
