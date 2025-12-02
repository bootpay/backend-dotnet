using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 구독 조정 타입 상수
    /// </summary>
    public static class SubscriptionAdjustmentType
    {
        public const int PeriodDiscount = 1;
    }

    /// <summary>
    /// Commerce 정기구독 조정
    /// </summary>
    public class CommerceOrderSubscriptionAdjustment
    {
        [JsonProperty("order_subscription_adjustment_id")]
        public string OrderSubscriptionAdjustmentId { get; set; }

        [JsonProperty("duration")]
        public int? Duration { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }

    /// <summary>
    /// 정기구독 조정 수정 파라미터
    /// </summary>
    public class OrderSubscriptionAdjustmentUpdateParams
    {
        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("order_subscription_adjustment_id")]
        public string OrderSubscriptionAdjustmentId { get; set; }

        [JsonProperty("duration")]
        public int? Duration { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }
    }
}
