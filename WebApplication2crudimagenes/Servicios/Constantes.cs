using Microsoft.AspNetCore.Identity;
using WebApplication2crudimagenes.Models;
using System.Security.Claims;

namespace WebApplication2crudimagenes.Servicios
{
    public class Constantes
    {

        public const string RolAdmin = "Administrador";
        public const string RolUsuario = "Usuario";

        /*var usuarioCorreo = User.Identity.Name;
        var usuario = await userManager.FindByNameAsync(usuarioCorreo.ToUpper());
        var user = await userManager.IsInRoleAsync(usuario, Constantes.RolUsuario);
        */
    }
}
