using System.ComponentModel.DataAnnotations;

namespace WebApplication2crudimagenes.Models
{
    public class Editorial
    {
        public int id { get; set; }
        [StringLength(maximumLength: 50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string nombre { get; set; }
        [Display(Name = "Direccion")]
        [Required(ErrorMessage = "El campo Direccion es requerido")]
        public string direccion { get; set; }
        [StringLength(maximumLength: 50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "El campo Ciudad es requerido")]
        public string ciudad { get; set; }

        [Display(Name = "Numero Telefono")]
        [Required(ErrorMessage = "El campo Numero Telefono es requerido")]
        [DataType(DataType.PhoneNumber)]
        public string telefono { get; set; }

        [Display(Name = "Correo Electronico")]
        [Required(ErrorMessage = "El campo Correo Electronico es requerido")]
        [DataType(DataType.EmailAddress)]
        public string correo { get; set; }


    }
}
