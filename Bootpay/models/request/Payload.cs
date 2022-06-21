 
using System.Collections.Generic;
 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Payload
    {
        public string pg { get; set; }

        [JsonProperty("order_name")]
        public string orderName { get; set; }

        [JsonProperty("order_id")]
        public string orderId { get; set; }

        public string method { get; set; }

        //public string method { get; set; }


        public double price { get; set; }
        [JsonProperty("tax_free")]
        public double taxFree { get; set; }

        public Dictionary<string, object> metadata { get; set; }


        public User user { get; set; }

        public SubscribeExtra extra { get; set; }
    }
}
