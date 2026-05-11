 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Token
    {
        [JsonProperty("application_id")]
        public string applicationId { get; set; }

        [JsonProperty("private_key")]
        public string privateKey { get; set; }

        [JsonProperty("client_key")]
        public string clientKey { get; set; }

        [JsonProperty("secret_key")]
        public string secretKey { get; set; }
    }
}
