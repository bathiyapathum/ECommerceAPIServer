// --------------------------------------------------------------------------------------------------------------------
// VendorProductService: Implements business logic for managing vendor products and orders.
// This service handles product creation, updating, deletion, stock management, and notifications.
// Author: Arachchi D.S.U - IT21182914
// Date: 06/10/2024
// --------------------------------------------------------------------------------------------------------------------

using ECommerceAPI.Application.DTOs.NotificationDTO;
using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.DTOs.ProductDTO;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Application.Interfaces.NotificationInterfaces;
using ECommerceAPI.Core.Entities.OrderEntity;
using ECommerceAPI.Core.Entities.ProductEntity;
using ECommerceAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace ECommerceAPI.Application.Features
{
    public class VendorProductService : IVendorProductService
    {
        private readonly VendorProductRepository _productRepository;
        private readonly OrderRepository _orderRepository;
        private readonly NotificationRepository _notificationRepository;


        public VendorProductService(VendorProductRepository productRepository, OrderRepository orderRepository, NotificationRepository notificationRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _notificationRepository= notificationRepository;
        }

        // Create a new vendor product
        public async Task CreateVendorProductAsync(VendorProductDTO productDTO)
        {
            // Ensure default values if not provided
            string productType = string.IsNullOrEmpty(productDTO.Type) ? "Anyone" : productDTO.Type;
            string productSize = string.IsNullOrEmpty(productDTO.Size) ? "Default" : productDTO.Size;

            var product = new VendorProduct
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                StockQuantity = productDTO.StockQuantity,
                Category = productDTO.Category,
                VendorId = productDTO.VendorId,
                ImageUrl = productDTO.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                StockStatus = productDTO.StockQuantity == 0 ? VendorStockStatus.OutOfStock.ToString() :
                              productDTO.StockQuantity < 5 ? VendorStockStatus.LowStock.ToString() : VendorStockStatus.Available.ToString(),
                Type = productType, // Set default or provided value
                Size = productSize, // Set default or provided value
                IsActive = false // Initially false
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
                // Ensure vendorId is retained and not overwritten
                product.VendorId = product.VendorId ?? productDTO.VendorId;

                product.ProductId = productId;
                product.Name = productDTO.Name;
                product.Description = productDTO.Description;
                product.Price = productDTO.Price;
                product.StockQuantity = productDTO.StockQuantity;
                product.Category = productDTO.Category;
                product.ImageUrl = productDTO.ImageUrl;
                product.Type = string.IsNullOrEmpty(productDTO.Type) ? product.Type : productDTO.Type;
                product.Size = string.IsNullOrEmpty(productDTO.Size) ? product.Size : productDTO.Size;
                product.UpdatedAt = DateTime.UtcNow;

                // Automatically update stock status based on new quantity
                product.StockStatus = product.StockQuantity == 0 ? VendorStockStatus.OutOfStock.ToString() :
                                      product.StockQuantity < 5 ? VendorStockStatus.LowStock.ToString() : VendorStockStatus.Available.ToString();

                await _productRepository.UpdateVendorProductAsync(product);
            }
        }

        // Delete a vendor product if it is not in pending state
        public async Task<string> DeleteVendorProductAsync(string productId)
        {
            try
            {
                var product = await _productRepository.GetVendorProductByIdAsync(productId);
                if (product == null)
                {
                    return "Product is not found";
                }

                var orderItems = await _orderRepository.GetVendorOrderAsync(product.VendorId);

                if (orderItems == null || orderItems.Count == 0)
                {
                    await _productRepository.DeleteVendorProductAsync(productId);
                    return "Success";
                }

                foreach (var orderItem in orderItems)
                {
                    if (orderItem.ProductId == productId && orderItem.Status == "PENDING")
                    {
                        return "Can't Delete, Product is Pending order Item";
                    }
                }

                await _productRepository.DeleteVendorProductAsync(productId);
                return "Success";
            }
            catch (Exception ex)
            {
                return ("Something went wrong DeleteVendorProduct: " + ex.Message);
            }
        }

        // Retrieve all products from all vendors
        public async Task<List<VendorProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
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
                    StockStatus = product.StockStatus,
                    ImageUrl = product.ImageUrl,
                    Type = product.Type, // Include Type in the DTO
                    Size = product.Size, // Include Size in the DTO
                    IsActive = product.IsActive // Include IsActive in the DTO
                });
            }

            return productDTOs;
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
                    StockStatus = product.StockStatus,
                    ImageUrl = product.ImageUrl,
                    Type = product.Type, // Include Type in the DTO
                    Size = product.Size, // Include Size in the DTO
                    IsActive = product.IsActive // Include IsActive in the DTO
                });
            }

            return productDTOs;
        }

        // Get a specific vendor product by productId
        public async Task<VendorProductDTO> GetVendorProductByIdAsync(string productId)
        {
            var product = await _productRepository.GetVendorProductByIdAsync(productId);
            if (product == null) return null;

            return new VendorProductDTO
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
                StockStatus = product.StockStatus,
                ImageUrl = product.ImageUrl,
                Type = product.Type, // Include Type in the DTO
                Size = product.Size, // Include Size in the DTO
                IsActive = product.IsActive // Include IsActive in the DTO
            };
        }

        // Toggle product activation
        public async Task<bool> ToggleProductActivationAsync(string productId)
        {
            NotificationService notificationService = new(_notificationRepository);
            var product = await _productRepository.GetVendorProductByIdAsync(productId);
            if (product != null)
            {
                // Toggle the isActive value
                product.IsActive = !product.IsActive;
                await _productRepository.UpdateVendorProductAsync(product);

                if (product.IsActive) {
                
                    NotificationDTO notification = new()
                    {
                        NotifyId = Guid.NewGuid().ToString(),
                        IsRead = false,
                        Message = $"Your product {product.Name} has been activated!",
                        UserId = product.VendorId,
                        CreatedDate = DateTime.UtcNow,
                        ReadBy = null,
                        RolesToNotify = null,
                        Scenario = Core.Enums.NotificationScenario.ProductActivated,
                        ScenarioId = product.ProductId

                    };
                    await notificationService.Send(notification);
                }
                else
                {
                    NotificationDTO notification = new()
                    {
                        NotifyId = Guid.NewGuid().ToString(),
                        IsRead = false,
                        Message = $"Your product {product.Name} has been deactivated!",
                        UserId = product.VendorId,
                        CreatedDate = DateTime.UtcNow,
                        ReadBy = null,
                        RolesToNotify = null,
                        Scenario = Core.Enums.NotificationScenario.ProductDeactivated,
                        ScenarioId = product.ProductId

                    };
                    await notificationService.Send(notification);
                }

                return true;
            }

            return false;
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

        // Get full details for specific order
        public async Task<VendorOrderDTO> GetOrderDetailsAsync(string ItemId)
        {
            try
            {
                var orderItem = await _orderRepository.GetVendorOrderItemsAsync(ItemId);

                if (orderItem == null)
                {
                    throw new Exception("Order Item is null or not found");
                }

                var order = await _orderRepository.GetOrderbyIdAsync(orderItem.OrderId);
                VendorOrderDTO createdItem = VendorOrderDTO.OrderMapper(order, orderItem);
                return createdItem;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Get all active orders for vendor
        public async Task<List<VendorOrderDTO>> GetAllAvailableOrdersAsync(string vendorId)
        {
            try
            {
                var result = await _orderRepository.GetVendorOrderAsync(vendorId);
                var orderItems = new List<VendorOrderDTO>();

                foreach (var order in result)
                {
                    var orderDetails = await GetOrderDetailsAsync(order.ItemId);
                    if (orderDetails != null)
                    {
                        orderItems.Add(orderDetails);
                    }
                }

                if (orderItems == null || orderItems.Count == 0)
                {
                    return null;
                }

                return orderItems;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
