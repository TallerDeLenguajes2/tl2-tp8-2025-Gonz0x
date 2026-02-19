using tl2_tp8_2025_Gonz0x.Interfaces;
using tl2_tp8_2025_Gonz0x.Models;
using Microsoft.Data.Sqlite;

namespace tl2_tp8_2025_Gonz0x.Repositorios
{
    public class ProductosRepository : IProductoRepository
    {
        private string cadenaConexion;

        public ProductosRepository(string cadenaConexion)
        {
            this.cadenaConexion = cadenaConexion;
        }
        
        public void CrearProducto(Productos producto)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "INSERT INTO Productos(Descripcion, Precio) VALUES(@Descripcion, @Precio)";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            comando.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
            int filas = comando.ExecuteNonQuery();
            if(filas == 0)
                throw new Exception("No se pudo crear el producto");
        }

        public void ModificarProducto(int id, Productos producto)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @idProducto";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@idProducto", id));
            comando.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            comando.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
            int filas = comando.ExecuteNonQuery();
            if (filas == 0)
                throw new Exception($"No existe el producto con ID {id}");
        }

        public List<Productos> ListarProductos()
        {
            var productos = new List<Productos>();
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos";
            using var comando = new SqliteCommand(sql, conexion);
            using SqliteDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                var producto = new Productos
                {
                    IdProducto = Convert.ToInt32(reader["IdProducto"]),
                    Descripcion = reader["Descripcion"].ToString(), 
                    Precio = Convert.ToDouble(reader["Precio"])
                };
                productos.Add(producto);
            }
            if (productos.Count == 0)
                throw new Exception("No existen productos cargados");

            return productos;
        }

        public Productos ObtenerProductoPorId(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos WHERE IdProducto = @IdProducto";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@IdProducto", id));
            using var lector = comando.ExecuteReader();
            if (!lector.Read())
                throw new Exception($"No existe el producto con ID {id}");

            return new Productos
            {
                IdProducto = Convert.ToInt32(lector["IdProducto"]),
                Descripcion = lector["Descripcion"].ToString(),
                Precio = Convert.ToDouble(lector["Precio"])
            };

        }  

        public void EliminarProducto(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "DELETE FROM Productos WHERE idProducto = @idProducto";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@idProducto", id));
            int filas = comando.ExecuteNonQuery();
            if (filas == 0)
                throw new Exception($"No se pudo eliminar. Producto {id} inexistente");
        }
    }
}