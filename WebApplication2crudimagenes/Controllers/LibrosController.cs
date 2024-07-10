using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using System.Runtime.InteropServices;
using WebApplication2crudimagenes.Models;
using WebApplication2crudimagenes.Servicios;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication2crudimagenes.Controllers
{
    public class LibrosController:Controller
    {
        private readonly IRepositoriLibros repositoriLibros;
        private readonly IWebHostEnvironment webHost;
        private readonly IRepositorioAutores repositorioAutores;
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IRepositorioEditorial repositorioEditorial;

        public LibrosController(IRepositoriLibros repositoriLibros, IWebHostEnvironment webHost, IRepositorioAutores repositorioAutores, 
            IRepositorioCategorias repositorioCategorias, IRepositorioEditorial repositorioEditorial)
        {
            this.repositoriLibros = repositoriLibros;
            this.webHost = webHost;
            this.repositorioAutores = repositorioAutores;
            this.repositorioCategorias = repositorioCategorias;
            this.repositorioEditorial = repositorioEditorial;
        }
        
        public async Task<IActionResult> Index(PaginacionViewModel paginacion)
        {
            var model = await repositoriLibros.ObtenerLibros(paginacion);
            var totalLibroos = await repositoriLibros.contarLibros();


            var respuestaModel = new PaginacionRespuesta<LibrosViewModel>()
            {
                Elementos = model,
                Pagina = paginacion.Pagina,
                RecordsPorPagina = paginacion.RecordsPorPagina,
                CantidadTotalRecords = totalLibroos,
                BaseURL = "/Libros"

            };


            return View(respuestaModel);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var modelo = new LibrosCreacionViewModel();

            modelo.tiposAutores = await ObtenerTiposAutores();
            modelo.tiposCategorias = await ObtenerTiposCategorias();
            modelo.tiposEditoriales = await ObtenerTiposEditoriales();

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(LibrosCreacionViewModel model)
        {

            String NombreArchivo = "";

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            NombreArchivo = subirImagen(model);

            Libros libros = new Libros
            {
                titulo = model.titulo,
                idAutor = model.idAutor,
                idCategorias = model.idCategorias,
                idEditorial = model.idEditorial,
                imagenPortada = NombreArchivo,
                isbn = model.isbn,
                precio = model.precio,
                stock = model.stock,
                FechaPublicacion = model.FechaPublicacion,
                sipnosis = model.sipnosis
            };
            await repositoriLibros.Crear(libros);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var modelo = await repositoriLibros.Buscar(id);
            if (modelo != null)
            {
                string carpeta = Path.Combine(webHost.WebRootPath, "Imagenes");
                string imagen = Path.Combine(Directory.GetCurrentDirectory(), carpeta, modelo.imagenPortada);
                if (imagen != null)
                {
                    if (System.IO.File.Exists(imagen))
                    {
                        System.IO.File.Delete(imagen);
                    }
                }
            }

            await repositoriLibros.Borrar(modelo.Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var modelo = await repositoriLibros.Buscar(id);
            if (modelo is null)
            {
                return NotFound();
            }

            modelo.tiposAutores = await ObtenerTiposAutores();
            modelo.tiposCategorias = await ObtenerTiposCategorias();
            modelo.tiposEditoriales = await ObtenerTiposEditoriales();

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(LibrosCreacionViewModel model)
        {
            var modelo = await repositoriLibros.Buscar(model.Id);
            string NombreArchivo = modelo.imagenPortada;

            if (ModelState.IsValid)
            {
                if (model.ImagenArchivo != null)
                {
                    string rutaArchivo = Path.Combine(webHost.WebRootPath, "Imagenes", modelo.imagenPortada);
                    if (System.IO.File.Exists(rutaArchivo))
                    {
                        System.IO.File.Delete(rutaArchivo);
                    }

                    NombreArchivo = subirImagen(model);
                }
            }
            Libros libros = new Libros
            {
                Id = model.Id,
                titulo = model.titulo,
                idAutor = model.idAutor,
                idCategorias = model.idCategorias,
                idEditorial = model.idEditorial,
                imagenPortada = NombreArchivo,
                isbn = model.isbn,
                FechaPublicacion = model.FechaPublicacion,
                sipnosis = model.sipnosis
            };

            await repositoriLibros.Actualizar(libros);

            return RedirectToAction("Index");
        }


        private string subirImagen(LibrosCreacionViewModel librosCreacion)
        {
            string NombreArchivo = "";
            String upload = Path.Combine(webHost.WebRootPath, "Imagenes");
            NombreArchivo = Guid.NewGuid().ToString() + "_" + librosCreacion.ImagenArchivo.FileName;
            String RuTaArchivo = Path.Combine(upload, NombreArchivo);
            librosCreacion.ImagenArchivo.CopyTo(new FileStream(RuTaArchivo, FileMode.Create));

            return NombreArchivo;
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposAutores()
        {
            var tiposAutores = await repositorioAutores.Obtener();
            return tiposAutores.Select(x => new SelectListItem(x.nombre, x.id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCategorias()
        {
            var tiposCategorias = await repositorioCategorias.Obtener();
            return tiposCategorias.Select(x => new SelectListItem(x.nombre, x.id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposEditoriales()
        {
            var tiposEditoriales = await repositorioEditorial.Obtener();
            return tiposEditoriales.Select(x => new SelectListItem(x.nombre, x.id.ToString()));
        }


    }
}
