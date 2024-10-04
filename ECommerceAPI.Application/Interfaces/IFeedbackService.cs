using ECommerceAPI.Application.DTOs.FeadbackDTO;
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
    }
}
