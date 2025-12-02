using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 법인 유형 상수
    /// </summary>
    public static class CorporateType
    {
        public const int Individual = 1;
        public const int Corporate = 2;
    }

    /// <summary>
    /// Commerce 사용자 그룹
    /// </summary>
    public class CommerceUserGroup
    {
        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("corporate_type")]
        public int? CorporateType { get; set; }

        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        [JsonProperty("count")]
        public int? Count { get; set; }

        [JsonProperty("last_updated_at")]
        public string LastUpdatedAt { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("zipcode")]
        public string Zipcode { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("address_detail")]
        public string AddressDetail { get; set; }

        [JsonProperty("corporate_extension")]
        public Dictionary<string, object> CorporateExtension { get; set; }

        [JsonProperty("auth_bank")]
        public bool? AuthBank { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("business_number")]
        public string BusinessNumber { get; set; }

        [JsonProperty("registration_number")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("corporate_established")]
        public string CorporateEstablished { get; set; }

        [JsonProperty("business_type")]
        public string BusinessType { get; set; }

        [JsonProperty("business_category")]
        public string BusinessCategory { get; set; }

        [JsonProperty("ceo_name")]
        public string CeoName { get; set; }

        [JsonProperty("auth_company")]
        public bool? AuthCompany { get; set; }

        [JsonProperty("manager_name")]
        public string ManagerName { get; set; }

        [JsonProperty("manager_phone")]
        public string ManagerPhone { get; set; }

        [JsonProperty("manager_email")]
        public string ManagerEmail { get; set; }

        [JsonProperty("personal_customs_clearance_code")]
        public string PersonalCustomsClearanceCode { get; set; }

        [JsonProperty("point")]
        public int? Point { get; set; }

        [JsonProperty("accumulation")]
        public int? Accumulation { get; set; }

        [JsonProperty("marketing_accept_type")]
        public int? MarketingAcceptType { get; set; }

        [JsonProperty("marketing_accept_create_at")]
        public string MarketingAcceptCreateAt { get; set; }

        [JsonProperty("marketing_accept_update_at")]
        public string MarketingAcceptUpdateAt { get; set; }

        [JsonProperty("use_subscription_aggregate_transaction")]
        public bool? UseSubscriptionAggregateTransaction { get; set; }

        [JsonProperty("subscription_month_day")]
        public int? SubscriptionMonthDay { get; set; }

        [JsonProperty("subscription_week_day")]
        public int? SubscriptionWeekDay { get; set; }

        [JsonProperty("use_limit")]
        public bool? UseLimit { get; set; }

        [JsonProperty("purchase_limit")]
        public int? PurchaseLimit { get; set; }

        [JsonProperty("subscribed_limit")]
        public int? SubscribedLimit { get; set; }

        [JsonProperty("limit_message")]
        public string LimitMessage { get; set; }

        [JsonProperty("external_uid")]
        public string ExternalUid { get; set; }

        [JsonProperty("is_external")]
        public string IsExternal { get; set; }
    }

    /// <summary>
    /// 사용자 그룹 목록 조회 파라미터
    /// </summary>
    public class UserGroupListParams : ListParams
    {
        [JsonProperty("corporate_type")]
        public int? CorporateType { get; set; }
    }

    /// <summary>
    /// 사용자 그룹 제한 설정 파라미터
    /// </summary>
    public class UserGroupLimitParams
    {
        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("use_limit")]
        public bool? UseLimit { get; set; }

        [JsonProperty("purchase_limit")]
        public int? PurchaseLimit { get; set; }

        [JsonProperty("subscribed_limit")]
        public int? SubscribedLimit { get; set; }

        [JsonProperty("limit_message")]
        public string LimitMessage { get; set; }
    }

    /// <summary>
    /// 사용자 그룹 거래 집계 파라미터
    /// </summary>
    public class UserGroupAggregateTransactionParams
    {
        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("use_subscription_aggregate_transaction")]
        public bool? UseSubscriptionAggregateTransaction { get; set; }

        [JsonProperty("subscription_month_day")]
        public int? SubscriptionMonthDay { get; set; }

        [JsonProperty("subscription_week_day")]
        public int? SubscriptionWeekDay { get; set; }
    }
}
