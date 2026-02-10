using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Notification;
using Ecommerce_ASP.NET.DTOs.Search;
using Ecommerce_ASP.NET.Models.Enums;

namespace Ecommerce_ASP.NET.Manager
{
    public class InventoryManagementManager
    {
        public readonly AppDbContext dbContext;
        public readonly NotificationManager notificationManager;
        public InventoryManagementManager(AppDbContext dbContext,NotificationManager notificationManager)
        {
            this.dbContext = dbContext;
            this.notificationManager = notificationManager;
        }
        public List<ProductDto>? GetAllProducts(int page = 5, int pageSize = 10)
        {
            var products = dbContext.Products.Skip((page - 1) * pageSize)
                .Take(pageSize).Select(p => new ProductDto
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    ImageUrl = p.image_url,
                    Quantity = p.stock
                }).ToList();
            return products;
        }
        public ProductDto? GetProductById(int productId)
        {
            var product = dbContext.Products.Where(p => p.id == productId)
                .Select(p => new ProductDto
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    ImageUrl = p.image_url,
                    Quantity = p.stock
                }).FirstOrDefault();
            return product;
        }
        public void UpdateProductStock(int productId, int newStock)
        {
            var product = dbContext.Products.Where(p => p.id == productId).FirstOrDefault();
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found!");
            }
            product.stock = newStock;
            dbContext.SaveChanges();
        }
        public ProductDto? GetLowStok()
        {
            var product = dbContext.Products.OrderBy(p => p.stock).Select(p => new ProductDto
            {
                Id = p.id,
                Name = p.name,
                Price = p.price,
                ImageUrl = p.image_url,
                Quantity = p.stock
            }).FirstOrDefault();
            return product;
        }
        public ProductDto? GetOutOfStock()
        {
            var product = dbContext.Products.Where(p => p.stock == 0).Select(p => new ProductDto
            {
                Id = p.id,
                Name = p.name,
                Price = p.price,
                ImageUrl = p.image_url,
                Quantity = p.stock
            }).FirstOrDefault();
            return product;
        }
        public NotificationDto CheckStockAlert(,int adminId,int productId)
        {
            var product = dbContext.Products.Where(p => p.id == productId).FirstOrDefault();
            if (product == null) return null;
            if(product.stock<=5)
            {
                notificationManager.SendNotificationToUser(adminId,
                    "Stock is low for product " ,NotificationType.)
                
            }

        }
    }
}
