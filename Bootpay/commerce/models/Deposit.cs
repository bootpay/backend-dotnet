using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>뱅크알람 입금 웹훅 이벤트 코드 (400~407).</summary>
    public static class BankDepositWebhookEvent
    {
        /// <summary>입금 알림 저장 (기본 미구독)</summary>
        public const int Received = 400;
        /// <summary>주문에 연결되어 입금 확인</summary>
        public const int Matched = 401;
        /// <summary>후보 주문이 여럿이라 판매자 선택 필요</summary>
        public const int Ambiguous = 402;
        /// <summary>연결할 주문 없음</summary>
        public const int Unmatched = 403;
        /// <summary>판매자가 직접 입금 확인</summary>
        public const int ConfirmedManually = 404;
        /// <summary>입금 기한 만료</summary>
        public const int Expired = 405;
        /// <summary>입금대기 취소</summary>
        public const int Cancelled = 406;
        /// <summary>입금 확인을 되돌려 다시 입금대기</summary>
        public const int Reverted = 407;
    }

    public class DepositAccount
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("bank_name")] public string BankName { get; set; }
        [JsonProperty("account_no")] public string AccountNo { get; set; }
        [JsonProperty("holder")] public string Holder { get; set; }
        [JsonProperty("primary")] public bool? Primary { get; set; }
    }

    public class DepositAccountsResponse
    {
        [JsonProperty("accounts")] public List<DepositAccount> Accounts { get; set; }
    }

    public class DepositWebhookSecretResponse
    {
        [JsonProperty("secret")] public string Secret { get; set; }
    }

    /// <summary>구매자 알림톡 (유료). 하나라도 true 면 BuyerPhone 필수, Link 는 https.</summary>
    public class DepositNotifyParams
    {
        [JsonProperty("deposit_guide")] public bool? DepositGuide { get; set; }
        [JsonProperty("deposit_paid")] public bool? DepositPaid { get; set; }
        [JsonProperty("link")] public string Link { get; set; }
    }

    /// <summary>입금대기 등록. MerchantOrderId · Price · Depositor 는 필수.</summary>
    public class DepositCreateParams
    {
        [JsonProperty("merchant_order_id")] public string MerchantOrderId { get; set; }
        [JsonProperty("price")] public double Price { get; set; }
        [JsonProperty("depositor")] public string Depositor { get; set; }
        [JsonProperty("order_name")] public string OrderName { get; set; }
        [JsonProperty("memo")] public string Memo { get; set; }
        [JsonProperty("buyer_name")] public string BuyerName { get; set; }
        [JsonProperty("buyer_phone")] public string BuyerPhone { get; set; }
        [JsonProperty("buyer_email")] public string BuyerEmail { get; set; }
        [JsonProperty("account_id")] public string AccountId { get; set; }
        [JsonProperty("webhook_url")] public string WebhookUrl { get; set; }
        [JsonProperty("notify")] public DepositNotifyParams Notify { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class DepositNotify
    {
        [JsonProperty("deposit_guide")] public bool? DepositGuide { get; set; }
        [JsonProperty("deposit_paid")] public bool? DepositPaid { get; set; }
    }

    /// <summary>주문에 연결된 입금 알림. 웹훅 본문에만 실린다.</summary>
    public class DepositNotice
    {
        [JsonProperty("key")] public string Key { get; set; }
        [JsonProperty("bank_name")] public string BankName { get; set; }
        [JsonProperty("account_tail")] public string AccountTail { get; set; }
        [JsonProperty("depositor")] public string Depositor { get; set; }
        [JsonProperty("amount")] public double? Amount { get; set; }
        [JsonProperty("received_at")] public string ReceivedAt { get; set; }
    }

    /// <summary>입금대기 주문. 등록·조회·취소 응답과 웹훅 data 가 같은 형태다.</summary>
    public class CommerceDeposit
    {
        [JsonProperty("merchant_order_id")] public string MerchantOrderId { get; set; }
        [JsonProperty("order_number")] public string OrderNumber { get; set; }
        [JsonProperty("order_name")] public string OrderName { get; set; }
        [JsonProperty("amount")] public double? Amount { get; set; }
        /// <summary>awaiting · paid · expired · cancelled</summary>
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("buyer_name")] public string BuyerName { get; set; }
        [JsonProperty("depositor")] public string Depositor { get; set; }
        [JsonProperty("account")] public DepositAccount Account { get; set; }
        [JsonProperty("deadline")] public string Deadline { get; set; }
        [JsonProperty("deadline_label")] public string DeadlineLabel { get; set; }
        [JsonProperty("paid_at")] public string PaidAt { get; set; }
        [JsonProperty("notify")] public DepositNotify Notify { get; set; }
        [JsonProperty("deposit")] public DepositNotice Deposit { get; set; }
    }
}
