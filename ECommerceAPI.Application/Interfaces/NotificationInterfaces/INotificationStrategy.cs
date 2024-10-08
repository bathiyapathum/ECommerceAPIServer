/******************************************************************************************
 * INotificationStrategy.cs
 * 
 * This interface defines the contract for implementing different notification strategies within the e-commerce application.
 * It provides a method for sending notifications based on the provided NotificationDTO.
 * 
 * Methods:
 * - Send: Asynchronously sends a notification using the specified notification strategy.
 * 
 * Author: Herath R P N M
 * Registration Number: IT21177828
 * Date: 2024-08-10
 * 
 * 
******************************************************************************************/


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
        // Send a notification using the specified strategy
        Task<string> Send(NotificationDTO notification);
    }
}
