using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Subscribe
    { 
        [JsonProperty("order_id")]        
        public string orderId { get; set; }


        public string pg { get; set; }

        [JsonProperty("item_name")]
        public string itemName { get; set; }

        [JsonProperty("card_no")]
        public string cardNo { get; set; }

        [JsonProperty("card_pw")]
        public string cardPw { get; set; }

        [JsonProperty("expire_year")]
        public string expireYear { get; set; }

        [JsonProperty("expire_month")]
        public string expireMonth { get; set; }

        [JsonProperty("identify_number")]
        public string identifyNumber { get; set; } 
    }
}
