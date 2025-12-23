using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class Address
    {
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string building { get; set; }
        [Required]
        public int PostalCode { get; set; }
        [Key]
        public int id { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        public User user { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public ICollection<Orders> orders { get; set; }
        
    }
}
