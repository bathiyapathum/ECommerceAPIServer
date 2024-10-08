/******************************************************************************************
 * NotificationService.cs
 * 
 * This class implements the INotificationService interface, providing methods for managing notifications within the e-commerce application.
 * It interacts with the NotificationRepository to fetch and send notifications, allowing users to receive updates regarding their orders 
 * and other relevant information.
 * 
 * Methods:
 * - GetAllNotificationsAsync: Retrieves a list of all notifications.
 * - GetUserNotifications: Fetches notifications specific to a user based on their user ID.
 * - Send: Sends a notification to users based on the provided notification data transfer object (DTO).
 * 
 * Author: Herath R P N M
 * Registration Number: IT21177828
 * 
 * This file is developed independently and focuses solely on notification handling.
 * 
 * Date - 2021-08-10
******************************************************************************************/

using ECommerceAPI.Application.DTOs.NotificationDTO;
using ECommerceAPI.Application.Features.NotificationServices;
using ECommerceAPI.Application.Interfaces.NotificationInterfaces;
using ECommerceAPI.Core.Entities.NotificationEntity;
using ECommerceAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationRepository _notificationRepository;
        public NotificationService(NotificationRepository notificationRepository) 
        {
            _notificationRepository = notificationRepository;
        }

        // Get all available notifications
        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            try
            {
                var notifications = await _notificationRepository.GetAllAsync();
                return notifications;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Get notifications specific to a given user
        public async Task<List<Notification>> GetUserNotifications(string userId)
        {
            try
            {
                var notifications = await _notificationRepository.GetAllByUserAsync(userId);
                return notifications;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Send a notification to users
        public async Task<string> Send(NotificationDTO notificationDTO)
        {
            try
            {
                WebNotificationStrategy webNotificationStrategy = new(_notificationRepository);
                var result = await webNotificationStrategy.Send(notificationDTO);
                return result;

            }catch (Exception ex) 
            {
                throw new Exception(ex.Message);

            }
           

        }
    }
}
