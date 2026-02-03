using Ecommerce_ASP.NET.DTOs.Category;
using Ecommerce_ASP.NET.Manager;
using Ecommerce_ASP.NET.Models;
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
        private readonly CategoryManager categoryManager;
        public CategoryController(CategoryManager categoryManager)
        {
            this.categoryManager = categoryManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddCategory")]
        public IActionResult AddCategory([FromBody] AddCategory addCategory)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            categoryManager.AddCategory(addCategory, userId);
            return Created();
        }
        [Authorize]
        [HttpGet("SearchCategory")]
        public IActionResult SearchCategory([FromQuery]string name)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No user id in token");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var categories = categoryManager.SearchCategory(name, userId);
            return Ok(categories);
        }
        [Authorize]
        [HttpGet("GetAllCategory")]
        public IActionResult GetAllCategory()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            List<Categories> categories = categoryManager.GetAllCategory(userId);
            if (categories == null || categories.Count == 0)
                return NotFound("No categories found.");
            return Ok(categories);
            
        }
        [Authorize]
        [HttpPut("UpdateCategory")]
        public IActionResult UpdateCategory([FromBody] CategoryDto categoryDto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            categoryManager.UpdateCategory(categoryDto, userId);
            return Ok();
        }
        [Authorize]
        [HttpDelete("DeleteCategory/{categoryId:int}")]
        public IActionResult DeleteCategory([FromRoute] int categoryId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            categoryManager.DeleteCategory(categoryId, userId);
            return Ok();
        }
        [Authorize]
        [HttpGet("GetCategoryById/{categoryId:int}")]
        public IActionResult GetCategoryById([FromRoute] int categoryId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var category = categoryManager.GetCategoryById(userId,categoryId);
            return Ok(category);
        }

        [Authorize]
        [HttpGet("GetCategorywithProduct/{categoryId:int}")]
        public IActionResult GetCategorywithProduct([FromRoute] int categoryId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var products = categoryManager.GetCategorywithProduct(userId,categoryId);
            return Ok(products);
        }
        
    }
}
