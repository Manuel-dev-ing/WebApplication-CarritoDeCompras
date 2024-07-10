using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2crudimagenes.Models
{
    public class Libros
    {
        public int Id { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "El campo Titulo es requerido")]
        public string titulo { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar un Autor")]
        [Display(Name = "Autor")]
        [Required(ErrorMessage = "El campo Autor es requerido")]
        public int idAutor { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una Categoria")]
        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo Categoria es requerido")]
        public int idCategorias { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una Editorial")]
        [Display(Name = "Editorial")]
        [Required(ErrorMessage = "El campo Editorial es requerido")]
        public int idEditorial { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo Precio es requerido")]
        public int precio { get; set; }

        [Display(Name = "Stock")]
        [Required(ErrorMessage = "El campo Stock es requerido")]
        public int stock { get; set; }

        public string imagenPortada { get; set; }

        [StringLength(maximumLength: 15, ErrorMessage = "El isbn no puede pasar de {1} caracteres")]
        [Display(Name = "ISBN")]
        [Required(ErrorMessage = "El campo ISBN es requerido")]
        public string isbn { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha de Publicacion")]
        [DataType(DataType.Date)]
        public DateTime FechaPublicacion { get; set; } = DateTime.Today;

        [Display(Name = "Sipnosis")]
        [Required(ErrorMessage = "El campo Sipnosis es requerido")]
        public string sipnosis { get; set; }


        [NotMapped]
        [Display(Name = "Imagen")]
        [Required(ErrorMessage = "El campo Imagen es requerido")]
        public IFormFile ImagenArchivo { get; set; }

        /*[NotMapped]
        public IFormFile File { get; set; }*/

    }

}
