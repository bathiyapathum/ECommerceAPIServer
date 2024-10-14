/******************************************************************************************
 * CancelRequest.cs
 * 
 * This class represents a cancellation request for an order within the e-commerce application. 
 * It defines the properties required to store and manage cancellation requests in the Firestore database, 
 * including details about the order, customer, and status of the request.
 * 
 * Properties:
 * - RequestId: Unique identifier for the cancellation request.
 * - OrderId: The ID of the order that is being requested for cancellation.
 * - CustomerId: The ID of the customer making the cancellation request.
 * - Status: Current status of the cancellation request (default is "PENDING").
 * - RequestNote: Additional notes provided by the customer regarding the cancellation.
 * - ResponseNote: Notes or comments provided in response to the cancellation request.
 * - ResponsedBy: Identifier for the user who responded to the cancellation request.
 * - CreatedAt: The timestamp when the cancellation request was created.
 * - ResolvedAt: The timestamp when the cancellation request was resolved, if applicable.
 * 
 * Author: Herath R P N M
 * Registration Number: IT21177828
 * Date: 2024-10-08
 * 
 * This entity is crucial for tracking and managing order cancellations effectively 
 * within the e-commerce system, ensuring that customer requests are handled in a timely manner.
 ******************************************************************************************/

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
        
        [FirestoreProperty("resolvedAt")]
        public DateTime? ResolvedAt { get; set; }
    }
}
