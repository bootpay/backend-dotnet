using System;
using System.Text.Json.Serialization;

namespace Bootpay.models
{
    public class SubscribeExtra
    {
        [JsonPropertyName("subscribe_test_payment")]
        public int subscribeTestPayment { get; set; }

        [JsonPropertyName("raw_data")]
        public int rawData { get; set; }
    }
}
