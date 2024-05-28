 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class SubscribeExtra
    {
        [JsonProperty("card_quota")]
        public string cardQuota { get; set; }
        
        [JsonProperty("subscribe_test_payment")]
        public int subscribeTestPayment { get; set; }

        [JsonProperty("raw_data")]
        public int rawData { get; set; }
    }
}
