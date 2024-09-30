using ECommerceAPI.Application.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IVendorProductService
    {
        Task CreateVendorProductAsync(VendorProductDTO productDTO);
        Task UpdateVendorProductAsync(string productId, VendorProductDTO productDTO);
        Task DeleteVendorProductAsync(string productId);
        Task<List<VendorProductDTO>> GetAllVendorProductsAsync(string vendorId);
        Task ManageVendorStockLevelsAsync(string productId, int quantityChange);
        Task NotifyVendorLowStockAsync(string productId);
    }
}
