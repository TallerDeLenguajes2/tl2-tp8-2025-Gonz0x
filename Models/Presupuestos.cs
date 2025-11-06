namespace tl2_tp8_2025_Gonz0x.Models
{
    public class Presupuestos
    {
        public int IdPresupuesto { get; set; }
        public string? NombreDestinatario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<PresupuestosDetalle> Detalle { get; set; }

        //Metodos
        public double MontoPresupuesto()
        {
            double total = 0;
            foreach (var detalle in Detalle)
            {
                total += detalle.Cantidad * detalle.Producto.Precio;
            }
            return total;
        }
        public double MontoPresupuestoConIva()
        {
            double total = MontoPresupuesto();
            return total * 1.21; // IVA 21%
        } //considerar iva 21
        public int CantidadProductos()
        {
            int cantidadTotal = 0;
            foreach (var detalle in Detalle)
            {
                cantidadTotal += detalle.Cantidad;
            }
            return cantidadTotal;
        } //contar total de productos
    }
}