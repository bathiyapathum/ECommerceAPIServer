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
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderAsync(string orderId);
        Task<Order> GetCustomerOrderAsync(string customerId);
        Task<OrderResponseDTO> GetCustomerCartOrderAsync(string customerId);
        Task<List<OrderResponseDTO>> GetCustomerPlacedOrderAsync(string customerId);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);
        Task <List<CancelRequest>> GetAllCancellationRequests();
        Task<string> PlaceOrderAsync(string orderId, string address, string tel);
        Task<string> MakeCancelOrderRequestAsync(CancelRequestDTO cancelRequestDTO);
        Task<string> ItemDeliverAsync(string itemId);
        Task<string> RespondToCancelRequest(CancelRequestDTO cancelRequestDTO);
        Task UpdateOrderDetailsAsync(string orderId, OrderDTO orderDTO);
        Task<string> UpdateOrderStatusAsync(string orderId, string status);
        //Task<string> GetTotalRevenue();
        //Task<Dictionary<string, int>> GetOrderStats();
        Task<string> CancelOrderAsync(string orderId, string note, string canceledBy);
        Task<string> RemoveItemFromCart(string orderId, string itemId);
        Task DeleteOrderAsync(string orderId);
    }
}
