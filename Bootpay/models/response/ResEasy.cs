using System;
using System.Collections.Generic; 
using Newtonsoft.Json;

namespace Bootpay.models.response
{
    public class ResVerify : ResDefault
    {
        public new ResVerifyData data { get; set; }
    }

    public class ResVerifyData
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }

        [JsonProperty("order_id")]
        public string orderId { get; set; }
        
        public string name { get; set; }
        public double price { get; set; }

        [JsonProperty("tax_free")]
        public double taxFree { get; set; }

        [JsonProperty("remain_price")]
        public double remainPrice { get; set; }

        [JsonProperty("remain_tax_free")]
        public double remainTaxFree { get; set; }

        [JsonProperty("cancelled_price")]
        public double cancelledPrice { get; set; }

        [JsonProperty("cancelled_tax_free")]
        public double cancelledTaxFree { get; set; }

        [JsonProperty("receipt_url")]
        public string receiptUrl { get; set; }
         
        public string unit { get; set; } 
        public string pg { get; set; }
        public string methd { get; set; }

        [JsonProperty("pg_name")]
        public string pgName { get; set; }

        [JsonProperty("method_name")]
        public string methodName { get; set; }

        [JsonProperty("params")]
        public Dictionary<string, object> paramsCustom { get; set; }

        [JsonProperty("payment_data")]
        public Dictionary<string, object> paymentData { get; set; }

        [JsonProperty("requested_at")]
        public string requestedAt { get; set; }

        [JsonProperty("purchased_at")]
        public string purchasedAt { get; set; }

        public int status { get; set; }

        [JsonProperty("status_en")]
        public string statusEn { get; set; }

        [JsonProperty("status_ko")]
        public string statusKn { get; set; } 
    }
}
