using Microsoft.AspNetCore.Mvc;
using WebApplication2crudimagenes.Models;
using WebApplication2crudimagenes.Servicios;

namespace WebApplication2crudimagenes.Controllers
{
    public class AutoresController : Controller
    {
        private readonly IRepositorioAutores repositorioAutores;

        public AutoresController(IRepositorioAutores repositorioAutores)
        {
            this.repositorioAutores = repositorioAutores;
        }

        public async Task<IActionResult> Index(PaginacionViewModel paginacion)
        {
            var modelo = await repositorioAutores.ObtenerAutores(paginacion);
            var totalAutores = await repositorioAutores.contarAutores();

            var respuestaModel = new PaginacionRespuesta<Autores>()
            {
                Elementos = modelo,
                Pagina = paginacion.Pagina,
                RecordsPorPagina = paginacion.RecordsPorPagina,
                CantidadTotalRecords = totalAutores,
                BaseURL = "/Autores"
            };

            return View(respuestaModel);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Autores autores)
        {
            if (!ModelState.IsValid)
            {
                return View(autores);
            }

            await repositorioAutores.Crear(autores);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var modelo = await repositorioAutores.ObtenerPorId(id);
            if (modelo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var modelo = await repositorioAutores.ObtenerPorId(id);
            
            if (modelo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioAutores.Borrar(modelo.id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var modelo = await repositorioAutores.ObtenerPorId(id);
            if (modelo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(Autores autores)
        {
            var modelo = await repositorioAutores.ObtenerPorId(autores.id);

            if (modelo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioAutores.Actualizar(autores);
            return RedirectToAction("Index");
        }



    }
}
