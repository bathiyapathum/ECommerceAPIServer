using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.FeadbackDTO
{
    public class FeedbackDTO
    {
        //public string Id { get; set; }
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string FeedbackMessage { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } 
        public DateTime Date { get; set; }
    }
}
