using Ecommerce_ASP.NET.DTOs.AddOrderItem;
using Ecommerce_ASP.NET.DTOs.Address;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.Payment;
using Ecommerce_ASP.NET.DTOs.Product;
using Ecommerce_ASP.NET.Models;
namespace Ecommerce_ASP.NET.DTOs.Order
{
    public class AddOrder
    {
        
        public ICollection<AddOrderItems>? addOrderItem { get; set; }
        
        public DiscountDto? DiscountCode { get; set; }
        public AddressDto? Address { get; set; }
        public PaymentDto? PaymentMethod { get; set; }

    }
}
