using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{
    public class UsuarioStore : IUserStore<Usuarios>, IUserEmailStore<Usuarios>, IUserPasswordStore<Usuarios>,
        IUserClaimStore<Usuarios>, IUserRoleStore<Usuarios> 
    {
        private readonly IRepositorioUsuarios repositorioUsuarios;

        private readonly List<IdentityUserClaim<string>> userClaims = new List<IdentityUserClaim<string>>();
        private readonly List<Usuarios> users = new List<Usuarios>();
        

        public UsuarioStore(IRepositorioUsuarios repositorioUsuarios)
        {
            this.repositorioUsuarios = repositorioUsuarios;
        }

        public Task AddClaimsAsync(Usuarios user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {

            foreach (var claim in claims)
            {
                userClaims.Add(new IdentityUserClaim<string>
                {
                    UserId = Convert.ToString(user.id),
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                });

            }
            return Task.CompletedTask;

        }

        public async Task AddToRoleAsync(Usuarios user, string roleName, CancellationToken cancellationToken)
        {

           await repositorioUsuarios.agregarRol(user, roleName);
           
        }

        public async Task<IdentityResult> CreateAsync(Usuarios user, CancellationToken cancellationToken)
        {
            await repositorioUsuarios.CrearUsuario(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(Usuarios user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }

        public Task<Usuarios> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuarios> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await repositorioUsuarios.BuscarPorId(Convert.ToInt32(userId));
        }

        public async Task<Usuarios> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var usuairoNormalizado = normalizedUserName.ToUpper();
            return await repositorioUsuarios.BuscarUsuarioPorCorreo(usuairoNormalizado);
        }

        public Task<IList<Claim>> GetClaimsAsync(Usuarios user, CancellationToken cancellationToken)
        {
            var claims = userClaims.Where(c => c.UserId == Convert.ToString(user.id))
                .Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();

            return Task.FromResult<IList<Claim>>(claims);
        }

        public Task<string> GetEmailAsync(Usuarios user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.correo);
        }

        public Task<bool> GetEmailConfirmedAsync(Usuarios user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(Usuarios user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(Usuarios user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(Usuarios user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.passwordHash);
        }

        public async Task<IList<string>> GetRolesAsync(Usuarios user, CancellationToken cancellationToken)
        {
            var roles = await repositorioUsuarios.obtenerRoles(user);
            return roles.ToList();
        }

        public Task<string> GetUserIdAsync(Usuarios user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.id.ToString());
        }

        public Task<string> GetUserNameAsync(Usuarios user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.correo);
        }

        public Task<IList<Usuarios>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {

            var user = userClaims.Where(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value)
                .Select(c => users.FirstOrDefault(u => Convert.ToString(u.id) == c.UserId))
                .Where(u => u != null).ToList();

            return Task.FromResult<IList<Usuarios>>(user);
        }

        public Task<IList<Usuarios>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(Usuarios user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        /*Se debe de implementar esta interfas*/
        public async Task<bool> IsInRoleAsync(Usuarios user, string roleName, CancellationToken cancellationToken)
        {

            var rol = await repositorioUsuarios.obtenerRolPorNombre(roleName);

            if (rol == null)
            {
                return false;
            }

            var usuarioRol = await repositorioUsuarios.obtenerUsuarioRol(user, rol);

            return usuarioRol != null;  

        }
        /*Se debe de implementar esta interfas*/


        public Task RemoveClaimsAsync(Usuarios user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(Usuarios user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(Usuarios user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetEmailAsync(Usuarios user, string email, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(Usuarios user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(Usuarios user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.correoNormalizado = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(Usuarios user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(Usuarios user, string passwordHash, CancellationToken cancellationToken)
        {
            user.passwordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(Usuarios user, string userName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(Usuarios user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);

        }



    }



}
