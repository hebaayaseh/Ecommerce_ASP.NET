using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
    public class Orders
    {
        public int id { get; set; }
        public decimal totalPrice { get; set; }
        public OrderStatus status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }

        public Orders()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }

    }
}
