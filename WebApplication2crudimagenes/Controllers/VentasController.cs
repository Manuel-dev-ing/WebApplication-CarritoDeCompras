using Microsoft.AspNetCore.Mvc;
using WebApplication2crudimagenes.Servicios;

namespace WebApplication2crudimagenes.Controllers
{
    public class VentasController:Controller
    {
        private readonly IRepositorioVentas repositorioVentas;

        public VentasController(IRepositorioVentas repositorioVentas)
        {
            this.repositorioVentas = repositorioVentas;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var modelo = await repositorioVentas.obtener();
            
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var modelo = await repositorioVentas.obtenerPorId(id);
            if (modelo == null)
            {
                return NotFound();
            }

            var detalleVenta = await repositorioVentas.obtenerDetalleVenta(modelo.id_venta);

            return View(detalleVenta);
        }


    }
}
