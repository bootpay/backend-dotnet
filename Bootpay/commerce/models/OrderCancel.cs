using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 주문 취소 목록 조회 파라미터
    /// </summary>
    public class OrderCancelListParams
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }
    }

    /// <summary>
    /// 취소 상품
    /// </summary>
    public class CancelProduct
    {
        [JsonProperty("order_product_id")]
        public string OrderProductId { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("qty")]
        public int? Qty { get; set; }

        [JsonProperty("cancel_price")]
        public int? CancelPrice { get; set; }
    }

    /// <summary>
    /// 취소 정기구독 청구
    /// </summary>
    public class CancelOrderSubscriptionBill
    {
        [JsonProperty("order_subscription_bill_id")]
        public string OrderSubscriptionBillId { get; set; }

        [JsonProperty("cancel_price")]
        public int? CancelPrice { get; set; }
    }

    /// <summary>
    /// 취소 요청 파라미터
    /// </summary>
    public class RequestCancelParameter
    {
        [JsonProperty("cancel_products")]
        public List<CancelProduct> CancelProducts { get; set; }

        [JsonProperty("cancel_order_subscription_bills")]
        public List<CancelOrderSubscriptionBill> CancelOrderSubscriptionBills { get; set; }

        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }

        [JsonProperty("cancel_type")]
        public int? CancelType { get; set; }

        [JsonProperty("refund_price")]
        public int? RefundPrice { get; set; }
    }

    /// <summary>
    /// 주문 취소 파라미터
    /// </summary>
    public class OrderCancelParams
    {
        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("request_cancel_parameters")]
        public RequestCancelParameter RequestCancelParameters { get; set; }

        [JsonProperty("is_supervisor")]
        public bool? IsSupervisor { get; set; }
    }

    /// <summary>
    /// 주문 취소 액션 파라미터
    /// 대상 ID 는 URL 로만 전송된다. 정식 이름은 order_cancellation_request_id 이며,
    /// 구 이름 OrderCancelRequestHistoryId 도 계속 지원한다. 승인/반려는 supervisor scope.
    /// </summary>
    public class OrderCancelActionParams
    {
        [JsonIgnore]
        public string OrderCancellationRequestId { get; set; }

        /// <summary>
        /// deprecated — OrderCancellationRequestId 를 사용할 것 (하위호환으로 계속 지원한다)
        /// </summary>
        [JsonIgnore]
        public string OrderCancelRequestHistoryId { get; set; }

        /// <summary>
        /// 서버가 읽는 값은 message 다.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }

        [JsonProperty("refund_price")]
        public int? RefundPrice { get; set; }
    }

    /// <summary>
    /// Commerce 주문 취소 요청 이력
    /// </summary>
    public class CommerceOrderCancelRequestHistory
    {
        [JsonProperty("order_cancel_request_history_id")]
        public string OrderCancelRequestHistoryId { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }

        [JsonProperty("cancel_type")]
        public int? CancelType { get; set; }

        [JsonProperty("requested_at")]
        public string RequestedAt { get; set; }

        [JsonProperty("processed_at")]
        public string ProcessedAt { get; set; }

        [JsonProperty("refund_price")]
        public int? RefundPrice { get; set; }
    }
}
