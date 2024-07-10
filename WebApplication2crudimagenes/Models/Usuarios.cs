namespace WebApplication2crudimagenes.Models
{
    public class Usuarios
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string correo { get; set; }
        public string correoNormalizado { get; set; }

        public string passwordHash { get; set; }

    }
}
