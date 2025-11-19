using tl2_tp8_2025_Gonz0x.Models;

namespace tl2_tp8_2025_Gonz0x.Interfaces
{
    public interface IPresupuestoRepository
    {
        List<Presupuestos> ListarPresupuestos();
        Presupuestos? ObtenerPresupuestoPorId(int id);
        void CrearPresupuesto(Presupuestos p);
        void ModificarPresupuesto(int id, Presupuestos p);
        void EliminarPresupuesto(int id);
        void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad);
    }
}