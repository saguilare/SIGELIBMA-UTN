using IMANA.SIGELIBMA.BLL.Services;
using IMANA.SIGELIBMA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGELIBMA.Controllers
{
    public class BookController : Controller
    {
        BookService bookService = new BookService();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {

            try
            {
                List<Libro> Books = bookService.GetAll();
                return Json(new { OperationStatus = true, Books = Books, Message = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetById(Libro bookp)
        {
            try
            {
                Libro book = bookService.GetById(bookp);
                return Json(new { OperationStatus = true, Book = book, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Delete(Libro bookp)
        {
            try
            {
                bool result = false;
                result = bookService.Delete(bookp);
                return Json(new { OperationStatus = true, Result = result, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Update(Libro bookp)
        {
            try
            {
                bool result = false;
                result = bookService.Update(bookp);
                return Json(new { OperationStatus = true, Result = result, Message = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { OperationStatus = false, Message = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Add(Libro bookp)
        {
            try
            {
                bool result = false;
                result = bookService.Add(bookp);
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