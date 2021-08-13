using System;
using System.Text.Json.Serialization;

namespace Bootpay.models.response
{
    public class ResBillingKey : ResDefault
    {
        public BillingKeyData data { get; set; }
    }

    public class BillingKeyData
    {
        [JsonPropertyName("billing_key")]
        public string billingKey { get; set; }

        [JsonPropertyName("pg_name")]
        public string pgName { get; set; }

        [JsonPropertyName("method_name")]
        public string methodName { get; set; }

        [JsonPropertyName("method")]
        public string method { get; set; }

        public BillingKeyCardData data { get; set; }

        [JsonPropertyName("e_at")]
        public string endAt { get; set; }

        [JsonPropertyName("c_at")]
        public string createAt { get; set; }
    }

    public class BillingKeyCardData
    {
        [JsonPropertyName("card_code")]
        public string cardCode { get; set; }

        [JsonPropertyName("card_name")]
        public string cardName { get; set; }

        [JsonPropertyName("card_no")]
        public string cardNo { get; set; }

        [JsonPropertyName("card_cl")]
        public string cardCl { get; set; } 
    }
}
