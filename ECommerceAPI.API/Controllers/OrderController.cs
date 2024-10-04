using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.Features;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
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


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId);
            return Ok(order);
        }

        //Order update method implementation

        [HttpPatch]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDTO orderDTO, [FromQuery] string orderId)
        {
            try
            {
                await _orderService.UpdateOrderDetailsAsync(orderId,orderDTO);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("cancel")]
        public async Task<IActionResult> CancelOrder([FromBody] OrderDTO orderDTO, [FromQuery] string orderId)
        {
            try
            {
                string note = orderDTO.Note;
                string canceledBy = orderDTO.CanceledBy;

                await _orderService.CancelOrderAsync(orderId, note, canceledBy);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder([FromQuery] string orderId)
        {
            try
            {
                await _orderService.DeleteOrderAsync(orderId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
