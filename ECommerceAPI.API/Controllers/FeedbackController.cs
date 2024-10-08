using ECommerceAPI.Application.DTOs.FeadbackDTO;
using ECommerceAPI.Application.DTOs.OrderDTO;
using System;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.API.Controllers
{
    //[Authorize(Roles = "Customer")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FeedbackController :ControllerBase
    {
        private readonly IFeedbackService _feadbackService;

        public FeedbackController(IFeedbackService feadbackService)
        {
            _feadbackService = feadbackService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceFeadback([FromBody] FeedbackDTO feadbackDTO)
        {
            try
            {
                await _feadbackService.PlaceFeedBack(feadbackDTO);
                return CreatedAtAction(nameof(PlaceFeadback), new { id = feadbackDTO }, feadbackDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateFeadback([FromBody] FeedbackDTO feadbackDTO, [FromQuery] string feadbackId)
        {
            try
            {
                await _feadbackService.UpdateFeedBack(feadbackId, feadbackDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("vender-ranking/{vendorId}")]
        public async Task<IActionResult> GetRatingforVendor(string vendorId)
        {
            var result =  await _feadbackService.GetRatingForVendor(vendorId);

            if(result != null)
            {
                return Ok(new
                {
                    User = result,
                });
            }
            
            return NotFound();
        }

        [HttpGet("product/feedback/{productId}")]
        public async Task<IActionResult> GetFeedbackForProduct(string productId)
        {
            var result = await _feadbackService.GetFeedbackForProduct(productId);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpPatch("UpdateFeedbackMessage/{feadbackId}")]
        public async Task<IActionResult> UpdateFeedbackMessage([FromBody] FeedbackDTO feadbackDTO, string feadbackId)
        {
            try
            {
                await _feadbackService.UpdateFeedbackMessage(feadbackId, feadbackDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
