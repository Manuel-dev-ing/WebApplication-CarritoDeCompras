namespace WebApplication2crudimagenes.Models
{
    public class PaginacionRespuesta
    {
        public int Pagina { get; set; } = 1;
        public int RecordsPorPagina { get; set; } = 9;
        public int CantidadTotalRecords { get; set; }
        public int CantidadTotalDePaginas => (int)Math.Ceiling((double)CantidadTotalRecords / RecordsPorPagina);
        public string BaseURL { get; set; }

    }

    public class PaginacionRespuesta<T> : PaginacionRespuesta
    {
        public IEnumerable<Categorias> categorias { get; set; }
        
        public IEnumerable<T> Elementos { get; set; }
     
    }



}
