using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Notification;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ecommerce_ASP.NET.Manager
{
    public class NotificationManager
    {
        public readonly AppDbContext dbContext;
        public NotificationManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<NotificationDto> GetAllNotification(int userId, int page, int pageSize)
        {
            var notifications = dbContext.notifications.Where(u => u.UserId == userId)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .OrderByDescending(n => n.CreatedAt)
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
            var notifications = dbContext.notifications.Where(n => n.UserId == userId).Where(n => !n.IsRead)
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
        public int GetUnreadNotificationsCount(int userId)
        {
            var count = dbContext.notifications.Where(n => n.UserId == userId).Where(n => !n.IsRead).Count();
            return count;
        }
        public void MarkAsRead(int notificationId,int userId)
        {
            var notification = dbContext.notifications.Where(n => n.Id == notificationId && n.UserId == userId).FirstOrDefault();
            if(notification== null)
            {
                 throw new KeyNotFoundException("Not Found!");
            }
            notification.IsRead = true;
            dbContext.SaveChanges();
        }
        public void MarkAllAsRead(int userId)
        {
            var notifications = dbContext.notifications.Where(n=>n.UserId== userId).Where(n => !n.IsRead).ToList();
            foreach(var notification in notifications)
            {
                notification.IsRead = true;
            }
            dbContext.SaveChanges();
        }
        public void MarkAsUnRead(int notificationId, int userId)
        {
            var notification = dbContext.notifications.Where(n => n.Id == notificationId && n.UserId == userId).FirstOrDefault();
            if (notification == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            notification.IsRead = false;
            dbContext.SaveChanges();
        }
        public void DeleteNotification(int userId, int notificationId)
        {
            var notification = dbContext.notifications.Where(u => u.Id == notificationId && u.UserId == userId).FirstOrDefault();
            if (notification == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            dbContext.Remove(notification);
            dbContext.SaveChanges();
        }
        public void DeleteAllNotification(int userId)
        {
            var notifications = dbContext.notifications.Where(n => n.UserId == userId).ToList();
            dbContext.RemoveRange(notifications);
            dbContext.SaveChanges();
        }
        public NotificationDto? SendNotificationToUser(int userId,string message,NotificationType type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
            };
            dbContext.notifications.Add(notification);
            dbContext.SaveChanges();
            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Message = notification.Message,
                Type = notification.Type.ToString(),
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt,
            };
        
        }
        public void SendNotificationToAllUsers(string message, NotificationType type)
        {
            var users = dbContext.Users.Select(u => u.id).ToList();

            var notifications = users.Select(userId => new Notification
            {
                UserId = userId,
                Message = message,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            dbContext.notifications.AddRange(notifications);
            dbContext.SaveChanges();
        }

    }
}
