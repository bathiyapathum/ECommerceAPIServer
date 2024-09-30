using Google.Cloud.Firestore;
using System;

namespace ECommerceAPI.Core.Entities
{
    [FirestoreData]
    public class VendorProduct
    {
        [FirestoreDocumentId]
        public string ProductId { get; set; }

        [FirestoreProperty("name")]
        public string Name { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("price")]
        public double Price { get; set; }

        [FirestoreProperty("stockQuantity")]
        public int StockQuantity { get; set; }

        [FirestoreProperty("category")]
        public string Category { get; set; }

        [FirestoreProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [FirestoreProperty("vendorId")]
        public string VendorId { get; set; }

        [FirestoreProperty("stockStatus")]
        public VendorStockStatus StockStatus { get; set; } = VendorStockStatus.Available;
    }

    public enum VendorStockStatus
    {
        Available,
        LowStock,
        Pending,
        OutOfStock
    }
}
