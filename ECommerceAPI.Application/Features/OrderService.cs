﻿using ECommerceAPI.Application.DTOs.NotificationDTO;
using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities.OrderEntity;
using ECommerceAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features
{
    internal class OrderService : IOrderService
    {
        private readonly  OrderRepository _orderRepository;
        private readonly  NotificationRepository _notificationRepository;

        public OrderService(OrderRepository orderRepository, NotificationRepository notificationRepository)
        {
            _orderRepository = orderRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task CancelOrderAsync(string orderId, string note, string canceledBy)
        {
            try
            {
                await _orderRepository.CancelOrderAsync(orderId, note, canceledBy);
                Order order =await GetOrderAsync(orderId);
                NotificationService notificationService = new(_notificationRepository);
                NotificationDTO notification = new()
                {
                    IsRead = false,
                    Message = "Your order "+orderId+" Canceled.",
                    Reason = note,
                    UserId= order.CustomerId                                        
                };
                await notificationService.SendNotification(notification);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
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

        public async Task DeleteOrderAsync(string orderId)
        {
            try
            {
                await _orderRepository.DeleteOrderAsync(orderId);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Order> GetOrderAsync(string orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderbyIdAsync(orderId);
                return order;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOrderDetailsAsync(string orderId ,OrderDTO orderDTO)
        {
            try
            {
                var order = new Dictionary<string, object>
                {
                    {"items" , orderDTO.Items },
                    {"status" , orderDTO.Status }
                };
                await _orderRepository.UpdateOrderAsync(orderId, order);
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task UpdateOrderStatusAsync(string orderId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
