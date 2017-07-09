using IMANA.SIGELIBMA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMANA.SIGELIBMA.MVC.Controllers
{
    public class FacturacionController : Controller
    {
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["session"] != null)
            {
                Sesion session = System.Web.HttpContext.Current.Session["session"] as Sesion;
                if (session != null)
                {
                    ViewBag.Title = "Facturacion";
                    return View();
                }
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}