using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Category;
using Ecommerce_ASP.NET.DTOs.Product;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ecommerce_ASP.NET.Manager
{
    public class ProductManager
    {
        private readonly AppDbContext dbContext;
        private readonly CategoryDto categoryDto;
        
        private readonly AddProduct addProduct;
        public ProductManager (AppDbContext dbContext , CategoryManager categoryManager , CategoryDto categoryDto  , AddProduct addProduct)
        {
            this.dbContext = dbContext;
            this.categoryDto = categoryDto;
            
            this.addProduct = addProduct;
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
                prod.stock += productdto.stock;
                prod.image_url = productdto.image;
                
            }
            dbContext.SaveChanges();
            return product;
        }
        public void AddNewProduct(AddProduct productdto , int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            var cat = dbContext.Categories.FirstOrDefault(c => c.id == productdto.category.Id);
            if(cat==null) throw new KeyNotFoundException("Category Not Found!");
            if (user == null) throw new UnauthorizedAccessException("Only Admin Can Add Products!");
            
            var product = new Products()
            {
                name = productdto.name,
                description = productdto.description,
                price = productdto.price,
                stock = productdto.stock,
                image_url = productdto.image,
                category = cat,

            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            
        }
        public ICollection<AddProduct>? GetAllProducts(int categoryId)
        {
            var products = dbContext.Products
                .Where(p => p.categoryId == categoryId)
                .Select(p => new AddProduct
                {
                    name = p.name,
                    price = p.price,
                    stock=p.stock,
                })
                .ToList();

            return products;
        }
        public void DeleteProduct(int productId , int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Only Admin Can Delete Products!");
            var product = dbContext.Products.FirstOrDefault(p => p.id == productId);
            if (product == null) throw new KeyNotFoundException("Product Not Found!");
            dbContext.Products.Remove(product);
        }
        public ICollection<AddProduct>? SearchProduct(string name)
        {
            var products = dbContext.Products.Where(p => p.name.Contains(name)).ToList()
            .Select(p => new AddProduct
             {
                name= p.name,
                price= p.price,
                stock= p.stock,
                description= p.description,
            });
            if(products == null ) return null;
            return products.ToList();
        }
    }
}
