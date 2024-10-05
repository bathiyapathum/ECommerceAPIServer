using ECommerceAPI.Core.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.OrderDTO
{
    public class CancelRequestDTO
    {
        public string RequestId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string RequestNote { get; set; }
        public string ResponseNote { get; set; }
        public string ResponsedBy { get; set; }
        public string CustomerId { get; set; }       

    }
}
