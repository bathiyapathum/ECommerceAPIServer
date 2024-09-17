using AutoMapper;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupReqDTO request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userService.CheckUserExists(request.Email);

            if (userExists)
                return BadRequest("User already exists");

            var user = _mapper.Map<User>(request);
            //var user = _mapper.Map<SignupReqDTO>(request);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var createdUser = await _userService.CreateUserAsync(user);

            if(createdUser != null) {
                return Ok("User created successfully");
            }

            return StatusCode(500, "Something Went wrong...");

        }
    }
}
