using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;
using WebApplication2crudimagenes.Models;
using WebApplication2crudimagenes.Servicios;

namespace WebApplication2crudimagenes.Controllers
{
    public class CarritoController:Controller
    {
        private readonly IRepositorioCarrito repositorioCarrito;
        private readonly SignInManager<Usuarios> signInManager;
        private readonly UserManager<Usuarios> userManager;

        public CarritoController(IRepositorioCarrito repositorioCarrito, SignInManager<Usuarios> signInManager,
            UserManager<Usuarios> userManager)
        {
            this.repositorioCarrito = repositorioCarrito;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }


        public async Task<IActionResult> Index(PaginacionViewModel paginacion, string categoria = "")
        {
            categoria = categoria == "Todo" ? categoria = "" : categoria;
            var modelo = await repositorioCarrito.ObtenerLibrosCarrito(paginacion, categoria);
            var totalLibros = await repositorioCarrito.Contar();
            var categorias = await repositorioCarrito.obtenerCategorias();

            var respuestaVM = new PaginacionRespuesta<LibrosCarrito>
            {
                categorias = categorias,
                Elementos = modelo,
                Pagina = paginacion.Pagina,
                RecordsPorPagina = paginacion.RecordsPorPagina,
                CantidadTotalRecords = totalLibros,
                BaseURL = "/Carrito"
            };


            return View(respuestaVM);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var modelo = await repositorioCarrito.ObtenerPorId(id);

            if (modelo is null)
            {
                return NotFound();
            }

            
            return View(modelo);
        }

        /*[HttpGet]
        public IActionResult Comprar()
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Index", "Carrito");

            }
            else
            {
                return RedirectToAction("Login", "Usuario");


            }

        }*/
        
        [HttpPost]
        public async Task<IActionResult> Comprar([FromBody] RequestCarritoViewModel request)
        {
            char caracterAremover = '$';
            var resultadoSubTotal = request.subTotal.Replace(caracterAremover.ToString(), string.Empty);
            var resultadoTotal = request.total.Replace(caracterAremover.ToString(), string.Empty);

            var libros = await repositorioCarrito.obtenerLibros();


            foreach (var item in request.itemsCarrito)
            {
                foreach (var libro in libros)
                {
                    if (item.Id == libro.Id)
                    {
                        if (item.Cantidad > libro.Stock)
                        {
                            var lista = item.Titulo;
                            return BadRequest(new { mensaje = lista, code = "400" });
                        }
                    }
                }
            }

            var correoUsuario = User.Identity.Name;
            var usuario = await userManager.FindByNameAsync(correoUsuario);
            var id_usuario = usuario.id;
            var fechaVenta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var subTotal = Convert.ToDecimal(resultadoSubTotal);
            var total = Convert.ToDecimal(resultadoTotal);
            var fechaCreacion = DateTime.Now.ToString("yyyy-MM-dd");
            var iva = total - subTotal;
            var ivaa = iva;

            await repositorioCarrito.Vender(request, id_usuario, fechaVenta, iva, subTotal, total, fechaCreacion);
            return Ok();

        }

        [HttpGet]
        public IActionResult DetalleCarrito()
        {
            ViewBag.urlRetorno = HttpContext.Request.Path + HttpContext.Request.QueryString;

            return View();
        }


    }
}
