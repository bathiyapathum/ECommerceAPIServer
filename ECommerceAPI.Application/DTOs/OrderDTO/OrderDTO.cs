using ECommerceAPI.Core.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.OrderDTO
{
    public class OrderDTO
    {
        public string CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string CanceledBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DispatchedAt { get; set; }
        public DateTime DeliveredAt { get; set; }

    }
    public class OrderItemDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string VendorId { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
    }
}
