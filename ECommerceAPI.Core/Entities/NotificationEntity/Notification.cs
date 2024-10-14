using ECommerceAPI.Core.Enums;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Core.Entities.NotificationEntity
{
    [FirestoreData]
    public class Notification
    {
        [FirestoreProperty("notifyId")]
        public string NotifyId { get; set; }

        [FirestoreProperty("userId")]
        public string UserId { get; set; }

        [FirestoreProperty("message")]
        public string Message { get; set; }

        [FirestoreProperty("isRead")]
        public bool IsRead { get; set; }        

        [FirestoreProperty("readBy")]
        public string ReadBy { get; set; }

        [FirestoreProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [FirestoreProperty("rolesToNotify")]
        public List<UserRole> RolesToNotify { get; set; } 

        [FirestoreProperty("scenario")]
        public NotificationScenario Scenario { get; set; }
        
        [FirestoreProperty("scenarioId")]
        public string ScenarioId { get; set; }
    }
}
