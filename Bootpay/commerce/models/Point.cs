using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 적립금 잔액
    /// </summary>
    public class PointBalance
    {
        [JsonProperty("available_balance")]
        public int? AvailableBalance { get; set; }

        [JsonProperty("total_earned")]
        public int? TotalEarned { get; set; }

        [JsonProperty("total_used")]
        public int? TotalUsed { get; set; }

        [JsonProperty("is_negative")]
        public bool? IsNegative { get; set; }
    }

    /// <summary>
    /// 적립금 거래 내역
    /// </summary>
    public class PointTransaction
    {
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("transaction_type")]
        public int? TransactionType { get; set; }

        [JsonProperty("amount")]
        public int? Amount { get; set; }

        [JsonProperty("balance_after")]
        public int? BalanceAfter { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("review_id")]
        public string ReviewId { get; set; }

        [JsonProperty("earned_at")]
        public string EarnedAt { get; set; }

        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }

        [JsonProperty("expired")]
        public bool? Expired { get; set; }

        [JsonProperty("remaining_balance")]
        public int? RemainingBalance { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }

    /// <summary>
    /// 적립금 내역 응답
    /// </summary>
    public class PointTransactionsResponse
    {
        [JsonProperty("transactions")]
        public List<PointTransaction> Transactions { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 적립금 내역 조회 파라미터
    /// </summary>
    public class PointTransactionsParams
    {
        [JsonProperty("page")]
        public int? Page { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("transaction_type")]
        public int? TransactionType { get; set; }
    }
}
