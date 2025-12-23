using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class OrderItems
    {
        public int id { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        public int quantity { get; set; }
        public Orders? Order { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Products? Product { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
    }
}
