using Ecommerce_ASP.NET.Manager;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        public readonly NotificationManager notificationManager;
        public NotificationsController(NotificationManager notificationManager)
        {
            this.notificationManager = notificationManager;
        }
        [Authorize]
        [HttpGet("GetAllNotifications")]
        public IActionResult GetAllNotifications(int page = 5, int pageSize = 10)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var notifications = notificationManager.GetAllNotification(userId, page, pageSize);
            if (notifications == null || !notifications.Any())
                return NotFound("No notifications found");
            return Ok(notifications);
        }
        [Authorize]
        [HttpGet("GetUnreadNotifications")]
        public IActionResult GetUnreadNotifications()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var notifications = notificationManager.GetUnreadNotifications(userId);
            if (notifications == null || !notifications.Any())
                return NotFound("No unread notifications found");
            return Ok(notifications);
        }
        [Authorize]
        [HttpGet("GetUnreadNotificationsCount")]
        public IActionResult GetUnreadNotificationsCount()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            var count = notificationManager.GetUnreadNotificationsCount(userId);
            return Ok(new { UnreadCount = count });
        }
        [Authorize]
        [HttpPost("MarkAsRead/{notificationId:int}")]
        public IActionResult MarkAsRead([FromRoute] int notificationId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            try
            {
                notificationManager.MarkAsRead(notificationId, userId);
                return Ok("Notification marked as read");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Notification with ID {notificationId} not found for this user");
            }
        }
        [Authorize]
        [HttpPost("MarkAllAsRead")]
        public IActionResult MarkAllAsRead()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            notificationManager.MarkAllAsRead(userId);
            return Ok("All notifications marked as read");
        }
        [Authorize]
        [HttpDelete("DeleteNotification/{notificationId:int}")]
        public IActionResult DeleteNotification([FromRoute] int notificationId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            try
            {
                notificationManager.DeleteNotification(notificationId, userId);
                return Ok("Notification deleted");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Notification with ID {notificationId} not found for this user");
            }
        }
        [Authorize]
        [HttpPost("MarkAsUnread/{notificationId:int}")]
        public IActionResult MarkAsUnread([FromRoute] int notificationId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            try
            {
                notificationManager.MarkAsUnRead(notificationId, userId);
                return Ok("Notification marked as unread");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Notification with ID {notificationId} not found for this user");
            }
        }
        [Authorize]
        [HttpDelete("DeleteAllNotifications")]
        public IActionResult DeleteAllNotifications()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("No user id in token");
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id format");
            notificationManager.DeleteAllNotification(userId);
            return Ok("All notifications deleted");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("SendNotificationToUser")]
        public IActionResult SendNotificationToUser(int userId, string message, NotificationType type)
        {
            try
            {
                notificationManager.SendNotificationToUser(userId, message, type);
                return Ok("Notification sent to user");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with ID {userId} not found");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("SendNotificationToAllUser")]
        public IActionResult SendNotificationToAllUser(string message, NotificationType type)
        {
            notificationManager.SendNotificationToAllUsers(message, type);
            if (string.IsNullOrEmpty(message))
                return BadRequest("Message cannot be empty");
            return Ok("Notification sent to all users");
        }
    }
}
