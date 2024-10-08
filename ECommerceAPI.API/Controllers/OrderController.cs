/******************************************************************************************
 * OrderController.cs
 * 
 * This file implements the API endpoints for managing orders in the E-Commerce application.
 * The controller provides functionality for creating, updating, deleting, and retrieving 
 * orders, as well as handling order cancellations and item deliveries. 
 * 
 * Authorization: 
 * - Admin and CSR roles have access to certain critical endpoints (e.g., updating order status, cancellations).
 * - Vendor role has access to specific vendor-related functionalities.
 * - Customer role has access to order placement and retrieval anc other functionalities.
 * - Unauthorized access to these endpoints will return a 401 Unauthorized response.
 * 
 * Contributions:
 * - IT21177828 - Herath R P N M:
 *   * Get all orders
 *   * Mark order as delivered
 *   * Cancel order
 *   * Mark order item as delivered
 *   * Request order cancellation
 *   * Get all cancellation requests
 *   * Respond to cancel requests
 * 
 * - IT21167850 - Hansana K. T:
 *   * Create order
 *   * Remove item from cart
 *   * Get order by ID
 *   * Get customer cart order
 *   * Get customer placed orders
 *   * Update order details
 *   * Delete order when it is in the cart
 *   * Place order
 * 
 * Date:2024/08/10
 ******************************************************************************************/

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

        // Create a new order
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

        // Get all orders in the system (Admin and CSR roles only)
        [Authorize(Roles = "Admin,CSR")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // Remove an item from the cart
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

        // Get an order by ID
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId);
            return Ok(order);
        }

        // Get the customer's cart order
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

        // Get the customer's placed orders
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


        // Update order details
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

        // Make order status as delivered (Admin and CSR roles only)
        [Authorize(Roles = "Admin,CSR")]
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

        // Cancel an order (Admin and CSR roles only)
        [Authorize(Roles = "Admin,CSR")]
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

        // Mark an order item as delivered (Admin, CSR, and Vendor roles only)
        [Authorize(Roles = "Admin,CSR,Vendor")]
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

        // Delete an order when it is in the cart
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

        // Place an order with the provided address and telephone number
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

        // Request order cancellation
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

        // Get all cancellation requests (Admin and CSR roles only)
        [Authorize(Roles = "Admin,CSR")]
        [HttpGet("cancel/request/all")]
        public async Task<IActionResult> GetAllRequestCancellation()
        {
            var order = await _orderService.GetAllCancellationRequests();
            return Ok(order);
        }

        // Respond to cancel requests (Admin and CSR roles only)
        [Authorize(Roles = "Admin,CSR")]
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
