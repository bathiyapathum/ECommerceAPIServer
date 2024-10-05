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
        public string Status { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string CanceledBy { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public string ProductName { get; set; }
        public int DeliveredItems { get; set; }
        public string VendorId { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DispatchedAt { get; set; }
        public DateTime DeliveredAt { get; set; }
        public List<OrderItemReference> Items { get; set; }

        public override string ToString()
        {
            var itemsString = Items != null
                ? string.Join(", ", Items.Select(item => item.ToString()))
                : "No items";

            return $"CustomerId: {CustomerId},\n Status: {Status},\n Note: {Note},\n CanceledBy: {CanceledBy},\n " +
                   $"CreatedAt: {CreatedAt},\n DispatchedAt: {DispatchedAt},\n DeliveredAt: {DeliveredAt},\n " +
                   $"Items: [{itemsString}]";
        }

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
