using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>입점사 지급 상태.</summary>
    public static class PayoutStatus
    {
        public const string Ready = "ready";
        public const string Held = "held";
        public const string Sending = "sending";
        public const string Requested = "requested";
        public const string Processing = "processing";
        public const string Succeeded = "succeeded";
        public const string Failed = "failed";
        public const string Cancelled = "cancelled";
        public const string Unknown = "unknown";
        public const string Void = "void";
    }

    /// <summary>입점사 지급 요청. SubSellerId · Amount · MerchantPayoutId 는 필수.</summary>
    public class PayoutCreateParams
    {
        [JsonProperty("sub_seller_id")] public string SubSellerId { get; set; }
        [JsonProperty("amount")] public long Amount { get; set; }
        [JsonProperty("merchant_payout_id")] public string MerchantPayoutId { get; set; }
        [JsonProperty("order_number")] public string OrderNumber { get; set; }
        /// <summary>지급 예정일(YYYY-MM-DD). 미지정시 오늘.</summary>
        [JsonProperty("payout_date")] public string PayoutDate { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    /// <summary>입점사 지급 목록 필터. 미지정 값은 쿼리에 싣지 않는다.</summary>
    public class PayoutListParams
    {
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("sub_seller_id")] public string SubSellerId { get; set; }
        /// <summary>지급 예정일 시작(YYYY-MM-DD, 포함).</summary>
        [JsonProperty("from")] public string From { get; set; }
        /// <summary>지급 예정일 종료(YYYY-MM-DD, 포함).</summary>
        [JsonProperty("to")] public string To { get; set; }
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    /// <summary>입점사 지급 항목. 등록·조회·목록 응답이 같은 형태다.</summary>
    public class CommercePayout
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("merchant_payout_id")] public string MerchantPayoutId { get; set; }
        [JsonProperty("sub_seller_id")] public string SubSellerId { get; set; }
        [JsonProperty("amount")] public long Amount { get; set; }
        /// <summary>ready · held · sending · requested · processing · succeeded · failed · cancelled · unknown · void</summary>
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("payout_date")] public string PayoutDate { get; set; }
        [JsonProperty("order_number")] public string OrderNumber { get; set; }
        [JsonProperty("last_code")] public string LastCode { get; set; }
        [JsonProperty("last_message")] public string LastMessage { get; set; }
        [JsonProperty("created_at")] public string CreatedAt { get; set; }
        [JsonProperty("updated_at")] public string UpdatedAt { get; set; }
    }
}
