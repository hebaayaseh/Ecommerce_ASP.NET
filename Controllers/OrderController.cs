using Ecommerce_ASP.NET.DTOs.AddOrderItem;
using Ecommerce_ASP.NET.DTOs.Address;
using Ecommerce_ASP.NET.DTOs.Cart;
using Ecommerce_ASP.NET.DTOs.CheckoutDto;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.Payment;
using Ecommerce_ASP.NET.Manager;
using Ecommerce_ASP.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderManager orderManager;
        public OrderController(OrderManager orderManager)
        {
            this.orderManager = orderManager;
        }
        [Authorize]
        [HttpPost("Checkout")]

        public IActionResult Checkout([FromBody] CheckoutRequestDto checkout)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            orderManager.Checkout(
                checkout.cartDto,
                userId,
                checkout.Address,
                checkout.Payment,
                checkout.Discount
            );

            return Ok("Order placed successfully");
        }
        [Authorize]
        [HttpGet("GetMyOrders")]
        public IActionResult GetMyOrders(int page = 1, int pageSize = 10)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            List<AddOrderItems> order  = orderManager.GetMyOrder(userId,page,pageSize);
            if (!order.Any())
                return NotFound("No orders found");
            return Ok(order);

        }
        [Authorize]
        [HttpGet("GetOrderDetails/{orderId:int}")]

        public IActionResult GetOrderDetails([FromRoute]int orderId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var details = orderManager.GetOrderDetails(userId,orderId);
            if (details == null) return NotFound();
            return Ok(details);
        }
        [Authorize]
        [HttpPost("CancelledOrder")]
        public IActionResult CancelledOrder([FromRoute]int orderId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            orderManager.CancelledOrder(userId, orderId);
            return Ok("Cancelled Order ");
        }
    }
}
