using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using WebApplication2crudimagenes.Models;
using WebApplication2crudimagenes.Servicios;

namespace WebApplication2crudimagenes.Controllers
{
    
    public class UsuarioController: Controller
    {
        private readonly UserManager<Usuarios> userManager;
        private readonly SignInManager<Usuarios> signInManager;
        private readonly IRepositorioUsuarios repositorioUsuarios;
        private readonly RoleManager<Rol> roleManager;

        //private string Admin = "Administrador";

        public UsuarioController(UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager, 
            IRepositorioUsuarios repositorioUsuarios, RoleManager<Rol> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.repositorioUsuarios = repositorioUsuarios;
            this.roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index(PaginacionViewModel paginacion)
        {
            var modelo = await repositorioUsuarios.obtenerUsuarios(paginacion);
            var totalUsuarios = await repositorioUsuarios.contarUsuarios();

            var respuestaModel = new PaginacionRespuesta<UsuariosViewModel>()
            {
                Elementos = modelo,
                Pagina = paginacion.Pagina,
                RecordsPorPagina = paginacion.RecordsPorPagina,
                CantidadTotalRecords = totalUsuarios,
                BaseURL = "/Usuario"
            };

            return View(respuestaModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Crear()
        {
            var modelo = new usuariosCreacionViewModel();
            modelo.tiposRol = await ObtenerTiposRol();

            return View(modelo);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Crear(usuariosCreacionViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                modelo.tiposRol = await ObtenerTiposRol();
                return View(modelo);
            }

            var usuario = new Usuarios()
            {
                nombre = modelo.nombre,
                apellidos = modelo.apellidos,
                correo = modelo.correo,

            };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.password);


            var resultadoBuscarPorCorreo = await userManager.FindByNameAsync(usuario.correo.ToUpper());

            var nombreRol = await repositorioUsuarios.obtenerRolPorId(modelo.idRol);

            var resultadoCreacionRol = await userManager.AddToRoleAsync(resultadoBuscarPorCorreo, nombreRol.nombre);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, usuario.nombre),
                new Claim(ClaimTypes.Surname, usuario.apellidos),
                new Claim(ClaimTypes.Role, nombreRol.nombre)

            };

            if (resultado.Succeeded && resultadoCreacionRol.Succeeded)
            {
                await userManager.AddClaimsAsync(usuario, claims);

                return RedirectToAction("Index");

            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                modelo.tiposRol = await ObtenerTiposRol();
                return View(modelo);
            }

        }


        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Borrar(int id)
        {
            var modelo = await repositorioUsuarios.obtenerUsuarioPorId(id);
            if (modelo == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(modelo);
        
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BorrarUsuario(int id)
        {
            var modelo = await repositorioUsuarios.obtenerUsuarioPorId(id);
            if (modelo == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            

            await repositorioUsuarios.eliminarUsuarioRol(modelo.id);


            await repositorioUsuarios.borrarUsuarioPorId(modelo.id);
            return RedirectToAction("Index", "Usuario");

        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int id)
        {
            var modelo = await repositorioUsuarios.buscarUsuarioPorId(id);
            if (modelo == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            modelo.tiposRol = await ObtenerTiposRol();
            var exiteRol = await repositorioUsuarios.buscarRolporUsuario(id);


            var editarViewModel = new EditarUsuarioViewModel()
            {
                id = modelo.id,
                nombre = modelo.nombre,
                apellidos = modelo.apellidos,
                correo = modelo.correo,
                exiteRol = exiteRol,
                tiposRol = modelo.tiposRol,
                idRol = modelo.idRol
            };

            return View(editarViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(EditarUsuarioViewModel modelo)
        {

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var roleId = modelo.idRol.ToString();
            var rol = await roleManager.FindByIdAsync(roleId);
            var resultadoBuscarPorCorreo = await userManager.FindByNameAsync(modelo.correo.ToUpper());


            if (rol != null)
            {
                await userManager.AddToRoleAsync(resultadoBuscarPorCorreo, rol.nombre);
            }

            await repositorioUsuarios.actualizarUsuario(modelo);

            return RedirectToAction("Index", "Usuario");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new Usuarios()
            {
                nombre = modelo.Nombre,
                apellidos = modelo.Apellidos,
                correo = modelo.Correo,

            };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);



            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, usuario.nombre),
                new Claim(ClaimTypes.Surname, usuario.apellidos),

            };

            if (resultado.Succeeded)
            {
                await userManager.AddClaimsAsync(usuario, claims);

                await signInManager.SignInAsync(usuario, isPersistent: true);

               
                return RedirectToAction("Index", "Carrito");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(modelo);
            }
        }

        [HttpGet]
        public IActionResult Login(string urlRetorno = null)
        {
            ViewBag.urlRetorno = urlRetorno;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel modelo, string urlRetorno = null)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(modelo.Email, modelo.Password,
                modelo.Recuerdame, lockoutOnFailure: false);

            var usuario = await userManager.FindByNameAsync(modelo.Email);
            
            var rolNombre = await repositorioUsuarios.buscarRolPorId(usuario);

            var existeRol = await userManager.IsInRoleAsync(usuario, rolNombre);

            if (resultado.Succeeded)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.GivenName, usuario.nombre),
                    new Claim(ClaimTypes.Surname, usuario.apellidos),
                    

                };

                await userManager.AddClaimsAsync(usuario, claims);

                if (urlRetorno != null && existeRol)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    return LocalRedirect(urlRetorno);
                }


               /* if (existeRol)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Carrito");
                }*/
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Nombre de usuario o password incorrecto");
                return View(modelo);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Carrito");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposRol()
        {
            var tiposRol = await repositorioUsuarios.obtenerRol();
            var resultado = tiposRol.Select(x => new SelectListItem(x.nombre, x.id.ToString())).ToList();

            var opcionPorDefecto = new SelectListItem("-- Seleccione una Categoria --", "0", true);
            resultado.Insert(0, opcionPorDefecto);
            return resultado;
        }
    }
}
