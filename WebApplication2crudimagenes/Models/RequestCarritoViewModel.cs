namespace WebApplication2crudimagenes.Models
{
    public class RequestCarritoViewModel
    {

        public IEnumerable<itemsCarritoViewModel> itemsCarrito { get; set; }
        public string subTotal { get; set; }
        public string total { get; set; }
    }
}
