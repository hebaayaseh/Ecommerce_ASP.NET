using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_ASP.NET.Controllers.UserManagment
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagmentController : ControllerBase
    {
        public readonly UserManagmentManeger userManagment;
        public UserManagmentController(UserManagmentManeger userManagment)
        {
            this.userManagment = userManagment;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUser")]
        public IActionResult GetAllUser(int page = 5, int pageSize = 10)
        {
            var users = userManagment.GetAllUser(page, pageSize);
            if (users == null || !users.Any())
            {
                return NotFound("No users found");
            }
            return Ok(users);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserById/{userId:int}")]
        public IActionResult GetUserById([FromRoute] int userId)
        {
            var user = userManagment.GetUserById(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found");
            }
            return Ok(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletUserById/{userId:int}")]
        public IActionResult DeletUserById([FromRoute] int userId)
        {
            var user = userManagment.DeletUserById(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found");
            }
            return Ok(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("SearchUser")]
        public IActionResult SearchUser([FromQuery] string str)
        {
            var user = userManagment.SearchUser(str);
            if (user == null)
            {
                return NotFound($"No user found matching '{str}'");
            }
            return Ok(user);
        }
    }
}
