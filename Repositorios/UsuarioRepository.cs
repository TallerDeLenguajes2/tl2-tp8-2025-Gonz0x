using Microsoft.Data.Sqlite;
using tl2_tp8_2025_Gonz0x.Interfaces;
using tl2_tp8_2025_Gonz0x.Models;

namespace tl2_tp8_2025_Gonz0x.Repositorios
{
    public class UsuarioRepository : IUserRepository
    {
        private readonly string _cadenaConexion;

        public UsuarioRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public Usuario? GetById(int id)
        {
            const string sql = @"
                SELECT Id, Nombre, User, Pass, Rol
                FROM Usuarios
                WHERE Id = @Id
            ";

            using var conexion = new SqliteConnection(_cadenaConexion);
            conexion.Open();

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Id", id);
            using var reader = comando.ExecuteReader();
            if (!reader.Read())
                throw new Exception($"No existe el usuario con ID {id}");

            return new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }

        public Usuario? GetUser(string usuario, string contrasena)
        {
            Usuario user = null;

            const string sql = @"
                SELECT Id, Nombre, User, Pass, Rol
                FROM Usuarios
                WHERE User = @Usuario AND Pass = @Contrasena
            ";

            using var conexion = new SqliteConnection(_cadenaConexion);
            conexion.Open();

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Usuario", usuario);
            comando.Parameters.AddWithValue("@Contrasena", contrasena);

            using var reader = comando.ExecuteReader();
            if (!reader.Read())
                throw new Exception("Usuario o contraseña incorrectos");

            return new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }
    }
}
