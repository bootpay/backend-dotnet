using System;
using Newtonsoft.Json;

namespace Bootpay.models.response
{
    public class ResBillingKey : ResDefault
    {

        //[JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public new BillingKeyData data { get; set; }
    }

    public class BillingKeyData
    {
        [JsonProperty("billing_key")]
        public string billingKey { get; set; }

        [JsonProperty("pg_name")]
        public string pgName { get; set; }

        [JsonProperty("method_name")]
        public string methodName { get; set; }

        [JsonProperty("method")]
        public string method { get; set; }

        public BillingKeyCardData data { get; set; }

        [JsonProperty("e_at")]
        public string endAt { get; set; }

        [JsonProperty("c_at")]
        public string createAt { get; set; }
    }

    public class BillingKeyCardData
    {
        [JsonProperty("card_code")]
        public string cardCode { get; set; }

        [JsonProperty("card_name")]
        public string cardName { get; set; }

        [JsonProperty("card_no")]
        public string cardNo { get; set; }

        [JsonProperty("card_cl")]
        public string cardCl { get; set; } 
    }
}
