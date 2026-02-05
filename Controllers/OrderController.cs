using Ecommerce_ASP.NET.DTOs.AddOrderItem;
using Ecommerce_ASP.NET.DTOs.Address;
using Ecommerce_ASP.NET.DTOs.Cart;
using Ecommerce_ASP.NET.DTOs.CheckoutDto;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.Invoice;
using Ecommerce_ASP.NET.DTOs.Payment;
using Ecommerce_ASP.NET.Manager;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

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
            List<OrderItemsDto> order  = orderManager.GetMyOrder(userId,page,pageSize);
            if (!order.Any())
                return NotFound("No orders found");
            return Ok(order);

        }
        [Authorize]
        [HttpGet("GetOrderTracking/{orderId:int}")]
        public IActionResult GetOrderTracking([FromRoute]int orderId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var order = orderManager.GetOrderTracking(userId, orderId);
            if (order == null) return NotFound();
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
        [HttpPost("CancelledOrder/{orderId:int}")]
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
        [Authorize]
        [HttpGet("DownloadInvoice/{orderId}")]
        public IActionResult DownloadInvoice([FromRoute]int orderId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();
            int userId = int.Parse(userIdClaim);
            var invoice = orderManager.DownloadInvoice(userId, orderId);
            if (invoice == null)
                return NotFound("Invoice not found");
            var content = GenerateInvoiceText(invoice);
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            return File(
                bytes,
                "application/pdf",
                $"Invoice_Order_{orderId}.txt"
            );
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            var orders = orderManager.GetAllOrder();
            if (!orders.Any())
                return NotFound("No orders found");
            return Ok(orders);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("GetOrdersByStatus")]
        public IActionResult GetOrdersByStatus([FromBody] OrderStatus orderStatus)
        {
            var order = orderManager.GetOrdersByStatus(orderStatus);
            if (order == null)
                return NotFound("Order not found");
            return Ok(order);
        }
        private string GenerateInvoiceText(InvoiceDto invoice)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Invoice for Order #{invoice.OrderId}");
            sb.AppendLine($"Order Date: {invoice.OrderDate}");
            sb.AppendLine("----------------------------------");
            foreach (var item in invoice.Items)
            {
                sb.AppendLine(
                    $"ProductId: {item.ProductId} | Qty: {item.Quantity} | Price: {item.Price}"
                );
            }
            sb.AppendLine("----------------------------------");
            sb.AppendLine($"Total: {invoice.TotalPrice}");
            return sb.ToString();
        }
    }
}
