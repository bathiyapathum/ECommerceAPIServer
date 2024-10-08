using ECommerceAPI.Application.DTOs.OrderDTO;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ECommerceAPI.Application.Features;
using System.Data;
using System.Collections.Generic;

namespace ECommerceAPI.API.Controllers
{
    
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DashboardController(IDashboardService dashboardService) : ControllerBase
    {
        private readonly IDashboardService _dashboardService = dashboardService;

        [HttpGet("total/revanue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var orders = await _dashboardService.GetTotalRevenue();
            return Ok(orders);
        }

        [HttpGet("order/stats")]
        public async Task<IActionResult> GetOrderStats()
        {
            var orders = await _dashboardService.GetOrderStats();
            return Ok(orders);
        }

        [HttpGet("available/user/count")]
        public async Task<IActionResult> GetUserCounts()
        {
            try
            {
                var result = await _dashboardService.GetAvailableUserCount();
                if (result == null)
                {
                    return NotFound("No user found");
                }

                return Ok(result);
            }
            catch (DataException ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("available/product/count")]
        public async Task<IActionResult> GetAvailableProductCounts()
        {
            try
            {
                var result = await _dashboardService.GetAvailableProductCounts();
                if (result == null)
                {
                    return NotFound("No product found");
                }

                return Ok(result);
            }
            catch (DataException ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("order/behaviour")]
        public async Task<IActionResult> GetWeeklyStats()
        {
            try
            {
                var result = await _dashboardService.GetWeeklyStats();
                if (result == null)
                {
                    return NotFound("Something went wrong while gettin stats");
                }

                return Ok(result);
            }
            catch (DataException ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
