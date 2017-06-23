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
        private RoleService roleService;

        // GET: MantRoles
        public ActionResult Index()
        {
      
            return View();
        }

        [HttpGet]
        public JsonResult  GetRoles() {
            try
            {
                RoleService service = new RoleService();
                List<Role> roles = service.GetAll();
                return Json(new { OperationStatus = true, Roles = roles, Message = "Operation OK" },JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}