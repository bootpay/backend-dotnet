using System; 
using Newtonsoft.Json;

namespace Bootpay.models.response
{
    public class ResCancel : ResDefault
    {
        public new ResCancelData data { get; set; }
    }

    public class ResCancelData
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }

        [JsonProperty("request_cancel_price")]
        public int requestCancelPrice { get; set; }

        [JsonProperty("remain_price")]
        public int remainPrice { get; set; }

        [JsonProperty("remain_tax_free")]
        public int remainTaxFree { get; set; }

        [JsonProperty("cancelled_price")]
        public int cancelledPrice { get; set; }

        [JsonProperty("cancelled_tax_free")]
        public int cancelledTaxFree { get; set; }

        [JsonProperty("revoked_at")]
        public string revokedAt { get; set; }
         
        public string tid { get; set; }
    }
}
