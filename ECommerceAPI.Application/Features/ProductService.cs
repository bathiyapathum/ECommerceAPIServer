using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities;
using ECommerceAPI.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDTO
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Stock = p.Stock
            }).ToList();
        }

        public async Task<ProductDTO> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock
            };
        }

        public async Task CreateAsync(ProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Price = productDTO.Price,
                Description = productDTO.Description,
                Stock = productDTO.Stock

            };
            await _productRepository.CreateAsync(product);
        }

        public async Task UpdateAsync(Guid id, ProductDTO productDTO)
        {
            var product = new Product
            {
                Id = id,
                Name = productDTO.Name,
                Price = productDTO.Price,
                Description = productDTO.Description,
                Stock = productDTO.Stock
            };

            await _productRepository.UpdateAsync(id, product);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
