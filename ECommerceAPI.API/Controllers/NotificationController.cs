using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Application.Interfaces.NotificationInterfaces;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("my/notifications")]
        public async Task<IActionResult> GetUserNotification([FromQuery] string userId)
        {
            var notifications = await _notificationService.GetUserNotifications(userId);
            return Ok(notifications);
        }

    }
}
