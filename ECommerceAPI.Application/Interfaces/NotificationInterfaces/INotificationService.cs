using ECommerceAPI.Application.DTOs.NotificationDTO;
using ECommerceAPI.Core.Entities.NotificationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces.NotificationInterfaces
{
    public interface INotificationService
    {
        Task<string> Send(NotificationDTO notificationDTO);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<List<Notification>> GetUserNotifications(string userId);
    }
}
