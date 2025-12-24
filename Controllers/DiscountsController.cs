using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.Manager;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly DiscountManager discountsManager;
        public DiscountsController(DiscountManager discountsController)
        {
            this.discountsManager = discountsController;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddDiscount")]
        public IActionResult AddDiscount([FromBody] DiscountDto discountDto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var discount = discountsManager.AddDiscountCode(discountDto, userId);
            if (discount == null)
                return BadRequest("Discount Creation Failed!");
            return Ok(discount);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateDiscount")]
        public IActionResult UpdateDiscount([FromBody] DiscountDto discountDto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            discountsManager.UpdateDiscount(discountDto, userId);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteDiscount/{discountId}")]
        public IActionResult DeleteDiscount([FromRoute] int discountId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            discountsManager.DeleteDiscount(discountId, userId);
            return Ok();
        }

    }
}
