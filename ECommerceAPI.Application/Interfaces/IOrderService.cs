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
        //Task<Order> CheckoutOrderAsync(OrderDTO orderDTO, string customerId);
        Task<Order> GetOrderAsync(string orderId);
        Task<Order> GetCustomerOrderAsync(string customerId);
        Task <List<CancelRequest>> GetAllCancellationRequests();
        Task<List<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);
        Task<string> UpdateOrderStatusAsync(string orderId, string status);
        Task<string> RespondToCancelRequest(CancelRequestDTO cancelRequestDTO);
        Task<string> RemoveItemFromCart(string orderId, string itemId);
        Task UpdateOrderDetailsAsync(string orderId, OrderDTO orderDTO);
        Task<OrderResponseDTO> GetCustomerCartOrderAsync(string customerId);
        Task DeleteOrderAsync(string orderId);
        Task<string> CancelOrderAsync(string orderId, string note, string canceledBy);
        Task<string> PlaceOrderAsync(string orderId, string address, string tel);
        Task<string> ItemDeliverAsync(string itemId);
        Task<string> MakeCancelOrderRequestAsync(CancelRequestDTO cancelRequestDTO);
    }
}
