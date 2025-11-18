using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tl2_tp8_2025_Gonz0x.ViewModels
{
    public class PresupuestoViewModel
    {
        public int IdPresupuesto { get; set; }

        [Display(Name = "Nombre o Email del Destinatario")]
        [Required(ErrorMessage = "El nombre del destinatario es obligatorio.")]
        public string NombreDestinatario { get; set; }

        [Display(Name = "Fecha de Creación")]
        [Required(ErrorMessage = "Debe ingresar una fecha.")]
        [DataType(DataType.Date)]
        //[CustomValidation(typeof(PresupuestoViewModel), nameof(ValidarFechaNoFutura))]
        public DateTime FechaCreacion { get; set; }

/*         public static ValidationResult? ValidarFechaNoFutura(DateTime fecha, ValidationContext context)
        {
            if (fecha > DateTime.Today)
                return new ValidationResult("La fecha no puede ser futura.");

            return ValidationResult.Success;
        } */
    }
}
