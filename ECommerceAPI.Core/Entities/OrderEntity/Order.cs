using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace ECommerceAPI.Core.Entities.OrderEntity
{
    [FirestoreData]
    public class Order
    {
        [FirestoreProperty("orderId")]
        public string OrderId { get; set; }

        [FirestoreProperty("customerId")]
        public string CustomerId { get; set; }

        [FirestoreProperty("status")]
        public string Status { get; set; }

        [FirestoreProperty("address")]
        public string Address { get; set; }

        [FirestoreProperty("isInCart")]
        public bool IsInCart { get; set; }

        [FirestoreProperty("note")]
        public string Note { get; set; }
        
        [FirestoreProperty("tel")]
        public string Tel { get; set; }

        [FirestoreProperty("canceledBy")]
        public string CanceledBy { get; set; }

        [FirestoreProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty("deliceredAt")]
        public DateTime? DeliveredAt { get; set; }

        [FirestoreProperty("items")]
        public List<OrderItemReference> Items { get; set; }

        public override string ToString()
        {
            var itemsString = Items != null && Items.Any()
                ? string.Join(",\n", Items.Select(item => item.ToString()))
                : "No items";

            return $"OrderId: {OrderId},\n" +
                   $"CustomerId: {CustomerId},\n" +
                   $"Status: {Status},\n" +
                   $"Note: {Note},\n" +
                   $"CanceledBy: {CanceledBy},\n" +
                   $"CreatedAt: {CreatedAt},\n" +
                   $"DeliveredAt: {(DeliveredAt.HasValue ? DeliveredAt.Value.ToString() : "Not Delivered")},\n" +
                   $"Items: [\n{itemsString}\n]";
        }

    }

    [FirestoreData]
    public class OrderItemReference
    {
        [FirestoreProperty("itemId")]
        public string ItemId { get; set; }

        [FirestoreProperty("productId")]
        public string ProductId { get; set; }

        [FirestoreProperty("vendorId")]
        public string VendorId { get; set; }        
        
        [FirestoreProperty("size")]
        public string Size { get; set; }

    }
}
