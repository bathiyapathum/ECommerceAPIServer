/******************************************************************************************
 * OrderRepository.cs
 * 
 * This class implements the IOrderService interface and provides various functionalities 
 * for managing orders in the e-commerce system. The OrderService handles the creation, 
 * retrieval, updating, and deletion of orders, as well as the management of order items, 
 * cancellations, and notifications.
 * 
 * Contributors:
 * - Herath R P N M - Admin and csr fuctionalities and vendor order retrival
 * - Registration No: IT21177828: 
 *    - Implemented the following methods:
 *     - Get all orders
 *     - Update order status to delivered
 *     - Cancel order
 *     - Mark item as delivered
 *     - Request cancellation
 *     - Retrieve all cancellation requests
 *     - Update cancellation response
 * 
 * - Hansana K. T - customer order creation and customer order retrival
 * - Registration No: IT21167850:
 *    - Implemented the following methods:
 *     - Create order
 *     - Retrieve a specific order
 *     - Retrieve customer cart order
 *     - Retrieve customer placed orders
 *     - Remove item from cart
 *     - Place order
 *     - Delete order
 * 
 * Date: 2024/08/10
 * 
 ******************************************************************************************/

using ECommerceAPI.Core.Entities.OrderEntity;
using ECommerceAPI.Infrastructure.Persistance;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MongoDB.Bson;
using ECommerceAPI.Core.Entities.UserEntity;
using Google.Cloud.Firestore.V1;
using static Google.Rpc.Context.AttributeContext.Types;
using Google.Apis.Logging;

namespace ECommerceAPI.Infrastructure.Repositories
{
    public class OrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public FirestoreDb FirestoreDatabase => _context._firestoreDb;

        /***
         * Creating order
         * Author : Hansana K T - IT21167850
         ***/
        public async Task<bool> CreateAsync(Order order)
        {
            try
            {
                var orderItem = await _context.FirestoreDatabase.Collection("Orders").Document(order.OrderId.ToString()).SetAsync(order);
                if (orderItem != null) {
                    return true;
                }      
                return false ;
            }catch (Exception ex)
            {
                throw new Exception($"Error while execution of CreateAsync:  {ex.Message}");
            }
        }

        /***
         * GetAllAsync get all orders for admins and csr
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<List<Order>> GetAllAsync()
        {
            var orders = await _context.FirestoreDatabase.Collection("Orders").GetSnapshotAsync();
            return orders.Select(orders => orders.ConvertTo<Order>()).ToList();
        }

        /***
         * GetAllCancelRequests get all cancel request for admin and csr
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<List<CancelRequest>> GetAllCancelRequests()
        {
            var requests = await _context.FirestoreDatabase.Collection("CancelRequest").GetSnapshotAsync();
            return requests.Select(request => request.ConvertTo<CancelRequest>()).ToList();
        }

        /***
         * GetOrderbyIdAsync orders by its order id
         * Author : Hansana K T - IT21167850
         ***/
        public async Task<Order> GetOrderbyIdAsync(string orderId)
        {
            return await _context.FirestoreDatabase.Collection("Orders")
                .WhereEqualTo("orderId", orderId)
                .Limit(1)
                .GetSnapshotAsync()
                .ContinueWith(task =>
            {
                var snapshot = task.Result;
                if (snapshot.Count == 0)
                {
                    return null;
                }
                return snapshot.Documents[0].ConvertTo<Order>();
            });

        }

        /***
         * GetCustomerOrderAsync get order that are in the cart
         * Author : Hansana K T - IT21167850
         ***/
        public async Task<Order> GetCustomerOrderAsync(string customerId)
        {
            return await _context.FirestoreDatabase.Collection("Orders")
                .WhereEqualTo("customerId", customerId)
                .WhereEqualTo("isInCart", true)
                .Limit(1)
                .GetSnapshotAsync()
                .ContinueWith(task =>
                {
                    var snapshot = task.Result;
                    if (snapshot.Count == 0)
                    {
                        return null;
                    }
                    return snapshot.Documents[0].ConvertTo<Order>();
                });
        }

        /***
         * GetOrderStats for Show in teh admin pannel 
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<Dictionary<string, int>> GetOrderStats()
        {
            try
            {
                var orderStats = new Dictionary<string, int> {
                { "CANCELED", 0 },
                { "DELIVERED", 0 },
                { "PENDING", 0 },
                { "PARTIALY_DELIVERED",0 }
                };

                var snapshot = await _context.FirestoreDatabase.Collection("Orders")
                  .WhereEqualTo("isInCart", false)
                  .Limit(200)
                  .GetSnapshotAsync();

                if (snapshot.Count == 0)
                {
                    return orderStats;
                }

                foreach (var document in snapshot.Documents)
                {
                    var status = document.GetValue<string>("status");

                    if (orderStats.ContainsKey(status.ToUpper()))
                    {
                        orderStats[status.ToUpper()] += 1;
                    }
                    if("PARTIALY-DELIVERED".Equals(status, StringComparison.CurrentCultureIgnoreCase))
                    {
                        orderStats["PARTIALY_DELIVERED"] += 1;
                    }
                }

                return orderStats;
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while getting order stats: {ex.Message}");
            }
        }


        /***
         * GetCustomerPlacdOrderAsync get customers previous placed orders
         * Author : Hansana K T - IT21167850
         ***/
        public async Task<List<Order>> GetCustomerPlacdOrderAsync(string customerId)
        {

            var requests = await _context.FirestoreDatabase.Collection("Orders")
                .WhereEqualTo("customerId", customerId)
                .WhereEqualTo("isInCart", false)
                .Limit(100)
                .GetSnapshotAsync();
            if (requests.Count == 0)
            {
                return null;
            }
            return requests.Select(request => request.ConvertTo<Order>()).ToList();
        }

        /***
         * GetVendorOrderAsync get orders specifically for verdors that active state
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<List<OrderItem>> GetVendorOrderAsync(string vendorId)
        {
            try
            {
                var snapshot = await _context.FirestoreDatabase.Collection("OrderItem")
                    .WhereEqualTo("vendorId", vendorId)
                    .WhereEqualTo("isActive", true)
                    .Limit(100)
                    .GetSnapshotAsync();

                if (snapshot.Count == 0)
                {
                    return null;
                }

                return snapshot.Select(orders => orders.ConvertTo<OrderItem>()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while GetVendorOrderAsync:{ex.Message}");
            }
               
               
        }


        /***
         * Get OrderItems by OrderID, ProductID, CustomerID
         * Author : Hansana K T - IT21167850
         ***/
       
        public async Task<OrderItem> GetCustomerOrderItemsAsync(string orderId, string customerId, string productId)
        {
            try
            {
                var snapshot = await _context.FirestoreDatabase.Collection("OrderItems")
                    .WhereEqualTo("orderId", orderId)
                    .WhereEqualTo("customerId", customerId)
                    .WhereEqualTo("productId", productId)
                    .Limit(1)
                    .GetSnapshotAsync();

                if (snapshot.Count == 0)
                {
                    return null;
                }

                // Convert the first matching document to an OrderItem object
                return snapshot.Documents[0].ConvertTo<OrderItem>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving order item: {ex.Message}");
            }
        }

        /***
         * GetVendorOrderItemsAsync for get active vendor orders for specific vendors 
         * Author : Herath R P N M - IT21177828 
         ***/
        public async Task<OrderItem> GetVendorOrderItemsAsync(string itemId)
        {
            try
            {
                var snapshot = await _context.FirestoreDatabase.Collection("OrderItem")
                    .WhereEqualTo("itemId", itemId)
                    .WhereEqualTo("isActive", true)
                    .Limit(100)
                    .GetSnapshotAsync();

                if (snapshot.Count == 0)
                {
                    return null;
                }
                return snapshot.Documents[0].ConvertTo<OrderItem>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while GetVendorOrderAsync:{ex.Message}");
            }
               
               
        }

        /***
         * GetVendorOrderItemByIdAsync for identify orders related to vendors
         * Author : Hansana K T - IT21167850
         ***/
        public async Task<OrderItem> GetVendorOrderItemByIdAsync(string itemId)
        {
            try
            {
                var snapshot = await _context.FirestoreDatabase.Collection("OrderItem")
                    .WhereEqualTo("itemId", itemId)
                    .Limit(1)
                    .GetSnapshotAsync();

                if (snapshot.Count == 0)
                {
                    return null;
                }
                return snapshot.Documents[0].ConvertTo<OrderItem>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while GetVendorOrderAsync:{ex.Message}");
            }


        }

        /***
         * UpdateOrderItemAsync for update order items
         * Author : Hansana K T - IT21167850
         ***/
        //Update OrderItems
        public async Task<bool> UpdateOrderItemAsync(string itemId, Dictionary<string, object> updatedField)
        {
            try
            {
                var documentRef = _context.FirestoreDatabase.Collection("OrderItem").Document(itemId);
                var snapshot = await documentRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    await documentRef.UpdateAsync(updatedField);
                    return true;
                }
                else
                {
                    return false;

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while execution of UpdateOrderItemAsync:  {ex.Message}");

            }

        }

        /***
         * CreateOrderItemAsync for create order items
         * Author : Hansana K T - IT21167850
         ***/
        //Create OrderItems
        public async Task<bool> CreateOrderItemAsync(OrderItem orderItem)
        {
            try
            {   
                var result = await _context.FirestoreDatabase.Collection("OrderItem").Document(orderItem.ItemId.ToString()).SetAsync(orderItem);
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while execution of CreateOrderItemAsync:  {ex.Message}");

            }

        }

        /***
         * UpdateOrderAsync for update order
         * Author : Hansana K T - IT21167850
         ***/
        //Update order details
        public async Task<bool> UpdateOrderAsync(string orderId, Dictionary<string, object> updatedFields)
        {
            try
            {
                var documentRef = _context.FirestoreDatabase.Collection("Orders").Document(orderId);
                var snapshot = await documentRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    await documentRef.UpdateAsync(updatedFields);
                    return true;
                }
                else
                {
                    return false;
                    throw new Exception("Order not found.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while executing UpdateOrderAsync: {ex.Message}");
            
            }

        }

        /***
         * DeleteOrderAsync for delete orders
         * Author : Hansana K T - IT21167850
         ***/
        public async Task DeleteOrderAsync(string orderId)
        {
            // Retrieve the document by orderId and delete it
            var documentRef = _context.FirestoreDatabase.Collection("Orders").Document(orderId);

            var snapshot = await documentRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                // Delete the document
                await documentRef.DeleteAsync();
            }
            else
            {
                throw new Exception("Order not found.");
            }
        }

        /***
         * CancelOrderAsync this method is used to cancel the order by the customer used Admin and CSR
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<bool> CancelOrderAsync(string orderId, string note, string canceledBy)
        {
            try
            {
                var documentRef = _context.FirestoreDatabase.Collection("Orders").Document(orderId);

                var snapshot = await documentRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    var refData = snapshot.ConvertTo<Order>();

                    if (refData != null)
                    {                        
                        var order = new Dictionary<string, object>
                        {                    
                            {"status" , "CANCELED" },
                            {"note" , note },
                            {"canceledBy" , canceledBy },

                        };
                        // Cancel Order
                        var result = await documentRef.UpdateAsync(order);
                        return true;

                    }
                    return false;
                }
                else
                {
                    return false;
                    throw new Exception("Order not found.");
                }

            }
            catch (Exception ex) 
            { 
                throw new Exception($"Something went wrong while CancelOrderAsync: {ex.Message}");
            }

        }

        /***
         * GetOrderAsync
         * Author : Hansana K T - IT21167850
         ***/
        public async Task<Order> GetOrderAsync(string orderId)
        {
            var documentSnapshot = await _context.FirestoreDatabase.Collection("Orders").Document(orderId).GetSnapshotAsync();

            if (!documentSnapshot.Exists)
            {
                return null;
            }

            return documentSnapshot.ConvertTo<Order>();
        }

        /***
         * Creating order cancel request
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<bool> CreateOrderCancelRequest(CancelRequest cancelRequest)
        {
            try
            {
                var result = await _context.FirestoreDatabase.Collection("CancelRequest").Document(cancelRequest.RequestId).SetAsync(cancelRequest);
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while execution of CreateOrderCancelRequest:  {ex.Message}");

            }

        }

        /***
         * GetRequestCancelationByOrderAsync for orders cancellation requests
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<bool> GetRequestCancelationByOrderAsync(string orderId)
        {
            return await _context.FirestoreDatabase.Collection("CancelRequest")
                .WhereEqualTo("orderId", orderId)
                .Limit(1)
                .GetSnapshotAsync()
                .ContinueWith(task =>
                {
                    var snapshot = task.Result;
                    if (snapshot.Count > 0)
                    {
                        return false;
                    }
                    return true;
                });
        }

        /***
         * GetRequestCancelationByOrderForResponseAsync for orders cancellation requests
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<CancelRequest> GetRequestCancelationByOrderForResponseAsync(string requestId)
        {
            var snapshot = await _context.FirestoreDatabase.Collection("CancelRequest")
                 .WhereEqualTo("requestId", requestId)
                 .Limit(1)
                 .GetSnapshotAsync();

            if (snapshot.Count == 0)
            {
                return null;
            }
            return snapshot.Documents[0].ConvertTo<CancelRequest>();
        }


        /***
         * Remove order item from order
         * Author : Hansana K T - IT21167850
         ***/
        public async Task<bool> RemoveItemFromOrder(string itemId)
        {
            try
            {
                var documentRef = _context.FirestoreDatabase.Collection("OrderItem").Document(itemId);
                var snapshot = await documentRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    await documentRef.DeleteAsync();
                    return true;
                }
                else
                {
                    return false;
                    throw new Exception("OrderItem not found.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while executing RemoveItemFromOrder: {ex.Message}");

            }

        }

        /***
         * ResponseToCancelOrderRequest for orders cancellation requests
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<bool> ResponseToCancelOrderRequest(string requestId, Dictionary<string, object> updatedFields)
        {
            try
            {
                var documentRef = _context.FirestoreDatabase.Collection("CancelRequest").Document(requestId);
                var snapshot = await documentRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    await documentRef.UpdateAsync(updatedFields);
                    return true;
                }
                else
                {
                    return false;
                    throw new Exception("CancelRequest not found.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while executing ResponseToCancelOrderRequest: {ex.Message}");

            }

        }

        /***
         * GetTotalRevenue for orders
         * Author : Herath R P N M - IT21177828
         ***/
        public async Task<string> GetTotalRevenue()
        {
            try
            {
                decimal totalRevenue = 0;
                var requests = await _context.FirestoreDatabase.Collection("OrderItem")
                  .WhereEqualTo("status", "DELIVERED")
                  .Limit(200)
                  .GetSnapshotAsync();

                if (requests.Count == 0)
                {
                    return totalRevenue.ToString();
                }
                foreach (var document in requests.Documents)
                {                   
                    var priceString = document.GetValue<string>("price");
                   
                    if (!string.IsNullOrEmpty(priceString))
                    {
                        if (decimal.TryParse(priceString, out decimal price))
                        {
                            totalRevenue += price;
                        }
                    }
                }
                return totalRevenue.ToString("F2");
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong while GetTotalRevenue: {ex.Message}");
            }
        }

    }
}
