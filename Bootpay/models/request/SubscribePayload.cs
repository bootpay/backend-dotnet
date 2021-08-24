using System;
using System.Collections.Generic; 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class SubscribePayload
    {
        [JsonProperty("billing_key")]
        public string billingKey { get; set; }

        [JsonProperty("item_name")]
        public string itemName { get; set; }
        public long price { get; set; }

        [JsonProperty("tax_free")]
        public int taxFree { get; set; }

        [JsonProperty("order_id")]
        public string orderId { get; set; }
        public int quota { get; set; }
        public int interest { get; set; }

        [JsonProperty("user_info")]
        public User userInfo { get; set; }
        public List<Item> items { get; set; }

        [JsonProperty("feedback_url")]
        public string feedbackUrl { get; set; }

        [JsonProperty("feedback_content_type")]
        public string feedbackContentType { get; set; }
        public SubscribeExtra extra { get; set; }

        [JsonProperty("scheduler_type")]
        public string schedulerType { get; set; }

        [JsonProperty("execute_at")]
        public long executeAt { get; set; }
    }
}
