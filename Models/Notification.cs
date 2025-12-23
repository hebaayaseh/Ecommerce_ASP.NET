using Ecommerce_ASP.NET.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("user")]
        public int UserId { get; set; }
        public User user { get; set; }

        public string? Message { get; set; }
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationType Status { get; set; }
    }
}
