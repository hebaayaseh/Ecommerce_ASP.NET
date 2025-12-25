using Ecommerce_ASP.NET.Models.Enums;

namespace Ecommerce_ASP.NET.DTOs.Discount
{
    public class DiscountUserDto
    {
        public string code { get; set; }
        public DiscountType discountType { get; set; }
    }
}
