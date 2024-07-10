using Dapper;
using System.Data;
using System.Data.SqlClient;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{
    public interface IRepositorioCarrito
    {
        Task<int> Contar();
        Task<IEnumerable<Categorias>> obtenerCategorias();
        Task<IEnumerable<LibrosCarrito>> ObtenerLibrosCarrito(PaginacionViewModel paginacion, string categorias);
        Task<IEnumerable<librosItemsViewModel>> obtenerLibros();
        Task<LibrosCarrito> ObtenerPorId(int id);
        Task Vender(RequestCarritoViewModel request, int id_usuario, string fechaVenta, decimal iva, decimal subTotal, decimal total, string fechaCreacion);
    }


    public class RepositorioCarrito : IRepositorioCarrito
    {
        private readonly string ConnectionString;

        public RepositorioCarrito(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Categorias>> obtenerCategorias()
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryAsync<Categorias>("SELECT * from Categorias WHERE estado = 1 ORDER BY nombre DESC");
        }

        public async Task<IEnumerable<librosItemsViewModel>> obtenerLibros()
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryAsync<librosItemsViewModel>("SELECT Id, titulo, idAutor, precio, stock FROM Libros");
        }

        public async Task<IEnumerable<LibrosCarrito>> ObtenerLibrosCarrito(PaginacionViewModel paginacion, string categorias)
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryAsync<LibrosCarrito>("sp_listarLibrosCarrito", new { 
                categorias,
                paginacion.RecordsASaltar,
                paginacion.RecordsPorPagina

            }, commandType: CommandType.StoredProcedure);
        }


        public async Task Vender(RequestCarritoViewModel request, int id_usuario, string fechaVenta, decimal iva, decimal subTotal, decimal total, string fechaCreacion)
        {
            using var connection = new SqlConnection(ConnectionString);


            var libroTable = new DataTable();
            libroTable.Columns.Add("Id", typeof(string));
            libroTable.Columns.Add("Titulo", typeof(string));
            libroTable.Columns.Add("Autor", typeof(string));
            libroTable.Columns.Add("Precio", typeof(string));
            libroTable.Columns.Add("Cantidad", typeof(int));

            foreach (var item in request.itemsCarrito)
            {
                libroTable.Rows.Add(item.Id, item.Titulo, item.Autor, item.Precio, item.Cantidad);

            }

            var parametro = new DynamicParameters();

            parametro.Add("id_usuario", id_usuario);
            parametro.Add("fechaVenta", fechaVenta);
            parametro.Add("iva", iva);
            parametro.Add("subTotal", subTotal);
            parametro.Add("total", total);
            parametro.Add("fechaCreacion", fechaCreacion);
            parametro.Add("Libros", libroTable.AsTableValuedParameter("TVP_Libros"));
            await connection.ExecuteAsync("sp_procesoVenta", parametro, commandType: CommandType.StoredProcedure);

        }


        public async Task<int> Contar()
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Libros WHERE estado = 1");
        }

        public async Task<LibrosCarrito> ObtenerPorId(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryFirstOrDefaultAsync<LibrosCarrito>(@"SELECT l.Id as Id, Titulo, CONCAT(a.nombre, ' ', a.apellidos) as Autor, c.nombre as Categoria,
	            e.nombre as Editorial,imagenPortada, isbn, fechaPublicacion, numeroPaginas,
	            idioma, sipnosis 
	            FROM Libros l
	            INNER JOIN Autores a ON l.idAutor = a.id
	            INNER JOIN Editorial e ON l.idEditorial = e.id
	            INNER JOIN Categorias c ON l.idCategorias = c.id
	            WHERE l.Id = @id AND l.estado = 1", new { id });
        }

    }
}
