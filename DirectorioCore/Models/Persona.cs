using System.ComponentModel.DataAnnotations;

namespace DirectorioCore.Models
{
    public class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        public string ApellidoPaterno { get; set; }

        public string? ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria")]
        public string Identificacion { get; set; }
    }
}
