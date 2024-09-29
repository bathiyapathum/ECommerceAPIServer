using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Core.Entities;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderDTO orderDTO);
        Task<Order> GetOrderAsync(string orderId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);
        Task UpdateOrderStatusAsync(string orderId, string status);
        Task UpdateOrderDetailsAsync(OrderDTO orderDTO);
        Task DeleteOrderAsync(string orderId);
        Task CancelOrderAsync(string orderId);
    }
}
