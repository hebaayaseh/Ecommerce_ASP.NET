using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.Order;
using Ecommerce_ASP.NET.DTOs.UserDto;
using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserManager userManager;
        
        public UserController(UserManager userManager)
        {
            this.userManager = userManager;
           
        }
        
        [Authorize]
        [HttpGet("GetProfile")]
        public IActionResult GetProfile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");

            var user = userManager.GetProfile(userId);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }
        [Authorize]
        [HttpPut("UpdateProfile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfile userdto)
        {

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");

            var user = userManager.UpdateProfile(userId , userdto);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUser()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var users = userManager.GetAllUsers(userId);
            if(users == null)
                return NotFound("No Users Found!");
            return Ok(users);
        }
        //[Authorize]
        //[HttpPost("CreateOrder")]
        /*public IActionResult CreateOrder([FromBody] AddOrder order)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var orders = userManager.createOrder(order , userId);
            if(orders==null)
                return BadRequest("Order Creation Failed!");
            return Ok(orders);
        }*/

    }
}
