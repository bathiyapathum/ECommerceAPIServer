﻿using ECommerceAPI.Application.DTOs.ProductDTO;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities.ProductEntity;
using ECommerceAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features
{
    public class VendorProductService : IVendorProductService
    {
        private readonly VendorProductRepository _productRepository;

        public VendorProductService(VendorProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Create a new vendor product
        public async Task CreateVendorProductAsync(VendorProductDTO productDTO)
        {
            var product = new VendorProduct
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                StockQuantity = productDTO.StockQuantity,
                Category = productDTO.Category,
                VendorId = productDTO.VendorId,
                ImageUrl = productDTO.ImageUrl, // Added ImageUrl field
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                StockStatus = productDTO.StockQuantity == 0 ? VendorStockStatus.OutOfStock.ToString() :
                              productDTO.StockQuantity < 5 ? VendorStockStatus.LowStock.ToString() : VendorStockStatus.Available.ToString()
            };

            await _productRepository.AddVendorProductAsync(product);

            // The ProductId field will now be populated with the Firebase ID after adding the document
        }

        // Update an existing vendor product
        public async Task UpdateVendorProductAsync(string productId, VendorProductDTO productDTO)
        {
            var product = await _productRepository.GetVendorProductByIdAsync(productId);
            if (product != null)
            {
                product.Name = productDTO.Name;
                product.Description = productDTO.Description;
                product.Price = productDTO.Price;
                product.StockQuantity = productDTO.StockQuantity;
                product.Category = productDTO.Category;
                product.VendorId = productDTO.VendorId;
                product.ImageUrl = productDTO.ImageUrl; // Added ImageUrl field
                product.UpdatedAt = DateTime.UtcNow;

                // Automatically update stock status based on new quantity
                product.StockStatus = product.StockQuantity == 0 ? VendorStockStatus.OutOfStock.ToString() :
                                      product.StockQuantity < 5 ? VendorStockStatus.LowStock.ToString() : VendorStockStatus.Available.ToString();

                await _productRepository.UpdateVendorProductAsync(product);
            }
        }

        // Delete a vendor product if it is not in pending state
        public async Task DeleteVendorProductAsync(string productId)
        {
            var product = await _productRepository.GetVendorProductByIdAsync(productId);
            if (product != null)
            {
                if (product.StockStatus != VendorStockStatus.Pending.ToString())
                {
                    await _productRepository.DeleteVendorProductAsync(productId);
                }
            }
        }

        // Get all vendor products for a specific vendor
        public async Task<List<VendorProductDTO>> GetAllVendorProductsAsync(string vendorId)
        {
            var products = await _productRepository.GetVendorProductsByVendorAsync(vendorId);
            var productDTOs = new List<VendorProductDTO>();

            foreach (var product in products)
            {
                productDTOs.Add(new VendorProductDTO
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    Category = product.Category,
                    VendorId = product.VendorId,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                    StockStatus = product.StockStatus, // Add StockStatus to the DTO
                    ImageUrl = product.ImageUrl // Add ImageUrl to the DTO
                });
            }

            return productDTOs;
        }

        // Manage stock levels and notify vendor if stock is low
        public async Task ManageVendorStockLevelsAsync(string productId, int quantityChange)
        {
            var product = await _productRepository.GetVendorProductByIdAsync(productId);
            if (product != null)
            {
                product.StockQuantity += quantityChange;

                // Automatically update stock status based on new quantity
                product.StockStatus = product.StockQuantity == 0 ? VendorStockStatus.OutOfStock.ToString() :
                                      product.StockQuantity < 5 ? VendorStockStatus.LowStock.ToString() : VendorStockStatus.Available.ToString();

                if (product.StockQuantity < 5)
                {
                    await NotifyVendorLowStockAsync(productId);
                }

                await _productRepository.UpdateVendorProductAsync(product);
            }
        }

        // Notify vendor when stock is low
        public async Task NotifyVendorLowStockAsync(string productId)
        {
            // Simulate sending a notification
            Console.WriteLine($"Vendor Product {productId} is low on stock!");
        }
    }
}
