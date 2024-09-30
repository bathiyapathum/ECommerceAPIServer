﻿using ECommerceAPI.Application.DTOs.ProductDTO;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/vendor/products")]
    public class VendorProductController : ControllerBase
    {
        private readonly IVendorProductService _productService;

        public VendorProductController(IVendorProductService productService)
        {
            _productService = productService;
        }

        // Create a new vendor product
        [HttpPost("create")]
        public async Task<IActionResult> CreateVendorProduct([FromBody] VendorProductDTO productDTO)
        {
            await _productService.CreateVendorProductAsync(productDTO);
            return CreatedAtAction(nameof(CreateVendorProduct), new { id = productDTO.ProductId }, productDTO);
        }

        // Update an existing vendor product
        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateVendorProduct(string productId, [FromBody] VendorProductDTO productDTO)
        {
            await _productService.UpdateVendorProductAsync(productId, productDTO);
            return NoContent();
        }

        // Delete a vendor product
        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteVendorProduct(string productId)
        {
            await _productService.DeleteVendorProductAsync(productId);
            return NoContent();
        }

        // Get all vendor products by vendorId
        [HttpGet("{vendorId}")]
        public async Task<IActionResult> GetVendorProducts(string vendorId)
        {
            var products = await _productService.GetAllVendorProductsAsync(vendorId);
            return Ok(products);
        }

        //getting all products across all vendors
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }
    }
}
