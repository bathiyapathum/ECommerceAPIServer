using Google.Cloud.Firestore;
using System;

namespace ECommerceAPI.Core.Entities
{
    [FirestoreData]
    public class Product
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public double Price { get; set; }

        [FirestoreProperty]
        public string Description { get; set; }

        [FirestoreProperty]
        public int StockQuantity { get; set; }

        [FirestoreProperty]
        public string ImageUrl { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        public DateTime UpdatedAt { get; set; }

        [FirestoreProperty]
        public int CategoryId { get; set; }


    }
}
