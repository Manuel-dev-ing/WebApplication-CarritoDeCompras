using Microsoft.AspNetCore.Mvc;
using WebApplication2crudimagenes.Models;
using WebApplication2crudimagenes.Servicios;

namespace WebApplication2crudimagenes.Controllers
{
    public class CategoriasController:Controller
    {
        private readonly IRepositorioCategorias repositorioCategorias;

        public CategoriasController(IRepositorioCategorias repositorioCategorias)
        {
            this.repositorioCategorias = repositorioCategorias;
        }

        public async Task<IActionResult> Index(PaginacionViewModel paginacion)
        {
            var modelo = await repositorioCategorias.ObtenerCategorias(paginacion);
            var totalCategorias = await repositorioCategorias.contarCategorias();

            var respuestaModel = new PaginacionRespuesta<Categorias>()
            {
                Elementos = modelo,
                Pagina = paginacion.Pagina,
                RecordsPorPagina = paginacion.RecordsPorPagina,
                CantidadTotalRecords = totalCategorias,
                BaseURL = "/Categorias"
            };


            return View(respuestaModel);
        }


        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categorias modelo)
        {
           
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            await repositorioCategorias.Crear(modelo);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var modelo = await repositorioCategorias.ObtenerPorId(id);

            if (modelo is null)
            {
                return NotFound();
            }

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCategoria(int id)
        {
            var modelo = await repositorioCategorias.ObtenerPorId(id);

            if (modelo is null)
            {
                return NotFound();
            }

            await repositorioCategorias.Borrar(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {

            var modelo = await repositorioCategorias.ObtenerPorId(id);
            if (modelo is null)
            {
                return NotFound();
            }


            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categorias categorias)
        {
            if (!ModelState.IsValid)
            {
                return View(categorias);
            }

            await repositorioCategorias.Actualizar(categorias);

            return RedirectToAction("Index");
        }



    }
}
