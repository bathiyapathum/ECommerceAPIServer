/******************************************************************************************
 * WebNotificationStrategy.cs
 * 
 * This class implements the INotificationStrategy interface to handle the sending of web notifications 
 * within the e-commerce application. It creates notifications based on the provided NotificationDTO and 
 * determines which user roles should be notified based on specific scenarios such as low stock, order 
 * cancellation, and order delivery.
 * 
 * Methods:
 * - Send: Asynchronously creates and sends a web notification based on the input NotificationDTO.
 * - GetRolesForScenario: Determines the appropriate user roles to notify based on the given notification scenario.
 * 
 * Author: Herath R P N M
 * Registration Number: IT21177828
 * Date: 2024-10-08
 * 
 * This strategy enhances the communication workflow by targeting notifications to relevant users, 
 * thereby improving operational efficiency within the application.
*******************************************************************************************/


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

        public async Task<string> Send(NotificationDTO notificationDTO)
        {
            try
            {
                var notification = new Notification
                {
                    NotifyId = Guid.NewGuid().ToString(),
                    IsRead = false,
                    Message = notificationDTO.Message,
                    UserId = notificationDTO.UserId,
                    CreatedDate = DateTime.UtcNow,
                    ReadBy = notificationDTO.ReadBy,
                    RolesToNotify = GetRolesForScenario(notificationDTO.Scenario),
                    Scenario = notificationDTO.Scenario,
                    ScenarioId = notificationDTO.ScenarioId
                };

                var result = await _notificationRepository.CreateAsync(notification);
                return result;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        private List<UserRole> GetRolesForScenario(NotificationScenario scenario)
        {
            switch (scenario)
            {
                case NotificationScenario.StockLow:
                    return new List<UserRole> { UserRole.Vendor };
                
                case NotificationScenario.OrderPlaced:
                    return new List<UserRole> { UserRole.Customer };

                case NotificationScenario.OrderCancelRequest:
                    return new List<UserRole> { UserRole.Admin, UserRole.CSR };

                case NotificationScenario.OrderDelivered:
                    return new List<UserRole> { UserRole.Customer };
                
                case NotificationScenario.OrderCancelled:
                    return new List<UserRole> { UserRole.Customer, UserRole.Vendor };

                case NotificationScenario.PaymentFailed:
                    return new List<UserRole> { UserRole.Customer };

                default:
                    return new List<UserRole>();
            }
        }
    }
}
