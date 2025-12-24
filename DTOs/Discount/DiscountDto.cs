using Ecommerce_ASP.NET.Models.Enums;

namespace Ecommerce_ASP.NET.DTOs.Discount
{
    public class DiscountDto
    {
        public int id { get; set; }
        public string code { get; set; }
        public DiscountType discountType { get; set; }
        public decimal amount { get; set; }
        public DateTime? ExpiryDate { get; set; }
<<<<<<< HEAD
        public int MaxUsage { get; set; }
=======
        public bool IsActive { get; set; }
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
>>>>>>> e580a701cd0cfeae561d9bffa2f9c6100d5bf0fc
        public int minimumOrderAmount { get; set; }

    }
}
