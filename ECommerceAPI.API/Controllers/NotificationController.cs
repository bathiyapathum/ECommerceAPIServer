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

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        //get all notifications for a specific user
        [HttpGet("my/notifications")]
        public async Task<IActionResult> GetUserNotification([FromQuery] string userId)
        {
            var notifications = await _notificationService.GetUserNotifications(userId);
            return Ok(notifications);
        }


    
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
