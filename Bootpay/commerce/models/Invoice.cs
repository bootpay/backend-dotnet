using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 청구서 발송 타입 상수
    /// </summary>
    public static class InvoiceSendType
    {
        public const int Sms = 1;
        public const int Kakao = 2;
        public const int Email = 3;
        public const int Push = 4;
    }

    /// <summary>
    /// Commerce 청구서
    /// </summary>
    public class CommerceInvoice
    {
        [JsonProperty("invoice_id")]
        public string InvoiceId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("created_owner_id")]
        public string CreatedOwnerId { get; set; }

        [JsonProperty("created_owner_type")]
        public int? CreatedOwnerType { get; set; }

        [JsonProperty("unit")]
        public int? Unit { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("use_redirect")]
        public bool? UseRedirect { get; set; }

        [JsonProperty("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("subscription_type")]
        public int? SubscriptionType { get; set; }

        [JsonProperty("subscription_start_at")]
        public string SubscriptionStartAt { get; set; }

        [JsonProperty("subscription_end_at")]
        public string SubscriptionEndAt { get; set; }

        [JsonProperty("expired_at")]
        public string ExpiredAt { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("deleted")]
        public bool? Deleted { get; set; }

        [JsonProperty("user_collection_type")]
        public int? UserCollectionType { get; set; }

        [JsonProperty("use_link_redirect")]
        public bool? UseLinkRedirect { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("send_status")]
        public int? SendStatus { get; set; }

        [JsonProperty("send_types")]
        public List<int> SendTypes { get; set; }

        [JsonProperty("message_template_id")]
        public string MessageTemplateId { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("message_from")]
        public string MessageFrom { get; set; }

        [JsonProperty("message_type")]
        public int? MessageType { get; set; }

        [JsonProperty("message_response")]
        public string MessageResponse { get; set; }

        [JsonProperty("sent_at")]
        public string SentAt { get; set; }

        [JsonProperty("pay_at")]
        public string PayAt { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("use_editable_username")]
        public bool? UseEditableUsername { get; set; }

        [JsonProperty("use_editable_phone")]
        public bool? UseEditablePhone { get; set; }

        [JsonProperty("use_editable_email")]
        public bool? UseEditableEmail { get; set; }

        [JsonProperty("use_memo")]
        public bool? UseMemo { get; set; }

        [JsonProperty("product_ids")]
        public List<string> ProductIds { get; set; }

        [JsonProperty("product_option_ids")]
        public List<string> ProductOptionIds { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("webhook_url")]
        public string WebhookUrl { get; set; }

        [JsonProperty("header_content_type")]
        public int? HeaderContentType { get; set; }

        [JsonProperty("webhook_retry_count")]
        public int? WebhookRetryCount { get; set; }

        [JsonProperty("product_type")]
        public int? ProductType { get; set; }

        [JsonProperty("is_open_link")]
        public bool? IsOpenLink { get; set; }

        [JsonProperty("invoice_items")]
        public List<CommerceInvoiceItem> InvoiceItems { get; set; }

        [JsonProperty("selected_users")]
        public List<string> SelectedUsers { get; set; }
    }

    /// <summary>
    /// 청구서 항목
    /// </summary>
    public class CommerceInvoiceItem
    {
        [JsonProperty("invoice_item_id")]
        public string InvoiceItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("qty")]
        public int? Qty { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }
    }

    /// <summary>
    /// 청구서 목록 조회 파라미터
    /// </summary>
    public class InvoiceListParams : ListParams
    {
    }

    /// <summary>
    /// 청구서 생성 파라미터
    /// </summary>
    public class InvoiceCreateParams
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("expired_at")]
        public string ExpiredAt { get; set; }

        [JsonProperty("invoice_items")]
        public List<CommerceInvoiceItem> InvoiceItems { get; set; }

        [JsonProperty("send_types")]
        public List<int> SendTypes { get; set; }

        [JsonProperty("webhook_url")]
        public string WebhookUrl { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }
    }
}
