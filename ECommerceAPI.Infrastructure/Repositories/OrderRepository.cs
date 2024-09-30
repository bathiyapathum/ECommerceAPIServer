using ECommerceAPI.Core.Entities;
using ECommerceAPI.Infrastructure.Persistance;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Repositories
{
    public class OrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Order order)
        {
            await _context.FirestoreDatabase.Collection("Orders").Document(order.OrderId.ToString()).SetAsync(order);
        }
        public async Task<List<Order>> GetAllAsync()
        {
            var orders = await _context.FirestoreDatabase.Collection("Orders").GetSnapshotAsync();
            return orders.Select(orders => orders.ConvertTo<Order>()).ToList();
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
        public async Task UpdateOrderAsync(string orderId, Dictionary<string, object> updatedFields)
        {
            // Retrieve the document by orderId
            var documentRef = _context.FirestoreDatabase.Collection("Orders").Document(orderId);

            // Check if the document exists
            var snapshot = await documentRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                // Update only the provided fields in the dictionary
                await documentRef.UpdateAsync(updatedFields);
            }
            else
            {
                throw new Exception("Order not found.");
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


    }
}
