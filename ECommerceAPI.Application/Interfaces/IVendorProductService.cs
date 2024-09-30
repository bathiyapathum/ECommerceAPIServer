using System.Threading.Tasks;
using System.Collections.Generic;
using ECommerceAPI.Application.DTOs.ProductDTO;

namespace ECommerceAPI.Application.Interfaces
{
    // IVendorProductService interface
    public interface IVendorProductService
    {
        Task CreateVendorProductAsync(VendorProductDTO productDTO);
        Task UpdateVendorProductAsync(string productId, VendorProductDTO productDTO);
        Task DeleteVendorProductAsync(string productId);
        Task<List<VendorProductDTO>> GetAllProductsAsync();
        Task<List<VendorProductDTO>> GetAllVendorProductsAsync(string vendorId);
        Task ManageVendorStockLevelsAsync(string productId, int quantityChange);
        Task NotifyVendorLowStockAsync(string productId);
    }
}
