using System.Collections.Generic;
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

        /// <summary>
        /// 범위 지정 시작 회차 (조정항목 생성 전용).
        ///
        /// <para>회차 지정 방법 3가지 (아래로 갈수록 넓다).</para>
        /// <list type="bullet">
        ///   <item><description><c>Duration = 5</c> → 5회차 한 건만</description></item>
        ///   <item><description><c>DurationFrom = 3, DurationTo = 7</c> → 3~7회차 각각 한 건씩 (총 5건)</description></item>
        ///   <item><description><c>DurationFrom = 3, IsUnlimited = true</c> → 3회차부터 계약 끝까지 (레코드는 1건, <c>DurationTo</c> 는 무시)</description></item>
        /// </list>
        /// <para>상한은 계약 총회차이며, 총회차가 무제한인 계약은 60회차까지다.
        /// 이미 결제가 끝난 회차는 거절된다. 범위 중 한 회차라도 최종 금액이 음수면 전부 거절된다 (부분 반영 없음).</para>
        /// </summary>
        [JsonProperty("duration_from")]
        public int? DurationFrom { get; set; }

        /// <summary>
        /// 범위 지정 종료 회차 (조정항목 생성 전용, <c>IsUnlimited = true</c> 이면 무시된다)
        /// </summary>
        [JsonProperty("duration_to")]
        public int? DurationTo { get; set; }

        /// <summary>
        /// <c>DurationFrom</c> 회차부터 계약 끝까지 적용한다 (조정항목 생성 전용).
        /// 명시적 <c>false</c> 도 그대로 전송된다.
        /// </summary>
        [JsonProperty("is_unlimited")]
        public bool? IsUnlimited { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }

    /// <summary>
    /// 정기구독 조정 수정 파라미터
    /// 서버는 duration(회차) 단위로 adjustments 배열을 통째로 교체한다. duration 미지정시 1 이 적용된다. supervisor scope.
    /// </summary>
    public class OrderSubscriptionAdjustmentUpdateParams
    {
        /// <summary>
        /// order_subscription_id 는 URL 로만 전송된다.
        /// </summary>
        [JsonIgnore]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("adjustments")]
        public List<CommerceOrderSubscriptionAdjustment> Adjustments { get; set; }

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
