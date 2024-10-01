using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Core.Entities.OrderEntity;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderDTO orderDTO);
        Task<Order> GetOrderAsync(string orderId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);
        Task UpdateOrderStatusAsync(string orderId, string status);
        Task UpdateOrderDetailsAsync(string orderId, OrderDTO orderDTO);
        Task DeleteOrderAsync(string orderId);
        Task CancelOrderAsync(string orderId);
    }
}
