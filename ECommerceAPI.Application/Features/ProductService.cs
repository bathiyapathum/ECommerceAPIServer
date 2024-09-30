using ECommerceAPI.Application.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerceAPI.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Data;
using ECommerceAPI.Application.DTOs.ProductDTO;
using ECommerceAPI.Core.Entities.ProductEntity;


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
            try
            {
                var products = new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    Description = productDTO.Description,
                    StockQuantity = productDTO.StockQuantity,
                    ImageUrl = productDTO.ImageUrl,
                    CreatedAt = productDTO.CreatedAt,
                    UpdatedAt = productDTO.UpdatedAt,
                    CategoryId = productDTO.CategoryId
                };
                await _productRepository.CreateAsync(products);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<ProductDTO>> GetAllAsync()
        {
            try
            {
                List<Product> products = await _productRepository.GetAllAsync();

                if(products == null)
                {
                    throw new DataException("Invalid email or password");
                }

                List<ProductDTO> productsDTO = products.Select(products => new ProductDTO
                {
                    Name = products.Name,
                    Price = products.Price,
                    Description = products.Description,
                    StockQuantity = products.StockQuantity,
                    ImageUrl = products.ImageUrl,
                    CreatedAt = products.CreatedAt,
                    UpdatedAt = products.UpdatedAt,
                    CategoryId = products.CategoryId
                }).ToList();

                return productsDTO;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

    }
}
