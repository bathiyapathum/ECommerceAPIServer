using ECommerceAPI.Application.DTOs.FeadbackDTO;
using ECommerceAPI.Application.DTOs.OrderDTO;
using System;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.API.Controllers
{
    [Authorize(Roles = "Customer")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FeedbackControler :ControllerBase
    {
        private readonly IFeedbackService _feadbackService;

        public FeedbackControler(IFeedbackService feadbackService)
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

    }
}
