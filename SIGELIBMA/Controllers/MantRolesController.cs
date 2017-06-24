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
        private RolService roleService = new RolService();


        [HttpGet]
        public ActionResult Index()
        { 
            return View();
        }

        [HttpGet]
        public JsonResult GetAll() {

            try
            {
                List<Role> roles = roleService.GetAll();
                return Json(new { OperationStatus = true, Roles = roles, Message = "Operation OK" },JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetById(Role rolp)
        {
            try
            {
                Role rol = roleService.GetById(rolp);
                return Json(new { OperationStatus = true, Rol = rol, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Delete(Role rolp)
        {
            try
            {
                bool result = false;
                result = roleService.Delete(rolp);
                return Json(new { OperationStatus = true, Result = result, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Update(Role rolp)
        {
            try
            {
                bool result = false;
                result = roleService.Update(rolp);
                return Json(new { OperationStatus = true, Result = result, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" });
            }
        }

    }
}