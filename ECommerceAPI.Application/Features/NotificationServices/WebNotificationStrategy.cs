using ECommerceAPI.Application.DTOs.NotificationDTO;
using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.Interfaces.NotificationInterfaces;
using ECommerceAPI.Core.Entities.NotificationEntity;
using ECommerceAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.NotificationServices
{
    public class WebNotificationStrategy : INotificationStrategy
    {
        private readonly NotificationRepository _notificationRepository;
        public WebNotificationStrategy(NotificationRepository notificationRepository) 
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<string> Send(NotificationDTO notificationDTO)
        {
            try
            {
                var notification = new Notification
                {
                    NotifyId = Guid.NewGuid().ToString(),
                    IsRead = notificationDTO.IsRead,
                    Message = notificationDTO.Message,
                    Reason = notificationDTO.Reason,
                    SentDate = DateTime.UtcNow,
                    UserId = notificationDTO.UserId
                };

                var result = await _notificationRepository.CreateAsync(notification);
                return result;

            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
