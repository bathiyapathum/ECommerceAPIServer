using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Core.Enums
{
    public enum NotificationScenario
    {
        StockLow,
        OrderPlaced,
        OrderCancelled,
        OrderCancelRequest,
        OrderDelivered,
        PaymentFailed,
        ProductActivated,
        ProductDeactivated,
    }
}
