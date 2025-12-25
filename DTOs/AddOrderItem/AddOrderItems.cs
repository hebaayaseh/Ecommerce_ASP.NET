
using Ecommerce_ASP.NET.DTOs.Address;
using Ecommerce_ASP.NET.DTOs.Product;

namespace Ecommerce_ASP.NET.DTOs.AddOrderItem
{
    public class AddOrderItems
    {
        public int id {  get; set; }
        public int quantity { get; set; }
        public int ProductId { get; set; }
        public decimal PriceAtPurchase { get; set; }
        
    }
}
