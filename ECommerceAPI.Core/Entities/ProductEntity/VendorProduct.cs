using Google.Cloud.Firestore;
using System;

namespace ECommerceAPI.Core.Entities.ProductEntity
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

        // Store StockStatus as string
        [FirestoreProperty("stockStatus")]
        public string StockStatus { get; set; } = VendorStockStatus.Available.ToString();

        [FirestoreProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [FirestoreProperty("type")]
        public string Type { get; set; } = "Anyone"; // Default value

        [FirestoreProperty("size")]
        public string Size { get; set; } = "Default"; // Default value
    }

    public enum VendorStockStatus
    {
        Available,
        LowStock,
        Pending,
        OutOfStock
    }
}
