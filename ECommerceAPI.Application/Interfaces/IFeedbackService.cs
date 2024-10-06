using ECommerceAPI.Application.DTOs.FeadbackDTO;
using ECommerceAPI.Core.Entities.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task PlaceFeedBack(FeedbackDTO feadbackDTO);
        Task<User> GetRatingForVendor(string vendorId);
        Task UpdateFeedBack(string feadbackId, FeedbackDTO feadbackDTO);
        Task UpdateFeedbackMessage(string feadbackId, FeedbackDTO feadbackDTO);
    }
}
