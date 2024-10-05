using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Core.Entities.OrderEntity
{
    [FirestoreData]
    public class CancelRequest
    {
        [FirestoreProperty("requestId")]
        public string RequestId { get; set; }

        [FirestoreProperty("orderId")]
        public string OrderId { get; set; }

        [FirestoreProperty("customerId")]
        public string CustomerId { get; set; }

        [FirestoreProperty("status")]
        public string Status { get; set; } = "PENDING";
        
        [FirestoreProperty("requestNote")]
        public string RequestNote { get; set; }
        
        [FirestoreProperty("responseNote")]
        public string ResponsNote { get; set; }
        
        [FirestoreProperty("responsedBy")]
        public string ResponsedBy { get; set; }

        [FirestoreProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
