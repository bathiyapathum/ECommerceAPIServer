using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Core.Entities.UserEntity
{
    [FirestoreData]
    public class Feedback
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("customerId")]
        public string CustomerId { get; set; }

        [FirestoreProperty("orderId")]
        public string OrderId { get; set; }

        [FirestoreProperty("productId")]
        public string ProductId { get; set; }

        [FirestoreProperty("feedbackMessage")]
        public string FeedbackMessage { get; set; }

        [FirestoreProperty("rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [FirestoreProperty("date")]
        public DateTime Date { get; set; }
    }
}
