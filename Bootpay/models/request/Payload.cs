using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bootpay.models
{
    public class Payload
    {
        public string pg { get; set; }
        public string method { get; set; }
        public List<string> methods { get; set; }
        public long price { get; set; }

        [JsonPropertyName("order_id")]
        public string orderId { get; set; }

        [JsonPropertyName("params")]
        public string paramCustom { get; set; }

        [JsonPropertyName("tax_free")]
        public int taxFree { get; set; }
        public string name { get; set; }

        [JsonPropertyName("user_info")]
        public User userInfo { get; set; }

        public List<Item> items { get; set; }

        [JsonPropertyName("return_url")]
        public string returnUrl { get; set; }
        public Extra extra { get; set; }
    }
}
