using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bootpay.models.response
{
    public class ResBillingSubscribe : ResDefault
    {
        public BillingKeyData data { get; set; }
    }

    public class BillingSubscribeData
    {
        [JsonPropertyName("receipt_id")]
        public string receiptId { get; set; }

        [JsonPropertyName("price")]
        public double price { get; set; }

        [JsonPropertyName("card_no")]
        public string cardNo { get; set; }

        [JsonPropertyName("card_code")]
        public string cardCode { get; set; }

        [JsonPropertyName("card_name")]
        public string cardName { get; set; }

        [JsonPropertyName("card_quota")]
        public string cardQuota { get; set; }

        [JsonPropertyName("params")]
        public Dictionary<string, object> paramsCustom { get; set; }

        [JsonPropertyName("item_name")]
        public string itemName { get; set; }

        [JsonPropertyName("order_id")]
        public string orderId { get; set; }

        public string url { get; set; }

        [JsonPropertyName("payment_name")]
        public string paymentName { get; set; }

        [JsonPropertyName("pg_name")]
        public string pgName { get; set; }
                
        public string pg { get; set; }
        public string method { get; set; }

        [JsonPropertyName("method_name")]
        public string methodName { get; set; }

        [JsonPropertyName("requested_at")]
        public string requestedAt { get; set; }

        [JsonPropertyName("purchased_at")]
        public string purchasedAt { get; set; }

        public int status { get; set; }
    }
}
