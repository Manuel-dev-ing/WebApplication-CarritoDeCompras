using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{
    public interface IRepositorioEditorial
    {
        Task<Editorial> borrar(int id);
        Task<int> contarEditorial();
        Task Crear(Editorial editorial);
        Task Editar(Editorial editorial);
        Task<IEnumerable<Editorial>> Obtener();
        Task<IEnumerable<Editorial>> ObtenerEditorial(PaginacionViewModel paginacion);
        Task<Editorial> ObtenerPorId(int id);
    }


    public class RepositorioEditorial: IRepositorioEditorial
    {
        private readonly string ConnectionString;

        public RepositorioEditorial(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Editorial> ObtenerPorId(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryFirstOrDefaultAsync<Editorial>(@"
                SELECT * FROM Editorial WHERE estado = 1 AND id = @id", new { id });
        }

        public async Task<IEnumerable<Editorial>> Obtener()
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryAsync<Editorial>(@"SELECT * FROM Editorial WHERE estado = 1");
        }
        public async Task<IEnumerable<Editorial>> ObtenerEditorial(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryAsync<Editorial>(@"SELECT * FROM Editorial WHERE estado = 1
                                                       ORDER BY nombre
                                                       OFFSET @RecordsASaltar ROWS FETCH NEXT @RecordsPorPagina ROWS ONLY",
                                                       new { paginacion.RecordsASaltar, paginacion.RecordsPorPagina });
        }

        public async Task<int> contarEditorial()
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Editorial WHERE estado = 1");
        }

        public async Task Crear(Editorial editorial)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.ExecuteAsync("sp_CrearEditorial", new { 
                editorial.nombre,
                editorial.direccion,
                editorial.ciudad,
                editorial.telefono,
                editorial.correo
            }
            ,commandType: CommandType.StoredProcedure);
        
        }

        public async Task<Editorial> borrar(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryFirstOrDefaultAsync<Editorial>(@"
                UPDATE Editorial SET estado = 0 WHERE id = @id", new { id });
        }

        public async Task Editar(Editorial editorial)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.ExecuteAsync(@"sp_ActualizarEditorial", editorial, commandType: CommandType.StoredProcedure);
        }


    }
}
