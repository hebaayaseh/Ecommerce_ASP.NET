using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class CartItems
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int quantity { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public Products product { get; set; }
        [ForeignKey("product")]
        public int productId { get; set; }
       
        public CartItems()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }

    }
}
