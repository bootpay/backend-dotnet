using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bootpay.models
{
    public class Submit
    {
        [JsonPropertyName("receipt_id")]
        public string receiptId { get; set; }
    }
}
