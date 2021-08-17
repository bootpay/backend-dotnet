using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Payload
    {
        public string pg { get; set; }
        public string method { get; set; }
        public List<string> methods { get; set; }
        public long price { get; set; }

        [JsonProperty("order_id")]
        public string orderId { get; set; }

        [JsonProperty("params")]
        public string paramCustom { get; set; }

        [JsonProperty("tax_free")]
        public int taxFree { get; set; }
        public string name { get; set; }

        [JsonProperty("user_info")]
        public User userInfo { get; set; }

        public List<Item> items { get; set; }

        [JsonProperty("return_url")]
        public string returnUrl { get; set; }
        public Extra extra { get; set; }
    }
}
