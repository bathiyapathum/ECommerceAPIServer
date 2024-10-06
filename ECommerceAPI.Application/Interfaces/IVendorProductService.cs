// --------------------------------------------------------------------------------------------------------------------
// IVendorProductService: Interface defining operations for vendor products and order management.
// This interface declares methods for creating, updating, deleting, and managing products,
// as well as handling stock levels and order details.
// Author: Arachchi D.S.U - IT21182914
// Date: 06/10/2024
// --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using ECommerceAPI.Application.DTOs.ProductDTO;
using ECommerceAPI.Core.Entities.OrderEntity;
using ECommerceAPI.Application.DTOs.OrderDTO;

namespace ECommerceAPI.Application.Interfaces
{
    // IVendorProductService interface
    public interface IVendorProductService
    {
        Task CreateVendorProductAsync(VendorProductDTO productDTO);
        Task UpdateVendorProductAsync(string productId, VendorProductDTO productDTO);
        Task<string> DeleteVendorProductAsync(string productId);
        Task<List<VendorProductDTO>> GetAllProductsAsync();
        Task<List<VendorProductDTO>> GetAllVendorProductsAsync(string vendorId);
        Task<VendorProductDTO> GetVendorProductByIdAsync(string productId);
        Task ManageVendorStockLevelsAsync(string productId, int quantityChange);
        Task NotifyVendorLowStockAsync(string productId);
        Task<VendorOrderDTO> GetOrderDetailsAsync(string orderId);
        Task<List<OrderItem>> GetAllAvailableOrdersAsync(string vendorId);
    }
}
