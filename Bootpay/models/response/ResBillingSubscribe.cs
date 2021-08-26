using System;
using System.Collections.Generic; 
using Newtonsoft.Json;

namespace Bootpay.models.response
{
    public class ResBillingSubscribe : ResDefault
    {
        public new BillingKeyData data { get; set; }
    }

    public class BillingSubscribeData
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }

        [JsonProperty("price")]
        public double price { get; set; }

        [JsonProperty("card_no")]
        public string cardNo { get; set; }

        [JsonProperty("card_code")]
        public string cardCode { get; set; }

        [JsonProperty("card_name")]
        public string cardName { get; set; }

        [JsonProperty("card_quota")]
        public string cardQuota { get; set; }

        [JsonProperty("params")]
        public Dictionary<string, object> paramsCustom { get; set; }

        [JsonProperty("item_name")]
        public string itemName { get; set; }

        [JsonProperty("order_id")]
        public string orderId { get; set; }

        public string url { get; set; }

        [JsonProperty("payment_name")]
        public string paymentName { get; set; }

        [JsonProperty("pg_name")]
        public string pgName { get; set; }
                
        public string pg { get; set; }
        public string method { get; set; }

        [JsonProperty("method_name")]
        public string methodName { get; set; }

        [JsonProperty("requested_at")]
        public string requestedAt { get; set; }

        [JsonProperty("purchased_at")]
        public string purchasedAt { get; set; }

        public int status { get; set; }
    }
}
