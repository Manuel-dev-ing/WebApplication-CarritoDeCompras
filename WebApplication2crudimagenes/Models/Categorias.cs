using System.ComponentModel.DataAnnotations;

namespace WebApplication2crudimagenes.Models
{
    public class Categorias
    {

        public int id { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo Categoria es requerido")]
        public string nombre { get; set; }

    }

}
