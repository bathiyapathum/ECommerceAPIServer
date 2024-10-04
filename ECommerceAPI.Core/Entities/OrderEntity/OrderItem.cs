using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Core.Entities.OrderEntity
{
    [FirestoreData]
    public class OrderItem
    {
        [FirestoreProperty("itemId")]
        public string ItemId { get; set; }    
        
        [FirestoreProperty("orderId")]
        public string OrderId { get; set; }        
        
        [FirestoreProperty("productId")]
        public string ProductId { get; set; }

        [FirestoreProperty("vendorId")]
        public string VendorId { get; set; }

        [FirestoreProperty("productName")]
        public string ProductName { get; set; }

        [FirestoreProperty("quantity")]
        public int Quantity { get; set; }

        [FirestoreProperty("price")]
        public string Price { get; set; }

        [FirestoreProperty("isCanceled")]
        public bool IsCanceled { get; set; } = false;
        
        [FirestoreProperty("isActive")]
        public bool IsActive { get; set; } = false;

        [FirestoreProperty("status")]
        public string Status { get; set; } = "Hidden";

        public override string ToString()
        {
            return $"ItemId: {ItemId}, " +
                   $"ProductId: {ProductId}, " +
                   $"ProductName: {ProductName}, " +
                   $"VendorId: {VendorId}, " +
                   $"Quantity: {Quantity}, " +
                   $"Price: {Price}, " +
                   $"IsCanceled: {IsCanceled}, " +
                   $"Status: {Status}";
        }

    }
}
