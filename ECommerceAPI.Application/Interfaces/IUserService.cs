using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Core.Entities;
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

        //Task<User> GetUserByEmailAsync(string email);
    }
}
