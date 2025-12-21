using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class CartItems
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public ICollection<Products> Products { get; set; }
       
        public CartItems()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }

    }
}
