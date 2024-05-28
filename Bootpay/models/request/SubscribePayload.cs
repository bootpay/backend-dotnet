using System;
using System.Collections.Generic; 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class SubscribePayload
    {
        [JsonProperty("billing_key")]
        public string billingKey { get; set; }

        [JsonProperty("order_name")]
        public string orderName { get; set; }

        [JsonProperty("order_id")]
        public string orderId { get; set; }

        public double price { get; set; }

        [JsonProperty("tax_free")]
        public double taxFree { get; set; }

        [JsonProperty("card_quota")]
        public string cardQuota { get; set; }

        [JsonProperty("card_interest")]
        public string cardInterest { get; set; }

        public User user { get; set; }

        public List<Item> items { get; set; }

        [JsonProperty("feedback_url")]
        public string feedbackUrl { get; set; }

        [JsonProperty("content_type")]
        public string contentType { get; set; }

        public Dictionary<string, object> metadata { get; set; }

        public SubscribeExtra extra { get; set; }


        [JsonProperty("reserve_execute_at")]
        public string reserveExecuteAt { get; set; }



        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }

    }
}
