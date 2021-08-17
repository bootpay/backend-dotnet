using System;
using System.Text.Json.Serialization;

namespace Bootpay.models
{
    public class Cancel
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }
        public double price { get; set; }
        public string name { get; set; }
        public string reason { get; set; }
        public RefundData refund { get; set; }
    }
}
