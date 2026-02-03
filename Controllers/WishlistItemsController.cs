using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistItemsController : ControllerBase
    {
        private readonly WishlistItemsManager wishlistItemsManager;
        public WishlistItemsController(WishlistItemsManager wishlistItemsManager)
        {
            this.wishlistItemsManager = wishlistItemsManager;
        }
        [Authorize]
        [HttpGet("GetWishlistItems")]
        public IActionResult GetWishlistItems()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var wishlistItems = wishlistItemsManager.GetWishlistItems(userId);
            if (wishlistItems == null || wishlistItems.Count == 0)
                return NotFound("Wishlist is empty.");
            return Ok(wishlistItems);
        }
        [Authorize]
        [HttpPost("AddToWishlist/{productId:int}")]
        public IActionResult AddProductToWishlist([FromRoute] int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
             wishlistItemsManager.AddProduct(userId, productId);
            return Ok("Product added to wishlist successfully.");
        }
        [Authorize]
        [HttpDelete("RemoveFromWishlist/{productId:int}")]
        public IActionResult RemoveProductFromWishlist([FromRoute] int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            wishlistItemsManager.RemoveProduct(userId, productId);
            return Ok("Product removed from wishlist successfully.");
        }
        [Authorize]
        [HttpPost("MoveToCart")]
        public IActionResult MoveToCart([FromBody] int productId, int quantity)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var product = wishlistItemsManager.MoveToCart(userId, productId, quantity);
            if (product == null)
                return BadRequest("Could not move product to cart.");
            return Ok("Product moved to cart successfully.");
        }

    }
}
