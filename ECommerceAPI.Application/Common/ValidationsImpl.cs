using ECommerceAPI.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Application.Features;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ECommerceAPI.Application.DTOs.UserDTO;
using ECommerceAPI.Infrastructure.Repositories;
using ECommerceAPI.Application.DTOs.FeadbackDTO;

namespace ECommerceAPI.Application.Common
{
    public class ValidationsImpl : IValidations
    {
        private readonly UserRepository userRepository;
        private readonly VendorProductRepository productRe;
        private readonly OrderRepository orderRe;

        public ValidationsImpl(UserRepository userRepository, VendorProductRepository productRe, OrderRepository orderRe)
        {
            this.userRepository = userRepository;
            this.productRe = productRe;
            this.orderRe = orderRe;
        }

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

        public async Task<bool> IsUserValid(string userID)
        {
            var isAvailable = await userRepository.GetUserByIdAsync(userID);

            if (isAvailable != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsValidProductID(string productID)
        {
            var isAvailable = await productRe.GetVendorProductByIdAsync(productID);

            if (isAvailable != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsValidOrderId(string orderID)
        {
            var isAvailable = await orderRe.GetOrderAsync(orderID);

            if (isAvailable != null)
            {
                return true;
            }

            return false;
        }

        public List<string> ValidateUserRole(SignupReqDTO signupReqDTO, UserRole role)
        {
            var errors = new List<string>();

            if (role != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("Only admin can create vendor accounts.");
            }

            if (signupReqDTO.Role != UserRole.Vendor && signupReqDTO.Role != UserRole.CSR)
            {
                throw new UnauthorizedAccessException("Invalid User Type.");
            }

            return errors;
        }

        public async Task<bool> IsItemandOrderIDTallywithUser(FeedbackDTO feedbackDTO)
        {
            var errors = new List<string>();

            var orderDetails = await orderRe.GetOrderAsync(feedbackDTO.OrderId);

            if (orderDetails.CustomerId != feedbackDTO.CustomerId)
            {
                errors.Add("Customer Id does not match with the order.");
                return false;
            }

            bool productIdMatchFound = orderDetails.Items.Any(item => item.ProductId == feedbackDTO.ProductId);

            if (!productIdMatchFound) 
                return false;

            //orderDetails.Items.ForEach(item => {   
            //    if (item.ProductId != feedbackDTO.ProductId)
            //    {
            //        errors.Add("Product Id does not match with the order.");
                    
            //    }
            //});

            return true;

        }

        public bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

    }
}
