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
        [JsonProperty("search_date_from")]
        public string SearchDateFrom { get; set; }

        [JsonProperty("search_date_to")]
        public string SearchDateTo { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

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
    /// 구독 계약 내용 변경 — supervisor scope. order_subscription_id 는 URL 로만 전송된다.
    /// </summary>
    public class OrderSubscriptionUpdateParams
    {
        [JsonIgnore]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("product_option_id")]
        public string ProductOptionId { get; set; }

        [JsonProperty("order_name")]
        public string OrderName { get; set; }

        [JsonProperty("total_subscription_duration")]
        public int? TotalSubscriptionDuration { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("address_id")]
        public string AddressId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("use_free_trial")]
        public bool? UseFreeTrial { get; set; }

        [JsonProperty("free_trial_day")]
        public int? FreeTrialDay { get; set; }

        [JsonProperty("service_start_at")]
        public string ServiceStartAt { get; set; }

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
    /// 중도인수 요청 파라미터 (POST /v1/order_subscriptions/requests/ing/purchase)
    /// </summary>
    public class OrderSubscriptionPurchaseParams
    {
        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    /// <summary>
    /// 구독 이전/승계 요청 파라미터 (POST /v1/order_subscriptions/requests/ing/transfer)
    /// </summary>
    public class OrderSubscriptionTransferParams
    {
        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("new_user_id")]
        public string NewUserId { get; set; }

        [JsonProperty("new_username")]
        public string NewUsername { get; set; }

        [JsonProperty("new_user_email")]
        public string NewUserEmail { get; set; }

        [JsonProperty("new_user_phone")]
        public string NewUserPhone { get; set; }

        [JsonProperty("new_user_address")]
        public string NewUserAddress { get; set; }

        [JsonProperty("wallet_id")]
        public string WalletId { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
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

    public class SupervisorOrderSubscriptionApproveParams
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public class SupervisorOrderSubscriptionRejectParams
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public class SupervisorOrderSubscriptionTerminateParams
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("termination_fee")]
        public int? TerminationFee { get; set; }

        [JsonProperty("last_bill_refund_price")]
        public int? LastBillRefundPrice { get; set; }

        [JsonProperty("final_fee")]
        public int? FinalFee { get; set; }

        [JsonProperty("service_end_at")]
        public string ServiceEndAt { get; set; }

        [JsonProperty("cancel_date")]
        public string CancelDate { get; set; }
    }

    public class SupervisorOrderSubscriptionPauseParams
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("paused_at")]
        public string PausedAt { get; set; }

        [JsonProperty("expected_resume_at")]
        public string ExpectedResumeAt { get; set; }
    }

    public class SupervisorOrderSubscriptionResumeParams
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    /// <summary>
    /// 수시결제(온디맨드) charge_key 즉시 결제 파라미터 (POST /v1/order_subscriptions/charge)
    /// charge_key 는 body 로만 전송한다 (URL/query 금지 — 액세스 로그 노출 방지). supervisor scope.
    /// </summary>
    public class SupervisorOrderSubscriptionChargeParams
    {
        [JsonProperty("charge_key")]
        public string ChargeKey { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("user")]
        public Dictionary<string, object> User { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }
    }

    /// <summary>
    /// 수시결제(온디맨드) charge_key 해지 파라미터 (DELETE /v1/order_subscriptions/charge)
    /// 해지 이후 해당 키로의 재결제는 불가능하다. supervisor scope.
    /// </summary>
    public class SupervisorOrderSubscriptionChargeRevokeParams
    {
        [JsonProperty("charge_key")]
        public string ChargeKey { get; set; }

        [JsonProperty("user")]
        public Dictionary<string, object> User { get; set; }
    }

    /// <summary>
    /// 수시결제 charge 응답
    /// </summary>
    public class OrderSubscriptionChargeResponse
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("receipt_id")]
        public string ReceiptId { get; set; }

        [JsonProperty("charge_key")]
        public string ChargeKey { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }
    }

    /// <summary>
    /// 수시결제 charge 해지 응답
    /// </summary>
    public class OrderSubscriptionChargeRevokeResponse
    {
        [JsonProperty("charge_key")]
        public string ChargeKey { get; set; }

        [JsonProperty("revoked_at")]
        public string RevokedAt { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }
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
