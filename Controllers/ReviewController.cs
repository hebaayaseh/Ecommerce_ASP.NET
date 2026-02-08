using Ecommerce_ASP.NET.DTOs.Review;
using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        public readonly ReviewManager reviewManager;
        public ReviewController(ReviewManager reviewManager)
        {
            this.reviewManager = reviewManager;
        }
        [Authorize]
        [HttpPost("AddReviews")]
        public IActionResult AddReview([FromBody] ReviewDto reviewDto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            reviewManager.AddReview(reviewDto);
            if (reviewDto == null)
            {
                return BadRequest("Failed to add review");
            }
            return Ok("Review added successfully");
        }
        [Authorize]
        [HttpGet("GetProductReviews/{productId:int}")]
        public IActionResult GetProductReviews([FromRoute] int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var reviews = reviewManager.GetProductReview(productId);
            if (reviews == null || !reviews.Any())
            {
                return NotFound("No reviews found for this product");
            }
            return Ok(reviews);
        }
        [Authorize]
        [HttpPut("UpdateReview")]
        public IActionResult UpdateReview([FromBody] ReviewDto reviewDto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var updatedReview = reviewManager.UpdateReview(reviewDto);
            if (updatedReview == null)
            {
                return NotFound("Review not found or update failed");
            }
            return Ok(updatedReview);
        }
        [Authorize]
        [HttpDelete("DeleteReview/{review:int}")]
        public IActionResult DeleteReview([FromRoute] int reviewId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            reviewManager.DeleteReview(reviewId);
            return Ok("Review deleted successfully");
        }
        [Authorize]
        [HttpGet("GetMyReviews")]
        public IActionResult GetMyReviews()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var reviews = reviewManager.GetMyReview(userId);
            if (reviews == null || !reviews.Any())
            {
                return NotFound("No reviews found for this user");
            }
            return Ok(reviews);
        }
        [Authorize]
        [HttpGet("GetAverageRating/{productId:int}")]
        public IActionResult GetAverageRating([FromRoute] int productId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var averageRating = reviewManager.GetAverageRating(productId);
            return Ok(averageRating);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllReviews")]
        public IActionResult GetAllReviews()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var reviews = reviewManager.GetAllReviews();
            if (reviews == null || !reviews.Any())
            {
                return NotFound("No reviews found");
            }
            return Ok(reviews);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("AdminDeleteReview/{reviewId:int}")]
        public IActionResult AdminDeleteReview([FromRoute] int reviewId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            reviewManager.AdminDeleteReviews(reviewId);
            return Ok("Review deleted successfully by admin");
        }
    }
}
