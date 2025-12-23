using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class Products
    {
        public int id {get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public string? image_url { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        
        public ICollection<CartItems> cartItems { get; set; }
        public ICollection<WishlistItems>? wishlistItems { get; set; }
        public ICollection<Review>? review { get; set; }
        
        public Categories category { get; set; }
        [ForeignKey("category")]
        public int categoryId { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
        public Products()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }


    }
}
