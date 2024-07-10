namespace WebApplication2crudimagenes.Models
{
    public class LibrosCarrito
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Autor { get; set; }

        public string Categoria { get; set; }

        public string Editorial { get; set; }

        public int precio { get; set; }

        public int stock { get; set; }

        public string imagenPortada { get; set; }

        public string isbn { get; set; }

        public string idioma { get; set; }
        
        public int numeroPaginas { get; set; }
        
        public DateTime FechaPublicacion { get; set; }

        public string sipnosis { get; set; }

    }
}
