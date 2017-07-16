using IMANA.SIGELIBMA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMANA.SIGELIBMA.BLL.Services;

namespace SIGELIBMA.Controllers
{
    public class MantRolesController : Controller
    {
        private RolService rolService = new RolService();


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Roles";
            return View();
        }


        [HttpGet]
        public JsonResult GetInitData()
        {

            try
            {
                List<Rol> Roles = rolService.GetAll();
                return Json(new { OperationStatus = true, Roles = Roles, Message = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAll() {

            try
            {
               
                List<Rol> Roles = rolService.GetAll();
                return Json(new { OperationStatus = true, Roles = Roles, Message = "Operation OK" },JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetById(Rol rolp)
        {
            try
            {
                Rol rol = rolService.GetById(rolp);
                return Json(new { OperationStatus = true, Rol = rol, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(Rol rolp)
        {
            try
            {
                bool result = false;
                result = rolService.Delete(rolp);
                return Json(new { OperationStatus = true, Result = result, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(Rol rolp)
        {
            try
            {
                bool result = false;
                result = rolService.Update(rolp);
                return Json(new { OperationStatus = true, Result = result, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(Rol rolp)
        {
            try
            {
                bool result = false;
                result = rolService.Add(rolp);
                return Json(new { OperationStatus = true, Result = result, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                Response.StatusCode = 400;
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}