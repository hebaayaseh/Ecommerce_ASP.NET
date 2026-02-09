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
        public IActionResult GetAllUser(int page=5,int pageSize=10)
        {
            var users = userManagment.GetAllUser(page,pageSize);
            if (users == null || !users.Any())
            {
                return NotFound("No users found");
            }
            return Ok(users);
        }
         

    }
}
