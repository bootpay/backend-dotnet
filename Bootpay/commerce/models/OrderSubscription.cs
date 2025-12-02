using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// Commerce 정기구독
    /// </summary>
    public class CommerceOrderSubscription
    {
        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("order_pre_id")]
        public string OrderPreId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("wallet_id")]
        public string WalletId { get; set; }

        [JsonProperty("subscription_billing_type")]
        public int? SubscriptionBillingType { get; set; }

        [JsonProperty("subscription_payment_cycle_type")]
        public int? SubscriptionPaymentCycleType { get; set; }

        [JsonProperty("subscription_payment_date")]
        public int? SubscriptionPaymentDate { get; set; }

        [JsonProperty("subscription_billing_base_day")]
        public int? SubscriptionBillingBaseDay { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("is_first_prepaid")]
        public bool? IsFirstPrepaid { get; set; }

        [JsonProperty("one_unit_price")]
        public int? OneUnitPrice { get; set; }

        [JsonProperty("one_unit_tax_free_price")]
        public int? OneUnitTaxFreePrice { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("setup_price")]
        public int? SetupPrice { get; set; }

        [JsonProperty("unit")]
        public int? Unit { get; set; }

        [JsonProperty("order_name")]
        public string OrderName { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("option_names")]
        public List<string> OptionNames { get; set; }

        [JsonProperty("service_start_at")]
        public string ServiceStartAt { get; set; }

        [JsonProperty("service_end_at")]
        public string ServiceEndAt { get; set; }

        [JsonProperty("last_billing_created_at")]
        public string LastBillingCreatedAt { get; set; }

        [JsonProperty("latest_purchased_at")]
        public string LatestPurchasedAt { get; set; }

        [JsonProperty("latest_failed_at")]
        public string LatestFailedAt { get; set; }

        [JsonProperty("payment_next_at")]
        public string PaymentNextAt { get; set; }

        [JsonProperty("current_duration")]
        public int? CurrentDuration { get; set; }

        [JsonProperty("created_last_duration")]
        public int? CreatedLastDuration { get; set; }

        [JsonProperty("payment_last_duration")]
        public int? PaymentLastDuration { get; set; }

        [JsonProperty("total_subscription_duration")]
        public int? TotalSubscriptionDuration { get; set; }

        [JsonProperty("membership_type")]
        public int? MembershipType { get; set; }

        [JsonProperty("use_subscription_times")]
        public bool? UseSubscriptionTimes { get; set; }

        [JsonProperty("renewal_status")]
        public int? RenewalStatus { get; set; }

        [JsonProperty("cancel_status")]
        public int? CancelStatus { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("cancel_at")]
        public string CancelAt { get; set; }
    }

    /// <summary>
    /// 정기구독 목록 조회 파라미터
    /// </summary>
    public class OrderSubscriptionListParams : ListParams
    {
        [JsonProperty("s_at")]
        public string SAt { get; set; }

        [JsonProperty("e_at")]
        public string EAt { get; set; }

        [JsonProperty("request_type")]
        public string RequestType { get; set; }

        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    /// <summary>
    /// 정기구독 수정 파라미터
    /// </summary>
    public class OrderSubscriptionUpdateParams
    {
        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("next_billing_at")]
        public string NextBillingAt { get; set; }

        [JsonProperty("billing_key")]
        public string BillingKey { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("payment_next_at")]
        public string PaymentNextAt { get; set; }

        [JsonProperty("service_end_at")]
        public string ServiceEndAt { get; set; }
    }

    /// <summary>
    /// 정기구독 일시정지 파라미터
    /// </summary>
    public class OrderSubscriptionPauseParams
    {
        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("paused_at")]
        public string PausedAt { get; set; }

        [JsonProperty("expected_resume_at")]
        public string ExpectedResumeAt { get; set; }
    }

    /// <summary>
    /// 정기구독 재개 파라미터
    /// </summary>
    public class OrderSubscriptionResumeParams
    {
        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("resume_at")]
        public string ResumeAt { get; set; }
    }

    /// <summary>
    /// 정기구독 해지 파라미터
    /// </summary>
    public class OrderSubscriptionTerminationParams
    {
        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("termination_fee")]
        public int? TerminationFee { get; set; }

        [JsonProperty("last_bill_refund_price")]
        public int? LastBillRefundPrice { get; set; }

        [JsonProperty("final_fee")]
        public int? FinalFee { get; set; }

        [JsonProperty("service_end_at")]
        public string ServiceEndAt { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    /// <summary>
    /// 해지 수수료 계산 응답
    /// </summary>
    public class CalcTerminateFeeResponse
    {
        [JsonProperty("termination_fee")]
        public int? TerminationFee { get; set; }

        [JsonProperty("refund_amount")]
        public int? RefundAmount { get; set; }

        [JsonProperty("last_bill_refund_price")]
        public int? LastBillRefundPrice { get; set; }

        [JsonProperty("final_fee")]
        public int? FinalFee { get; set; }
    }
}
