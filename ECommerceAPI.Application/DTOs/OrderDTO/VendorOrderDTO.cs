using ECommerceAPI.Core.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.OrderDTO
{
    public class VendorOrderDTO
    {
        public string ItemId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string VendorId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public string Address { get; set; }
        public string Size { get; set; }
        public string Tel { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"ItemId: {ItemId},\n OrderId: {OrderId},\n ProductId: {ProductId},\n VendorId: {VendorId},\n " +
                   $"ProductName: {ProductName},\n Quantity: {Quantity},\n Status: {Price},\n " +
                   $"Status: [{Status}]"+ $"CreatedAt: [{CreatedAt}]"+ $"Address: [{Address}]"+ $"Tel: [{Tel}]"+
                   $"ImageUrl: [{ImageUrl}]"+ $"Size: [{Size}]";
        }

        public static VendorOrderDTO OrderMapper(Order order, OrderItem orderItem)
        {
            if(order == null || orderItem == null)
            {
                return null;
            }
            VendorOrderDTO vendorOrderDTO = new VendorOrderDTO
            {
                ItemId = orderItem.ItemId,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                VendorId = orderItem.VendorId,
                ProductName = orderItem.ProductName,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
                Address = order.Address,
                Size = orderItem.Size,
                Tel = order.Tel,
                ImageUrl = orderItem.ImageUrl,
                Status = orderItem.Status,
                CreatedAt = orderItem.CreatedAt,
            };
            return vendorOrderDTO;
        }
    }
}
