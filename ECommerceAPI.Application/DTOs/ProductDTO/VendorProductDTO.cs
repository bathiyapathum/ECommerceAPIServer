// --------------------------------------------------------------------------------------------------------------------
// VendorProductDTO: Defines the data transfer object for vendor products.
// This DTO is used to transfer product data between the client and server.
// Author: Arachchi D.S.U - IT21182914
// Date: 06/10/2024
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace ECommerceAPI.Application.DTOs.ProductDTO
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
        public string StockStatus { get; set; }
        public string ImageUrl { get; set; }

        public string Type { get; set; } = "Anyone"; //default value

        public string Size { get; set; } = "Default";
        public bool IsActive { get; set; } = false;
    }
}
