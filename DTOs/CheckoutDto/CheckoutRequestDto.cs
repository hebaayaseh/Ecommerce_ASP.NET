using Ecommerce_ASP.NET.DTOs.Address;
using Ecommerce_ASP.NET.DTOs.Cart;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.Payment;

namespace Ecommerce_ASP.NET.DTOs.CheckoutDto
{
    public class CheckoutRequestDto
    {
        public CartDto cartDto { get; set; }
        public AddressDto Address { get; set; }
        public DiscountUserDto? Discount { get; set; }
        public PaymentDto Payment { get; set; }
        
        
    }
}
