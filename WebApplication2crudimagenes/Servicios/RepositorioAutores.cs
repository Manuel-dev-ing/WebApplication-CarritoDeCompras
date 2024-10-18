using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{
    public interface IRepositorioAutores
    {
        Task Actualizar(Autores autores);
        Task Borrar(int id);
        Task<int> contarAutores();
        Task Crear(Autores autores);
        Task<IEnumerable<Autores>> Obtener();
        Task<IEnumerable<Autores>> ObtenerAutores(PaginacionViewModel paginacion);
        Task<Autores> ObtenerPorId(int id);
    }


    public class RepositorioAutores: IRepositorioAutores
    {
        private readonly string? connectionString;

        public RepositorioAutores(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Autores>> Obtener()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Autores>(@"SELECT * FROM Autores WHERE estado=1");

        }

        public async Task<IEnumerable<Autores>> ObtenerAutores(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Autores>(@"SELECT * FROM Autores WHERE estado=1
                                                       ORDER BY nombre
                                                       OFFSET @RecordsASaltar ROWS FETCH NEXT @RecordsPorPagina ROWS ONLY",
                                                       new { paginacion.RecordsASaltar, paginacion.RecordsPorPagina});

        }

        public async Task<int> contarAutores()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Autores WHERE estado = 1");
        }

        public async Task Crear(Autores autores)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("sp_crearAutores", new { 
                autores.nombre,
                autores.apellidos,
                autores.direccion,
                autores.telefono,   
                autores.fechaNacimiento
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE Autores SET estado = 0 WHERE id = @id", new { id });
        }

        public async Task<Autores> ObtenerPorId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Autores>(@"
                SELECT * FROM Autores WHERE id = @id AND estado=1", new { id });
        }

        public async Task Actualizar(Autores autores)
        {

            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("sp_ActualizarAutor", new { 
                autores.id,
                autores.nombre,
                autores.apellidos,
                autores.direccion,
                autores.telefono,
                autores.fechaNacimiento
            }, commandType: CommandType.StoredProcedure);
        }





    }


}
