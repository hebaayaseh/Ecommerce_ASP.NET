using Ecommerce_ASP.NET.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("orders")]
        public int orderId { get; set; }
        public Orders orders { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
