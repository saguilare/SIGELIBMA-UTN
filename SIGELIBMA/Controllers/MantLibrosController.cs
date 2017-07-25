using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using IMANA.SIGELIBMA.DAL.DTOs;

namespace SIGELIBMA.Controllers
{
    public class MantLibrosController : Controller
    {

        LibroServicio LibroServicio = new LibroServicio();
        CategoriasLibroServicio CategoriaServicio = new CategoriasLibroServicio();
        AutorServicio AutorServicio = new AutorServicio();
        ProveedorServicio ProveedorServicio = new ProveedorServicio();

        [HttpGet]
        // GET: MantLibros
        public ActionResult Index()
        {
            ViewBag.Title = "Libros";
            return View();
        }
        [HttpGet]
        public JsonResult ObtenerTodos()
        {

            try
            {
                List<Libro> librosdb = LibroServicio.ObtenerTodos();
                List<LibroDTO> libros = new List<LibroDTO>();
                foreach(Libro libroDB in librosdb)
                {
                    LibroDTO libro = new LibroDTO { Codigo = libroDB.Codigo, Titulo= libroDB.Titulo, Descripcion = libroDB.Descripcion, Fecha = libroDB.Fecha.Value.ToString("MM/dd/yyyy"),
                    Categoria1 = new CategoriaDTO { Codigo = libroDB.Categoria1.Codigo, Descripcion = libroDB.Categoria1.Descripcion, Estado = libroDB.Categoria1.Estado },
                    Autor1 = new AutorDTO {Codigo = libroDB .Autor1.Codigo, Nombre = libroDB .Autor1.Nombre, Apellidos = libroDB.Autor1.Apellidos, Estado= libroDB.Autor1.Estado},
                    Proveedor1= new ProveedorDTO {Codigo = libroDB.Proveedor1.Codigo, Nombre= libroDB.Proveedor1.Nombre, Telefono = libroDB.Proveedor1.Telefono, Correo = libroDB.Proveedor1.Correo, Estado = libroDB.Proveedor1.Estado },
                    PrecioBase = libroDB.PrecioBase, PorcentajeGanancia = libroDB.PorcentajeGanancia, PrecioVentaSinImpuestos = libroDB.PrecioVentaSinImpuestos, PrecioVentaConImpuestos = libroDB.PrecioVentaConImpuestos, Imagen = libroDB.Imagen, Estado = libroDB.Estado};
                    libros.Add(libro);
                }

                return Json(new { EstadoOperacion = true, Libros = libros, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObtenerPorId(Libro librop)
        {
            try
            {
                Libro libro = LibroServicio.ObtenerPorId(librop);
                return Json(new { EstadoOperacion = true, Libro = libro, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Desabilitar(Libro librop)
        {
            try
            {
                bool resultado = false;
                Libro libro = new Libro();
                libro = librop;
                libro.Autor = librop.Autor1.Codigo;
                libro.Categoria = librop.Categoria1.Codigo;
                libro.Proveedor = librop.Proveedor1.Codigo;
                resultado = LibroServicio.Desabilitar(librop);
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Modificar(Libro librop)
        {
            try
            {
                bool resultado = false;
                resultado = LibroServicio.Modificar(librop);
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Agregar(Libro librop)
        {
            try
            {
                bool resultado = false;
                resultado = LibroServicio.Agregar(librop);
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        [HttpGet]
        public JsonResult ObtenerCategorias()
        {

            try
            {
                var categorias = CategoriaServicio.ObtenerTodos().Select(x => new
                {
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Categorias = categorias, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ObtenerAutores()
        {

            try
            {
                var autores = AutorServicio.ObtenerTodos().Select(x => new
                {
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Apellidos = x.Apellidos,
                    Estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Autores = autores, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ObtenerProveedores()
        {

            try
            {
                var proveedores = ProveedorServicio.ObtenerTodos().Select(x => new
                {
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Telefono = x.Telefono,
                    Correo = x.Correo,
                    Estado = x.Estado
                });
                return Json(new { EstadoOperacion = true, Proveedores = proveedores, Mensaje = "Operation OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}