using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bootpay.models
{
    public class SubscribePayload
    {
        [JsonPropertyName("billing_key")]
        public string billingKey { get; set; }

        [JsonPropertyName("item_name")]
        public string itemName { get; set; }
        public long price { get; set; }

        [JsonPropertyName("tax_free")]
        public int taxFree { get; set; }

        [JsonPropertyName("order_id")]
        public string orderId { get; set; }
        public int quota { get; set; }
        public int interest { get; set; }

        [JsonPropertyName("user_info")]
        public User userInfo { get; set; }
        public List<Item> items { get; set; }

        [JsonPropertyName("feedback_url")]
        public string feedbackUrl { get; set; }

        [JsonPropertyName("feedback_content_type")]
        public string feedbackContentType { get; set; }
        public SubscribeExtra extra { get; set; }

        [JsonPropertyName("scheduler_type")]
        public string schedulerType { get; set; }

        [JsonPropertyName("execute_at")]
        public long executeAt { get; set; }
    }
}
