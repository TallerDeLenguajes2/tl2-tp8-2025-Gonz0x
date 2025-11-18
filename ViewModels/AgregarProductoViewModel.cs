using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace tl2_tp8_2025_Gonz0x.ViewModels
{
    public class AgregarProductoViewModel
    {
        [Required]
        public int IdPresupuesto { get; set; }
        
        [Display(Name = "Producto a agregar")]
        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        public int IdProducto { get; set; }

         [Display(Name = "Producto a agregar")]
        [Required(ErrorMessage = "Debe ingresar una cantidad.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }
        
        [ValidateNever]
        public SelectList ListaProductos { get; set; }
    }
}
