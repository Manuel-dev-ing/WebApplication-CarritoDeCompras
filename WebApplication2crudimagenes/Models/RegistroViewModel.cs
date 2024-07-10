using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2crudimagenes.Models
{
    public class RegistroViewModel
    {

        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [EmailAddress]
        public string Correo { get; set; }
        [Required]
        public string Password { get; set; }


    }
}
