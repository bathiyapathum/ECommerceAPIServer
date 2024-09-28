using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities;
using ECommerceAPI.Infrastructure.Repositories;
using FirebaseAdmin.Auth.Hash;
using Google.Type;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Google.Rpc.Context.AttributeContext.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace ECommerceAPI.Application.Features
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IValidations _validations;

        public UserService(UserRepository userRepository, IValidations validations)
        {
            _userRepository = userRepository;
            _validations = validations;
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

        public async Task CreateUserAsync(SignupReqDTO signupReqDTO)
        {
            List<string>  errors =_validations.ValidateUserInputs(signupReqDTO);

            if(errors.Count > 0)
            {
                throw new DataException(string.Join(",", errors));
            }



            var user = new User
            {
                Password = signupReqDTO.Password,
                Email = signupReqDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(signupReqDTO.Password),
                Re_PasswordHash = BCrypt.Net.BCrypt.HashPassword(signupReqDTO.Re_Password),
                FirstName = signupReqDTO.FirstName,
                LastName = signupReqDTO.LastName,
                Role = (Core.Entities.UserRole)signupReqDTO.Role,
                CreatedDate = signupReqDTO.CreatedDate,
                IsActive = signupReqDTO.IsActive,
                ProfilePicture = signupReqDTO.ProfilePicture,
                Addresss = new Core.Entities.Address
                {
                    Street = signupReqDTO.Addresss.Street,
                    City = signupReqDTO.Addresss.City,
                    State = signupReqDTO.Addresss.State,
                    Country = signupReqDTO.Addresss.Country,
                    ZipCode = signupReqDTO.Addresss.ZipCode

                },
                PhoneNumber = signupReqDTO.PhoneNumber

            };

            await _userRepository.CreateUserAsync(user);
        }


    }
}
