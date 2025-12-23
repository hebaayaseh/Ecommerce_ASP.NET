using Ecommerce_ASP.NET.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    
    public class User
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string f_name { get; set; }
        public string? l_name { get; set; }
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string passwordHash { get; set; }
        [Required]
        public UserRole role { get; set; }
        public ICollection<Notification> notification { get; set; }
        public Wishlist? wishlist { get; set; }
        public string? phone { get; set; }
        public ICollection<Review>? review { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public ICollection<CartItems> cart { get; set; }
        public ICollection<Orders> orders { get; set; }
        public ICollection<Address>? addresses { get; set; }

        public User()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

        }

    }
}
