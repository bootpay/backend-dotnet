using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bootpay.models
{
    public class Subscribe
    {
        [JsonPropertyName("order_id")]
        public string orderId { get; set; }


        public string pgs { get; set; }

        [JsonPropertyName("item_name")]
        public string itemName { get; set; }

        [JsonPropertyName("card_no")]
        public string cardNo { get; set; }

        [JsonPropertyName("card_pw")]
        public string cardPw { get; set; }

        [JsonPropertyName("expire_year")]
        public string expireYear { get; set; }

        [JsonPropertyName("expire_month")]
        public string expireMonth { get; set; }

        [JsonPropertyName("identify_number")]
        public string identifyNumber { get; set; } 
    }
}
