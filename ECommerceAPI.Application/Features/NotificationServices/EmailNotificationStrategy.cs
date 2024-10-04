using ECommerceAPI.Application.DTOs.NotificationDTO;
using ECommerceAPI.Application.Interfaces.NotificationInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.NotificationServices
{
    public class EmailNotificationStrategy : INotificationStrategy
    {
        public Task Send(NotificationDTO notification)
        {
            throw new NotImplementedException();
        }
    }
}
