using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities;
using System;
using System.Threading.Tasks;
using ECommerceAPI.Infrastructure.Repositories;


namespace ECommerceAPI.Application.Features
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateAsync(ProductDTO productDTO)
        {
            var products = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = productDTO.Name,
                Price = productDTO.Price,
                Description = productDTO.Description,
                Stock = productDTO.Stock

            };
            await _productRepository.CreateAsync(products);
        }

    }
}
