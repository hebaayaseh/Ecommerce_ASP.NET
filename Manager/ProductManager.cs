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
             return UpdateProduct(productdto,userId);

        }

        public List<Products> UpdateProduct(AddProduct productdto,int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Only Admin Can Update Products!");
            var products = dbContext.Products.Where(p => p.name == productdto.name).ToList();
            
            foreach (var product in products)
            {
                product.description = productdto.description;
                product.price = productdto.price;
                product.stock += productdto.stock;
                product.image_url = productdto.image;
                
            }
            dbContext.SaveChanges();
            return products;
        }
        public void AddNewProduct(AddProduct productdto , int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            var category = dbContext.Categories.FirstOrDefault(c => c.id == productdto.category.Id);
            if(category==null) throw new KeyNotFoundException("Category Not Found! , Please Add Category First.");
            if (user == null) throw new UnauthorizedAccessException("Only Admin Can Add Products!");
            
            var product = new Products()
            {
                name = productdto.name,
                description = productdto.description,
                price = productdto.price,
                stock = productdto.stock,
                image_url = productdto.image,
                category = category,

            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            
        }
        public void UpdateProductStock(int productId,int newSocket,int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Only Admin Can Update Products!");
            var product = dbContext.Products.FirstOrDefault(p => p.id == productId);
            if (product == null) throw new KeyNotFoundException("Product Not Found!");
            product.stock = newSocket;
        }
        public void UploadImages(int userId, int productId, IFormFile imageFile)
        {
            var product = dbContext.Products
                .FirstOrDefault(p => p.id == productId && p.id == userId);

            if (product == null)
                throw new Exception("Product not found or access denied");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid image type");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var folderPath = Path.Combine("wwwroot", "product-images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            product.image_url = $"/product-images/{fileName}";
            product.updated_at = DateTime.Now;

            dbContext.SaveChanges();
        }

        public ICollection<Products>? GetAllProduct (int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId || u.role == UserRole.Customer && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Please Login!");
            var products = dbContext.Products.ToList();
            if (products == null) return null;  
            return products;
        }

        public Products? GetProductById(int productId , int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId || u.role == UserRole.Customer && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Please Login!");
            var product = dbContext.Products.FirstOrDefault(product => product.id == productId);
            if (product == null) return null;
            return product;
        }

        public ICollection<AddProduct>? GetProductsByCategory(int categoryId ,int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId || u.role == UserRole.Customer && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Please Login!");
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
        public AddProduct ? searchProductById(int productId,int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId || u.role == UserRole.Customer && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Please Login!");
            var product = dbContext.Products.Where(p => p.id == productId).FirstOrDefault();
            if (product == null) return null;
            var productDto = new AddProduct
            {
                name = product.name,
                price = product.price,
                stock = product.stock,
                description = product.description,
            };
            return productDto;
        }
        public ICollection<AddProduct>? SearchProductByName(string name,int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId || u.role == UserRole.Customer && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Please Login!");
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
        
        public decimal GetPrice(int productId)
        {
            var product = dbContext.Products.Where(p=>p.id==productId).FirstOrDefault();
            if(product==null)  throw new KeyNotFoundException("Not Found Product!");
            return product.price;
        }
    }
}
