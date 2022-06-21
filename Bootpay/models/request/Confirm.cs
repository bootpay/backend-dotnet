 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Confirm
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }
    }
}
