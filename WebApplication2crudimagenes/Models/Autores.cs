using System.ComponentModel.DataAnnotations;

namespace WebApplication2crudimagenes.Models
{
    public class Autores
    {
        public int id { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string nombre { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El campo Apellidos es requerido")]
        public string apellidos { get; set; }

        [Display(Name = "Direccion")]
        [Required(ErrorMessage = "El campo Direccion es requerido")]
        public string direccion { get; set; }

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El campo Telefono es requerido")]
        [StringLength(maximumLength: 13, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        public string telefono { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo Fecha de Nacimiento es requerido")]
        public DateTime fechaNacimiento { get; set; } = DateTime.Today;
    }
}
