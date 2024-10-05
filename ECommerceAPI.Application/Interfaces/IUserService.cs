﻿using ECommerceAPI.Application.DTOs.UserDTO;
using ECommerceAPI.Core.Entities.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> CheckUserExists(string email);
        Task CreateUserAsync(SignupReqDTO user);
        Task CreateUserAsync(SignupReqDTO user, DTOs.UserDTO.UserRole role);
        Task<UserLogin> UserLoginAsync(LoginReqDTO userCred);
        Task<UserLogin> GetUserByIdAsync(string userId);
        Task<bool> ActivateUser(ChangePasswordReqDTO changePasswordReqDTO);
        Task<bool> ActivateUser(string userId); 
        Task<bool> DeactivateUser(string userId);
        Task<List<User>> GetInactiveUsers();
    }
}
