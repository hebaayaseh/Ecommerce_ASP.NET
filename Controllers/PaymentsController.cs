using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        public PaymentManager paymentManager;
        public PaymentsController(PaymentManager paymentManager)
        {
            this.paymentManager = paymentManager;
        }
        [Authorize]
        [HttpGet("GetPaymentMethods")]
        public IActionResult GetPaymentMethods()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var method=paymentManager.GetPaymentMethod();
            if (method == null) 
                return NotFound("No payment methods found");
            return Ok(method);
        }
    }
}
