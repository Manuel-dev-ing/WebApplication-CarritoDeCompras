using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SqlClient;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Actualizar(Categorias categorias);
        Task Borrar(int id);
        Task<int> contarCategorias();
        Task Crear(Categorias categorias);
        Task<IEnumerable<Categorias>> Obtener();
        Task<IEnumerable<Categorias>> ObtenerCategorias(PaginacionViewModel paginacion);
        Task<Categorias> ObtenerPorId(int id);
    }


    public class RepositorioCategorias:IRepositorioCategorias
    {
        private readonly string ConnectionString;

        public RepositorioCategorias(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Categorias> ObtenerPorId(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryFirstOrDefaultAsync<Categorias>(@"SELECT * FROM Categorias WHERE id = @id AND estado = 1", new { id });
        }

        public async Task<IEnumerable<Categorias>> Obtener()
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryAsync<Categorias>("SELECT * FROM Categorias WHERE estado=1");
        }

        public async Task<IEnumerable<Categorias>> ObtenerCategorias(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryAsync<Categorias>(@"SELECT * FROM Categorias WHERE estado=1 
            ORDER BY nombre OFFSET @RecordsASaltar ROWS FETCH NEXT @RecordsPorPagina ROWS ONLY", 
            new { paginacion.RecordsASaltar, paginacion.RecordsPorPagina });
        }

        public async Task<int> contarCategorias()
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Categorias WHERE estado = 1");
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.ExecuteAsync("UPDATE Categorias SET estado = 0 WHERE id = @id", new { id });
        }


        public async Task Crear(Categorias categorias)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.ExecuteAsync("sp_CrearCategoria", new { 
                categorias.nombre 
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task Actualizar(Categorias categorias)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.ExecuteAsync("sp_ActualizarCategoria", new
            {
                categorias.id,
                categorias.nombre
            }, commandType: CommandType.StoredProcedure);

        }


    }
}
