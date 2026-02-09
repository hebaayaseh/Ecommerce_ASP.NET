using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Search;

namespace Ecommerce_ASP.NET.Manager
{
    public class SearchManager
    {
        public readonly AppDbContext dbContext;
        public SearchManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<ProductDto>? GetProductsByCategortName(string categoryName)
        {
            var products = dbContext.Products.Where(p => p.category.name==categoryName)
                .Select(p => new ProductDto
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    ImageUrl = p.image_url,
                    Quantity = p.stock
                }).ToList();
            return products;
        }
        public List<ProductDto>? Suggestions(string Name)
        {
            var products = dbContext.Products.Where(p => p.category.name.Contains(Name))
                .Select(p => new ProductDto
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    ImageUrl = p.image_url,
                    Quantity = p.stock
                }).ToList();
            return products;
        }
        public List<ProductDto>? GetProductsByName(string productName)
        {
            var products = dbContext.Products.Where(p => p.name.Contains(productName))
                .Select(p => new ProductDto
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    ImageUrl = p.image_url,
                    Quantity = p.stock
                }).ToList();
            return products;
        }
        public List<ProductDto>? GetProductsRang(int minPrice,int maxPrice)
        {
            var products = dbContext.Products.Where(p=>p.price<=maxPrice&&p.price>=minPrice)
                .Select(p => new ProductDto
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    ImageUrl = p.image_url,
                    Quantity = p.stock
                }).ToList();
            return products;
        }
        public List<ProductDto>? GetProductsPriceAsc()
        {
            var products = dbContext.Products.OrderBy(p => p.price)
                .Select(p => new ProductDto
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    ImageUrl = p.image_url,
                    Quantity = p.stock
                }).ToList();
            return products;
        }
        public List<ProductDto>? GetProductsPriceDesc()
        {
            var products = dbContext.Products.OrderByDescending(p => p.price)
                .Select(p => new ProductDto
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    ImageUrl = p.image_url,
                    Quantity = p.stock
                }).ToList();
            return products;
        }
        public List<ProductDto>? GetNewestProduct()
        {
            var products = dbContext.Products.OrderByDescending(p => p.created_at)
                .Select(p => new ProductDto
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    ImageUrl = p.image_url,
                    Quantity = p.stock
                }).ToList();
            return products;
        }

        public ProductDto AutoComplete(string name)
        {
            var product = dbContext.Products.Where(p=>p.name.Contains(name))
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
    }
}
