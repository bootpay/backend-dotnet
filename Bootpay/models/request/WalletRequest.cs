using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class WalletRequest
    {
        [JsonProperty("user_id")]
        public string userId { get; set; }

        [JsonProperty("order_name")]
        public string orderName { get; set; }

        public double price { get; set; }

        [JsonProperty("tax_free")]
        public double taxFree { get; set; }

        [JsonProperty("order_id")]
        public string orderId { get; set; }

        [JsonProperty("webhook_url")]
        public string webhookUrl { get; set; }

        [JsonProperty("content_type")]
        public string contentType { get; set; }

        public List<Item> items { get; set; }

        public User user { get; set; }

        public SubscribeExtra extra { get; set; }

        public Dictionary<string, object> metadata { get; set; }

        public bool sandbox { get; set; }
    }
}
