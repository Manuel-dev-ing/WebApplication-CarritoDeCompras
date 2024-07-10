using Dapper;
using System.Data.SqlClient;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{
    public interface IRepositorioVentas
    {
        Task<IEnumerable<Ventas>> obtener();
        Task<IEnumerable<DetalleVentaViewModel>> obtenerDetalleVenta(int id);
        Task<DetalleVentaViewModel> obtenerPorId(int id);
    }




    public class RepositorioVentas: IRepositorioVentas
    {
        private readonly string connectionString;

        public RepositorioVentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Ventas>> obtener()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Ventas>(@"
            SELECT Ventas.id, CONCAT(Usuarios.nombre,' ', Usuarios.apellidos) as usuario, fecha_venta, subtotal, impuesto, total 
            FROM Ventas
            INNER JOIN Usuarios ON Ventas.id_usuario = Usuarios.id");
        }

        public async Task<DetalleVentaViewModel> obtenerPorId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<DetalleVentaViewModel>(@"
            SELECT TOP 1 * FROM Detalle_Ventas WHERE id_venta = @id", new { id });

        }
        public async Task<IEnumerable<DetalleVentaViewModel>> obtenerDetalleVenta(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<DetalleVentaViewModel>(@"
            SELECT * FROM Detalle_Ventas WHERE id_venta = @id", new { id });
        }

    }
}
