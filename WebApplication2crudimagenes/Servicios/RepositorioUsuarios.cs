using Dapper;
using System.Data;
using System.Data.SqlClient;
using WebApplication2crudimagenes.Models;

namespace WebApplication2crudimagenes.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task actualizarUsuario(EditarUsuarioViewModel editarUsuarioViewModel);
        Task agregarRol(Usuarios usuarios, string rolNombre);
        Task borrarUsuarioPorId(int id);
        Task<Usuarios> BuscarPorId(int id);
        Task<string> buscarRolPorId(Usuarios usuarios);
        Task<Rol> buscarRolporUsuario(int id);
        Task<Usuarios> BuscarUsuarioPorCorreo(string correoNormalizado);
        Task<usuariosCreacionViewModel> buscarUsuarioPorId(int id);
        Task<int> contarUsuarios();
        Task CrearUsuario(Usuarios usuarios);
        Task eliminarUsuarioRol(int id);
        Task<IEnumerable<Rol>> obtenerRol();
        Task<IEnumerable<string>> obtenerRoles(Usuarios usuarios);
        Task<Rol> obtenerRolPorId(int id);
        Task<Rol> obtenerRolPorNombre(string nombreRol);
        Task<Usuarios> obtenerUsuarioPorId(int id);
        Task<UsuarioRol> obtenerUsuarioRol(Usuarios usuarios, Rol rol);
        Task<IEnumerable<UsuariosViewModel>> obtenerUsuarios(PaginacionViewModel paginacion);
    }


    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");   
        }

        public async Task borrarUsuarioPorId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM Usuarios WHERE id = @id", new { id });
        }

        public async Task<Usuarios> obtenerUsuarioPorId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Usuarios>(@"SELECT * FROM Usuarios WHERE id = @id", new { id });
        }

        public async Task<IEnumerable<Rol>> obtenerRol()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Rol>(@"SELECT * FROM Rol");
        }

        public async Task<Rol> obtenerRolPorId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Rol>(@"SELECT * FROM Rol WHERE id = @id", new { id });
        }

        public async Task<IEnumerable<UsuariosViewModel>> obtenerUsuarios(PaginacionViewModel paginacion)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<UsuariosViewModel>("sp_listarUsuarios", new { paginacion.RecordsASaltar, paginacion.RecordsPorPagina }, 
                                            commandType: CommandType.StoredProcedure);
        }

        public async Task eliminarUsuarioRol(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE FROM UsuarioRol WHERE usuario_id = @idUsuario", new { idUsuario = id } );
        }

        public async Task actualizarUsuario(EditarUsuarioViewModel editarUsuarioViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("sp_actualizarUsuario", new { 
                            editarUsuarioViewModel.id,
                            editarUsuarioViewModel.nombre,
                            editarUsuarioViewModel.apellidos,
                            editarUsuarioViewModel.correo,
                            editarUsuarioViewModel.idRol
                            }, commandType: CommandType.StoredProcedure );
        
        }

        public async Task CrearUsuario(Usuarios usuarios)
        {
            using var conecction = new SqlConnection(connectionString);
            await conecction.ExecuteAsync(@"INSERT INTO Usuarios(nombre, apellidos, correo, correoNormalizado,
                                passwordHash) 
                                VALUES(@nombre, @apellidos, @correo, @correoNormalizado, @passwordHash)", usuarios);

        }


        

        public async Task<Usuarios> BuscarUsuarioPorCorreo(string correoNormalizado)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuarios>(@"
                SELECT * FROM Usuarios WHERE correoNormalizado = @correoNormalizado", new { correoNormalizado = correoNormalizado.ToUpper() });
        }

        public async Task<Usuarios> BuscarPorId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuarios>(@"
                SELECT * FROM Usuarios WHERE id = @id", new { id });


        }
        public async Task<usuariosCreacionViewModel> buscarUsuarioPorId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<usuariosCreacionViewModel>(@"
                SELECT UsuarioRol.rol_id as idRol, Usuarios.nombre as nombre, Usuarios.apellidos as apellidos,
                Usuarios.correo as correo
                FROM Usuarios 
                LEFT JOIN UsuarioRol ON Usuarios.id = UsuarioRol.usuario_id
                WHERE id = @id", new { id });


        }

        public async Task agregarRol(Usuarios usuarios, string rolNombre)
        {
            var connection = new SqlConnection(connectionString);
            var role = await connection.QueryFirstOrDefaultAsync<Rol>(@"
                SELECT * FROM Rol WHERE nombre = @Nombre", new { Nombre = rolNombre });

            if (role == null)
            {
                throw new InvalidOperationException($"Rol '{rolNombre}' no existe ");
            }


            await connection.ExecuteAsync(@"INSERT INTO UsuarioRol (usuario_id, rol_id) VALUES (@userId, @rolId)",
               new { userId = usuarios.id, rolId = role.id });

        }

        public async Task<IEnumerable<string>> obtenerRoles(Usuarios usuarios)
        {
            var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<string>(@"SELECT r.nombre 
                                    FROM Rol r
                                    INNER JOIN UsuarioRol ur ON r.id  = ur.rol_id
                                    WHERE ur.usuario_id = @usuario_id", new { usuario_id = usuarios.id });

        }


        public async Task<Rol> buscarRolporUsuario(int id)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Rol>(@"
                      SELECT Rol.nombre FROM UsuarioRol  
                      INNER JOIN Rol ON UsuarioRol.rol_id = Rol.id
                      INNER JOIN Usuarios ON UsuarioRol.usuario_id = Usuarios.id
                      WHERE Usuarios.id = @usuario_id", new { usuario_id = id });
        }

        public async Task<string> buscarRolPorId(Usuarios usuarios)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<string>(@"
                      SELECT Rol.nombre FROM UsuarioRol  
                      INNER JOIN Rol ON UsuarioRol.rol_id = Rol.id
                      INNER JOIN Usuarios ON UsuarioRol.usuario_id = Usuarios.id
                      WHERE Usuarios.id = @usuario_id", new { usuario_id = usuarios.id });
        }

        public async Task<Rol> obtenerRolPorNombre(string nombreRol)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Rol>(@"SELECT * FROM
                                                Rol WHERE nombre = @nombre", new { nombre = nombreRol });
        }


        public async Task<UsuarioRol> obtenerUsuarioRol(Usuarios usuarios, Rol rol)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<UsuarioRol>(@"SELECT * FROM
            UsuarioRol WHERE usuario_id = @usuarioId AND rol_id = @rolId", 
            new { usuarioId = usuarios.id, rolId = rol.id });
        }

        public async Task<int> contarUsuarios()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Usuarios");
        }

    }

}
