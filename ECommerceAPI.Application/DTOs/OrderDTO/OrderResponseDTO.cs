using ECommerceAPI.Core.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.OrderDTO
{
    public class OrderResponseDTO
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string CanceledBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> Items { get; set; }

        public static OrderResponseDTO ItemMapper(Order order, List<OrderItem> items)
        {
            return new OrderResponseDTO
            {
                CustomerId = order.CustomerId,
                OrderId = order.OrderId,
                Status = order.Status,
                Note = order.Note,
                CanceledBy = order.CanceledBy,
                CreatedAt = order.CreatedAt,
                Items = items
            };

        }

    }

}
