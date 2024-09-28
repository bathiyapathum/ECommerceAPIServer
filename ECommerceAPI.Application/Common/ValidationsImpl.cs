using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Application.Features;

namespace ECommerceAPI.Application.Common
{
    public class ValidationsImpl : IValidations
    {
        public List<string> ValidateUserInputs(SignupReqDTO signupReqDTO)
        {
            var errors = new List<string>();

            // Check if the email is valid
            if (string.IsNullOrWhiteSpace(signupReqDTO.Email) || !IsValidEmail(signupReqDTO.Email))
            {
                errors.Add("Invalid email format.");
            }

            // Check if the password meets criteria (e.g., length)
            if (string.IsNullOrWhiteSpace(signupReqDTO.Password) || signupReqDTO.Password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters long.");
            }

            // Check if password and re-entered password match
            if (signupReqDTO.Password != signupReqDTO.Re_Password)
            {
                errors.Add("Passwords do not match.");
            }

            return errors;
        }
        public bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

    }
}
