using Ecommerce_ASP.NET.DTOs.Product;

namespace Ecommerce_ASP.NET.DTOs.Cart
{
    public class CartDto
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        
    }
}
