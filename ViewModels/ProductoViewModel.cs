using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace tl2_tp8_2025_Gonz0x.ViewModels
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }

        [Display(Name = "Descripción del Producto")]
        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        public string? Descripcion { get; set; }

        [Display(Name = "Precio Unitario")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public double Precio { get; set; }
    }
}
