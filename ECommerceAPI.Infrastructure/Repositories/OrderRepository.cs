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

        //Creating Order
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
        public async Task<List<Order>> GetAllAsync()
        {
            var orders = await _context.FirestoreDatabase.Collection("Orders").GetSnapshotAsync();
            return orders.Select(orders => orders.ConvertTo<Order>()).ToList();
        }
        
        public async Task<List<CancelRequest>> GetAllCancelRequests()
        {
            var requests = await _context.FirestoreDatabase.Collection("CancelRequest").GetSnapshotAsync();
            return requests.Select(request => request.ConvertTo<CancelRequest>()).ToList();
        }

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


        //Get OrderItems by OrderID, ProductID, CustomerID
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

        public async Task DeleteOrderAsync(string orderId)
        {
            // Retrieve the document by orderId and delete it
            var documentRef = _context.FirestoreDatabase.Collection("Orders").Document(orderId);

            // Check if document exists
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

        public async Task<bool> CancelOrderAsync(string orderId, string note, string canceledBy)
        {
            try
            {
                // Retrieve the document by orderId and delete it
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


        public async Task<Order> GetOrderAsync(string orderId)
        {
            var documentSnapshot = await _context.FirestoreDatabase.Collection("Orders").Document(orderId).GetSnapshotAsync();

            if (!documentSnapshot.Exists)
            {
                return null;
            }

            return documentSnapshot.ConvertTo<Order>();
        }

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

            // Convert the first matching document to an CancelRequest object
            return snapshot.Documents[0].ConvertTo<CancelRequest>();
        }
        

        //Update order details
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


    }
}
