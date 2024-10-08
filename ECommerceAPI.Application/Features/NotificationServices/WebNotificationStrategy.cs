using ECommerceAPI.Application.DTOs.NotificationDTO;
using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.Interfaces.NotificationInterfaces;
using ECommerceAPI.Core.Entities.NotificationEntity;
using ECommerceAPI.Core.Enums;
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

        // Send notification
        public async Task<string> Send(NotificationDTO notificationDTO)
        {
            try
            {
                var notification = new Notification
                {
                    NotifyId = Guid.NewGuid().ToString(),
                    IsRead = false,
                    Message = notificationDTO.Message,
                    Reason = notificationDTO.Reason,
                    SentDate = DateTime.UtcNow,
                    UserId = notificationDTO.UserId,
                    RolesToNotify = GetRolesForScenario(notificationDTO.Scenario),
                    Scenario = notificationDTO.Scenario
                };

                var result = await _notificationRepository.CreateAsync(notification);
                return result;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        // Get roles for a given scenario
        private List<UserRole> GetRolesForScenario(NotificationScenario scenario)
        {
            switch (scenario)
            {
                case NotificationScenario.StockLow:
                    return new List<UserRole> { UserRole.Vendor };

                case NotificationScenario.OrderCancel:
                    return new List<UserRole> { UserRole.Admin, UserRole.CSR };

                case NotificationScenario.OrderShipped:
                    return new List<UserRole> { UserRole.Customer };

                default:
                    return new List<UserRole>();
            }
        }
    }
}
