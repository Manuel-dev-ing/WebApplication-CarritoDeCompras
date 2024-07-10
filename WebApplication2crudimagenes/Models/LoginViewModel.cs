using System.ComponentModel.DataAnnotations;

namespace WebApplication2crudimagenes.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public bool Recuerdame { get; set; }


    }


}
