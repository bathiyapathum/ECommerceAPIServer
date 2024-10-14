/******************************************************************************************
 * INotificationService.cs
 * 
 * This interface defines the contract for notification-related services within the e-commerce application.
 * It provides methods for sending notifications and retrieving notifications for all users or specific users.
 * 
 * Methods:
 * - Send: Sends a notification based on the provided NotificationDTO.
 * - GetAllNotificationsAsync: Asynchronously retrieves a list of all notifications.
 * - GetUserNotifications: Asynchronously retrieves notifications specific to a user based on their user ID.
 * 
 * Author: Herath R P N M
 * Registration Number: IT21177828
 * 
 * Date : 2021-08-10
 ******************************************************************************************/


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
        // Send a notification
        Task<string> Send(NotificationDTO notificationDTO);
        // Get all notifications
        Task<List<Notification>> GetAllNotificationsAsync(string userRole);
        // Mark a notification as read
        Task<string> MarkAsRead(string notificationId, string readBy);
        // Get user-specific notifications
        Task<List<Notification>> GetUserNotifications(string userId);
    }
}
