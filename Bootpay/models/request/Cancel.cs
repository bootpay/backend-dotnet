using System;
using System.Text.Json.Serialization;

namespace Bootpay.models
{
    public class Cancel
    {
        [JsonPropertyName("receipt_id")]
        public string receiptId { get; set; }
        public double price { get; set; }
        public string name { get; set; }
        public string reason { get; set; }
        public RefundData refund { get; set; }
    }
}
