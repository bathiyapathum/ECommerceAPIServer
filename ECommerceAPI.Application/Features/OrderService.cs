using ECommerceAPI.Application.DTOs.NotificationDTO;
using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities.OrderEntity;
using ECommerceAPI.Infrastructure.Repositories;
using ECommerceAPI.Application.Features;
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
        private readonly OrderRepository _orderRepository;
        private readonly NotificationRepository _notificationRepository;
        private readonly VendorProductRepository _vendorProductRepository;

        public OrderService(OrderRepository orderRepository, NotificationRepository notificationRepository, VendorProductRepository vendorProductRepository)
        {
            _orderRepository = orderRepository;
            _notificationRepository = notificationRepository;
            _vendorProductRepository = vendorProductRepository;
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

                    var result = await _orderRepository.CreateAsync(order);

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
                            ImageUrl = orderDTO.ImageUrl,
                            Size = orderDTO.Size,
                            CreatedAt = DateTime.UtcNow,
                        };

                        var orderItemsRes = await _orderRepository.CreateOrderItemAsync(orderItem);

                        //Create reference to create order
                        var orderRef = new OrderItemReference
                        {
                            ProductId = orderDTO.ProductId,
                            ItemId = orderItem.ItemId,
                            VendorId = orderDTO.VendorId,
                            Size = orderDTO.Size,
                        };

                        List<OrderItemReference> refList = [orderRef];

                        if (orderItemsRes)
                        {
                            //update the main order to include order item Ref in array

                            var updatedFields = new Dictionary<string, object>{
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
                else
                {
                    //Order is exist
                    Debug.WriteLine("Order found.");

                    var items = details.Items;
                    bool isItemExist = false;

                    if (items != null)
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
                            Size = item.Size,
                        });

                    }

                    //check item exist or not
                    foreach (var item in refList)
                    {
                        //if item exist in the order append the price and quantity
                        if (item.ProductId == orderDTO.ProductId && item.VendorId == orderDTO.VendorId && item.Size == orderDTO.Size)
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
                            ImageUrl = orderDTO.ImageUrl,
                            Size = orderDTO.Size,
                            CreatedAt = DateTime.UtcNow,
                        };

                        var result = await _orderRepository.CreateOrderItemAsync(newOrderItem);

                        if (result)
                        {
                            //append orderItem
                            var orderRef = new OrderItemReference
                            {
                                ProductId = orderDTO.ProductId,
                                ItemId = newOrderItem.ItemId,
                                VendorId = orderDTO.VendorId,
                                Size = orderDTO.Size,
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

        public async Task<OrderResponseDTO> GetCustomerCartOrderAsync(string customerId)
        {
            try
            {
                var order = await _orderRepository.GetCustomerOrderAsync(customerId);
                if (order == null)
                {
                    return null;
                }

                //create response object to return with product name, image, etc

                var orderItems = order.Items;

                var newItems = new List<OrderItem>();

                foreach (var item in orderItems)
                {
                    var result = await _orderRepository.GetVendorOrderItemByIdAsync(item.ItemId);

                    newItems.Add(new OrderItem
                    {
                        ItemId = result.ItemId,
                        ProductId = result.ProductId,
                        VendorId = result.VendorId,
                        ProductName = result.ProductName,
                        OrderId = result.OrderId,
                        Quantity = result.Quantity,
                        Price = result.Price,
                        ImageUrl = result.ImageUrl,
                        Size = result.Size,
                        CreatedAt = result.CreatedAt,
                    });

                }
                OrderResponseDTO response = OrderResponseDTO.ItemMapper(order, newItems);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        
        public async Task<List<OrderResponseDTO>> GetCustomerPlacedOrderAsync(string customerId)
        {
            try
            {
                var order = await _orderRepository.GetCustomerPlacdOrderAsync(customerId);
                if (order == null)
                {
                    return null;
                }

                List<OrderResponseDTO> orderResponseDTO = [];
                //create response object to return with product name, image, etc
                foreach (var item in order)
                {
                    var orderItems = item.Items;

                    var newItems = new List<OrderItem>();

                    foreach (var orderItem in orderItems)
                    {
                        var result = await _orderRepository.GetVendorOrderItemByIdAsync(orderItem.ItemId);

                        newItems.Add(new OrderItem
                        {
                            ItemId = result.ItemId,
                            ProductId = result.ProductId,
                            VendorId = result.VendorId,
                            ProductName = result.ProductName,
                            OrderId = result.OrderId,
                            Quantity = result.Quantity,
                            Price = result.Price,
                            ImageUrl = result.ImageUrl,
                            Size = result.Size,
                            CreatedAt = result.CreatedAt,
                        });

                    }
                    OrderResponseDTO response = OrderResponseDTO.ItemMapper(item, newItems);
                    orderResponseDTO.Add(response);


                }
                    return orderResponseDTO;

                //var orderItems = order.Items;

                //var newItems = new List<OrderItem>();

                //foreach (var item in orderItems)
                //{
                //    var result = await _orderRepository.GetVendorOrderItemByIdAsync(item.ItemId);

                //    newItems.Add(new OrderItem
                //    {
                //        ItemId = result.ItemId,
                //        ProductId = result.ProductId,
                //        VendorId = result.VendorId,
                //        ProductName = result.ProductName,
                //        OrderId = result.OrderId,
                //        Quantity = result.Quantity,
                //        Price = result.Price,
                //        ImageUrl = result.ImageUrl,
                //        Size = result.Size,
                //        CreatedAt = result.CreatedAt,
                //    });

                //}
                //OrderResponseDTO response = OrderResponseDTO.ItemMapper(order, newItems);

                //return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        //public async Task<string> GetTotalRevenue()
        //{
        //    try
        //    {
        //        var revenue = await _orderRepository.GetTotalRevenue();
        //        return revenue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        
        //public async Task<Dictionary<string, int>> GetOrderStats()
        //{
        //    try
        //    {
        //        var revenue = await _orderRepository.GetOrderStats();
        //        return revenue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

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

        public async Task<string> CancelOrderAsync(string orderId, string note, string canceledBy)
        {
            try
            {
                var existingItem =await _orderRepository.GetOrderAsync(orderId);

                NotificationService notificationService = new(_notificationRepository);

                if (existingItem == null)
                {
                    return "Order not found";
                }
                if (existingItem.IsInCart)
                {
                    return "Still order is in the cart";
                }
                else if (existingItem.Status == "CANCELED")
                {
                    return "Order is already canceled";
                }
                else if (existingItem.Status == "DELIVERED")
                {
                    return "Order is already delivered";
                }else if(existingItem.Status == "PARTIALY-DELIVERED")
                {
                    return "Order is PARTIALY-DELIVERED";
                }

                var response = await _orderRepository.CancelOrderAsync(orderId, note, canceledBy);

                if(!response)
                {
                    return "Something went wrong while canceling order";
                }

                //Order order = await GetOrderAsync(orderId);
                foreach (var item in existingItem.Items)
                {
                    var itemResult = await _orderRepository.UpdateOrderItemAsync(item.ItemId, new Dictionary<string, object> { { "status", "CANCELED" } });

                    if (itemResult)
                    {
                        NotificationDTO notification = new()
                        {
                            IsRead = false,
                            Message = $"Order {item.ItemId} has been canceled!",
                            Reason = "Customer request cancelation",
                            UserId = item.VendorId,
                        };
                        await notificationService.Send(notification);
                    }
                }

                NotificationDTO notificationCus = new()
                {
                    IsRead = false,
                    Message = "Your order " + orderId + " has canceled.",
                    Reason = note,
                    UserId = existingItem.CustomerId
                };

                var resutl = await notificationService.Send(notificationCus);
                if (resutl != null)
                {
                    return "Order Canceled Successfully";
                }
                return "Something went wrong while sending notification";
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

        public async Task UpdateOrderDetailsAsync(string orderId, OrderDTO orderDTO)
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

        public async Task<string> UpdateOrderStatusAsync(string orderId, string status)
        {
            try
            {
                var order = await _orderRepository.GetOrderAsync(orderId);
                if (order == null)
                {
                    return "Order not found";
                }
                else if (order.Status == "DELIVERED")
                {
                    return "Order is already delivered";
                }
                else if (order.Status == "CANCELED")
                {
                    return "Order is already canceled";
                }
                else
                {
                    if (status == "DELIVERED")
                    {
                        foreach (var item in order.Items)
                        {
                            var itemResult = await _orderRepository.UpdateOrderItemAsync(item.ItemId, new Dictionary<string, object> { { "status", "DELIVERED" } });

                            if (itemResult)
                            {
                                NotificationService notificationService = new(_notificationRepository);

                                NotificationDTO notification = new()
                                {
                                    IsRead = false,
                                    Message = $"Order {item.ItemId} has been delivered successfully",
                                    Reason = "Order delivered",
                                    UserId = item.VendorId,
                                };
                                await notificationService.Send(notification);
                            }
                        }
                    }

                    var response = await _orderRepository.UpdateOrderAsync(orderId, new Dictionary<string, object> { { "status", status } });
                    if (response)
                    {
                        NotificationService notificationService = new(_notificationRepository);

                        NotificationDTO notification = new()
                        {
                            IsRead = false,
                            Message = "Order placed with" + orderId + " for you.",
                            Reason = "Placing new order",
                            UserId = order.CustomerId
                        };
                        await notificationService.Send(notification);

                        return "Order status updated successfully";
                    }
                    else
                    {
                        return $"Something went wrong while updating order status to: {status} ";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> PlaceOrderAsync(string orderId, string address, string tel)
        {
            try
            {
                NotificationService notificationService = new(_notificationRepository);
                //place order
                Dictionary<string, object> updatedOrder = new()
                {
                    {"isInCart", false},
                    {"status", "PENDING"},
                    {"address", address},
                    {"tel", tel }
                };

                var existingOrder = await _orderRepository.GetOrderAsync(orderId);
                if (existingOrder == null) {
                    return "Order not found";
                }
                
                if(existingOrder.Status == "CANCELED")
                {
                    return "Order is already canceled";
                }

                if (existingOrder.Status == "DELIVERED")
                {
                    return "Order is already delivered";
                }
                if (existingOrder.Status == "PARTIALY-DELIVERED")
                {
                    return "Order is PARTIALY-DELIVERED";
                }
                if (existingOrder.Status == "PENDING")
                {
                    return "Order is already placed";
                }

                foreach (var item in existingOrder.Items)
                {
                    var orderItem = await _orderRepository.GetVendorOrderItemByIdAsync(item.ItemId);
                    var product = await _vendorProductRepository.GetVendorProductByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        return "Product item not found";
                    }
                    if (product.StockQuantity < orderItem.Quantity)
                    {
                        return $"Product {product.Name} is out of stock";
                    }

                }

                foreach(var item in existingOrder.Items)
                {
                    var orderItem = await _orderRepository.GetVendorOrderItemByIdAsync(item.ItemId);
                    var product = await _vendorProductRepository.GetVendorProductByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        return "Product item not found";
                    }
                    if (product.StockQuantity < orderItem.Quantity)
                    {
                        return $"Product {product.Name} is out of stock";
                    }
                    var updateResult = await _vendorProductRepository.UpdateVendorProductStockAsync(item.ProductId, new Dictionary<string, object> { { "stockQuantity", (product.StockQuantity - orderItem.Quantity) } });
                    if (!updateResult)
                    {
                        return "Something went wrong while updating product";
                    }

                }

                var result =  await _orderRepository.UpdateOrderAsync(orderId, updatedOrder);

                if (result)
                {
                    var order = await _orderRepository.GetOrderAsync(orderId);

                    foreach (var item in order.Items)
                    {
                        var itemResult = await _orderRepository.UpdateOrderItemAsync(item.ItemId, new Dictionary<string, object> { { "status", "PENDING" },{ "isActive", true } });

                        if (itemResult)
                        {
                            NotificationDTO notification = new()
                            {
                                IsRead = false,
                                Message = "Order placed with" + orderId + " for you.",
                                Reason = "Placing new order",
                                UserId = item.VendorId
                            };
                            await notificationService.Send(notification);
                        }                       
                    }

                    NotificationDTO Custnotification = new()
                    {
                        IsRead = false,
                        Message = "Congratulation your order: " + orderId + "has been placed successfully",
                        Reason = "Placing new order",
                        UserId = order.CustomerId
                    };
                    var res = await notificationService.Send(Custnotification);

                    if (res != null)
                    {
                        return "Order Placed Successfully";
                    }
                        return "Something went wrong while sending notification.";
                }
                else
                {
                    return "Something went wrong while placing order";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<string> MakeCancelOrderRequestAsync(CancelRequestDTO cancelRequestDTO)
        {
            try
            {
                var customerOrder = await _orderRepository.GetOrderAsync(cancelRequestDTO.OrderId);

                if (customerOrder == null)
                {
                    return "Order not found";
                }
                if(customerOrder.CustomerId != cancelRequestDTO.CustomerId)
                {
                    return "You are not authorized to cancel this order";
                }
                if (customerOrder.Status == "CANCELED")
                {
                    return "Order is already canceled";
                }
                if (customerOrder.Status == "DELIVERED")
                {
                    return "Order is already delivered";
                }

                var item =  await _orderRepository.GetRequestCancelationByOrderAsync(cancelRequestDTO.OrderId);

                if (!item)
                {
                    return "Cancel request already sent";
                }

                var request = new CancelRequest
                {
                    RequestId = Guid.NewGuid().ToString(),
                    OrderId = cancelRequestDTO.OrderId,
                    CustomerId = cancelRequestDTO.CustomerId,
                    RequestNote = cancelRequestDTO.RequestNote,
                    CreatedAt = DateTime.UtcNow,
                };

                var resutl = await _orderRepository.CreateOrderCancelRequest(request);
                if(resutl)
                {
                    return "Cancel request sent successfully";
                }
                else
                {
                    return "Something went wrong while sending cancel request";
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Some thing went wrong CancelOrderAsync:{ex.Message}");
            }

        }

        public async Task<List<CancelRequest>> GetAllCancellationRequests()
        {
            try
            {
                var requests = await _orderRepository.GetAllCancelRequests();
                return requests;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> RespondToCancelRequest(CancelRequestDTO cancelRequestDTO)
        {
            try
            {
                var item = await _orderRepository.GetRequestCancelationByOrderForResponseAsync(cancelRequestDTO.RequestId);

                if (item == null)
                {
                    return "Request not found";
                }

                if (item.Status == "CANCELED")
                {
                    return "Order is already Canceled";
                }else if (item.Status == "APPROVED")
                {
                    return "Order is already Approved";
                }

                Dictionary<string, object> updatedFields = new()
                {
                    {"status", cancelRequestDTO.Status },
                    {"responsedBy", cancelRequestDTO.ResponsedBy },
                    {"responseNote", cancelRequestDTO.ResponseNote },
                    {"resolvedAt", DateTime.UtcNow }
                };

                var response = await _orderRepository.ResponseToCancelOrderRequest(cancelRequestDTO.RequestId, updatedFields);

                if (!response)
                {
                    return "Something went wrong";
                }

                return "Cancel request response sent successfully";
            
            }
            catch (Exception ex)
            {
                throw new Exception($"Some thing went wrong CancelOrderAsync:{ex.Message}");
            }

        }


        public async Task<string> RemoveItemFromCart(string orderId, string itemId)
        {
            var order = await _orderRepository.GetOrderAsync(orderId);

            if (order == null)
            {
                return "Order not found";
            }

            var items = order.Items;


            foreach (var item in items) 
            {
                if (item.ItemId == itemId)
                {
                    var result = await _orderRepository.RemoveItemFromOrder(itemId);

                    if (result)
                    {
                        items.RemoveAll(x => x.ItemId == itemId);

                        var updatedFields = new Dictionary<string, object>
                        {
                            {"items", items }
                        };

                        var updatedOrder = await _orderRepository.UpdateOrderAsync(orderId, updatedFields);

                        if (updatedOrder)
                        {
                            return "Item removed from cart successfully";
                        }
                        else
                        {
                            return "Something went wrong while updating order";
                        }

                    }
                    else
                    {
                        return "Something went wrong while removing item from cart";
                    }
                }
            }
            return "Item not found in the cart";

        }
        


        public async Task<string> ItemDeliverAsync(string itemId)
        {
            try
            {
                var orderItem = await _orderRepository.GetVendorOrderItemByIdAsync(itemId);
                if (orderItem == null)
                {
                    return "Order Item not found";
                }
                var order = await _orderRepository.GetOrderAsync(orderItem.OrderId);
                if (order == null)
                {
                    return "Order not found";
                }
                if (order.Status == "CANCELED")
                {
                    return "Order is already canceled";
                }
                if (orderItem.Status == "DELIVERED")
                {
                    return "Order item is already delivered";
                }

                if (order.DeliveredItems == order.Items.Count)
                {
                    return "Order is already delivered";
                }
                if (order.DeliveredItems + 1 == order.Items.Count)
                {
                    await _orderRepository.UpdateOrderAsync(order.OrderId, new Dictionary<string, object> { { "status", "DELIVERED" }, { "deliveredItems", order.DeliveredItems + 1 }, { "deliveredAt", DateTime.UtcNow} });
                }
                else
                {
                    await _orderRepository.UpdateOrderAsync(order.OrderId, new Dictionary<string, object> { { "status", "PARTIALY-DELIVERED" }, { "deliveredItems", order.DeliveredItems + 1 } });
                }

                var result = await _orderRepository.UpdateOrderItemAsync(itemId, new Dictionary<string, object> { { "status", "DELIVERED" } });
                if (result)
                {
                    await _orderRepository.UpdateOrderAsync(itemId, new Dictionary<string, object> { { "isActive", false } });
                    return "Order Item Delivered Successfully";
                }
                else
                {
                    return "Something went wrong while updating item status";
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Some thing went wrong ItemDeliverAsync:{ex.Message}");
            }
        }


    }

}
