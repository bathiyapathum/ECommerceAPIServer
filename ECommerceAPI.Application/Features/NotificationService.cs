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

        public async Task<string> SendNotification(NotificationDTO notificationDTO)
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
