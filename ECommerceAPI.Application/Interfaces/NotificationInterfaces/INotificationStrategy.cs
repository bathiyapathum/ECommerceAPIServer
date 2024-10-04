using ECommerceAPI.Application.DTOs.NotificationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces.NotificationInterfaces
{
    public interface INotificationStrategy
    {
        Task<string> Send(NotificationDTO notification);
    }
}
