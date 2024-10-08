/******************************************************************************************
 * File Name: NotificationController.cs
 * Description: This controller handles notification-related API requests, providing 
 *              services for retrieving and sending notifications in the e-commerce 
 *              application.
 *
 * Author: Herath R. P. N. M
 * Registration No: IT21177828
 * Date: 2024/08/10
 *
 * API Endpoints:
 *  - GetAll: Retrieves all available notifications.
 *  - GetUserNotification: Fetches notifications specific to a given user, based on the 
 *                         provided user ID.
 *  - SendNotification: Sends a notification to users based on the provided 
 *                      NotificationDTO object.
 *
 * Error Handling:
 *  - Returns 400 if invalid notification data is provided.
 *  - Returns 500 for internal server errors.
 *
 ******************************************************************************************/

using ECommerceAPI.Application.DTOs.NotificationDTO;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Application.Interfaces.NotificationInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class NotificationController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService _notificationService = notificationService;

        //Get all available notifications
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        //Get notifications specific to a given user
        [HttpGet("my/notifications")]
        public async Task<IActionResult> GetUserNotification([FromQuery] string userId)
        {
            var notifications = await _notificationService.GetUserNotifications(userId);
            return Ok(notifications);
        }

        //Send a notification to users
        [HttpPost("Send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDTO notificationDTO)
        {
            if (notificationDTO == null)
                return BadRequest("Invalid notification data");

            try
            {
                var result = await _notificationService.Send(notificationDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
