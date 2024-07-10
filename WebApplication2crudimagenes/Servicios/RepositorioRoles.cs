using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{
    public interface IRepositorioRoles
    {
        Task<int> actualizar(Rol role);
        Task<Rol> buscarPorId(string roleId);
        Task<Rol> buscarPorRolNombre(string normalizedRoleName);
        Task<int> crearRol(Rol role);
        Task<int> eliminarRol(Rol role);
    }



    public class RepositorioRoles: IRepositorioRoles
    {
        private readonly string connectionString;

        public RepositorioRoles(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }


        public async Task<int> crearRol(Rol role)
        {
            var connection = new SqlConnection(connectionString);

            return await connection.ExecuteAsync(@"INSERT INTO Rol (nombre) VALUES (@rol)",
               new { rol = role.nombre });
        }

        public async Task<int> eliminarRol(Rol role)
        {
            var connection = new SqlConnection(connectionString);
            return await connection.ExecuteAsync(@"DELETE FROM Rol WHERE id = @id_rol",
               new { id_rol = role.id });
        }

        public async Task<Rol> buscarPorId(string roleId)
        {
            var rolId = Convert.ToInt32(roleId);

            var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Rol>("SELECT * FROM Rol WHERE id = @id_rol",
                new { id_rol = rolId });
        }


        public async Task<Rol> buscarPorRolNombre(string normalizedRoleName)
        {
            var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Rol>("SELECT * FROM Rol WHERE nombreNormalizado = @nombre",
                new { nombre = normalizedRoleName });
        }
        
        public async Task<int> actualizar(Rol role)
        {
            var connection = new SqlConnection(connectionString);
            return await connection.ExecuteAsync(@"UPDATE Rol SET nombre = @nombreRol WHERE id = @id_rol",
               new { nombreRol = role.nombre, id_rol = role.id });
        }





    }
}
