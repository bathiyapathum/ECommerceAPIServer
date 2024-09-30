using System;

namespace ECommerceAPI.Application.DTOs
{
    public class VendorProductDTO
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public string VendorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string StockStatus { get; set; }  // New field for StockStatus
    }
}
