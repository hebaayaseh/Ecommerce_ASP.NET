using Ecommerce_ASP.NET.DTOs.Category;
using Ecommerce_ASP.NET.DTOs.Product;
using Ecommerce_ASP.NET.Manager;
using Ecommerce_ASP.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [HttpPost("AddExistingProduct")]
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
        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct([FromBody]AddProduct addProduct )
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
             List<Products> products= productManager.UpdateProduct(addProduct,userId);
            if (products == null) return NotFound();
            return Ok(products);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddNewProduct")]
        public IActionResult AddNewProduct([FromForm] AddProduct productdto)
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
        public IActionResult DeleteProduct([FromRoute]  int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
             productManager.DeleteProduct(productId, userId);
           
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProductStocks")]
        public IActionResult UpdateProductStocks([FromRoute] int productId,int stockQuantity)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
             productManager.UpdateProductStock( productId, stockQuantity, userId);
            return Ok();
        }
        [HttpPost("products/{productId}/image")]
        [Consumes("multipart/form-data")]
        public IActionResult UploadProductImage(
        [FromRoute] int productId,
         IFormFile imageFile)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");

            productManager.UploadImages(userId, productId, imageFile);
            return Ok();
        }

        [Authorize]
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var products = productManager.GetAllProduct(userId);
            if (products == null) return NotFound("No Products Found!");
            return Ok(products);
        }
        [Authorize]
        [HttpGet("GetProductById")]
        public IActionResult GetProductById([FromRoute] int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var product = productManager.GetProductById(productId, userId);
            if (product == null) return NotFound("Product Not Found!");
            return Ok(product);
        }
        
        [Authorize]
        [HttpGet("GetAllProductsByCategor")]
        public IActionResult GetAllProductsByCategory([FromRoute] int categoryDto )
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var products = productManager.GetProductsByCategory(categoryDto,userId);
            if(products==null) return NotFound("No Products Found!");
            return Ok(products);
        }
       
        [Authorize]
        [HttpGet("SearchProductByName")]
        public IActionResult SearchProductByName([FromQuery]string productName)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var products = productManager.SearchProductByName(productName,userId);
            if (products == null) return NotFound("No Products Found!");
            return Ok(products);
        }
        [Authorize]
        [HttpGet("SearchProductById")]
        public IActionResult SearchProductById([FromRoute] int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var product = productManager.GetProductById( productId, userId);
            if (product == null) return NotFound("Product Not Found!");
            return Ok(product);
        }

    }
}
