using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Notification;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_ASP.NET.Manager
{
    public class NotificationManager
    {
        public readonly AppDbContext dbContext;
        public NotificationManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<NotificationDto> GetAllNotification(int userId,int page,int pageSize)
        {
            var notifications = dbContext.notifications.Where(u => u.UserId == userId)
                .Skip((page-1)*pageSize).Take(pageSize)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Type = n.Type.ToString(),
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                }).ToList();
            return notifications;
        }
        public List<NotificationDto>? GetUnreadNotifications(int userId)
        {
            var notifications = dbContext.notifications.Where(n=>n.UserId==userId).Where(n => !n.IsRead)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Type = n.Type.ToString(),
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                }).ToList();
            return notifications;
        }
    }
}
