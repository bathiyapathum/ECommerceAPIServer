/******************************************************************************************
 * File Name: DashboardController.cs
 * Description: This controller handles dashboard-related API requests, providing services 
 *              such as fetching available user and product counts, order statistics, and 
 *              weekly order behavior (including order counts, cancellation requests, 
 *              and responses).
 *
 * Author: Herath R. P. N. M
 * Registration No: IT21177828
 * Date: 2024/08/10
 *
 * API Endpoints:
 *  - GetTotalRevenue: Fetches the total revenue from all completed orders.
 *  - GetOrderStats: Provides detailed statistics about the orders.
 *  - GetUserCounts: Retrieves the count of currently available users.
 *  - GetAvailableProductCounts: Retrieves the count of available products.
 *  - GetWeeklyStats: Provides the weekly statistics on orders, including cancellations 
 *                    and responses.
 *
 * Error Handling:
 *  - Returns 404 if no data is found for user or product counts.
 *  - Returns 400 for validation errors and 500 for any other server-related errors.
 *
 ******************************************************************************************/

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

        //Get the total revenue from all completed orders
        [HttpGet("total/revanue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var orders = await _dashboardService.GetTotalRevenue();
            return Ok(orders);
        }

        //Get the order statistics
        [HttpGet("order/stats")]
        public async Task<IActionResult> GetOrderStats()
        {
            var orders = await _dashboardService.GetOrderStats();
            return Ok(orders);
        }

        //Get the available user counts
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

        //Get the available product counts
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

        //Get the weekly statistics on orders
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
