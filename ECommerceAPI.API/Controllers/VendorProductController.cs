using ECommerceAPI.Application.DTOs.ProductDTO;
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
            var result = await _productService.DeleteVendorProductAsync(productId);
            if(result == "Success")
            {
                return Ok("Product deleted successfully.");
            }
            return BadRequest($"Something went wrong DeleteVendorProduct: {result}");
        }

        // Get all vendor products by vendorId
        [HttpGet("{vendorId}")]
        public async Task<IActionResult> GetVendorProducts(string vendorId)
        {
            var products = await _productService.GetAllVendorProductsAsync(vendorId);
            return Ok(products);
        }

        // Get a specific vendor product by productId
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetVendorProductById(string productId)
        {
            var product = await _productService.GetVendorProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }

        [HttpGet("my/order")]
        public async Task<IActionResult> GetVendorOrderByItemId([FromQuery] string itemId)
        {
            var product = await _productService.GetOrderDetailsAsync(itemId);
            if (product == null)
            {
                return NotFound("Vendor Order not found");
            }
            return Ok(product);
        }

        [HttpGet("my/order/all")]
        public async Task<IActionResult> GetAllVendorOrderItems([FromQuery] string vendorId)
        {
            var products = await _productService.GetAllAvailableOrdersAsync(vendorId);

            if (products == null || products.Count == 0)
            {
                return NotFound("No Vendor order found.");
            }
            return Ok(products);
        }

        // Get all products across all vendors
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
