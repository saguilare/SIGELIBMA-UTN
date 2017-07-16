using IMANA.SIGELIBMA.BLL.Services;
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
            //if (System.Web.HttpContext.Current.Session["session"] != null)
            //{ 
            //    Sesion session = System.Web.HttpContext.Current.Session["session"] as Sesion;
            //    if (session != null)
            //    {
            //        ViewBag.Title = "Facturacion";
            //        return View();
            //    }
            //}
            //return RedirectToAction("Login", "Login");

            return View();
        }

        [HttpGet]
        public JsonResult Init() {
            try
            {
                var books = GetBooks();
                //var categories = GetCategories();

                return Json(new { OperationStatus = true,Books = books, Message = "Operacion exitosa" },JsonRequestBehavior.AllowGet);
   
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        [HttpPost]
        public JsonResult OpenCashBox(string param) {
            try
            {
                string caja = "45";
                if (OpenCashBox())
                {
                    return Json(new { OperationStatus = true, Message = string.Format("Caja {0} inicializada", caja) });
                }
                else
                {
                    return Json(new { OperationStatus = false, Message = string.Format("No se pudo inicializar la Caja {0} ", caja) });
                }
                
            }
            catch (Exception ex)
            {
                //TODO
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ProcessPayment(string param)
        {
            try
            {
                bool result = true;
                if (result)
                {
                    return Json(new { OperationStatus = true,Factura = 456456, Message = "Transaccion Exitosa"});
                }
                else
                {
                    return Json(new { OperationStatus = false, Message = "Error: No se registro la factura" });
                }

            }
            catch (Exception ex)
            {
                //TODO
                throw ex;
            }
        }

        private bool OpenCashBox() {
            return true;
        }

        private object GetBooks()
        {
            try
            {

                BookService service = new BookService();
                List<Libro> books = service.GetAll();
                //transform and simplify list to avoid circular dependency issues 
                var newList = books.Select(item => new
                {
                    Codigo = item.Codigo,
                    Autor = item.Autor1.Apellidos + ", " + item.Autor1.Nombre,
                    Precio = item.PrecioVentaConImpuestos,
                    Descripcion = item.Descripcion,
                    Image = item.Imagen,
                    Titulo = item.Titulo

                });


                return newList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private object GetCategories()
        {
            try
            {
                BookCatService service = new BookCatService();
                List<Categoria> cats = service.GetAll().Where(x => x.Estado == 1).ToList();
                //remove child elements to avoid circular dependency errors
                var newList = cats.Select(item => new
                {
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    Libros = item.Libro.Select(libro => new
                    {
                        Codigo = libro.Codigo,
                        Autor = libro.Autor1.Apellidos + ", " + libro.Autor1.Nombre,
                        Precio = libro.PrecioVentaConImpuestos,
                        Descripcion = libro.Descripcion,
                        Image = libro.Imagen,
                        Titulo = libro.Titulo
                    })
                });


                return newList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}