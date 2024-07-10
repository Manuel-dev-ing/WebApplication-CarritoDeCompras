namespace WebApplication2crudimagenes.Models
{
    public class LibrosViewModel
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string autor { get; set; }
        public string categoria { get; set; }
        public string editorial { get; set; }
        public string isbn { get; set; }
        public DateTime fechaPublicacion { get; set; }
        public int estado { get; set; }
    }
}
