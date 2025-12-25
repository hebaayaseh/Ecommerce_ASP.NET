using Ecommerce_ASP.NET.DTOs.Cart;
using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartIltemController : ControllerBase
    {
        private readonly CartManager cartManager;
        public CartIltemController(CartManager cartManager)
        {
            this.cartManager = cartManager;
        }
        [Authorize]
        [HttpPost("AddToCart")]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var cart = cartManager.AddToCart(userId, productId, quantity);
            if(cart == null)
                return BadRequest("Could not add product to cart.");
            return Ok("Product added to cart successfully.");
            
        }
        [Authorize]
        [HttpPost("DeleteCart/{catrId:int}")]
        public IActionResult DeleteCart([FromRoute]int catrId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            cartManager.deleteCart(userId, catrId);
            return Ok();
        }
        [Authorize]
        [HttpPost("DeleteProductFromCart/{productId:int}")]
        public IActionResult DeleteProductFromCart([FromRoute] int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            cartManager.deletProductFromCart(userId, productId);
            return Ok();
        }
        [Authorize]
        [HttpPost("DeleteQuantityForProduct")]
        public IActionResult DeleteQuantityForProduct([FromBody]CartDto cartDto,int quantity)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var cart=cartManager.DeleteQuantityForProduct(userId,cartDto,quantity);
            if(cart==null) return NotFound();
            return Ok(cart);
        }
    }
}
