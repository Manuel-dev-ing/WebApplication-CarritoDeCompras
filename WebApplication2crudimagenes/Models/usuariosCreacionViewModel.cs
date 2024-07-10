using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2crudimagenes.Models
{
    public class usuariosCreacionViewModel
    {
        public int id { get; set; }

        [Display(Name = "Tipo de Rol")]
        public int idRol { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string nombre { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El campo Apellidos es requerido")]
        public string apellidos { get; set; }

        [EmailAddress]
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo Correo es requerido")]
        public string correo { get; set; }

        [DataType(DataType.Password)]
        
        public string password { get; set; }

        
        public IEnumerable<SelectListItem> tiposRol { get; set; }



    }
}
