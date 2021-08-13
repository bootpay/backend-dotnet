using System;
using System.Text.Json.Serialization;

namespace Bootpay.models.response
{
    public class ResCancel : ResDefault
    {
        public ResCancelData data { get; set; }
    }

    public class ResCancelData
    {
        [JsonPropertyName("receipt_id")]
        public string receiptId { get; set; }

        [JsonPropertyName("request_cancel_price")]
        public int requestCancelPrice { get; set; }

        [JsonPropertyName("remain_price")]
        public int remainPrice { get; set; }

        [JsonPropertyName("remain_tax_free")]
        public int remainTaxFree { get; set; }

        [JsonPropertyName("cancelled_price")]
        public int cancelledPrice { get; set; }

        [JsonPropertyName("cancelled_tax_free")]
        public int cancelledTaxFree { get; set; }

        [JsonPropertyName("revoked_at")]
        public string revokedAt { get; set; }
         
        public string tid { get; set; }
    }
}
