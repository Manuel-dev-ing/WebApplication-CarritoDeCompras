using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{

    public interface IRepositoriLibros
    {
        Task Actualizar(Libros libros);
        Task Borrar(int id);
        Task<LibrosCreacionViewModel> Buscar(int id);
        Task<int> contarLibros();
        Task Crear(Libros libros);
        Task<IEnumerable<LibrosViewModel>> Obtener();
        Task<IEnumerable<LibrosViewModel>> ObtenerLibros(PaginacionViewModel paginacion);
    }


    public class RepositorioLibros:IRepositoriLibros
    {
        private readonly string? connectionString;

        public RepositorioLibros(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<IEnumerable<LibrosViewModel>> Obtener()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<LibrosViewModel>("sp_Listar", commandType: System.Data.CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<LibrosViewModel>> ObtenerLibros(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<LibrosViewModel>("sp_Listar", new { paginacion.RecordsASaltar, paginacion.RecordsPorPagina },
                                                                    commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> contarLibros()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Libros WHERE estado = 1");
        }

        public async Task Crear(Libros libros)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("sp_CrearLibros", new { 
                  libros.titulo, 
                  libros.idAutor, 
                  libros.idCategorias, 
                  libros.idEditorial, 
                  libros.imagenPortada, 
                  libros.isbn, 
                  libros.precio,
                  libros.stock,
                  libros.FechaPublicacion,
                  libros.sipnosis
            }, commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<LibrosCreacionViewModel> Buscar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<LibrosCreacionViewModel>(@"SELECT * FROM Libros WHERE Id = @id AND estado = 1 ", new { id });
        }

        public async Task<LibrosCreacionViewModel> BuscarLibros(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<LibrosCreacionViewModel>(@"SELECT * FROM Libros WHERE Id = @Id ", new { id });
        }

        public async Task Actualizar(Libros libros)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("sp_ActualizarLibros", libros, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("sp_EliminarLibros", new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }


    }
}
