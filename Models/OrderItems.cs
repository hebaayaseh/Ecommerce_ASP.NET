using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class OrderItems
    {
        public int id { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public Orders Order { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Products Product { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
    }
}
