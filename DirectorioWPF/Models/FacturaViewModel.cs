using System.ComponentModel.DataAnnotations;

namespace DirectorioWPF.Models
{
    public class FacturaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de factura es obligatorio")]
        public string NumeroFactura { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El ID de persona es obligatorio")]
        public int PersonaId { get; set; }

        public string Descripcion { get; set; }
        public string NombrePersona { get; set; }
        public string IdentificacionPersona { get; set; }
    }
}
