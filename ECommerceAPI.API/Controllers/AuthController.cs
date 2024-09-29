using AutoMapper;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Features;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using System;

namespace ECommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IMapper mapper, IAuthService authService)
        {
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupReqDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest(ModelState);

                var userExists = await _userService.CheckUserExists(request.Email);

                if (userExists)
                    return BadRequest("User already exists");


                await _userService.CreateUserAsync(request);
                return CreatedAtAction(nameof(Signup), new { id = request }, request);
            }
            catch(DataException ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userService.CheckUserExists(request.Email);

            if (!userExists)
                return BadRequest("Invalid User Name or Email");

            try
            {
                var user = await _userService.UserLoginAsync(request);

                var token = _authService.GenerateJwtToken(user);
                return Ok(new 
                {
                    Token = token,
                    User = user 
                });

            }
            catch(DataException ex)
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
