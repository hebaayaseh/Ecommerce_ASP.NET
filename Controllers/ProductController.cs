using Ecommerce_ASP.NET.DTOs.Category;
using Ecommerce_ASP.NET.DTOs.Product;
using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductManager productManager;
        public ProductController(ProductManager productManager)
        {
            this.productManager = productManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateProduct")]
        public IActionResult AddExsistProduct([FromForm] AddProduct productdto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var product= productManager.AddExsistProduct(productdto,userId);
            if(product==null) return NotFound("Product Cant Found!");
            return Created();

        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddNewProduct")]
        public IActionResult AddNewProduct([FromBody] AddProduct productdto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
             productManager.AddNewProduct(productdto, userId);
            return Created();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct(int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
             productManager.DeleteProduct(productId, userId);
            return NoContent();
        }
        [Authorize]
        [HttpGet("GetAllProductsByCategor")]
        public IActionResult GetAllProductsByCategory(int categoryDto )
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var products = productManager.GetAllProducts(categoryDto,userId);
            if(products==null) return NotFound("No Products Found!");
            return Ok(products);
        }
        [Authorize]
        [HttpGet("SearchProduct")]
        public IActionResult SearchToProduct(string productName)
        {
            var products = productManager.SearchProduct(productName);
            if (products == null) return NotFound("No Products Found!");
            return Ok(products);
        }
    }
}
