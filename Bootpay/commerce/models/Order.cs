using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 구독 결제 타입 상수
    /// </summary>
    public static class SubscriptionBillingType
    {
        public const int None = 0;
        public const int Each = 1;
        public const int Group = 2;
    }

    /// <summary>
    /// 선택된 상품 옵션
    /// </summary>
    public class CommerceChosenProductOption
    {
        [JsonProperty("chosen_product_option_id")]
        public string ChosenProductOptionId { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("product_option_id")]
        public string ProductOptionId { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("option_name")]
        public string OptionName { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("qty")]
        public int? Qty { get; set; }
    }

    /// <summary>
    /// Commerce 주문
    /// </summary>
    public class CommerceOrder
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("order_pre_id")]
        public string OrderPreId { get; set; }

        [JsonProperty("chosen_product_options")]
        public List<CommerceChosenProductOption> ChosenProductOptions { get; set; }

        [JsonProperty("parent_order_id")]
        public string ParentOrderId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("currency")]
        public int? Currency { get; set; }

        [JsonProperty("is_subscription")]
        public bool? IsSubscription { get; set; }

        [JsonProperty("is_leaf")]
        public bool? IsLeaf { get; set; }

        [JsonProperty("total_price")]
        public int? TotalPrice { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("discount_amount")]
        public int? DiscountAmount { get; set; }

        [JsonProperty("delivery_price")]
        public int? DeliveryPrice { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("receipt_id")]
        public string ReceiptId { get; set; }

        [JsonProperty("webhook_url")]
        public string WebhookUrl { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("cancelled_request_history")]
        public List<CommerceOrderCancellationRequestHistory> CancelledRequestHistory { get; set; }
    }

    /// <summary>
    /// 주문 취소 요청 이력
    /// </summary>
    public class CommerceOrderCancellationRequestHistory
    {
        [JsonProperty("order_cancellation_request_history_id")]
        public string OrderCancellationRequestHistoryId { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

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
    }

    /// <summary>
    /// 주문 목록 조회 파라미터
    /// </summary>
    public class OrderListParams : ListParams
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("status")]
        public List<int> Status { get; set; }

        [JsonProperty("payment_status")]
        public List<int> PaymentStatus { get; set; }

        [JsonProperty("cs_type")]
        public string CsType { get; set; }

        [JsonProperty("css_at")]
        public string CssAt { get; set; }

        [JsonProperty("cse_at")]
        public string CseAt { get; set; }

        [JsonProperty("subscription_billing_type")]
        public int? SubscriptionBillingType { get; set; }

        [JsonProperty("order_subscription_ids")]
        public List<string> OrderSubscriptionIds { get; set; }
    }
}
