using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class SubscribeExtra
    {
        [JsonProperty("subscribe_test_payment")]
        public int subscribeTestPayment { get; set; }

        [JsonProperty("raw_data")]
        public int rawData { get; set; }
    }
}
