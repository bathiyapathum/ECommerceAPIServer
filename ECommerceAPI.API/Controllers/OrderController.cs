using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.Features;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Threading.Tasks;

namespace ECommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDTO orderDTO)
        {
            await _orderService.CreateOrderAsync(orderDTO);
            return CreatedAtAction(nameof(Create) ,new { id = orderDTO }, orderDTO);
        }

        [HttpPost("checkout/{id}")]
        public async Task<IActionResult> Checkout([FromBody] OrderDTO orderDTO, string id)
        {
            //await _orderService.CheckoutOrderAsync(orderDTO,id);
            return CreatedAtAction(nameof(Checkout), new { id = orderDTO }, orderDTO);
        }


    }
}
