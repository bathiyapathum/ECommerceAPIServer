using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Application.Interfaces.NotificationInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NotificationController: ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _notificationService.GetAllNotificationsAsync();
            return Ok(orders.ToString());
        }

    }
}
