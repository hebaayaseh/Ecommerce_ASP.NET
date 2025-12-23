using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Product;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;

namespace Ecommerce_ASP.NET.Manager
{
    public class ProductManager
    {
        private readonly AppDbContext dbContext;
        private readonly CategoryManager categoryManager;
        public ProductManager (AppDbContext dbContext , CategoryManager categoryManager)
        {
            this.dbContext = dbContext;
            this.categoryManager = categoryManager;
        }
        public ICollection<Products>? AddExsistProduct(AddProduct productdto,int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin&&u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Only Admin Can Add Products!");
              var product=dbContext.Products.Where(p=>p.name== productdto.name).ToList();
            if (product == null) return null;
             return UpdateProduct(productdto);

        }

        private ICollection<Products> UpdateProduct(AddProduct productdto)
        {
            var product = dbContext.Products.Where(p => p.name == productdto.name).ToList();
            foreach (var prod in product)
            {
                prod.description = productdto.description;
                prod.price = productdto.price;
                prod.stock += productdto.quantity;
                prod.image_url = productdto.image;
                
            }
            dbContext.SaveChanges();
            return product;
        }
        public Products AddNewProduct(AddProduct productdto , int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Only Admin Can Add Products!");
            var product = new Products()
            {
                name = productdto.name,
                description = productdto.description,
                price = productdto.price,
                stock = productdto.quantity,
                image_url = productdto.image,
                category = productdto.category,

            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            return product;
        }
        
    }
}
