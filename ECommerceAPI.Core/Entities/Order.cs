using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace ECommerceAPI.Core.Entities
{
    [FirestoreData]
    public class Order
    {
        [FirestoreProperty("orderId")]
        public string OrderId { get; set; }

        [FirestoreProperty("customerId")]
        public string CustomerId { get; set; }

        [FirestoreProperty("items")]
        public List<OrderItem> Items { get; set; }

        [FirestoreProperty("status")]
        public string Status { get; set; } // Processing, Delivered, Canceled, etc.

        [FirestoreProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty("dispatchedAt")]
        public DateTime? DispatchedAt { get; set; }

        [FirestoreProperty("DeliceredAt")]
        public DateTime? DeliveredAt { get; set; }
    }

    [FirestoreData]
    public class OrderItem 
    {
        [FirestoreProperty]
        public string ProductId { get; set; }

        [FirestoreProperty]
        public string ProductName { get; set; }

        [FirestoreProperty]
        public string VendorId { get; set; }

        [FirestoreProperty]
        public int Quantity { get; set; }

        [FirestoreProperty]
        public string Price { get; set; }
    }
}
