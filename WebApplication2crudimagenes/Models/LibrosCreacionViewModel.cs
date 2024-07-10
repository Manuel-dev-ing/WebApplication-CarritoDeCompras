using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2crudimagenes.Models
{
    public class LibrosCreacionViewModel: Libros
    {
        public IEnumerable<SelectListItem> tiposAutores { get; set; }
        public IEnumerable<SelectListItem> tiposCategorias { get; set; }
        public IEnumerable<SelectListItem> tiposEditoriales { get; set; }


        
        
    }
}
