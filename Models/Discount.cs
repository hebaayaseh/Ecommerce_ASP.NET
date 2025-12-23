using Ecommerce_ASP.NET.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_ASP.NET.Models
{
    public class Discount
    {
        public int Id { get; set; }
        [Required]
        public string? Code { get; set; }
        public DiscountType Type { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public int minimumOrderAmount { get; set; }
        public ICollection<Orders>? Orders { get; set; }

        

        }
}
