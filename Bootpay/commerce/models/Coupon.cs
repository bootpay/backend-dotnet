using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// Commerce 쿠폰
    /// </summary>
    public class CommerceCoupon
    {
        [JsonProperty("coupon_id")]
        public string CouponId { get; set; }

        [JsonProperty("coupon_template_id")]
        public string CouponTemplateId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("discount_type")]
        public int? DiscountType { get; set; }

        [JsonProperty("discount_value")]
        public int? DiscountValue { get; set; }

        [JsonProperty("min_order_amount")]
        public int? MinOrderAmount { get; set; }

        [JsonProperty("max_discount_amount")]
        public int? MaxDiscountAmount { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("issued_at")]
        public string IssuedAt { get; set; }

        [JsonProperty("used_at")]
        public string UsedAt { get; set; }

        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }

    /// <summary>
    /// 쿠폰 목록 조회 파라미터
    /// </summary>
    public class CouponListParams
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("page")]
        public int? Page { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

    /// <summary>
    /// 쿠폰 다운로드 파라미터
    /// </summary>
    public class CouponDownloadParams
    {
        [JsonProperty("coupon_template_id")]
        public string CouponTemplateId { get; set; }
    }
}
