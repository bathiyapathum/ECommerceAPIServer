using AutoMapper;
using ECommerceAPI.Application.Features;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using ECommerceAPI.Application.DTOs.UserDTO;
using FirebaseAdmin.Messaging;

namespace ECommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IMapper mapper, IAuthService authService)
        {
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }


        /***
         * GetRequestCancelationByOrderForResponseAsync for orders cancellation requests
         * Author : Gunaratne M H B P T - IT21180552
         ***/
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


        /***
         * Register new user by admin
         * Author : Gunaratne M H B P T - IT21180552
         ***/
        [Authorize(Roles = "Admin")]
        [HttpPost("create-by-admin")]
        public async Task<IActionResult> RegisterNewUser([FromBody] SignupReqDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userExists = await _userService.CheckUserExists(request.Email);

                if (userExists)
                    return StatusCode(405,"User already exists");

                var loggedInUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(loggedInUserRole))
                {
                    return Unauthorized("User role is not defined.");
                }

                if (!Enum.TryParse(loggedInUserRole, out Application.DTOs.UserDTO.UserRole userRole))
                {
                    return BadRequest("Invalid user role.");
                }


                await _userService.CreateUserAsync(request, userRole);
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

        /***
         * Login user by admin
         * Author : Gunaratne M H B P T - IT21180552
         ***/
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

                if(user.Role.ToString() == "Vendor" || user.Role.ToString() == "CSR")
                {
                    var token = _authService.GenerateJwtToken(user);
                    return Ok(new
                    {
                        Token = token,
                        User = user
                    });
                }
                else
                {
                    if (user.IsActive)
                    {
                        var token = _authService.GenerateJwtToken(user);
                        return Ok(new
                        {
                            Token = token,
                            User = user
                        });
                    }
                    else
                    {
                        return StatusCode(403, new
                        {
                            Message = "Account not activated. Please activate your account to proceed.",
                            UserId = user.Id,
                        });
                    }
                }



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


        /***
         * Activate user by admin/CSR
         * Author : Gunaratne M H B P T - IT21180552
         ***/
        [Authorize(Policy = "AccountActivatePolicy")]
        [HttpPost("activate-crv-vendor")]
        public async Task<IActionResult> ActivateUser([FromBody] ChangePasswordReqDTO changePasswordReqDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var loggedInUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(loggedInUserRole))
                {
                    return Unauthorized("User role is not defined.");
                }

                await _userService.ActivateUser(changePasswordReqDTO);
                return Ok("User activated successfully");
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

        /***
         * activate customer account by admin/CSR
         * Author : Gunaratne M H B P T - IT21180552
         ***/
        [Authorize(Roles = "Admin,CSR")]
        [HttpPatch("activate-customer/{customerID}")]
        public async Task<IActionResult> ActivateCustomer(string customerID)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var loggedInUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(loggedInUserRole))
                {
                    return Unauthorized("User role is not defined.");
                }

                await _userService.ActivateUser(customerID);
                return Ok("User activated successfully");
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

        /***
         * Deactivate  user by admin
         * Author : Gunaratne M H B P T - IT21180552
         ***/
        [Authorize(Roles = "Admin,Customer,CSR")]
        [HttpPatch("deactivate-user/{customerID}")]
        public async Task<IActionResult> DeactivateUser(string customerID)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var loggedInUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(loggedInUserRole))
                {
                    return Unauthorized("User role is not defined.");
                }

                await _userService.DeactivateUser(customerID);
                return Ok("User deactivated successfully");
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

        /***
         * Get inactive user by admin
         * Author : Gunaratne M H B P T - IT21180552
         ***/
        [HttpGet("get-Inactive-users")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            try
            {
                var users = await _userService.GetInactiveUsers();
                return Ok(users);
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

        /***
         * Update  users 
         * Author : Gunaratne M H B P T - IT21180552
         ***/
        [HttpPatch("update-user/{userID}")]
        public async Task<IActionResult> UpdateUser(string userID, [FromBody] UpdateUserReqDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var loggedInUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(loggedInUserRole))
                {
                    return Unauthorized("User role is not defined.");
                }

                await _userService.UpdateUserAsync(request, userID);
                return Ok("User updated successfully");
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
