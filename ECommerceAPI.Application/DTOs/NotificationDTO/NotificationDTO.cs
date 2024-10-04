using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.NotificationDTO
{
    public class NotificationDTO
    {
        public string NotifyId { get; set; }

        public string Message { get; set; }

        public string UserId { get; set; }

        public string Reason { get; set; }

        public bool IsRead { get; set; }

        public DateTime SentDate { get; set; }
    }
}
