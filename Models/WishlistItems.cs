using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class WishlistItems
    {
        [Key]
        public int id { get; set; }
        public DateTime AddedAt { get; set; }
        public Products? Products { get; set; }
        
        public User user { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        [ForeignKey("Products")]
        public int productId { get; set; }
        public WishlistItems()
        {
            AddedAt = DateTime.UtcNow;
        }
    }
}
