using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMANA.SIGELIBMA.BLL.Servicios;
using IMANA.SIGELIBMA.DAL;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Globalization;
using SIGELIBMA.Models;
using SIGELIBMA.Filters;

namespace SIGELIBMA.Controllers
{
    [ExceptionFilter]
    [ValidateSessionFilter]
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
                List<LibroModel> libros = new List<LibroModel>();
                foreach(Libro libroDB in librosdb)
                {
                    LibroModel libro = new LibroModel
                    { Codigo = libroDB.Codigo, Titulo= libroDB.Titulo, Descripcion = libroDB.Descripcion, Fecha = libroDB.Fecha.ToString("MM/dd/yyyy"),
                    Categoria1 = new CategoriaModel { Codigo = libroDB.Categoria1.Codigo, Descripcion = libroDB.Categoria1.Descripcion, Estado = libroDB.Categoria1.Estado == null ? 1 : (Int32)libroDB.Categoria1.Estado },
                    Autor1 = new AutorModel {Codigo = libroDB .Autor1.Codigo, Nombre = libroDB .Autor1.Nombre, Apellidos = libroDB.Autor1.Apellidos, Estado= libroDB.Autor1.Estado == null ? 1 : (Int32)libroDB.Autor1.Estado},
                    Proveedor1= new ProveedorModel {Codigo = libroDB.Proveedor1.Codigo, Nombre= libroDB.Proveedor1.Nombre, Telefono = libroDB.Proveedor1.Telefono, Correo = libroDB.Proveedor1.Correo, Estado = libroDB.Proveedor1.Estado == null ? 1 : (Int32)libroDB.Proveedor1.Estado },
                    PrecioBase = libroDB.PrecioBase, PorcentajeGanancia = libroDB.PorcentajeGanancia, PrecioVentaSinImpuestos = libroDB.PrecioVentaSinImpuestos, PrecioVentaConImpuestos = libroDB.PrecioVentaConImpuestos, NombreImagen = libroDB.Imagen, Estado = libroDB.Estado == null ? 1 : (Int32)libroDB.Estado
                    };
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
        public JsonResult validarLibro(Libro librop)
        {
            try
            {
                bool existeLibro = false;
                Libro libro = LibroServicio.ObtenerPorId(librop);
                if (libro != null)
                {
                    existeLibro = true;
                }
                return Json(new { EstadoOperacion = true, ExisteLibro = existeLibro, Mensaje = "Operation OK" });
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
        public JsonResult Modificar(LibroModel librop)
        {
            try
            {
                bool resultado = false;
                String path = HttpContext.Server.MapPath(ConfigurationManager.AppSettings["rutaImagenes"]);
                int fileExtPos = librop.NombreImagen.LastIndexOf(".");
                string imagensinextensiones = librop.NombreImagen.Substring(0, fileExtPos);
                List<string> names = new List<string>(librop.Imagen.Split(','));
                byte[] imageBytes = Convert.FromBase64String(names[1].ToString());
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    Image image = Image.FromStream(ms, true);
                    image.Save(path + librop.NombreImagen);
                }

                DateTime dt = DateTime.ParseExact(librop.Fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture);


                Libro libro = new Libro
                {
                    Codigo = librop.Codigo,
                    Titulo = librop.Titulo,
                    Descripcion = librop.Descripcion,
                    Fecha = dt,
                    Categoria = librop.Categoria1.Codigo,
                    Autor = librop.Autor1.Codigo,
                    Proveedor = librop.Proveedor1.Codigo,
                    PrecioBase = librop.PrecioBase,
                    PorcentajeGanancia = librop.PorcentajeGanancia,
                    PrecioVentaSinImpuestos = librop.PrecioVentaSinImpuestos,
                    PrecioVentaConImpuestos = librop.PrecioVentaConImpuestos,
                    Imagen = imagensinextensiones,
                    Estado = librop.Estado
                };
                resultado = LibroServicio.Modificar(libro);
                return Json(new { EstadoOperacion = resultado, Mensaje = "Operation OK" });
            }
            catch (Exception e)
            {

                //TODO handle ex
                return Json(new { EstadoOperacion = false, Mensaje = "Exception thrown, please verify backend services" });
            }
        }

        [HttpPost]
        public JsonResult Agregar(LibroModel librop)
        {
            try
            {
                
                bool resultado = false;
                String path = HttpContext.Server.MapPath(ConfigurationManager.AppSettings["rutaImagenes"]);
                int fileExtPos = librop.NombreImagen.LastIndexOf(".");
                string imagensinextensiones = librop.NombreImagen.Substring(0, fileExtPos);
                List <string> names = new List<string>(librop.Imagen.Split(','));
                byte[] imageBytes = Convert.FromBase64String(names[1].ToString());
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    Image image = Image.FromStream(ms, true);
                    image.Save(path+librop.NombreImagen);
                }

                DateTime dt = DateTime.ParseExact(librop.Fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture);


                Libro libro = new Libro { Codigo = librop.Codigo, Titulo = librop.Titulo, Descripcion = librop.Descripcion, Fecha= dt, Categoria = librop.Categoria1.Codigo, Autor = librop.Autor1.Codigo, Proveedor = librop.Proveedor1.Codigo,
                    Proveedor1 = new Proveedor { Codigo = librop.Proveedor1.Codigo, Nombre = librop.Proveedor1.Nombre, Telefono = librop.Proveedor1.Telefono, Correo = librop.Proveedor1.Correo, Estado = librop.Proveedor1.Estado },
                    PrecioBase = librop.PrecioBase, PorcentajeGanancia = librop.PorcentajeGanancia, PrecioVentaSinImpuestos = librop.PrecioVentaSinImpuestos, PrecioVentaConImpuestos = librop.PrecioVentaConImpuestos, Imagen = imagensinextensiones, Estado = librop.Estado
                };

                resultado = LibroServicio.Agregar(libro);
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