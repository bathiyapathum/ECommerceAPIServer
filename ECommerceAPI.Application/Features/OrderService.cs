using ECommerceAPI.Application.DTOs.NotificationDTO;
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
        /**
         * @CreateOrderAsync - This method initiates an order for a customer.
         *
         * If the order does not exist in the customer's cart, it creates a new order
         * and adds the provided item details to the cart. The item is then added to the
         * OrderItem collection, allowing vendors to access the relevant order information.
         * Finally, it updates the Order with a reference to the newly added OrderItem.
         *
         * If the order already exists, the method checks if the product is part of the
         * current order. If the product is found, it updates the quantity and price in the
         * specific OrderItem. If the product is not present, it creates a new OrderItem and
         * adds a reference to the Order's items array.
         *
         * Params: Accepts CustomerId and the selected item to be added to the order.
         */

        public async Task CreateOrderAsync(OrderDTO orderDTO)
        {
            try
            {
                //Get customer order if exist in the cart
                Order details = await GetCustomerOrderAsync(orderDTO.CustomerId);

                if (details == null)
                {
                    //create order and add orderItem
                    Debug.WriteLine("Order is not exist.");

                    var order = new Order
                    {
                        OrderId = Guid.NewGuid().ToString(),
                        CustomerId = orderDTO.CustomerId,
                        IsInCart = true,
                        CreatedAt = DateTime.UtcNow,
                    };

                    var result =  await _orderRepository.CreateAsync(order);

                    if (result) 
                    {
                        //Create orderItems
                        var orderItem = new OrderItem
                        {
                            ItemId = Guid.NewGuid().ToString(),
                            OrderId = order.OrderId,
                            ProductId = orderDTO.ProductId,
                            VendorId = orderDTO.VendorId,
                            Quantity = orderDTO.Quantity,
                            Price = orderDTO.Price,
                            ProductName = orderDTO.ProductName,
                        };

                        var orderItemsRes = await _orderRepository.CreateOrderItemAsync(orderItem);

                        //Create reference to create order
                        var orderRef = new OrderItemReference
                        {
                            ProductId = orderDTO.ProductId,
                            ItemId = orderItem.ItemId,
                            VendorId = orderDTO.VendorId
                        };

                        List<OrderItemReference> refList = [orderRef];

                        if (orderItemsRes)
                        {
                            //update the main order to include order item Ref in array

                            var updatedFields = new Dictionary <string, object>{
                                { "items", refList}
                            };

                            var updatedOrder = await _orderRepository.UpdateOrderAsync(order.OrderId, updatedFields);

                            if (updatedOrder)
                            {
                                Debug.WriteLine("Order updated successfully");
                            }
                            else
                            {
                                Debug.WriteLine("Something went wrong while updating order");
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Something went wrong while creating order");
                    }
                }
                else {
                    //Order is exist
                    Debug.WriteLine("Order found.");

                    var items = details.Items;
                    bool isItemExist = false;

                    if(items != null)
                    {
                        Debug.WriteLine("Item is not null.");
                        Debug.WriteLine(details);
                    }

                    //Create a list with existing items
                    List<OrderItemReference> refList = [];
                    foreach (var item in items)
                    {
                        refList.Add(new OrderItemReference
                        {
                            ItemId = item.ItemId,
                            VendorId = item.VendorId,
                            ProductId = item.ProductId,
                        });
                        
                    }

                    //check item exist or not
                    foreach(var item in refList)
                    {
                        //if item exist in the order append the price and quantity
                        if(item.ProductId == orderDTO.ProductId && item.VendorId == orderDTO.VendorId)
                        {
                            Debug.WriteLine("Item in the list");
                            isItemExist = true;
                            var updatedField = new Dictionary<string, object>
                            {
                                {"quantity", orderDTO.Quantity },
                                {"price", orderDTO.Price }
                            };

                            //update item
                            await _orderRepository.UpdateOrderItemAsync(item.ItemId, updatedField);
                            break;
                        }

                    }

                    //if item not exist in the list create new item
                    if (!isItemExist)
                    {
                        Debug.WriteLine("Item not in the list");

                        OrderItem newOrderItem = new()
                        {
                            ItemId = Guid.NewGuid().ToString(),
                            OrderId = details.OrderId,
                            VendorId = orderDTO.VendorId,
                            ProductId = orderDTO.ProductId,
                            ProductName = orderDTO.ProductName,
                            Quantity = orderDTO.Quantity,
                            Price = orderDTO.Price,
                        };

                        var result = await _orderRepository.CreateOrderItemAsync(newOrderItem);

                        if (result)
                        {
                            //append orderItem
                            var orderRef = new OrderItemReference
                            {
                                ProductId = orderDTO.ProductId,
                                ItemId = newOrderItem.ItemId,
                                VendorId = orderDTO.VendorId
                            };
                            refList.Add(orderRef);
                        }
                    }

                    var updatedFields = new Dictionary<string, object>{
                        { "items", refList}
                    };

                    var updatedOrder = await _orderRepository.UpdateOrderAsync(details.OrderId, updatedFields);

                    if (updatedOrder)
                    {
                        Debug.WriteLine("Order updated successfully");
                    }
                    else
                    {
                        Debug.WriteLine("Something went wrong while updating order");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Order> GetCustomerOrderAsync(string customerId)
        {
            try
            {
                var order = await _orderRepository.GetCustomerOrderAsync(customerId);
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
