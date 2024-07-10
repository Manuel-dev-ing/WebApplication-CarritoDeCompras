using Microsoft.AspNetCore.Mvc;
using WebApplication2crudimagenes.Models;
using WebApplication2crudimagenes.Servicios;

namespace WebApplication2crudimagenes.Controllers
{
    public class EditorialController : Controller
    {
        private readonly IRepositorioEditorial repositorioEditorial;

        public EditorialController(IRepositorioEditorial repositorioEditorial)
        {
            this.repositorioEditorial = repositorioEditorial;
        }

        public async Task<IActionResult> Index(PaginacionViewModel paginacion)
        {
            var modelo = await repositorioEditorial.ObtenerEditorial(paginacion);
            var totalEditorial = await repositorioEditorial.contarEditorial();

            var respuestaModel = new PaginacionRespuesta<Editorial>()
            {
                Elementos = modelo,
                Pagina = paginacion.Pagina,
                RecordsPorPagina = paginacion.RecordsPorPagina,
                CantidadTotalRecords = totalEditorial,
                BaseURL = "/Editorial"

            };

            return View(respuestaModel);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Editorial editorial)
        {
            if (!ModelState.IsValid)
            {
                return View(editorial);
            }

            await repositorioEditorial.Crear(editorial);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var modelo = await repositorioEditorial.ObtenerPorId(id);

            if (modelo is null)
            {
                return NotFound();
            }

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarEditorial(int id)
        {
            var modelo = await repositorioEditorial.ObtenerPorId(id);

            if (modelo is null)
            {
                return NotFound();
            }

            await repositorioEditorial.borrar(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {

            var modelo = await repositorioEditorial.ObtenerPorId(id);
            if (modelo is null)
            {
                return NotFound();
            }

            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> EditarEditorial(Editorial editorial)
        {
            var modelo = await repositorioEditorial.ObtenerPorId(editorial.id);
            if (modelo is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            await repositorioEditorial.Editar(editorial);

            return RedirectToAction("Index");
        }



    }
}
