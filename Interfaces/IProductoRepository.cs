using tl2_tp8_2025_Gonz0x.Models;

namespace tl2_tp8_2025_Gonz0x.Interfaces
{
    public interface IProductoRepository
    {
        List<Productos> ListarProductos();
        Productos? ObtenerProductoPorId(int id);
        void CrearProducto(Productos p);
        void ModificarProducto(int id, Productos p);
        void EliminarProducto(int id);
    }
}