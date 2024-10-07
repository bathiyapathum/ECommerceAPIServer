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
        OrderCancel,
        OrderShipped,
        PaymentFailed,

        OrderCreated,
        OrderUpdated,
        OrderDeleted,
        OrderDelivered,
        OrderCancelled,
        OrderReturned,
        OrderRefunded,
        OrderPaymentFailed,
        OrderPaymentSucceeded,
        OrderPaymentPending,
        OrderPaymentCancelled,
        OrderPaymentRefunded,
        OrderPaymentCaptured,
        OrderPaymentAuthorized,
        OrderPaymentVoided,
        OrderPaymentFailedToCapture,
        OrderPaymentFailedToAuthorize,
        OrderPaymentFailedToVoid,
        OrderPaymentFailedToRefund,
        OrderPaymentFailedToCancel,
        OrderPaymentFailedToCaptureRefund,
        OrderPaymentFailedToAuthorizeRefund,
        OrderPaymentFailedToVoidRefund,
        OrderPaymentFailedToCancelRefund,
        OrderPaymentFailedToCaptureVoid,
        OrderPaymentFailedToAuthorizeVoid,
        OrderPaymentFailedToRefundVoid,
        OrderPaymentFailedToCancelVoid,
        OrderPaymentFailedToCaptureAuthorize,
        OrderPaymentFailedToCaptureVoidRefund,
        OrderPaymentFailedToCaptureVoidCancel,
        OrderPaymentFailedToCaptureVoidAuthorize,
        OrderPaymentFailedToCaptureVoidRefundAuthorize,
        OrderPaymentFailedToCaptureVoidRefundCancel,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancel,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCapture,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelVoid,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelRefund,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelAuthorize,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureRefund,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureAuthorize,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureVoid,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureAuthorizeRefund,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureVoidRefund,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureVoidAuthorize,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureVoidRefundAuthorize,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureVoidRefundCancel,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureVoidRefundAuthorizeCancel,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureVoidRefundAuthorizeCapture,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureVoidRefundAuthorizeVoid,
        OrderPaymentFailedToCaptureVoidRefundAuthorizeCancelCaptureVoidRef

    }
}
