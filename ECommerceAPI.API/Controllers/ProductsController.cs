using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using ECommerceAPI.Application.DTOs;

namespace ECommerceAPI.API.Controllers
{
    [ApiController]
    [Route("hi/fdfr/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDTO productDTO)
        {
            await _productService.CreateAsync(productDTO);
            return CreatedAtAction(nameof(Create), new { id = productDTO }, productDTO); 
        }

        [HttpGet]
        public async Task<IActionResult> Creates([FromBody] ProductDTO productDTO)
        {
            await _productService.CreateAsync(productDTO);
            return CreatedAtAction(nameof(Create), new { id = productDTO }, productDTO); 
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var products = await _productService.GetAllAsync();
        //    return Ok(products);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var product = await _productService.GetByIdAsync(id);
        //    if(product == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(product);
        //}




        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(Guid id, [FromBody] ProductDTO productDTO)
        //{
        //    await _productService.UpdateAsync(id, productDTO);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    await _productService.DeleteAsync(id);
        //    return NoContent();
        //}
    }
}
