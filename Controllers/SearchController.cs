using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        public readonly SearchManager searchManager;
        public SearchController(SearchManager searchManager)
        {
            this.searchManager = searchManager;
        }
        [Authorize]
        [HttpGet("GetProductsByCategoryName")]
        public IActionResult GetProductsByCategoryName([FromQuery] string categoryName)
        {
            var products = searchManager.GetProductsByCategortName(categoryName);
            if (products == null || !products.Any())
                return NotFound($"No products found in category '{categoryName}'");
            return Ok(products);
        }
        [Authorize]
        [HttpGet("GetProductsByName")]
        public IActionResult GetProductsByName([FromQuery] string productName)
        {
            var products = searchManager.GetProductsByName(productName);
            if (products == null || !products.Any())
                return NotFound($"No products found matching '{productName}'");
            return Ok(products);
        }
        [Authorize]
        [HttpGet("GetProductsByPriceRange")]
        public IActionResult GetProductsByPriceRange([FromQuery] int minPrice, [FromQuery] int maxPrice)
        {
            var products = searchManager.GetProductsRang(minPrice, maxPrice);
            if (products == null || !products.Any())
                return NotFound($"No products found in price range {minPrice} - {maxPrice}");
            return Ok(products);
        }
        [Authorize]
        [HttpGet("GetProductSuggestions")]
        public IActionResult GetProductSuggestions([FromQuery] string name)
        {
            var products = searchManager.Suggestions(name);
            if (products == null || !products.Any())
                return NotFound($"No product suggestions found matching '{name}'");
            return Ok(products);
        }
        [Authorize]
        [HttpGet("GetNewestProduct")]
        public IActionResult GetNewestProduct()
        {
            var products = searchManager.GetProductsByCategortName("Newest");
            if (products == null || !products.Any())
                return NotFound("No newest products found");
            return Ok(products);
        }
        [Authorize]
        [HttpGet("AutoComplete")]
        public IActionResult AutoComplete([FromQuery] string name)
        {
            var products = searchManager.Suggestions(name);
            if (products == null || !products.Any())
                return NotFound($"No product suggestions found matching '{name}'");
            var productNames = products.Select(p => p.Name).ToList();
            return Ok(productNames);
        }
        [Authorize]
        [HttpGet("GetProductsPriceAsc")]
        public IActionResult GetProductsPriceAsc()
        {
            var products = searchManager.GetProductsPriceAsc();
            if (products == null || !products.Any())
                return NotFound("No products found");
            return Ok(products);
        }
        [Authorize]
        [HttpGet("GetProductsPriceDesc")]
        public IActionResult GetProductsPriceDesc()
        {
            var products = searchManager.GetProductsPriceDesc();
            if (products == null || !products.Any())
                return NotFound("No products found");
            return Ok(products);
        }
    }
}
