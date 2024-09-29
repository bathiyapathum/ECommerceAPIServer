using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities;
using ECommerceAPI.Infrastructure.Repositories;
using System;
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
            try
            {
                var users = await _userRepository.GetUserbyEmailAsync(email);
                if (users != null)
                {
                    return true;
                }
                return false;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }   


        }

        public async Task CreateUserAsync(SignupReqDTO signupReqDTO)
        {
            try
            {
                List<string> errors = _validations.ValidateUserInputs(signupReqDTO);

                if (errors.Count > 0)
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
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }

            
        }

        public Task<UserLogin> UserLoginAsync(LoginReqDTO userCred)
        {
            try
            {
                User user = _userRepository.GetUserbyEmailAsync(userCred.Email).Result;

                if (user == null)
                {
                    throw new DataException("Invalid email or password");
                }

                if (!BCrypt.Net.BCrypt.Verify(userCred.Password, user.PasswordHash))
                {
                    throw new DataException("Invalid  password");
                }

                UserLogin a = new UserLogin
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = (UserRoleLogin)user.Role,
                    CreatedDate = user.CreatedDate,
                    IsActive = user.IsActive,
                    ProfilePicture = user.ProfilePicture,
                    Addresss = new AddressLogin
                    {
                        Street = user.Addresss.Street,
                        City = user.Addresss.City,
                        State = user.Addresss.State,
                        Country = user.Addresss.Country,
                        ZipCode = user.Addresss.ZipCode

                    },
                    PhoneNumber = user.PhoneNumber
                };

                return Task.FromResult(a);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }   
           

        }


    }
}
