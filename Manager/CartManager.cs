using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Cart;
using Ecommerce_ASP.NET.DTOs.Product;
using Ecommerce_ASP.NET.Models;
using System.Reflection.Metadata.Ecma335;

namespace Ecommerce_ASP.NET.Manager
{
    public class CartManager
    {
        private readonly AppDbContext dbContext;
        public readonly CartDto cartDto;
        public CartManager(AppDbContext dbContext, CartDto cartDto)
        {
            this.dbContext = dbContext;
            this.cartDto = cartDto;
        }
        public CartDto? AddToCart(int userId, int productId, int quantity)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null)
                throw new UnauthorizedAccessException("User Not Found!");

            var product = dbContext.Products.FirstOrDefault(p => p.id == productId);
            if (product == null)
                return null;

            if (quantity > product.stock)
                return null;

            var existItem = dbContext.CartItems
                .FirstOrDefault(ci => ci.UserId == userId && ci.productId == productId);

            if (existItem != null)
            {
                existItem.quantity += quantity;
                existItem.updated_at = DateTime.Now;
                

                dbContext.SaveChanges();

                return new CartDto
                {
                    productId = productId,
                    quantity = existItem.quantity
                };
            }
            else
            {
                var item = new CartItems
                {
                    UserId = userId,
                    productId = productId,
                    quantity = quantity,
                    created_at = DateTime.Now
                };

                

                dbContext.CartItems.Add(item);
                dbContext.SaveChanges();

                return new CartDto
                {
                    productId = productId,
                    quantity = quantity
                };
            }
        }

    }
}
