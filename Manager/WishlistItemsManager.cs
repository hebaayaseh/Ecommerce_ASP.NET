using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Cart;
using Ecommerce_ASP.NET.DTOs.WishlistItems;
using Ecommerce_ASP.NET.Models;

namespace Ecommerce_ASP.NET.Manager
{
    public class WishlistItemsManager
    {
        private readonly AppDbContext dbContext;
        private readonly CartManager cartManager;
        public WishlistItemsManager(AppDbContext dbContext, CartManager cartManager)
        {
            this.dbContext = dbContext;
            this.cartManager = cartManager;
        }
        public ICollection<WishlistItemsDto> GetWishlistItems(int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null) throw new Exception("User not found");
            var wishlistItems = dbContext.wishlist.Where(w => w.userId == userId).Select(
                w => new WishlistItemsDto
                {
                    id = w.id,
                    AddedAt = w.AddedAt,
                    userId = w.userId,
                    productId = w.productId
                })
                .ToList();
            return wishlistItems;
        }
        public void AddProduct(int userId,int productId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null) throw new Exception("User not found");
            var WishlistItems = new Models.WishlistItems
            {
                userId = userId,
                productId = productId,
                AddedAt = DateTime.UtcNow
            };
            dbContext.wishlist.Add(WishlistItems);
            dbContext.SaveChanges();
        }
        public void RemoveProduct(int userId,int productId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null) throw new Exception("User not found");
            var WishlistItems = dbContext.wishlist.FirstOrDefault(w=>w.userId==userId && w.productId==productId);
            if (WishlistItems == null) throw new Exception("Product not found in wishlist");
            dbContext.wishlist.Remove(WishlistItems);
            dbContext.SaveChanges();
        }
        public CartDto? MoveToCart(int userId,int productId,int quantity)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null) throw new Exception("User not found");
            var WishlistItems = dbContext.wishlist.FirstOrDefault(w => w.userId == userId && w.productId == productId);
            if (WishlistItems == null) throw new Exception("Product not found in wishlist");
            RemoveProduct(userId, productId);
            return cartManager.AddToCart(userId, productId, quantity);
        }

    }
}
