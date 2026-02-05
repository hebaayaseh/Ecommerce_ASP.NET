using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Cart;
using Ecommerce_ASP.NET.Models;
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
        public List<CartDto> GetCart(int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null)
                throw new UnauthorizedAccessException("User Not Found!");

            return dbContext.CartItems
                .Where(ci => ci.UserId == userId)
                .Select(ci => new CartDto
                {
                    Id = ci.id,
                    productId = ci.productId,
                    quantity = ci.quantity
                })
                .ToList();
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
        public void deleteCart(int userId,int cartId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId );
            if (user == null)
                throw new UnauthorizedAccessException("User Not Found!");
            var cart = dbContext.CartItems.FirstOrDefault(u => u.UserId == userId && u.id == cartId);
            if (cart == null) throw new Exception("Dont Have Cart!");
            dbContext.CartItems.Remove(cart);
            dbContext.SaveChanges();
        }
        public void deletProductFromCart(int userId,int productId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null)
                throw new UnauthorizedAccessException("User Not Found!");

            var product = dbContext.CartItems
                .Where(ci => ci.UserId == userId && ci.productId == productId)
                .ToList();

            if (!product.Any())
                throw new Exception("No Product Found In Cart!");

            dbContext.CartItems.RemoveRange(product);
            dbContext.SaveChanges();
        }
        public CartItems? DeleteQuantityForProduct(int userId, CartDto cartDto, int quantity)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null)
                throw new UnauthorizedAccessException("User Not Found!");

            var cartItem = dbContext.CartItems
                .FirstOrDefault(ci => ci.UserId == userId && ci.productId == cartDto.productId);

            if (cartItem == null)
                return null;

            if (quantity <= 0)
                return null;

            if (quantity > cartItem.quantity)
                return null;

            cartItem.quantity -= quantity;

            if (cartItem.quantity == 0)
            {
                dbContext.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.updated_at = DateTime.Now;
            }

            dbContext.SaveChanges();
            return cartItem;
        }
        
    }
}
