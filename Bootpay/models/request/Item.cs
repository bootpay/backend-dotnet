using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Item
    {
        [JsonProperty("item_name")]
        public string itemName { get; set; }
        public int qty { get; set; }
        public string unique { get; set; }
        public long price { get; set; }
        public string cat1 { get; set; }
        public string cat2 { get; set; }
        public string cat3 { get; set; }
    }
}
