using tl2_tp8_2025_Gonz0x.Models;
using Microsoft.Data.Sqlite;

namespace tl2_tp7_2025_Gonz0x.Repositorios.PresupuestosRepository
{
    public class PresupuestosRepository
    {
        private string cadenaConexion = "Data Source=Tienda.db";
        
        public void CrearPresupuesto(Presupuestos presupuesto)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "INSERT INTO Presupuestos(IdPresupuesto, NombreDestinatario, FechaCreacion) VALUES(@IdPresupuesto, @NombreDestinatario, @FechaCreacion)";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@IdPresupuesto", presupuesto.IdPresupuesto));
            comando.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
            comando.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
            //comando.Parameters.Add(new SqliteParameter("@Detalle", presupuesto.Detalle));
            comando.ExecuteNonQuery();
        }

        public List<Presupuestos> ListarPresupuestos()
        {
            var presupuestos = new List<Presupuestos>();
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "SELECT * FROM Presupuestos";
            using var comando = new SqliteCommand(sql, conexion);
            using SqliteDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                var presupuesto = new Presupuestos
                {
                    IdPresupuesto = Convert.ToInt32(reader["IdPresupuesto"]),
                    NombreDestinatario = reader["NombreDestinatario"].ToString(), 
                    FechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString()!),
                    Detalle = new List<PresupuestosDetalle>()
                };
                presupuestos.Add(presupuesto);
            }
            foreach (var presupuesto in presupuestos)
            {
                string sqlDetalle = @"
                    SELECT pd.IdProducto, p.Descripcion, p.Precio, pd.Cantidad
                    FROM PresupuestosDetalle pd
                    JOIN Productos p ON pd.IdProducto = p.IdProducto
                    WHERE pd.IdPresupuesto = @IdPresupuesto";

                using var comandoDetalle = new SqliteCommand(sqlDetalle, conexion);
                comandoDetalle.Parameters.Add(new SqliteParameter("@IdPresupuesto", presupuesto.IdPresupuesto));

                using var lectorDetalle = comandoDetalle.ExecuteReader();
                while (lectorDetalle.Read())
                {
                    var producto = new Productos
                    {
                        IdProducto = Convert.ToInt32(lectorDetalle["IdProducto"]),
                        Descripcion = lectorDetalle["Descripcion"].ToString(),
                        Precio = Convert.ToDouble(lectorDetalle["Precio"])
                    };

                    var detalle = new PresupuestosDetalle
                    {
                        Producto = producto,
                        Cantidad = Convert.ToInt32(lectorDetalle["cantidad"])
                    };

                    presupuesto.Detalle.Add(detalle);
                }
            }

            return presupuestos;
        }

        public void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = @"
                INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, Cantidad)
                VALUES (@IdPresupuesto, @IdProducto, @Cantidad)";
            
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@IdPresupuesto", idPresupuesto));
            comando.Parameters.Add(new SqliteParameter("@IdProducto", idProducto));
            comando.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));

            comando.ExecuteNonQuery();
        }

        public Presupuestos? ObtenerPresupuestoPorId(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = "SELECT IdPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos WHERE IdPresupuesto = @IdPresupuesto";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@IdPresupuesto", id));

            using var lector = comando.ExecuteReader();
            if (!lector.Read())
            {
                return null; // No existe
            }

            var presupuesto = new Presupuestos
            {
                IdPresupuesto = Convert.ToInt32(lector["IdPresupuesto"]),
                NombreDestinatario = lector["NombreDestinatario"].ToString(),
                FechaCreacion = DateTime.Parse(lector["FechaCreacion"].ToString()!),
                Detalle = new List<PresupuestosDetalle>()
            };

            lector.Close();

            // 2️⃣ Obtener los productos asociados a ese presupuesto
            string sqlDetalle = @"
                SELECT pd.IdProducto, p.Descripcion, p.Precio, pd.Cantidad
                FROM PresupuestosDetalle pd
                JOIN Productos p ON pd.IdProducto = p.IdProducto
                WHERE pd.IdPresupuesto = @IdPresupuesto";

            using var comandoDetalle = new SqliteCommand(sqlDetalle, conexion);
            comandoDetalle.Parameters.Add(new SqliteParameter("@IdPresupuesto", id));

            using var lectorDetalle = comandoDetalle.ExecuteReader();
            while (lectorDetalle.Read())
            {
                var producto = new Productos
                {
                    IdProducto = Convert.ToInt32(lectorDetalle["IdProducto"]),
                    Descripcion = lectorDetalle["Descripcion"].ToString(),
                    Precio = Convert.ToDouble(lectorDetalle["Precio"])
                };

                var detalle = new PresupuestosDetalle
                {
                    Producto = producto,
                    Cantidad = Convert.ToInt32(lectorDetalle["Cantidad"])
                };

                presupuesto.Detalle.Add(detalle);
            }

            return presupuesto;
        }

        public void EliminarPresupuesto(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "DELETE FROM Presupuestos WHERE IdPresupuesto = @IdPresupuesto";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@IdPresupuesto", id));
            comando.ExecuteNonQuery();
        }
    }
}