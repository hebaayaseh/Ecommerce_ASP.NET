using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class WishlistItems
    {
        public int id { get; set; }
        public DateTime AddedAt { get; set; }
        public ICollection<Products>? Products { get; set; }
        [ForeignKey("productId")]
        public int productId { get; set; }
        public ICollection<Wishlist>? wishlist { get; set; }
        [ForeignKey("wishlistId")]
        public int wishlistId { get; set; }
    }
}
