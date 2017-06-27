using IMANA.SIGELIBMA.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGELIBMA.Controllers
{
    public class FacturacionController : Controller
    {
        // GET: Facturacion
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["session"] != null)
            {
                SystemSession session = System.Web.HttpContext.Current.Session["session"] as SystemSession;
                if (session.Status == true)
                {
                    return View();
                }

            }
     
             return RedirectToAction("Login", "Login");
            
           
        }

        

    }
}