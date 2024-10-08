using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.Features;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDTO orderDTO)
        {
            if(orderDTO.CustomerId == null)
            {
                return BadRequest("Customer Id is required");
            }
            else if (orderDTO.VendorId == null)
            {
                return BadRequest("Vendor Id is required");
            }
            else if (orderDTO.ProductId == null) {
                return BadRequest("Product Id is required");
            }
            else if (orderDTO.Quantity == 0)
            {
                return BadRequest("Quantity is required");
            }
            else if (orderDTO.Price == null)
            {
                return BadRequest("Price is required");
            }

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

        [HttpDelete("cart/item")]
        public async Task<IActionResult> RemoveItemFromCart([FromQuery] string orderId, string itemId)
        {
            if(orderId == null)
            {
                return BadRequest("Order Id is required");
            }
            if(itemId == null)
            {
                return BadRequest("Item Id is required");
            }

            var response = await _orderService.RemoveItemFromCart(orderId, itemId);
            if (response == "Item removed from cart successfully")
            {
                return Ok("Item removed from cart successfully");
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId);
            return Ok(order);
        }  
        
        [HttpGet("cart")]
        public async Task<IActionResult> GetCustomerCartOrder([FromQuery] string customerId)
        {
            var order = await _orderService.GetCustomerCartOrderAsync(customerId);
            if(order == null)
            {
                return NotFound("Order not found");
            }
            return Ok(order);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetCustomerPlacedOrder([FromQuery] string customerId)
        {
            if (customerId == null)
            {
                return BadRequest("Customer Id is required");
            }
            var order = await _orderService.GetCustomerPlacedOrderAsync(customerId);
            if (order == null)
            {
                return NotFound("Orders not found");
            }
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

        //updare order status
        [HttpPatch("delivered")]
        public async Task<IActionResult> UpdateOrderStatus([FromQuery] string orderId)
        {
            try
            {

                var result = await _orderService.UpdateOrderStatusAsync(orderId, "DELIVERED");
                if(result == "Order status updated successfully")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //cancel orders
        [HttpPatch("cancel")]
        public async Task<IActionResult> CancelOrder([FromBody] OrderDTO orderDTO, [FromQuery] string orderId)
        {
            try
            {
                string note = orderDTO.Note;
                string canceledBy = orderDTO.CanceledBy;

                var response = await _orderService.CancelOrderAsync(orderId, note, canceledBy);
                if (response == "Order Canceled Successfully")
                {
                    return Ok("Order Canceled Successfully");
                }
                else
                {
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Error while CancelOrder: {ex.Message}");
            }
        }

        //mark order item as delivered
        [HttpPatch("item/delivered")]
        public async Task<IActionResult> ItemDelivered([FromQuery] string itemId)
        {
            try
            {
                var response = await _orderService.ItemDeliverAsync(itemId);
                if (response == "Order Item Delivered Successfully")
                {
                    return Ok("Order Item Delivered Successfully");
                }
                else
                {
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Error while CancelOrder: {ex.Message}");
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

        [HttpPost("placing")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDTO orderDTO, [FromQuery] string orderId)
        {
            try
            {
                string address = orderDTO.Address;
                string tel = orderDTO.Tel;

                var response = await _orderService.PlaceOrderAsync(orderId, address, tel);
                if (response == "Order Placed Successfully")
                {
                    return Ok("Order Placed Successfully");
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error while PlaceOrder: {ex.Message}");
            }
        } 
        
        [HttpPost("request/cancel")]
        public async Task<IActionResult> RequestCancellation([FromBody] CancelRequestDTO cancelRequest)
        {
            try
            {
                if (cancelRequest.OrderId == null)
                {
                    return BadRequest("Order Id is required");
                }
                if(cancelRequest.RequestNote == null)
                {
                    return BadRequest("Note is required");
                }
                if(cancelRequest.CustomerId == null)
                {
                    return BadRequest("Customer Id is required");
                }

                var response = await _orderService.MakeCancelOrderRequestAsync(cancelRequest);
                if (response == "Cancel request sent successfully")
                {
                    return Ok("Cancel request sent successfully");
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error while RequestCancellation: {ex.Message}");
            }
        }


        [HttpGet("cancel/request/all")]
        public async Task<IActionResult> GetAllRequestCancellation()
        {
            var order = await _orderService.GetAllCancellationRequests();
            return Ok(order);
        }

        [HttpPatch("cancel/response")]
        public async Task<IActionResult> UpdateCancelOrderResponse([FromBody] CancelRequestDTO cancelRequestDTO)
        {
            try
            {
                var response = await _orderService.RespondToCancelRequest(cancelRequestDTO);
                if (response == "Cancel request response sent successfully")
                {
                    return Ok("Cancel request response sent successfully");
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error while CancelOrder: {ex.Message}");
            }
        }



    }
}
