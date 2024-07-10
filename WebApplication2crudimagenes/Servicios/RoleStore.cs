using Microsoft.AspNetCore.Identity;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{
    public class RoleStore : IRoleStore<Rol>
    {
        private readonly IRepositorioRoles repositorioRoles;

        public RoleStore(IRepositorioRoles repositorioRoles)
        {
            this.repositorioRoles = repositorioRoles;
        }

        public async Task<IdentityResult> CreateAsync(Rol role, CancellationToken cancellationToken)
        {
            await repositorioRoles.crearRol(role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Rol role, CancellationToken cancellationToken)
        {
            var resultado = await repositorioRoles.eliminarRol(role);
            return resultado > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<Rol> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await repositorioRoles.buscarPorId(roleId);
        }

        public async Task<Rol> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await repositorioRoles.buscarPorRolNombre(normalizedRoleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(Rol role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.nombreNormalizado);
        }

        public Task<string> GetRoleIdAsync(Rol role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.id);
        }

        public Task<string> GetRoleNameAsync(Rol role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.nombre);

        }

        public Task SetNormalizedRoleNameAsync(Rol role, string normalizedName, CancellationToken cancellationToken)
        {
            role.nombreNormalizado = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Rol role, string roleName, CancellationToken cancellationToken)
        {
            role.nombre  = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Rol role, CancellationToken cancellationToken)
        {
            var resultado = await repositorioRoles.actualizar(role);
            return resultado > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }
    }
}
