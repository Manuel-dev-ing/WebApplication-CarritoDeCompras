namespace WebApplication2crudimagenes.Models
{
    public class Ventas
    {

        public int id { get; set; }
        public string usuario { get; set; }
        public DateTime fecha_venta { get; set; }
        public Decimal subTotal { get; set; }
        public Decimal impuesto { get; set; }
        public Decimal total { get; set; }
    
    }
}
