using ECommerceAPI.Core.Enums;
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

        public bool IsRead { get; set; }

        public string ReadBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<UserRole> RolesToNotify { get; set; }

        public NotificationScenario Scenario { get; set; }

        public string ScenarioId { get; set; }

    }
}
