using Ecommerce_ASP.NET.DTOs.Category;
using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly  CategoryManager categoryManager;
        public CategoryController(CategoryManager categoryManager)
        {
            this.categoryManager = categoryManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddCategory")]
        public IActionResult AddCategory([FromBody]AddCategory addCategory )
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            categoryManager.AddCategory(addCategory,userId);
            return Created();
        }
        [Authorize]
        [HttpGet("SeachCategory")]
        public IActionResult SeachCategory(string name)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var categories = categoryManager.SearchCategory(name,userId);
            return Ok(categories);
        }
    }
}
