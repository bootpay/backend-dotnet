using System; 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Cancel
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }
        [JsonProperty("cancel_id")]
        public string cancelId { get; set; }
        [JsonProperty("cancel_price")]
        public double cancelPrice { get; set; }
        [JsonProperty("cancel_tax_free")]
        public double cancelTaxFree { get; set; }

        [JsonProperty("cancel_uername")]
        public string cancelUsername { get; set; }

        [JsonProperty("cancel_message")]
        public string cancelMessage { get; set; }

        //public string reason { get; set; }
        public RefundData refund { get; set; }
    }
}
