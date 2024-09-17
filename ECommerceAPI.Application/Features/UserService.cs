using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities;
using ECommerceAPI.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CheckUserExists(string email)
        {
            var users = await _userRepository.GetUserbyEmailAsync(email);
            if(users != null)
            {
                return true;
            }

            return false; 
        }

        public async Task<User> CreateUserAsync(User signupReqDTO)
        {
            //var user = new User
            //{
            //    password = signupReqDTO.Password,
            //    Email = signupReqDTO.Email,
            //    PasswordHash = signupReqDTO.HashedPassword
            //};

            await _userRepository.CreateUserAsync(signupReqDTO);
            return signupReqDTO;
        }
    }
}
