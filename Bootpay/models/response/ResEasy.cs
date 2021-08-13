using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bootpay.models.response
{
    public class ResVerify : ResDefault
    {
        public ResVerifyData data { get; set; }
    }

    public class ResVerifyData
    {
        [JsonPropertyName("receipt_id")]
        public string receiptId { get; set; }

        [JsonPropertyName("order_id")]
        public string orderId { get; set; }
        
        public string name { get; set; }
        public double price { get; set; }

        [JsonPropertyName("tax_free")]
        public double taxFree { get; set; }

        [JsonPropertyName("remain_price")]
        public double remainPrice { get; set; }

        [JsonPropertyName("remain_tax_free")]
        public double remainTaxFree { get; set; }

        [JsonPropertyName("cancelled_price")]
        public double cancelledPrice { get; set; }

        [JsonPropertyName("cancelled_tax_free")]
        public double cancelledTaxFree { get; set; }

        [JsonPropertyName("receipt_url")]
        public string receiptUrl { get; set; }
         
        public string unit { get; set; } 
        public string pg { get; set; }
        public string methd { get; set; }

        [JsonPropertyName("pg_name")]
        public string pgName { get; set; }

        [JsonPropertyName("method_name")]
        public string methodName { get; set; }

        [JsonPropertyName("params")]
        public Dictionary<string, object> paramsCustom { get; set; }

        [JsonPropertyName("payment_data")]
        public Dictionary<string, object> paymentData { get; set; }

        [JsonPropertyName("requested_at")]
        public string requestedAt { get; set; }

        [JsonPropertyName("purchased_at")]
        public string purchasedAt { get; set; }

        public int status { get; set; }

        [JsonPropertyName("status_en")]
        public string statusEn { get; set; }

        [JsonPropertyName("status_ko")]
        public string statusKn { get; set; } 
    }
}
