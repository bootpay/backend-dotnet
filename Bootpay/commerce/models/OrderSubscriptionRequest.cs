using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 정기구독 요청
    /// </summary>
    public class OrderSubscriptionRequest
    {
        [JsonProperty("order_subscription_request_history_id")]
        public string OrderSubscriptionRequestHistoryId { get; set; }

        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("request_type")]
        public int? RequestType { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("requested_at")]
        public string RequestedAt { get; set; }

        [JsonProperty("processed_at")]
        public string ProcessedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// 정기구독 요청 목록 조회 파라미터
    /// </summary>
    public class OrderSubscriptionRequestListParams
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("page")]
        public int? Page { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("request_type")]
        public int? RequestType { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("s_at")]
        public string SAt { get; set; }

        [JsonProperty("e_at")]
        public string EAt { get; set; }

        [JsonProperty("keyword")]
        public string Keyword { get; set; }
    }

    /// <summary>
    /// 정기구독 요청 승인/거절 파라미터
    /// </summary>
    public class OrderSubscriptionRequestUpdateParams
    {
        [JsonIgnore]
        public string OrderSubscriptionRequestHistoryId { get; set; }

        /// <summary>
        /// "approve" 또는 "reject"
        /// </summary>
        [JsonProperty("approval")]
        public string Approval { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("termination_fee")]
        public int? TerminationFee { get; set; }

        [JsonProperty("last_bill_refund_price")]
        public int? LastBillRefundPrice { get; set; }

        [JsonProperty("final_fee")]
        public int? FinalFee { get; set; }

        [JsonProperty("service_end_at")]
        public string ServiceEndAt { get; set; }
    }
}
