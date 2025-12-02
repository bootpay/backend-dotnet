using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// Commerce 정기구독 청구
    /// </summary>
    public class CommerceOrderSubscriptionBill
    {
        [JsonProperty("order_subscription_bill_id")]
        public string OrderSubscriptionBillId { get; set; }

        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("subscription_billing_type")]
        public int? SubscriptionBillingType { get; set; }

        [JsonProperty("order_name")]
        public string OrderName { get; set; }

        [JsonProperty("paid_wallet_id")]
        public string PaidWalletId { get; set; }

        [JsonProperty("reserved_wallet_id")]
        public string ReservedWalletId { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("order_pre_id")]
        public string OrderPreId { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("duration")]
        public int? Duration { get; set; }

        [JsonProperty("total_subscription_duration")]
        public int? TotalSubscriptionDuration { get; set; }

        [JsonProperty("one_unit_price")]
        public int? OneUnitPrice { get; set; }

        [JsonProperty("one_unit_tax_free_price")]
        public int? OneUnitTaxFreePrice { get; set; }

        [JsonProperty("setup_price")]
        public int? SetupPrice { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("unit")]
        public int? Unit { get; set; }

        [JsonProperty("purchase_price")]
        public int? PurchasePrice { get; set; }

        [JsonProperty("purchase_tax_free_price")]
        public int? PurchaseTaxFreePrice { get; set; }

        [JsonProperty("cancelled_price")]
        public int? CancelledPrice { get; set; }

        [JsonProperty("cancelled_tax_free_price")]
        public int? CancelledTaxFreePrice { get; set; }

        [JsonProperty("cancelled_fee")]
        public int? CancelledFee { get; set; }

        [JsonProperty("membership_type")]
        public int? MembershipType { get; set; }

        [JsonProperty("address_id")]
        public string AddressId { get; set; }

        [JsonProperty("user_address")]
        public string UserAddress { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("user_phone")]
        public string UserPhone { get; set; }

        [JsonProperty("user_email")]
        public string UserEmail { get; set; }

        [JsonProperty("user_company_name")]
        public string UserCompanyName { get; set; }

        [JsonProperty("user_business_number")]
        public string UserBusinessNumber { get; set; }

        [JsonProperty("product_ids")]
        public List<string> ProductIds { get; set; }

        [JsonProperty("product_option_ids")]
        public List<string> ProductOptionIds { get; set; }

        [JsonProperty("product_snapshot_ids")]
        public List<string> ProductSnapshotIds { get; set; }

        [JsonProperty("product_option_snapshot_ids")]
        public List<string> ProductOptionSnapshotIds { get; set; }

        [JsonProperty("product_type")]
        public int? ProductType { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("reserve_payment_at")]
        public string ReservePaymentAt { get; set; }

        [JsonProperty("purchased_at")]
        public string PurchasedAt { get; set; }

        [JsonProperty("revoked_at")]
        public string RevokedAt { get; set; }

        [JsonProperty("last_error_at")]
        public string LastErrorAt { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("cancel_status")]
        public int? CancelStatus { get; set; }

        [JsonProperty("test_code")]
        public string TestCode { get; set; }

        [JsonProperty("service_start_at")]
        public string ServiceStartAt { get; set; }

        [JsonProperty("service_end_at")]
        public string ServiceEndAt { get; set; }
    }

    /// <summary>
    /// 정기구독 청구 목록 조회 파라미터
    /// </summary>
    public class OrderSubscriptionBillListParams : ListParams
    {
        [JsonProperty("order_subscription_id")]
        public string OrderSubscriptionId { get; set; }

        [JsonProperty("status")]
        public List<int> Status { get; set; }
    }
}
