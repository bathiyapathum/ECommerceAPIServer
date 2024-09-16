using ECommerceAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(Guid id);
        Task CreateAsync(ProductDTO product);
        Task UpdateAsync(Guid id, ProductDTO product);
        Task DeleteAsync(Guid id);
    }
}
