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
        [HttpPost("AddProduct")]
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
        public IActionResult AddNewProduct([FromForm] AddProduct productdto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var product = productManager.AddNewProduct(productdto, userId);
            if (product == null) return NotFound("Product Cant Add!");
            return Created();
        }
    }
}
