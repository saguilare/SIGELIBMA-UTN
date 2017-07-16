﻿using IMANA.SIGELIBMA.DAL;
using SIGELIBMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGELIBMA.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Login(int? code)
        {
            int errorCode = Convert.ToInt32((code != null) ? code : 1);
            if (System.Web.HttpContext.Current.Session["session"] != null)
            {
                Sesion session = System.Web.HttpContext.Current.Session["session"] as Sesion;
                if (session != null)
                {
                    return RedirectToAction("Index", "Facturacion");
                }
            }

            ViewBag.code = errorCode;
            ViewBag.Title = "Login";
            return View();
        }

        [HttpPost]
        public ActionResult validateLogin(UserLoginModel login)
        {

            try
            {

                if (ValidateUser(login))
                {
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Home");
                    return Json(new { OperationStatus = true, Url = redirectUrl });
                }
                else
                {
                    return Json(new { OperationStatus = false, Message = "Acceso denegado, por favor verifique sus credenciales." });
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(new { OperationStatus = false, Message = "System error,validate login ex thrown" });
            }

        }

        private bool ValidateUser(UserLoginModel login) {
            try
            {
                //validate against DB
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}