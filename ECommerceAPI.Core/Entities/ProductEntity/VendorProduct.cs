using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [FirestoreProperty("feedbackInfo")]
        public List<FeedbackInfo> FeedbackInfo { get; set; }
    }

    public enum VendorStockStatus
    {
        Available,
        LowStock,
        Pending,
        OutOfStock
    }

    [FirestoreData]
    public class FeedbackInfo
    {
        [FirestoreProperty("customerId")]
        public string CustomerId { get; set; }

        [FirestoreProperty("orderId")]
        public string OrderId { get; set; }

        [FirestoreProperty("feedbackMessage")]
        public string FeedbackMessage { get; set; }

        [FirestoreProperty("rating")]
        public int Rating { get; set; }

        [FirestoreProperty("date")]
        public DateTime Date { get; set; }
    }
}
