using ECommerceAPI.Application.Common;
using ECommerceAPI.Application.DTOs.FeadbackDTO;
using ECommerceAPI.Application.DTOs.UserDTO;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Core.Entities.ProductEntity;
using ECommerceAPI.Core.Entities.UserEntity;
using ECommerceAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceAPI.Application.Features
{
    public class FeedbackService : IFeedbackService
    {
        private readonly UserRepository _userRepository;
        private readonly VendorProductRepository _vendorProductRepository;
        private readonly FeedbackRepository _feedbackRepository;
        private readonly IValidations _validations;

        public FeedbackService(UserRepository userRepository, IValidations validations, VendorProductRepository vendorProductRepository, FeedbackRepository feedbackRepository)

        {
            _userRepository = userRepository;
            _validations = validations;
            _vendorProductRepository = vendorProductRepository;
            _feedbackRepository = feedbackRepository;
        }
        public async Task PlaceFeedBack(FeedbackDTO feadbackDTO)
        {
            if (await _validations.IsFeedBackAlreadyAdded(feadbackDTO))
            {
                throw new DataException("Feedback Already Added");
            }

            if (!await _validations.IsUserValid(feadbackDTO.CustomerId))
            {
                throw new DataException("Invalid User Id");
            }

            if (!await _validations.IsValidProductID(feadbackDTO.ProductId))
            {
                throw new DataException("Invalid Product Id");
            }

            if (!await _validations.IsValidOrderId(feadbackDTO.OrderId))
            {
                throw new DataException("Invalid Order Id");
            }
            if(!await _validations.IsItemandOrderIDTallywithUser(feadbackDTO))
            {
                throw new DataException("Item ID not matched with User ID or Order ID");
            }


            try
            {
                await _feedbackRepository.FirestoreDatabase.RunTransactionAsync(async transaction =>
                {
                    var feedback = new Feedback
                    {
                        //Id = feadbackDTO.Id,
                        CustomerId = feadbackDTO.CustomerId,
                        OrderId = feadbackDTO.OrderId,
                        ProductId = feadbackDTO.ProductId,
                        FeedbackMessage = feadbackDTO.FeedbackMessage,
                        Rating = feadbackDTO.Rating,
                        Date = feadbackDTO.Date
                    };

                    await _feedbackRepository.CreateFeedbackAsync(feedback, transaction);

                    FeedbackInfo feedbacktoProduct = new FeedbackInfo
                    {
                        CustomerId = feadbackDTO.CustomerId,
                        OrderId = feadbackDTO.OrderId,
                        FeedbackMessage = feadbackDTO.FeedbackMessage,
                        Rating = feadbackDTO.Rating,
                        Date = feadbackDTO.Date
                    };

                    _vendorProductRepository.UpdateVendorProduct(feedbacktoProduct, feadbackDTO.ProductId, transaction);

                    await _vendorProductRepository.UpdateVendorProductRating(feadbackDTO.ProductId, feadbackDTO.Rating, transaction);
                });

               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateFeedBack(string feadbackId, FeedbackDTO feadbackDTO)
        {
            if (!await _validations.IsFeedBackAvailable(feadbackId))
            {
                throw new DataException("Invalid User Id");
            }
        }

        public async Task<User> GetRatingForVendor(string vendorId)
        {
            try
            {
                return await _feedbackRepository.GetRatingForVendor(vendorId);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateFeedbackMessage(string feadbackId, FeedbackDTO feadbackDTO)
        {

        }
    }
}
