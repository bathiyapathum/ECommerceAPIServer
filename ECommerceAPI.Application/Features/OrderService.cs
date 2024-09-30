using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities.OrderEntity;
using ECommerceAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features
{
    internal class OrderService : IOrderService
    {
        private readonly  OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;        
        }

        public Task CancelOrderAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateOrderAsync(OrderDTO orderDTO)
        {
            try
            {
                var order = new Order
                {
                    OrderId = Guid.NewGuid().ToString(),
                    CustomerId = orderDTO.CustomerId,
                    Items = orderDTO.Items,
                    CreatedAt = DateTime.UtcNow,
                    DeliveredAt = orderDTO.DeliveredAt,
                    DispatchedAt = orderDTO.DispatchedAt,
                    Status = orderDTO.Status,
                };
                await _orderRepository.CreateAsync(order);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task DeleteOrderAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderDetailsAsync(OrderDTO orderDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderStatusAsync(string orderId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
